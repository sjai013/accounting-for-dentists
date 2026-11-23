using System.IO.Compression;
using System.Text;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.API.Export;

public interface IDataExportService
{
    Task<DataExportResult> ExportAsync(int[]? financialYears = null, CancellationToken cancellationToken = default);
}

public sealed class DataExportResult
{
    public required byte[] Bytes { get; init; }
    public string Filename { get; init; } = "accounting-data.zip";
}

public sealed class DataExportService(
    IDbContextFactory<AccountingContext> contextFactory,
    TenantProvider tenantProvider) : IDataExportService
{
    public async Task<DataExportResult> ExportAsync(int[]? financialYears = null, CancellationToken cancellationToken = default)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        IQueryable<ExpensesEntity> expensesQuery = context.Expenses
            .Include(x => x.DateReference)
            .Include(x => x.Attachment)
            .AsNoTracking();

        IQueryable<SalesEntity> salesQuery = context.Sales
            .Include(x => x.DateReference)
            .Include(x => x.Attachment)
            .AsNoTracking();

        IQueryable<ContractIncomeEntity> contractQuery = context.ContractIncome
            .Include(x => x.InvoiceDateReference)
            .Include(x => x.Attachment)
            .AsNoTracking();

        if (financialYears is { Length: > 0 })
        {
            var startDates = financialYears.Select(year => new DateOnly(year - 1, 7, 1)).ToArray();
            var endDates = financialYears.Select(year => new DateOnly(year, 7, 1)).ToArray();

            expensesQuery = expensesQuery.Where(x => startDates.Any(start => x.DateReference.Date >= start)
                && endDates.Any(end => x.DateReference.Date < end));
            salesQuery = salesQuery.Where(x => startDates.Any(start => x.DateReference.Date >= start)
                && endDates.Any(end => x.DateReference.Date < end));
            contractQuery = contractQuery.Where(x => startDates.Any(start => x.InvoiceDateReference.Date >= start)
                && endDates.Any(end => x.InvoiceDateReference.Date < end));
        }

        var expenses = await expensesQuery
            .OrderBy(x => x.DateReference.Date)
            .ToListAsync(cancellationToken);

        var sales = await salesQuery
            .OrderBy(x => x.DateReference.Date)
            .ToListAsync(cancellationToken);

        var contractAttachments = await contractQuery
            .ToListAsync(cancellationToken);

        if (financialYears is { Length: > 0 })
        {
            var ranges = financialYears
                .Distinct()
                .Select(year => (Start: new DateOnly(year - 1, 7, 1), End: new DateOnly(year, 7, 1)))
                .ToArray();

            expenses = expenses.Where(x => InFinancialYear(x.DateReference.Date, ranges)).ToList();
            sales = sales.Where(x => InFinancialYear(x.DateReference.Date, ranges)).ToList();
            contractAttachments = contractAttachments
                .Where(x => InFinancialYear(x.InvoiceDateReference.Date, ranges))
                .ToList();
        }

        using var workbook = CreateWorkbook(expenses, sales);
        using var zipStream = new MemoryStream();

        using (var archive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
        {
            var workbookEntry = archive.CreateEntry("accounting-data.xlsx", CompressionLevel.SmallestSize);
            await using (var workbookStream = workbookEntry.Open())
            {
                workbook.SaveAs(workbookStream);
            }

            var allAttachments = expenses.Select(x => (x.DateReference.Date, x.Attachment))
                .Concat(sales.Select(x => (x.DateReference.Date, x.Attachment)))
                .Concat(contractAttachments.Select(x => (x.InvoiceDateReference.Date, x.Attachment)));
            await AddAttachmentsAsync(archive, allAttachments, cancellationToken);
        }

        return new DataExportResult
        {
            Bytes = zipStream.ToArray(),
            Filename = $"accounting-data-{DateTime.Now:yyyyMMdd-HHmmss}.zip"
        };
    }

    private XLWorkbook CreateWorkbook(List<ExpensesEntity> expenses, List<SalesEntity> sales)
    {
        var workbook = new XLWorkbook();

        var expensesSheet = workbook.Worksheets.Add("Expenses");
        WriteExpenseHeaders(expensesSheet);
        for (var i = 0; i < expenses.Count; i++)
        {
            var row = i + 2;
            var item = expenses[i];
            var attachmentPath = AttachmentPath(item.DateReference.Date, item.Attachment);
            expensesSheet.Cell(row, 1).Value = item.DateReference.Date.ToString("yyyy-MM-dd");
            expensesSheet.Cell(row, 2).Value = item.BusinessName;
            expensesSheet.Cell(row, 3).Value = item.Description;
            expensesSheet.Cell(row, 4).Value = (double)item.Amount;
            expensesSheet.Cell(row, 5).Value = (double)item.GST;
            expensesSheet.Cell(row, 6).Value = (double)item.Total;
            expensesSheet.Cell(row, 7).Value = attachmentPath;
        }

        var salesSheet = workbook.Worksheets.Add("Sales");
        WriteSalesHeaders(salesSheet);
        for (var i = 0; i < sales.Count; i++)
        {
            var row = i + 2;
            var item = sales[i];
            var attachmentPath = AttachmentPath(item.DateReference.Date, item.Attachment);
            salesSheet.Cell(row, 1).Value = item.DateReference.Date.ToString("yyyy-MM-dd");
            salesSheet.Cell(row, 2).Value = item.BusinessName;
            salesSheet.Cell(row, 3).Value = item.Description;
            salesSheet.Cell(row, 4).Value = (double)item.Amount;
            salesSheet.Cell(row, 5).Value = (double)item.GST;
            salesSheet.Cell(row, 6).Value = (double)item.Total;
            salesSheet.Cell(row, 7).Value = attachmentPath;
        }

        var basSheet = workbook.Worksheets.Add("BAS");
        var headers = new[] { "Month", "Total Sales (G1)", "GST-Free Sales (G3)", "GST on Sales (1A)", "Total Purchases", "GST on Purchases (1B)", "Net GST" };
        for (var i = 0; i < headers.Length; i++) basSheet.Cell(1, i + 1).Value = headers[i];

        var months = expenses.Select(x => new { x.DateReference.Date.Year, x.DateReference.Date.Month })
            .Concat(sales.Select(x => new { x.DateReference.Date.Year, x.DateReference.Date.Month }))
            .Distinct()
            .OrderBy(x => x.Year).ThenBy(x => x.Month);

        var rowNumber = 2;
        foreach (var month in months)
        {
            var monthSales = sales.Where(x => x.DateReference.Date.Year == month.Year && x.DateReference.Date.Month == month.Month).ToList();
            var monthExpenses = expenses.Where(x => x.DateReference.Date.Year == month.Year && x.DateReference.Date.Month == month.Month).ToList();
            var salesAmount = monthSales.Sum(x => x.Amount);
            var salesGst = monthSales.Sum(x => x.GST);
            var expenseAmount = monthExpenses.Sum(x => x.Amount);
            var expenseGst = monthExpenses.Sum(x => x.GST);
            var gstFreeSales = monthSales.Sum(CalculateGSTFreeSalesAmount);

            basSheet.Cell(rowNumber, 1).Value = $"{month.Year}-{month.Month:D2}";
            basSheet.Cell(rowNumber, 2).Value = (double)salesAmount;
            basSheet.Cell(rowNumber, 3).Value = (double)gstFreeSales;
            basSheet.Cell(rowNumber, 4).Value = (double)salesGst;
            basSheet.Cell(rowNumber, 5).Value = (double)expenseAmount;
            basSheet.Cell(rowNumber, 6).Value = (double)expenseGst;
            basSheet.Cell(rowNumber, 7).Value = (double)(expenseGst - salesGst);
            rowNumber++;
        }

        foreach (var sheet in workbook.Worksheets)
        {
            sheet.SheetView.FreezeRows(1);
            sheet.Columns().AdjustToContents();
            var headerRange = sheet.Range(1, 1, 1, sheet.LastColumnUsed()?.ColumnNumber() ?? 1);
            headerRange.Style.Font.Bold = true;
            headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
        }

        return workbook;
    }

    private static void WriteExpenseHeaders(IXLWorksheet sheet)
    {
        var headers = new[] { "Date", "Business", "Description", "Amount", "GST", "Total", "Attachment" };
        for (var i = 0; i < headers.Length; i++) sheet.Cell(1, i + 1).Value = headers[i];
    }

    private static void WriteSalesHeaders(IXLWorksheet sheet)
    {
        var headers = new[] { "Date", "Business", "Description", "Amount", "GST", "Total", "Attachment" };
        for (var i = 0; i < headers.Length; i++) sheet.Cell(1, i + 1).Value = headers[i];
    }

    private async Task AddAttachmentsAsync(
        ZipArchive archive,
        IEnumerable<(DateOnly Date, AttachmentEntity? Attachment)> records,
        CancellationToken cancellationToken)
    {
        var usedPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var (date, attachment) in records)
        {
            if (attachment is null) continue;

            var monthFolder = date.ToString("yyyy-MM");
            var originalFilename = SanitizeFilename(attachment.CustomerFilename);
            var relativePath = $"Attachments/{monthFolder}/{originalFilename}";

            var suffix = 2;
            while (!usedPaths.Add(relativePath))
            {
                var extension = Path.GetExtension(originalFilename);
                var name = Path.GetFileNameWithoutExtension(originalFilename);
                relativePath = $"Attachments/{monthFolder}/{name}-{suffix}{extension}";
                suffix++;
            }

            var encryptedPath = attachment.GetPath(tenantProvider.AttachmentsDirectory());
            if (!File.Exists(encryptedPath)) continue;

            var encryptedBytes = await File.ReadAllBytesAsync(encryptedPath, cancellationToken);
            var bytes = attachment.Key.Length > 0
                ? attachment.Decrypt(encryptedBytes, tenantProvider.GetUserObjectId()).Bytes
                : encryptedBytes;

            var entry = archive.CreateEntry(relativePath, CompressionLevel.Optimal);
            await using var entryStream = entry.Open();
            await entryStream.WriteAsync(bytes, cancellationToken);
        }
    }

    private static string AttachmentPath(DateOnly date, AttachmentEntity? attachment)
    {
        if (attachment is null) return string.Empty;
        var filename = SanitizeFilename(attachment.CustomerFilename);
        return $"Attachments/{date:yyyy-MM}/{filename}";
    }

    private static string SanitizeFilename(string filename)
    {
        var invalid = Path.GetInvalidFileNameChars();
        var safe = new string(filename.Where(character => !invalid.Contains(character) && character != '/' && character != '\\').ToArray()).Trim();
        return string.IsNullOrWhiteSpace(safe) ? "attachment.bin" : safe;
    }

    private static decimal CalculateGSTFreeSalesAmount(SalesEntity sales)
    {
        decimal expectedGst = sales.Amount * 0.1m;
        decimal actualGst = sales.GST;
        decimal gstDelta = expectedGst - actualGst;
        if (gstDelta < 1) return 0;

        var salesWithGst = actualGst * 10m;
        var salesWithoutGst = sales.Amount - salesWithGst;
        return salesWithoutGst < 0 ? 0 : salesWithoutGst;
    }

    private static bool InFinancialYear(DateOnly date, (DateOnly Start, DateOnly End)[] ranges)
    {
        return ranges.Any(range => date >= range.Start && date < range.End);
    }
}

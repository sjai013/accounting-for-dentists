using System.Security.Cryptography;
using AccountingForDentists.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Contract.Shared;

public partial class ContractIncomeForm(IDbContextFactory<AccountingContext> contextFactory)
{
    const int maxFileSize = 5000000;
    [SupplyParameterFromForm]
    public ContractViewModel Model { get; set; } = new();

    [Parameter]
    public ContractViewModel? InitialModel { get; set; }

    string[] RegisteredBusinessNames { get; set; } = [];

    [Parameter]
    public required EventCallback<ContractViewModel> OnSubmit { get; set; }

    [Parameter]
    public required EventCallback OnCancel { get; set; }

    private string InvoiceFileInputId { get; set; } = Guid.NewGuid().ToString();

    protected override async Task OnInitializedAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync();

        RegisteredBusinessNames = await context.Businesses.Select(x => x.Name).ToArrayAsync();
    }

    protected override void OnParametersSet()
    {
        Model = InitialModel ?? Model;
    }

    private async Task Submit(Microsoft.AspNetCore.Components.Forms.EditContext args)
    {
        await OnSubmit.InvokeAsync(Model);

    }
    private async Task Cancel(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        await OnCancel.InvokeAsync();
    }

    private async Task LoadInvoiceFile(Microsoft.AspNetCore.Components.Forms.InputFileChangeEventArgs args)
    {
        InvoiceFileInputId = Guid.NewGuid().ToString();

        using var stream = args.File.OpenReadStream(maxFileSize);
        using var memoryStream = new MemoryStream();
        using var md5 = MD5.Create();
        await stream.CopyToAsync(memoryStream);
        string md5hash = Convert.ToHexStringLower(md5.ComputeHash(memoryStream));
        Model.File = new()
        {
            Bytes = memoryStream.ToArray(),
            Name = args.File.Name,
            MD5Hash = md5hash
        };

    }
    private void UnloadInvoiceFile(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        Model.File = null;
    }
}

public class ContractViewModel
{
    public decimal TotalSalesAmount { get; set; }
    public decimal TotalSalesGSTAmount { get; set; }
    public decimal TotalExpensesAmount { get; set; }
    public decimal TotalExpensesGSTAmount { get; set; }
    public decimal IncomeAmount { get => (TotalSalesAmount + TotalSalesGSTAmount) - (TotalExpensesAmount + TotalExpensesGSTAmount); }
    public decimal IncomeGST { get => TotalExpensesGSTAmount - TotalSalesGSTAmount; }
    public decimal TotalIncome { get => IncomeAmount + IncomeGST; }
    public DateTime InvoiceDate { get; set; } = DateTime.Today;
    public string ClinicName { get; set; } = string.Empty;
    public FileModel? File { get; set; }

    public class FileModel
    {
        public byte[] Bytes = [];
        public string Name = string.Empty;
        public string MD5Hash = string.Empty;
    }
}
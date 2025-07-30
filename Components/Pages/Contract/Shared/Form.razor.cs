using AccountingForDentists.Components.Pages.Shared.InputFile;
using AccountingForDentists.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace AccountingForDentists.Components.Pages.Contract.Shared;

public partial class Form(IDbContextFactory<AccountingContext> contextFactory, NavigationManager navigationManager)
{
    const int maxFileSize = 5000000;
    [SupplyParameterFromForm]
    public ContractViewModel Model { get; set; } = new();

    [Parameter]
    public ContractViewModel? InitialModel { get; set; }

    public FileSelectedViewModel? SelectedFile { get; set; }

    string[] RegisteredBusinessNames { get; set; } = [];

    [Parameter]
    public required EventCallback<ContractSubmitViewModel> OnSubmit { get; set; }

    [Parameter]
    public required EventCallback OnCancel { get; set; }

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
        await OnSubmit.InvokeAsync(new()
        {
            AttachmentId = Model.AttachmentId,
            ClinicName = Model.ClinicName,
            File = SelectedFile != null ? new()
            {
                Bytes = SelectedFile.Bytes,
                Filename = SelectedFile.Filename
            } : null,
            InvoiceDate = Model.InvoiceDate,
            TotalExpensesAmount = Model.TotalExpensesAmount,
            TotalExpensesGSTAmount = Model.TotalExpensesGSTAmount,
            TotalSalesAmount = Model.TotalSalesAmount,
            TotalSalesGSTAmount = Model.TotalSalesGSTAmount
        });

    }
    private async Task Cancel(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        await OnCancel.InvokeAsync();
    }
    private Task SelectFile(FileSelectedViewModel args)
    {
        SelectedFile = args;
        Model.AttachmentId = null;
        Model.File = new()
        {
            Filename = SelectedFile.Filename,
            Size = SelectedFile.Bytes.Length
        };
        return Task.CompletedTask;
    }
    private Task FileDownload(FileViewModel args)
    {
        if (Model.File is null) return Task.CompletedTask;
        navigationManager.NavigateTo($"/portal/download/{Model.AttachmentId}", true);
        return Task.CompletedTask;
    }
    private Task RemoveFile()
    {
        Model.AttachmentId = null;
        Model.File = null;
        SelectedFile = null;
        return Task.CompletedTask;
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
    public DateOnly InvoiceDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string ClinicName { get; set; } = string.Empty;
    public Guid? AttachmentId { get; set; }
    public FileViewModel? File { get; set; }
}

public class ContractSubmitViewModel
{
    public decimal TotalSalesAmount { get; set; }
    public decimal TotalSalesGSTAmount { get; set; }
    public decimal TotalExpensesAmount { get; set; }
    public decimal TotalExpensesGSTAmount { get; set; }
    public decimal IncomeAmount { get => (TotalSalesAmount + TotalSalesGSTAmount) - (TotalExpensesAmount + TotalExpensesGSTAmount); }
    public decimal IncomeGST { get => TotalExpensesGSTAmount - TotalSalesGSTAmount; }
    public decimal TotalIncome { get => IncomeAmount + IncomeGST; }
    public DateOnly InvoiceDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public string ClinicName { get; set; } = string.Empty;
    public Guid? AttachmentId { get; set; }
    public FileSelectedViewModel? File { get; set; }
}
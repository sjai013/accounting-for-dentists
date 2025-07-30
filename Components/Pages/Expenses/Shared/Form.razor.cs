using AccountingForDentists.Components.Pages.Shared.InputFile;
using AccountingForDentists.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace AccountingForDentists.Components.Pages.Expenses.Shared;

public partial class Form(IDbContextFactory<AccountingContext> contextFactory, NavigationManager navigationManager)
{
    public string[] RegisteredBusinessNames { get; set; } = [];
    public string SelectedItem = string.Empty;

    [Parameter]
    public ExpensesFormViewModel? InitialModel { get; set; }
    public ExpensesFormViewModel Model { get; set; } = new();
    public FileSelectedViewModel? SelectedFile { get; set; }
    private bool IsDisabled { get; set; } = false;

    [Parameter]
    public required EventCallback<ExpensesFormSubmitViewModel> OnSubmit { get; set; }

    [Parameter]
    public required EventCallback OnCancel { get; set; }
    protected override async Task OnInitializedAsync()
    {
        using var context = await contextFactory.CreateDbContextAsync();

        RegisteredBusinessNames = await context.Businesses.Select(x => x.Name).ToArrayAsync();
    }

    protected override void OnParametersSet()
    {
        Model = InitialModel ?? new() { };
    }
    private async Task Submit(Microsoft.AspNetCore.Components.Forms.EditContext args)
    {
        IsDisabled = true;
        try
        {
            await Task.Delay(1000);

            await OnSubmit.InvokeAsync(new()
            {
                Amount = Model.Amount,
                AttachmentId = Model.AttachmentId,
                BusinessName = Model.BusinessName,
                Description = Model.Description,
                File = SelectedFile is not null ? new()
                {
                    Bytes = SelectedFile.Bytes,
                    Filename = SelectedFile.Filename
                } : null,
                GST = Model.GST,
                InvoiceDate = Model.InvoiceDate
            });
        }
        finally
        {
            IsDisabled = false;
        }

    }
    private Task FileDownload(FileViewModel args)
    {
        if (Model.File is null) return Task.CompletedTask;
        navigationManager.NavigateTo($"/portal/download/{Model.AttachmentId}", true);
        return Task.CompletedTask;
    }
    private Task SelectFile(FileSelectedViewModel args)
    {
        SelectedFile = args;
        Model.AttachmentId = null;
        return Task.CompletedTask;
    }
    private Task Cancel(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        return OnCancel.InvokeAsync();
    }
    private Task RemoveFile()
    {
        Model.AttachmentId = null;
        Model.File = null;
        SelectedFile = null;
        return Task.CompletedTask;
    }
}

public class ExpensesFormViewModel
{
    public DateOnly InvoiceDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public decimal Amount { get; set; }
    public decimal GST { get; set; }
    public decimal Total => Amount + GST;
    public string Description { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public Guid? AttachmentId { get; set; }
    public FileViewModel? File { get; set; }
}

public class ExpensesFormSubmitViewModel
{
    public DateOnly InvoiceDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
    public decimal Amount { get; set; }
    public decimal GST { get; set; }
    public decimal Total => Amount + GST;
    public string Description { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public Guid? AttachmentId { get; set; }
    public FileSelectedViewModel? File { get; set; }
}
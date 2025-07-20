using AccountingForDentists.Components.Pages.Shared.InputFile;
using AccountingForDentists.Infrastructure;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace AccountingForDentists.Components.Pages.Expenses.Shared;

public partial class Form(IDbContextFactory<AccountingContext> contextFactory, IJSRuntime JSRuntime)
{
    public string[] RegisteredBusinessNames { get; set; } = [];
    public string SelectedItem = string.Empty;

    [Parameter]
    public ExpensesFormViewModel? InitialModel { get; set; }
    public ExpensesFormViewModel Model { get; set; } = new();


    [Parameter]
    public required EventCallback<ExpensesFormViewModel> OnSubmit { get; set; }

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
    private Task Submit(Microsoft.AspNetCore.Components.Forms.EditContext args)
    {
        return OnSubmit.InvokeAsync(Model);
    }
    private async Task FileDownload(FileViewModel args)
    {
        if (Model.File is null) return;
        Model.AttachmentId = null;
        await JSRuntime.InvokeVoidAsync("open", $"/portal/download/{Model.AttachmentId}", "");
    }
    private Task SelectFile(FileViewModel args)
    {
        Model.File = args;
        Model.AttachmentId = null;
        return Task.CompletedTask;
    }
    private Task Cancel(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        return OnCancel.InvokeAsync();
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
using AccountingForDentists.Components.Pages.Contract.Shared;
using AccountingForDentists.Components.Pages.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.JSInterop;

namespace AccountingForDentists.Components.Pages.Contract;

public partial class Index(IDbContextFactory<AccountingContext> contextFactory, NavigationManager navigationManager)
{
    List<ContractIncomeEntity>? SFAEntities;

    [SupplyParameterFromQuery]
    public string? Business { get; set; }

    [SupplyParameterFromQuery]
    public int FY { get; set; }

    List<OptionList<string>.Option>? Businesses { get; set; }

    DeleteModal DeleteConfirmModal { get; set; } = null!;

    protected override void OnInitialized()
    {
        if (FY == default)
            FY = DateTime.Now.AddMonths(6).Year;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        var waitTask = Task.Delay(250);
        if (firstRender)
        {
            await UpdateBusinessEntities();
            await waitTask;
            this.StateHasChanged();
        }

        if (SFAEntities is null)
        {
            await RenderUpdateEntities();
            await waitTask;
            this.StateHasChanged();
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        this.SFAEntities = null;
        this.StateHasChanged();
    }

    private async Task UpdateBusinessEntities()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        List<string> businessesEntities = await context.ContractIncome
            .Select(x => x.BusinessName.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .OrderBy(x => x)
            .Distinct()
            .ToListAsync();
        this.Businesses = [new OptionList<string>.Option() { Label = "All", Value = null }, new OptionList<string>.Option() { Label = "(Unspecified)", Value = "" }, .. businessesEntities.Select(x => new OptionList<string>.Option() { Label = x, Value = x })];
    }

    private async Task RenderUpdateEntities()
    {
        List<ContractIncomeEntity> sfaEntities = await GetEntities();
        this.SFAEntities = sfaEntities;
    }

    private async Task<List<ContractIncomeEntity>> GetEntities()
    {
        using var context = await contextFactory.CreateDbContextAsync();
        var sfaEntitiesQuery = context.ContractIncome
        .Include(x => x.SalesEntity)
        .Include(x => x.ExpensesEntity)
        .Include(x => x.Attachment)
        .Where(x => Business == null || x.BusinessName.Trim() == Business.Trim());

        if (FY != default)
        {
            DateOnly startDate = DateOnly.FromDateTime(new DateTime(FY - 1, 7, 1));
            DateOnly endDate = DateOnly.FromDateTime(new DateTime(FY, 7, 1));
            sfaEntitiesQuery = sfaEntitiesQuery.Where(x => x.InvoiceDateReference.Date >= startDate && x.InvoiceDateReference.Date < endDate);
        }

        List<ContractIncomeEntity> sfaEntities = await sfaEntitiesQuery
        .OrderByDescending(x => x.InvoiceDateReference.Date)
        .Include(x => x.InvoiceDateReference)
        .ToListAsync();

        return sfaEntities;
    }

    private void SelectBusiness(string? selectedBusiness)
    {
        string newUri;
        Dictionary<string, object?> uriParams = new()
        {
            ["Business"] = selectedBusiness,
            ["FY"] = FY
        };

        newUri = navigationManager.GetUriWithQueryParameters(uriParams);
        Business = selectedBusiness;
        navigationManager.NavigateTo(newUri);
    }

    void SelectFY(int FY)
    {
        this.FY = FY;
        string newUri;
        if (FY == default)
        {
            newUri = navigationManager.GetUriWithQueryParameter("FY", (int?)null);
        }
        else
        {
            newUri = navigationManager.GetUriWithQueryParameter("FY", FY);
        }
        navigationManager.NavigateTo(newUri);
    }

    void EditContractIncome(ContractIncomeEntity item)
    {
        string currentBaseUri = navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);
        string baseEditUri = $"{currentBaseUri}/Edit/{item.ContractualAgreementId}";
        Dictionary<string, string?> param = new() {
            {"returnUri", navigationManager.Uri}
        };

        Uri editUri = new(QueryHelpers.AddQueryString(baseEditUri, param));

        navigationManager.NavigateTo(editUri.ToString(), forceLoad: true);
    }

    void AddContractIncome(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        string currentBaseUri = navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);
        string baseAddUri = $"{currentBaseUri}/Add";
        Dictionary<string, string?> param = new() {
            {"returnUri", navigationManager.Uri}
        };

        Uri addUri = new(QueryHelpers.AddQueryString(baseAddUri, param));

        navigationManager.NavigateTo(addUri.ToString(), forceLoad: true);
    }

    Task DownloadInvoice(ContractIncomeEntity item)
    {
        if (item.Attachment is null) return Task.CompletedTask;
        navigationManager.NavigateTo($"/portal/download/{item.Attachment.AttachmentId}", true);
        return Task.CompletedTask;
    }

    private static ContractListItem.ContractListItemViewModel ContractListItemViewModelFromEntity(ContractIncomeEntity entity)
    {
        return new()
        {
            BusinessName = entity.BusinessName,
            InvoiceDate = entity.InvoiceDateReference.Date,
            ExpensesExclGst = entity.ExpensesEntity?.Amount ?? 0,
            SalesExclGst = entity.SalesEntity?.Amount ?? 0,
            ShowAttachmentButton = entity.Attachment is not null
        };
    }

    private async Task DeleteContractIncome(ContractIncomeEntity item)
    {
        using var context = await contextFactory.CreateDbContextAsync();

        // Delete expense
        if (item.ExpensesEntity is not null) context.Expenses.Remove(item.ExpensesEntity);

        // Delete sale
        if (item.SalesEntity is not null) context.Sales.Remove(item.SalesEntity);

        // Delete SFA record
        context.ContractIncome.Remove(item);
        await context.SaveChangesAsync();
        this.SFAEntities = null;
    }
}
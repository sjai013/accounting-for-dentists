using AccountingForDentists.Components.Pages.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Sales;

public partial class Index(IDbContextFactory<AccountingContext> contextFactory, NavigationManager navigationManager)
{
    [SupplyParameterFromQuery]
    public string? Business { get; set; }

    [SupplyParameterFromQuery]
    public int FY { get; set; }

    List<OptionList<string>.Option>? Businesses { get; set; }

    public List<SalesEntity>? SaleEntities { get; set; }
    public string? Error { get; set; }

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

        if (SaleEntities is null)
        {
            using var context = await contextFactory.CreateDbContextAsync();
            var entitiesQuery = context.Sales
                .Include(x => x.DateReference)
                .Where(x => Business == null || x.BusinessName.Trim() == Business.Trim());

            if (FY != default)
            {
                DateOnly startDate = DateOnly.FromDateTime(new DateTime(FY - 1, 7, 1));
                DateOnly endDate = DateOnly.FromDateTime(new DateTime(FY, 7, 1));
                entitiesQuery = entitiesQuery.Where(x => x.DateReference.Date >= startDate && x.DateReference.Date < endDate);
            }

            SaleEntities = await entitiesQuery.OrderByDescending(x => x.DateReference.Date).ToListAsync();
            await waitTask;
            this.StateHasChanged();
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        this.SaleEntities = null;
        this.StateHasChanged();
    }

    private async Task DeleteSale(SalesEntity item)
    {
        try
        {
            using var context = await contextFactory.CreateDbContextAsync();
            context.Sales.Remove(item);
            await context.SaveChangesAsync();
            SaleEntities?.Remove(item);
            this.StateHasChanged();
        }
        catch
        {
            this.Error = "There was an error deleting this record.  It may be linked to an income entry.";
        }

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

    private Task EditSale(SalesEntity item)
    {
        string currentBaseUri = navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);
        string baseEditUri = $"{currentBaseUri}/Edit/{item.SalesId}";
        Dictionary<string, string?> param = new() {
            {"returnUri", navigationManager.Uri}
        };

        Uri editUri = new(QueryHelpers.AddQueryString(baseEditUri, param));

        navigationManager.NavigateTo(editUri.ToString(), forceLoad: true);

        return Task.CompletedTask;
    }
    private Task AddExpense(Microsoft.AspNetCore.Components.Web.MouseEventArgs args)
    {
        string currentBaseUri = navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);
        string addUriString = $"{currentBaseUri}/Add";
        Dictionary<string, string?> param = new() {
            {"returnUri", navigationManager.Uri}
        };

        Uri addUri = new(QueryHelpers.AddQueryString(addUriString, param));
        navigationManager.NavigateTo(addUri.ToString(), forceLoad: true);
        return Task.CompletedTask;
    }

}
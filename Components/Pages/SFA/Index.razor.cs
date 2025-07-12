using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.SFA;

public partial class Index(AccountingContext context, NavigationManager navigationManager)
{
    List<ServiceFacilitiesAgreementEntity> SFAEntities = [];

    [SupplyParameterFromQuery]
    public string? Business { get; set; }

    [SupplyParameterFromQuery]
    public int FY { get; set; }

    List<string> Businesses { get; set; } = [];

    protected override async Task OnParametersSetAsync()
    {
        var sfaEntitiesQuery = context.ServiceFacilitiesAgreements
            .Include(x => x.SalesEntity)
            .Include(x => x.ExpensesEntity)
            .Where(x => x.BusinessName == Business || Business == null);

        if (FY != default)
        {
            DateOnly startDate = DateOnly.FromDateTime(new DateTime(FY - 1, 7, 1));
            DateOnly endDate = DateOnly.FromDateTime(new DateTime(FY, 7, 1));
            sfaEntitiesQuery = sfaEntitiesQuery.Where(x => x.InvoiceDate >= startDate && x.InvoiceDate < endDate);
        }

        this.SFAEntities = await sfaEntitiesQuery
            .OrderByDescending(x => x.InvoiceDate)
            .ToListAsync();

        this.Businesses = await context.Businesses.Select(x => x.Name).OrderBy(x => x).ToListAsync();
    }
    private void SelectBusiness(string? selectedBusiness)
    {
        string newUri;
        if (selectedBusiness == Business)
        {
            newUri = navigationManager.GetUriWithQueryParameter("Business", (string?)null);
            Business = null;

        }
        else
        {
            newUri = navigationManager.GetUriWithQueryParameter("Business", selectedBusiness);
            Business = selectedBusiness;

        }
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
}
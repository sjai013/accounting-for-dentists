using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Contract;

public partial class Edit(AccountingContext context)
{
    [Parameter]
    public string entityGuidString { get; set; }

    ContractualAgreementsEntity? entity { get; set; }

    protected override async Task OnParametersSetAsync()
    {
        if (!Guid.TryParse(entityGuidString, out var entityGuid))
        {
            return;
        }

        entity = await context.ContractIncome.Where(x => x.ContractualAgreementId == entityGuid)
                .Include(x => x.SalesEntity)
                .Include(x => x.ExpensesEntity)
                .SingleOrDefaultAsync();

        if (entity is null) return;


    }
}
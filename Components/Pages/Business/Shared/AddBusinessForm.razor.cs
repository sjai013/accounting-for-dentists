using System.Threading.Tasks;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;

namespace AccountingForDentists.Components.Pages.Business.Shared;

public partial class AddBusinessForm(AccountingContext context, TenantProvider tenantProvider)
{
    public async Task AddNewEntity(BusinessEntityModel model)
    {
        var tenantId = tenantProvider.TenantId;
        var userObjectId = tenantProvider.UserObjectId;

        var businessEntity = new BusinessEntity()
        {
            TenantId = tenantId,
            UserObjectId = userObjectId,
            Name = model.BusinessName,
        };

        context.Businesses.Add(businessEntity);
        await context.SaveChangesAsync();
    }
}
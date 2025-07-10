using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Business;

public partial class Index(AccountingContext context)
{
    public IEnumerable<BusinessEntity> GetBusinessEntities()
    {
        return context.Businesses.AsEnumerable();
    }
}
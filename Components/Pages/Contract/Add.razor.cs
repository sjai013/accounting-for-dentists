using System.Security.Cryptography;
using AccountingForDentists.Components.Pages.Contract.Shared;
using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Contract;

public partial class Add(IDbContextFactory<AccountingContext> contextFactory, NavigationManager navigationManager)
{
    public string[] RegisteredBusinessNames { get; set; } = [];

    [SupplyParameterFromQuery]
    public string? ReturnUri { get; set; } = string.Empty;

    public string? Error { get; set; }

    private async Task Submit(ContractSubmitViewModel model)
    {
        using var context = await contextFactory.CreateDbContextAsync();
        DateContainerEntity dateReference = new()
        {
            DateContainerId = Guid.CreateVersion7(),
            Date = model.InvoiceDate
        };
        context.DateReferences.Add(dateReference);

        ContractIncomeEntity entity = new()
        {
            ContractualAgreementId = Guid.CreateVersion7(),
            InvoiceDateReference = dateReference,
        };

        context.ContractIncome.Add(entity);
        try
        {
            using var transaction = await context.Database.BeginTransactionAsync();
            HelperMethods.AddOrUpdate(context, model, ref entity);

            await context.SaveChangesAsync();
            await transaction.CommitAsync();
            NavigateBack();
        }
        catch (Exception e)
        {
            Error = e.Message;
        }
    }

    public void NavigateBack()
    {
        if (!string.IsNullOrEmpty(ReturnUri))
        {
            navigationManager.NavigateTo(ReturnUri);
        }
    }
}

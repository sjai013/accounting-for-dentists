using AccountingForDentists.Infrastructure;
using AccountingForDentists.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.Components.Pages.Expenses;

public partial class Index(IDbContextFactory<AccountingContext> contextFactory, NavigationManager navigationManager)
{
    public List<ExpensesEntity>? ExpenseEntities { get; set; }
    public string? Error { get; set; }
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        Task waitTask = Task.Delay(500);
        using var context = await contextFactory.CreateDbContextAsync();
        var entities = await context.Expenses.Include(x => x.DateReference).OrderByDescending(x => x.DateReference.Date).ToListAsync();
        await waitTask;
        ExpenseEntities = entities;
        StateHasChanged();
    }

    private async Task DeleteExpense(ExpensesEntity item)
    {
        try
        {
            using var context = await contextFactory.CreateDbContextAsync();
            context.Expenses.Remove(item);
            await context.SaveChangesAsync();
            ExpenseEntities.Remove(item);
            this.StateHasChanged();
        }
        catch
        {
            this.Error = "There was an error deleting this record.  It may be linked to an income entry.";
        }

    }

    private Task EditExpense(ExpensesEntity item)
    {
        string currentBaseUri = navigationManager.ToAbsoluteUri(navigationManager.Uri).GetLeftPart(UriPartial.Path);
        string baseEditUri = $"{currentBaseUri}/Edit/{item.ExpensesId}";
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
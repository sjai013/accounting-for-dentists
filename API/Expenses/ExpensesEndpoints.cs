using AccountingForDentists.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace AccountingForDentists.API.Expenses;

public static class ExpensesEndpoints
{
    public static void MapExpensesEndpoints(this WebApplication app)
    {
        app.MapPost("/api/expense/{expenseId}", async (string expenseId, IFormFile file, IDbContextFactory<AccountingContext> contextFactory) =>
        {
            if (!Guid.TryParse(expenseId, out var parsedExpenseId))
            {
                return Results.BadRequest("Invalid expense ID format.");
            }

            // Placeholder for actual add/update logic
            using var context = contextFactory.CreateDbContext();
            var existingExpense = context.Expenses.Where(x => x.ExpensesId == parsedExpenseId);
            if (existingExpense is null)
            {
                // Add new expense logic here

            }
            else
            {
                // Update existing expense logic here
            }

            return Results.Ok();
        });
    }
}

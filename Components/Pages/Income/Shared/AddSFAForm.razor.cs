namespace AccountingForDentists.Components.Pages.Income.Shared;

public partial class AddSFAForm
{
    public SFAModel Model { get; set; } = new();

    public async Task Submit()
    {

    }

    public class SFAModel
    {
        public decimal TotalSalesAmount { get; set; }
        public decimal ExpensesAmount { get; set; }
        public decimal ServiceFeeAmount { get; set; }
        public decimal ServiceFeeGSTAmount { get; set; }
        public decimal IncomeAmount { get; set; }
    }
}

namespace AccountingForDentists.Models
{
    public class Income
    {
        public Guid TenantId { get; set; }
        public Guid IncomeId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public decimal GST { get; set; }
        public decimal Total { get; set; }
    }
}
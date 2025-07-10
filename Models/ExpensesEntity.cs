namespace AccountingForDentists.Models
{
    public record ExpensesEntity
    {
        public Guid ExpensesId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public decimal GST { get; set; }
        public decimal Total { get; set; }
        public string Description { get; set; } = string.Empty;
        public SalesEntity? Sales { get; set; }
        public string BusinessEntity { get; set; } = string.Empty;
    }

}
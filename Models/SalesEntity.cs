namespace AccountingForDentists.Models
{
    public record SalesEntity
    {
        public required string TenantId { get; set; }
        public required string UserObjectId { get; set; }
        public Guid SalesId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public decimal GST { get; set; }
        public decimal Total { get; set; }
        public string Description { get; set; } = string.Empty;
        public ICollection<ExpensesEntity>? Expenses { get; set; }
        public BusinessEntity? BusinessEntity { get; set; }
    }

}
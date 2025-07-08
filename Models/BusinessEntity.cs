namespace AccountingForDentists.Models
{
    public record BusinessEntity
    {
        public Guid TenantId { get; set; }
        public Guid BusinessEntityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<SalesEntity>? Sales { get; set; }
        public ICollection<ExpensesEntity>? Expenses { get; set; }
    }

}
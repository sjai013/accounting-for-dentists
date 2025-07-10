namespace AccountingForDentists.Models
{
    public record BusinessEntity
    {
        public required string TenantId { get; set; }
        public required string UserObjectId { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<SalesEntity>? Sales { get; set; }
        public ICollection<ExpensesEntity>? Expenses { get; set; }
    }

}
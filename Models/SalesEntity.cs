namespace AccountingForDentists.Models
{
    public record SalesEntity
    {
        public Guid SalesId { get; set; }
        public DateOnly Date { get; set; }
        public decimal Amount { get; set; }
        public decimal GST { get; set; }
        public decimal Total => Amount + GST;
        public string Description { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
    }

}
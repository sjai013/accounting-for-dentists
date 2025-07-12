using System.ComponentModel.DataAnnotations.Schema;

namespace AccountingForDentists.Models
{
    public record SalesEntity
    {
        public required Guid TenantId { get; set; }
        public required Guid UserId { get; set; }
        public required Guid SalesId { get; set; }
        public DateOnly Date { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(9,2)")]
        public decimal GST { get; set; }
        public decimal Total => Amount + GST;
        public string Description { get; set; } = string.Empty;
        public string BusinessName { get; set; } = string.Empty;
    }

}
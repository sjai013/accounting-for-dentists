namespace AccountingForDentists.Models;

public record BusinessEntity
{
    public required Guid TenantId { get; set; }
    public required Guid UserId { get; set; }
    public required string Name { get; set; } = string.Empty;
}

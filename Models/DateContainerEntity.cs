namespace AccountingForDentists.Models;

/// <summary>
/// This is used to enforce consistent dates between various entitites
/// For example, between a Contract Income entity and its related 
/// Sales and Expenses entities
/// </summary>
public record DateContainerEntity
{
    public required Guid TenantId { get; set; }
    public required Guid UserId { get; set; }
    public required Guid DateContainerId { get; set; }
    public DateOnly Date { get; set; }
}
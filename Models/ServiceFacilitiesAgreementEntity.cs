namespace AccountingForDentists.Models;

public record ServiceFacilitiesAgreementEntity
{
    public required Guid TenantId { get; set; }
    public required Guid UserId { get; set; }
    public required Guid ServiceFacilityAgreementId { get; set; }
    public DateOnly InvoiceDate { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public ExpensesEntity? ExpensesEntity { get; set; }
    public SalesEntity? SalesEntity { get; set; }
}
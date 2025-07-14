namespace AccountingForDentists.Models;

public record ContractIncomeEntity
{
    public required Guid TenantId { get; set; }
    public required Guid UserId { get; set; }
    public required Guid ContractualAgreementId { get; set; }
    public DateOnly InvoiceDate { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public ExpensesEntity? ExpensesEntity { get; set; }
    public SalesEntity? SalesEntity { get; set; }
}
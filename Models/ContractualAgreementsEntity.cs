namespace AccountingForDentists.Models;

public record ContractIncomeEntity
{
    public required Guid ContractualAgreementId { get; set; }
    public required DateContainerEntity InvoiceDateReference { get; set; }
    public string BusinessName { get; set; } = string.Empty;
    public ExpensesEntity? ExpensesEntity { get; set; }
    public SalesEntity? SalesEntity { get; set; }
    public AttachmentEntity? Attachment { get; set; }
}

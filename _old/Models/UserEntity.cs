namespace AccountingForDentists.Models
{
    public record UserEntity
    {
        public Guid UserId { get; set; }
        public required string BusinessName { get; set; }

    }

}
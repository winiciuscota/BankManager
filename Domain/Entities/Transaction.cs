namespace BankManager.Api.Domain.Entities;
public class Transaction
{
    public Transaction(decimal amount)
    {
        Amount = amount;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public decimal Amount { get; set; }
}
namespace BankManager.Api.Domain.Entities;
public class Account
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public decimal Balance => Transactions.Sum(t => t.Amount);

    public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    public Account(string name, decimal initialBalance = 100)
    {
        Name = name;

        if(initialBalance < 100)
            throw new ArgumentException("Initial balance must be greater than 100");

        Transactions.Add(new(initialBalance));
    }

    public void Deposit(decimal amount)
    {
        if (amount > 10000)
            throw new ArgumentException("Deposit amount must not exceed $10,000");

        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be greater than 0");

        Transactions.Add(new(amount));
    }

    public void Withdraw(decimal amount)
    {
        if (Balance - amount < 100)
            throw new ArgumentException("Balance must be greater than 100");

        if (amount > Balance * .9m)
            throw new ArgumentException("Withdraw amount must not exceed 90% of the balance");

        if (amount <= 0)
            throw new ArgumentException("Withdraw amount must be greater than 0");

        Transactions.Add(new(-amount));
    }
}
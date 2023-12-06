namespace CurrencyExchange.Contracts.Models
{
    public sealed class UserModel
    {
        public Guid UserId { get; set; }
        public string UserLogin { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
    }
}

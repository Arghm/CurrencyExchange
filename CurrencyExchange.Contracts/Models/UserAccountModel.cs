namespace CurrencyExchange.Contracts.Models
{
    public class UserAccountModel
    {
        public Guid UserId { get; set; }
        public string UserLogin { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public AccountModel[] Accounts { get; set; }
    }
}

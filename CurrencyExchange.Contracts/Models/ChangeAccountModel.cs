namespace CurrencyExchange.Contracts.Models
{
    public sealed class ChangeAccountModel
    {
        public Guid UserId { get; set; }
        public string CurrencyCode { get; set; }
        public decimal ChangeSum { get; set; }
    }
}

namespace CurrencyExchange.Contracts.Entities
{
    public class ExchangeCurrency
    {
        public Guid UserId { get; set; }
        public Guid FromCurrencyId { get; set; }
        public Guid ToCurrencyId { get; set; }

        /// <summary>
        /// Курс валют.
        /// </summary>
        public decimal ExcangeRate { get; set; }

        /// <summary>
        /// Сумма зачисления на счет на который переводятся деньги.
        /// </summary>
        public decimal ExchangeToAmount { get; set; }

        /// <summary>
        /// Сумма списания со счета с которого переводятся деньги.
        /// </summary>
        public decimal ExchangeFromAmount { get; set; }

        /// <summary>
        /// Комиссия в %.
        /// </summary>
        public decimal ExchangeFeePercentage { get; set; }
    }
}

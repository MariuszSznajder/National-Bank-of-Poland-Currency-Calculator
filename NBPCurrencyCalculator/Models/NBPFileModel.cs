namespace NBPCurrencyCalculator.Models
{
    internal class NBPFileModel
    {
        public class Rate
        {
            public string no { get; set; }
            public string effectiveDate { get; set; }
            public double mid { get; set; }
            public double bid { get; set; }
        }

        public class Root
        {
            public Rate rate { get; set; }
            public string table { get; set; }
            public string currency { get; set; }
            public string code { get; set; }
            public List<Rate> rates { get; set; }
        }

        public class ExchangeRate
        {
            public string currency { get; set; }
            public string code { get; set; }
            public decimal mid { get; set; }
        }

        public class ExchangeRateTable
        {
            public string table { get; set; }
            public string no { get; set; }
            public DateTime effectiveDate { get; set; }
            public List<ExchangeRate> rates { get; set; }
        }
    }
}

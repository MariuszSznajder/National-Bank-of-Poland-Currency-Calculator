using NBPCurrencyCalculator.DataGenerator;
using System.Globalization;
using System.Text.RegularExpressions;

namespace NBPCurrencyCalculator.GetHTMLData
{
    internal class JSONDataProvider
    {
        private static HttpClient client = new HttpClient();
        private static string path = "https://api.nbp.pl/api/exchangerates/tables/a/?format=json";

        public string GetData(string url)
        {
            string result = string.Empty;
            try
            {
                using (HttpResponseMessage response = client.GetAsync(url).Result)
                {
                    using (HttpContent content = response.Content)
                    {
                        result = content.ReadAsStringAsync().Result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        #region helpers
        public string GetURL()
        {
            string readyUrl = string.Empty;

            string firstPartOfLink = "http://api.nbp.pl/api/exchangerates/rates/a/";
            string lastPartOfLink = "/last/1/?format=json";

            readyUrl = string.Concat(firstPartOfLink, GetUserCurrency(), lastPartOfLink);
            return readyUrl;
        }

        public string GetURLForSpecificDate()
        {
            string readyUrl = string.Empty;

            string firstPartOfLink = "http://api.nbp.pl/api/exchangerates/rates/a/";
            string lastPartOfLink = "/?format=json";

            readyUrl = string.Concat(firstPartOfLink, GetUserCurrency(), "/",
            GetExchangeRateDate(), lastPartOfLink);
            return readyUrl;
        }



        private protected string GetUserCurrency()
        {
            List<string> currencies = Generator.GetListOfRates(GetData(path));
            Console.WriteLine("Please provide currency ID:");
            string currencyID = CurrencyIDValidator(Convert.ToString(Console.ReadLine()).ToUpper());
            while (!currencies.Contains(currencyID))
            {
                Console.WriteLine("Please provide existing currency ID:");
                currencyID = CurrencyIDValidator(Convert.ToString(Console.ReadLine()).ToUpper());

            }
            return currencyID;
        }


        private protected static string GetExchangeRateDate()
        {
            DateTime date;
            Console.WriteLine("Please provide currency exchange rate date (yyyy-MM-dd):");

            string currencyExchangeRateDate = DateformatValidator(Convert.ToString(Console.ReadLine()).ToLower());
            while (currencyExchangeRateDate.isFutureDate() || currencyExchangeRateDate.isWeekend())
            {
                if (currencyExchangeRateDate.isFutureDate())
                {
                    Console.WriteLine("Currency is not quoted in the future! Please provide correct date:");
                    currencyExchangeRateDate = DateformatValidator(Convert.ToString(Console.ReadLine()).ToLower());
                }
                if (currencyExchangeRateDate.isWeekend())
                {
                    Console.WriteLine("Currency is not quoted in the weekend days! Please provide other day:");
                    currencyExchangeRateDate = DateformatValidator(Convert.ToString(Console.ReadLine()).ToLower());
                }
            }
            return currencyExchangeRateDate;

        }

        private string CurrencyIDValidator(string input)
        {
            while (input.Length != 3)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Please provide correct currency ID:");
                Console.ResetColor();
                input = Convert.ToString(Console.ReadLine());
                if (input.Length == 3)
                    break;
            }
            input = ForeignCurrencyChecker(input);
            return input;
        }

        private static string DateformatValidator(string userInput)
        {
            const string format = @"\d{4}-\d{2}-\d{2}";
            var match = Regex.Match(userInput, format, RegexOptions.IgnoreCase);
            if (!match.Success)
            {
                Console.WriteLine("Please provide date in correct format!");
                userInput = Convert.ToString(Console.ReadLine());
            }
            return userInput;
        }

        private static string ForeignCurrencyChecker(string input)
        {
            const string pln = "pln";

            while (string.Equals(input, pln, StringComparison.OrdinalIgnoreCase))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Polish ISO Code is not allowed! Provide foreign currency id:");
                Console.ResetColor();
                input = Console.ReadLine();
                if (input != pln)
                    break;
            }
            return input;
        }

    }

    public static class DatesChecker
    {
        private static DateTime date;
        public static bool isWeekend(this string input)
        {
            DateTime dateTime = DateValidator(input, out date);
            return dateTime.DayOfWeek == DayOfWeek.Saturday || dateTime.DayOfWeek == DayOfWeek.Sunday;

        }

        public static bool isFutureDate(this string input)
        {
            DateTime dateTime = DateValidator(input, out date);
            return dateTime > DateTime.Today;
        }

        private static DateTime DateValidator(string input, out DateTime date)
        {
            string[] formats =
                { "dd/MM/yyyy", "dd/M/yyyy", "d/M/yyyy", "d/MM/yyyy",
                    "dd/MM/yy", "dd/M/yy", "d/M/yy", "d/MM/yy", "yyyy-MM-dd"};

            DateTime.TryParseExact(Convert.ToString(input), formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out date);
            return date;
        }
    }
    #endregion
}

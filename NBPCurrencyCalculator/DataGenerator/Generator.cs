using NBPCurrencyCalculator.ApplicationMenu;
using NBPCurrencyCalculator.GetHTMLData;
using Newtonsoft.Json;
using static NBPCurrencyCalculator.Models.NBPFileModel;

namespace NBPCurrencyCalculator.DataGenerator
{
    internal class Generator : JSONDataProvider
    {
        private static bool dataReader = true;

        public static void DisplayData(string inputFile)
        {
            while (dataReader)
            {
                Root getData = GetJsonData(inputFile);


                if (string.IsNullOrEmpty(inputFile))
                {
                    continue;
                }

                foreach (var data in getData.rates)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"Effective date: {data.effectiveDate}, Currency quote: {data.mid}\n");
                    Console.ResetColor();
                    return;
                }
                Console.WriteLine($"\nCurrency ID date does not exist. Do you want to provide other id? (Y/N)");
                if (Console.ReadKey().Key.Equals(ConsoleKey.Y))
                {
                    Console.WriteLine("Please provide currency ID: ");
                }
                else
                {
                    Console.Clear();
                    dataReader = false;
                    AppMenu.ClosingApp(10);
                    AppMenu.PressEnter();
                }
            }
        }



        public static void GetCalculatedValue(string jsonFilePath)
        {
            while (true)
            {
                Root getData = GetJsonData(jsonFilePath);

                if (string.IsNullOrEmpty(jsonFilePath))
                {
                    continue;
                }

                foreach (var data in getData.rates)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    var calculatedData = data.mid *= UserAmount();
                    Console.Write($"For date: {data.effectiveDate} value of your currency is: {Math.Round(calculatedData, 2)} pln.");
                    Console.ResetColor();
                    return;
                }
            }
        }


        private protected static int UserAmount()
        {
            int amount = 0;

            Console.WriteLine("Please provide amount to calculate:");
            Console.ResetColor();
            var input = Console.ReadLine();

            while (!int.TryParse(input, out amount))
            {
                Console.WriteLine("Please provide a valid integer:");
                input = Console.ReadLine();
            }

            return amount;
        }

        #region helpers


        private static Root GetJsonData(string inputFile)
        {
            return JsonConvert.DeserializeObject<Root>(inputFile);
        }

        public static List<string> GetListOfRates(string jsonFilePath)
        {
            while (true)
            {
                List<string> exchanges = new List<string>();
                List<ExchangeRateTable> exchangeRateTables = JsonConvert.DeserializeObject<List<ExchangeRateTable>>(jsonFilePath);

                if (string.IsNullOrEmpty(jsonFilePath))
                {
                    continue;
                }
                foreach (var table in exchangeRateTables)
                {
                    foreach (var rate in table.rates)
                    {
                        exchanges.Add(rate.code);
                    }
                }
                return exchanges;
            }
        }
        public static void DisplayListOfRates(string jsonFilePath)
        {


            List<ExchangeRateTable> exchangeRateTables = JsonConvert.DeserializeObject<List<ExchangeRateTable>>(jsonFilePath);

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("List of all available quoted instruments:");
            Console.ResetColor();
            foreach (var table in exchangeRateTables)
            {
                foreach (var rate in table.rates)
                {
                    Console.Write(rate.code + ", ");
                }
            }
        }


        #endregion
    }
}

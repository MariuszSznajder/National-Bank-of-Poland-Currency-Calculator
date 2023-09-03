using NBPCurrencyCalculator.DataGenerator;
using NBPCurrencyCalculator.GetHTMLData;

namespace NBPCurrencyCalculator.ApplicationMenu
{
    internal class AppMenu : Generator
    {
        private static JSONDataProvider _provider = new JSONDataProvider();
        private static bool _anotherOperation = true;

        private static string url = "https://api.nbp.pl/api/exchangerates/tables/a/?format=json";

        private static ConsoleKeyInfo key;

        public static void DisplayUserData()
        {
            WelcomeUser();
            DisplayMenu();
            key = Console.ReadKey();
            while (_anotherOperation)
            {
                if (key.Key != ConsoleKey.D1 && key.Key != ConsoleKey.D2 && key.Key != ConsoleKey.D3 && key.Key != ConsoleKey.D4)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("\nPlease provide correct operation:");
                    Console.ResetColor();
                    key = Console.ReadKey();
                    if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.D3 || key.Key == ConsoleKey.D4)
                    {
                        Operations();
                    }
                }
                else
                {
                    Operations();
                }
                Console.WriteLine("\nDo you want to perform another operation? (y/n)");
                key = Console.ReadKey();
                if (key.Key == ConsoleKey.N)
                {
                    _anotherOperation = false;
                    Console.Clear();
                    ClosingApp(10);
                    PressEnter();
                }
                if (key.Key == ConsoleKey.Y)
                {
                    Console.WriteLine("\nPlease choose operation: ");
                    key = Console.ReadKey();
                }
            }
        }

        private static void Operations()
        {
            switch (key.Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("");
                    DisplayData(_provider.GetData(_provider.GetURL()));
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine("");
                    GetCalculatedValue(_provider.GetData(_provider.GetURLForSpecificDate()));
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("");
                    DisplayData(_provider.GetData(_provider.GetURLForSpecificDate()));
                    break;
                case ConsoleKey.D4:
                    Console.WriteLine("");
                    Generator.DisplayListOfRates(_provider.GetData(url));
                    break;
            }
        }

        private static void WelcomeUser()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            LoadingApp(10);
            Console.Clear();
            Console.WriteLine("Online Exchange Calculator");
            Console.ResetColor();
            Thread.Sleep(3000);
            Console.Clear();
        }

        private static void DisplayMenu()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Choose one of the following options:");
            Console.ResetColor();
            Console.WriteLine("1: Display current exchange rate.");
            Console.WriteLine("2: Provide amount to calculate exchange rate.");
            Console.WriteLine("3: Provide date to get historical value for specific date exchange rate.");
            Console.WriteLine("4: Display all available instruments.");
        }

        private static void LoadingApp(int timer)
        {
            Console.Write("Loading app.");
            for (int i = 0; i < timer; i++)
            {
                Thread.Sleep(200);
                Console.Write(".");
            }
        }

        public static void PressEnter()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Press enter to exit!");
            Console.ResetColor();
        }

        public static void ClosingApp(int timer)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("Closing application.");
            for (int i = 0; i < timer; i++)
            {
                Thread.Sleep(200);
                Console.Write(".");
            }
            Console.ResetColor();
        }
    }
}

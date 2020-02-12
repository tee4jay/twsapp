using TwsClient;
using System;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var marketData = new MarketData();
            marketData.PriceTicked += marketData_PriceTicked;

            marketData.Start();
            Console.WriteLine("Press any key to stop...");
            Console.ReadKey();

            Console.WriteLine("Stopping...");
            marketData.Stop();
        }

        static void marketData_PriceTicked(object sender, PriceTickedEventArgs e)
        {
            Console.WriteLine("PriceTicked: " + e.Price);
        }
    }
}

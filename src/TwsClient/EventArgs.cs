using System;

namespace TwsClient
{
    public class PriceTickedEventArgs : EventArgs
    {
        public double Price { get; private set; }

        public PriceTickedEventArgs(double price)
        {
            this.Price = price;
        }
    }
}
using System;
using TwsClient.Models;

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

    public class MarketDataTickedEventArgs : EventArgs
    {
        public string Type { get; private set; }
        public string MarketDataTicked { get; private set; }

        public MarketDataTickedEventArgs(string type, string message)
        {
            this.Type = type;
            this.MarketDataTicked = message;
        }
    }

    public class RtvTickedEventArgs : EventArgs
    {
        public RTVolume Rtv { get; private set; }

        public RtvTickedEventArgs(RTVolume rtv)
        {
            this.Rtv = rtv;
        }
    }
}
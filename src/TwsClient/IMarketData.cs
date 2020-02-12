using System;

namespace TwsClient
{
    public interface IMarketData
    {
        event EventHandler<PriceTickedEventArgs> PriceTicked;

        void Start();
        void Stop();
    }
}
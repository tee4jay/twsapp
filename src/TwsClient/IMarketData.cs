using System;
using System.Collections.Concurrent;

namespace TwsClient
{
    public interface IMarketData
    {
        BlockingCollection<string> Messages { get; }
        void Start();
        void Stop();
    }
}
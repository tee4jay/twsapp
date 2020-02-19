using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WebApp.Hubs;
using TwsClient;

namespace WebApp
{
    public class MonitorMarketDataService : BackgroundService
    {
        private readonly ILogger<MonitorMarketDataService> _logger;
        private readonly IHubContext<TwsHub> _hubContext;
        private readonly MarketData _marketData;
        public MonitorMarketDataService(
            ILogger<MonitorMarketDataService> logger,
            IHubContext<TwsHub> hubContext,
            MarketData marketData)
        {
            _logger = logger;
            _hubContext = hubContext;
            _marketData = marketData;
            _marketData.MarketDataTicked += marketData_Ticked;
        }

        private async void marketData_Ticked(object sender, MarketDataTickedEventArgs e)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", e.Type, e.MarketDataTicked);
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"GracePeriodManagerService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug($" GracePeriod background task is stopping."));

            return Task.Run(() => {
                _marketData.Start();

                while (!stoppingToken.IsCancellationRequested)
                {
                    //var item = messages.Take(stoppingToken);

                    _logger.LogDebug($"GracePeriod task doing background work.");

                    // This eShopOnContainers method is querying a database table
                    // and publishing events into the Event Bus (RabbitMQ / ServiceBus)
                    //CheckConfirmedGracePeriodOrders();
                    //await _hubContext.Clients.All.SendAsync("ReceiveMessage", "background", "Time: " + DateTime.Now);

                    //Console.WriteLine("PriceTicked: " + item);
                    //await _hubContext.Clients.All.SendAsync("ReceiveMessage", "PriceTicked", item);

                    //await Task.Delay(1000, stoppingToken);
                }

                _marketData.Stop();

                _logger.LogDebug($"GracePeriod background task is stopping.");
            });
        }

    }
}
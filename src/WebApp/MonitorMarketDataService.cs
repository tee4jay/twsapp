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
        private readonly IMarketData _marketData;
        public MonitorMarketDataService(
            ILogger<MonitorMarketDataService> logger,
            IHubContext<TwsHub> hubContext,
            IMarketData marketData
            )
        {
            _logger = logger;
            _hubContext = hubContext;
            _marketData = marketData;

            _marketData.PriceTicked += marketData_PriceTicked;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogDebug($"GracePeriodManagerService is starting.");

            stoppingToken.Register(() =>
                _logger.LogDebug($" GracePeriod background task is stopping."));

            _marketData.Start();

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogDebug($"GracePeriod task doing background work.");

                // This eShopOnContainers method is querying a database table
                // and publishing events into the Event Bus (RabbitMQ / ServiceBus)
                //CheckConfirmedGracePeriodOrders();
                //await _hubContext.Clients.All.SendAsync("ReceiveMessage", "background", "Time: " + DateTime.Now);

                await Task.Delay(1000, stoppingToken);
            }

            _marketData.Stop();

            _logger.LogDebug($"GracePeriod background task is stopping.");
        }

        private void marketData_PriceTicked(object sender, PriceTickedEventArgs e)
        {
            //Console.WriteLine("PriceTicked: " + e.Price);
            Task.Run(async () => await _hubContext.Clients.All.SendAsync("ReceiveMessage", "PriceTicked", e.Price)).Wait();
        }
    }
}
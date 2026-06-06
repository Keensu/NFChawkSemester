using NFChawk.Models.Interfaces;
using NFChawk.Services;

namespace NFChawk.Background
{
    public class AuctionBackgroundService
        : BackgroundService
    {
        private readonly IServiceScopeFactory
            _scopeFactory;

        public AuctionBackgroundService(
            IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope =
                    _scopeFactory.CreateScope();

                var service = scope.ServiceProvider
                    .GetRequiredService
                    <IAuctionFinalizationService>();

                await service
                    .FinalizeExpiredAuctionsAsync();

                await Task.Delay(
                    TimeSpan.FromSeconds(30),
                    stoppingToken);
            }
        }
    }
}
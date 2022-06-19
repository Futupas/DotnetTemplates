using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FutupasConsoleApp
{
    internal class MainWorker : IHostedService
    {
        protected readonly IConfiguration config;
        protected readonly ILogger logger;


        public MainWorker(
            IConfiguration config,
            ILoggerFactory loggerFactory
        ) {
            this.config = config;
            logger = loggerFactory.CreateLogger<MainWorker>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation($"Hello world, my value is {config["MyKey"]}");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}

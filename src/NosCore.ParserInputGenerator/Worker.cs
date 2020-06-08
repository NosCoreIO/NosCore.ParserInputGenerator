using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NosCore.Shared.I18N;

namespace NosCore.ParserInputGenerator
{
    public class Worker : BackgroundService
    {
        private ILogger<Worker> _logger;
        private const string ConsoleText = "PARSER INPUT GENERATOR - NosCoreIO";


        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                Logger.PrintHeader(ConsoleText);
            }
            catch
            {
                // ignored as header is not important
            }

        }
    }
}

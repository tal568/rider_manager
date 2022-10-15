using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using rider_manager.Interfaces;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;
        private readonly IRideManger _rideManger;

        public Worker(ILogger<Worker> logger, IConfiguration config, IRideManger rideManger)
        {
            _logger = logger;
            _config = config;
            _rideManger = rideManger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            int wait_sec = _config.GetValue<int>("wait_sec");
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(wait_sec));
            _logger.LogInformation("wating for next tik in {sec}", wait_sec);
            while (await timer.WaitForNextTickAsync())
            {
                try
                {
                    bool found_ride = _rideManger.LoadMassages()
                    .ChoseMassage()
                    .ReplayToChosenMassage();


                    if (found_ride)
                        timer.Dispose();
                }
                catch (Exception e)
                {
                    _logger.LogError("failed to run trying in {wait_sec} sec" + e.Message + "\n" + e.StackTrace, wait_sec);
                }


            }
        }
    }
}
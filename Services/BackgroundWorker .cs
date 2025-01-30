namespace Unified.Connectors.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class BackgroundWorker : BackgroundService
    {
        private readonly IBackgroundTaskQueue _taskQueue;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<BackgroundWorker> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="taskQueue"></param>
        /// <param name="scopeFactory"></param>
        /// <param name="logger"></param>
        public BackgroundWorker(IBackgroundTaskQueue taskQueue, IServiceScopeFactory scopeFactory, ILogger<BackgroundWorker> logger)
        {
            _taskQueue = taskQueue;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stoppingToken"></param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var workItem = await _taskQueue.DequeueAsync(stoppingToken);

                try
                {
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        await workItem(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred executing background task.");
                }
            }
        }
    }


}

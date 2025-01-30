using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Unified.Connectors.DBContext;
using Unified.Connectors.EntityModels;
using Unified.Connectors.Model;
using Unified.Connectors.Services;

namespace Unified.Connectors.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HoldController : ControllerBase
    {
        private readonly UnifiedDbContext _dbContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;

        public HoldController(UnifiedDbContext dbContext, IServiceScopeFactory scopeFactory, IBackgroundTaskQueue backgroundTaskQueue)
        {
            _dbContext = dbContext;
            _backgroundTaskQueue = backgroundTaskQueue;
            _scopeFactory = scopeFactory;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateJob([FromBody] UserRequestModel userRequestModel)
        {
            try
            {
                var job = new JobQueue
                {
                    JobId = Guid.NewGuid(),
                    Status = "InProgress",
                    CreationDate = DateTime.UtcNow,
                    ModificationDate = DateTime.UtcNow,
                };

                _dbContext.JobQueues.Add(job);
                await _dbContext.SaveChangesAsync();

                //Async operation
                _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
                {
                    await ProcessJobAsync(job.JobId, userRequestModel);
                });
                return Ok(new
                {
                    Message = "Job created successfully",
                    JobId = job.JobId
                });
            }
            catch (Exception ex)
            {
                throw new Exception("Error Processing while Creating JobQueue");
            }
            finally
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobId"></param>
        /// <param name="userRequestModel"></param>
        /// <returns></returns>
        private async Task ProcessJobAsync(Guid jobId, UserRequestModel userRequestModel)
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<UnifiedDbContext>();
                try
                {
                    var holdStats = new HoldService(dbContext).CreateHoldAsync(jobId, userRequestModel).GetAwaiter().GetResult();
                    if (holdStats.indexedItemCount != null && holdStats.indexedItemsSize != null)
                    {
                        var job = dbContext.JobQueues.FirstOrDefaultAsync(x => x.JobId == jobId).Result;
                        if (job != null)
                        {
                            job.Status = "Complete";
                            job.Count = holdStats.indexedItemCount;
                            job.Size = holdStats.indexedItemsSize;
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    await UpdateStatusAsync(dbContext, jobId, ex.Message);
                    throw new Exception(ex.Message);
                }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="jobId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task UpdateStatusAsync(UnifiedDbContext dbContext, Guid jobId, string message)
        {
            try
            {
                var job = dbContext.JobQueues.FirstOrDefaultAsync(x => x.JobId == jobId).Result;
                if (job != null)
                {
                    job.Status = "Failed";
                    job.Error = message;
                    job.ModificationDate = DateTime.UtcNow;

                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }
    }

}

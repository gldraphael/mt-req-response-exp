using CommonLib.MessageContracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApp
{
    public class WorkerService
    {
        private readonly IRequestClient<GetInfo> getinfoClient;
        private readonly ILogger<WorkerService> logger;

        public WorkerService(
            IRequestClient<GetInfo> getinfoClient,
            ILogger<WorkerService> logger)
        {
            this.getinfoClient = getinfoClient;
            this.logger = logger;
        }

        public async Task<GetInfoResponse?> GetInfoAsync(
            int id, 
            TimeSpan? timeout = null, 
            CancellationToken cancellationToken = default)
        {
            try
            {
                var response = await getinfoClient.GetResponse<GetInfoResponse>(
                    new GetInfo { Id = id }, cancellationToken, timeout ?? RequestTimeout.Default);
                return response.Message;
            }
            catch (MassTransitException me)
            {
                logger.LogError(exception: me, message: me.Message);
                return null;
            }
        }
    }
}

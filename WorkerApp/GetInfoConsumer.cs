using CommonLib.MessageContracts;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WorkerApp
{
    public sealed class GetInfoConsumer : IConsumer<GetInfo>
    {
        private readonly GetInfoService getInfoService;
        private readonly ILogger<GetInfoConsumer> logger;

        public GetInfoConsumer(GetInfoService getInfoService, ILogger<GetInfoConsumer> logger)
        {
            this.getInfoService = getInfoService;
            this.logger = logger;
        }

        public async Task Consume(ConsumeContext<GetInfo> context)
        {
            await Task.Delay(5_000); // wait for 5 seconds to simulate a long-running job
            var response = getInfoService.GetInfo(context.Message.Id);
            logger.LogInformation("{@GetInfoResponse}", response);
            await context.RespondAsync(response);
        }
    }
}

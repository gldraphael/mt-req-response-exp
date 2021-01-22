using ClientApp;
using CommonLib.Extensions;
using CommonLib.MessageContracts;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;


async Task Run(WorkerService worker)
{
    // send 10 requests to the worker app
    // and print the responses as they come in
    var tasks = Enumerable.Range(1, 10)
        .Select(id => 
            worker.GetInfoAsync(id, timeout: TimeSpan.FromSeconds(7))
                  .ContinueWith(r => {
                    Log.Information("[{@id}]: {@Result}", id, r.Result);
                  }
            ));
    await Task.WhenAll(tasks);
}


// setup configuration
var config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

// configure serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(config)
    .CreateLogger();

// setup the DI container
var serviceProvider = (new ServiceCollection() as IServiceCollection)
    .AddLogging(lb => lb.AddSerilog(dispose: true))
    .AddSingleton<WorkerService>()
    .AddMassTransit(
            config: config,
            configureDependencies: x =>
            {
                x.AddRequestClient<GetInfo>();
            },
            configureEndpoints: (bc, rc, sp) => { },
            configureRabbitMq: (rbc, sp) => { },
            configureAzureServiceBus: (sbc, sp) => { }
        )
    .BuildServiceProvider();
    


var worker = serviceProvider.GetRequiredService<WorkerService>();
var bus = serviceProvider.GetRequiredService<IBusControl>();
await bus.StartAsync();

try
{
    /**
     * Re: https://github.com/MassTransit/MassTransit/issues/2256
     * There are two things to try:
     * 1. Execute Run(), and then wait for long enough for the temporary queue to get auto-deleted, 
     *    before executing Run() again.
     *    I've tried this, but the temporary queue never got autodeleted.
     * 2. Execute Run(), and then wait for a short period of time. Manually delete the temporary queue
     *    during the wait time, before executing Run() again.
     *    I'm able to reproduce #2256 with this approach. 
     */

    await Run(worker);
    Log.Information("Delete the temporary queue now.");
    await Task.Delay(TimeSpan.FromMinutes(1));
    Log.Information("Continuing execution...");
    await Run(worker);
}
finally
{
    await bus.StopAsync();
}

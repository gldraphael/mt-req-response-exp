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


WorkerService? worker;
async Task Run(int numberOfRequests)
{
    // send `numberOfRequests` requests to the worker app
    // and print the responses as they come in
    var tasks = Enumerable.Range(1, numberOfRequests)
        .Select(id => {

            return worker.GetInfoAsync(id, timeout: TimeSpan.FromMinutes(2))
                .ContinueWith(r =>
                {
                    Log.Information("[{@id}]: {@Result}", id, r.Result);
                }
            );
        });
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
    


worker = serviceProvider.GetRequiredService<WorkerService>();
var bus = serviceProvider.GetRequiredService<IBusControl>();
await bus.StartAsync();

try
{
    await Run(numberOfRequests: 1);
    Log.Information("Manually delete the temporary queue now.");
    await Task.Delay(TimeSpan.FromMinutes(1));
    Log.Information("Continuing execution...");
    await Run(numberOfRequests: 1);
    Log.Information("Re-running...");
    await Run(numberOfRequests: 1);
}
finally
{
    await bus.StopAsync();
}

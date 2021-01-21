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
            configureEndpoints: (a, b, c) => { },
            configureRabbitMq: (rbc, sp) => { },
            configureAzureServiceBus: (sbc, sp) => { }
        )
    .BuildServiceProvider();
    


var worker = serviceProvider.GetRequiredService<WorkerService>();
var bus = serviceProvider.GetRequiredService<IBusControl>();
await bus.StartAsync();

try
{
    var tasks = Enumerable.Range(1, 10)
        .Select(id => 
            worker.GetInfoAsync(id, timeout: TimeSpan.FromSeconds(6))
                  .ContinueWith(r => {
                    Log.Information("[{@id}]: {@Result}", id, r.Result);
                  }
            ));
    await Task.WhenAll(tasks);
}
finally
{
    await bus.StopAsync();
}

using CommonLib.Internal;
using Microsoft.Extensions.Configuration;

namespace CommonLib.Extensions
{
    internal static class ConfigurationExtensions
    {
        public static string GetAzureServiceBusConnectionString(this IConfiguration config) =>
            config.GetConnectionString("AzureServiceBus");

        public static MessagingTransport GetMessagingTransport(this IConfiguration config) =>
            config.GetValue<MessagingTransport>("Messaging:Transport");

        public static RabbitMQOptions GetRabbitMqOptions(this IConfiguration config) =>
            config.GetSection("RabbitMQ").Get<RabbitMQOptions>();
    }
}

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Linq;
using HealthChecks.System;

namespace NetCoreHealthCheckSample.Extensions
{
    internal static class RegisterExtensions
    {
        internal static void AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks()
                .AddSqlServer(
                    configuration.GetConnectionString("DefaultConnection"),
                    "SELECT 1;",
                    "Sql Server",
                    HealthStatus.Degraded,
                    timeout: TimeSpan.FromSeconds(30),
                    tags: new[] { "db", "sql", "sqlServer", })
                .AddRedis(
                    $"{configuration["Redis:Endpoint"]},abortConnect=false",
                    "Redis",
                    HealthStatus.Degraded,
                    new[] { "redis", "cache" })
                .AddRabbitMQ(configuration.GetConnectionString("RabbitMQ"),
                    null,
                    "RabbitMQ",
                    HealthStatus.Degraded,
                    new[]
                    {
                        "rabbitmq", "queue", "message", "broker"
                    })
                .AddElasticsearch(configuration["Serilog:ElasticsearchEndpoint"],
                    "ElasticSearch",
                    HealthStatus.Degraded,
                    new[]
                    {
                    "elastic","search"
                    })
                .AddDiskStorageHealthCheck(delegate(DiskStorageOptions options)
                    {
                        options.AddDrive(@"C:\", 10000); 

                    }, 
                    "C: Drive", 
                    HealthStatus.Degraded)
                .AddUrlGroup(new Uri(configuration["UrlGroup:Hesapkurdu"]),
                    "Hesapkurdu",
                    HealthStatus.Degraded,
                    new[]
                    {
                    "url"
                    });


            services.AddHealthChecksUI(setupSettings: settings =>
            {
                settings.AddWebhookNotification("Slack Notification WebHook", "Your_Slack_WebHook_Uri_Goes_Here",
                    "{\"text\": \"[[LIVENESS]] is failing with the error message : [[FAILURE]]\"}",
                    "{\"text\": \"[[LIVENESS]] is recovered.All is up & running !\"}");
            });
            //.AddCheck("Test Database",new SqlCheck(Configuration.GetConnectionString("DefaultConnection"))); //our custom check
        }
    }
}

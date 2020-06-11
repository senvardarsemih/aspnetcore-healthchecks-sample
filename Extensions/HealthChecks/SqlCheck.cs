using System;
using System.Data.SqlClient;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace NetCoreHealthCheckSample.Extensions.HealthChecks
{
    public class SqlCheck : IHealthCheck
    {
        private readonly string _connectionString;
        public SqlCheck(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            await using (var connection = new SqlConnection(_connectionString))
            {
                try
                {
                    await connection.OpenAsync(cancellationToken);

                    var command = connection.CreateCommand();
                    command.CommandText = "Select 1";

                    await command.ExecuteNonQueryAsync(cancellationToken);
                }
                catch (Exception e)
                { 
                    return new HealthCheckResult(context.Registration.FailureStatus,exception:e);
                }
            }
            return HealthCheckResult.Healthy();
        }
    }
}

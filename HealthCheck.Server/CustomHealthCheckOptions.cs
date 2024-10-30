using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System.Text.Json;

namespace HealthCheck.Server;

public class CustomHealthCheckOptions: HealthCheckOptions
{
    public CustomHealthCheckOptions() : base()
    {
        var jsonSerializationOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        ResponseWriter = async (c, r) =>
        {
            c.Response.ContentType = "application/json";
            c.Response.StatusCode = StatusCodes.Status200OK;
            
            var result = JsonSerializer.Serialize(
                new
                {
                    checks = r.Entries.Select(e =>
                        new
                        {
                            name = e.Key,
                            responseTime = e.Value.Duration,
                            status = e.Value.Status.ToString(),
                            description = e.Value.Description
                        }),
                    totalStatus = r.Status.ToString(),
                    totalResponseTime = r.TotalDuration.TotalMilliseconds
                }, 
                jsonSerializationOptions);
            await c.Response.WriteAsync(result);
        };
    }
}
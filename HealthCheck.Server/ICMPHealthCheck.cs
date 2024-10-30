﻿using System.Net.NetworkInformation;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace HealthCheck.Server;

public class ICMPHealthCheck: IHealthCheck
{
    private readonly string _host;
    private readonly int _healthyRoundTripTime;

    public ICMPHealthCheck(string host, int healthyRoundTripTime)
    {
        _host = host;
        _healthyRoundTripTime = healthyRoundTripTime;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = new CancellationToken())
    {
        try
        {
            using var ping = new Ping();
            var reply = await ping.SendPingAsync(_host);

            switch (reply.Status)
            {
                case IPStatus.Success:
                    var msg = $"ICPM to {_host} took {reply.RoundtripTime} ms.";
                    return reply.RoundtripTime > _healthyRoundTripTime
                        ? HealthCheckResult.Degraded(msg)
                        : HealthCheckResult.Healthy(msg);
                default:
                    return HealthCheckResult.Unhealthy($"ICMP to {_host} failed: {reply.Status}");
            }
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy($"ICMP to {_host} failed: {e.Message}");
        }
    }
}
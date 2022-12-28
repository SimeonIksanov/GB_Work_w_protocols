using System;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.HttpLogging;
using Polly;
using SampleService.Controllers;
using SampleService.Services.Client;
using SampleService.Services.Client.Impl;

namespace SampleService.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureHttpFactory(this IServiceCollection services)
    {
        services.AddHttpClient<IRootServiceClient, RootServiceClient>()
            .AddTransientHttpErrorPolicy(policy =>
                policy.WaitAndRetryAsync(
                    retryCount: 3,
                    sleepDurationProvider: c => TimeSpan.FromSeconds(c * 3),
                    onRetry: (response, sleepDuration, attempt, context) =>
                    {
                        var logger = services.BuildServiceProvider().GetRequiredService<ILogger<Program>>();

                        var exception = response.Exception != null
                            ? response.Exception
                            : new Exception($"\n{response.Result.StatusCode}: {response.Result.RequestMessage}");

                        logger.LogError(exception, $"(attempt: {attempt}) RootServiceClient request exception.");
                    })
            );
    }

    public static void ConfigureLogging(this IServiceCollection services)
    {
        services.AddHttpLogging(options =>
        {
            options.LoggingFields = HttpLoggingFields.All | HttpLoggingFields.RequestQuery;
            options.RequestBodyLogLimit = 4096;
            options.ResponseBodyLogLimit = 4096;
            options.RequestHeaders.Add("Authentication");
            options.RequestHeaders.Add("X-Real-IP");
            options.RequestHeaders.Add("X-Forwarded-For");
        });
    }
}

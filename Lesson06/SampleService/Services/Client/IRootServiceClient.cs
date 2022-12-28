using System;
using RootServiceNamespace;

namespace SampleService.Services.Client;

public interface IRootServiceClient
{
    RootServiceNamespace.RootServiceClient RootServiceClient { get; }
    Task<IEnumerable<WeatherForecast>> Get();
}


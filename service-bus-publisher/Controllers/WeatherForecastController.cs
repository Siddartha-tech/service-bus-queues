using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;

namespace service_bus_publisher.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ServiceBusClient _client;

    public WeatherForecastController(ServiceBusClient client, ILogger<WeatherForecastController> logger)
    {
        _client = client;
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost]
    public async Task Post(WeatherForecast data)
    {
        // string connectionString = "Endpoint=sb://service-bus-practise-namespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=mZYoWDW+dM/04AI9N9O8gKQ2FFroZckX6lImmrNktBw=";
        // ServiceBusClient client = new ServiceBusClient(connectionString);
        ServiceBusSender sender = _client.CreateSender("add-weather-data");
        string body = JsonSerializer.Serialize(data);
        ServiceBusMessage message = new ServiceBusMessage(body);
        if (body.Contains("Cool"))
        {
            message.ScheduledEnqueueTime = DateTimeOffset.UtcNow.AddSeconds(15);
        }
        await sender.SendMessageAsync(message);
    }
}

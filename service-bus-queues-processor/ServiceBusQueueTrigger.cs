using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ServiceBusQueuesProcessor.Function
{
    public static class ServiceBusQueueTrigger
    {
        [FunctionName("ServiceBusQueueTrigger")]
        public static void Run([ServiceBusTrigger("add-weather-data", Connection = "servicebuspractisenamespace")] string myQueueItem, ILogger log)
        {
            Console.WriteLine($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
            log.LogInformation($"C# ServiceBus queue trigger function processed message: {myQueueItem}");
        }
    }
}

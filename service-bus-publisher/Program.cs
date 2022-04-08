using Azure.Identity;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAzureClients(builder =>
{
    // string connectionString = "Endpoint=sb://service-bus-practise-namespace.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=mZYoWDW+dM/04AI9N9O8gKQ2FFroZckX6lImmrNktBw=";
    //* while using managed identity we cannot use this directly *//
    // builder.AddServiceBusClient(connectionString);
    builder.AddClient<ServiceBusClient, ServiceBusClientOptions>((_, _, _) =>
    {
        return new ServiceBusClient("service-bus-practise-namespace.servicebus.windows.net", new DefaultAzureCredential());
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

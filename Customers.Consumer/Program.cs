using Amazon.SQS;
using Customers.Consumer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHostedService<QueueConsumerService>();
builder.Services.AddSingleton<IAmazonSQS, AmazonSQSClient>();
var app = builder.Build();


app.Run();
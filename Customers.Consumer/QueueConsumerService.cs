using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Customers.Consumer.Events;

namespace Customers.Consumer;

public class QueueConsumerService : BackgroundService
{
    private readonly IAmazonSQS _sqsClient;
    private readonly ILogger<QueueConsumerService> _logger;

    public QueueConsumerService(IAmazonSQS sqsClient, ILogger<QueueConsumerService> logger)
    {
        _sqsClient = sqsClient;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var queueUrlResponse = await _sqsClient.GetQueueUrlAsync("merchants", stoppingToken);
        var messageRequest = new ReceiveMessageRequest
        {
            MaxNumberOfMessages = 1,
            MessageAttributeNames = new List<string>{ "All"},
            MessageSystemAttributeNames = new List<string>{"All"},
            QueueUrl = queueUrlResponse.QueueUrl
        };

        while (!stoppingToken.IsCancellationRequested)
        {
            var response = await _sqsClient.ReceiveMessageAsync(messageRequest, stoppingToken);

            foreach (var message in response.Messages)
            {
                var messageType = message.MessageAttributes["MessageType"].StringValue;
                switch (messageType)
                {
                    case nameof(MerchantCreated):
                        var created = JsonSerializer.Deserialize<MerchantCreated>(message.Body);
                        _logger.LogInformation("Customer Created - {CustomerId}", created?.Id);
                        break;
                    case nameof(MerchantUpdated):
                        var updated = JsonSerializer.Deserialize<MerchantCreated>(message.Body);
                        _logger.LogInformation("Customer updated - {CustomerId}", updated?.Id);
                        break;
                    case nameof(MerchantDeleted):
                        var deleted = JsonSerializer.Deserialize<MerchantCreated>(message.Body);
                        _logger.LogInformation("Customer deleted - {CustomerId}", deleted?.Id);
                        break;
                    default:
                        _logger.LogInformation("Message not in right format");
                        break;
                }
                //_logger.LogInformation("Finished processing message - {MessageType}", messageType);
                await _sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle, stoppingToken);
                //_logger.LogInformation("Deleted message - {MessageType}", messageType);
            }
            await Task.Delay(3000, stoppingToken);
        }
    }
}
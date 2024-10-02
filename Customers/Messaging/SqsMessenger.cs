using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Options;

namespace Customers.Api.Messaging;

public class SqsMessenger : ISqsMessenger
{
    private readonly IAmazonSQS _sqs;
    private readonly IOptions<QueueSettings> _settings;

    public SqsMessenger(IAmazonSQS sqs, IOptions<QueueSettings> settings)
    {
        _sqs = sqs;
        _settings = settings;
    }

    public async Task<SendMessageResponse> SendMessageAsync<T>(T message)
    {
        var queueUrlResponse = await _sqs.GetQueueUrlAsync(_settings.Value.Name);

        var request = new SendMessageRequest
        {
            MessageAttributes = new Dictionary<string, MessageAttributeValue>
            {
                {
                    "MessageType", new MessageAttributeValue
                    {
                        DataType = "String",
                        StringValue = typeof(T).Name
                    }
                }
            },
            MessageBody = JsonSerializer.Serialize(message),
            QueueUrl = queueUrlResponse.QueueUrl
        };

        var response = await _sqs.SendMessageAsync(request);
        return response;

    }
}
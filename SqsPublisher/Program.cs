using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsPublisher;

var sqsClient = new AmazonSQSClient();

var merchant = new MerchantCreated
{
    Id = Guid.NewGuid(),
    Name = "EatAway Services Pvt Ltd",
    EmailAddress = "admin@eway.com"
};

var queueUrlResponse = await sqsClient.GetQueueUrlAsync("merchants");

var sendMessageRequest = new SendMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageBody = JsonSerializer.Serialize(merchant),
    MessageAttributes = new Dictionary<string, MessageAttributeValue>
    {
        {
            "MessageType", new MessageAttributeValue
            {
                DataType = "String",
                StringValue = nameof(MerchantCreated)
            }
        }
    }
    
};

var response = await sqsClient.SendMessageAsync(sendMessageRequest);

Console.WriteLine(response.MessageId);
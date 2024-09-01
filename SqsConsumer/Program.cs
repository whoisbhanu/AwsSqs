using System.Text.Json;
using Amazon.SQS;
using Amazon.SQS.Model;
using SqsConsumer;

var cts = new CancellationTokenSource();
var sqsClient = new AmazonSQSClient();
var queueUrlResponse = await sqsClient.GetQueueUrlAsync("merchants");
var messageRequest = new ReceiveMessageRequest
{
    QueueUrl = queueUrlResponse.QueueUrl,
    MessageAttributeNames = new List<string>{ "All"},
    MessageSystemAttributeNames = new List<string>{"All"}
};

while (!cts.IsCancellationRequested)
{
    var response = await sqsClient.ReceiveMessageAsync(messageRequest, cts.Token);
    foreach (var message in response.Messages)
    {
        Console.WriteLine($"MessageId : {message.MessageId}");
        Console.WriteLine($"Body : {message.Body}");
        var merchantCreated = JsonSerializer.Deserialize<MerchantCreated>(message.Body);

        await sqsClient.DeleteMessageAsync(queueUrlResponse.QueueUrl, message.ReceiptHandle);
    }

    await Task.Delay(3000, cts.Token);
}
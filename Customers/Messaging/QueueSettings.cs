namespace Customers.Api.Messaging;

public record QueueSettings
{
    public const string Key = "Queue";
    public string Name { get; init; }
}
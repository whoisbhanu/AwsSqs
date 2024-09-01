namespace Customers.Api.Contracts.Events;

public class MerchantUpdated
{
    public Guid Id { get; set; }
    public string EmailAddress { get; set; }
    public string GitHubUsername { get; set; }
    public string FullName { get; set; }
    public DateTime DateOfBirth { get; set; }
}
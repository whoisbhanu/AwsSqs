using Customers.Api.Contracts.Data;
using Customers.Api.Domain;
using Customers.Api.Contracts.Events;

namespace Customers.Api.Mapping;

public static class DtoToDomainMapper
{
    public static Customer ToCustomer(this CustomerDto customerDto)
    {
        return new Customer
        {
            Id = customerDto.Id,
            Email = customerDto.Email,
            GitHubUsername = customerDto.GitHubUsername,
            FullName = customerDto.FullName,
            DateOfBirth = customerDto.DateOfBirth
        };
    }
    
    public static MerchantCreated ToMerchantCreated(this CustomerDto customerDto)
    {
        return new MerchantCreated()
        {
            Id = customerDto.Id,
            EmailAddress= customerDto.Email,
            GitHubUsername = customerDto.GitHubUsername,
            FullName = customerDto.FullName,
            DateOfBirth = customerDto.DateOfBirth
        };
    }
    
    public static MerchantUpdated ToMerchantUpdated(this CustomerDto customerDto)
    {
        return new MerchantUpdated()
        {
            Id = customerDto.Id,
            EmailAddress= customerDto.Email,
            GitHubUsername = customerDto.GitHubUsername,
            FullName = customerDto.FullName,
            DateOfBirth = customerDto.DateOfBirth
        };
    }
}

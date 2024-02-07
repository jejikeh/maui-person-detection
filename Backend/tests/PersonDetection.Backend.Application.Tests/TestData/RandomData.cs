using Bogus;
using Microsoft.AspNetCore.Identity;
using PersonDetection.Backend.Infrastructure.Models.Identity;

namespace PersonDetection.Backend.Application.Tests.TestData;

public static class RandomData
{
    private static readonly Faker _faker = new Faker();
    
    public static IdentityUser GenerateUser()
    {
        return new IdentityUser(_faker.Internet.UserName()) 
        {
            Email = _faker.Internet.Email()
        };
    } 
}
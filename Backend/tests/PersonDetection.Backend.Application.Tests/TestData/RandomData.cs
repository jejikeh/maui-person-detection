using Bogus;
using PersonDetection.Backend.Infrastructure.Models.Identity;

namespace PersonDetection.Backend.Application.Tests.TestData;

public static class RandomData
{
    private static readonly Faker _faker = new Faker();
    
    public static User GenerateUser()
    {
        return new User(_faker.Internet.UserName(), _faker.Internet.Email());
    } 
}
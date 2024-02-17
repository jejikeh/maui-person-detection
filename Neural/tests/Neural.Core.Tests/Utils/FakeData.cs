using Bogus;

namespace Neural.Core.Tests.Utils;

public class FakeData
{
    private static readonly Faker _faker = new Faker();
    
    public static string FilePath() => _faker.System.FilePath();
}
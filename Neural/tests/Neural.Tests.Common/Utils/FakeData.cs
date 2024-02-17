using Bogus;

namespace Neural.Tests.Common.Utils;

public class FakeData
{
    private static readonly Faker _faker = new Faker();
    
    public static string FilePath() => _faker.System.FilePath();
}
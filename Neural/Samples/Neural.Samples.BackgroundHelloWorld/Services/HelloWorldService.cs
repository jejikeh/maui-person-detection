namespace Neural.Samples.BackgroundHelloWorld.Services;

public class HelloWorldService
{
    public async Task<string> HelloAsync()
    {
        await Task.Delay(1000 * Random.Shared.Next(1, 3));
        
        return "Hello, World!";
    }

    public async Task<string> ByeAsync()
    {
        await Task.Delay(1000 * Random.Shared.Next(3, 6));
        
        return "Bye, World!";
    }
}
using PersonDetection.Backend.Presentation.Models.PhotoProcess;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseHttpsRedirection();

app.MapGet("photo", (PhotoToProcess photoToProcess) => new ProcessedPhoto
{
    PhotoBase64 = "hello world",
    PersonsCount = 1
});

app.Run();
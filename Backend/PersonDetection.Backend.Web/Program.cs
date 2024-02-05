using Microsoft.AspNetCore.Mvc;
using PersonDetection.Backend.Application;
using PersonDetection.Backend.Application.Models;
using PersonDetection.Backend.Application.Services;
using PersonDetection.Backend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication(builder.Configuration)
    .AddInfrastructure();
    
var app = builder.Build();

app.MapPost("imagegen", async ([FromBody] Photo photo,[FromServices] PhotoProcessingService photoProcessingService) =>
{
    return Results.Ok(await photoProcessingService.ProcessPhotoAsync(photo));
});

app.Run();
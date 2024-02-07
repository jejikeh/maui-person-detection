using PersonDetection.Backend.Web.Configurations;

var builder = WebApplication.CreateBuilder(args).Configure();
var app = builder.Build().Configure();

await app.RunAppAsync();
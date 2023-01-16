using Blog.Service.NotionService;
using Blog.Shared.ValueObjects;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var appSettings = new AppSettings();
configuration.Bind(appSettings);

services.AddSingleton(appSettings);
services.AddRazorPages();
services.AddServerSideBlazor();
services.AddNotionClient(appSettings);

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
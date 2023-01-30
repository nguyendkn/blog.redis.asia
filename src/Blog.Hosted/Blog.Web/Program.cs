using Blog.Domain.DbContexts;
using Blog.Jobs;
using Blog.Shared.Extensions.HangfireExtensions;
using Blog.Shared.ValueObjects;
using Package.Mongo;
using Package.Notion;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;
var appSettings = new AppSettings();
configuration.Bind(appSettings);

services.AddSingleton(appSettings);
services.AddRazorPages();
services.AddServerSideBlazor();
services.AddMongoContext<BlogContext>(appSettings.MongoConfigs);
services.AddNotionClient(appSettings.NotionConfigs);
services.AddScoped<ICronJob, NotionDataService>();

services.AddHangfireConfiguration(appSettings, ScheduleContext.Schema);

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");
app.UseJobs<Program>();
app.UseHangfire<Program>();


app.Run();
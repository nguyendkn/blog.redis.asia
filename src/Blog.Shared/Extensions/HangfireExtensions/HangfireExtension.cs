using Blog.Shared.ValueObjects;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using HangfireBasicAuthenticationFilter;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace Blog.Shared.Extensions.HangfireExtensions;

public static class HangfireExtension
{
    public static void AddHangfireConfiguration(this IServiceCollection services, AppSettings appSettings,
        string scheduleSchema)
    {
        var mongoConfig = appSettings.MongoConfigs;
        var mongoUrlBuilder = new MongoUrlBuilder(appSettings.MongoConfigs.ConnectionString());
        var mongoClient = new MongoClient(mongoUrlBuilder.ToMongoUrl());
        var storage = new MongoStorage(mongoClient, mongoConfig.Database, new MongoStorageOptions
        {
            MigrationOptions = new MongoMigrationOptions
            {
                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                BackupStrategy = new CollectionMongoBackupStrategy()
            },
            Prefix = scheduleSchema,
            CheckConnection = false
        });
        services.AddHangfire(x => x.UseStorage(storage));
        JobStorage.Current = storage;
        services.AddHangfireServer();
    }

    public static void UseJobs<TProgram>(this WebApplication app)
    {
        var appSettings = AppSettings.Configuration<TProgram>();

        using var scope = app.Services.CreateScope();

        var jobs = scope.ServiceProvider.GetServices<ICronJob>();

        var scheduleTasks = appSettings.ScheduledTasks.ToDictionary(scheduledTask => scheduledTask.Id);

        foreach (var recurringJob in jobs)
        {
            var name = recurringJob.GetType().Name;

            if (scheduleTasks.TryGetValue(name, out var exp))
            {
                RecurringJob.AddOrUpdate(name, () => recurringJob.Run(), exp.CronExpression);
            }
        }
    }

    public static void UseHangfire<TProgram>(this WebApplication app)
    {
        var appSettings = AppSettings.Configuration<TProgram>();

        app.UseHangfireDashboard("/hangfire", new DashboardOptions
        {
            DashboardTitle = "Hangfire Dashboard",
            Authorization = new[]
            {
                new HangfireCustomBasicAuthenticationFilter
                {
                    User = appSettings.Hangfire.UserName,
                    Pass = appSettings.Hangfire.Password
                }
            },
            IgnoreAntiforgeryToken = true
        });
    }
}
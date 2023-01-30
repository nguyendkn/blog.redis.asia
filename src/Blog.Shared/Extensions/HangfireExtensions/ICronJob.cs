using Hangfire;

namespace Blog.Shared.Extensions.HangfireExtensions;

[AutomaticRetry(Attempts = 0)]
public interface ICronJob
{
    Task<string> Run();
}

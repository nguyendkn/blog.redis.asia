namespace Blog.Shared.Extensions.HangfireExtensions;

public class ScheduledTask
{
    public string Id { get; set; } = default!;

    public string Title { get; set; } = default!;

    public string Detail { get; set; } = default!;

    public string CronExpression { get; set; } = default!;

    public DateTime? LastExecution { get; set; }

    public DateTime? NextExecution { get; set; }

    public string Message { get; set; } = default!;
}

namespace Blog.Shared.Domain;

public class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTime PublishedAt { get; set; } = DateTime.Now;
}
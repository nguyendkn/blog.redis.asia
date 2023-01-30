namespace Blog.Shared.Domain;

public class Entity
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public bool Deleted { get; set; }
}
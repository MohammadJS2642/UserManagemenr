namespace UserManagement.Domain.Entities;

public abstract class AuditableEntity
{
    public DateTime CreatedAt { get; private set; }
    public DateTime ModfiedAt { get; private set; }

    protected AuditableEntity()
    {
        CreatedAt = ModfiedAt = DateTime.UtcNow;
    }

    public void Modify()
    {
        ModfiedAt = DateTime.UtcNow;
    }
}

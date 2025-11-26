namespace UserManagement.Domain.Security;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PermissionAttribute(string code, string description) : Attribute
{
    public string Code { get; } = code ?? throw new ArgumentNullException(nameof(code));
    public string Description { get; } = description ?? throw new ArgumentNullException(nameof(description));
}

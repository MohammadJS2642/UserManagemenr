namespace UserManagement.WebApi.Middleware;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PermissionAttribute(string code, string description) : Attribute/*, IFilterFactory*/
{
    public string Code { get; } = code ?? throw new ArgumentNullException(nameof(code));
    public string Description { get; } = description ?? throw new ArgumentNullException(nameof(description));

    //public bool IsReusable => false;

    //public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    //{
    //    return serviceProvider.GetRequiredService<PermissionFilter>();
    //}
}

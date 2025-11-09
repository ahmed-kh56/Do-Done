namespace DoDone.Application.Common.Authorization;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DynamicAuthorizeAttribute : Attribute
{
    public DynamicAuthorizeAttribute(Guid projectId, string role)
    {
        ProjectId = projectId;
        Role = role;
    }

    Guid ProjectId { get; set; }
    public string Role { get; set; }
    public override string ToString()
    => $"{Role??""}:{ProjectId}";
}

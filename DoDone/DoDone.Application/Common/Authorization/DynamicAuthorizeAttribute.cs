namespace DoDone.Application.Common.Authorization;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DynamicAuthorizeAttribute : Attribute
{
    Guid ProjectId { get; set; }
    public string Role { get; set; }
    public override string ToString()
    => $"{Role}:{ProjectId}";
}

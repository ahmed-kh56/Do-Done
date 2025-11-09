namespace DoDone.Application.Common.Authorization;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class AuthorizeAttribute : Attribute
{
    public string Role { get; set; }
}

namespace DoDone.Application.Common.Authorization;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class AuthorizationLogicAttribute : Attribute
{
    public AuthorizationMode Mode { get; }

    public AuthorizationLogicAttribute(AuthorizationMode mode)
    {
        Mode = mode;
    }
}
public enum AuthorizationMode
{
    And,
    Or
}




namespace DoDone.Authentication
{
    public record RegisterRequest(
        string FullName,
        string NameToShow,
        string Email,
        string Password);
 
}

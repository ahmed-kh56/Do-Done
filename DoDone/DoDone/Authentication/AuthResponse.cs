namespace DoDone.Authentication
{
    public record AuthResponse(
        Guid UserId,
        string FullName,
        string NameToShow,
        string Email,
        string Token);

}

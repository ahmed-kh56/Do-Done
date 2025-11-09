using ErrorOr;


namespace DoDone.Domain.Common
{
    public interface IPasswordHasher
    {

        public ErrorOr<string> HashPassword(string password);
        public bool IsCorrectPassword(string password, string hash);
        

    }
}

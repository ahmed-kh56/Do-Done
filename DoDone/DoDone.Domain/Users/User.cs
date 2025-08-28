using ErrorOr;
using DoDone.Domain.Common;
using System.Data;

namespace DoDone.Domain.Users;

public class User
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string ShowName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public bool IsVerified { get; set; } = false;

    public string _passwordHash = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public string? ProfilePhotoLink = null!;
    public bool Is2FAEnabled { get; set; } = false;
    public ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();


    private User() { }

    public User(string fullName,
        string nameToShow,
        string email,
        string passwordHash, 
        Guid? id = null, 
        bool isVerified = false, 
        string? profilePhotolink = null)
    {
        Id = id ?? Guid.NewGuid();
        FullName = fullName;
        ShowName = nameToShow;
        Email = email;
        _passwordHash = passwordHash;
        IsVerified = isVerified;
        ProfilePhotoLink = profilePhotolink;
    }

    public void ChangePhoto(string newprofilePhotolink)
    {
        ProfilePhotoLink = newprofilePhotolink;
    }

    public bool IsCorrectPasswordHash(string password, IPasswordHasher passwordHasher)
    {
        return passwordHasher.IsCorrectPassword(password, _passwordHash);
    }


    public void UpdateProfile(
        string? showName,
        string? fullName,
        string? photoPath,
        string? email)
    {
        if (showName is not null)
            ShowName = showName;

        if (fullName is not null)
            FullName = fullName;

        if (photoPath is not null)
            ProfilePhotoLink = photoPath;

        if (email is not null)
        {
            Email = email;
            IsVerified = false; 
        }
    }


    public ErrorOr<Success> UpdatePassword(string newPasswordHash, IPasswordHasher _passwordHasher)
    {
        var result = _passwordHasher.HashPassword(newPasswordHash);
        if (result.IsError)
        {
            return result.Errors;
        }
        _passwordHash = result.Value;
        return Result.Success;
    }

    public void ConfirmEmail()
    {
        IsVerified = true;
    }

    public void NullThePassword()
    {
        _passwordHash = "";
    }
}

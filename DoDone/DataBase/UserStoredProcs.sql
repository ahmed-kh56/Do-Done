CREATE OR ALTER PROCEDURE AddUser
    @Id UNIQUEIDENTIFIER,
    @FullName NVARCHAR(256),
    @ShowName NVARCHAR(256),
    @Email NVARCHAR(256),
    @IsVerified BIT,
    @_passwordHash NVARCHAR(MAX),
    @CreatedAt DATETIME2,
    @ProfilePhotoLink NVARCHAR(MAX) = NULL,
    @Is2FAEnabled BIT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Users (Id, FullName, ShowName, Email, IsVerified, _passwordHash, CreatedAt, ProfilePhotoLink, Is2FAEnabled)
    VALUES (@Id, @FullName, @ShowName, @Email, @IsVerified, @_passwordHash, @CreatedAt, @ProfilePhotoLink, @Is2FAEnabled);
END


CREATE OR ALTER PROCEDURE ExistsUserByEmail
    @Email NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    IF EXISTS (SELECT 1 FROM Users WHERE Email = @Email)
        SELECT CAST(1 AS BIT) AS ExistsResult;
    ELSE
        SELECT CAST(0 AS BIT) AS ExistsResult;
END



CREATE OR ALTER PROCEDURE GetUserByEmail
    @Email NVARCHAR(256)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 
        Id,
        FullName,
        ShowName,
        Email,
        IsVerified,
        _passwordHash,
        CreatedAt,
        ProfilePhotoLink,
        Is2FAEnabled
    FROM Users
    WHERE Email = @Email;
END

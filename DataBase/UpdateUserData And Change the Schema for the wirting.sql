USE [DoDoneTasksData]
GO

CREATE OR ALTER PROCEDURE [dbo].[UpdateUserData]
    @Id UNIQUEIDENTIFIER,
    @FullName NVARCHAR(256),
    @ShowName NVARCHAR(256),
    @Email NVARCHAR(256),
    @IsVerified BIT,
    @_passwordHash NVARCHAR(MAX),
    @ProfilePhotoLink NVARCHAR(MAX) = NULL,
    @Is2FAEnabled BIT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users 
    SET FullName =@FullName,
        ShowName=@ShowName,
        Email= @Email,
        IsVerified=@IsVerified,
        _passwordHash=@_passwordHash,
        ProfilePhotoLink=@ProfilePhotoLink,
        Is2FAEnabled=@Is2FAEnabled
    WHERE Id =@Id
END

GO

CREATE SCHEMA Writing
GO


ALTER SCHEMA Writing TRANSFER Dbo.AddUser;
ALTER SCHEMA Writing TRANSFER Dbo.AddUserToken;
ALTER SCHEMA Writing TRANSFER Dbo.UpdateUserDAta;
ALTER SCHEMA Writing TRANSFER Dbo.UpdateUserToken;

ALTER SCHEMA Reading TRANSFER Dbo.ExistsUserByEmail;

ALTER SCHEMA Reading TRANSFER Dbo.GetUserByEmail;
ALTER SCHEMA Reading TRANSFER Dbo.GetUserToken;


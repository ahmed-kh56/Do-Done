CREATE OR ALTER PROCEDURE UpdateUserToken
    @Id UNIQUEIDENTIFIER,
    @IsUsed BIT,
    @UsedAt DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE UserTokens
    SET IsUsed = @IsUsed,
        UsedAt = @UsedAt
    WHERE Id = @Id;
END


CREATE OR ALTER PROCEDURE GetUserToken
    @UserId UNIQUEIDENTIFIER,
    @Token NVARCHAR(256),
    @Type NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP 1 *
    FROM UserTokens
    WHERE UserId = @UserId
      AND Token = @Token
      AND Type = @Type
      AND IsUsed = 0;
END



CREATE OR ALTER PROCEDURE AddUserToken
    @Id UNIQUEIDENTIFIER = NULL,
    @UserId UNIQUEIDENTIFIER,
    @Type NVARCHAR(50),
    @Token NVARCHAR(256),
    @CreatedAt DATETIME2 = NULL,
    @IsUsed BIT = 0,
    @UsedAt DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO UserTokens (Id, UserId, Type, Token, CreatedAt, IsUsed, UsedAt)
    VALUES (
        ISNULL(@Id, NEWID()),
        @UserId,
        @Type,
        @Token,
        ISNULL(@CreatedAt, GETUTCDATE()),
        @IsUsed,
        @UsedAt
    );
END



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

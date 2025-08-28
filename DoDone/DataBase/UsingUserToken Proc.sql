CREATE OR ALTER PROCEDURE [Reading].[UsingUserToken]
    @UserId UNIQUEIDENTIFIER,
    @Token NVARCHAR(256),
    @Type NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Now DATETIME2 = GETUTCDATE();
    DECLARE @ExpireAt DATETIME2;
    DECLARE @Id UNIQUEIDENTIFIER;

    SELECT TOP 1 
        @Id = ut.Id,
        @ExpireAt = CASE ut.Type
                        WHEN 'email-confirmation' THEN DATEADD(HOUR, 24, ut.CreatedAt)
                        WHEN 'password-reset' THEN DATEADD(MINUTE, 15, ut.CreatedAt)
                        WHEN 'refresh-token' THEN DATEADD(DAY, 7, ut.CreatedAt)
                        WHEN '2fa' THEN DATEADD(MINUTE, 5, ut.CreatedAt)
                    END
    FROM Reading.NotUsedUserTokens AS ut
    WHERE ut.UserId = @UserId
        AND ut.Type = @Type
        AND ut.Token = @Token;

    IF @Id IS NULL
    BEGIN
        SELECT 404 AS StatusCode;
        RETURN;
    END

    IF @ExpireAt < @Now
    BEGIN
        UPDATE dbo.UserTokens
            SET IsUsed = 1
            WHERE Id = @Id;
        SELECT 0 AS StatusCode;
        RETURN;
    END

    UPDATE dbo.UserTokens
    SET IsUsed = 1,
        UsedAt = @Now
    WHERE Id = @Id;

    SELECT 200 AS StatusCode;
END

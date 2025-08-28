CREATE VIEW Reading.NotUsedUserTokens
WITH SCHEMABINDING
AS
SELECT 
    ut.Id,
    ut.UserId,
    ut.Type,
    ut.Token,
    ut.CreatedAt,
    ut.IsUsed
FROM dbo.UserTokens ut
WHERE ut.IsUsed = 0;

GO

CREATE UNIQUE CLUSTERED INDEX UIX_NotUsedUserTokens_Id
ON Reading.NotUsedUserTokens (Id);
GO


CREATE INDEX IX_NotUsedUserTokens_UserId
ON Reading.NotUsedUserTokens(UserId)
GO
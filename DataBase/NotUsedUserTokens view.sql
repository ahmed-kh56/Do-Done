CREATE OR ALTER VIEW [Reading.UserData].NotUsedUserTokens
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
ON [Reading.UserData].NotUsedUserTokens (Id);
GO


CREATE INDEX IX_NotUsedUserTokens_UserId
ON [Reading.UserData].NotUsedUserTokens(UserId)
GO
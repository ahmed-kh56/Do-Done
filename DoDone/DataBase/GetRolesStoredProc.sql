CREATE OR ALTER PROCEDURE GetActiveUserRoles
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM ProjectUserRoles
    WHERE UserId = @UserId
      AND LeftAt IS NULL;
END

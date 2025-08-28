
CREATE OR ALTER PROCEDURE Reading.GetUserProjects
    @UserId UNIQUEIDENTIFIER,
    @OnlyActive BIT = 0  
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        UR.UserId,
        UR.ProjectId,
        UR.UserRole,
        UR.StartedAt,
        UR.LeftAt,
        UR.IsLeft
    FROM Reading.UserRoles UR
    WHERE UR.UserId = @UserId
      AND (@OnlyActive = 0 OR UR.IsLeft = 0); 
END
GO
Reading.GetUserProjects 'ebc060e3-396e-4b94-9548-3989d803cc19'
CREATE OR ALTER PROCEDURE [Reading.ProjectData].[GetProjectUserRoles]
    @PurId UNIQUEIDENTIFIER = NULL,
    @ProjectId UNIQUEIDENTIFIER = NULL,
    @UserId UNIQUEIDENTIFIER = NULL,
    @OnlyActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        PUR.Id,
        PUR.UserId,
        PUR.ProjectId,
        PUR.RoleId,
        PUR.StartedAt,
        PUR.LeftAt
    FROM dbo.ProjectUserRoles PUR
    WHERE 
        (
            (@PurId IS NOT NULL AND PUR.Id = @PurId)

            OR (@PurId IS NULL AND @UserId IS NOT NULL AND @ProjectId IS NULL AND PUR.UserId = @UserId)

            OR (@PurId IS NULL AND @UserId IS NOT NULL AND @ProjectId IS NOT NULL 
                AND PUR.UserId = @UserId AND PUR.ProjectId = @ProjectId)

            OR (@PurId IS NULL AND @UserId IS NULL AND @ProjectId IS NOT NULL AND PUR.ProjectId = @ProjectId)
        )
        AND (
            @OnlyActive = 0
            OR (PUR.LeftAt IS NULL OR PUR.LeftAt > GETUTCDATE())
        );
END

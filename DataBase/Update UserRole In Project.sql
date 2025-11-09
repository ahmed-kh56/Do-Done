CREATE PROCEDURE [Writing.ProjectData].[UpdateUserRoleInProject]
    @PurId UNIQUEIDENTIFIER = NULL,
    @ProjectId UNIQUEIDENTIFIER = NULL,
    @UserId UNIQUEIDENTIFIER = NULL,
    @RoleId TINYINT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE PUR
    SET PUR.RoleId = @RoleId
    FROM ProjectUserRoles PUR
    WHERE
        (@PurId IS NOT NULL AND PUR.Id = @PurId)
        OR
        (@PurId IS NULL AND PUR.ProjectId = @ProjectId AND PUR.UserId = @UserId);
END

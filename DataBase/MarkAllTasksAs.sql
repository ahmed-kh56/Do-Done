CREATE OR ALTER PROCEDURE [Writing.FeatureTasks].[MarkAllTasksAs]
    @Status INT,
    @PurId UNIQUEIDENTIFIER = NULL,
    @ProjectId UNIQUEIDENTIFIER = NULL,
    @OnlyIfNotDone BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE t
    SET t.Status = @Status
    FROM dbo.TaskItem t
    JOIN dbo.Feature f ON t.FeatureId = f.Id
    JOIN dbo.ProjectUserRoles pur ON f.AssignedProjectUserRoleId = pur.Id
    WHERE
        (@PurId IS NULL OR pur.Id = @PurId)
        AND (@ProjectId IS NULL OR pur.ProjectId = @ProjectId)
        AND (@OnlyIfNotDone = 0 OR t.Status <> 'Done');
END

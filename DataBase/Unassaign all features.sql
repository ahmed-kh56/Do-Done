CREATE OR ALTER PROCEDURE [Writing.FeatureTasks].UnassaignAll
    @featureId UNIQUEIDENTIFIER = NULL,
    @pur UNIQUEIDENTIFIER = NULL,
    @projectId UNIQUEIDENTIFIER = NULL,
    @userId UNIQUEIDENTIFIER = NULL,
    @onlyNotCompleted BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE F
    SET AssignedProjectUserRoleId = F.ProjectId
    FROM Feature F
    LEFT JOIN ProjectUserRoles PUR ON F.AssignedProjectUserRoleId = PUR.Id
    WHERE
        -- 1. FeatureId
        (@featureId IS NOT NULL AND F.Id = @featureId)
        -- 2. purId
        OR (@featureId IS NULL AND @pur IS NOT NULL AND F.AssignedProjectUserRoleId = @pur)
        -- 3. projectId + userId
        OR (@featureId IS NULL AND @pur IS NULL 
            AND @projectId IS NOT NULL AND @userId IS NOT NULL
            AND PUR.ProjectId = @projectId
            AND PUR.UserId = @userId)
        -- 4. projectId
        OR (@featureId IS NULL AND @pur IS NULL 
            AND @projectId IS NOT NULL AND @userId IS NULL
            AND PUR.ProjectId = @projectId)
        -- 5. userId
        OR (@featureId IS NULL AND @pur IS NULL 
            AND @projectId IS NULL AND @userId IS NOT NULL 
            AND PUR.UserId = @userId)
    AND (@onlyNotCompleted = 0 OR F.IsCompleted = 0);

END
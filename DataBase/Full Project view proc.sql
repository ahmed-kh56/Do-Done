CREATE OR ALTER PROC [Reading.ProjectData].GetProjectDetailsById
    @ProjectId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- 1) Project Summary
    SELECT 
        ps.Id,
        ps.Name,
        ps.CreatedAt,
        ps.IsStarted,
        ps.StartDate,
        ps.EndDate,
        ps.EmployeeCount,
        ps.FeaturesCount,
        ps.TotalTasks
    FROM [Reading.ProjectData].ProjectSummary ps
    WHERE ps.Id = @ProjectId;

    -- 2) Employees in Project
    SELECT 
        pur.Id AS Id,
        pur.ProjectId,
        u.Id AS EmployeeId,
        u.Email AS EmployeeEmail,
        u.FullName AS EmployeeName,
        u.ProfilePhotoLink AS EmployeePhotoLink,
        u.ShowName AS EmployeeShowName,
        CASE WHEN pur.LeftAt IS NOT NULL THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsLeft,
        CASE WHEN r.NormalizedName = 'SCRUMMASTER' THEN CAST(1 AS BIT) ELSE CAST(0 AS BIT) END AS IsScrumMaster
    FROM dbo.ProjectUserRoles pur
    JOIN dbo.Users u ON u.Id = pur.UserId
    JOIN dbo.Projects p ON p.Id = pur.ProjectId
    JOIN dbo.Roles r ON r.Id = pur.RoleId
    WHERE pur.ProjectId = @ProjectId;

    -- 3) Features of Project
    SELECT 
        f.FeatureId,
        f.ProjectId,
        f.Title,
        f.Description,
        f.AssinedPURId,
        f.AssignedUserId,
        f.AssignedUserName,
        f.IsCompleted,
        f.TotalTasks
    FROM [Reading.FeatureTasks].FeatureSummaryView f
    WHERE f.ProjectId = @ProjectId;
END
GO

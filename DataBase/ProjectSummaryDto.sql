CREATE OR ALTER VIEW [Reading.ProjectData].ActiveProjectsScrumMasters
WITH SCHEMABINDING
AS
SELECT pur.ProjectId,
       p.Name,
       pur.UserId AS ScrumMasterId,
       u.FullName AS ScrumMasterName,
       u.ShowName AS ScrumMasrerShowName
FROM dbo.ProjectUserRoles pur
JOIN dbo.users u ON pur.UserId = u.Id
JOIN dbo.Projects p ON pur.ProjectId=p.Id 
JOIN dbo.Roles r ON r.Id=pur.RoleId
WHERE r.Name = 'ScrumMaster' and pur.LeftAt is null ;
GO

CREATE UNIQUE CLUSTERED INDEX IX_ActiveProjectsScrumMasters_ProjectId
ON [Reading.ProjectData].ActiveProjectsScrumMasters (ProjectId);
Go

CREATE OR ALTER VIEW [Reading.ProjectData].ProjectSummary
WITH SCHEMABINDING
AS
SELECT 
    p.Id,
    p.Name,
    p.CreatedAt,
    p.IsStarted,
    p.StartDate,
    p.IsCompleted,
    p.EndDate,
    COUNT_BIG(pur.UserId) AS EmployeeCount,
    COUNT_BIG(f.Id) AS FeaturesCount,
    COUNT_BIG(*) AS RowCount1,
    COUNT_BIG(ti.Id) AS TotalTasks
FROM dbo.projects p
JOIN dbo.ProjectUserRoles pur ON pur.ProjectId = p.Id
JOIN dbo.Feature f ON f.AssignedProjectUserRoleId = pur.Id
JOIN dbo.TaskItem ti ON ti.FeatureId = f.Id
GROUP BY 
    p.Id, p.Name, p.CreatedAt, p.IsStarted, p.StartDate, p.IsCompleted, p.EndDate;
GO

-- Clustered Index
CREATE UNIQUE CLUSTERED INDEX IX_ProjectSummary_ProjectId
ON [Reading.ProjectData].ProjectSummary (Id);

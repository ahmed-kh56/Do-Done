ALTER TABLE dbo.Feature 
ADD IsCompleted BIT NOT NULL;

-- 1. تعديل الـ View
CREATE OR ALTER VIEW [Reading.FeatureTasks].FeatureSummaryView
WITH SCHEMABINDING
AS
SELECT
    f.Id AS FeatureId,
    pur.ProjectId,                     -- ضفنا ProjectId
    f.Title,
    f.Description,
    pur.Id AS AssinedPURId,
    pur.UserId AS AssignedUserId,
    u.FullName AS AssignedUserName,
    f.IsCompleted,
    COUNT_BIG(t.Id) AS TotalTasks,
    COUNT_BIG(*) AS NeededCount
FROM dbo.Feature f
JOIN dbo.ProjectUserRoles pur
    ON f.AssignedProjectUserRoleId = pur.Id
JOIN dbo.Users u
    ON pur.UserId = u.Id
JOIN dbo.TaskItem t
    ON t.FeatureId = f.Id
GROUP BY
    f.Id,
    pur.ProjectId,                     -- ضفنا ProjectId هنا كمان
    f.Title,
    f.Description,
    pur.Id,
    pur.UserId,
    u.FullName,
    f.IsCompleted;
GO


ALTER SCHEMA [Reading.FeatureTasks] TRANSFER Reading.FeatureSummaryView;


-- 2. إنشاء Clustered Index على FeatureId (مطلوب للـ Indexed View)
CREATE UNIQUE CLUSTERED INDEX IX_FeatureSummaryView_FeatureId
ON [Reading.FeatureTasks].FeatureSummaryView(FeatureId);
GO

-- 3. إنشاء Nonclustered Index على ProjectId + AssignedUserId لتحسين الفلاتر
CREATE NONCLUSTERED INDEX IX_FeatureSummaryView_ProjectUser
ON [Reading.FeatureTasks].FeatureSummaryView(ProjectId, AssignedUserId);
GO
CREATE NONCLUSTERED INDEX IX_FeatureSummaryView_Project
ON [Reading.FeatureTasks].FeatureSummaryView(ProjectId);
GO
CREATE NONCLUSTERED INDEX IX_FeatureSummaryView_User
ON [Reading.FeatureTasks].FeatureSummaryView(AssignedUserId);
GO


CREATE OR ALTER PROC [Reading.FeatureTasks].GetFeatureSummary
    @ProjectId UNIQUEIDENTIFIER = NULL,
    @AssignedUserId UNIQUEIDENTIFIER = NULL,
    @FeatureId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM [Reading.FeatureTasks].FeatureSummaryView
    WHERE (@ProjectId IS NULL OR ProjectId = @ProjectId)
      AND (@AssignedUserId IS NULL OR AssignedUserId = @AssignedUserId)
      AND (@FeatureId IS NULL OR FeatureId = @FeatureId)
    ORDER BY FeatureId;  -- ممكن تعدل الـ ORDER حسب الحاجة
END


ALTER SCHEMA [Reading.FeatureTasks] TRANSFER Reading.GetFeatureSummary;

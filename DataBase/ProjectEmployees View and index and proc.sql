CREATE OR ALTER VIEW [Reading.ProjectData].ProjectsEmployees
WITH SCHEMABINDING
AS
SELECT 
    PUR.Id AS Id,
    PUR.Id AS ProjectId,
    U.Id AS EmployeeId,
    U.Email AS EmployeeEmail,
    U.FullName AS EmployeeName,
    U.ProfilePhotoLink AS EmployeePhotoLink,
    U.ShowName AS EmployeeShowName,
    CAST (CASE WHEN R.Name = 'ScrumMaster' THEN 1 ELSE 0 END AS bit ) AS IsScrumMaster,
    CAST (CASE WHEN PUR.LeftAt IS NULL THEN 0 ELSE 1 END AS bit ) AS ISLEFT
FROM dbo.ProjectUserRoles PUR
JOIN dbo.Users U ON U.Id = PUR.UserId
JOIN dbo.Roles R ON R.Id = PUR.RoleId;
GO

CREATE UNIQUE CLUSTERED INDEX UIX_ProjectsEmployees
ON [Reading.ProjectData].ProjectsEmployees(Id);

CREATE INDEX IX_ProjectsEmployees
ON [Reading.ProjectData].ProjectsEmployees(ProjectId);


CREATE OR ALTER PROC [Reading.ProjectData].GetProjectEmployeesByProjectId
@ProjectId UNIQUEIDENTIFIER,
@Historical BIT
AS
BEGIN 
SELECT *
FROM [Reading.ProjectData].ProjectsEmployees
Where ProjectId =@ProjectId AND (@Historical = 1 OR IsLeft = 0 );
END


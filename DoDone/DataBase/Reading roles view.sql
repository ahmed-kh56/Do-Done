USE DoDoneTasksData
GO

CREATE SCHEMA Reading
GO


CREATE OR ALTER VIEW Reading.UserRoles
WITH SCHEMABINDING  
AS 
SELECT 
PRU.Id AS Id,
PRU.LeftAt AS LeftAt,
PRU.StartedAt AS StartedAt,
U.Id AS UserId,
U.Email AS UserEmail,
R.Id AS RoleId,
R.Name AS UserRole,
P.Id AS ProjectId,
p.Name AS ProjectName,
CASE WHEN LeftAt IS NULL THEN 0 ELSE 1 END AS IsLeft
FROM dbo.ProjectUserRoles PRU 
JOIN dbo.Users U ON U.Id=PRU.UserId 
JOIN dbo.Roles R ON R.Id =PRU.RoleId 
JOIN dbo.Projects P ON P.Id =PRU.ProjectId
GO
SELECT * FROM Reading.UserRoles
GO
CREATE UNIQUE CLUSTERED INDEX UIX_RolesByProject
ON Reading.UserRoles(Id)
GO

CREATE Index IX_UserId_IsLeft_RolesByProject on Reading.UserRoles(UserId, IsLeft)
INCLUDE (UserRole,ProjectId,StartedAt,LeftAt)


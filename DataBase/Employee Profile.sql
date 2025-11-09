USE DoDoneTasksData
GO





CREATE OR ALTER VIEW [Reading.UserData].EmployeeProfilesView
AS
SELECT
    u.Id AS EmployeeId,
    u.FullName,
    u.ShowName,
    u.Email,
    u.ProfilePhotoLink,
    u.CreatedAt,
    COUNT(pur.ProjectId) AS ProjectCount,
    uh.RoleName AS HighestRoleName
FROM dbo.Users u
LEFT JOIN dbo.ProjectUserRoles pur
    ON u.Id = pur.UserId
LEFT JOIN [Reading.UserData].UserHighestRole uh
    ON u.Id = uh.UserId
GROUP BY
    u.Id,
    u.FullName,
    u.ShowName,
    u.Email,
    u.ProfilePhotoLink,
    u.CreatedAt,
    uh.RoleName;
GO
-- بقت عادية مفيش اندكسات دلوقت
/*
CREATE UNIQUE CLUSTERED INDEX UIX_EmployeeProfilesView
ON Reading.EmployeeProfilesView(EmployeeId);

CREATE NONCLUSTERED INDEX IX_EmployeeProfilesView_SearchName
ON Reading.EmployeeProfilesView(ShowName);
*/



CREATE OR ALTER PROC [Reading.UserData].GetEmployeeProfile
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM [Reading.UserData].EmployeeProfilesView
    WHERE EmployeeId = @UserId;
END


USE DoDoneTasksData
GO


CREATE OR ALTER PROC [Reading.UserData].GetEmployees
    @BageNum INT = 0,
    @BageSize INT = 12
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT
        *
    FROM
        [Reading.UserData].EmployeeProfilesView
    ORDER BY
        FullName ASC
    OFFSET @BageNum * @BageSize ROWS
    FETCH NEXT @BageSize ROWS ONLY;
END
GO
CREATE OR ALTER TRIGGER TR_ProjectUserRole_Insert
ON dbo.ProjectUserRoles
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Reading.UserData].AllUsersRules (UserId, ProjectId, FeatureId, RoleName, StartedAt, LeftAt)
    SELECT 
        i.UserId,
        i.ProjectId,
        NULL AS FeatureId,
        r.Name AS RoleName,
        i.StartedAt,
        NULL AS LeftAt
    FROM inserted i
    JOIN Roles r ON i.RoleId = r.Id;
END


GO





CREATE OR ALTER TRIGGER TR_ProjectUserRole_Update
ON dbo.ProjectUserRoles
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId UNIQUEIDENTIFIER;
    DECLARE @ProjectId UNIQUEIDENTIFIER;
    DECLARE @RoleName NVARCHAR(100);

    -- نفترض row واحدة فقط تتغير في نفس الوقت
    SELECT TOP 1
        @UserId = i.UserId,
        @ProjectId = i.ProjectId,
        @RoleName = r.Name
    FROM inserted i
    JOIN Roles r ON i.RoleId = r.Id
    WHERE i.LeftAt IS NOT NULL; -- بس اللي انتهت دلوقتي

    -- نحدث AllUsersRules
    UPDATE ru
    SET ru.LeftAt = GETDATE()
    FROM [Reading.UserData].AllUsersRules ru
    WHERE ru.UserId = @UserId
      AND ru.ProjectId = @ProjectId
      AND ru.FeatureId IS NULL
      AND ru.RoleName = @RoleName
      AND ru.LeftAt IS NULL; -- بس الرولات النشطة
END
GO

ALTER TABLE dbo.ProjectUserRoles
ALTER COLUMN RoleId TINYINT NOT NULL;
GO
ALTER TABLE dbo.ProjectUserRoles
ALTER COLUMN ProjectId UNIQUEIDENTIFIER NOT NULL;
GO

CREATE OR ALTER TRIGGER TR_ProjectUserRole_Update
ON dbo.ProjectUserRoles
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- نحدث AllUsersRules لكل صف اتغير
    UPDATE ru
    SET ru.LeftAt = GETDATE()
    FROM [Reading.UserData].AllUsersRules ru
    JOIN (
        SELECT i.UserId, i.ProjectId, r.Name AS RoleName
        FROM inserted i
        JOIN Roles r ON i.RoleId = r.Id
        WHERE i.LeftAt IS NOT NULL -- بس اللي انتهت دلوقتي
    ) x
      ON ru.UserId = x.UserId
     AND ru.ProjectId = x.ProjectId
     AND ru.RoleName = x.RoleName
    WHERE ru.FeatureId IS NULL
      AND ru.LeftAt IS NULL; -- بس الرولات النشطة
END
GO



CREATE OR ALTER TRIGGER TR_ProjectUserRole_Insert
ON dbo.ProjectUserRoles
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Reading.UserData].AllUserRoles (UserId, ProjectId, FeatureId, RoleId, StartedAt, LeftAt)
    SELECT 
        i.UserId,
        i.ProjectId,
        NULL AS FeatureId,
        i.RoleId,
        i.StartedAt,
        NULL AS LeftAt
    FROM inserted i;
END
GO


CREATE OR ALTER TRIGGER TR_ProjectUserRole_Update
ON dbo.ProjectUserRoles
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- نحدث AllUserRules لكل صف اتغير LeftAt بتاعه
    UPDATE ru
    SET ru.LeftAt = i.LeftAt
    FROM [Reading.UserData].AllUserRoles ru
    JOIN inserted i
      ON ru.UserId = i.UserId
     AND ru.ProjectId = i.ProjectId
     AND ru.RoleId = i.RoleId
    WHERE ru.FeatureId IS NULL
      AND ru.LeftAt IS NULL
      AND i.LeftAt IS NOT NULL; -- بس اللي اتعمله end دلوقتي
END
GO


ALTER TABLE [Reading.UserData].AllUsersRoles
    ADD RoleId TINYINT NOT NULL;
ALTER TABLE [Reading.UserData].UserHighestRole
    ADD RoleId TINYINT NOT NULL;
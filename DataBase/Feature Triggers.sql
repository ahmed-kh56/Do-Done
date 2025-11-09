CREATE OR ALTER TRIGGER TR_Feature_Insert
ON dbo.Feature
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Reading.UserData].AllUsersRules (UserId, ProjectId, FeatureId, RoleName, StartedAt, LeftAt)
    SELECT 
        pur.UserId,
        f.ProjectId,
        f.Id AS FeatureId,
        r.Name AS RoleName,
        GETDATE() AS StartedAt,
        NULL AS LeftAt
    FROM inserted f
    JOIN dbo.ProjectUserRoles pur ON f.AssignedProjectUserRoleId = pur.Id
    JOIN Roles r ON pur.RoleId = r.Id;
END
GO



CREATE TRIGGER TR_Feature_Update
ON dbo.Feature
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @FeatureId UNIQUEIDENTIFIER;
    DECLARE @OldUserId UNIQUEIDENTIFIER;
    DECLARE @NewUserId UNIQUEIDENTIFIER;
    DECLARE @RoleName NVARCHAR(100);
    DECLARE @ProjectId UNIQUEIDENTIFIER;

    -- نفترض row واحدة تتغير في نفس الوقت
    SELECT TOP 1
        f.Id AS FeatureId,
        oldpur.UserId AS OldUserId,
        newpur.UserId AS NewUserId,
        f.ProjectId AS ProjectId,
        r.Name AS RoleName
    FROM inserted f
    JOIN deleted d ON f.Id = d.Id
    JOIN ProjectUserRoles oldpur ON d.AssignedProjectUserRoleId = oldpur.Id
    JOIN ProjectUserRoles newpur ON f.AssignedProjectUserRoleId = newpur.Id
    JOIN Roles r ON newpur.RoleId = r.Id
    WHERE f.AssignedProjectUserRoleId <> d.AssignedProjectUserRoleId;

    -- 1️⃣ Close old role
    UPDATE ru
    SET LeftAt = GETDATE()
    FROM Reading.AllUsersRules ru
    WHERE ru.UserId = @OldUserId
      AND ru.ProjectId = @ProjectId
      AND ru.FeatureId = @FeatureId
      AND ru.RoleName = @RoleName
      AND ru.LeftAt IS NULL;

    -- 2️⃣ Insert new role
    INSERT INTO Reading.AllUsersRules (UserId, ProjectId, FeatureId, RoleName, StartedAt, LeftAt)
    VALUES (@NewUserId, @ProjectId, @FeatureId, @RoleName, GETDATE(), NULL);
END





CREATE OR ALTER TRIGGER TR_Feature_Update
ON dbo.Feature
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @FeatureId UNIQUEIDENTIFIER;
    DECLARE @OldUserId UNIQUEIDENTIFIER;
    DECLARE @NewUserId UNIQUEIDENTIFIER;
    DECLARE @RoleName NVARCHAR(100);
    DECLARE @ProjectId UNIQUEIDENTIFIER;

    -- نفترض row واحدة تتغير في نفس الوقت
    SELECT TOP 1
        f.Id AS FeatureId,
        oldpur.UserId AS OldUserId,
        newpur.UserId AS NewUserId,
        f.ProjectId AS ProjectId,
        r.Name AS RoleName
    FROM inserted f
    JOIN deleted d ON f.Id = d.Id
    JOIN ProjectUserRoles oldpur ON d.AssignedProjectUserRoleId = oldpur.Id
    JOIN ProjectUserRoles newpur ON f.AssignedProjectUserRoleId = newpur.Id
    JOIN Roles r ON newpur.RoleId = r.Id
    WHERE f.AssignedProjectUserRoleId <> d.AssignedProjectUserRoleId;

    -- 1️⃣ Close old role
    UPDATE ru
    SET LeftAt = GETDATE()
    FROM [Reading.UserData].AllUsersRules ru
    WHERE ru.UserId = @OldUserId
      AND ru.ProjectId = @ProjectId
      AND ru.FeatureId = @FeatureId
      AND ru.RoleName = @RoleName
      AND ru.LeftAt IS NULL;

    -- 2️⃣ Insert new role
    INSERT INTO [Reading.UserData].AllUsersRules (UserId, ProjectId, FeatureId, RoleName, StartedAt, LeftAt)
    VALUES (@NewUserId, @ProjectId, @FeatureId, @RoleName, GETDATE(), NULL);
END






----------------------
----------------------



CREATE OR ALTER TRIGGER TR_Feature_Update
ON dbo.Feature
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Changes AS (
        SELECT 
            f.Id AS FeatureId,
            oldpur.UserId AS OldUserId,
            newpur.UserId AS NewUserId,
            f.ProjectId AS ProjectId,
            rold.Name AS OldRoleName,
            rnew.Name AS NewRoleName
        FROM inserted f
        JOIN deleted d 
            ON f.Id = d.Id
        JOIN ProjectUserRoles oldpur 
            ON d.AssignedProjectUserRoleId = oldpur.Id
        JOIN ProjectUserRoles newpur 
            ON f.AssignedProjectUserRoleId = newpur.Id
        JOIN Roles rold 
            ON oldpur.RoleId = rold.Id
        JOIN Roles rnew 
            ON newpur.RoleId = rnew.Id
        WHERE f.AssignedProjectUserRoleId <> d.AssignedProjectUserRoleId
    )
    -- 1️⃣ قفل الرول القديم
    UPDATE ru
    SET LeftAt = GETDATE()
    FROM [Reading.UserData].AllUsersRoles ru
    JOIN Changes c
      ON ru.UserId   = c.OldUserId
     AND ru.ProjectId = c.ProjectId
     AND ru.FeatureId = c.FeatureId
     AND ru.RoleName  = c.OldRoleName
     AND ru.LeftAt IS NULL;

    -- 2️⃣ إدخال الرول الجديد
    INSERT INTO [Reading.UserData].AllUsersRoles
           (UserId, ProjectId, FeatureId, RoleName, StartedAt, LeftAt)
    SELECT 
        c.NewUserId,
        c.ProjectId,
        c.FeatureId,
        c.NewRoleName,
        GETDATE(),
        NULL
    FROM Changes c;
END




CREATE OR ALTER TRIGGER TR_Feature_Insert
ON dbo.Feature
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Reading.UserData].AllUsersRoles (UserId, ProjectId, FeatureId, RoleName, StartedAt, LeftAt)
    SELECT 
        pur.UserId,
        f.ProjectId,
        f.Id AS FeatureId,
        r.Name AS RoleName,
        GETDATE() AS StartedAt,
        NULL AS LeftAt
    FROM inserted f
    JOIN dbo.ProjectUserRoles pur 
        ON f.AssignedProjectUserRoleId = pur.Id
    JOIN Roles r 
        ON pur.RoleId = r.Id
    WHERE f.AssignedProjectUserRoleId IS NOT NULL; -- 👈 عشان نتجنب مشاكل لو الـ Feature مالوش AssignedRole
END
GO

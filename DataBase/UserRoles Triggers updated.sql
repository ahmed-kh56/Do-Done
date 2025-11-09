CREATE TRIGGER TR_UserRole_Insert
ON UserRoles
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Reading.AllUsersRules(UserId, ProjectId, FeatureId, RoleName, StartedAt, LeftAt)
    SELECT 
        i.UserId,
        NULL AS ProjectId,
        NULL AS FeatureId,
        r.Name,
        GETDATE() AS StartedAt,
        NULL AS LeftAt
    FROM inserted i
    JOIN Roles r ON i.RoleId = r.Id;
END


GO





CREATE OR ALTER TRIGGER TR_UserRole_Update
ON UserRoles
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId UNIQUEIDENTIFIER;
    DECLARE @RoleName NVARCHAR(100);

    -- نجيب الـ UserId و RoleName للـ row اللي اتعمل لها تحديث
    SELECT TOP 1 
        @UserId = i.UserId,
        @RoleName = r.Name
    FROM inserted i
    JOIN Roles r ON i.RoleId = r.Id
    WHERE i.LeftAt IS NOT NULL; -- الرول اللي انتهت دلوقتي

    -- نحدث الـ Rules
    UPDATE Reading.AllUsersRules
    SET LeftAt = GETDATE()
    WHERE UserId = @UserId
      AND RoleName = @RoleName
      AND ProjectId IS NULL
      AND FeatureId IS NULL
      AND LeftAt IS NULL; -- بس الرولات النشطة
END



ALTER TRIGGER [dbo].[TR_UserRole_Insert]
ON [dbo].[UserRoles]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- اللوج (History)
    INSERT INTO [Reading.UserData].AllUsersRules(UserId, ProjectId, FeatureId, RoleName, StartedAt, LeftAt)
    SELECT 
        i.UserId,
        NULL AS ProjectId,
        NULL AS FeatureId,
        r.Name,
        GETDATE() AS StartedAt,
        NULL AS LeftAt
    FROM inserted i
    JOIN Roles r ON i.RoleId = r.Id;

    -- تحديث UserHighestRole (أعلى رول)
    DECLARE @UserId UNIQUEIDENTIFIER;

    SELECT TOP 1 @UserId = i.UserId
    FROM inserted i;

    DECLARE @HighestRoleName NVARCHAR(100);

    SELECT TOP 1 @HighestRoleName = r.Name
    FROM UserRoles ur
    JOIN Roles r ON ur.RoleId = r.Id
    WHERE ur.UserId = @UserId AND ur.LeftAt IS NULL
    ORDER BY r.Id DESC;

    IF EXISTS (SELECT 1 FROM [Reading.UserData].UserHighestRole WHERE UserId = @UserId)
    BEGIN
        UPDATE [Reading.UserData].UserHighestRole
        SET RoleName = @HighestRoleName
        WHERE UserId = @UserId;
    END
    ELSE
    BEGIN
        INSERT INTO [Reading.UserData].UserHighestRole (UserId, RoleName)
        VALUES (@UserId, @HighestRoleName);
    END
END





CREATE OR ALTER TRIGGER TR_UserRole_Update
ON UserRoles
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId UNIQUEIDENTIFIER;

    -- نفترض إن التحديث هو LeftAt فقط (soft delete)
    SELECT TOP 1 
        @UserId = i.UserId
    FROM inserted i
    WHERE i.LeftAt IS NOT NULL;

    IF @UserId IS NULL
        RETURN;

    -- نقفل الرول في الجدول الموحد
    UPDATE aur
    SET LeftAt = GETDATE()
    FROM [Reading.UserData].AllUsersRules aur
    JOIN inserted i 
        ON aur.UserId = i.UserId
    JOIN Roles r 
        ON i.RoleId = r.Id
       AND aur.RoleName = r.Name
    WHERE aur.ProjectId IS NULL
      AND aur.FeatureId IS NULL
      AND aur.LeftAt IS NULL
      AND i.LeftAt IS NOT NULL;

    -- نحسب أعلى رول متبقي لليوزر
    DECLARE @HighestRoleName NVARCHAR(100);

    SELECT TOP 1 @HighestRoleName = r.Name
    FROM UserRoles ur
    JOIN Roles r ON ur.RoleId = r.Id
    WHERE ur.UserId = @UserId 
      AND ur.LeftAt IS NULL
    ORDER BY r.Id DESC;

    -- نعمل UPSERT يدوي
    IF EXISTS (SELECT 1 FROM [Reading.UserData].UserHighestRole WHERE UserId = @UserId)
    BEGIN
        UPDATE [Reading.UserData].UserHighestRole
        SET RoleName = @HighestRoleName
        WHERE UserId = @UserId;
    END
    ELSE
    BEGIN
        INSERT INTO [Reading.UserData].UserHighestRole (UserId, RoleName)
        VALUES (@UserId, @HighestRoleName);
    END
END



-------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
--------------------------------------------------------------------------------------------------------------------------
------------------------------


-- v3.0


CREATE OR ALTER TRIGGER [dbo].[TR_UserRole_Insert]
ON [dbo].[UserRoles]
AFTER INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- اللوج (History)
    INSERT INTO [Reading.UserData].AllUsersRoles
        (UserId, ProjectId, FeatureId,RoleId , RoleName, StartedAt, LeftAt)
    SELECT 
        i.UserId,
        NULL AS ProjectId,
        NULL AS FeatureId,
        r.Id   AS RoleId,     -- إضافة RoleId
        r.Name AS RoleName,
        GETDATE() AS StartedAt,
        NULL AS LeftAt
    FROM inserted i
    JOIN Roles r ON i.RoleId = r.Id;

    -- تحديث UserHighestRole (أعلى رول)
    DECLARE @UserId UNIQUEIDENTIFIER;

    SELECT TOP 1 @UserId = i.UserId
    FROM inserted i;

    DECLARE @HighestRoleId TINYINT;
    DECLARE @HighestRoleName NVARCHAR(100);

    SELECT TOP 1 
        @HighestRoleId = r.Id,
        @HighestRoleName = r.Name
    FROM UserRoles ur
    JOIN Roles r ON ur.RoleId = r.Id
    WHERE ur.UserId = @UserId AND ur.LeftAt IS NULL
    ORDER BY r.Id DESC;

    IF EXISTS (SELECT 1 FROM [Reading.UserData].UserHighestRole WHERE UserId = @UserId)
    BEGIN
        UPDATE [Reading.UserData].UserHighestRole
        SET RoleId = @HighestRoleId,   -- تحديث بالـ RoleId
            RoleName = @HighestRoleName
        WHERE UserId = @UserId;
    END
    ELSE
    BEGIN
        INSERT INTO [Reading.UserData].UserHighestRole (UserId, RoleId, RoleName)
        VALUES (@UserId, @HighestRoleId, @HighestRoleName);
    END
END






-------------------------------
------------------------------
CREATE OR ALTER TRIGGER [dbo].[TR_UserRole_Update]
ON [dbo].[UserRoles]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId UNIQUEIDENTIFIER;

    -- نفترض إن التحديث هو LeftAt فقط (soft delete)
    SELECT TOP 1 @UserId = i.UserId
    FROM inserted i
    WHERE i.LeftAt IS NOT NULL;

    IF @UserId IS NULL
        RETURN;

    -- نقفل الرول في الجدول الموحد
    UPDATE aur
    SET LeftAt = GETDATE()
    FROM [Reading.UserData].AllUsersRoles aur
    JOIN inserted i ON aur.UserId = i.UserId
    JOIN Roles r ON i.RoleId = r.Id
    WHERE aur.ProjectId IS NULL
      AND aur.FeatureId IS NULL
      AND aur.LeftAt IS NULL
      AND aur.RoleId = r.Id  -- مطابق RoleId بدل RoleName
      AND i.LeftAt IS NOT NULL;

    -- نحسب أعلى رول متبقي لليوزر
    DECLARE @HighestRoleId TINYINT;
    DECLARE @HighestRoleName NVARCHAR(100);

    SELECT TOP 1 
        @HighestRoleId = r.Id,
        @HighestRoleName = r.Name
    FROM UserRoles ur
    JOIN Roles r ON ur.RoleId = r.Id
    WHERE ur.UserId = @UserId 
      AND ur.LeftAt IS NULL
    ORDER BY r.Id DESC;

    -- نعمل UPSERT يدوي
    IF EXISTS (SELECT 1 FROM [Reading.UserData].UserHighestRole WHERE UserId = @UserId)
    BEGIN
        UPDATE [Reading.UserData].UserHighestRole
        SET RoleId = @HighestRoleId,
            RoleName = @HighestRoleName
        WHERE UserId = @UserId;
    END
    ELSE
    BEGIN
        INSERT INTO [Reading.UserData].UserHighestRole (UserId, RoleId, RoleName)
        VALUES (@UserId, @HighestRoleId, @HighestRoleName);
    END
END

CREATE PROCEDURE [Writing.ProjectData].[MarkProjectEmployeesAsEnded]
    @ProjectId UNIQUEIDENTIFIER = NULL,
    @UserId UNIQUEIDENTIFIER = NULL,
    @PurId UNIQUEIDENTIFIER = NULL,  -- Primary Key للجدول ProjectUserRoles
    @LeftAt DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- استخدم وقت السيرفر لو LeftAt مش مبعوت
    IF @LeftAt IS NULL
        SET @LeftAt = SYSUTCDATETIME();

    UPDATE ProjectUserRoles
    SET LeftAt = @LeftAt
    WHERE 
        (
            -- 3) لو مبعوت PurId → استعمله مباشرة
            (@PurId IS NOT NULL AND Id = @PurId)
        )
        OR
        (
            -- 2) لو مبعوت ProjectId + UserId → استعملهم
            (@PurId IS NULL AND @ProjectId IS NOT NULL AND @UserId IS NOT NULL 
             AND ProjectId = @ProjectId AND UserId = @UserId)
        )
        OR
        (
            -- 1) لو مبعوت ProjectId بس → استعمله
            (@PurId IS NULL AND @UserId IS NULL AND @ProjectId IS NOT NULL 
             AND ProjectId = @ProjectId)
        )
        AND LeftAt IS NULL; -- عشان ما يكتبش فوق اللي خلص بالفعل
END

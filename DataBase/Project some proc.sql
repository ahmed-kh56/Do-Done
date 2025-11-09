CREATE PROCEDURE [Reading.ProjectData].[GetProjectById]
    @ProjectId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        Name,
        CreatedAt,
        IsStarted,
        StartDate,
        IsCompleted,
        EndDate
    FROM Projects
    WHERE Id = @ProjectId;
END



CREATE PROCEDURE [Writing.ProjectData].[AddProject]
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(200),
    @CreatedAt DATETIME2,
    @IsStarted BIT,
    @StartDate DATETIME2 = NULL,
    @IsCompleted BIT,
    @EndDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Projects (
        Id,
        Name,
        CreatedAt,
        IsStarted,
        StartDate,
        IsCompleted,
        EndDate
    )
    VALUES (
        @Id,
        @Name,
        @CreatedAt,
        @IsStarted,
        @StartDate,
        @IsCompleted,
        @EndDate
    );
END




CREATE PROCEDURE [Writing.ProjectData].[UpdateProject]
    @Id UNIQUEIDENTIFIER,
    @Name NVARCHAR(200),
    @IsStarted BIT,
    @StartDate DATETIME2 = NULL,
    @IsCompleted BIT,
    @EndDate DATETIME2 = NULL
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Projects
    SET 
        Name = @Name,
        IsStarted = @IsStarted,
        StartDate = @StartDate,
        IsCompleted = @IsCompleted,
        EndDate = @EndDate
    WHERE Id = @Id;
END

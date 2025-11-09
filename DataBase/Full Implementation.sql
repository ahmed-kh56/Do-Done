CREATE DATABASE DoDoneTasksData;
GO

USE DoDoneTasksData;
GO

-- جدول Roles
CREATE TABLE Roles (
    Id TINYINT NOT NULL PRIMARY KEY,
    Name NVARCHAR(256) NOT NULL,
    NormalizedName NVARCHAR(256) NOT NULL
);
CREATE INDEX IX_Roles_NormalizedName ON Roles(NormalizedName);
GO

-- جدول Users
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    FullName NVARCHAR(256) NOT NULL,
    ShowName NVARCHAR(256) NOT NULL,
    Email NVARCHAR(256) NOT NULL,
    IsVerified BIT NOT NULL DEFAULT(0),
    _passwordHash NVARCHAR(MAX) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    ProfilePhotoLink NVARCHAR(MAX) NULL,
    Is2FAEnabled BIT NOT NULL DEFAULT(0)
);
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_FullName ON Users(FullName);
GO

-- جدول Projects (بدون ScrumMasterId)
CREATE TABLE Projects (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Name NVARCHAR(256) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    IsStarted BIT NOT NULL,
    StartDate DATETIME2 NULL,
    IsCompleted BIT NOT NULL,
    EndDate DATETIME2 NULL,
);
GO


-- جدول UserTokens
CREATE TABLE UserTokens (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Token NVARCHAR(256) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    IsUsed BIT NOT NULL DEFAULT(0),
    UsedAt DATETIME2 NULL
);
CREATE INDEX IX_UserTokens_UserId ON UserTokens(UserId);
CREATE INDEX IX_UserTokens_Type ON UserTokens(Type);
ALTER TABLE UserTokens
ADD CONSTRAINT FK_UserTokens_Users FOREIGN KEY (UserId) REFERENCES Users(Id);
GO

-- جدول ProjectUserRoles (دمج ProjectEmployee + UserRoles)
CREATE TABLE ProjectUserRoles (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    ProjectId UNIQUEIDENTIFIER,
    RoleId TINYINT,
    StartedAt DATETIME2 NOT NULL,
    LeftAt DATETIME2 NULL
);
CREATE INDEX IX_ProjectUserRoles_UserId ON ProjectUserRoles(UserId);
CREATE INDEX IX_ProjectUserRoles_ProjectId ON ProjectUserRoles(ProjectId);
CREATE INDEX IX_ProjectUserRoles_RoleId ON ProjectUserRoles(RoleId);
ALTER TABLE ProjectUserRoles
ADD CONSTRAINT FK_ProjectUserRoles_Users FOREIGN KEY (UserId) REFERENCES Users(Id);
ALTER TABLE ProjectUserRoles
ADD CONSTRAINT FK_ProjectUserRoles_Projects FOREIGN KEY (ProjectId) REFERENCES Projects(Id);
ALTER TABLE ProjectUserRoles
ADD CONSTRAINT FK_ProjectUserRoles_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id);
GO

-- جدول Feature
CREATE TABLE Feature (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Title NVARCHAR(256) NOT NULL,
    Description NVARCHAR(MAX) ,
    ProjectId UNIQUEIDENTIFIER NOT NULL,
    AssignedProjectUserRoleId UNIQUEIDENTIFIER NOT NULL
);
CREATE INDEX IX_Feature_AssignedProjectUserRoleId ON Feature(AssignedProjectUserRoleId);

ALTER TABLE Feature
ADD CONSTRAINT FK_Feature_ProjectUserRoles FOREIGN KEY (AssignedProjectUserRoleId) REFERENCES ProjectUserRoles(Id);
GO

-- جدول TaskItem
CREATE TABLE TaskItem (
    Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    Title NVARCHAR(256) NOT NULL,
    Description NVARCHAR(MAX) ,
    Status NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT(GETUTCDATE()),
    FeatureId UNIQUEIDENTIFIER NOT NULL
);
CREATE INDEX IX_TaskItem_FeatureId ON TaskItem(FeatureId);
CREATE INDEX IX_TaskItem_Status ON TaskItem(Status);
ALTER TABLE TaskItem
ADD CONSTRAINT FK_TaskItem_Feature FOREIGN KEY (FeatureId) REFERENCES Feature(Id);
GO

CREATE TABLE dbo.UserRoles
(
    [Id] INT PRIMARY KEY IDENTITY,
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [RoleId] TINYINT NOT NULL,
    [StartDate] DATETIME2 NOT NULL,
    [EndDate] DATETIME2 NULL,
    CONSTRAINT FK_UserRoles_User FOREIGN KEY ([UserId]) REFERENCES dbo.[Users](Id),
    CONSTRAINT FK_UserRoles_Role FOREIGN KEY ([RoleId]) REFERENCES dbo.Roles(Id)
);

-- Non-Clustered Index على UserId
CREATE NONCLUSTERED INDEX IX_UserRoles_UserId
ON dbo.UserRoles([UserId]);

-- Non-Clustered Index على RoleId
CREATE NONCLUSTERED INDEX IX_UserRoles_RoleId
ON dbo.UserRoles([RoleId]);

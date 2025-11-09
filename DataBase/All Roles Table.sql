CREATE TABLE Reading.AllUsersRules
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    UserId UNIQUEIDENTIFIER NOT NULL,
    ProjectId UNIQUEIDENTIFIER NULL,       
    FeatureId UNIQUEIDENTIFIER NULL,       
    RoleName NVARCHAR(100) NOT NULL,
    StartedAt DATETIME2 NOT NULL DEFAULT GETDATE(),
    LeftAt DATETIME2 NULL,
 );

CREATE NONCLUSTERED INDEX IX_Rules_User
ON Reading.AllUsersRules(UserId)
INCLUDE (ProjectId, FeatureId, RoleName, StartedAt, LeftAt);

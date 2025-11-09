CREATE TABLE [Writing.UserData].EmailOutbox
(
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    EmailType NVARCHAR(50) NOT NULL,    
    Token NVARCHAR(255) NULL,           
    IsSent BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
    SentAt DATETIME2 NULL
);
ALTER TABLE [Writing.UserData].EmailOutbox
ADD ErrorMessage NVARCHAR(500) NULL;

ALTER TABLE [Writing.UserData].EmailOutbox
ADD RetryCount INT NOT NULL DEFAULT 0;


ALTER TABLE [Writing.UserData].EmailOutbox
ADD UserName NVARCHAR(100) NULL,
    UserEmail NVARCHAR(255)NOT NULL;


CREATE INDEX IX_EmailOutbox_IsSent_CreatedAt
ON [Writing.UserData].EmailOutbox (IsSent);



CREATE OR ALTER VIEW [Reading.UserData].PendingEmails
WITH SCHEMABINDING
AS
SELECT 
  Id,
  UserId,
  UserName,      
  UserEmail,    
  EmailType,
  Token,
  CreatedAt,
  IsSent,
  RetryCount
FROM [Writing.UserData].EmailOutbox 
WHERE IsSent = 0
  AND RetryCount <= 5;
GO


-- لازم نعمل clustered index أول
CREATE UNIQUE CLUSTERED INDEX IX_PendingEmails_Id
ON [Reading.UserData].PendingEmails(Id);



CREATE OR ALTER PROCEDURE [Reading.UserData].[GetPendingEmails]
    @Take INT = 50
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@Take)
        Id,
        UserId,
        UserName,     
        UserEmail,    
        EmailType,
        Token,
        CreatedAt,
        RetryCount,
        IsSent
    FROM [Reading.UserData].PendingEmails
    ORDER BY CreatedAt; 
END
GO



CREATE OR ALTER PROCEDURE [Writing.UserData].[AddEmailOutbox]
    @Id UNIQUEIDENTIFIER,
    @UserId UNIQUEIDENTIFIER,
    @UserName NVARCHAR(100),
    @UserEmail NVARCHAR(255),
    @EmailType NVARCHAR(50),
    @Token NVARCHAR(255) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [Writing.UserData].EmailOutbox 
        (Id, UserId, UserName, UserEmail, EmailType, Token)
    VALUES 
        (@Id, @UserId, @UserName, @UserEmail, @EmailType, @Token);
END
GO





CREATE OR ALTER PROCEDURE [Writing.UserData].[UpdateEmailOutbox]
    @Id UNIQUEIDENTIFIER,
    @IsSent BIT,
    @SentAt DATETIME2 NULL,
    @ErrorMessage NVARCHAR(MAX) NULL,
    @RetryCount INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE [Writing.UserData].EmailOutbox
    SET 
        IsSent = @IsSent,
        SentAt = @SentAt,
        ErrorMessage = @ErrorMessage,
        RetryCount = @RetryCount
    WHERE Id = @Id;
END
GO

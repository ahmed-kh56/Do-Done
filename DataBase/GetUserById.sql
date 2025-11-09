CREATE PROCEDURE [Reading.UserData].[GetUserById]
    @UserId UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT *
    FROM [Users] 
    WHERE Id = @UserId;
END

CREATE OR ALTER PROC [Reading.UserData].[GetRoleIdByName]
@RoleName NVARCHAR(60)='Employee'
AS
BEGIN 
SELECT Id
FROM Roles
WHERE Name =@RoleName
End
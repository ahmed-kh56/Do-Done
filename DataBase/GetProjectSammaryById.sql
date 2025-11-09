USE DoDoneTasksData
GO


CREATE OR ALTER PROC [Reading.ProjectData].GetProjectSummaryById
@ProjectId UniqueIdentifier
AS
BEGIN 
SELECT *FROM [Reading.ProjectData].ProjectSummary
WHERE Id =@ProjectId
END

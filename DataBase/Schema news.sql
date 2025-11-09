
USE DoDoneTasksData
GO





CREATE SCHEMA [Reading.UserData]
GO
CREATE SCHEMA [Reading.ProjectData]
GO
CREATE SCHEMA [Reading.FeatureTasks]
GO
CREATE SCHEMA [Writing.UserData]
GO
CREATE SCHEMA [Writing.FeatureTasks]
GO
CREATE SCHEMA [Writing.ProjectData]





ALTER SCHEMA [Reading.UserData] TRANSFER Reading.AllUsersRules;
ALTER SCHEMA [Reading.UserData] TRANSFER Reading.UserHighestRole;



-- UserData
ALTER SCHEMA [Reading.UserData] TRANSFER Reading.NotUsedUserTokens;
ALTER SCHEMA [Reading.UserData] TRANSFER [Reading.ProjectData].EmployeeProfilesView;

-- ProjectData (موظفين وبروجيكتس)
ALTER SCHEMA [Reading.ProjectData] TRANSFER Reading.ProjectsEmployees;
ALTER SCHEMA [Reading.ProjectData] TRANSFER Reading.ProjectSummary;
ALTER SCHEMA [Reading.ProjectData] TRANSFER Reading.ActiveProjectsScrumMasters;

-- FeatureTasks
ALTER SCHEMA [Reading.FeatureTasks] TRANSFER Reading.FeatureSummaryView;






-- UserData
ALTER SCHEMA [Reading.UserData] TRANSFER Reading.ExistsUserByEmail;
ALTER SCHEMA [Reading.UserData] TRANSFER Reading.GetUserByEmail;
ALTER SCHEMA [Reading.UserData] TRANSFER Reading.GetUserRoles;
ALTER SCHEMA [Reading.UserData] TRANSFER Reading.UseUserToken;
ALTER SCHEMA [Writing.UserData] TRANSFER Writing.AddUser;
ALTER SCHEMA [Writing.UserData] TRANSFER Writing.AddUserToken;
ALTER SCHEMA [Writing.UserData] TRANSFER Writing.UpdateUserData;
ALTER SCHEMA [Writing.UserData] TRANSFER Writing.UpdateUserToken;
ALTER SCHEMA [Reading.UserData] TRANSFER Reading.GetEmployeeProfile;
ALTER SCHEMA [Reading.UserData] TRANSFER Reading.GetEmployees;

-- ProjectData (موظفين وبروجيكتس)
ALTER SCHEMA [Reading.ProjectData] TRANSFER Reading.GetProjectEmployeesByProjectId;
ALTER SCHEMA [Reading.ProjectData] TRANSFER Reading.GetProjectSummaryById;

-- FeatureTasks
ALTER SCHEMA [Reading.FeatureTasks] TRANSFER Reading.GetFeatureSummary;


-- ابحث عن البروسيدورز أو التريجرز اللي جواها كويريز لسكيمات مختلفة
SELECT  
    o.type_desc AS ObjectType,
    s.name AS SchemaName,
    o.name AS ObjectName,
    m.definition AS ObjectDefinition
FROM sys.sql_modules m
JOIN sys.objects o ON m.object_id = o.object_id
JOIN sys.schemas s ON o.schema_id = s.schema_id
WHERE 
    (
        (m.definition LIKE '%Reading.%' OR m.definition LIKE '%Writing.%')
        AND m.definition NOT LIKE '%Reading.UserData%'
        AND m.definition NOT LIKE '%Reading.FeatureTasks%'
        AND m.definition NOT LIKE '%Reading.ProjectData%'

    )
ORDER BY o.type_desc, SchemaName, ObjectName;




SELECT referencing_schema_name,
       referencing_entity_name,
       referencing_class_desc
FROM sys.dm_sql_referencing_entities ('Reading.UserHighestRole', 'OBJECT');

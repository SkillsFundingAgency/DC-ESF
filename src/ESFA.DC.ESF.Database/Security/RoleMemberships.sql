
GO
ALTER ROLE [db_datawriter] ADD MEMBER [ESF_RW_User];
GO
ALTER ROLE [db_datareader] ADD MEMBER [ESF_RW_User];
GO
ALTER ROLE [db_datareader] ADD MEMBER [ESF_RO_User];

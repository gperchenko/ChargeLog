delete from [Provider]
GO

DBCC CHECKIDENT ('[Provider]', RESEED, 0);
GO

delete From [Car]
GO

DBCC CHECKIDENT ('[Car]', RESEED, 0);
GO

delete from [Location]
go

DBCC CHECKIDENT ('[Location]', RESEED, 0);
GO

delete from [Session]
GO

DBCC CHECKIDENT ('[Session]', RESEED, 0);
GO


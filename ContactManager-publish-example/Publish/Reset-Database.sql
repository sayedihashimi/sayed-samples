print 'dropping database ContactManager'

ALTER DATABASE ContactManager SET SINGLE_USER WITH ROLLBACK IMMEDIATE
GO
drop database ContactManager

GO

print 'creating database ContactManager'
create database ContactManager

ALTER DATABASE ContactManager SET MULTI_USER WITH NO_WAIT
USE [BankingDb]
GO

INSERT INTO [dbo].[Users]
           ([Id]
           ,[AccessFailedCount]
           ,[Email]
           ,[EmailConfirmed]
           ,[LockoutEnabled]
           ,[PasswordHash]
           ,[PhoneNumberConfirmed]
           ,[TwoFactorEnabled]
           ,[UserName])
     VALUES
           ('1'
			,0
           ,'testConcurrency'
           ,0
           ,0
           ,'1234567'
           ,0
           ,0
           ,'testConcurrency')
GO


INSERT INTO [dbo].[Account]
           ([AccountName]
           ,[AccountNumber]
           ,[Balance]
           ,[Password]
           ,[UserId])
     VALUES
           ('1234'
           ,'test-2134'
           ,100000
           ,'123456'
           ,1)
GO





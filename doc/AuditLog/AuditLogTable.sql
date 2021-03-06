if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[tblAuditLogRecord]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[tblAuditLogRecord]
GO

CREATE TABLE [dbo].[tblAuditLogRecord] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[EntityId] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Action] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Created] [datetime] NULL ,
	[UserId] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[UserName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[PropertyType] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[NewValue] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[OldValue] [nvarchar] (1000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[PropertyName] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[EntityType] [nvarchar] (255) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tblAuditLogRecord] WITH NOCHECK ADD 
	 PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 
GO


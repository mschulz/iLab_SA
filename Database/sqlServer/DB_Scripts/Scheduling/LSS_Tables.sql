
-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_LSS_Policy_Credential_Sets]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[LSS_Policy] DROP CONSTRAINT FK_LSS_Policy_Credential_Sets
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Permitted_Groups_Credential_Sets]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Permitted_Groups] DROP CONSTRAINT FK_Permitted_Groups_Credential_Sets
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Reservation_Info_Credential_Sets]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Reservation_Info] DROP CONSTRAINT FK_Reservation_Info_Credential_Sets
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_LSS_Policy_Experiment_Info]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[LSS_Policy] DROP CONSTRAINT FK_LSS_Policy_Experiment_Info
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Permitted_Experiments_Experiment_Info]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Permitted_Experiments] DROP CONSTRAINT FK_Permitted_Experiments_Experiment_Info
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Reservation_Info_Experiment_Info]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Reservation_Info] DROP CONSTRAINT FK_Reservation_Info_Experiment_Info
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Reservation_Info_USS_Info]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Reservation_Info] DROP CONSTRAINT FK_Reservation_Info_USS_Info
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Permitted_Experiments_Recurrence]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Permitted_Experiments] DROP CONSTRAINT FK_Permitted_Experiments_Recurrence
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Permitted_Groups_Recurrence]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Permitted_Groups] DROP CONSTRAINT FK_Permitted_Groups_Recurrence
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Credential_Sets]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Credential_Sets]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Experiment_Info]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Experiment_Info]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LSS_Policy]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[LSS_Policy]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LS_Resources]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[LS_Resources]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Permitted_Experiments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Permitted_Experiments]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Permitted_Groups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Permitted_Groups]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Recurrence]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reservation_Info]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Reservation_Info]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USS_Info]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[USS_Info]
GO

CREATE TABLE [dbo].[Credential_Sets] (
	[Credential_Set_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Service_Broker_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Service_Broker_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Group_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Experiment_Info] (
	[Experiment_Info_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Lab_Client_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Lab_Server_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Lab_Server_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Lab_Client_Version] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Lab_Client_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Provider_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Contact_Email] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Prepare_Time] [int] NOT NULL default 1,
	[Recover_Time] [int] NOT NULL ,
	[Minimum_Time] [int] NOT NULL ,
	[Early_Arrive_Time] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[LSS_Policy] (
	[LSS_Policy_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Rule] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Experiment_Info_ID] [int] NOT NULL ,
	[Credential_Set_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[LS_Resources] (
	[Resource_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Lab_Server_Guid] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Lab_Server_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Contact_Email] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[description] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Permitted_Experiments] (
	[Permitted_Experiment_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Experiment_Info_ID] [int] NOT NULL ,
	[Recurrence_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Permitted_Groups] (
	[Permitted_Group_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Credential_Set_ID] [int] NOT NULL ,
	[Recurrence_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Recurrence] (
	[Recurrence_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Resource_ID] [int] NOT NULL ,
	[Recurrence_Start_Date] [datetime] NOT NULL ,
	[Recurrence_Num_Days] [int] NOT NULL ,
	[Recurrence_Start_Offset] [int] NOT NULL ,
	[Recurrence_End_Offset] [int] NOT NULL ,
	[Recurrence_Type] [int] NOT NULL ,
	[Quantum] [int] NOT NULL ,
	[Day_Mask] [tinyint] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Reservation_Info] (
	[Reservation_Info_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Resource_ID] [int] NOT NULL ,
	[Start_Time] [datetime] NOT NULL ,
	[End_Time] [datetime] NOT NULL ,
	[Experiment_Info_ID] [int] NOT NULL ,
	[Credential_Set_ID] [int] NOT NULL,
	[USS_Info_ID] [int] NOT NULL,
	[Status] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[USS_Info] (
	[USS_Info_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[coupon_ID] [bigint] NOT NULL ,
	[USS_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[USS_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[USS_URL] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[domain_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Credential_Sets] WITH NOCHECK ADD 
	CONSTRAINT [PK_CredentialSets] PRIMARY KEY  CLUSTERED 
	(
		[Credential_Set_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Experiment_Info] WITH NOCHECK ADD 
	CONSTRAINT [PK_Experiment_Info] PRIMARY KEY  CLUSTERED 
	(
		[Experiment_Info_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[LSS_Policy] WITH NOCHECK ADD 
	CONSTRAINT [PK_LSS_Policy] PRIMARY KEY  CLUSTERED 
	(
		[LSS_Policy_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[LS_Resources] WITH NOCHECK ADD 
	CONSTRAINT [PK_LS_Resources] PRIMARY KEY  CLUSTERED 
	(
		[Resource_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Permitted_Experiments] WITH NOCHECK ADD 
	CONSTRAINT [PK_PermittedExperiments] PRIMARY KEY  CLUSTERED 
	(
		[Permitted_Experiment_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Permitted_Groups] WITH NOCHECK ADD 
	CONSTRAINT [PK_PermittedGroups] PRIMARY KEY  CLUSTERED 
	(
		[Permitted_Group_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Recurrence] WITH NOCHECK ADD 
	CONSTRAINT [PK_Recurrence] PRIMARY KEY  CLUSTERED 
	(
		[Recurrence_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Reservation_Info] WITH NOCHECK ADD 
	CONSTRAINT [PK_Reservation_Info] PRIMARY KEY  CLUSTERED 
	(
		[Reservation_Info_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[USS_Info] WITH NOCHECK ADD 
	CONSTRAINT [PK_USS_Info] PRIMARY KEY  CLUSTERED 
	(
		[USS_Info_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Experiment_Info] ADD 
	CONSTRAINT [DF_Experiment_Info_Early_Arrive_Time] DEFAULT (0) FOR [Early_Arrive_Time]
GO

ALTER TABLE [dbo].[USS_Info] ADD 
	CONSTRAINT [IX_USS_Info] UNIQUE  NONCLUSTERED 
	(
		[USS_GUID]
	)  ON [PRIMARY] 
GO


ALTER TABLE [dbo].[LSS_Policy] ADD 
	CONSTRAINT [FK_LSS_Policy_Credential_Sets] FOREIGN KEY 
	(
		[Credential_Set_ID]
	) REFERENCES [dbo].[Credential_Sets] (
		[Credential_Set_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_LSS_Policy_Experiment_Info] FOREIGN KEY 
	(
		[Experiment_Info_ID]
	) REFERENCES [dbo].[Experiment_Info] (
		[Experiment_Info_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[Permitted_Experiments] ADD 
	CONSTRAINT [FK_Permitted_Experiments_Experiment_Info] FOREIGN KEY 
	(
		[Experiment_Info_ID]
	) REFERENCES [dbo].[Experiment_Info] (
		[Experiment_Info_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Permitted_Experiments_Recurrence] FOREIGN KEY 
	(
		[Recurrence_ID]
	) REFERENCES [dbo].[Recurrence] (
		[Recurrence_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[Permitted_Groups] ADD 
	CONSTRAINT [FK_Permitted_Groups_Credential_Sets] FOREIGN KEY 
	(
		[Credential_Set_ID]
	) REFERENCES [dbo].[Credential_Sets] (
		[Credential_Set_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Permitted_Groups_Recurrence] FOREIGN KEY 
	(
		[Recurrence_ID]
	) REFERENCES [dbo].[Recurrence] (
		[Recurrence_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[Reservation_Info] ADD 
	CONSTRAINT [FK_Reservation_Info_Credential_Sets] FOREIGN KEY 
	(
		[Credential_Set_ID]
	) REFERENCES [dbo].[Credential_Sets] (
		[Credential_Set_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Reservation_Info_Experiment_Info] FOREIGN KEY 
	(
		[Experiment_Info_ID]
	) REFERENCES [dbo].[Experiment_Info] (
		[Experiment_Info_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE , 

	CONSTRAINT [FK_Reservation_Info_USS_Info] FOREIGN KEY 
	(
		[USS_Info_ID]
	) REFERENCES [dbo].[USS_Info] (
		[USS_Info_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

-- set a default USS for management groups that do not have an USS
SET IDENTITY_INSERT USS_Info ON
INSERT INTO USS_Info(USS_Info_ID,coupon_ID,USS_GUID,USS_Name,USS_URL,domain_GUID)
	Values(0,0,'0','No USS Assigned','0','0');
SET IDENTITY_INSERT USS_Info OFF
GO

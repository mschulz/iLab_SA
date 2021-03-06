-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Reservations_Credential_Sets]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT FK_Reservations_Credential_Sets
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_USS_Policy_Credential_Sets]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[USS_Policy] DROP CONSTRAINT FK_USS_Policy_Credential_Sets
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Experiment_Info_LSS_Info]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Experiment_Info] DROP CONSTRAINT FK_Experiment_Info_LSS_Info
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Reservations_Experiment_Info]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Reservations] DROP CONSTRAINT FK_Reservations_Experiment_Info
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_USS_Policy_Experiment_Info]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[USS_Policy] DROP CONSTRAINT FK_USS_Policy_Experiment_Info
GO

/****** Object:  Table [dbo].[Reservations]    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reservations]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Reservations]
GO

/****** Object:  Table [dbo].[USS_Policy]    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USS_Policy]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[USS_Policy]
GO

/****** Object:  Table [dbo].[Experiment_Info]    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Experiment_Info]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Experiment_Info]
GO

/****** Object:  Table [dbo].[Credential_Sets]    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Credential_Sets]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Credential_Sets]
GO

/****** Object:  Table [dbo].[LSS_Info]    Script Date: 10/4/2006 12:39:19 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LSS_Info]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[LSS_Info]
GO

/****** Object:  Table [dbo].[Credential_Sets]    Script Date: 10/4/2006 12:39:21 PM ******/
CREATE TABLE [dbo].[Credential_Sets] (
	[Credential_Set_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Group_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Service_Broker_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Service_Broker_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[LSS_Info]    Script Date: 10/4/2006 12:39:22 PM ******/
CREATE TABLE [dbo].[LSS_Info] (
	[LSS_Info_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Revoke_Coupon_ID] [bigint] NOT NULL ,
	[Domain_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[LSS_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[LSS_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[LSS_URL] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Experiment_Info]    Script Date: 10/4/2006 12:39:22 PM ******/
CREATE TABLE [dbo].[Experiment_Info] (
	[Experiment_Info_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Lab_Client_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Lab_Server_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Lab_Server_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Lab_Client_Version] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Lab_Client_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Provider_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[LSS_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Reservations]    Script Date: 10/4/2006 12:39:22 PM ******/
CREATE TABLE [dbo].[Reservations] (
	[Reservation_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Start_Time] [datetime] NOT NULL ,
	[End_Time] [datetime] NOT NULL ,
	[Credential_Set_ID] [int] NOT NULL ,
	[Experiment_Info_ID] [int] NOT NULL ,
	[User_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL

) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[USS_Policy]    Script Date: 10/4/2006 12:39:22 PM ******/
CREATE TABLE [dbo].[USS_Policy] (
	[USS_Policy_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Experiment_Info_ID] [int] NOT NULL ,
	[Rule] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Credential_Set_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Credential_Sets] WITH NOCHECK ADD 
	CONSTRAINT [PK_CredentialSets] PRIMARY KEY  CLUSTERED 
	(
		[Credential_Set_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[LSS_Info] WITH NOCHECK ADD 
	CONSTRAINT [PK_LSS_Info] PRIMARY KEY  CLUSTERED 
	(
		[LSS_Info_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Experiment_Info] WITH NOCHECK ADD 
	CONSTRAINT [PK_Experiment_Info] PRIMARY KEY  CLUSTERED 
	(
		[Experiment_Info_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Reservations] WITH NOCHECK ADD 
	CONSTRAINT [PK_Reservations] PRIMARY KEY  CLUSTERED 
	(
		[Reservation_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[USS_Policy] WITH NOCHECK ADD 
	CONSTRAINT [PK_USS_Policy] PRIMARY KEY  CLUSTERED 
	(
		[USS_Policy_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[LSS_Info] ADD 
	CONSTRAINT [IX_LSS_Info] UNIQUE  NONCLUSTERED 
	(
		[LSS_GUID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Experiment_Info] ADD 
	CONSTRAINT [FK_Experiment_Info_LSS_Info] FOREIGN KEY 
	(
		[LSS_GUID]
	) REFERENCES [dbo].[LSS_Info] (
		[LSS_GUID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[Reservations] ADD 
	CONSTRAINT [FK_Reservations_Credential_Sets] FOREIGN KEY 
	(
		[Credential_Set_ID]
	) REFERENCES [dbo].[Credential_Sets] (
		[Credential_Set_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Reservations_Experiment_Info] FOREIGN KEY 
	(
		[Experiment_Info_ID]
	) REFERENCES [dbo].[Experiment_Info] (
		[Experiment_Info_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[USS_Policy] ADD 
	CONSTRAINT [FK_USS_Policy_Credential_Sets] FOREIGN KEY 
	(
		[Credential_Set_ID]
	) REFERENCES [dbo].[Credential_Sets] (
		[Credential_Set_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_USS_Policy_Experiment_Info] FOREIGN KEY 
	(
		[Experiment_Info_ID]
	) REFERENCES [dbo].[Experiment_Info] (
		[Experiment_Info_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE
GO

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO




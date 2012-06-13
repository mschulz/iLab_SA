-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ExperimentRecord_Experiment]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ExperimentRecord] DROP CONSTRAINT FK_ExperimentRecord_Experiment
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentBlob]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ExperimentBlob]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentRecord]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ExperimentRecord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Experiment]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Experiment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LabApp]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[LabApp]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LocalGroup]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[LocalGroup]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Permission]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Permission]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SBGroup]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[SBGroup]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Task]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Task]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[VI]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[VI]
GO


CREATE TABLE [dbo].[Experiment] (
	[Experiment_ID] [bigint] NOT NULL ,
	[Group_ID] [int] NOT NULL ,
	[LabApp_ID] [int] NOT NULL ,
	[DateCreated] [datetime] NULL ,
	[LastModified] [datetime] NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ExperimentBlob] (
	[Experiment_ID] [bigint] NOT NULL ,
	[Blob_ID] [bigint] NOT NULL ,
	[ByteCount] [bigint] NOT NULL ,
	[checksum] [bigint] NOT NULL ,
	[description] [varchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[checksumType] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ExperimentRecord] (
	[experiment_ID] [bigint] NOT NULL ,
	[sponsor_GUID] [varchar] (50) NULL ,
	[submitter_GUID] [varchar] (50) NULL ,
	[sequenceNum] [int] NULL ,
	[createTime] [datetime] NULL ,
	[xmlSearchable] [bit] NULL ,
	[recordType] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[contents] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [LabApp] (
	[LabApp_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Width] [int] NOT NULL ,
	[Height] [int] NOT NULL ,
	[Type] [int] NOT NULL ,
	[Port] [int] NULL ,
	[Description] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Comment] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[DataSource] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[ExtraInfo] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[AppKey] [nvarchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[AppGuid] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Path] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Application] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Title] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Version] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Rev] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Server] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Page] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[CgiURL] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Contact] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




CREATE TABLE [dbo].[LocalGroup] (
	[GroupID] [int] IDENTITY (1, 1) NOT NULL ,
	[GroupName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Contact] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Comment] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Permission] (
	[Group_ID] [int] NOT NULL ,
	[LabApp_ID] [int] NOT NULL ,
	[Role] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[SBGroup] (
	[GroupID] [int] NOT NULL ,
	[SBGroup_ID] [int] NOT NULL ,
	[SB_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[GroupName] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Contact] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Comment] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Task] (
	[Task_ID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[Coupon_ID] [bigint] NULL ,
	[Experiment_ID] [bigint] NULL ,
	[Status] [int] NOT NULL ,
	[LabApp_ID] [int] NOT NULL ,
	[GroupName] [nvarchar](256) NOT NULL ,
	[StartTime] [datetime] NOT NULL ,
	[EndTime] [datetime] NOT NULL ,
	[Issuer_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Storage] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, 
	[Data] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	
) ON [PRIMARY]
GO



CREATE UNIQUE  INDEX [IX_SBGroup] ON [dbo].[SBGroup] ([SBGroup_ID], [SB_GUID])

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


ALTER TABLE [dbo].[Experiment] WITH NOCHECK ADD 
	CONSTRAINT [PK_Experiment] PRIMARY KEY  CLUSTERED 
	(
		[Experiment_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ExperimentRecord] ADD 
	CONSTRAINT [DF_ExperimentRecord_xmlSearchable] DEFAULT (0) FOR [xmlSearchable]
GO

ALTER TABLE [dbo].[ExperimentRecord] ADD 
	CONSTRAINT [FK_ExperimentRecord_Experiment] FOREIGN KEY 
	(
		[experiment_ID]
	) REFERENCES [dbo].[Experiment] (
		[Experiment_ID]
	)
GO

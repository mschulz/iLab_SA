
-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

--
-- Tables common to all ticketing services.
--

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Ticket_Ticket_Type]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Ticket] DROP CONSTRAINT FK_Ticket_Ticket_Type
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ticket_Type]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Ticket_Type]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_IssuedTicket_Redeemer]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[IssuedTicket] DROP CONSTRAINT FK_IssuedTicket_Redeemer
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_IssuedTicket_Sponsor]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[IssuedTicket] DROP CONSTRAINT FK_IssuedTicket_Sponsor
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Lab_Server_To_Client_Map_Lab_Servers]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Lab_Server_To_Client_Map] DROP CONSTRAINT FK_Lab_Server_To_Client_Map_Lab_Servers
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_System_Messages_Lab_Servers]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[System_Messages] DROP CONSTRAINT FK_System_Messages_Lab_Servers
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Ticket_Issuer]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Ticket] DROP CONSTRAINT FK_Ticket_Issuer
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Ticket_Redeemer]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Ticket] DROP CONSTRAINT FK_Ticket_Redeemer
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Ticket_Sponsor]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Ticket] DROP CONSTRAINT FK_Ticket_Sponsor
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ProcessAgent]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ProcessAgent]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ProcessAgent_Coupon]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ProcessAgent] DROP CONSTRAINT FK_ProcessAgent_Coupon
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Ticket_Coupon]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Ticket] DROP CONSTRAINT FK_Ticket_Coupon
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ProcessAgent_ProcessAgent_Type]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ProcessAgent] DROP CONSTRAINT FK_ProcessAgent_ProcessAgent_Type
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Ticket_Issuer]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Ticket] DROP CONSTRAINT FK_Ticket_Issuer
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Ticket_Sponsor]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Ticket] DROP CONSTRAINT FK_Ticket_Sponsor
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Ticket_Redeemer]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Ticket] DROP CONSTRAINT FK_Ticket_Redeemer
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Ticket_Ticket_Type]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Ticket] DROP CONSTRAINT FK_Ticket_Ticket_Type
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Coupon]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Coupon]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ProcessAgent_Type]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ProcessAgent_Type]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ProcessAgent]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ProcessAgent]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ticket]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Ticket]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Ticket_Type]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Ticket_Type]
GO

CREATE TABLE [dbo].[Coupon] (
	[Coupon_ID] [bigint] NOT NULL ,
	[Cancelled] [bit] NOT NULL,
	[Issuer_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Passkey] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT [IX_Coupon] UNIQUE  NONCLUSTERED 
	(
		[Coupon_ID],
		[Issuer_GUID]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ProcessAgent_Type] (
	[ProcessAgent_Type_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Short_Name] [char](4) NOT NULL,
	[Description] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	CONSTRAINT [PK_ProcessAgent_Type] PRIMARY KEY  CLUSTERED 
	(
		[ProcessAgent_Type_ID]
	)  ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [ProcessAgent] (
	[Agent_ID] [int] IDENTITY (10, 1) NOT NULL ,
	[Retired] [bit] NOT NULL CONSTRAINT [DF_ProcessAgent_Retired] DEFAULT (0),
	[Self] [bit] NOT NULL CONSTRAINT [DF_ProcessAgent_Self] DEFAULT (0),
	[ProcessAgent_Type_ID] [int] NOT NULL ,
	[IdentIn_ID] [bigint] NULL ,
	[IdentOut_ID] [bigint] NULL ,
	[Issuer_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Domain_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Agent_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Agent_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[WebService_URL] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Codebase_URL] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Info_URL] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Contact_Email] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Bug_Email] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Location] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Description] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	CONSTRAINT [PK_ProcessAgent] PRIMARY KEY  CLUSTERED 
	(
		[Agent_ID]
	)  ON [PRIMARY] ,
	CONSTRAINT [IX_ProcessAgent] UNIQUE  NONCLUSTERED 
	(
		[Agent_GUID]
	)  ON [PRIMARY] ,
	CONSTRAINT [FK_ProcessAgent_ProcessAgent_Type] FOREIGN KEY 
	(
		[ProcessAgent_Type_ID]
	) REFERENCES [ProcessAgent_Type] (
		[ProcessAgent_Type_ID]
	) ON DELETE NO ACTION  ON UPDATE NO ACTION 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Ticket_Type] (
	[Ticket_Type_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Short_Description] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Abstract] [bit] NOT NULL,
CONSTRAINT [PK_Ticket_Type] PRIMARY KEY  CLUSTERED 
	(
		[Ticket_Type_ID]
	)  ON [PRIMARY] 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Ticket] (
	[Ticket_ID] [bigint] NOT NULL ,
	[Ticket_Type_ID] [int] NOT NULL ,
	[Creation_Time] [DateTime] NOT NULL, 
	[Duration] [bigint] NOT NULL ,
	[Cancelled] [bit] NOT NULL ,
	[Payload] [ntext]  NULL ,
	[Coupon_ID] [bigint] NOT NULL ,
	[Issuer_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Redeemer_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Sponsor_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	CONSTRAINT [IX_Ticket] UNIQUE NONCLUSTERED 
	(
		[Ticket_ID],
		[Issuer_Guid]
	), 
	CONSTRAINT [IX_Ticket_Value] UNIQUE  NONCLUSTERED 
	(
		[Ticket_Type_ID],
		[Redeemer_GUID],
		[Issuer_GUID],
		[Coupon_ID]
	)
	 ON [PRIMARY] 

) ON [PRIMARY]
GO


ALTER TABLE [dbo].[Ticket] ADD 
	CONSTRAINT [FK_Ticket_Coupon] FOREIGN KEY 
	(
		[Coupon_ID], [Issuer_GUID]
	) REFERENCES [dbo].[Coupon] (
		[Coupon_ID] , [Issuer_GUID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Ticket_Ticket_Type] FOREIGN KEY 
	(
		[Ticket_Type_ID]
	) REFERENCES [dbo].[Ticket_Type] (
		[Ticket_Type_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


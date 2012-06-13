-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$


--
-- TicketIssuer additional tables. These tables should be added to the ProcessAgentTables for a ServiceBroker's Database.
--

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_IssuedTicket_IssuedCoupon]') 
and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[IssuedTicket] DROP CONSTRAINT FK_IssuedTicket_IssuedCoupon
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_IssuedTicket_Sponsor]') 
and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[IssuedTicket] DROP CONSTRAINT FK_IssuedTicket_Sponsor
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_IssuedTicket_Redeemer]') 
and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[IssuedTicket] DROP CONSTRAINT FK_IssuedTicket_Redeemer
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_IssuedTicket_Ticket_Type]') 
and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[IssuedTicket] DROP CONSTRAINT FK_IssuedTicket_Ticket_Type
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IssuedCoupon]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[IssuedCoupon]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IssuedTicket]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[IssuedTicket]
GO

CREATE TABLE [dbo].[IssuedCoupon] (
	[Coupon_ID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[Cancelled] [bit] NOT NULL,
	[Passkey] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[IssuedTicket] (
	[Ticket_id] [bigint] IDENTITY (1, 1) NOT NULL ,
	[Ticket_Type_ID] [int] NOT NULL ,
	[Coupon_ID] [bigint] NOT NULL ,
	[creation_Time] [DateTime] NOT NULL ,
	[duration] [bigint] NOT NULL ,
	[Payload] [ntext]  NULL, 
	[Cancelled] [bit] NOT NULL ,
	[Sponsor_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Redeemer_GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	
	
	
) ON [PRIMARY]
GO


ALTER TABLE [dbo].[IssuedCoupon] WITH NOCHECK ADD 
	CONSTRAINT [PK_IssuedCoupon] PRIMARY KEY  CLUSTERED 
	(
		[Coupon_ID]
	)  ON [PRIMARY] 
GO


ALTER TABLE [dbo].[IssuedTicket] WITH NOCHECK ADD 
	CONSTRAINT [PK_IssuedTicket] PRIMARY KEY  CLUSTERED 
	(
		[Ticket_id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[IssuedTicket] ADD 
	CONSTRAINT [IX_IssuedTicket] UNIQUE  NONCLUSTERED 
	(
		[Ticket_Type_ID],
		[Redeemer_GUID],
		[Coupon_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[IssuedTicket] ADD 
	CONSTRAINT [FK_IssuedTicket_IssuedCoupon] FOREIGN KEY 
	(
		[Coupon_ID]
	) REFERENCES [dbo].[IssuedCoupon] (
		[Coupon_ID]
	) ON DELETE CASCADE,
	CONSTRAINT [FK_IssuedTicket_Sponsor] FOREIGN KEY 
	(
		[Sponsor_GUID]
	) REFERENCES [dbo].[ProcessAgent] (
		[Agent_GUID]
	)ON DELETE CASCADE,
	CONSTRAINT [FK_IssuedTicket_Redeemer] FOREIGN KEY 
	(
		[Redeemer_GUID]
	) REFERENCES [dbo].[ProcessAgent] (
		[Agent_GUID]
	)ON DELETE NO ACTION,
	CONSTRAINT [FK_IssuedTicket_Ticket_Type] FOREIGN KEY 
	(
		[Ticket_Type_ID]
	) REFERENCES [dbo].[Ticket_Type] (
		[Ticket_Type_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO


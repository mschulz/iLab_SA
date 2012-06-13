/* Convert isb db VERSION 3.0.4 TO 3.0.6 */

-- Copyright (c) 2011 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

/*** TicketIssuer Changes ***/

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
	)ON DELETE NO ACTION
GO


/***** ServiceBroker Changes ***/

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Client_Items_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Client_Items] DROP CONSTRAINT FK_Client_Items_Users
GO

/* Session History does not exist in 3.0.3 */
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_SessionHistory_Session]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Session_History] DROP CONSTRAINT FK_SessionHistory_Session
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_SessionHistory_Group]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Session_History] DROP CONSTRAINT FK_SessionHistory_Group
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_SessionHistory_Client]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Session_History] DROP CONSTRAINT FK_SessionHistory_Client
GO



/* drop Tables **/



/* Not Curently used but renamed */
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LabSession_To_Experiment]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[LabSession_To_Experiment]
GO

/* Renamed tablefor this change */
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Lab_Session_To_Experiment]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Lab_Session_To_Experiment]
GO

/* Added for this change */
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Session_History]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Session_History]
GO


CREATE TABLE [dbo].[Lab_Session_To_Experiment](
	[LabSession_ID] [bigint],
	[experiment_id] [bigint]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[User_Sessions]
	ALTER COLUMN Effective_Group_ID  int null 
go

/****** Object:  Table [dbo].[Session_History]    Script Date: 8/30/2005 4:08:00 PM ******/
CREATE TABLE [dbo].[Session_History] (
	[Session_ID] [bigint] NOT NULL ,
	[Modify_Time] [datetime] NOT NULL DEFAULT GETUTCDATE(),
	[Group_ID] [int] NOT NULL ,
	[Client_ID] [int] NOT NULL ,
	[Session_Key] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Session_History] ADD 
	CONSTRAINT [FK_SessionHistory_Session] FOREIGN KEY 
	(
		[Session_ID]
	) REFERENCES [dbo].[User_Sessions] (
		[Session_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_SessionHistory_Group] FOREIGN KEY 
	(
		[Group_ID]
	) REFERENCES [dbo].[Groups] (
		[Group_ID]
	) ON DELETE NO ACTION  ON UPDATE NO ACTION ,
	CONSTRAINT [FK_SessionHistory_Client] FOREIGN KEY 
	(
		[Client_ID]
	) REFERENCES [dbo].[Lab_Clients] (
		[Client_ID]
	) ON DELETE NO ACTION  ON UPDATE NO ACTION 
GO

ALTER TABLE [dbo].[Lab_Clients]
	ADD Documentation_Url 
		nvarchar(512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
go

CREATE INDEX IX_LAB_CLIENTS ON LAB_CLIENTS (Client_Guid ASC)
go


ALTER TABLE [dbo].[Client_Items] ADD
	CONSTRAINT [FK_Client_Items_Users] FOREIGN KEY 
	(
		[User_ID]
	) REFERENCES [dbo].[Agents] (
		[Agent_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO
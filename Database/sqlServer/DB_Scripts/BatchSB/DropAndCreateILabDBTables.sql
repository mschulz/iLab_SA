-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Agent_Hierarchy_Agents]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Agent_Hierarchy] DROP CONSTRAINT FK_Agent_Hierarchy_Agents
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Grants_Agents]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Grants] DROP CONSTRAINT FK_Grants_Agents
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Principals_Authentication_Types]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Principals] DROP CONSTRAINT FK_Principals_Authentication_Types
GO



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Grants_Functions]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Grants] DROP CONSTRAINT FK_Grants_Functions
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Groups_Group_Types]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Groups] DROP CONSTRAINT FK_Groups_Group_Types
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Agent_Hierarchy_Groups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Agent_Hierarchy] DROP CONSTRAINT FK_Agent_Hierarchy_Groups
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Groups_Groups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Groups] DROP CONSTRAINT FK_Groups_Groups
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_System_Messages_Groups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[System_Messages] DROP CONSTRAINT FK_System_Messages_Groups
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_User_Sessions_Groups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[User_Sessions] DROP CONSTRAINT FK_User_Sessions_Groups
GO



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Lab_Server_To_Client_Map_Lab_Clients]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Lab_Server_To_Client_Map] DROP CONSTRAINT FK_Lab_Server_To_Client_Map_Lab_Clients
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Experiment_Information_Lab_Servers]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Experiment_Information] DROP CONSTRAINT FK_Experiment_Information_Lab_Servers
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Lab_Server_To_Client_Map_Lab_Servers]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Lab_Server_To_Client_Map] DROP CONSTRAINT FK_Lab_Server_To_Client_Map_Lab_Servers
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_System_Messages_Lab_Servers]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[System_Messages] DROP CONSTRAINT FK_System_Messages_Lab_Servers
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_System_Messages_Message_Types]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[System_Messages] DROP CONSTRAINT FK_System_Messages_Message_Types
GO



if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Qualifiers_Qualifier_Types]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Qualifiers] DROP CONSTRAINT FK_Qualifiers_Qualifier_Types
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Grants_Qualifiers]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Grants] DROP CONSTRAINT FK_Grants_Qualifiers
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Qualifier_Hierarchy_Qualifiers]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Qualifier_Hierarchy] DROP CONSTRAINT FK_Qualifier_Hierarchy_Qualifiers
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Qualifier_Hierarchy_Qualifiers1]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Qualifier_Hierarchy] DROP CONSTRAINT FK_Qualifier_Hierarchy_Qualifiers1
GO




if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Experiment_Information_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Experiment_Information] DROP CONSTRAINT FK_Experiment_Information_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Principals_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Principals] DROP CONSTRAINT FK_Principals_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_User_Sessions_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[User_Sessions] DROP CONSTRAINT FK_User_Sessions_Users
GO

/****** Object:  Table [dbo].[Agent_Hierarchy]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Agent_Hierarchy]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Agent_Hierarchy]
GO

/****** Object:  Table [dbo].[Agents]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Agents]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Agents]
GO

/****** Object:  Table [dbo].[Authentication_Types]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Authentication_Types]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Authentication_Types]
GO




/****** Object:  Table [dbo].[Client_Types]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Client_Types]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Client_Types]
GO





/****** Object:  Table [dbo].[Functions]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Functions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Functions]
GO

/****** Object:  Table [dbo].[Grants]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Grants]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Grants]
GO

/****** Object:  Table [dbo].[Group_Types]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Group_Types]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Group_Types]
GO

/****** Object:  Table [dbo].[Groups]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Groups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Groups]
GO

/****** Object:  Table [dbo].[Lab_Clients]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Lab_Clients]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Lab_Clients]
GO

/****** Object:  Table [dbo].[Lab_Server_To_Client_Map]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Lab_Server_To_Client_Map]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Lab_Server_To_Client_Map]
GO

/****** Object:  Table [dbo].[Lab_Servers]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Lab_Servers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Lab_Servers]
GO

/****** Object:  Table [dbo].[Message_Types]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Message_Types]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Message_Types]
GO

/****** Object:  Table [dbo].[Principals]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Principals]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Principals]
GO


/****** Object:  Table [dbo].[Qualifier_Hierarchy]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Qualifier_Hierarchy]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Qualifier_Hierarchy]
GO

/****** Object:  Table [dbo].[Qualifier_Types]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Qualifier_Types]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Qualifier_Types]
GO

/****** Object:  Table [dbo].[Qualifiers]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Qualifiers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Qualifiers]
GO

/****** Object:  Table [dbo].[Record_Attributes]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Record_Attributes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Record_Attributes]
GO


/****** Object:  Table [dbo].[System_Messages]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[System_Messages]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[System_Messages]
GO


/****** Object:  Table [dbo].[User_Sessions]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[User_Sessions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[User_Sessions]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Users]
GO

/****** Object:  Table [dbo].[Agent_Hierarchy]    Script Date: 8/30/2005 4:07:54 PM ******/
CREATE TABLE [dbo].[Agent_Hierarchy] (
	[Agent_ID] [numeric](18, 0) NOT NULL ,
	[Parent_Group_ID] [numeric](18, 0) NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Agents]    Script Date: 8/30/2005 4:07:55 PM ******/
CREATE TABLE [dbo].[Agents] (
	[Agent_Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Is_Group] [bit] NOT NULL ,
	[Agent_ID] [numeric](18, 0) IDENTITY (2, 1) NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Authentication_Types]    Script Date: 8/30/2005 4:07:55 PM ******/
CREATE TABLE [dbo].[Authentication_Types] (
	[Auth_Type_ID] [numeric](18, 0) IDENTITY (1, 1) NOT NULL ,
	[Description] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Client_Types]    Script Date: 8/30/2005 4:07:56 PM ******/
CREATE TABLE [dbo].[Client_Types] (
	[Client_Type_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Description] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO



/****** Object:  Table [dbo].[Functions]    Script Date: 8/30/2005 4:07:57 PM ******/
CREATE TABLE [dbo].[Functions] (
	[Function_ID] [numeric](18, 0) IDENTITY (1, 1) NOT NULL ,
	[Function_Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Description] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Grants]    Script Date: 8/30/2005 4:07:57 PM ******/
CREATE TABLE [dbo].[Grants] (
	[Agent_ID] [numeric](18, 0) NOT NULL ,
	[Function_ID] [numeric](18, 0) NOT NULL ,
	[Qualifier_ID] [numeric](18, 0) NOT NULL ,
	[Date_Created] [datetime] NOT NULL ,
	[Grant_ID] [numeric](18, 0) IDENTITY (1, 1) NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Group_Types]    Script Date: 8/30/2005 4:07:57 PM ******/
CREATE TABLE [dbo].[Group_Types] (
	[Group_Type_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Description] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Groups]    Script Date: 8/30/2005 4:07:57 PM ******/
CREATE TABLE [dbo].[Groups] (
	[Group_Name] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Description] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Date_Created] [datetime] NOT NULL ,
	[Group_ID] [numeric](18, 0) NOT NULL ,
	[Email] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Group_Type_ID] [int] NOT NULL ,
	[Associated_Group_ID] [numeric](18, 0) NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Lab_Clients]    Script Date: 8/30/2005 4:07:57 PM ******/
CREATE TABLE [dbo].[Lab_Clients] (
	[Lab_Client_Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Loader_Script] [varchar] (3000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Long_Description] [varchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Contact_Email] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Short_Description] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Version] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Date_Created] [datetime] NOT NULL ,
	[Contact_First_Name] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Contact_Last_Name] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Client_ID] [numeric](18, 0) IDENTITY (1, 1) NOT NULL ,
	[Notes] [varchar] (2000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Client_Type_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Lab_Server_To_Client_Map]    Script Date: 8/30/2005 4:07:58 PM ******/
CREATE TABLE [dbo].[Lab_Server_To_Client_Map] (
	[Client_ID] [numeric](18, 0) NOT NULL ,
	[Lab_Server_ID] [numeric](18, 0) NOT NULL ,
	[Display_Order] [int] NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Lab_Servers]    Script Date: 8/30/2005 4:07:58 PM ******/
CREATE TABLE [dbo].[Lab_Servers] (
	[GUID] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Web_Service_URL] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Description] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Info_URL] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Lab_Server_Name] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Contact_First_Name] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Contact_Last_Name] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Contact_Email] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Date_Created] [datetime] NOT NULL ,
	[Outgoing_Passkey] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Incoming_Passkey] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Lab_Server_ID] [numeric](18, 0) IDENTITY (1, 1) NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Message_Types]    Script Date: 8/30/2005 4:07:58 PM ******/
CREATE TABLE [dbo].[Message_Types] (
	[Message_Type_ID] [numeric](18, 0) IDENTITY (1, 1) NOT NULL ,
	[Description] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Principals]    Script Date: 8/30/2005 4:07:58 PM ******/
CREATE TABLE [dbo].[Principals] (
	[Principal_ID] [numeric](18, 0) IDENTITY (1, 1) NOT NULL ,
	[Auth_Type_ID] [numeric](18, 0) NOT NULL ,
	[User_ID] [numeric](18, 0) NOT NULL ,
	[Principal_String] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO



/****** Object:  Table [dbo].[Qualifier_Hierarchy]    Script Date: 8/30/2005 4:07:59 PM ******/
CREATE TABLE [dbo].[Qualifier_Hierarchy] (
	[Qualifier_ID] [numeric](18, 0) NOT NULL ,
	[Parent_Qualifier_ID] [numeric](18, 0) NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Qualifier_Types]    Script Date: 8/30/2005 4:07:59 PM ******/
CREATE TABLE [dbo].[Qualifier_Types] (
	[Qualifier_Type_ID] [numeric](18, 0) NOT NULL ,
	[Description] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Qualifiers]    Script Date: 8/30/2005 4:07:59 PM ******/
CREATE TABLE [dbo].[Qualifiers] (
	[Qualifier_ID] [numeric](18, 0) IDENTITY (1, 1) NOT NULL ,
	[Qualifier_Type_ID] [numeric](18, 0) NOT NULL ,
	[Qualifier_Reference_ID] [numeric](18, 0) NOT NULL ,
	[Qualifier_Name] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Date_Created] [datetime] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Record_Attributes]    Script Date: 8/30/2005 4:07:59 PM ******/
CREATE TABLE [dbo].[Record_Attributes] (
	[Attribute_ID] [numeric](18, 0) IDENTITY (1, 1) NOT NULL ,
	[Record_ID] [numeric](18, 0) NOT NULL ,
	[Attribute_Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Attribute_Value] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[System_Messages]    Script Date: 8/30/2005 4:07:59 PM ******/
CREATE TABLE [dbo].[System_Messages] (
	[System_Message_ID] [numeric](18, 0) IDENTITY (1, 1) NOT NULL ,
	[Message_Title] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Message_Type_ID] [numeric](18, 0) NOT NULL ,
	[Message_Body] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Group_ID] [numeric](18, 0) NOT NULL ,
	[Lab_Server_ID] [numeric](18, 0) NULL ,
	[To_Be_Displayed] [bit] NOT NULL ,
	[Last_Modified] [datetime] NOT NULL ,
	[Date_Created] [datetime] NOT NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO



/****** Object:  Table [dbo].[User_Sessions]    Script Date: 8/30/2005 4:08:00 PM ******/
CREATE TABLE [dbo].[User_Sessions] (
	[Session_ID] [numeric](18, 0) IDENTITY (1, 1) NOT NULL ,
	[Session_Start_Time] [datetime] NOT NULL ,
	[Session_End_Time] [datetime] NULL ,
	[User_ID] [numeric](18, 0) NOT NULL ,
	[Effective_Group_ID] [numeric](18, 0) NOT NULL ,
	[Session_Key] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 8/30/2005 4:08:00 PM ******/
CREATE TABLE [dbo].[Users] (
	[User_Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[First_Name] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Last_Name] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Email] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Affiliation] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Date_Created] [datetime] NOT NULL ,
	[Xml_Extension] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Password] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Lock_User] [bit] NOT NULL ,
	[User_ID] [numeric](18, 0) NOT NULL ,
	[Signup_Reason] [varchar] (4000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Agent_Hierarchy] WITH NOCHECK ADD 
	CONSTRAINT [PK_Agent_Hierarchy] PRIMARY KEY  CLUSTERED 
	(
		[Agent_ID],
		[Parent_Group_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Agents] WITH NOCHECK ADD 
	CONSTRAINT [PK_Agents] PRIMARY KEY  CLUSTERED 
	(
		[Agent_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Authentication_Types] WITH NOCHECK ADD 
	CONSTRAINT [PK_Authentication_Types] PRIMARY KEY  CLUSTERED 
	(
		[Auth_Type_ID]
	)  ON [PRIMARY] 
GO


ALTER TABLE [dbo].[Client_Types] WITH NOCHECK ADD 
	CONSTRAINT [PK_Client_Types] PRIMARY KEY  CLUSTERED 
	(
		[Client_Type_ID]
	)  ON [PRIMARY] 
GO



ALTER TABLE [dbo].[Functions] WITH NOCHECK ADD 
	CONSTRAINT [PK_Functions] PRIMARY KEY  CLUSTERED 
	(
		[Function_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Grants] WITH NOCHECK ADD 
	CONSTRAINT [PK_Grants] PRIMARY KEY  CLUSTERED 
	(
		[Grant_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Group_Types] WITH NOCHECK ADD 
	CONSTRAINT [PK_Group_Types] PRIMARY KEY  CLUSTERED 
	(
		[Group_Type_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Groups] WITH NOCHECK ADD 
	CONSTRAINT [PK_Groups] PRIMARY KEY  CLUSTERED 
	(
		[Group_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Lab_Clients] WITH NOCHECK ADD 
	CONSTRAINT [PK_LabClients] PRIMARY KEY  CLUSTERED 
	(
		[Client_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Lab_Server_To_Client_Map] WITH NOCHECK ADD 
	CONSTRAINT [PK_LabServer_Client_Map] PRIMARY KEY  CLUSTERED 
	(
		[Client_ID],
		[Lab_Server_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Lab_Servers] WITH NOCHECK ADD 
	CONSTRAINT [PK_LabServers] PRIMARY KEY  CLUSTERED 
	(
		[Lab_Server_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Message_Types] WITH NOCHECK ADD 
	CONSTRAINT [PK_Message_Types] PRIMARY KEY  CLUSTERED 
	(
		[Message_Type_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Principals] WITH NOCHECK ADD 
	CONSTRAINT [PK_Principals] PRIMARY KEY  CLUSTERED 
	(
		[Principal_ID]
	)  ON [PRIMARY] 
GO



ALTER TABLE [dbo].[Qualifier_Hierarchy] WITH NOCHECK ADD 
	CONSTRAINT [PK_Qualifier_Hierarchy] PRIMARY KEY  CLUSTERED 
	(
		[Qualifier_ID],
		[Parent_Qualifier_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Qualifier_Types] WITH NOCHECK ADD 
	CONSTRAINT [PK_Qualifier_Types] PRIMARY KEY  CLUSTERED 
	(
		[Qualifier_Type_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Qualifiers] WITH NOCHECK ADD 
	CONSTRAINT [PK_Qualifiers] PRIMARY KEY  CLUSTERED 
	(
		[Qualifier_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Record_Attributes] WITH NOCHECK ADD 
	CONSTRAINT [PK_Record_Attributes] PRIMARY KEY  CLUSTERED 
	(
		[Attribute_ID]
	)  ON [PRIMARY] 
GO



ALTER TABLE [dbo].[System_Messages] WITH NOCHECK ADD 
	CONSTRAINT [PK_System_Messages] PRIMARY KEY  CLUSTERED 
	(
		[System_Message_ID]
	)  ON [PRIMARY] 
GO



ALTER TABLE [dbo].[User_Sessions] WITH NOCHECK ADD 
	CONSTRAINT [PK_User_Sessions] PRIMARY KEY  CLUSTERED 
	(
		[Session_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Users] WITH NOCHECK ADD 
	CONSTRAINT [PK_Users] PRIMARY KEY  CLUSTERED 
	(
		[User_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Agents] ADD 
	CONSTRAINT [IX_Agents] UNIQUE  NONCLUSTERED 
	(
		[Agent_Name]
	)  ON [PRIMARY] 
GO



ALTER TABLE [dbo].[Grants] ADD 
	CONSTRAINT [DF_Grants_Date_Created] DEFAULT (getdate()) FOR [Date_Created],
	CONSTRAINT [IX_Grants] UNIQUE  NONCLUSTERED 
	(
		[Agent_ID],
		[Function_ID],
		[Qualifier_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Groups] ADD 
	CONSTRAINT [DF_Groups_Date_Created] DEFAULT (getdate()) FOR [Date_Created]
GO

ALTER TABLE [dbo].[Lab_Clients] ADD 
	CONSTRAINT [DF_LabClients_Date_Created] DEFAULT (getdate()) FOR [Date_Created]
GO

ALTER TABLE [dbo].[Lab_Servers] ADD 
	CONSTRAINT [DF_LabServers_Date_Created] DEFAULT (getdate()) FOR [Date_Created]
GO

ALTER TABLE [dbo].[Principals] ADD 
	CONSTRAINT [IX_Principals] UNIQUE  NONCLUSTERED 
	(
		[Principal_String],
		[Auth_Type_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Qualifier_Hierarchy] ADD 
	CONSTRAINT [DF_Qualifier_Hierarchy_Parent_Qualifier_ID] DEFAULT (0) FOR [Parent_Qualifier_ID]
GO

ALTER TABLE [dbo].[Qualifiers] ADD 
	CONSTRAINT [DF_Qualifiers_Date_Created] DEFAULT (getdate()) FOR [Date_Created],
	CONSTRAINT [IX_Qualifiers] UNIQUE  NONCLUSTERED 
	(
		[Qualifier_Type_ID],
		[Qualifier_Reference_ID]
	)  ON [PRIMARY] 
GO



ALTER TABLE [dbo].[System_Messages] ADD 
	CONSTRAINT [DF_System_Messages_Last_Modified] DEFAULT (getdate()) FOR [Last_Modified],
	CONSTRAINT [DF_System_Messages_Date_Created] DEFAULT (getdate()) FOR [Date_Created]
GO



ALTER TABLE [dbo].[User_Sessions] ADD 
	CONSTRAINT [DF_User_Sessions_Login_Time] DEFAULT (getdate()) FOR [Session_Start_Time]
GO

ALTER TABLE [dbo].[Users] ADD 
	CONSTRAINT [DF_Users_Date_Created] DEFAULT (getdate()) FOR [Date_Created],
	CONSTRAINT [DF_Users_Lock_User] DEFAULT (0) FOR [Lock_User]
GO

ALTER TABLE [dbo].[Agent_Hierarchy] ADD 
	CONSTRAINT [FK_Agent_Hierarchy_Agents] FOREIGN KEY 
	(
		[Agent_ID]
	) REFERENCES [dbo].[Agents] (
		[Agent_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Agent_Hierarchy_Groups] FOREIGN KEY 
	(
		[Parent_Group_ID]
	) REFERENCES [dbo].[Groups] (
		[Group_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO


ALTER TABLE [dbo].[Client_Info] ADD 
	CONSTRAINT [FK_Client_Info_Lab_Clients] FOREIGN KEY 
	(
		[Client_ID]
	) REFERENCES [dbo].[Lab_Clients] (
		[Client_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO




ALTER TABLE [dbo].[Grants] ADD 
	CONSTRAINT [FK_Grants_Agents] FOREIGN KEY 
	(
		[Agent_ID]
	) REFERENCES [dbo].[Agents] (
		[Agent_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Grants_Functions] FOREIGN KEY 
	(
		[Function_ID]
	) REFERENCES [dbo].[Functions] (
		[Function_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Grants_Qualifiers] FOREIGN KEY 
	(
		[Qualifier_ID]
	) REFERENCES [dbo].[Qualifiers] (
		[Qualifier_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[Groups] ADD 
	CONSTRAINT [FK_Groups_Group_Types] FOREIGN KEY 
	(
		[Group_Type_ID]
	) REFERENCES [dbo].[Group_Types] (
		[Group_Type_ID]
	) ON UPDATE CASCADE ,
	CONSTRAINT [FK_Groups_Groups] FOREIGN KEY 
	(
		[Associated_Group_ID]
	) REFERENCES [dbo].[Groups] (
		[Group_ID]
	)
GO

ALTER TABLE [dbo].[Lab_Clients] ADD 
	CONSTRAINT [FK_Lab_Clients_Client_Types] FOREIGN KEY 
	(
		[Client_Type_ID]
	) REFERENCES [dbo].[Client_Types] (
		[Client_Type_ID]
	) ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[Lab_Server_To_Client_Map] ADD 
	CONSTRAINT [FK_Lab_Server_To_Client_Map_Lab_Clients] FOREIGN KEY 
	(
		[Client_ID]
	) REFERENCES [dbo].[Lab_Clients] (
		[Client_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Lab_Server_To_Client_Map_Lab_Servers] FOREIGN KEY 
	(
		[Lab_Server_ID]
	) REFERENCES [dbo].[Lab_Servers] (
		[Lab_Server_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[Principals] ADD 
	CONSTRAINT [FK_Principals_Authentication_Types] FOREIGN KEY 
	(
		[Auth_Type_ID]
	) REFERENCES [dbo].[Authentication_Types] (
		[Auth_Type_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Principals_Users] FOREIGN KEY 
	(
		[User_ID]
	) REFERENCES [dbo].[Users] (
		[User_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[Qualifier_Hierarchy] ADD 
	CONSTRAINT [FK_Qualifier_Hierarchy_Qualifiers] FOREIGN KEY 
	(
		[Qualifier_ID]
	) REFERENCES [dbo].[Qualifiers] (
		[Qualifier_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Qualifier_Hierarchy_Qualifiers1] FOREIGN KEY 
	(
		[Parent_Qualifier_ID]
	) REFERENCES [dbo].[Qualifiers] (
		[Qualifier_ID]
	)
GO

ALTER TABLE [dbo].[Qualifiers] ADD 
	CONSTRAINT [FK_Qualifiers_Qualifier_Types] FOREIGN KEY 
	(
		[Qualifier_Type_ID]
	) REFERENCES [dbo].[Qualifier_Types] (
		[Qualifier_Type_ID]
	) ON UPDATE CASCADE 
GO


ALTER TABLE [dbo].[System_Messages] ADD 
	CONSTRAINT [FK_System_Messages_Groups] FOREIGN KEY 
	(
		[Group_ID]
	) REFERENCES [dbo].[Groups] (
		[Group_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_System_Messages_Lab_Servers] FOREIGN KEY 
	(
		[Lab_Server_ID]
	) REFERENCES [dbo].[Lab_Servers] (
		[Lab_Server_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_System_Messages_Message_Types] FOREIGN KEY 
	(
		[Message_Type_ID]
	) REFERENCES [dbo].[Message_Types] (
		[Message_Type_ID]
	) ON UPDATE CASCADE 
GO



ALTER TABLE [dbo].[User_Sessions] ADD 
	CONSTRAINT [FK_User_Sessions_Groups] FOREIGN KEY 
	(
		[Effective_Group_ID]
	) REFERENCES [dbo].[Groups] (
		[Group_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_User_Sessions_Users] FOREIGN KEY 
	(
		[User_ID]
	) REFERENCES [dbo].[Users] (
		[User_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO


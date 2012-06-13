/*** ServiceBroker Tables ***/

-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_AdminURLs_ProcessAgent]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[AdminURLs] DROP CONSTRAINT FK_AdminURLs_ProcessAgent
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Grants_Groups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Grants] DROP CONSTRAINT FK_Grants_Groups
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Group_Hierarchy_Groups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Agent_Hierarchy] DROP CONSTRAINT FK_Group_Hierarchy_Groups
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Principals_Authentication_Types]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Principals] DROP CONSTRAINT FK_Principals_Authentication_Types
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Lab_Clients_Client_Types]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Lab_Clients] DROP CONSTRAINT FK_Lab_Clients_Client_Types
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Static_ProcessAgent_Coupons]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Static_ProcessAgents] DROP CONSTRAINT FK_Static_ProcessAgent_Coupons
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Tickets_Coupons]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Tickets] DROP CONSTRAINT FK_Tickets_Coupons
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Blobs_Access_Experiment_Blobs]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Blobs_Access] DROP CONSTRAINT FK_Blobs_Access_Experiment_Blobs
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ExperimentCoupon_Coupon]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ExperimentCoupon] DROP CONSTRAINT FK_ExperimentCoupon_Coupon
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ExperimentCoupon_Experiment]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ExperimentCoupon] DROP CONSTRAINT FK_ExperimentCoupon_Experiment
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Experiment_Blobs_Experiment_Records]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Experiment_Blobs] DROP CONSTRAINT FK_Experiment_Blobs_Experiment_Records
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Record_Attributes_Experiment_Records]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Record_Attributes] DROP CONSTRAINT FK_Record_Attributes_Experiment_Records
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Experiment_Blobs_Experiments]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Experiment_Blobs] DROP CONSTRAINT FK_Experiment_Blobs_Experiments
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Experiment_Records_Experiments]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Experiment_Records] DROP CONSTRAINT FK_Experiment_Records_Experiments
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Grants_Functions]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Grants] DROP CONSTRAINT FK_Grants_Functions
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Groups_Group_Types]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Groups] DROP CONSTRAINT FK_Groups_Group_Types
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Group_Hierarchy_Groups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Group_Hierarchy] DROP CONSTRAINT FK_Group_Hierarchy_Groups
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Experiments_ESS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Experiments] DROP CONSTRAINT FK_Experiments_ESS
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Experiments_LS]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Experiments] DROP CONSTRAINT FK_Experiments_LS
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Experiments_Clients]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Experiments] DROP CONSTRAINT FK_Experiments_Clients
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Experiments_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Experiments] DROP CONSTRAINT FK_Experiments_Users
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Experiments_Groups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Experiments] DROP CONSTRAINT FK_Experiments_Groups
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Groups_Groups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Groups] DROP CONSTRAINT FK_Groups_Groups
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_LabServer_To_Client_Agent]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Lab_Server_To_Client_Map] DROP CONSTRAINT FK_LabServer_To_Client_Agent
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_LabServer_To_Client_Client]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Lab_Server_To_Client_Map] DROP CONSTRAINT FK_LabServer_To_Client_Client
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_System_Messages_Groups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[System_Messages] DROP CONSTRAINT FK_System_Messages_Groups
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_User_Sessions_Groups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[User_Sessions] DROP CONSTRAINT FK_User_Sessions_Groups
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_SessionHistory_Session]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Session_History] DROP CONSTRAINT FK_SessionHistory_Session
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_SessionHistory_Group]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Session_History] DROP CONSTRAINT FK_SessionHistory_Group
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_SessionHistory_Client]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Session_History] DROP CONSTRAINT FK_SessionHistory_Client
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Client_Info_Lab_Clients]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Client_Info] DROP CONSTRAINT FK_Client_Info_Lab_Clients
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Client_Items_Lab_Clients]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Client_Items] DROP CONSTRAINT FK_Client_Items_Lab_Clients
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Client_Metadata_Lab_Clients]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Client_Metadata] DROP CONSTRAINT FK_Client_Metadata_Lab_Clients
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Lab_Server_To_Client_Map_Lab_Clients]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Lab_Server_To_Client_Map] DROP CONSTRAINT FK_Lab_Server_To_Client_Map_Lab_Clients
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Lab_Server_To_Client_Map_Lab_Servers]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Lab_Server_To_Client_Map] DROP CONSTRAINT FK_Lab_Server_To_Client_Map_Lab_Servers
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_System_Messages_Services]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[System_Messages] DROP CONSTRAINT FK_System_Messages_Services
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_System_Messages_Message_Types]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[System_Messages] DROP CONSTRAINT FK_System_Messages_Message_Types
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Static_ProcessAgent_ProcessAgent_Types]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Static_ProcessAgents] DROP CONSTRAINT FK_Static_ProcessAgent_ProcessAgent_Types
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


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Client_Items_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Client_Items] DROP CONSTRAINT FK_Client_Items_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Experiments_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Experiment_Information] DROP CONSTRAINT FK_Experiments_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Principals_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Principals] DROP CONSTRAINT FK_Principals_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_User_Sessions_Users]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[User_Sessions] DROP CONSTRAINT FK_User_Sessions_Users
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_User_Groups_User]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[User_Groups] DROP CONSTRAINT FK_User_Groups_User
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_User_Groups_Group]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[User_Groups] DROP CONSTRAINT FK_User_Groups_Group
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_User_Groups_Groups]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[User_Groups] DROP CONSTRAINT FK_FK_User_Groups_Groups
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_User_Groups_User]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[User_Groups] DROP CONSTRAINT FK_FK_User_Groups_User
GO
/*** DROP TABLES  ***/

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AdminURLs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[AdminURLs]
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

/****** Object:  Table [dbo].[Authority]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Authority]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Authority]
GO

/****** Object:  Table [dbo].[Blobs_Access]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Blobs_Access]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Blobs_Access]
GO

/****** Object:  Table [dbo].[Client_Info]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Client_Info]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Client_Info]
GO

/****** Object:  Table [dbo].[Client_Items]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Client_Items]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Client_Items]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Client_Metadata]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Client_Metadata]
GO

/****** Object:  Table [dbo].[Client_Types]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Client_Types]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Client_Types]
GO

/****** Object:  Table [dbo].[Experiment_Blobs]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Experiment_Blobs]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Experiment_Blobs]
GO

/****** Object:  Table [dbo].[Experiment_Information]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Experiment_Information]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Experiment_Information]
GO

/****** Object:  Table [dbo].[Experiment_Records]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Experiment_Records]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Experiment_Records]
GO

/****** Object:  Table [dbo].[ExperimentCoupon]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentCoupon]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ExperimentCoupon]
GO
/****** Object:  Table [dbo].[Experiments]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Experiments]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Experiments]
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

/****** Object:  Table [dbo].[Agent_Hierarchy]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Group_Hierarchy]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Group_Hierarchy]
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

/****** Object:  Table [dbo].[Lab_SSessions]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Lab_Sessions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Lab_Sessions]
GO

/****** Object:  Table [dbo].[Lab_Session_To_Experiment]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Lab_Session_To_Experiment]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Lab_Session_To_Experiment]
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

/****** Object:  Table [dbo].[Record_Attributes]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Registration]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Registration]
GO

/****** Object:  Table [dbo].[System_Messages]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[System_Messages]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[System_Messages]
GO

/****** Object:  Table [dbo].[User_Sessions]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[User_Sessions]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[User_Sessions]
GO
/****** Object:  Table [dbo].[Session_History]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Session_History]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Session_History]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[User_Groups]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[User_Groups]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 8/30/2005 4:07:53 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Users]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Users]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ResourceMappingValues_ResourceMappingKeys]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ResourceMappingValues] DROP CONSTRAINT FK_ResourceMappingValues_ResourceMappingKeys
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ResourceMappingKeys_ResourceMappingTypes]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ResourceMappingKeys] DROP CONSTRAINT FK_ResourceMappingKeys_ResourceMappingTypes
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_ResourceMappingValues_ResourceMappingTypes]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[ResourceMappingValues] DROP CONSTRAINT FK_ResourceMappingValues_ResourceMappingTypes
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ResourceMappingResourceTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ResourceMappingResourceTypes]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ResourceMappingKeys]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ResourceMappingKeys]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ResourceMappingStrings]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ResourceMappingStrings]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ResourceMappingTypes]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ResourceMappingTypes]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ResourceMappingValues]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ResourceMappingValues]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ResourceMap]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[ResourceMap]
GO

CREATE TABLE [dbo].[AdminURLs] (
	[id] [int] IDENTITY (1, 1) NOT NULL ,
	[ProcessAgentID] [int] NOT NULL ,
	[Ticket_Type_ID] [int] NOT NULL,
	[AdminURL] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL  
) ON [PRIMARY]
GO



/****** Object:  Table [dbo].[Agents]    Script Date: 8/30/2005 4:07:55 PM ****/
--CREATE TABLE [dbo].[Agents] (
--	[Agent_ID] [int] IDENTITY (2, 1) NOT NULL,
--	[Is_Group] [bit] NOT NULL,
--	[Date_Created] [datetime] NOT NULL DEFAULT GETUTCDATE(),
--	[Agent_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
--	
--) ON [PRIMARY]
--GO


/****** Object:  Table [dbo].[Authentication_Types]    Script Date: 8/30/2005 4:07:55 PM ******/
CREATE TABLE [dbo].[Authentication_Types] (
	[Auth_Type_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Auth_Name] [varchar] (100) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Description] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Authority](
	[Authority_ID] [int] IDENTITY(1,1) NOT NULL,
	[Auth_Type_ID] [int] NOT NULL,
	[Default_Group_ID] [int] NOT NULL,
	[Authority_Guid] [varchar](50) NOT NULL,
	[Authority_Name] [nvarchar](256) NOT NULL,
	[Authority_URL] [nvarchar](512) NOT NULL,
	[Pass_Phrase] [varchar](256) NULL,
	[Email_Proxy] [nvarchar](512) NULL,
	[Description] [nvarchar](512) NULL,
	[Contact_Email] [nvarchar](256) NULL,
	[Bug_Email] [nvarchar](256) NULL,
	[Location] [nvarchar](256) NULL,
) ON [PRIMARY]

GO


/****** Object:  Table [dbo].[Client_Info]    Script Date: 8/30/2005 4:07:55 PM ******/
CREATE TABLE [dbo].[Client_Info] (
	[Client_Info_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Client_ID] [int] NOT NULL ,
	[Display_Order] [int] NULL ,
	[Info_URL] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Info_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Description] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Client_Items]    Script Date: 8/30/2005 4:07:55 PM ******/
CREATE TABLE [dbo].[Client_Items] (
	[Client_Item_ID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[User_ID] [int] NOT NULL ,
	[Client_ID] [int] NOT NULL,
	[Item_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Item_Value] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Client_Metadata] (
	[Client_MD_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Client_ID] [int] NOT NULL,
	[Auth_Coupon_ID] [bigint] NOT NULL,
	[Modification_Time] [datetime] NOT NULL ,
	[SCO_Guid] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL, 
	[Metadata] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[SCO] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Metadata_Format] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Client_Types]    Script Date: 8/30/2005 4:07:56 PM ******/
CREATE TABLE [dbo].[Client_Types] (
	[Client_Type_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Description] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Experiments]    Script Date: 8/30/2005 4:07:56 PM ******/

CREATE TABLE [dbo].[Experiments] (
	[Experiment_ID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[LabSession_ID] [bigint] NOT NULL Default 0,
	[status] [int] NOT NULL ,
	[User_ID] [int] NOT NULL ,
	[Group_ID] [int] NOT NULL ,
	[Agent_ID] [int] NOT NULL ,
	[Client_ID] [int] NOT NULL ,
	[Record_Count] [int] NULL,
	[ESS_ID] [int] NULL ,
	[ScheduledStart] [datetime] NULL ,
	[Duration] [bigint] NULL ,
	[CreationTime] [datetime] NOT NULL ,	
	[CloseTime] [datetime] NULL ,
	[Annotation] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


/****** Object:  Table [dbo].[ExperimentCoupon]    Script Date: 12/12/2006 ******/

CREATE TABLE [dbo].[ExperimentCoupon] (
	[Experiment_ID] [bigint] NOT NULL ,
	[Coupon_ID] [bigint] NOT NULL
)
GO

/****** Object:  Table [dbo].[Functions]    Script Date: 8/30/2005 4:07:57 PM ******/
CREATE TABLE [dbo].[Functions] (
	[Function_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Function_Name] [varchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Description] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Grants]    Script Date: 8/30/2005 4:07:57 PM ******/
CREATE TABLE [dbo].[Grants] (
	[Agent_ID] [int] NOT NULL ,
	[Function_ID] [int] NOT NULL ,
	[Qualifier_ID] [int] NOT NULL ,
	[Date_Created] [datetime] NOT NULL ,
	[Grant_ID] [int] IDENTITY (1, 1) NOT NULL 
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Groups]    Script Date: 8/30/2005 4:07:57 PM ******/
CREATE TABLE [dbo].[Groups] (
	[Group_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Group_Type_ID] [int] NOT NULL ,
	[Associated_Group_ID] [int] NULL ,
	[AccessCode] [int] NOT NULL DEFAULT 0,
	[Date_Created] [datetime] NOT NULL DEFAULT GETUTCDATE(),
	[Group_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Email] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Description] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Group_Hierarchy] (
	[Group_ID] [int] NOT NULL ,
	[Parent_Group_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Group_Types]    Script Date: 8/30/2005 4:07:57 PM ******/
CREATE TABLE [dbo].[Group_Types] (
	[Group_Type_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Description] [varchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Lab_Clients]    Script Date: 8/30/2005 4:07:57 PM ******/
CREATE TABLE [dbo].[Lab_Clients] (
	[Client_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Client_Type_ID] [int] NOT NULL ,
	[NeedsScheduling] [bit] NOT NULL,
	[NeedsESS] [bit] NOT NULL,
	[IsReentrant] [bit] NOT NULL,
	[Date_Created] [datetime] NOT NULL,
	[Long_Description] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Notes] [ntext] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Client_Guid] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Lab_Client_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Version] [nvarchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Short_Description] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Loader_Script] [nvarchar](2000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Contact_Email] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Contact_First_Name] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Contact_Last_Name] [nvarchar] (128) COLLATE SQL_Latin1_General_CP1_CI_AS NULL,
	[Documentation_URL] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
	
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[Lab_Sessions] (
	[LabSession_ID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[Coupon_ID] [bigint] NOT NULL,
	[User_ID] [int] NOT NULL ,
	[Group_ID] [int] NOT NULL ,
	[Agent_ID] [int] NOT NULL ,
	[Client_ID] [int] NOT NULL ,
	[ScheduledStart] [datetime] NULL ,
	[Duration] [bigint] NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Lab_Session_To_Experiment](
	[LabSession_ID] [bigint],
	[experiment_id] [bigint]
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Lab_Server_To_Client_Map]    Script Date: 8/30/2005 4:07:58 PM ******/
CREATE TABLE [dbo].[Lab_Server_To_Client_Map] (
	[Client_ID] [int] NOT NULL ,
	[Agent_ID] [int] NOT NULL ,
	[Display_Order] [int] NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Message_Types]    Script Date: 8/30/2005 4:07:58 PM ******/
CREATE TABLE [dbo].[Message_Types] (
	[Message_Type_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Description] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Principals]    Script Date: 8/30/2005 4:07:58 PM ******/
CREATE TABLE [dbo].[Principals] (
	[Principal_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Auth_Type_ID] [int] NOT NULL ,
	[User_ID] [int] NOT NULL ,
	[Principal_String] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[Qualifier_Hierarchy]    Script Date: 8/30/2005 4:07:59 PM ******/
CREATE TABLE [dbo].[Qualifier_Hierarchy] (
	[Qualifier_ID] [int] NOT NULL ,
	[Parent_Qualifier_ID] [int] NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Qualifier_Types]    Script Date: 8/30/2005 4:07:59 PM ******/
CREATE TABLE [dbo].[Qualifier_Types] (
	[Qualifier_Type_ID] [int] NOT NULL ,
	[Description] [varchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Qualifiers]    Script Date: 8/30/2005 4:07:59 PM ******/
CREATE TABLE [dbo].[Qualifiers] (
	[Qualifier_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Qualifier_Type_ID] [int] NOT NULL ,
	[Qualifier_Reference_ID] [int] NOT NULL ,
	[Date_Created] [datetime] NOT NULL ,
	[Qualifier_Name] [nvarchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL
	
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Registration] (
	[record_ID] [int] IDENTITY (1,1) NOT NULL,
	[couponId] [int] NULL,
	[status] [int] NOT NULL,
	[createTime] [DATETIME] NOT NULL,
	[lastModTime] [DATETIME] NOT NULL,
	[descriptor] [NTEXT] NOT NULL,
	[registerGuid] [varchar](50) NOT NULL,
	[sourceGuid] [varchar](50) NOT NULL,
	[couponGuid] [varchar](50) NULL,
	[email] [nvarchar] (256) NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ResourceMappingKeys] (
	[Mapping_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[MappingKey_Type] [int] NOT NULL ,
	[MappingKey] [int] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ResourceMappingStrings] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[String_Value] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/* These may be set by the specific service broker */
CREATE TABLE [dbo].[ResourceMappingTypes] (
	[Type_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Type_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[Description] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[ResourceMappingValues] (
	[MappingValue_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Mapping_ID] [int] NOT NULL ,
	[MappingValue_Type] [int] NOT NULL, 
	[MappingValue] [int] NOT NULL
	
) ON [PRIMARY]
GO

/* Defined set of resource types */
CREATE TABLE [dbo].[ResourceMappingResourceTypes] (
	[ID] [int] IDENTITY (1, 1) NOT NULL ,
	[ResourceType_Value] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Session_History]    Script Date: 8/30/2005 4:08:00 PM ******/
CREATE TABLE [dbo].[Session_History] (
	[Session_ID] [bigint] NOT NULL ,
	[Modify_Time] [datetime] NOT NULL DEFAULT GETUTCDATE(),
	[Group_ID] [int] NOT NULL ,
	[Client_ID] [int] NOT NULL ,
	[Session_Key] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[System_Messages]    Script Date: 8/30/2005 4:07:59 PM ******/
CREATE TABLE [dbo].[System_Messages] (
	[System_Message_ID] [int] IDENTITY (1, 1) NOT NULL ,
	[Message_Type_ID] [int] NOT NULL ,
	[Agent_ID] [int] NULL ,
	[Client_ID] [int] NULL ,
	[Group_ID] [int] NULL ,
	[To_Be_Displayed] [bit] NOT NULL ,
	[Last_Modified] [datetime] NOT NULL ,
	[Date_Created] [datetime] NOT NULL ,
	[Message_Title] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Message_Body] [nvarchar] (3000) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL
	
) ON [PRIMARY]
GO


/****** Object:  Table [dbo].[User_Sessions]    Script Date: 8/30/2005 4:08:00 PM ******/
CREATE TABLE [dbo].[User_Sessions] (
	[Session_ID] [bigint] IDENTITY (1, 1) NOT NULL ,
	[Modify_Time] [datetime] NOT NULL , /* Deprecated */
	[User_ID] [int] NOT NULL ,
	[Effective_Group_ID] [int] NULL , /* Current group see also Session_History */
	[Client_ID] [int] NULL , /* Current client see also Session_History */
	[Session_Start_Time] [datetime] NOT NULL ,
	[Session_End_Time] [datetime] NULL ,
	[TZ_Offset] [int] NOT NULL CONSTRAINT [DF_User_Sessions_tz_Offset] DEFAULT (0),
	[Session_Key] [varchar] (512) COLLATE SQL_Latin1_General_CP1_CI_AS NULL /* Current key see also Session_History */
) ON [PRIMARY]
GO

/****** Object:  Table [dbo].[Users]    Script Date: 8/30/2005 4:08:00 PM ******/
CREATE TABLE [dbo].[Users] (
	[User_ID] [int]  IDENTITY (1, 1) NOT NULL ,
	[Lock_User] [bit] NOT NULL ,
	[Auth_ID] [int] NOT NULL Default 0,
	[Date_Created] [datetime] NOT NULL DEFAULT GETUTCDATE(),
	[Xml_Extension] [text] COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Password] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[User_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
	[First_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Last_Name] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Email] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Affiliation] [nvarchar] (256) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
	[Signup_Reason] [nvarchar] (2048) COLLATE SQL_Latin1_General_CP1_CI_AS NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[User_Groups](
	Group_ID int NOT NULL,
	User_ID int NOT NULL
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Authentication_Types] WITH NOCHECK ADD 
	CONSTRAINT [PK_Authentication_Types] PRIMARY KEY  CLUSTERED 
	(
		[Auth_Type_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Authority] WITH NOCHECK ADD 
	CONSTRAINT [PK_Authority] PRIMARY KEY  CLUSTERED 
	(
		[Authority_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Client_Info] WITH NOCHECK ADD 
	CONSTRAINT [PK_Client_Info] PRIMARY KEY  CLUSTERED 
	(
		[Client_Info_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Client_Items] WITH NOCHECK ADD 
	CONSTRAINT [PK_Client_Items] PRIMARY KEY  CLUSTERED 
	(
		[Client_Item_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Client_Metadata] WITH NOCHECK ADD 
	CONSTRAINT [PK_Client_Metadata] PRIMARY KEY  CLUSTERED 
	(
		[Client_MD_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Client_Types] WITH NOCHECK ADD 
	CONSTRAINT [PK_Client_Types] PRIMARY KEY  CLUSTERED 
	(
		[Client_Type_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Experiments] WITH NOCHECK ADD 
	CONSTRAINT [PK_Experiments] PRIMARY KEY  CLUSTERED 
	(
		[Experiment_ID]
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


ALTER TABLE [dbo].[Groups] WITH NOCHECK ADD 
	CONSTRAINT [PK_Groups] PRIMARY KEY  CLUSTERED 
	(
		[Group_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Group_Hierarchy] WITH NOCHECK ADD 
	CONSTRAINT [PK_Group_Hierarchy] PRIMARY KEY  CLUSTERED 
	(
		[Group_ID],
		[Parent_Group_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Group_Types] WITH NOCHECK ADD 
	CONSTRAINT [PK_Group_Types] PRIMARY KEY  CLUSTERED 
	(
		[Group_Type_ID]
	)  ON [PRIMARY] 
GO


ALTER TABLE [dbo].[Lab_Clients] WITH NOCHECK ADD 
	CONSTRAINT [PK_LabClients] PRIMARY KEY  CLUSTERED 
	(
		[Client_ID]
	)  ON [PRIMARY],
	CONSTRAINT [IX_Lab_Clients] UNIQUE  NONCLUSTERED 
	(
		[Client_Guid]
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


ALTER TABLE [dbo].[Registration] WITH NOCHECK ADD 
	CONSTRAINT [PK_Registration] PRIMARY KEY  CLUSTERED 
	(
		[record_ID]
	)  ON [PRIMARY] 
GO


ALTER TABLE [dbo].[System_Messages] WITH NOCHECK ADD 
	CONSTRAINT [PK_System_Messages] PRIMARY KEY  CLUSTERED 
	(
		[System_Message_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[ResourceMappingResourceTypes] WITH NOCHECK ADD 
	CONSTRAINT [PK_ResourceMappingResourceTypes] PRIMARY KEY  CLUSTERED 
	(
		[ID]
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


ALTER TABLE [dbo].[User_Groups] WITH NOCHECK ADD 
	CONSTRAINT [PK_User_Groups] PRIMARY KEY  CLUSTERED 
	(
		[Group_ID],
		[User_ID]
	)  ON [PRIMARY] 
GO


ALTER TABLE [dbo].[Groups] ADD 
	CONSTRAINT [IX_Groups] UNIQUE  NONCLUSTERED 
	(
		[Group_Name]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Experiments] ADD 
	CONSTRAINT [DF_Experiments_CreationTime] DEFAULT (getUtcDate()) FOR [CreationTime]
GO

ALTER TABLE [dbo].[Grants] ADD 
	CONSTRAINT [DF_Grants_Date_Created] DEFAULT (getUtcdate()) FOR [Date_Created],
	CONSTRAINT [IX_Grants] UNIQUE  NONCLUSTERED 
	(
		[Agent_ID],
		[Function_ID],
		[Qualifier_ID]
	)  ON [PRIMARY] 
GO


ALTER TABLE [dbo].[Lab_Clients] ADD
	CONSTRAINT [DF_NeedsScheduling] DEFAULT (0) FOR [NeedsScheduling],
	CONSTRAINT [DF_NeedsESS] DEFAULT (0) FOR [NeedsESS], 
	CONSTRAINT [DF_IsReentrant] DEFAULT (0) FOR [IsReentrant],  
	CONSTRAINT [DF_LabClients_Date_Created] DEFAULT (getUtcdate()) FOR [Date_Created]
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
	CONSTRAINT [DF_Qualifiers_Date_Created] DEFAULT (getUtcdate()) FOR [Date_Created],
	CONSTRAINT [IX_Qualifiers] UNIQUE  NONCLUSTERED 
	(
		[Qualifier_Type_ID],
		[Qualifier_Reference_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Registration] ADD 
	CONSTRAINT [DF_createTime] DEFAULT (getUtcdate()) FOR [createTime],
        CONSTRAINT [DF_lastModTime] DEFAULT (getUtcdate()) FOR [lastModTime]
GO

ALTER TABLE [dbo].[System_Messages] ADD 
	CONSTRAINT [DF_System_Messages_Last_Modified] DEFAULT (getUtcdate()) FOR [Last_Modified],
	CONSTRAINT [DF_System_Messages_Date_Created] DEFAULT (getUtcdate()) FOR [Date_Created]
GO



ALTER TABLE [dbo].[User_Sessions] ADD 
	CONSTRAINT [DF_User_Sessions_Login_Time] DEFAULT (getUtcdate()) FOR [Session_Start_Time]
GO


ALTER TABLE [dbo].[Users] ADD 
	CONSTRAINT [DF_Users_Lock_User] DEFAULT (0) FOR [Lock_User]
GO

ALTER TABLE [dbo].[Users] ADD 
	CONSTRAINT [IX_User_Name] UNIQUE  NONCLUSTERED 
	(
		[User_Name],
		[Auth_ID]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Group_Hierarchy] ADD 
	CONSTRAINT [FK_Group_Hierarchy_Group] FOREIGN KEY 
	(
		[Group_ID]
	) REFERENCES [dbo].[Groups] (
		[Group_ID]
	)ON DELETE CASCADE  ON UPDATE CASCADE,
	CONSTRAINT [FK_Group_Hierarchy_Parent] FOREIGN KEY 
	(
		[Parent_Group_ID]
	) REFERENCES [dbo].[Groups] (
		[Group_ID]
	)
GO

ALTER TABLE [dbo].[AdminURLs] ADD 
	CONSTRAINT [FK_AdminURLs_ProcessAgent] FOREIGN KEY 
	(
		[ProcessAgentID]
	) REFERENCES [dbo].[ProcessAgent] (
		[Agent_ID]
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

ALTER TABLE [dbo].[Client_Items] ADD 
	CONSTRAINT [FK_Client_Items_Lab_Clients] FOREIGN KEY 
	(
		[Client_ID]
	) REFERENCES [dbo].[Lab_Clients] (
		[Client_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Client_Items_Users] FOREIGN KEY 
	(
		[User_ID]
	) REFERENCES [dbo].[Users] (
		[User_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[Client_Metadata] ADD 
	CONSTRAINT [FK_Client_Metadata_Lab_Clients] FOREIGN KEY 
	(
		[Client_ID]
	) REFERENCES [dbo].[Lab_Clients] (
		[Client_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[Experiments] ADD 
	CONSTRAINT [FK_Experiments_Users] FOREIGN KEY 
	(
		[User_ID]
	) REFERENCES [dbo].[Users] (
		[User_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE,
	CONSTRAINT [FK_Experiments_Groups] FOREIGN KEY 
	(
		[Group_ID]
	) REFERENCES [dbo].[Groups] (
		[Group_ID]
	) ON DELETE NO ACTION  ON UPDATE NO ACTION ,
	CONSTRAINT [FK_Experiments_LS] FOREIGN KEY 
	(
		[Agent_ID]
	) REFERENCES [dbo].[ProcessAgent] (
		[Agent_ID]
	) ON DELETE NO ACTION  ON UPDATE NO ACTION ,
	CONSTRAINT [FK_Experiments_ESS] FOREIGN KEY 
	(
		[ESS_ID]
	) REFERENCES [dbo].[ProcessAgent] (
		[Agent_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_Experiments_Clients] FOREIGN KEY 
	(
		[Client_ID]
	) REFERENCES [dbo].[Lab_Clients] (
		[Client_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE
	 
GO

ALTER TABLE [dbo].[ExperimentCoupon] ADD 
	CONSTRAINT [FK_ExperimentCoupon_Experiment] FOREIGN KEY 
	(
		[Experiment_ID]
	) REFERENCES [dbo].[Experiments] (
		[Experiment_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE,
	CONSTRAINT [FK_ExperimentCoupon_Coupon] FOREIGN KEY 
	(
		[Coupon_ID]
	) REFERENCES [dbo].[IssuedCoupon] (
		[Coupon_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE
GO

ALTER TABLE [dbo].[Grants] ADD 
	CONSTRAINT [FK_Grants_Groups] FOREIGN KEY 
	(
		[Agent_ID]
	) REFERENCES [dbo].[Groups] (
		[Group_ID]
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
	CONSTRAINT [FK_LabServer_To_Client_Agent] FOREIGN KEY 
	(
		[Agent_ID]
	) REFERENCES [dbo].[ProcessAgent] (
		[Agent_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_LabServer_To_Client_Client] FOREIGN KEY 
	(
		[Client_ID]
	) REFERENCES [dbo].[Lab_Clients] (
		[Client_ID]
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
	CONSTRAINT [FK_System_Messages_ProcessAgent] FOREIGN KEY 
	(
		[Agent_ID]
	) REFERENCES [dbo].[ProcessAgent] (
		[agent_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_System_Messages_Message_Types] FOREIGN KEY 
	(
		[Message_Type_ID]
	) REFERENCES [dbo].[Message_Types] (
		[Message_Type_ID]
	) ON UPDATE CASCADE 
GO

ALTER TABLE [dbo].[User_Groups] ADD 
	CONSTRAINT [FK_User_Groups_Group] FOREIGN KEY 
	(
		[Group_ID]
	) REFERENCES [dbo].[Groups] (
		[Group_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE ,
	CONSTRAINT [FK_User_Groups_User] FOREIGN KEY 
	(
		[User_ID]
	) REFERENCES [dbo].[Users] (
		[User_ID]
	) ON DELETE CASCADE  ON UPDATE CASCADE 
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



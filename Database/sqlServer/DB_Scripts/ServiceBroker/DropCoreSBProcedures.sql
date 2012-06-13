-- Copyright (c) 2010 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

/****** Object:  Stored Procedure dbo.AddBlobToExperimentRecord    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IsSuperuser]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[IsSuperUser]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[getUserExperimentIDs]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[getUserExperimentIDs]
GO

/****** Object:  Stored Procedure dbo.AddBlobToExperimentRecord    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddBlobToExperimentRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddBlobToExperimentRecord]
GO

/****** Object:  Stored Procedure dbo.AddClientInfo    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddClientInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddClientInfo]
GO

/****** Object:  Stored Procedure dbo.AddExperimentRecord    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddExperimentRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddExperimentRecord]
GO

/****** Object:  Stored Procedure dbo.AddGrant    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddGrant]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddGrant]
GO

/****** Object:  Stored Procedure dbo.AddGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddGroup]
GO

/****** Object:  Stored Procedure dbo.AddLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddLabClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddLabClient]
GO

/****** Object:  Stored Procedure dbo.AddLabServerClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddLabServerClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddLabServerClient]
GO

/****** Object:  Stored Procedure dbo.AddMemberToGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddMemberToGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddMemberToGroup]
GO

/****** Object:  Stored Procedure dbo.AddQualifier    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddQualifier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddQualifier]
GO

/****** Object:  Stored Procedure dbo.AddQualifierHierarchy    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddQualifierHierarchy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddQualifierHierarchy]
GO

/****** Object:  Stored Procedure dbo.AddQualifier    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyQualifierName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyQualifierName]
GO

/****** Object:  Stored Procedure dbo.AddRecordAttribute    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddRecordAttribute]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddRecordAttribute]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.AddSBID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddSBID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddSBID]
GO

/****** Object:  Stored Procedure dbo.AddSystemMessage    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddSystemMessage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddSystemMessage]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.AddToAgentHierarchy    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddToAgentHierarchy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddToAgentHierarchy]
GO

/****** Object:  Stored Procedure dbo.AddUser    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddUser]
GO

/****** Object:  Stored Procedure dbo.AddUserLogin    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddUserLogin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddUserLogin]
GO

/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddUserSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddUserSession]
GO
/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyUserSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyUserSession]
GO
/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetSessionGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetSessionGroup]
GO
/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetSessionClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetSessionClient]
GO
/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetSessionKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetSessionKey]
GO
/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetSessionTzOffset]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetSessionTzOffset]
GO

/****** Object:  Stored Procedure dbo.CloseExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CloseExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CloseExperiment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CountServerClients]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CountServerClients]
GO

/****** Object:  Stored Procedure dbo.CreateBLOB    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreateBLOB]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreateBLOB]
GO

/****** Object:  Stored Procedure dbo.CreateExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreateExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreateExperiment]
GO

/****** Object:  Stored Procedure dbo.CreateExperimentIndex    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreateExperimentIndex]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreateExperimentIndex]
GO

/****** Object:  Stored Procedure dbo.GetEssInfoForExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetEssInfoForExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetEssInfoForExperiment]
GO

/****** Object:  Stored Procedure dbo.CreateNativePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreateNativePrincipal]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreateNativePrincipal]
GO

/****** Object:  Stored Procedure dbo.DeleteClientInfo    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteClientInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteClientInfo]
GO

/****** Object:  Stored Procedure dbo.DeleteClientItem    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteClientItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteClientItem]
GO

/****** Object:  Stored Procedure dbo.DeleteExperimentCoupon    Script Date: 12/12/2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteExperimentCoupon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteExperimentCoupon]
GO

/****** Object:  Stored Procedure dbo.InsertExperimentCoupon    Script Date: 12/12/2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateExperimentStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateExperimentStatus]
GO

/****** Object:  Stored Procedure dbo.InsertExperimentCoupon    Script Date: 12/12/2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateExperimentStatusCode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateExperimentStatusCode]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetExperimentStatusCode]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetExperimentStatusCode]
GO

/****** Object:  Stored Procedure dbo.InsertExperimentCoupon    Script Date: 12/12/2006 ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertExperimentCoupon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertExperimentCoupon]
GO
/****** Object:  Stored Procedure dbo.RetrieveExperimentCoupon    Script Date: 12/12/2006 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentCoupon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentCoupon]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentCouponID    Script Date: 12/12/2006 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentCouponID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentCouponID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentAdminInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentAdminInfo]
GO

/****** Object:  Stored Procedure dbo.DeleteExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteExperiment]
GO

/****** Object:  Stored Procedure dbo.DeleteExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteExperimentInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteExperimentInformation]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.DeleteExperimentInformation_rb    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteExperimentInformation_rb]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteExperimentInformation_rb]
GO

/****** Object:  Stored Procedure dbo.DeleteGrant    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteGrant]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteGrant]
GO

/****** Object:  Stored Procedure dbo.DeleteGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteGroup]
GO

/****** Object:  Stored Procedure dbo.DeleteLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteLabClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteLabClient]
GO

/****** Object:  Stored Procedure dbo.DeleteLabServerClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteLabServerClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteLabServerClient]
GO

/****** Object:  Stored Procedure dbo.DeleteMemberFromGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteMemberFromGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteMemberFromGroup]
GO

/****** Object:  Stored Procedure dbo.NUMERICClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[NUMERICClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[NUMERICClient]
GO

/****** Object:  Stored Procedure dbo.DeletePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeletePrincipal]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeletePrincipal]
GO

/****** Object:  Stored Procedure dbo.DeleteNativePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteNativePrincipal]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteNativePrincipal]
GO

/****** Object:  Stored Procedure dbo.DeleteQualifier    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteQualifier]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteQualifier]
GO

/****** Object:  Stored Procedure dbo.DeleteQualifierHierarchy    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteQualifierHierarchy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteQualifierHierarchy]
GO

/****** Object:  Stored Procedure dbo.DeleteRecordAttribute    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteRecordAttribute]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteRecordAttribute]
GO

/****** Object:  Stored Procedure dbo.DeleteSystemMessage    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteSystemMessage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteSystemMessage]
GO

/****** Object:  Stored Procedure dbo.DeleteSystemMessage    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteSystemMessageByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteSystemMessageByID]
GO

/****** Object:  Stored Procedure dbo.DeleteSystemMessages    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteSystemMessages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteSystemMessages]
GO

/****** Object:  Stored Procedure dbo.DeleteUser    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteUser]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.GetBlobAccess    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBlobAccess]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBlobAccess]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.GetBlobAssociation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBlobAssociation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBlobAssociation]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.GetBlobStorage    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetBlobStorage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetBlobStorage]
GO

/****** Object:  Stored Procedure dbo.GetExperimentIdleTime    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetCollectionCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetCollectionCount]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.GetExperimentIdleTime    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetExperimentIdleTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetExperimentIdleTime]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.FindExperiments    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FindExperiments]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[FindExperiments]
GO

/****** Object:  Stored Procedure dbo.ModifyExperimentOwner    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyExperimentOwner]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyExperimentOwner]
GO

/****** Object:  Stored Procedure dbo.ModifyExperimentStatus    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyExperimentStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyExperimentStatus]
GO

/****** Object:  Stored Procedure dbo.ModifyGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyGroup]
GO

/****** Object:  Stored Procedure dbo.ModifyLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyLabClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyLabClient]
GO

/****** Object:  Stored Procedure dbo.ModifySystemMessage    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifySystemMessage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifySystemMessage]
GO

/****** Object:  Stored Procedure dbo.ModifyUser    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyUser]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.ResetAgentHierarchy    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ResetAgentHierarchy]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ResetAgentHierarchy]
GO


/****** Object:  Stored Procedure dbo.RetrieveAgent    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAgent]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAgent]
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAgentGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAgentGroup]
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentHierarchyTable    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAgentHierarchyTable]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAgentHierarchyTable]
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentType    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAgentType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAgentType]
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentsTable    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAgentsTable]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAgentsTable]
GO

/****** Object:  Stored Procedure dbo.RetrieveAssociatedGroupID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAssociatedGroupID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAssociatedGroupID]
GO

/****** Object:  Stored Procedure dbo.RetrieveBlobAccess    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveBlobAccess]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveBlobAccess]
GO

/****** Object:  Stored Procedure dbo.RetrieveBlobAssociation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveBlobAssociation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveBlobAssociation]
GO

/****** Object:  Stored Procedure dbo.RetrieveBlobStorage    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveBlobStorage]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveBlobStorage]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.RetrieveExecutionError    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExecutionError]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExecutionError]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.RetrieveExecutionWarnings    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExecutionWarnings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExecutionWarnings]
GO

/****** Object:  Stored Procedure dbo.RetrieveBLOBsForExperimentRecord    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveBLOBsForExperimentRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveBLOBsForExperimentRecord]
GO

/****** Object:  Stored Procedure dbo.RetrieveClientInfo    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveClientInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveClientInfo]
GO

/****** Object:  Stored Procedure dbo.RetrieveClientItem    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveClientItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveClientItem]
GO

/****** Object:  Stored Procedure dbo.RetrieveClientItemNames    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveClientItemNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveClientItemNames]
GO

/****** Object:  Stored Procedure dbo.RetrieveClientServerIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveClientServerIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveClientServerIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperiment]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentRawData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentRawData]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentAnnotation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentAnnotation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentAnnotation]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentGroup]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentIdleTime    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentIdleTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentIdleTime]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentInfo]
GO
/****** Object:  Stored Procedure dbo.RetrieveExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentInformation]
GO


/****** Object:  Stored Procedure dbo.RetrieveAuthorizedExpIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAuthorizedExpIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInformat    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveActiveExpIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveActiveExpIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInformat    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAuthorizedExpIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAuthorizedExpIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInformat    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAuthorizedExpIDsCriteria]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAuthorizedExpIDsCriteria]
GO



/****** Object:  Stored Procedure dbo.RetrieveExperimentOwner    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentOwner]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentOwner]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentRecord    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentRecord]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentAdminInfos]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentAdminInfos]
GO
/****** Object:  Stored Procedure dbo.RetrieveExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperimentSummary]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperimentSummary]
GO
/****** Object:  Stored Procedure dbo.RetrieveGrantsTable    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGrantsTable]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGrantsTable]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroup]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUserGroups]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveUserGroups]
GO
/****** Object:  Stored Procedure dbo.RetrieveGroupAdminGroupID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupAdminGroupID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupAdminGroupID]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupExperimentIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupExperimentIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupExperimentIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveAdminGroupIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAdminGroupIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAdminGroupIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupID]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupName   Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupName]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupMembers    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupMembers]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupMembers]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupRequestGroupID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupRequestGroupID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupRequestGroupID]
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabClient]
GO
/****** Object:  Stored Procedure dbo.RetrieveLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabClientByGuid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabClientByGuid]
GO
/****** Object:  Stored Procedure dbo.RetrieveLabClientIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabClientID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabClientID]
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClientIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabClientIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabClientIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClientTypes    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabClientTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabClientTypes]
GO

/****** Object:  Stored Procedure dbo.RetrieveNativePassword    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveNativePassword]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveNativePassword]
GO

/****** Object:  Stored Procedure dbo.RetrieveNativePrincipals    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveNativePrincipals]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveNativePrincipals]
GO

/****** Object:  Stored Procedure dbo.RetrieveOrphanedUserIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveOrphanedUserIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveOrphanedUserIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveQualifierHierarchyTable    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveQualifierHierarchyTable]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveQualifierHierarchyTable]
GO

/****** Object:  Stored Procedure dbo.RetrieveQualifiersTable    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveQualifiersTable]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveQualifiersTable]
GO

/****** Object:  Stored Procedure dbo.RetrieveRecordAttributeByID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveRecordAttributeByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveRecordAttributeByID]
GO

/****** Object:  Stored Procedure dbo.RetrieveRecordAttributeByName    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveRecordAttributeByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveRecordAttributeByName]
GO

/****** Object:  Stored Procedure dbo.RetrieveRecordsForExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveRecordsForExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveRecordsForExperiment]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.RetrieveSBIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveSBIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveSBIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessageByID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveSystemMessageByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveSystemMessageByID]
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessageByIDForAdmin    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveSystemMessageByIDForAdmin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveSystemMessageByIDForAdmin]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveSystemMessagesForGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveSystemMessagesForGroup]
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessages    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveSystemMessages]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveSystemMessages]
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessagesForAdmin    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveSystemMessagesForAdmin]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveSystemMessagesForAdmin]
GO

/****** Object:  Stored Procedure dbo.RetrieveUser    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUser]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveUser]
GO

/****** Object:  Stored Procedure dbo.RetrieveUserEmail    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUserEmail]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveUserEmail]
GO

/****** Object:  Stored Procedure dbo.RetrieveUserID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUserID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveUserID]
GO

/****** Object:  Stored Procedure dbo.RetrieveUserName    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUserName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveUserName]
GO
/****** Object:  Stored Procedure dbo.RetrieveUserIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveUserIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveUserIDs]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.RetrieveValidationError    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveValidationError]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveValidationError]
GO

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.RetrieveValidationWarnings    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveValidationWarnings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveValidationWarnings]
GO

/****** Object:  Stored Procedure dbo.SaveBlobXMLExtensionSchemaURL    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveBlobXMLExtensionSchemaURL]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveBlobXMLExtensionSchemaURL]
GO

/****** Object:  Stored Procedure dbo.SaveClientItem    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveClientItem]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveClientItem]
GO

/****** Object:  Stored Procedure dbo.SaveExperimentAnnotation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveExperimentAnnotation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveExperimentAnnotation]
GO

/****** Object:  Stored Procedure dbo.SaveExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveExperimentInformation]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveExperimentInformation]
GO

/****** Object:  Stored Procedure dbo.SaveExperimentInformation    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateEssInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateEssInfo]
GO

/****** Object:  Stored Procedure dbo.SaveGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveGroup]
GO

/****** Object:  Stored Procedure dbo.SaveLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveLabClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveLabClient]
GO
/****** Object:  Stored Procedure dbo.SaveNativePassword    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveNativePassword]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveNativePassword]
GO

/****** Object:  Stored Procedure dbo.SaveNativePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveNativePrincipal]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveNativePrincipal]
GO

/****** Object:  Stored Procedure dbo.SaveResultXMLExtensionSchemaURL    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveResultXMLExtensionSchemaURL]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveResultXMLExtensionSchemaURL]
GO

/****** Object:  Stored Procedure dbo.SaveUserLogoutTime    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveUserLogoutTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveUserLogoutTime]
GO

/****** Object:  Stored Procedure dbo.SaveUserSessionEndTime    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveUserSessionEndTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveUserSessionEndTime]
GO

/****** Object:  Stored Procedure dbo.SelectSessionHistory    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectSessionHistory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectSessionHistory]
GO
/****** Object:  Stored Procedure dbo.SelectAllUserSessions    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectAllUserSessions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectAllUserSessions]
GO

/****** Object:  Stored Procedure dbo.SelectUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectUserSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectUserSession]
GO
/****** Object:  Stored Procedure dbo.SelectUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectSessionInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectSessionInfo]
GO
/****** Object:  Stored Procedure dbo.SelectUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateEssInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateEssInfo]
GO

/****** Object:  Stored Procedure dbo.GetAdminProcessAgentTags    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAdminProcessAgentTags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAdminProcessAgentTags]
GO

/****** Object:  Stored Procedure dbo.GetAdminProcessAgentTags    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAdminServiceTags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAdminServiceTags]
GO

/****** Object:  Stored Procedure dbo.GetProcessAgentAdminGrants    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetProcessAgentAdminGrants]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetProcessAgentAdminGrants]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteAdminURL]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteAdminURL]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteAdminURLbyID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteAdminURLbyID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertAdminURL]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertAdminURL]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyAdminURL]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyAdminURL]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyAdminUrlCodebase]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyAdminUrlCodebase]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyClientCodebase]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyClientCodebase]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveAdminURLs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveAdminURLs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceTypeStrings]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceTypeStrings]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddResourceMappingKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddResourceMappingKey]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddResourceMappingValue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddResourceMappingValue]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddResourceMappingString]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddResourceMappingString]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateResourceMappingString]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateResourceMappingString]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddResourceMappingResourceType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddResourceMappingResourceType]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceIDsByKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceIDsByKey]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceMappingByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceMappingByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceTypeString]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceTypeString]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceStringByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceStringByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceTypeByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceTypeByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceMappingIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceMappingIDs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetMappingIdByKeyValue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetMappingIdByKeyValue]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceMapIdsByValue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceMapIdsByValue]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetResourceTypeNames]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetResourceTypeNames]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetMappingStringTag]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetMappingStringTag]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertResourceMappingKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertResourceMappingKey]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertResourceMappingValue]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertResourceMappingValue]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteResourceMapping]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteResourceMapping]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertRegistration]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertRegistration]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectRegistrations]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectRegistrations]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectRegistrationRecord]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectRegistrationRecord]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectRegistration]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectRegistration]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectRegistrationByStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectRegistrationByStatus]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectRegistrationByRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectRegistrationByRange]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateRegistrationStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateRegistrationStatus]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLoaderScript]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetLoaderScript]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetLoaderScript]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetLoaderScript]
GO


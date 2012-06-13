-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

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

/****** Object:  Stored Procedure dbo.AddLabServer    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddLabServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddLabServer]
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

/****** Object:  Stored Procedure dbo.CreateNativePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreateNativePrincipal]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreateNativePrincipal]
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

/****** Object:  Stored Procedure dbo.DeleteLabServer    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteLabServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteLabServer]
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



/****** Object:  Stored Procedure dbo.ModifyGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyGroup]
GO

/****** Object:  Stored Procedure dbo.ModifyInPasskey    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyInPasskey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyInPasskey]
GO

/****** Object:  Stored Procedure dbo.ModifyLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyLabClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyLabClient]
GO

/****** Object:  Stored Procedure dbo.ModifyLabServer    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyLabServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyLabServer]
GO

/****** Object:  Stored Procedure dbo.ModifyOutPasskey    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyOutPasskey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyOutPasskey]
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

/*Old: Drop this procedure - do not create again*/
/****** Object:  Stored Procedure dbo.ResetDatabase    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ResetDatabase]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ResetDatabase]
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

/****** Object:  Stored Procedure dbo.RetrieveClientServerIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveClientServerIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveClientServerIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveExperiment    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveExperiment]
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


/****** Object:  Stored Procedure dbo.RetrieveGrantsTable    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGrantsTable]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGrantsTable]
GO

/****** Object:  Stored Procedure dbo.RetrieveGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroup]
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

/****** Object:  Stored Procedure dbo.RetrieveGroupID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveGroupID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveGroupID]
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

/****** Object:  Stored Procedure dbo.RetrieveLabClientIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabClientIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabClientIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClientTypes    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabClientTypes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabClientTypes]
GO

/****** Object:  Stored Procedure dbo.RetrieveLabServer    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabServer]
GO

/****** Object:  Stored Procedure dbo.RetrieveLabServerIDs    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabServerIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabServerIDs]
GO

/****** Object:  Stored Procedure dbo.RetrieveLabServerID    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabServerID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabServerID]
GO

/****** Object:  Stored Procedure dbo.RetrieveLabServerName    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RetrieveLabServerName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RetrieveLabServerName]
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

/****** Object:  Stored Procedure dbo.SaveGroup    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveGroup]
GO

/****** Object:  Stored Procedure dbo.SaveLabClient    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveLabClient]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveLabClient]
GO

/****** Object:  Stored Procedure dbo.SaveLabServer    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SaveLabServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SaveLabServer]
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

/****** Object:  Stored Procedure dbo.SelectAllUserSessions    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectAllUserSessions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectAllUserSessions]
GO

/****** Object:  Stored Procedure dbo.SelectInPasskey    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectInPasskey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectInPasskey]
GO

/****** Object:  Stored Procedure dbo.SelectOutPasskey    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectOutPasskey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectOutPasskey]
GO

/****** Object:  Stored Procedure dbo.SelectUserSession    Script Date: 5/18/2005 4:17:55 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectUserSession]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectUserSession]
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/* CREATE STORED PROCEDURES */



/****** Object:  Stored Procedure dbo.AddGrant    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddGrant
	@agentID numeric,
	@functionName varchar(50),
	@qualifierID numeric
AS
	DECLARE @functionID numeric
	
	select @functionID = (select function_id from functions where 
								upper(function_name)=upper(@functionName))
	insert into grants(agent_ID, function_ID,qualifier_ID)
	values (@agentID,@functionID,@qualifierID)

	select ident_current('grants')
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddGroup    Script Date: 5/18/2005 4:17:55 PM ******/
CREATE PROCEDURE AddGroup
	@groupName varchar(256),
	@description varchar(4000),
	@email varchar(50),
	@parentGroupID numeric, 
	@groupType varchar(256),
	@associatedGroupID numeric
AS
BEGIN TRANSACTION
	DECLARE @agentID numeric
	DECLARE @groupTypeID numeric

	select @groupTypeID = (select  group_type_id from group_types where description=@groupType);	
	
	insert into agents (agent_name, is_group)
	values (@groupName, 1)
	if (@@error > 0)
		goto on_error
	select @agentID=(select ident_current('Agents'))

	/* Assume that the parent group id here is a legal value. 
	Any corrections for -1 will be done in API code */
	insert into  agent_hierarchy (parent_group_ID, agent_ID)
	values (@parentGroupID, @agentID)
	if (@@error > 0)
		goto on_error
	
	insert into groups (group_id, group_name,description,email, group_type_id, associated_group_id)
	values (@agentID, @groupname, @description,@email, @groupTypeID, @associatedGroupID)
	if (@@error > 0)
		goto on_error

COMMIT TRANSACTION
	select @agentID
return
	on_error: 
	ROLLBACK TRANSACTION
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddLabClient    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddLabClient
	@labClientName varchar(256),
	@shortDescription varchar(256),
	@longDescription varchar(2000),
	@version varchar(50),
	@loaderScript varchar (2500),
	@clientType varchar (4000),
	@email varchar(50),
	@firstName varchar(128),
	@lastName varchar(128),
	@notes varchar(2000)
AS
		DECLARE @clientTypeID NUMERIC
		
		SELECT @clientTypeID = (SELECT client_type_id FROM client_types 
							WHERE upper(description) = upper(@clientType))
							
		INSERT INTO lab_clients (lab_client_name, short_description, long_description,version, 
						loader_script, client_type_ID, contact_email, contact_first_name, contact_last_name, notes)
		VALUES ( @labClientName, @shortDescription, @longDescription, @Version,
				@loaderScript,@clientTypeID, @email, @firstName, @lastName, @notes)
		
		SELECT ident_current('lab_clients');
RETURN
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddLabServer    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddLabServer
	@labServerName varchar(256),
	@labServerGUID varchar(50),
	@webServiceURL varchar (256),
	@labServerDescription varchar(4000),
	@labInfoURL varchar(256),
	@contactFirstName varchar(256),
	@contactLastName varchar(256),
	@contactEmail varchar(50)
AS
	insert into Lab_Servers (lab_server_name, GUID, web_service_URL,description,info_URL,
			 contact_first_name, contact_last_name, contact_email)
		values ( @labServerName, @labServerGUID, @webServiceURL, @labServerDescription, 
			@labInfoURL, @contactFirstName, @contactLastName, @contactEmail)
	
	select ident_current('lab_servers')
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddLabServerClient    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddLabServerClient
	@labClientID numeric,
	@labServerID numeric,
	@displayOrder int
AS
	insert into lab_server_to_client_map (client_id,lab_server_id, display_order)
	values (@labClientID, @labServerID, @displayOrder)
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddMemberToGroup    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddMemberToGroup
	@groupID numeric,
	@memberID numeric
AS
BEGIN TRANSACTION
	DECLARE @isGroup bit;
	DECLARE @orphanedGroupID numeric;
	DECLARE @newUserGroupID numeric;
	DECLARE @rootGroupID numeric

	select @isGroup = (select is_group from agents  where (agent_id=@memberID) );
	select @rootGroupID = (select group_id from groups where group_name = 'ROOT')

	begin
		if (@isGroup = 0)
		begin
			if (@groupID!=@rootGroupID) -- not trying to transfer to root
			begin 
				-- if agent is member of orphaned user group then delete them from it.
				select @orphanedGroupID = (select group_id from groups where group_name = 'OrphanedUserGroup');

				delete from agent_hierarchy 
				where agent_id= @memberID and parent_group_id = @orphanedGroupID;
				if (@@error > 0)
					goto on_error

				-- add to other group
				insert into agent_hierarchy (agent_id, parent_group_id)
				values (@memberID, @groupID);
				if (@@error > 0)
					goto on_error

			end
		end
		else 	/*If group then set qualifiier parents appropriately */
		begin
			DECLARE @groupQualifierID numeric;
			DECLARE @parentGroupQualifierID numeric;
			DECLARE @ECQualifierID numeric;
			DECLARE @parentECQualifierID numeric;

			DECLARE @rootQualifierID numeric;
			select @rootQualifierID = (select qualifier_id from Qualifiers where qualifier_name = 'ROOT');


			-- set group qualifiers
			select @groupQualifierID = (select qualifier_id from Qualifiers where Qualifier_reference_ID=@memberID 
							and Qualifier_Type_ID = (select Qualifier_Type_ID from Qualifier_Types where description='Group') );

			-- if added to root group then set parent qualifier to root	
			if (@groupID = @rootGroupID)
				select @parentGroupQualifierID = @rootQualifierID
			else
				select @parentGroupQualifierID = (select qualifier_id from Qualifiers where Qualifier_reference_ID=@groupID 
							and Qualifier_Type_ID = (select Qualifier_Type_ID from Qualifier_Types where description='Group') );

			insert into qualifier_hierarchy (qualifier_ID, parent_qualifier_ID)
			values (@groupQualifierID, @parentGroupQualifierID);
			if (@@error > 0)
				goto on_error
			
			-- set experiment qualifiers
			select @ECQualifierID = (select qualifier_id from Qualifiers where Qualifier_reference_ID=@memberID 
							and Qualifier_Type_ID = (select Qualifier_Type_ID from Qualifier_Types where description='Experiment Collection') );
						
			-- if added to root group then set experiment collection qualifier parent to root	
			if (@groupID = @rootGroupID)
				select @parentECQualifierID = @rootQualifierID
			else
				select @parentECQualifierID = (select qualifier_id from Qualifiers where Qualifier_reference_ID=@groupID 
					and Qualifier_Type_ID = (select Qualifier_Type_ID from Qualifier_Types where description='Experiment Collection') );
			
			insert into qualifier_hierarchy (qualifier_ID, parent_qualifier_ID)
			values (@ECQualifierID, @parentECQualifierID);
			if (@@error > 0)
				goto on_error
			
			insert into  agent_hierarchy (parent_group_ID, agent_ID)
			values (@groupID, @memberID);
			if (@@error > 0)
				goto on_error
		-- This is an insert and  NOT an update because agents can belong to multiple groups
		end
	end

COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddQualifier    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddQualifier
	@qualifierTypeID numeric,
	@qualifierReferenceID numeric,
	@qualifierName varchar (256)
AS
	/*DECLARE @qualifierTypeID numeric
	select @qualifierTypeID = (select qualifier_type_id from qualifier_Types where 
				upper(description) = upper(@qualifierType))*/
	insert into qualifiers(qualifier_Type_id, qualifier_reference_id, qualifier_name)
	values (@qualifierTypeID,@qualifierReferenceID, @qualifierName)
	
	select ident_current('qualifiers')
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddQualifierHierarchy    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddQualifierHierarchy
	@parentQualifierID numeric,
	@qualifierID numeric
AS
	
	insert into  qualifier_hierarchy (parent_qualifier_ID, qualifier_ID)
	values (@parentQualifierID, @qualifierID)
-- This is an insert and  NOT an update because qualifiers can have multiple parents
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



/****** Object:  Stored Procedure dbo.AddSystemMessage    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddSystemMessage
	@messageType varchar(256),
	@messageTitle varchar(256),
	@toBeDisplayed bit,
	@groupID numeric,
	@labServerID numeric,
	@messageBody text 

AS 
	DECLARE @messageTypeID numeric

	select @messageTypeID = (select message_type_id from message_types 
							where upper(description) = upper(@messageType))
	insert into System_Messages (message_type_id, to_be_displayed, group_id, lab_server_id,
								message_body, message_title)
		values (@messageTypeID,  @toBeDisplayed, @groupID,@labServerID, @messageBody, @messageTitle)
	
	select ident_current('system_messages')
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddUser    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddUser
	@userName varchar(50),
	@firstName varchar(256),
	@lastName varchar(256),
	@email varchar(50),
	@affiliation varchar(256),
	@reason varchar(4000),
	@XMLExtension text,
	@lockUser bit,
	@principalString varchar (50),
	@authenType varchar(256),
	@initialGroupID numeric
AS
	DECLARE @authTypeID numeric
	DECLARE @agentID numeric
	--DECLARE @parentGroupID numeric
	DECLARE @userID numeric

BEGIN TRANSACTION
	
	begin
		insert into agents (agent_name, is_group)
		values (@userName, 0)
		if (@@error > 0)
			goto on_error
		select @agentID=(select ident_current('Agents'))

		--select @parentGroupID = (select group_id from groups 
		--						where upper(group_name)=upper(@initialGroupID))
		insert into  agent_hierarchy (parent_group_ID, agent_ID)
		values (@initialGroupID, @agentID)
		if (@@error > 0)
			goto on_error
	
		insert into users (user_id, user_name,first_name,last_name,email, 
							affiliation, signup_reason, XML_Extension, lock_user)
		values (@agentID, @userName, @firstName,  @lastName, @email, 
				@affiliation, @reason, @XMLExtension, @lockUser)
		if (@@error > 0)
			goto on_error
		
		select @authTypeID = (select auth_type_id from authentication_types 
							where upper(description)=upper(@authenType))
		insert into principals (user_id, principal_string, auth_type_ID)
		values (@agentID,@principalString, @authTypeID)
		if (@@error > 0)
			goto on_error
	end
COMMIT TRANSACTION	
		
		select @agentID
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.AddUserSession    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE AddUserSession
	@userID numeric,
	@groupID numeric,
	@sessionKey varchar(512)
AS 
	insert into user_sessions (user_id, effective_group_id, session_key)
		values (@userID, @groupID,@sessionKey )
	
	select ident_current('user_sessions')
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO



/****** Object:  Stored Procedure dbo.CreateNativePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE CreateNativePrincipal
	@userName varchar(50)
AS
	DECLARE @authTypeID numeric
	DECLARE @userID numeric
	
	select @authTypeID=(select auth_type_id from authentication_types where
					upper(description) = 'NATIVE')	
	select @userID = (select user_ID from users where user_name=@userName)
	
	-- since no principal string is specified in authen api, inserting the username as principal string
	insert into Principals (user_ID, auth_type_id, principal_string)
	values (@userID,@authTypeID, @userName)
	
	select @userID
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteGrant    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteGrant
	@grantID numeric
AS
	delete from Grants
	where Grant_ID = @grantID;
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteGroup    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteGroup
	@groupID numeric
AS
BEGIN TRANSACTION
	delete from Agents 
	where agent_id = @groupID
	if (@@error > 0)
		goto on_error

	delete from Agent_Hierarchy
	where Parent_Group_ID = @groupID;
	if (@@error > 0)
		goto on_error
-- AgentId is taken care of by referential integrity
	
	delete from Groups
	where Group_ID = @groupID;
	if (@@error > 0)
		goto on_error
		
	delete from Qualifiers
	where Qualifier_Reference_ID = @groupID and 
		Qualifier_Type_ID = (select Qualifier_Type_ID from Qualifier_Types where description='Group')
	if (@@error > 0)
		goto on_error
		
	delete from Qualifiers
	where Qualifier_Reference_ID = @groupID and 
		Qualifier_Type_ID = (select Qualifier_Type_ID from Qualifier_Types where description='Experiment Collection')
		
COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteLabClient    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteLabClient
	@labClientID numeric
AS
BEGIN TRANSACTION
	delete from Lab_Clients
	where Client_ID = @labClientID;
	if (@@error > 0)
		goto on_error
	
	delete from Qualifiers
	where qualifier_reference_ID = @labClientID and 
	qualifier_type_ID = (select qualifier_type_ID from qualifier_Types 
				where description = 'Lab Client');
	if (@@error > 0)
		goto on_error
COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteLabServer    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteLabServer
	@labServerID numeric
AS
BEGIN TRANSACTION

	delete from Lab_Servers
	where Lab_Server_ID = @labServerID;
	if (@@error > 0)
		goto on_error

	delete from Qualifiers
	where qualifier_reference_ID = @labServerID and 
	qualifier_type_ID = (select qualifier_type_ID from qualifier_Types 
				where description = 'Lab Server');
	if (@@error > 0)
		goto on_error

COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteLabServerClient    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteLabServerClient
	@labClientID numeric
AS
	delete from lab_server_to_client_map
	where client_id = @labClientID
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteMemberFromGroup    Script Date: 5/18/2005 4:17:55 PM ******/
CREATE PROCEDURE DeleteMemberFromGroup
	@groupID numeric,
	@memberID numeric
AS
BEGIN TRANSACTION
	DECLARE @isGroup bit;
	select @isGroup = (select is_group from agents
				 where (agent_id=@memberID) )

	DECLARE @rootGroupID numeric
	select @rootGroupID = (select group_id from groups where group_name = 'ROOT')
	
	if (@isGroup = 0) /* if user */
	begin
		/* Get Orphaned user group ID */
		DECLARE @orphanedGroupID numeric;
		select @orphanedGroupID = (select group_id from groups where group_name = 'OrphanedUserGroup');
		
		/* If user already belongs to the Orphaned Users Group - delete from system */
		if (@groupID=@orphanedGroupID)
		begin
			delete from users where user_id=@memberID;
			if (@@error > 0)
				goto on_error

			delete from agents where agent_id=@memberID;
			if (@@error > 0)
				goto on_error

		end
		else
			/*check parents of member*/
			if ((select count( parent_group_id) from agent_hierarchy where agent_id=@memberID)>1)
				/* multiple parents*/
				delete from agent_hierarchy where agent_id=@memberID and parent_group_id=@groupID
			else
			begin
				/* if single parent */

				-- single parent cannot be root
				if (@groupID != @rootGroupID)
					update agent_hierarchy 
					set parent_group_ID = @orphanedGroupID
					where parent_group_ID = @groupID and  agent_ID = @memberID;
			end
	end
	else
	if (@isGroup = 1) /* Group */
	begin
		DECLARE @rootQualifierID numeric;
		select @rootQualifierID = (select qualifier_id from Qualifiers where qualifier_name = 'ROOT');

		/* get group qualifier */
		DECLARE @qualifierID numeric;
		select @qualifierID = (select qualifier_id from Qualifiers where qualifier_reference_id=@memberID 
					and qualifier_type_id  = (select Qualifier_Type_ID from Qualifier_Types where description='Group'))
		
		
		/* get experiment collection qualifier */
		DECLARE @experimentCollectionQualifierID numeric;
		select @experimentCollectionqualifierID = (select qualifier_id from Qualifiers where qualifier_reference_id=@memberID 
					and qualifier_type_id  = (select Qualifier_Type_ID from Qualifier_Types where description='Experiment Collection'))

		DECLARE @parentQualifierID numeric;
		DECLARE @parentECQualifierID numeric;

		-- if being removed from root
		if (@groupID = @rootGroupID)
		begin
			select @parentQualifierID = @rootQualifierID;
			select @parentECQualifierID = @rootQualifierID;
		end
		else
		begin
			/* get parent qualifier */
			select @parentQualifierID = (select qualifier_id from Qualifiers where qualifier_reference_id=@groupID 
				and qualifier_type_id  = (select Qualifier_Type_ID from Qualifier_Types where description='Group'))

			/* get parent experiment collection qualifier */
			select @parentECQualifierID = (select qualifier_id from Qualifiers where qualifier_reference_id=@groupID 
				and qualifier_type_id  = (select Qualifier_Type_ID from Qualifier_Types where description='Experiment Collection'))
		end
		

		/*check parents of agent*/
		if ((select count( parent_group_id) from agent_hierarchy where agent_id=@memberID)>1)
		
		/* multiple parents - delete relationships*/
		begin
			delete from agent_hierarchy where agent_id=@memberID and parent_group_id=@groupID
			if (@@error > 0)
				goto on_error
				
			delete from qualifier_hierarchy where qualifier_id = @qualifierID and parent_qualifier_id=@parentQualifierID
			if (@@error > 0)
				goto on_error
				
			delete from qualifier_hierarchy where qualifier_id = @experimentCollectionQualifierID and parent_qualifier_id=@parentECQualifierID
			if (@@error > 0)
				goto on_error
		end
		else
		/* single parent - move all to ROOT */
		-- single parent cannot be root
		if (@groupID != @rootGroupID)
		begin
			update agent_hierarchy 
			set parent_group_ID = @rootGroupID
			where parent_group_ID = @groupID and  agent_ID = @memberID;
			if (@@error > 0)
				goto on_error
				
			update qualifier_hierarchy
			set parent_qualifier_ID = @rootQualifierID
			where parent_qualifier_ID = @parentQualifierID and qualifier_ID=@qualifierID
			if (@@error > 0)
				goto on_error
				
			update qualifier_hierarchy
			set parent_qualifier_ID = @rootQualifierID
			where parent_qualifier_ID = @parentECQualifierID and qualifier_ID=@experimentCollectionQualifierID
			if (@@error > 0)
				goto on_error
		end
	end
COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.DeleteNativePrincipal    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteNativePrincipal
	@userID numeric
AS
	DECLARE @NativeTypeID numeric;
	
	select @NativeTypeID = (select auth_type_id from Authentication_Types where upper(description)='NATIVE')
	
	delete from Principals
	where user_ID = @userID and auth_type_id=@NativeTypeID;
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteQualifier    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteQualifier
	@qualifierID numeric
AS
	delete from Qualifiers
	where Qualifier_ID = @qualifierID;
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteQualifierHierarchy    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteQualifierHierarchy
	@parentQualifierID numeric,
	@qualifierID numeric
AS
	delete from  qualifier_hierarchy 
	where parent_qualifier_ID = @parentQualifierID and qualifier_ID = @qualifierID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.DeleteSystemMessageByID    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteSystemMessageByID
	@messageID numeric
AS
	delete from System_Messages
	where System_Message_ID = @messageID;
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteSystemMessages    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteSystemMessages
	@messageType varchar(256),
	@groupID numeric,
	@labServerID numeric

/* Delete by message type & group/lab server */
AS
	DECLARE @messageTypeID numeric

	select @messageTypeID = (select message_type_id from message_types 
							where upper(description) = upper(@messageType))
							
	delete from System_Messages
	where message_type_id = @messageTypeID and group_ID = @groupID and lab_server_ID=@labServerID;

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.DeleteUser    Script Date: 5/18/2005 4:17:55 PM ******/

CREATE PROCEDURE DeleteUser
	@userID numeric
AS
BEGIN TRANSACTION
	DECLARE @userName varchar(50)
	DECLARE @qualifierTypeID numeric
	DECLARE @agentID numeric
	
	select @agentID = (select agent_ID from Agents where agent_id = @userID)
	select @userName = (select user_name from Users where user_ID = @userID)
	delete from Agents 
	where agent_name = @userName
	if (@@error > 0)
		goto on_error
		
	/* Must update this logic if a qualifier type user is added */
	select @qualifierTypeID = (select qualifier_type_id from qualifier_types	
										where description = 'Agent')
	delete from qualifiers
	where qualifier_reference_ID = @agentID and qualifier_type_id=@qualifierTypeID
	if (@@error > 0)
		goto on_error
		
	delete from users
	where user_ID = @userID
	if (@@error > 0)
		goto on_error
COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifyGroup    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE ModifyGroup
	@groupID numeric,
	@groupName varchar(256),
	@description varchar(4000),
	@email varchar(50)
AS
BEGIN TRANSACTION
	DECLARE @oldGroupName varchar(256)
	DECLARE @qualifierTypeID1 numeric
	DECLARE @qualifierTypeID2 numeric
	
	begin
		select @oldGroupName = (select group_name from Groups where group_id=@groupID)
		if (@oldGroupName <> @groupName)
		begin
			update agents
			set agent_name = @groupName
			where agent_name = @oldGroupName;
			if (@@error > 0)
				goto on_error
				
			select @qualifierTypeID1 = (select qualifier_type_id from qualifier_types	
										where description = 'Agent')
			select @qualifierTypeID2 = (select qualifier_type_id from qualifier_types	
								where description = 'Group')
								
			update qualifiers
			set qualifier_name = @groupName
			where qualifier_reference_id = @groupID and qualifier_name = @oldGroupName 
					and (qualifier_type_id = @qualifierTypeID1 or qualifier_type_id = @qualifierTypeID2)
			if (@@error > 0)
				goto on_error
		end
	
		update groups 
		set group_name = @groupName, description = @description, email=@email
		where group_id = @groupID
		if (@@error > 0)
			goto on_error
	end
COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifyInPasskey    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE ModifyInPasskey
	@lsID numeric,
	@passKey varchar(256)
AS
	begin
		update lab_servers
		set incoming_passkey = @passKey
		where lab_server_id = @lsID
	end
	
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifyLabClient    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE ModifyLabClient
	@labClientID numeric,
	@labClientName varchar(256),
	@shortDescription varchar(256),
	@longDescription varchar(2000),
	@version varchar(50),
	@loaderScript varchar (2500),
	@clientType varchar (4000),
	@email varchar(50),
	@firstName varchar(128),
	@lastName varchar(128),
	@notes varchar(2000)
AS
		DECLARE @clientTypeID NUMERIC
		
		SELECT @clientTypeID = (SELECT client_type_id FROM client_types 
							WHERE upper(description) = upper(@clientType))
							
		UPDATE Lab_Clients
		SET lab_client_name = @labClientName, short_description=@shortDescription, 
			long_description=@longDescription, version = @version, 
			loader_script = @loaderScript, client_type_id=@clientTypeID, contact_email = @email, 
			contact_first_name=@firstName, contact_last_name=@lastName, notes=@notes
		WHERE client_ID = @labClientID;
RETURN
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifyLabServer    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE ModifyLabServer
	@labServerID numeric,
	@labServerName varchar(256),
	@labServerGUID varchar(50),
	@webServiceURL varchar (256),
	@labServerDescription varchar(4000),
	@labInfoURL varchar(256),
	@contactFirstName varchar(256),
	@contactLastName varchar(256),
	@contactEmail varchar(50)
AS
		update Lab_Servers
		set lab_server_name = @labServerName, GUID=@labServerGUID, 
		web_service_URL = @webServiceURL, description=@labServerDescription, 
		info_URL = @labInfoURL, contact_first_name=@contactFirstName, 	
		contact_last_name=@contactLastName, contact_email = @contactEmail
		where lab_server_ID = @labServerID;
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifyOutPasskey    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE ModifyOutPasskey
	@lsID numeric,
	@passKey varchar(256)
AS
	begin
		update lab_servers
		set outgoing_passkey = @passKey
		where lab_server_id = @lsID
	end
	
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifySystemMessage    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE ModifySystemMessage
	@messageID	numeric,
	@messageType varchar(256),
	@messageTitle varchar(256),
	@toBeDisplayed bit,
	@groupID numeric,
	@labServerID numeric,
	@messageBody text 
AS
	DECLARE @messageTypeID numeric

	select @messageTypeID = (select message_type_id from message_types 
							where upper(description) = upper(@messageType))
							
	update System_Messages
	set message_type_id = @messageTypeID, to_be_displayed=@toBeDisplayed, 
	group_ID = @groupID, lab_server_id=@labServerID, message_title = @messageTitle,
	message_body = @messageBody, last_modified = getdate()
	where system_message_ID = @messageID;

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ModifyUser    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE ModifyUser
	@userID numeric,
	@userName varchar(50),
	@firstName varchar(256),
	@lastName varchar(256),
	@email varchar(50),
	@affiliation varchar(256),
	@reason varchar(4000),
	@XMLExtension text,
	@lockUser bit,
	@principalString varchar (50),
	@authenType varchar(256)
AS
BEGIN TRANSACTION
	DECLARE @authTypeID numeric
	DECLARE @oldUserName varchar(50)
	DECLARE @qualifierTypeID numeric
		
	begin
		select @oldUserName = (select user_name from Users where user_id=@userID)
		if (@oldUserName != @userName)
		begin
			update agents
			set agent_name = @userName
			where agent_name = @oldUserName;
			if (@@error > 0)
				goto on_error
				
		/* Must update this logic if a qualifier type user is added */
			select @qualifierTypeID = (select qualifier_type_id from qualifier_types	
										where description = 'Agent')
			update qualifiers
			set qualifier_name = @userName
			where qualifier_reference_id = @userID and qualifier_name = @oldUserName 
					and qualifier_type_id = @qualifierTypeID
			if (@@error > 0)
				goto on_error
		end
		
		update users
		set user_name=@userName, first_name = @firstName, last_name=@lastName, 
			affiliation = @affiliation,  signup_reason=@reason, 
			XML_Extension = @XMLExtension, email = @email, lock_user=@lockUser
		where user_ID = @userID;
		if (@@error > 0)
			goto on_error
	
		select @authTypeID = (select auth_type_id from authentication_types where upper(description)=upper(@authenType))		
		update Principals 
		set principal_string=@principalString, auth_type_id = @authTypeID
		where user_id = @userID;
		if (@@error > 0)
			goto on_error
	end
COMMIT TRANSACTION	
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveAgent    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAgent
	@agentID numeric
AS
	select agent_ID, is_group
	from   agents
	where agent_ID = @agentID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentGroup    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAgentGroup
	@agentID numeric
AS
	select parent_group_ID 
	from   agent_hierarchy
	where agent_ID = @agentID --and (parent_group_id!= (select group_id from groups where group_name='ROOT'))
	-- don't select root as a group. root is parent for everyone!
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentHierarchyTable    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAgentHierarchyTable
AS
	select agent_id,parent_group_id
	from agent_hierarchy
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentType    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAgentType
	@agentID numeric
AS
	select  is_group
	from  agents
	where (agent_id=@agentID)
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveAgentsTable    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAgentsTable
-- Procedure was added by Karim - need this for constructing the tree control
AS
	select agent_id, is_group
	from agents
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveAssociatedGroupID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveAssociatedGroupID
	@groupID numeric
AS
	select associated_group_ID
	from groups
	where group_id = @groupID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO




/****** Object:  Stored Procedure dbo.RetrieveClientInfo    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveClientInfo
	@labClientID numeric
AS
	select client_info_id, info_URL, info_name, display_order, description
	from client_info
	where client_id=@labClientID
	order by display_order
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveClientItem    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveClientItem
	@clientID numeric,
	@userID numeric,
	@itemName varchar(256)
AS
	select item_value 
	from client_items
	where client_id = @ClientID and user_ID=@userID and item_Name = @itemName;
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveClientItemNames    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveClientItemNames
	@clientID numeric,
	@userID numeric
AS
	select item_name
	 from client_items
	where client_id = @ClientID and user_ID=@userID;
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveClientServerIDs    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveClientServerIDs
	@labClientID numeric
AS
	select lab_server_id
	from lab_server_to_client_map
	where client_id=@labClientID
	order by display_order
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGrantsTable    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGrantsTable
AS
	select agent_id, f.function_name, qualifier_id, grant_id
	from grants g, functions f
	where g.function_id =f.function_id
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroup    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGroup
	@groupID numeric
AS
	select  group_name, g.description AS description, email, gt.description AS group_type, date_created
	from groups g, group_types gt
	where group_ID= @groupID and g.group_type_id=gt.group_type_id
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupExperimentIDs    Script Date: 5/18/2005 4:17:56 PM ******/
CREATE PROCEDURE RetrieveGroupExperimentIDs
	@groupID numeric
AS
	select Experiment_ID
	from Experiment_Information
	where Effective_Group_ID = @groupID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupIDs    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGroupIDs
AS
	select group_id
	from groups
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGroupID
	@groupName varchar(256)
AS
	select group_ID
	from groups
	where group_name = @groupName
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupMembers    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGroupMembers
	@groupID numeric
AS
	select ah.agent_ID, ag.is_group, ag.agent_name
	from   agent_hierarchy ah, agents ag
	where ah.parent_group_ID = @groupID and ah.agent_id=ag.agent_id
	order by agent_name
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupRequestGroupID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGroupRequestGroupID
	@groupID numeric
AS
	DECLARE @requestGroupType numeric
	
	select @requestGroupType = (Select group_type_id from group_types where description
								 = 'Request Group');
	select group_ID from groups
	where associated_group_id = @groupID and group_type_id = @requestGroupType
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveGroupAdminGroupID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveGroupAdminGroupID
	@groupID numeric
AS
	DECLARE @adminGroupType numeric
	
	select @adminGroupType = (Select group_type_id from group_types where description
								 = 'Course Staff Group');
	select group_ID from groups
	where associated_group_id = @groupID and group_type_id = @adminGroupType
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClient    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveLabClient
	@labClientID numeric
AS
	select lab_client_name, short_description, long_description, version, loader_script, 
		contact_email, contact_first_name, contact_last_name, notes, date_created,
		client_types.description
	from lab_clients, client_types
	where client_id = @labClientID and lab_clients.client_type_id = client_types.client_type_id
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClientIDs    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveLabClientIDs
AS
	select client_id
	from lab_clients
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLabClientTypes    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveLabClientTypes
AS
	select description
	from client_types
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveLabServer    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveLabServer
	@labServerID numeric
AS
	select lab_server_name, GUID, web_service_URL, description, info_URL, 
	contact_first_name, contact_last_name, contact_email, date_created
	from lab_servers
	where lab_server_id = @labServerID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUserID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveLabServerID
	@GUID varchar(50)
AS
	select lab_server_ID
	from lab_servers
	where GUID = @GUID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.RetrieveLabServerIDs    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveLabServerIDs
AS
	select lab_server_id
	from lab_servers
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveNativePassword    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveNativePassword
	@userID numeric
AS
	select password
	from users
	where user_ID =@userID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveNativePrincipals    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveNativePrincipals
AS
	select user_ID
	from   principals 
	where auth_type_id=(select auth_type_id from authentication_types where
					upper(description) = 'NATIVE')
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveOrphanedUserIDs    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveOrphanedUserIDs
AS
	DECLARE @orphanedGroupID numeric
	
	select @orphanedGroupID = (select group_ID from Groups where group_name = 'OrphanedUserGroup')
	
	select user_id
	from users, agent_hierarchy ah, agents
	where ah.parent_group_id=@orphanedGroupID and ah.agent_id=agents.agent_id and agents.agent_name = users.user_name
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveQualifierHierarchyTable    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveQualifierHierarchyTable
AS
	select qualifier_id, parent_qualifier_id
	from qualifier_hierarchy
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveQualifiersTable    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveQualifiersTable
AS
	select qualifier_id, qualifier_Reference_ID, qualifier_Type_ID, qualifier_name, date_created
	from qualifiers
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.RetrieveSystemMessageByID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveSystemMessageByID
	@messageID numeric
/*Retrieve by message ID*/
AS
	select  message_body, to_be_displayed, last_modified, group_id, 
			lab_server_id,message_title, description
	from system_messages sm, message_types mt
	where sm.system_message_ID= @messageID and to_be_Displayed=1
		and sm.message_type_id=mt.message_type_id
	
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessageByIDForAdmin    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveSystemMessageByIDForAdmin
	@messageID numeric
/*Retrieve by message ID for admin pages (all messages)*/
AS
	select  message_body, to_be_displayed, last_modified, group_id, 
			lab_server_id,message_title, description
	from system_messages sm, message_types mt
	where sm.system_message_ID= @messageID and sm.message_type_id=mt.message_type_id
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessages    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveSystemMessages
/*Retrieves by message type and group */
	@messageType varchar(256),
	@groupID numeric,
	@labServerID numeric
AS
	DECLARE @messageTypeID numeric
	
	select @messageTypeID = (select message_type_id from message_types 
						where upper(description) = upper(@messageType))
	
	select system_message_ID, message_body, to_be_displayed, last_modified, message_title
	from system_messages sm
	where sm.message_type_id=@messageTypeID and to_be_displayed =1 
			and group_ID=@groupID and lab_server_ID=@labServerID
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveSystemMessagesForAdmin    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveSystemMessagesForAdmin
/*Retrieves by message type and group */
	@messageType varchar(256),
	@groupID numeric,
	@labServerID numeric
AS
	DECLARE @messageTypeID numeric
	
	select @messageTypeID = (select message_type_id from message_types 
						where upper(description) = upper(@messageType))
	
	select system_message_ID, message_body, to_be_displayed, last_modified, message_title
	from system_messages sm
	where sm.message_type_id=@messageTypeID	and group_ID=@groupID and lab_server_ID=@labServerID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUser    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveUser
	@userID numeric
AS
	select user_name, first_name, last_name, affiliation, XML_extension, signup_reason, 
			email, date_created, lock_user, password
	from users u 
	where u.user_id = @userID 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUserEmail    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveUserEmail
	@userName varchar(50)
AS
	select email
	from users
	where user_name = @userName
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUserID    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveUserID
	@userName varchar(50)
AS
	select user_ID
	from users
	where user_name = @userName
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveUserIDs    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE RetrieveUserIDs
AS
	select user_id
	from users
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SaveBlobXMLExtensionSchemaURL    Script Date: 5/18/2005 4:17:56 PM ******/

CREATE PROCEDURE SaveBlobXMLExtensionSchemaURL
 	@labserverID varchar(256),
	@URL varchar(256)
AS
/* hardcoded account ID*/
	update accounts
	set blob_XML_Extension_schema_URL = @URL
	where account_ID = 2
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SaveNativePassword    Script Date: 5/18/2005 4:17:57 PM ******/

CREATE PROCEDURE SaveNativePassword
	@userID numeric,
	@password varchar(256)
AS
BEGIN TRANSACTION
	update users
	set password = @password
	where user_id =@userID
	IF (@@ERROR <> 0) goto on_error
COMMIT TRANSACTION
return
	on_error: 
	ROLLBACK TRANSACTION
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SaveResultXMLExtensionSchemaURL    Script Date: 5/18/2005 4:17:57 PM ******/
/* This procedure is probably not being used anywhere */
CREATE PROCEDURE SaveResultXMLExtensionSchemaURL
 	@labserverID numeric,
	@URL varchar(256)
AS
/* hardcoded account ID*/
	update accounts
	set result_XML_Extension_schema_URL = @URL
	where account_ID = 2
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SaveUserSessionEndTime    Script Date: 5/18/2005 4:17:57 PM ******/

CREATE PROCEDURE SaveUserSessionEndTime
	@sessionID numeric

AS 
	update user_sessions set session_end_time = getdate()
	where session_id=@sessionID
	
	select session_end_time from user_sessions where session_ID = @sessionID
	return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SelectAllUserSessions    Script Date: 5/18/2005 4:17:57 PM ******/

CREATE PROCEDURE SelectAllUserSessions
	@userID numeric,
	@groupID numeric,
	@TimeBefore DateTime,
	@TimeAfter DateTime

AS 
	select session_ID, session_start_time, session_end_time, effective_group_ID, session_key
	from user_sessions 
	where user_ID=@userID
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SelectInPasskey    Script Date: 5/18/2005 4:17:57 PM ******/

CREATE PROCEDURE SelectInPasskey
	@lsID numeric
AS
	begin
		select incoming_passkey
		from  lab_servers
		where lab_server_id = @lsID
	end
	
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SelectOutPasskey    Script Date: 5/18/2005 4:17:57 PM ******/

CREATE PROCEDURE SelectOutPasskey
	@lsID numeric
AS
	begin
		select outgoing_passkey
		from  lab_servers
		where lab_server_id = @lsID
	end
	
return
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.SelectUserSession    Script Date: 5/18/2005 4:17:57 PM ******/

CREATE PROCEDURE SelectUserSession
	@sessionID numeric
AS 
	
	select user_id, effective_group_id, session_start_time, session_end_time, session_key
	 from user_sessions where session_ID = @sessionID
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CredentialSet_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CredentialSet_Add]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CredentialSet_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CredentialSet_Delete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CredentialSet_GetID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CredentialSet_GetID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CredentialSet_Modify]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CredentialSet_Modify]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CredentialSet_ModifyServiceBroker]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CredentialSet_ModifyServiceBroker]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CredentialSet_Remove]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CredentialSet_Remove]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CredentialSet_RetrieveByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CredentialSet_RetrieveByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CredentialSet_RetrieveIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CredentialSet_RetrieveIDs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CredentialSets_RetrieveByLS]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CredentialSets_RetrieveByLS]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentInfo_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ExperimentInfo_Add]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentInfo_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ExperimentInfo_Delete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentInfo_Modify]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ExperimentInfo_Modify]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentInfo_ModifyCore]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ExperimentInfo_ModifyCore]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentInfo_ModifyLabServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ExperimentInfo_ModifyLabServer]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentInfo_Retrieve]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ExperimentInfo_Retrieve]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentInfo_RetrieveByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ExperimentInfo_RetrieveByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentInfo_RetrieveIDByExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ExperimentInfo_RetrieveIDByExperiment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentInfo_RetrieveIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ExperimentInfo_RetrieveIDs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ExperimentInfo_RetrieveIDsByLabServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ExperimentInfo_RetrieveIDsByLabServer]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IsPermittedExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[IsPermittedExperiment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[IsPermittedGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[IsPermittedGroup]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LSSPolicy_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LSSPolicy_Add]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LSSPolicy_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LSSPolicy_Delete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LSSPolicy_Modify]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LSSPolicy_Modify]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LSSPolicy_RetrieveByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LSSPolicy_RetrieveByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LSSPolicy_RetrieveIDsByExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LSSPolicy_RetrieveIDsByExperiment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[LabServer_RetrieveName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[LabServer_RetrieveName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PermittedExperiment_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PermittedExperiment_Add]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PermittedExperiment_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PermittedExperiment_Delete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PermittedExperiment_RetrieveByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PermittedExperiment_RetrieveByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PermittedExperiment_RetrieveID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PermittedExperiment_RetrieveID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PermittedExperiment_RetrieveIDByRecur]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PermittedExperiment_RetrieveIDByRecur]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PermittedExperiment_RetrieveIDsByRecurrence]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PermittedExperiment_RetrieveIDsByRecurrence]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PermittedExperiment_RetrieveIDsForRecurrence]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PermittedExperiment_RetrieveIDsForRecurrence]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PermittedGroup_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PermittedGroup_Add]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PermittedGroup_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PermittedGroup_Delete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PermittedGroup_RetrieveIDByRecur]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PermittedGroup_RetrieveIDByRecur]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[PermittedGroup_RetrieveIDsForRecurrence]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[PermittedGroup_RetrieveIDsForRecurrence]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_Add]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_Delete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_Get]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_Get]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_GetIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_GetIDs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_RetrieveByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_RetrieveByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_RetrieveIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_RetrieveIDs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_RetrieveIDsByResourceAndTime]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_RetrieveIDsByResourceAndTime]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrences_Retrieve]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrences_Retrieve]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reservation_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reservation_Add]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Reservation_GetTimes]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Reservation_GetTimes]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReservationData_Retrieve]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReservationData_Retrieve]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReservationInfo_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReservationInfo_Add]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReservationInfo_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReservationInfo_Delete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReservationInfo_DeleteByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReservationInfo_DeleteByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReservationInfo_RetrieveByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReservationInfo_RetrieveByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReservationInfo_RetrieveIDByLabServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReservationInfo_RetrieveIDByLabServer]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReservationInfo_RetrieveIDByResource]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReservationInfo_RetrieveIDByResource]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReservationInfo_RetrieveIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReservationInfo_RetrieveIDs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReservationInfo_RetrieveIDsByExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReservationInfo_RetrieveIDsByExperiment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReservationTags_Retrieve]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReservationTags_Retrieve]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ReservationTags_RetrieveByLabServer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ReservationTags_RetrieveByLabServer]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Resource_AddGetID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Resource_AddGetID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Resource_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Resource_Delete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Resource_Get]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Resource_Get]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Resource_GetByGuid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Resource_GetByGuid]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Resource_GetTags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Resource_GetTags]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Resource_GetTagsByGuid]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Resource_GetTagsByGuid]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Resource_Insert]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Resource_Insert]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Resource_SetDescription]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Resource_SetDescription]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USSInfo_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USSInfo_Add]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USSInfo_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USSInfo_Delete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USSInfo_Modify]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USSInfo_Modify]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USSInfo_RetrieveByGUID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USSInfo_RetrieveByGUID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USSInfo_RetrieveByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USSInfo_RetrieveByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USSInfo_RetrieveID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USSInfo_RetrieveID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[USSInfo_RetrieveIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[USSInfo_RetrieveIDs]
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


-- DROP FUNCTIONS

/****** Object:  User Defined Function dbo.GetReservationIDs    Script Date: 4/25/2011 1:41:06 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetReservationIDs]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[GetReservationIDs]
GO

/****** Object:  User Defined Function dbo.GetReservationIDs    Script Date: 4/25/2011 1:41:06 PM ******/
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetReservationIDs_ByIDs]') and xtype in (N'FN', N'IF', N'TF'))
drop function [dbo].[GetReservationIDs_ByIDs]
GO


-- CREATE FUNCTIONS

CREATE Function [dbo].[GetReservationIDs]
(
@sbGuid varchar (50) = null,
@groupName nvarchar(256) = null,
@labServerGuid varchar(50) = null,
@clientGuid varchar(50) = null,
@resourceID int = null,
@start datetime = '1753-01-01 00:00:00',
@end datetime = '9999-12-31 00:00:00'
)
RETURNS @resIds TABLE(id int)
AS
BEGIN

declare @target int

DECLARE @credIds TABLE ( cID int )
DECLARE @expIds TABLE ( eID int )
--DECLARE @resIds TABLE {id int)
set @target = 0

if (@sbGuid is not null) and ( LEN(@sbGuid) > 0)
set @target = 1
if (@groupName is not null ) and (LEN(@groupName) > 0)
set @target = @target | 2
if (@resourceID is not null ) AND (@ResourceID > 0)
set @target = @target | 4
if (@labServerGuid is not null)  AND (LEN(@labServerGuid) > 0)
set @target = @target | 8
if (@clientGuid is not null)  AND (LEN(@clientGuid) > 0)
set @target = @target | 16

if(@target & 3 = 3)
BEGIN
INSERT INTO  @credIds select credential_Set_id from Credential_Sets where Group_Name = @groupName and Service_Broker_Guid = @sbGuid
END
else if @target & 3 = 1
BEGIN
INSERT INTO  @credIds select credential_Set_id from Credential_Sets where Service_Broker_Guid = @sbGuid
END
else if @target & 3 = 2
BEGIN
INSERT INTO  @credIds select Credential_Sets.Credential_Set_ID from Credential_Sets where Group_Name = @groupName
END

if @target & 24 = 24
BEGIN
insert into @expIds select Experiment_Info_ID from Experiment_Info where Lab_Server_GUID = @labServerGuid and Lab_Client_GUID = @clientGuid
END
else if @target & 24 = 8
BEGIN
insert into @expIds select Experiment_Info.Experiment_Info_ID from Experiment_Info where Lab_Server_GUID = @labServerGuid
END
else if @target & 24 = 16
BEGIN
insert into @expIds select Experiment_Info_ID from Experiment_Info where Lab_Client_GUID = @clientGuid
END

if(@target = 0) -- no Filelds
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end
else if( @target = 4 ) -- ResourceID only
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Resource_ID = @resourceID
else if( @target = 31) -- All Fields
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Resource_ID = @resourceID and r.Credential_Set_ID in (select * from @credIds) and r.Experiment_Info_ID in (select * from @expIds)
else if (@target & 3 > 0) AND (@target & 24 >0) AND (@target & 4 = 4)
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end
	and r.Resource_ID = @resourceID and r.Credential_Set_ID in (select * from @credIds) and r.Experiment_Info_ID in (select * from @expIds)
else if (@target & 3 > 0) AND (@target & 24 >0) AND (@target & 4 = 0)
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Credential_Set_ID in (select * from @credIds) and r.Experiment_Info_ID in (select * from @expIds)
else if (@target & 3 > 0) AND (@target & 24 = 0) AND (@target & 4 = 4)
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Resource_ID = @resourceID and r.Credential_Set_ID in (select * from @credIds)
else if (@target & 3 > 0) AND (@target & 24 = 0) AND (@target & 4 = 0)
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Credential_Set_ID in (select * from @credIds)
else if (@target & 3 = 0) AND (@target & 24 >0) AND (@target & 4 = 4)
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Resource_ID = @resourceID and r.Experiment_Info_ID in (select * from @expIds)
else if (@target & 3 = 0) AND (@target & 24 >0) AND (@target & 4 = 0)
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Experiment_Info_ID in (select * from @expIds)

--DROP TABLE #credIds
--DROP TABLE #expIds
RETURN
END
GO

CREATE Function [dbo].[GetReservationIDs_ByIDs]
(
@resourceID int = 0,
@expID int = 0,
@credID int = 0,
@start datetime = '1753-01-01 00:00:00',
@end datetime = '9999-12-31 00:00:00'
)
RETURNS @resIds TABLE(id int)
AS
BEGIN

declare @target int

set @target = 0

if (@resourceID is not null) and ( @resourceID > 0)
set @target = 1
if (@expID is not null ) and (@expID > 0)
set @target = @target | 2
if (@credID is not null ) AND (@credID > 0)
set @target = @target | 4

if(@target = 0) -- no Filelds
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end
else if( @target = 1 ) -- ResourceID only
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Resource_ID = @resourceID
else if( @target = 2 ) 
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Experiment_Info_ID = @expID
else if( @target = 3 ) 
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Resource_ID = @resourceID and r.Experiment_Info_ID = @expID
else if( @target = 4) -- All Fields
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Credential_Set_ID = @credId
else if( @target = 5) -- All Fields
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Resource_ID = @resourceID and r.Credential_Set_ID = @credId
	
else if( @target = 6) -- All Fields
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Credential_Set_ID = @credId and r.Experiment_Info_ID = @expId
else if( @target = 7) -- All Fields
	insert into @resIds select  r.Reservation_Info_ID
	from Reservation_Info r
	where  r.End_Time>@start and r.Start_Time<@end 
	and r.Resource_ID = @resourceID and r.Credential_Set_ID = @credId and r.Experiment_Info_ID = @expId

RETURN
END
GO


-- CREATE PROCEDURES

/*** Start of Procedures ***/

CREATE PROCEDURE CredentialSet_Add

@serviceBrokerGUID varchar(50),
@serviceBrokerName nvarchar(256),
@groupName nvarchar(256)

AS

insert into Credential_Sets(Service_Broker_GUID,Service_Broker_Name, Group_Name) 
values (@serviceBrokerGUID,@serviceBrokerName,@groupName)
select ident_current('Credential_Sets')


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE CredentialSet_Delete

@credentialSetID int

AS

delete from Credential_Sets where Credential_Set_ID=@credentialSetID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE CredentialSet_GetID

@serviceBrokerGUID varchar(50),
@groupName nvarchar(256)

AS
select Credential_Set_ID from Credential_Sets
where Service_Broker_GUID = @serviceBrokerGuid AND Group_Name = @groupName
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE CredentialSet_Modify
@credentialSetID int,
@serviceBrokerGUID varchar(50),
@serviceBrokerName nvarchar(256),
@groupName nvarchar(256)

 AS

update Credential_Sets set Service_Broker_GUID=@serviceBrokerGUID, Service_Broker_Name=@serviceBrokerName, 
Group_Name=@groupName
where Credential_Set_ID=@credentialSetID
select @@rowcount

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE CredentialSet_ModifyServiceBroker
@originalGUID varchar(50),
@serviceBrokerGUID varchar(50),
@serviceBrokerName nvarchar(256)

 AS

update Credential_Sets set Service_Broker_GUID=@serviceBrokerGUID, Service_Broker_Name=@serviceBrokerName 
where Service_Broker_GUID = @originalGUID
select @@rowcount

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE CredentialSet_Remove

@serviceBrokerGUID varchar(50),
@serviceBrokerName nvarchar(256),
@groupName nvarchar(256)

AS

delete  from Credential_Sets 
where Service_Broker_GUID=@serviceBrokerGUID and Service_Broker_Name=@serviceBrokerName 
and Group_Name=@groupName

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE CredentialSet_RetrieveByID
@credentialSetID int

AS

select Service_Broker_GUID, Service_Broker_Name, Group_Name from Credential_Sets 
where Credential_Set_ID=@credentialSetID

return


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE CredentialSet_RetrieveIDs

 AS

select Credential_Set_ID from Credential_Sets
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE CredentialSets_RetrieveByLS
@lsGuid varchar (50)

AS

select Credential_Set_ID, Service_Broker_GUID, Service_Broker_Name, Group_Name from Credential_Sets 
where Credential_Set_ID in 
(select distinct Credential_set_id from Permitted_Groups where Recurrence_ID in
	(select Recurrence_ID from Recurrence where resource_ID in 
		(Select resource_ID from LS_Resources where Lab_server_Guid = @lsGuid)
	)
)
order by Group_Name
return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE ExperimentInfo_Add 
@labClientGUID varchar(50),
@labServerGUID varchar(50),
@labServerName nvarchar(256),
@labClientVersion nvarchar(50),
@labClientName nvarchar(256),
@providerName nvarchar(256),
@contactEmail nvarchar(256),
@prepareTime int,
@recoverTime int,
@minimumTime int,
@earlyArriveTime int

AS

insert into Experiment_Info(Lab_Client_GUID,Lab_Server_GUID, Lab_Server_Name, Lab_Client_Version, Lab_Client_Name,
 Provider_Name, Contact_Email, Prepare_Time, Recover_Time, Minimum_Time, Early_Arrive_Time) 
values (@labClientGuid, @labServerGUID, @labServerName, @labClientVersion,@labClientName, 
 @providerName, @contactEmail, @prepareTime, @recoverTime, @minimumTime,@earlyArriveTime)
select ident_current('Experiment_Info')


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE ExperimentInfo_Delete 

@experimentInfoID int

AS

delete from Experiment_Info where Experiment_Info_ID= @experimentInfoID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ExperimentInfo_Modify

@experimentInfoID int,
@labClientGUID varchar(50),
@labServerGUID varchar(50),
@labServerName nvarchar(256),
@labClientVersion nvarchar(50),
@labClientName nvarchar(256),
@providerName nvarchar(256),
@contactEmail nvarchar(256),
@prepareTime int,
@recoverTime int,
@minimumTime int,
@earlyArriveTime int

 AS

update Experiment_Info set Lab_Client_GUID=@labClientGUID,Lab_Server_GUID=@labServerGUID, 
Lab_Server_Name=@labServerName, Lab_Client_Version=@labClientVersion,Lab_Client_Name=@labClientName, 
Provider_Name=@providerName, Contact_Email=@contactEmail,
Prepare_Time=@prepareTime, Recover_Time=@recoverTime, 
Minimum_Time=@minimumTime, Early_Arrive_Time=@earlyArriveTime 
where Experiment_Info_ID=@experimentInfoID

select @@rowcount
RETURN
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO



CREATE PROCEDURE ExperimentInfo_ModifyCore

@experimentInfoID int,
@labClientGUID varchar(50),
@labServerGUID varchar(50),
@labServerName nvarchar(256),
@labClientVersion nvarchar(50),
@labClientName nvarchar(256),
@providerName nvarchar(256)

 AS

update Experiment_Info set Lab_Client_GUID=@labClientGUID,Lab_Server_GUID=@labServerGUID, 
Lab_Server_Name=@labServerName, Lab_Client_Version=@labClientVersion,Lab_Client_Name=@labClientName, 
Provider_Name=@providerName
where Experiment_Info_ID=@experimentInfoID

select @@rowcount

RETURN
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ExperimentInfo_ModifyLabServer
@originalGUID varchar(50),
@labServerGUID varchar(50),
@labServerName nvarchar(256)

 AS

update Experiment_Info set Lab_Server_GUID=@labServerGUID, Lab_Server_Name=@labServerName
where Lab_Server_GUID = @originalGuid
select @@rowcount
RETURN
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ExperimentInfo_Retrieve 
@experimentInfoID int

AS

select Lab_Client_GUID, Lab_Server_GUID, Lab_Server_Name, Lab_Client_Version, Lab_Client_Name,
Provider_Name, Contact_Email, Prepare_Time, Recover_Time, Minimum_Time, Early_Arrive_Time 
from Experiment_Info where Experiment_Info_ID=@experimentInfoID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE ExperimentInfo_RetrieveByID 
@experimentInfoID int

AS

select Lab_Client_GUID, Lab_Server_GUID, Lab_Server_Name, Lab_Client_Version, Lab_Client_Name,
Provider_Name, Contact_Email, Prepare_Time, Recover_Time, Minimum_Time, Early_Arrive_Time 
from Experiment_Info where Experiment_Info_ID=@experimentInfoID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ExperimentInfo_RetrieveIDByExperiment

@clientGuid varchar(50),
@labServerGuid varchar(50)

AS

select Experiment_Info_ID from Experiment_Info 
where Lab_Client_Guid=@clientGuid and Lab_Server_Guid=@labServerGuid
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE ExperimentInfo_RetrieveIDs
 AS

select Experiment_Info_ID from Experiment_Info


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ExperimentInfo_RetrieveIDsByLabServer

@labServerGUID varchar(50)

AS

select Experiment_Info_ID from Experiment_info where Lab_Server_GUID = @labServerGUID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



CREATE PROCEDURE IsPermittedExperiment 

@experimentID int,
@recurrenceID int

AS

if (select count(experiment_Info_ID) from Permitted_Experiments 
where Experiment_Info_ID= @experimentID and  Recurrence_ID = @recurrenceID) > 0
return 1
else
return 0


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



CREATE PROCEDURE IsPermittedGroup 

@groupID int,
@recurrenceID int

AS

if (select count(Credential_Set_ID) from Permitted_Groupss 
where Credential_Seto_ID= @groupID and  Recurrence_ID = @recurrenceID) > 0
return 1
else
return 0


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE LSSPolicy_Add 

@rule varchar(2048),
@experimentInfoID int,
@credentialSetID int

AS

insert into LSS_Policy([Rule], Experiment_Info_ID, Credential_Set_ID) values (@rule, @experimentInfoID,@credentialSetID)
select ident_current('LSS_Policy')


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE LSSPolicy_Delete

@lssPolicyID int

AS

delete from LSS_Policy where LSS_Policy_ID = @lssPolicyID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE LSSPolicy_Modify

@lssPolicyID int,
@credentialSetID int,
@experimentInfoID int,
@rule varchar(1024)

AS

update LSS_Policy set Credential_Set_ID = @credentialSetID,Experiment_Info_ID=@experimentInfoID, [Rule]=@rule where LSS_Policy_ID= @lssPolicyID

select @@rowcount

return
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE LSSPolicy_RetrieveByID
@lssPolicyID int
 
AS

select [Rule], Experiment_Info_ID, Credential_Set_ID from LSS_Policy where LSS_Policy_ID=@lssPolicyID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE LSSPolicy_RetrieveIDsByExperiment
@experimentInfoID int
AS

select LSS_Policy_ID from LSS_Policy where Experiment_Info_ID=@experimentInfoID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE LabServer_RetrieveName

@labServerGUID varchar(50)

AS

select DISTINCT Lab_Server_Name from Experiment_Info where Lab_Server_GUID = @labServerGUID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE PermittedExperiment_Add

@experimentInfoID int,
@recurrenceID int

AS

insert into Permitted_Experiments(Experiment_Info_ID, Recurrence_ID) values (@experimentInfoID, @recurrenceID)
select ident_current('Permitted_Experiments')
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE PermittedExperiment_Delete 

@experimentID int,
@recurrenceID int

AS

delete from Permitted_Experiments 
where Experiment_Info_ID= @experimentID
and  Recurrence_ID = @recurrenceID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE PermittedExperiment_RetrieveByID 

@permittedExperimentID int

AS

select Experiment_Info_ID, Recurrence_ID from Permitted_Experiments where Permitted_Experiment_ID=@permittedExperimentID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE PermittedExperiment_RetrieveID

@experimentInfoID int,
@timeBlockID bigint

AS

if (select count(*) from Permitted_Experiments 
where Experiment_Info_ID=@experimentInfoID 
and Recurrence_ID= ( select Recurrence_ID from Time_Blocks where Time_Block_ID=@timeBlockID)) ! = 0

select Permitted_Experiment_ID from Permitted_Experiments where Experiment_Info_ID=@experimentInfoID and  Recurrence_ID= ( select Recurrence_ID from Time_Blocks where Time_Block_ID=@timeBlockID)

else
select 'Return Status' = -1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE PermittedExperiment_RetrieveIDByRecur

@experimentInfoID int,
@recurrenceID int

AS

if (select count(*) from Permitted_Experiments where Experiment_Info_ID=@experimentInfoID and Recurrence_ID=@recurrenceID) ! = 0
select Permitted_Experiment_ID from Permitted_Experiments where Experiment_Info_ID=@experimentInfoID and  Recurrence_ID= @recurrenceID

else
select 'Return Status' = -1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE PermittedExperiment_RetrieveIDsByRecurrence
@recurrenceID int

AS

select Experiment_Info_ID from Permitted_Experiments where   Recurrence_ID= @recurrenceID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE PermittedExperiment_RetrieveIDsForRecurrence 
@recurrenceID int
AS

select Experiment_Info_ID from Permitted_Experiments 
where Recurrence_ID = @recurrenceID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE PermittedGroup_Add

@credentialSetID int,
@recurrenceID int

AS

insert into Permitted_Groups(Credential_Set_ID, Recurrence_ID) values (@credentialSetID, @recurrenceID)
select ident_current('Permitted_Groups')

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE PermittedGroup_Delete 

@groupID int,
@recurrenceID int

AS

delete from Permitted_Groups 
where Credential_Set_ID= @groupID
and  Recurrence_ID = @recurrenceID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE PermittedGroup_RetrieveIDByRecur

@groupID int,
@recurrenceID int

AS

if (select count(*) from Permitted_Groups where Credential_Set_ID=@groupID and Recurrence_ID=@recurrenceID) ! = 0
select Credential_Set_ID from Permitted_Groups where Credential_Set_ID=@groupID and  Recurrence_ID= @recurrenceID

else
select 'Return Status' = -1

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE PermittedGroup_RetrieveIDsForRecurrence 
@recurrenceID int
AS

select Credential_Set_ID from Permitted_Groups 
where Recurrence_ID = @recurrenceID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO




/****** Object:  Stored Procedure dbo.AddRecurrence    Script Date: 5/2/2006 5:51:11 PM ******/
CREATE PROCEDURE Recurrence_Add
@resourceID int,
@startDate datetime,
@numDays int,
@recurrenceType int,
@startOffset int,
@endOffset int,
@quantum int,
@dayMask tinyint

AS

insert into Recurrence (Recurrence_Start_Date, Recurrence_Num_Days, Recurrence_Type,Recurrence_Start_Offset, Recurrence_End_Offset,
Quantum, Resource_ID,Day_Mask) 
values (@startDate, @numDays, @recurrenceType, @startOffset, @endOffset,@quantum, @resourceID,@dayMask)
select ident_current('Recurrence')

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.DeleteRecurrence    Script Date: 5/2/2006 5:51:11 PM ******/
CREATE PROCEDURE Recurrence_Delete

@recurrenceID int

AS

delete from Recurrence where Recurrence_ID= @recurrenceID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE Recurrence_Get
@sbGuid varchar(50),
@groupName nvarchar(256),
@clientGuid varchar(50),
@lsGuid varchar(50),
@startTime DateTime,
@endTime DateTime
AS
declare @credentialSetId int
declare @experimentInfoId int

select @credentialSetId=Credential_Set_ID FROM CredentialSets 
	where Service_Broker_GUID= @sbGuid AND Group_Name =@groupName

select @experimentInfoId = experiment_Info_ID FROM Experiment_Info
	WHERE Lab_Client_Guid = @clientGuid and Lab_Server_Guid = @lsGuid

select Recurrence_ID,Recurrence_Start_Date, Recurrence_num_days, Recurrence_Type,  Recurrence_Start_Offset, Recurrence_End_offset,
 Quantum, Resource_ID, Day_Mask from Recurrence 
where Recurrence_ID IN (select distinct recurrence_id from permitted_Groups where credential_set_id = @credentialSetId
  	and recurrence_id in (Select recurrence_ID from permitted_experiments where experiment_info_id = @experimentInfoID))
	AND Recurrence_Start_Date < @endTime AND DateADD(day,Recurrence_Num_Days,Recurrence_Start_Date) >= @startTime

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE Recurrence_GetIDs
@credentialSetId int,
@experimentInfoId int
AS

select distinct recurrence_id from permitted_Groups where credential_set_id = @credentialSetId
  and recurrence_id in 
  (Select recurrence_ID from permitted_experiments where experiment_info_id = @experimentInfoID)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



/****** Object:  Stored Procedure dbo.RetrieveRecurrenceByID    Script Date: 5/2/2006 5:51:11 PM ******/

CREATE PROCEDURE Recurrence_RetrieveByID
@recurrenceID int

AS

select Recurrence_Start_Date, Recurrence_num_days, Recurrence_Type,  Recurrence_Start_Offset, Recurrence_end_offset,
 Quantum, Resource_ID, Day_Mask from Recurrence where Recurrence_ID=@recurrenceID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.RetrieveRecurrenceIDs    Script Date: 5/2/2006 5:51:11 PM ******/
CREATE PROCEDURE Recurrence_RetrieveIDs

AS

select Recurrence_ID from Recurrence order by Recurrence_Start_Date asc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE Recurrence_RetrieveIDsByResourceAndTime

@resourceID int,
@start DateTime,
@end DateTime

AS

select Recurrence_ID from Recurrence 
where resource_ID= @resourceID
AND(
 (DateAdd(ss,Recurrence_Start_Offset,Recurrence_Start_Date ) <= @end)
  AND (DateAdd(day,Recurrence_num_days,Recurrence_Start_Date ) >= @start)
)
order by Recurrence_Start_Date asc

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE Recurrences_Retrieve
@sbGuid varchar(50),
@group nvarchar(256),
@clientGuid varchar(50),
@lsGuid varchar(50),
@start datetime,
@end datetime

AS

declare @credSet int
declare @expId int
select @credSet= credential_Set_id from credential_sets where service_broker_guid = @sbGuid and Group_Name=@group
select @expId=Experiment_Info_ID from Experiment_Info where Lab_client_guid=@clientGuid and Lab_server_Guid=@lsGuid


select recurrence_id,resource_id,recurrence_type,day_mask,recurrence_start_date,recurrence_num_days,
recurrence_start_offset,recurrence_end_offset, quantum from Recurrence
where recurrence_id 
in (select DISTINCT recurrence_id from permitted_Experiments 
	where Experiment_Info_ID=@expID 
	AND recurrence_id in (select recurrence_id from Permitted_groups where credential_set_id =@credSet))
	and (recurrence_start_date <= @end AND DATEADD(day, recurrence_num_days,recurrence_start_date) >= @start)
ORDER BY recurrence_Start_Date


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE Reservation_Add
@resourceID int,
@startTime datetime,
@endTime datetime,
@experimentInfoID int,
@credentialSetID int,
@ussID int,
@status int


AS


insert into Reservation_Info( resource_id, Start_Time, End_Time, Experiment_Info_ID, Credential_Set_ID, USS_Info_ID, status) 
values ( @resourceID, @startTime, @endTime, @experimentInfoID, @credentialSetID, @ussID, @status)
select ident_current('Reservation_Info')


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE Reservation_GetTimes
@resourceID int,
@start datetime,
@end datetime
AS
select start_time, end_time from reservation_info 
where resource_ID = @resourceID
AND (@end > start_time AND @start < end_time)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



/****** Object:  Stored Procedure dbo.AddReservationInfo    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.AddReservationInfo    Script Date: 4/11/2006 6:19:42 PM ******/
CREATE PROCEDURE ReservationInfo_Add
@serviceBrokerGUID varchar(50),
@groupName nvarchar(256),
@ussGUID varchar(50),
@clientGuid varchar(50),
@labServerGuid varchar(50),
@startTime datetime,
@endTime datetime,
@status int


AS
declare 
@resourceID int,
@credentialSetID int,
@experimentInfoID int,
@ussId int

select @resourceID=(select resource_id from LS_Resources where Lab_Server_Guid = @labServerGuid)
select @credentialSetID=(select Credential_Set_ID from Credential_Sets where Service_Broker_GUID=@serviceBrokerGUID and Group_Name=@groupName)
select @experimentInfoID = (select Experiment_Info_ID from Experiment_Info where Lab_Client_Guid = @clientGuid and Lab_Server_Guid = @labServerGuid)
if @ussGuid Is Null
SET @ussId = 0
else
select @ussId = (select USS_Info_ID from USS_Info where USS_GUID = @ussGUID)
EXEC Reservation_Add @resourceID,@startTime,@endTime,@experimentInfoID,@credentialSetId, @ussId, @status

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.DeleteReservationInfo    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.DeleteReservationInfo    Script Date: 4/11/2006 6:19:41 PM ******/
CREATE PROCEDURE ReservationInfo_Delete

@serviceBrokerGUID varchar(50),
@groupName nvarchar(256),
@ussGUID varchar(50),
@clientGuid varchar(50),
@labServerGuid varchar(50),
@startTime DateTime,
@endTime DateTime

 AS
declare
@experimentInfoID int,
@credentialSetID int,
@ussID int
if @ussGUID IS NULL
	set @ussID = NULL
else 
	select @ussID = (select USS_Info_ID from USS_Info where USS_GUID = @ussGUID)

select @experimentInfoID=(select Experiment_Info_ID from Experiment_Info 
where Lab_Client_Guid=@clientGuid and Lab_Server_GUID=@labServerGuid)

select @credentialSetID=(select Credential_Set_ID from Credential_Sets 
where Service_Broker_GUID=@serviceBrokerGUID and Group_Name=@groupName)
select @ussID = (select USS_Info_ID from USS_Info where USS_GUID = @ussGUID)

delete from Reservation_Info 
where Start_Time=@startTime and End_Time=@endTime and Experiment_Info_ID=@experimentInfoID 
and Credential_Set_ID=@credentialSetID and USS_Info_ID = @ussID
select @@rowcount


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.DeleteReservationInfoByID    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.DeleteReservationInfoByID    Script Date: 4/11/2006 6:19:41 PM ******/
CREATE PROCEDURE ReservationInfo_DeleteByID

@reservationInfoID int 

AS

delete from Reservation_Info where Reservation_Info_ID= @reservationInfoID
select @@rowcount


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



CREATE PROCEDURE ReservationData_Retrieve
@resourceID int = 0,
@expID int = 0,
@credID int = 0,
@start Datetime ,
@end Datetime
 AS
BEGIN

select R.Reservation_Info_ID,R.Start_Time, R.End_Time, E.Lab_Client_GUID,E.Lab_Server_GUID, C.Group_Name,C.Service_Broker_Guid,R.USS_Info_ID,R.status
from Reservation_Info R, Experiment_info E, Credential_Sets C
where R.Reservation_Info_ID IN (Select * from GetReservationIDs_ByIDs( @resourceID, @expID,@credID, @start,@end))
and R.Experiment_Info_ID = E.Experiment_Info_ID and R.Credential_Set_ID = C.Credential_Set_ID
ORDER BY R.Start_Time desc

END

GO

/****** Object:  Stored Procedure dbo.RetrieveReservationInfoByID    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.RetrieveReservationInfoByID    Script Date: 4/11/2006 6:19:42 PM ******/
CREATE PROCEDURE ReservationInfo_RetrieveByID
@reservationInfoID int
 AS
select resource_ID,Start_Time, End_Time, Experiment_Info_ID, Credential_Set_ID, USS_Info_ID, status from Reservation_Info where Reservation_Info_ID=@reservationInfoID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.RetrieveReservationInfoIDByLabServer    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.RetrieveReservationInfoIDByLabServer    Script Date: 4/11/2006 6:19:42 PM ******/


CREATE PROCEDURE ReservationInfo_RetrieveIDByLabServer

@labServerGUID varchar(50),
@startTime DateTime,
@endTime DateTime

AS
Create table #expInfo (ids int)
INSERT INTO #expInfo SELECT Experiment_info_ID from Experiment_Info where Lab_Server_GUID = @labServerGUID
select Reservation_Info_ID from Reservation_Info
where  Experiment_Info_ID in (SELECT ids from #expInfo) and End_Time>@startTime and Start_Time<@endTime 



GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE ReservationInfo_RetrieveIDByResource

@resourceID int,
@startTime DateTime,
@endTime DateTime

AS
select Reservation_Info_ID from Reservation_Info  where Resource_ID = @resourceID 
and End_Time>@startTime and Start_Time<@endTime ORDER BY Start_Time asc


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.RetrieveReservationInfoIDs    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.RetrieveReservationInfoIDs    Script Date: 4/11/2006 6:19:42 PM ******/
CREATE PROCEDURE ReservationInfo_RetrieveIDs

@serviceBrokerGUID varchar(50),
@groupName nvarchar(256),
@ussGUID varchar(50),
@clientGuid varchar(50),
@labServerGuid varchar(50),
@startTime datetime,
@endTime datetime

 AS
declare
@credentialSetID int,
@experimentInfoID int,
@ussID int

if @ussGUID = NULL
SET @ussID = null
else
select @ussId = (select USS_Info_ID from USS_Info where USS_GUID = @ussGUID)

select @credentialSetID=Credential_Set_ID from Credential_Sets where Service_Broker_GUID=@serviceBrokerGUID and Group_Name=@groupName
select @experimentInfoID=Experiment_Info_ID from Experiment_Info where Lab_Client_Guid=@clientGuid and Lab_Server_Guid=@labServerGuid
select @ussId = (select USS_Info_ID from USS_Info where USS_GUID = @ussGUID)
select Reservation_Info_ID from Reservation_Info where Credential_Set_ID=@credentialSetID and Experiment_Info_ID=@experimentInfoID 
and USS_Info_ID = @ussID and End_Time>@startTime and Start_Time<@endTime ORDER BY Start_Time asc


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.RetrieveReserveInfoIDsByExperiment    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.RetrieveReserveInfoIDsByExperiment    Script Date: 4/11/2006 6:19:42 PM ******/
CREATE PROCEDURE ReservationInfo_RetrieveIDsByExperiment
@experimentInfoID int

AS 

select Reservation_Info_ID from Reservation_Info where Experiment_Info_ID=@experimentInfoID order by Start_Time asc


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

/****** Object:  Stored Procedure dbo.ReservationTags_Retrieve    Script Date: 5/20/2010 6:39:48 PM ******/

CREATE PROCEDURE ReservationTags_Retrieve
@resourceID int = 0,
@expID int = 0,
@credID int = 0,
@start Datetime ,
@end Datetime
 AS
BEGIN


select R.Reservation_Info_ID,R.Start_Time, R.End_Time, E.Lab_Client_Name, C.Group_Name,C.Service_Broker_Name,R.status
from Reservation_Info R, Experiment_info E, Credential_Sets C
where R.Reservation_Info_ID IN (Select * from GetReservationIDs_ByIDs( @resourceID, @expID,@credID, @start,@end))
and R.Experiment_Info_ID = E.Experiment_Info_ID and R.Credential_Set_ID = C.Credential_Set_ID
ORDER BY R.Start_Time desc

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE ReservationTags_RetrieveByLabServer
@guid varchar(50),
@start Datetime,
@end Datetime
 AS

if @start != NULL AND @end != NULL

select R.Reservation_Info_ID,R.Start_Time, R.End_Time, E.Lab_Client_Name, C.Group_Name,C.Service_Broker_Name,R.status
from Reservation_Info R, Experiment_Info E, Credential_Sets C 
where R.Experiment_Info_ID in (SELECT Experiment_Info_ID from Experiment_Info where Lab_Server_Guid = @guid)
  AND R.End_Time > @start AND R.start_time < @end
AND R.Experiment_Info_ID = E.Experiment_Info_ID AND R.Credential_Set_ID = C.Credential_Set_ID
ORDER BY R.Start_Time desc

ELSE IF @start!= NULL AND @end = NULL

select R.Reservation_Info_ID,R.Start_Time, R.End_Time, E.Lab_Client_Name, C.Group_Name,C.Service_Broker_Name,R.status
from Reservation_Info R, Experiment_Info E, Credential_Sets C 
where R.Experiment_Info_ID in (SELECT Experiment_Info_ID from Experiment_Info where Lab_Server_Guid = @guid)
  AND R.End_Time > @start
AND R.Experiment_Info_ID = E.Experiment_Info_ID AND R.Credential_Set_ID = C.Credential_Set_ID
ORDER BY R.Start_Time desc


ELSE IF @start = NULL AND @end != NULL

select R.Reservation_Info_ID,R.Start_Time, R.End_Time, E.Lab_Client_Name, C.Group_Name,C.Service_Broker_Name,R.status
from Reservation_Info R, Experiment_Info E, Credential_Sets C 
where R.Experiment_Info_ID in (SELECT Experiment_Info_ID from Experiment_Info where Lab_Server_Guid = @guid)
  AND R.Start_Time < @end
AND R.Experiment_Info_ID = E.Experiment_Info_ID AND R.Credential_Set_ID = C.Credential_Set_ID
ORDER BY R.Start_Time desc

ELSE

select R.Reservation_Info_ID,R.Start_Time, R.End_Time, E.Lab_Client_Name, C.Group_Name,C.Service_Broker_Name,R.status
from Reservation_Info R, Experiment_Info E, Credential_Sets C 
where R.Experiment_Info_ID in (SELECT Experiment_Info_ID from Experiment_Info where Lab_Server_Guid = @guid)
AND R.Experiment_Info_ID = E.Experiment_Info_ID AND R.Credential_Set_ID = C.Credential_Set_ID
ORDER BY R.Start_Time desc
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE Resource_AddGetID
@guid varchar (50),
@name nvarchar (256)
AS
if( select count(resource_ID) from LS_Resources where Lab_Server_Guid = @guid) >0
select resource_id from LS_Resources where Lab_Server_Guid = @guid
else
BEGIN
insert into LS_Resources (Lab_Server_Guid, Lab_Server_Name)
values (@guid,@name)
select ident_current('LSS_Resources')
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE Resource_Delete
@id int
AS
DELETE from LS_Resources where resource_id = @id
select @@rowcount

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE Resource_Get
@id int
AS
SELECT resource_id,Lab_Server_Guid,Lab_Server_Name,description 
from LS_resources where resource_id = @id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE Resource_GetByGuid
@guid varchar(50)
AS
SELECT resource_id,Lab_Server_Guid,Lab_Server_Name,description 
from LS_resources where Lab_Server_Guid = @guid

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE Resource_GetTags
AS

Select resource_id, Lab_Server_Name + ': ' + isnull(description,'')  from LS_Resources


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE Resource_GetTagsByGuid
@guid varchar (50)
AS

Select resource_id, Lab_Server_Name + ': ' + isnull(description ,'')
from LS_Resources where lab_Server_Guid = @guid

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE Resource_Insert
@guid varchar (50),
@name nvarchar (256),
@description nvarchar (2048)
AS
insert into LS_Resources (Lab_Server_Guid, Lab_Server_Name,description)
values (@guid,@name,@description)
select ident_current('LSS_Resources')

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE Resource_SetDescription
@id int,
@description nvarchar (2048)
AS
UPDATE LS_Resources set description=@description
where resource_id = @id
select @@rowcount

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO



/****** Object:  Stored Procedure dbo.AddUSSInfo    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.AddUSSInfo    Script Date: 4/11/2006 6:19:42 PM ******/
CREATE PROCEDURE USSInfo_Add

@ussGUID varchar(50),
@ussName nvarchar(256),
@ussURL nvarchar(512),
@couponId bigint,
@domainGuid varchar(50)


AS
insert into USS_Info(USS_GUID, USS_Name, USS_URL,coupon_id,domain_Guid) 
values (@ussGUID,@ussName, @ussURL,@couponID,@domainGuid) 
select ident_current('USS_Info')


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.DeleteUSSInfo    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.DeleteUSSInfo    Script Date: 4/11/2006 6:19:41 PM ******/
CREATE PROCEDURE USSInfo_Delete

@ussInfoID int

AS

delete from USS_Info where USS_Info_ID=@ussInfoID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO



/****** Object:  Stored Procedure dbo.ModifyUSSInfo    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.ModifyUSSInfo    Script Date: 4/11/2006 6:19:42 PM ******/
CREATE PROCEDURE USSInfo_Modify
@ussInfoID int,
@ussGUID varchar(50),
@ussName nvarchar(256),
@ussURL nvarchar(512),
@couponId bigint,
@domainGuid varchar(50)


AS

update USS_Info set USS_GUID=@ussGUID, USS_Name=@ussName, USS_URL=@ussURL,
coupon_id=@couponId, domain_guid=@domainGuid
where USS_Info_ID=@ussInfoID

select @@rowcount

return


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE USSInfo_RetrieveByGUID
@guid varchar(50)

AS

select USS_INFO_ID, USS_GUID, USS_Name, USS_URL, coupon_id, domain_Guid
from USS_Info where USS_GUID=@guid
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.RetrieveUSSInfoByID    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.RetrieveUSSInfoByID    Script Date: 4/11/2006 6:19:42 PM ******/
CREATE PROCEDURE USSInfo_RetrieveByID

@ussInfoID int

AS

select USS_GUID, USS_Name, USS_URL, coupon_id, domain_Guid
from USS_Info where USS_Info_ID=@ussInfoID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.RetrieveUSSInfoID    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.RetrieveUSSInfoID    Script Date: 4/11/2006 6:19:42 PM ******/
CREATE PROCEDURE USSInfo_RetrieveID
@ussGuid varchar(50)

AS

select USS_Info_ID from USS_Info where USS_GUID= @ussGuid


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


/****** Object:  Stored Procedure dbo.RetrieveUSSInfoIDs    Script Date: 5/2/2006 5:51:11 PM ******/

/****** Object:  Stored Procedure dbo.RetrieveUSSInfoIDs    Script Date: 4/11/2006 6:19:42 PM ******/
CREATE PROCEDURE USSInfo_RetrieveIDs
 AS
select USS_Info_ID from USS_Info


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


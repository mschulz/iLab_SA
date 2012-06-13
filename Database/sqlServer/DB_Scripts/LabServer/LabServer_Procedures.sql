-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetActiveTaskIDs]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetActiveTaskIDs]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetActiveTasks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetActiveTasks]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetTaskStatusByExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetTaskStatusByExperiment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetExpiredTasks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetExpiredTasks]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLocalGroupID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetLocalGroupID]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLocalGroupIDByName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)

drop procedure [dbo].[GetLocalGroupIDByName]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLocalGroupIDByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetLocalGroupIDByID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetTask]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetTask]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetTaskByExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetTaskByExperiment]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertLabApp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertLabApp]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyLabApp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyLabApp]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[ModifyLabPaths]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[ModifyLabPaths]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[RemoveLabApp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[RemoveLabApp]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLabApps]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetLabApps]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLabApp]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetLabApp]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLabAppByKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetLabAppByKey]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLabAppByGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetLabAppByGroup]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetAppIdForRemoteGroupName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetAppIdForRemoteGroupName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLabAppTags]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetLabAppTags]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLabAppTag]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetLabAppTag]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetLabAppTagByKey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetLabAppTagByKey]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetViIdForRemoteGroupID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetViIdForRemoteGroupID]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVIDisplayInfo]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVIDisplayInfo]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetVIRedirectforGroup]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetVIRedirectforGroup]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertTask]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertTask]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectTasks]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectTasks]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectTasksByStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectTasksByStatus]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectTasksByStatusRange]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectTasksByStatusRange]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectTask]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectTask]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SelectTaskByExperiment]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SelectTaskByExperiment]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetTaskData]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetTaskData]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetTaskStatus]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetTaskStatus]
GO



CREATE PROCEDURE GetLocalGroupIDByID
@groupID int,
@guid  varchar(50)
AS
select GroupID
from [SBGroup] where SBGroup_ID = @groupID and SB_GUID = @guid
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO




CREATE PROCEDURE GetLocalGroupIDByName
@groupName nvarchar(256),
@guid  varchar(50)
AS
select GroupID
from [SBGroup] where GroupName = @groupName and SB_GUID = @guid
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE InsertLabApp
@application nvarchar (100),
@guid varchar(50),
@appKey nvarchar (100),
@path nvarchar (256),
@version nvarchar (50),
@rev nvarchar (50),
@page nvarchar (256),
@title nvarchar (256),
@description ntext,
@comment ntext,
@width int,
@height int,
@type int,
@server nvarchar (256),
@port int,
@contact nvarchar (256),
@cgi nvarchar (512),
@datasource ntext,
@extra ntext

AS
 INSERT INTO LabApp (width, height, type, port, appGuid,appKey, server, path,
	DataSource, Application, version, rev,
	cgiURL, Page, Title, Description, contact, ExtraInfo, comment)
	values (@width, @height,@type, @port, @guid, @appKey, @server, @path,
	@datasource, @application, @version, @rev,
	@cgi, @Page, @title, @description, @contact, @extra, @comment)

 select ident_current('LabApp')

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE ModifyLabApp
@appId int,
@application nvarchar (100),
@guid varchar(50),
@appKey nvarchar (100),
@path nvarchar (256),
@version nvarchar (50),
@rev nvarchar (50),
@page nvarchar (256),
@title nvarchar (256),
@description ntext,
@comment ntext,
@width int,
@height int,
@type int,
@server nvarchar (256),
@port int,
@contact nvarchar (256),
@cgi nvarchar (512),
@datasource ntext,
@extra ntext

AS
 UPDATE LabApp set width=@width, height=@height, type=@type,port=@port, appGuid=@guid, appKey=@appKey,
	server=@server, path=@path, DataSource=@datasource, Application=@application, version=@version, rev=@rev,
	cgiURL=@cgi, Page=@Page, Title=@title, Description=@description,
	contact=@contact, ExtraInfo=@extra, comment=@comment
where LabApp_ID = @appId
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE ModifyLabPaths
@oldPath nvarchar(256),
@newPath nvarchar(256)

 AS

update Lab_App set DataSource=REPLACE(DataSource,@oldPath,@newPath), Path=REPLACE(Path,@oldPath,@newPath),
	 Application=REPLACE(Application,@oldPath,@newPath), Server=REPLACE(Server,@oldPath,@newPath),
	 Page=REPLACE(Page,@oldPath,@newPath), CgiURL=REPLACE(CgiURL,@oldPath,@newPath)

select @@rowcount

RETURN
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE RemoveLabApp
@appId int
AS
delete from permission where LabApp_ID = @appId
delete from LabApp where LabApp_id = @appId
go

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetLabApps 

AS

select LabApp_ID,path,Application,page,title,description,
extraInfo,contact,comment,width,height,datasource,server,port,
cgiURL, version,rev,appKey,appGuid  from LabApp

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE GetLabApp 
 @appId int
AS

select path,Application,page,title,description,extraInfo,contact,
comment,width,height,datasource,server,port,cgiURL,
version,rev,appKey,appGuid  from LabApp 
where LabApp_ID = @appId

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE GetLabAppByKey 

 @appKey nvarchar (100)
AS

select LabApp_ID,path,Application,page,title,description,extraInfo,contact,comment,
width,height,datasource,server,port,cgiURL, version,rev,appKey,appGuid  
from LabApp where appKey = @appKey

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetLabAppByGroup 
@groupName nvarchar (256),
@guid varchar (50)
AS
declare @app_id int;
declare @gid int;

 select @gID=GroupID from SBGroup where GroupName= @groupName and SB_GUID=@guid

  select @app_id = LabApp_ID from permission where Group_ID = @gID
select labApp_id,path,Application,page,title,description,extraInfo,contact,comment,
width,height,datasource,server,port,cgiURL, version,rev,appKey, appGuid
  from LabApp where LabApp_ID = @app_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetLabAppTags

as 
select LabApp_id,title from LabApp
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE GetLabAppTag
@appId int
as 
select LabApp_id,title from LabApp
where LabApp_id = @appId
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetLabAppTagByKey
@appKey nvarchar (100)
as 
select LabApp_id,title from LabApp
where AppKey = @appKey
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetVIDisplayInfo 
 @vi_id int
AS
select title,description,extraInfo,contact,comment,width,height from LAbApp where LabApp_ID = @vi_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetVIRedirectforGroup 
@groupID int

AS
declare
 @vi_id numeric
select @vi_id = (select LabApp_id from permission where group_ID = @groupID)
select LabApp_ID,path,application,page,width,height,datasource,server, port, cgiURL, version, extraInfo from LabApp where LAbApp_ID = @vi_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE GetAppIdForRemoteGroupName
@groupName nvarchar(256),
@guid varchar(50)

AS
  declare @gID int
  select @gID=GroupID from SBGroup where GroupName= @groupName and SB_GUID=@guid

  select LabApp_ID from permission where Group_ID = @gID


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetTaskStatusByExperiment
@id bigint,
@guid varchar(50)

AS
  declare @status int
  select @status=status from task where Experiment_ID = @id AND issuer_GUID = @guid

GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE GetTask
@taskid bigint

AS
select LabApp_ID,Experiment_ID,GroupName,StartTime,endTime,Status,Coupon_ID,Issuer_GUID,data 
from task where task_ID = @taskID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE GetTaskByExperiment
@experimentid bigint,
@sbguid varchar(50)

AS
select task_ID,LabApp_ID,Experiment_ID,GroupName,StartTime,endTime,Status,Coupon_ID,Issuer_GUID,data 
from task where Experiment_ID = @experimentid AND Issuer_GUID = @sbguid

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE GetExpiredTasks
@targetTime datetime

AS
select task_ID,LabApp_ID,Experiment_ID,GroupName,StartTime,endTime,Status,Coupon_ID,Issuer_GUID,data 
from task where Status BETWEEN 1 and 127 and endTime  < @targetTime

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO





CREATE PROCEDURE GetActiveTaskIDs

AS
select task_ID,Status 
from task where Status BETWEEN 1 and 127

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetActiveTasks

AS
select task_ID,LabApp_ID,Experiment_ID,GroupName,StartTime,endTime,Status,Coupon_ID,Issuer_GUID,data 
from task where Status BETWEEN 1 and 127
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE InsertTask
@appID bigint,
@expID bigint,
@couponID bigint,
@status int,
@startTime datetime,
@endTime datetime,
@issuerGUID varchar (50),
@groupName varchar (128),
@data nvarchar (2000)


AS
insert into Task (LabApp_ID,Experiment_ID,Coupon_ID,Status,StartTime,endTime,Issuer_GUID,GroupName,Data) 
 values (@appID,@expID,@couponID,@status,@startTime,@endTime,@issuerGUID,@groupName,@data)
select ident_current('Task')

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE SelectTask
@taskid bigint

AS
select Task_id,LabApp_ID,Experiment_ID,GroupName,StartTime,endTime,Status,Coupon_ID,Issuer_GUID,Data
 	from Task where task_id = @taskid


GO

CREATE PROCEDURE SelectTaskByExperiment
@experimentID bigint

AS
select Task_id,LabApp_ID,Experiment_ID,GroupName,StartTime,endTime,Status,Coupon_ID,Issuer_GUID,Data
 	from Task where Experiment_ID = @experimentID


GO

CREATE PROCEDURE SelectTasks


AS
select Task_id,LabApp_ID,Experiment_ID,GroupName,StartTime,endTime,Status,Coupon_ID,Issuer_GUID,Data
 	from Task


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE SelectTasksByStatus
@status int


AS
select Task_id,LabApp_ID,Experiment_ID,GroupName,StartTime,endTime,Status,Coupon_ID,Issuer_GUID,Data
 	from Task where Status = @status


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE SelectTasksByStatusRange
@low int,
@high int


AS
select Task_id,LabApp_ID,Experiment_ID,GroupName,StartTime,endTime,Status,Coupon_ID,Issuer_GUID,Data
 	from Task where Status between @Low AND @high


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE SetTaskData
@taskid bigint,
@data nvarchar (2000)
AS
update Task set Data = @data
where task_id = @taskid


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE SetTaskStatus
@taskID bigint,
@status int

AS
update task set status = @status where task_ID = @taskID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO





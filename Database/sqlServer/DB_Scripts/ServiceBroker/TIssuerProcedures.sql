
-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$


--
-- Stored Procedures specfic to a TicketIssuer Service ( Service Broker ).
-- These procedures should be added to a Ticketing Service database for services that Issue tickets.
--


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CancelIssuedCoupon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CancelIssuedCoupon]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreateCoupon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreateCoupon]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteIssuedCoupon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteIssuedCoupon]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetIssuedCoupon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetIssuedCoupon]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetIssuedCouponByPasskey]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetIssuedCouponByPasskey]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddTicket]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddTicket]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertIssuedTicket]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertIssuedTicket]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CancelIssuedTicket]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CancelIssuedTicket]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteIssuedTicket]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteIssuedTicket]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreateTicket]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreateTicket]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetExpiredIssuedTickets]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetExpiredIssuedTickets]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetIssuedCollectionCount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetIssuedCollectionCount]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetIssuedTicketID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetIssuedTicketID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetIssuedTickets]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetIssuedTickets]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetIssuedTicketCoupon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetIssuedTicketCoupon]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetIssuedTicket]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetIssuedTicket]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetIssuedTicketByFunction]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetIssuedTicketByFunction]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetIssuedTicketByID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetIssuedTicketByID]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetIssuedTicketByRedeemer]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetIssuedTicketByRedeemer]
GO
if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetIssuedTicketsByType]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetIssuedTicketsByType]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[InsertAdminURL]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[InsertAdminURL]
GO


if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[SetIdentificationCouponID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[SetIdentificationCouponID]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AuthenticateIssuedCoupon]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AuthenticateIssuedCoupon]
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

-----------

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

-- Inserts a new Coupon into the IssuedCoupon table, note the ISSUER_GUID is not stored
-- as it must be issued by this ticket issuer.
CREATE PROCEDURE CreateCoupon
@passKey varchar(100)

 AS

insert into IssuedCoupon( Passkey, Cancelled)
values(@passKey, 0)
select ident_current('IssuedCoupon')

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

-- Sets the cancelled bit to true for a coupon in the coupon table,
-- if a coupon is found any tickets are also cancelled.
-- Note: issuedCoupon is not checked.
CREATE PROCEDURE CancelIssuedCoupon
@couponID bigint
 AS

BEGIN TRANSACTION
begin
update IssuedCoupon set Cancelled = 1 where Coupon_ID=@couponID
if (@@rowcount = 0)
goto on_error
if (@@error>0)
goto on_error
update IssuedTicket set Cancelled =1 where Coupon_ID = @couponID
if (@@error>0)
goto on_error
end
COMMIT TRANSACTION
select 1
return

on_error:
ROLLBACK TRANSACTION
select 0
return

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE DeleteIssuedCoupon
@couponID bigint
 AS

BEGIN TRANSACTION
begin
delete from IssuedCoupon  where Coupon_ID=@couponID
if (@@rowcount = 0)
goto on_error
if (@@error>0)
goto on_error
end
COMMIT TRANSACTION
select 1
return

on_error:
ROLLBACK TRANSACTION
select 0
return

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
CREATE PROCEDURE GetIssuedCoupon 

@couponID bigint
 AS

select Cancelled, Passkey from IssuedCoupon
where Coupon_ID = @couponID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
CREATE PROCEDURE GetIssuedCouponByPasskey 

@passkey varchar(100)
 AS

select Coupon_ID, Cancelled from IssuedCoupon
where Passkey = @passkey
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE GetIssuedCollectionCount
	@couponID BigInt
AS
	select count(ticket_ID) from IssuedTicket where coupon_ID = @couponID
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE GetIssuedTicketCoupon
@type varchar(100),
@redeemer varchar(50),
@sponsor varchar(50)

AS

declare @typeid int
declare @tNow DateTime
set @tNow = GetUTCDate();
select @typeid = (select Ticket_Type_ID  from Ticket_Type where upper( Name ) =upper(@type) )
	
select coupon_ID, passkey from IssuedCoupon
where coupon_ID in (select distinct coupon_id from issuedTicket
where Ticket_Type_id = @typeid  and sponsor_guid = @sponsor and redeemer_guid = @redeemer
and cancelled = 0 and  ( duration = -1 or DateADD(ss,duration,creation_time) > @tNow))

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

--
-- IssuedTickets should not be stored within the ServiceBroker's TicketTable.
--


CREATE PROCEDURE InsertIssuedTicket
	@ticketType varchar(100),
	@couponID bigint,
	@redeemerGUID varchar(50),
        @sponsorGUID varchar(50),
        @payload ntext,
        @cancelled bit=0,
	@creationTime DateTime,
	@duration bigint
AS
        DECLARE @ticketTypeID int
        select @ticketTypeID = (select Ticket_Type_ID  from Ticket_Type where upper( Name ) =upper(@ticketType) )
	insert into IssuedTicket (Ticket_Type_ID, Coupon_ID,Redeemer_GUID, Sponsor_GUID, Creation_Time, 
		duration, payload, Cancelled)
		values ( @ticketTypeID, @couponID, @redeemerGUID, @sponsorGUID, @creationTime, @duration, 		
		@payload, @cancelled)
        select ident_current('IssuedTicket')
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO



CREATE PROCEDURE CancelIssuedTicket

@ticketType  varchar(100),
@redeemer  varchar(50),
@couponID  bigint

AS

DECLARE

@ticketTypeID int

select @ticketTypeID= (select Ticket_Type_ID from Ticket_Type where  upper(Name) = upper(@ticketType))

update IssuedTicket set Cancelled = 1 
where Ticket_Type_ID = @ticketTypeID and Redeemer_GUID = @redeemer and Coupon_ID = @couponID

select @@rowcount
return

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE DeleteIssuedTicket
@ticketID  bigint

AS

delete from IssuedTicket
where Ticket_ID = @ticketID

select @@rowcount
return

SET QUOTED_IDENTIFIER ON 

GO



CREATE PROCEDURE GetIssuedTicketID 
@couponID bigint,
@redeemerID varchar(50),
@ticketType varchar(100)

AS
DECLARE @ticketTypeID int

select @ticketTypeID=(select Ticket_Type_ID from Ticket_Type where (upper(Name) =upper( @ticketType) )) 
select Ticket_ID from IssuedTicket where (Coupon_ID = @couponID  AND Redeemer_GUID = @redeemerID  AND Ticket_Type_ID = @ticketTypeID)

return
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE [dbo].[GetIssuedTicketByFunction]
		@duration bigint,
        @ticketType varchar(100),
        @redeemerGuid varchar(50),
        @sponsorGuid varchar(50)
  AS
        DECLARE @ticketTypeID int
        select @ticketTypeID = (select Ticket_Type_ID  from Ticket_Type where upper( Name ) = upper(@ticketType) )
        if(@duration > 0)
        BEGIN
        select  Ticket_ID, upper(@ticketType), Coupon_ID, Redeemer_GUID, Sponsor_GUID, 
			Creation_Time, Duration, Payload, Cancelled
			from IssuedTicket  where Redeemer_GUID = @redeemerGuid 
			AND Sponsor_Guid = @sponsorGuid and Ticket_Type_ID = @ticketTypeID
			AND duration > 0 AND (DateADD(ss,duration,creation_time) >= DateADD(ss,@duration,GetUTCDate()))
		END
		ELSE
		BEGIN
        select  Ticket_ID, upper(@ticketType), Coupon_ID, Redeemer_GUID, Sponsor_GUID, 
			Creation_Time, Duration, Payload, Cancelled
			from IssuedTicket  where Redeemer_GUID = @redeemerGuid 
			AND Sponsor_Guid = @sponsorGuid and Ticket_Type_ID = @ticketTypeID
			AND Duration = -1 
		END
GO

CREATE PROCEDURE GetIssuedTicketByID
             @ticketID bigint
 AS
 	
	select  Ticket_ID, Ticket_Type.Name, Coupon_ID, Redeemer_GUID, Sponsor_GUID, 
	Creation_Time, duration, Payload, Cancelled
        from IssuedTicket join Ticket_Type on (IssuedTicket.Ticket_Type_ID = Ticket_Type.Ticket_Type_ID)
        where Ticket_ID = @ticketID;
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

-- CouponID argument returns all Tickets in a collection.
CREATE PROCEDURE GetIssuedTickets 
@couponID  bigint
 AS

	select  Ticket_ID, Ticket_Type.Name, Coupon_ID, Redeemer_GUID, Sponsor_GUID, 
	Creation_Time, duration, Payload, Cancelled
	from IssuedTicket  join Ticket_Type on (IssuedTicket.Ticket_Type_ID = Ticket_Type.Ticket_Type_ID)
	where (Coupon_ID= @couponID)


GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetIssuedTicket
	@couponID bigint,
        @ticketType varchar(100)
  AS
        DECLARE @ticketTypeID int

        select @ticketTypeID = (select Ticket_Type_ID  from Ticket_Type where upper( Name ) = upper(@ticketType) )
        select  Ticket_ID, upper(@ticketType), Coupon_ID, Redeemer_GUID, Sponsor_GUID, 
	Creation_Time, Duration, Payload, Cancelled
        from IssuedTicket  where coupon_ID = @couponID AND Ticket_Type_ID = @ticketTypeID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetIssuedTicketByRedeemer
	@couponID bigint,
        @ticketType varchar(100),
	@redeemer varchar(50)
  AS
        DECLARE @ticketTypeID int

        select @ticketTypeID = (select Ticket_Type_ID  from Ticket_Type where upper( Name ) = upper(@ticketType) )
        select  Ticket_ID, upper(@ticketType), Coupon_ID, Redeemer_GUID, Sponsor_GUID, 
	Creation_Time, Duration, Payload, Cancelled
        from IssuedTicket  
	where coupon_ID = @couponID AND Ticket_Type_ID = @ticketTypeID AND Redeemer_GUID = @redeemer

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE GetIssuedTicketsByType
              @ticketType varchar(100)
  AS
        DECLARE @ticketTypeID int

        select @ticketTypeID = (select Ticket_Type_ID  from Ticket_Type where upper( Name ) =upper(@ticketType) )
        select  Ticket_ID, upper(@ticketType), Coupon_ID, Redeemer_GUID, Sponsor_GUID, 
	Creation_Time, Duration, Payload, Cancelled
        from IssuedTicket  where Ticket_Type_ID = @ticketTypeID

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Create Procedure GetExpiredIssuedTickets
AS
declare @tNow DateTime
set @tNow = GetUTCDate();
select  Ticket_ID, Ticket_Type.Name, Coupon_ID, Redeemer_GUID, Sponsor_GUID, 
	Creation_Time, duration, Payload, Cancelled
	from IssuedTicket  join Ticket_Type on (IssuedTicket.Ticket_Type_ID = Ticket_Type.Ticket_Type_ID)
	where cancelled = 1 OR (cancelled = 0 and duration > 0 AND  DateADD(ss,duration,creation_time) < @tNow)
	order by coupon_ID

GO


CREATE PROCEDURE SetIdentificationCouponID

@agentGUID varchar(50),
@IdentOut_ID bigint
AS

update ProcessAgent set IdentOut_ID = @identOut_ID,  IdentIn_ID = @identOut_ID  where (Agent_GUID = @agentGUID)
select ident_current('ProcessAgents')
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

-- This version expects all coupons to be in the IssuedCoupon Table, 
CREATE PROCEDURE AuthenticateIssuedCoupon
@couponID bigint,
@passKey varchar(100)

 AS

select coupon_ID from IssuedCoupon 
where EXISTS ( SELECT * Where coupon_ID=@couponID AND passkey = @passKey AND cancelled = 0)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

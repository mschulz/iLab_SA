-- This is a patch for the 3.5.0 GetExpiredTickets which has problems with too many tickets
ALTER PROCEDURE GetExpiredTickets
AS
BEGIN
Create Table #ticks  (tId bigint,cId bigint, isGuid varchar (50))
create Table #coups (cId bigint,  isGuid varchar (50))
create table #coupAfter (cid bigint, isGuid varchar (50))
Create table #domains (isGuid varchar (50))
create Table #remove (cid bigint)

Declare @numTicks int
SET @numTicks = 0
Declare @numCoupons int
SET @numCoupons = 0;
Declare @numCoup int
SET @numCoup = 0;
Declare @domain varchar(50)

insert into #ticks select ticket_id, Coupon_ID, issuer_guid from ticket
	where cancelled = 1 OR 
	(duration != -1 and (DATEDIFF(second,creation_Time,GETUTCDATE()) > duration))
SET @numTicks = @@ROWCOUNT;
	
insert into #coups select distinct cid,isGuid from #ticks

BEGIN TRANSACTION
delete from Ticket where Ticket_ID IN (select tId from #ticks)
if (@@error <> 0)
		BEGIN
			ROLLBACK
			goto on_error
		END

insert into #coupAfter select distinct coupon_ID, issuer_guid from Ticket
where Coupon_ID in (Select cid from #coups)  group by coupon_ID,issuer_guid

insert into #domains select distinct isGuid from #coupAfter

DECLARE domainC CURSOR  LOCAL
for select isGuid from #domains
OPEN domainC

FETCH domainC INTO @domain
	While (@@FETCH_STATUS <> -1)
	BEGIN 
		TRUNCATE TABLE #remove
		insert into #remove select cid from #coups where isGuid = @domain
		delete from #remove where cid in (select cid from #coupAfter where isGuid = @domain)
		set @numCoup = (select COUNT(*) from #remove)
		set @numCoupons = @numCoupons + @numCoup
		delete from Coupon where issuer_guid = @domain AND Coupon_ID in ( select cid from #remove)
		if (@@error <> 0)
		BEGIN
			ROLLBACK
			CLOSE domainC
			DEALLOCATE domainC
			goto on_error
		END
		FETCH domainC INTO @domain
	END

CLOSE domainC
DEALLOCATE domainC

COMMIT TRANSACTION

Drop Table #ticks
Drop Table #coups
Drop Table #remove
-- Do not return any Selections, so the procedure call will 'Fall Through'
--select @numTicks, @numCoupons
return 0
on_error: 
	ROLLBACK TRANSACTION
	return -1
END
GO

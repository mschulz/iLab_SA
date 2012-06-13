/* Convert LSS db VERSION 3.0.6 TO 3.5.0 */

-- Copyright (c) 2011 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

IF NOT EXISTS ( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE  TABLE_NAME='Reservation_Info' AND COLUMN_NAME='USS_Info_ID' )
BEGIN
	ALTER TABLE  Reservation_Info ADD USS_Info_ID int NULL;
	declare @numUSS INT
	SET @numUSS = (select COUNT(*) from USS_Info)
	if @numUSS > 0
		if @numUSS = 1
			BEGIN
				update Reservation_Info set USS_Info_ID = (SELECT TOP 1 USS_Info_ID from USS_Info)
			END
		else
			BEGIN
				update Reservation_Info set USS_Info_ID = u.USS_Info_ID
				from Reservation_Info r, USS_Info u, Credential_Sets c
				where c.Credential_Set_ID = r.Credential_Set_ID AND c.USS_GUID = u.USS_GUID
			END
	ALTER TABLE Reservation_Info ALTER COLUMN USS_Info_ID INT NOT NULL;
	ALTER TABLE Credential_Sets ALTER COLUMN USS_GUID VARCHAR(50) NULL;
END
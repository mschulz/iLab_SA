/* Convert ISB db VERSION 3.0.6 TO 3.5.0 */

-- Copyright (c) 2011 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

IF NOT EXISTS ( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME='Agents' AND COLUMN_NAME='Date_Created' )
BEGIN
	ALTER TABLE  Agents ADD Date_Created DATETIME NOT NULL DEFAULT GETUTCDATE();
	IF EXISTS ( SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE  TABLE_NAME='Groups' AND COLUMN_NAME='Date_Created' )
	BEGIN
		update Agents set a.Date_Created = g.Date_Created from Agents a, Groups g where a.Agent_ID = g.Group_ID;
		ALTER TABLE  Groups DROP Column Date_Created;
	END
END

IF NOT EXISTS ( SELECT * FROM INFORMATION_SCHEMA.COLUMNS 
	WHERE  TABLE_NAME='Lab_Clients' AND COLUMN_NAME='Documentation_URL' )
	ALTER TABLE  Agents ADD Documentation_URL NVARCHAR(512) NULL
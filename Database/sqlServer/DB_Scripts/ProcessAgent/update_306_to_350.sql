/* Convert ProcessAgent db VERSION 3.0.4 TO 3.5.0 */

-- Copyright (c) 2011 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

SET IDENTITY_INSERT ProcessAgent_Type ON

if (SELECT COUNT(*) from ProcessAgent_Type where ProcessAgent_Type_ID = 0) = 0
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (0,' GPA', 'GENERIC PA');
if (SELECT COUNT(*) from ProcessAgent_Type where ProcessAgent_Type_ID = 1) = 0
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (1,'NOTA', 'NOT A PA');
if (SELECT COUNT(*) from ProcessAgent_Type where ProcessAgent_Type_ID = 128) = 0
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (128,'AUTH', 'AUTHORIZATION SERVICE');

if (SELECT COUNT(*) from ProcessAgent where ProcessAgent_Type_ID = 256) > 0
update ProcessAgent set ProcessAgent_Type_ID = 0 where ProcessAgent_Type_ID = 256

if (SELECT COUNT(*) from ProcessAgent_Type where ProcessAgent_Type_ID = 256) > 0
DELETE from ProcessAgent_Type where ProcessAgent_Type_ID = 256

SET IDENTITY_INSERT ProcessAgent_Type OFF
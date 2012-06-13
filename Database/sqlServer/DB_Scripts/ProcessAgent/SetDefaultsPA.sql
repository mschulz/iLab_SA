-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

/* created by Tingting. */

/* ENTERING DEFAULT VALUES IN ticket TABLES */

/*ProcessAgent_TYPES */

SET IDENTITY_INSERT ProcessAgent_Type ON

INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (0,' GPA', 'GENERIC PA');
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (1,'NOTA', 'NOT A PA');
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (4,' ISB', 'SERVICE BROKER');
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (5,' BSB', 'BATCH SERVICE BROKER');
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (6,' RSB', 'REMOTE SERVICE BROKER');
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (8,' ILS', 'LAB SERVER');
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (9,' BLS', 'BATCH LAB SERVER');
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (16,' ESS', 'EXPERIMENT STORAGE SERVER');
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (32,' USS', 'SCHEDULING SERVER');
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (64,' LSS', 'LAB SCHEDULING SERVER');
INSERT INTO ProcessAgent_Type(ProcessAgent_Type_ID, Short_Name, Description) VALUES (128,'AUTH', 'AUTHORIZATION SERVICE');



SET IDENTITY_INSERT ProcessAgent_Type OFF


/*TICKET_TYPES */

SET IDENTITY_INSERT Ticket_Type ON
/*Abstract Types*/
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (1,'ADMINISTER PA', 'Administer Process Agent', 1);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (2,'MANAGE PA', 'Manage Process Agent', 1);

INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (3,'AUTHENTICATE', 'Authenticate Ticket', 1);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (4,'LS', 'Lab Server Ticket', 1);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (5,'LSS', 'Lab Scheduling Server Ticket', 1);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (6,'USS', 'User Scheduling Server Ticket', 1);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (7,'ESS', 'Experiment Storage Server Ticket', 1);


/*Concrete Ticket Types*/
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (8,'AUTHENTICATE SERVICE BROKER', 'Authenticate Service Broker', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (9,'AUTHENTICATE AGENT', 'Authenticate Process Agent', 0);

INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (10,'REDEEM SESSION', 'Redeem Session', 0);

INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (11,'ADMINISTER ESS', 'Administer ESS', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (12,'ADMINISTER EXPERIMENT', 'Administer Experiment', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (13,'STORE RECORDS', 'Store Records', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (14,'RETRIEVE RECORDS', 'Retrieve Records', 0);

INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (15,'ADMINISTER USS', 'Administer USS', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (16,'MANAGE USS GROUP', 'USS Manage Group', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (17,'SCHEDULE SESSION', 'Schedule Session', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (18,'REVOKE RESERVATION','Revoke Reservation', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (19,'ALLOW EXPERIMENT EXECUTION','Allow Experiment Execution', 0);

INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (20,'ADMINISTER LSS','Administer LSS', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (21,'MANAGE LAB','Manage Lab', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (22,'REQUEST RESERVATION','Request Reservation', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (23,'REGISTER LS','Register LS', 0);

INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (24,'ADMINISTER LS','Administer LS', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (25,'EXECUTE EXPERIMENT','Execute Experiment', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (26,'CREATE EXPERIMENT','Create Experiment', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (27,'REDEEM RESERVATION','Redeem Reservation', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (28,'AUTHORIZE ACCESS','Authorize Access', 0);
INSERT INTO Ticket_Type(Ticket_Type_ID, Name, Short_Description, Abstract) VALUES (29,'AUTHORIZE CLIENT','Authorize Client', 0);


SET IDENTITY_INSERT Ticket_Type OFF

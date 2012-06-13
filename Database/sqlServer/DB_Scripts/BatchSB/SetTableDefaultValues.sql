-- Copyright (c) 2004 The Massachusetts Institute of Technology. All rights reserved.
-- $Id$

/* created by Charu. Last Modified 12/16/2004 */

/* ENTERING DEFAULT VALUES IN TABLES */

/* AUTHENTICATION_TYPES */
INSERT INTO Authentication_Types(description) VALUES ('Native');
INSERT INTO Authentication_Types(description) VALUES ('Kerberos_MIT');

/* CLIENT_TYPES */
INSERT INTO Client_Types(description) VALUES ('Applet Client');
INSERT INTO Client_Types(description) VALUES ('HTML Redirect Client');

/* FUNCTIONS */
INSERT INTO Functions (Function_Name, Description) VALUES ('addMember', 'allows one to add members to a group');
INSERT INTO Functions (Function_Name, Description) VALUES ('administerGroup', 'allows one to administer a group');
INSERT INTO Functions (Function_Name, Description) VALUES ('anyone', 'anyone can execute the method');
INSERT INTO Functions (Function_Name, Description) VALUES ('owner', 'only the owner of the designated item may execute the method');
INSERT INTO Functions (Function_Name, Description) VALUES ('readExperiment', 'allows one to read an experiment');
INSERT INTO Functions (Function_Name, Description) VALUES ('trusted', 'method only called during the execution of trusted administrative code');
INSERT INTO Functions (Function_Name, Description) VALUES ('useLabClient', 'allows one to use a lab client');
INSERT INTO Functions (Function_Name, Description) VALUES ('useLabServer', 'allows one to use a lab server');
INSERT INTO Functions (Function_Name, Description) VALUES ('writeExperiment', 'allows one to write to an experiment');
INSERT INTO Functions (Function_Name, Description) VALUES ('createExperiment', 'allows one to create an experiment');
INSERT INTO Functions (Function_Name, Description) VALUES ('useLabScheduling', 'allows one to configure a Lab Scheduing Service');
INSERT INTO Functions (Function_Name, Description) VALUES ('useUserScheduling', 'allows one to Schdule an experiment');
/* GROUP_TYPES */
INSERT INTO Group_Types(description) VALUES ('Regular Group');
INSERT INTO Group_Types(description) VALUES ('Request Group');
INSERT INTO Group_Types(description) VALUES ('Course Staff Group');
DBCC CHECKIDENT (Group_Types, RESEED, -1) ;
INSERT INTO Group_Types(description) VALUES ('Non-existent Group');
DBCC CHECKIDENT (Group_Types, RESEED, 3) ;

/* GROUPS & CORRESPONDING AGENTS*/
BEGIN
DECLARE @Agent_ID NUMERIC
DECLARE @Parent_Group_ID NUMERIC

INSERT INTO Agents (Agent_Name, Is_Group) VALUES ('ROOT', 1);
SELECT @Agent_ID = (SELECT ident_current('Agents'));
INSERT INTO Groups(Group_ID, Group_Name, description, group_type_ID) VALUES (@Agent_ID, 'ROOT','Root Group', 1);

INSERT INTO Agents (Agent_Name, Is_Group) VALUES ('NewUserGroup', 1);
SELECT @Agent_ID = (SELECT ident_current('Agents'));
SELECT @Parent_Group_ID = (SELECT Group_ID FROM Groups WHERE Group_Name = 'ROOT');
INSERT INTO Groups(Group_ID, Group_Name, description, group_type_ID) VALUES (@Agent_ID, 'NewUserGroup','New registered users who have not been moved to any group yet', 1);
INSERT INTO Agent_Hierarchy (Agent_ID, Parent_Group_ID) VALUES(@Agent_ID, @Parent_Group_ID);

INSERT INTO Agents (Agent_Name, Is_Group) VALUES ('OrphanedUserGroup', 1);
SELECT @Agent_ID = (SELECT ident_current('Agents'));
SELECT @Parent_Group_ID = (SELECT Group_ID FROM Groups WHERE Group_Name = 'ROOT');
INSERT INTO Groups(Group_ID, Group_Name, description, group_type_ID) VALUES (@Agent_ID,'OrphanedUserGroup','Users who no longer belong to any group',1);
INSERT INTO Agent_Hierarchy (Agent_ID, Parent_Group_ID) VALUES(@Agent_ID, @Parent_Group_ID);

INSERT INTO Agents (Agent_Name, Is_Group) VALUES ('SuperUserGroup', 1);
SELECT @Agent_ID = (SELECT ident_current('Agents'));
SELECT @Parent_Group_ID = (SELECT Group_ID FROM Groups WHERE Group_Name = 'ROOT');
INSERT INTO Groups(Group_ID, Group_Name, description, group_type_ID) VALUES (@Agent_ID,'SuperUserGroup','Administrators',1);
INSERT INTO Agent_Hierarchy (Agent_ID, Parent_Group_ID) VALUES(@Agent_ID, @Parent_Group_ID);

DBCC CHECKIDENT (AGENTS, RESEED, -1) ;
INSERT INTO Agents (Agent_Name, Is_Group) VALUES ('Group not assigned', 1);
INSERT INTO Groups(Group_ID, Group_Name, description, group_type_ID) VALUES (0, 'Group not assigned','If a groupID does not exist. This is an illegal group.',0);
END

UPDATE GROUPS SET associated_group_id = 0
DBCC CHECKIDENT (AGENTS, RESEED, 10) ;

/* LAB_SERVERS */

DBCC CHECKIDENT (LAB_SERVERS, RESEED, 0) ;
INSERT INTO Lab_Servers (GUID, Lab_Server_Name, Web_Service_URL, description, contact_first_name, contact_last_name, contact_email) VALUES (0,'Unknown Lab Server', 'none', 'This is to generate an id for a non-existent or unknown lab server, which can then be used as a foreign key in other tables such as System_Messages', 'ilab', 'ilab', 'ilab-debug@mit.edu');

DBCC CHECKIDENT (LAB_SERVERS, RESEED) ;


/* MESSAGE_TYPES */
INSERT INTO Message_Types(description) VALUES ('Lab');
INSERT INTO Message_Types(description) VALUES ('Group');
INSERT INTO Message_Types(description) VALUES ('System');

/* QUALIFIER_TYPES */
INSERT INTO Qualifier_Types(Qualifier_Type_ID, description) VALUES (0,'Null Qualifier Type');
INSERT INTO Qualifier_Types(Qualifier_Type_ID, description) VALUES (1,'Root');
INSERT INTO Qualifier_Types(Qualifier_Type_ID, description) VALUES (2,'Lab Client');
INSERT INTO Qualifier_Types(Qualifier_Type_ID, description) VALUES (3,'Lab Server');
INSERT INTO Qualifier_Types(Qualifier_Type_ID, description) VALUES (4,'Experiment');
INSERT INTO Qualifier_Types(Qualifier_Type_ID, description) VALUES (5,'Group');
INSERT INTO Qualifier_Types(Qualifier_Type_ID, description) VALUES (6,'Experiment Collection');
INSERT INTO Qualifier_Types(Qualifier_Type_ID, description) VALUES (7,'Lab Scheduling');
INSERT INTO Qualifier_Types(Qualifier_Type_ID, description) VALUES (8,'User Scheduling');


/* QUALIFIERS & QUALIFIER HIERARCHY */
DBCC CHECKIDENT (QUALIFIERS, RESEED, 0) ;
INSERT INTO Qualifiers(Qualifier_Type_ID, Qualifier_Reference_ID, Qualifier_Name) VALUES (0,0, 'Null Qualifier'); 
INSERT INTO Qualifiers(Qualifier_Type_ID, Qualifier_Reference_ID, Qualifier_Name) VALUES (1, 0,'ROOT');

BEGIN
DECLARE @GroupReference NUMERIC

SELECT  @GroupReference = (SELECT Group_ID FROM Groups where Group_Name='SuperUserGroup')
INSERT INTO Qualifiers(Qualifier_Type_ID, Qualifier_Reference_ID, Qualifier_Name) VALUES (5,@GroupReference ,'SuperUserGroup'); 

SELECT  @GroupReference = (SELECT Group_ID FROM Groups where Group_Name='NewUserGroup')
INSERT INTO Qualifiers(Qualifier_Type_ID, Qualifier_Reference_ID, Qualifier_Name) VALUES (5,@GroupReference,'NewUserGroup'); 

SELECT  @GroupReference = (SELECT Group_ID FROM Groups where Group_Name='OrphanedUserGroup')
INSERT INTO Qualifiers(Qualifier_Type_ID, Qualifier_Reference_ID, Qualifier_Name) VALUES (5,@GroupReference,'OrphanedUserGroup');

DBCC CHECKIDENT (QUALIFIERS, RESEED, 100) ;

END

/* Orphaned User group is a member of New User Group */
INSERT INTO Qualifier_Hierarchy (Qualifier_ID, Parent_Qualifier_ID) VALUES (4, 3);
/*Super User group is a member of Root */
INSERT INTO Qualifier_Hierarchy (Qualifier_ID, Parent_Qualifier_ID) VALUES (2, 1);

/* USERS & CORRESPONDING AGENTS & PRINCIPALS */
/* Default SuperUser password is ilab */
INSERT INTO Agents (Agent_Name, Is_Group) VALUES ('superUser', 0);
select @Agent_ID = (select ident_current('Agents'))
INSERT INTO Users (User_ID, User_Name, First_Name, Last_Name, Email, Affiliation, password, signup_reason) VALUES
(@Agent_ID, 'superUser', 'Super', 'User', 'ilab-debug@mit.edu', 'Other', '3759F4FF14D8494DF3B58671FF9251A9D0C41D54', 'Default Value');

INSERT INTO Principals (User_ID, Principal_String, Auth_Type_ID) VALUES (@Agent_ID, 'superUser', 1);

SELECT @Parent_Group_ID = (SELECT Group_ID FROM Groups WHERE Group_Name = 'SuperUserGroup');
INSERT INTO Agent_Hierarchy (Agent_ID, Parent_Group_ID) VALUES (@Agent_ID, @Parent_Group_ID);
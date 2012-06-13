/****** created by Charu. Last Modified 1/29/2004 *****/

/****** IMPORTANT - PLEASE READ ALL THE INSTRUCTIONS BEFORE YOU RUN THIS SCRIPT ******/

/****** INSTRUCTIONS ******/

/****** RUN THIS SCRIPT IF YOU WANT TO RESET THE VALUES IN THE DATABASE TO THE DEFAULT, WORKING SETTINGS. 
THIS WILL RETAIN SOME RECORDS IN TABLES  

2. AUTHENTICATION_TYPES
4. FUNCTIONS
5. QUALIFIER_TYPES
9. LAB_CLIENTS
10. LAB_SERVERS
11. SERVICE_BROKERs
12. LABSERVER_TO_CLIENTS_MAP


THE FOLLOWING TABLES WILL HAVE SOME DATA LEFT BEHIND (EXCEPT FOR THE VALUES MENTIONED BELOW, ALL OTHER DATA WILL BE DELETED)
-1. AGENTS 
(values left behind) - all records with Agent_ID  :
	- NewUserGroup
	- OrphanedUserGroup
	- SuperUserGroup
	- superUser


2. AGENT_HIERARCHY	
(values left behind) - all records with Agent_ID  :
	- NewUserGroup
	- OrphanedUserGroup
	- SuperUserGroup
	- superUser


3. GRANTS
(values left behind) - all records with Agent_ID  :
	- NewUserGroup
	- OrphanedUserGroup
	- SuperUserGroup
	- superUser

4. GROUPS	
(values left behind) - all records with group_ID  :
	- NewUserGroup
	- OrphanedUserGroup
	- SuperUserGroup


6. USERS
(values left behind) - all records with user_ID  :
	- superUser

7. PRINCIPALS (	through Cascade Delete from the Users table) - Identity column cannot be reset.
(values left behind) - all records with user_ID  :
	- superUser


7. QUALIFIERS - Identity column cannot be reset.
(values left behind) - all records with qualifier_ID, qualifier_reference_id  :
	- 0. root
	- 38, null qualifier 
	- 60, experiments (experiment_collection)
	- 78, SuperUserGroup
	- 80, NewUserGroup
	- 82, OrphanedUserGroup

8. QUALIFIER_HIERARCHY (through Cascade Delete from the Qualifiers table)
(values left behind) - all records with qualifier_ID, qualifier_reference_id  :
	- 0. root
	- 38, null qualifier 
	- 60, experiments (experiment_collection)
	- 78, SuperUserGroup
	- 80, NewUserGroup
	- 82, OrphanedUserGroup
	- 84, WebLab Client
	- 110, 6.012_students (group)
	- 111, 6.012_students (experiment_collection)
	- 140, 6.012_students-request (group)
	- 141, 6.012_students-request (experiment_collection)


ALL OTHER TABLES WILL BE CLEARED OF THEIR VALUES & AUTO INDICES FOR PRIMARY KEYS WILL BE RESET TO START FROM 1. 

-- END OF INSTRUCTIONS ******/


/***** START OF SCRIPT *****/

/****** DELETING SOME VALUES FROM THE FOLLOWING TABLES ******/

/* AGENTS*/
DELETE FROM AGENTS WHERE AGENT_ID NOT IN ('6.012_students', '6.012_students-request', 'NewUserGroup', 'OrphanedUserGroup', 'SuperUserGroup', 'superUser', 'johnDoe', 'Experiment_Collection_User');

/* AGENT_HIERARCHY */
DELETE FROM AGENT_HIERARCHY WHERE AGENT_ID NOT IN ('6.012_students', '6.012_students-request', 'NewUserGroup', 'OrphanedUserGroup', 'SuperUserGroup', 'superUser', 'johnDoe', 'Experiment_Collection_User');

/* GRANTS */
DELETE FROM GRANTS WHERE AGENT_ID NOT IN ('6.012_students', '6.012_students-request', 'NewUserGroup', 'OrphanedUserGroup', 'SuperUserGroup', 'superUser', 'johnDoe', 'Experiment_Collection_User');

/* GROUPS */
DELETE FROM GROUPS WHERE GROUP_ID NOT IN ('6.012_students', '6.012_students-request', 'NewUserGroup', 'OrphanedUserGroup', 'SuperUserGroup');

/* USERS */
DELETE FROM USERS WHERE USER_ID NOT IN ('superUser', 'johnDoe', 'Experiment_Collection_User');

/* QUALIFIERS */
DELETE FROM QUALIFIERS WHERE QUALIFIER_ID NOT IN (0,38,60,78,80,82,84,110,111,140,141);

/***** DELETING ALL VALUES FROM THE FOLLOWING TABLES *****/

/* ACCOUNTS */
DELETE FROM ACCOUNTS;

/* CLIENT_ITEMS */
DELETE FROM CLIENT_ITEMS;
DBCC CHECKIDENT (CLIENT_ITEMS, RESEED, 0) ;

/* EXPERIMENTS */
DELETE FROM EXPERIMENTS;
DBCC CHECKIDENT (EXPERIMENTS, RESEED, 0) ;

/***** EXPERIMENT_RESULTS, EXPERIMENT_BLOBS AND RESULT_MESSAGES GET EMPTIED BECAUSE OF CASCADE_DELETE - THE 3 FOLLOWING STATEMENTS ARE NOT REQUIRED EXCEPT TO RESET INDICES TO 0 *****/
/* EXPERIMENT_RESULTS */
DELETE FROM EXPERIMENT_RESULTS;
DBCC CHECKIDENT (EXPERIMENT_RESULTS, RESEED, 0) ;

/* RESULT_MESSAGES */
DELETE FROM RESULT_MESSAGES;
DBCC CHECKIDENT (RESULT_MESSAGES, RESEED, 0) ;

/* EXPERIMENT_BLOBS */
DELETE FROM EXPERIMENT_BLOBS;
DBCC CHECKIDENT (EXPERIMENT_BLOBS, RESEED, 0) ;



/* STORED_ITEM_SUMMARY */
DELETE FROM STORED_ITEM_SUMMARY;
DBCC CHECKIDENT (STORED_ITEM_SUMMARY, RESEED, 0) ;

/* SYSTEM_MESSAGES */
DELETE FROM SYSTEM_MESSAGES;
DBCC CHECKIDENT (SYSTEM_MESSAGES, RESEED, 0) ;
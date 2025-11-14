/*WARNING: This script MAY TRUNCATE AND DELETE DATA from tables prior to inserting data.
	Dependents: 
		1. Destination Database being inserted into must be created.  NOTE: Any pre-existing data in the affected tables will be deleted.
		2. EF Migrations should have already executed and created all entity tables

		FootballSimulation Databases:
		DEV/LOCAL/PROD: FootballSimulation
*/
USE [FootballSimulation]
GO

--MAKE SURE SYSTEM Administrator EXISTS:
IF NOT (EXISTS(select * from users where [user_name] = 'admin'))
BEGIN
	SET IDENTITY_INSERT dbo.users ON;

	INSERT INTO USERS (id, [user_name], email, first_name, last_name, [guid])
	SELECT 1, 'admin', 'admin@acme.com', 'System', 'Administrator', '62939B8A-24B8-4618-BF52-9B7A3985060F'

	SET IDENTITY_INSERT dbo.users OFF
END

--MAKE SURE THE DATABASE MIGRATIONS EXIST:
IF NOT (EXISTS(select 1 from __EFMigrationsHistory))
BEGIN
	PRINT 'EF Migrations have not been applied to this database.  Please run the EF Migrations prior to running this script.'
	RETURN
END


--ONLY ALLOW THE QUERIES BELOW TO CONTINUE IF THE SYSTEM Administrator EXISTS:
IF (EXISTS(select * from users where [user_name] = 'admin'))
BEGIN
	PRINT 'System Administrator exists, script can continue...'	

	--VIEW ALL THE USERS IN DATABASE
		SELECT * 
		FROM users u
			left join user_roles ur on u.id = ur.[user_id]
			left join roles r on ur.role_id = r.id

	--INSERT ALL THE USERS THAT DO NOT HAVE A ROLE (NOTE: THIS MAKES THEM AN ADMIN, SINCE THAT'S THE ONLY ROLE AVAILABLE ANYWAY)
		INSERT INTO user_roles(role_id, [user_id])
		SELECT 1, id
		FROM users u
			left join user_roles ur on u.id = ur.[user_id]
		WHERE ur.[user_id] is null

	--VIEW USER LOGIN HISTORY:
		SELECT * 
		FROM user_login_histories h
			join users u on h.[user_id] = u.id
END
ELSE
BEGIN
	PRINT 'System Administrator does not exist, script will not continue. Make sure the users table contains the user with [user_name]: admin.  If it exists, other troubleshooting is required.'
END
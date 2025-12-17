/*WARNING: This script MAY TRUNCATE AND DELETE DATA from tables prior to inserting data.
	Dependents: 
		1. Destination Database being inserted into must be created.  NOTE: Any pre-existing data in the affected tables will be deleted.
		2. EF Migrations should have already executed and created all entity tables

		Football Simulator Databases:
		DEV/LOCAL/PROD: Football_Simulator
*/
USE [Football_Simulator]
GO

--MAKE SURE SYSTEM Administrator EXISTS:
IF NOT (EXISTS(select * from users where [user_name] = 'admin'))
BEGIN
	SET IDENTITY_INSERT dbo.users ON;

	INSERT INTO USERS (id, [user_name], email, first_name, last_name, [guid])
	SELECT 1, 'admin', 'admin@footballsimulator.com', 'System', 'Administrator', '36744C51-0928-40EF-AAA0-6B4FE2B42908'

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

	BEGIN
	PRINT N'Adding countries...'
		INSERT INTO countries ([name], code)
		select 'United States' as [name], 'USA' as code UNION
		--COPY OTHER STATEMENTS GENERATED FROM BELOW QUERY
		--LOOKUP QUERY FROM OLD DB:
		--select 'select ''' + t.[name] + ''' as [name], ''' + t.[code] + ''' as code UNION', * from FOOTBALL_SIM.dbo.countries t where t.name is not null and t.code is not null
	END
	BEGIN
	PRINT N'Adding states...'
		INSERT INTO states ([name],abbreviation,fips,country_id)
		SELECT 'Alabama' AS [name], 'AL' AS abbreviation, '01' AS fips, (select co.id from countries co where co.code = 'USA') as country_id UNION
		--COPY OTHER STATEMENTS GENERATED FROM BELOW QUERY
		--LOOKUP QUERY FROM OLD DB:
		--SELECT 'SELECT ''' + t.[name] + ''' AS [name], ' + '''' + t.abbreviation + ''' AS abbreviation, ' + CASE WHEN t.fips IS NULL THEN 'NULL' ELSE '''' + t.fips + '''' END + ' AS fips, (select co.id from countries co where co.code = ''' + tco.code + ''') as country_id UNION', * FROM FOOTBALL_SIM.dbo.states t join FOOTBALL_SIM.dbo.countries tco on t.country_id = tco.id WHERE t.name IS NOT NULL AND t.abbreviation IS NOT NULL;
	END
	BEGIN
	PRINT N'Adding cities...'
		create table FOOTBALL_SIM.dbo._temp_cities ([name] nvarchar(200),prev_state_id int, new_state_id int)
		insert into FOOTBALL_SIM.dbo._temp_cities ([name], prev_state_id)
		select [name], state_id
		from FOOTBALL_SIM.dbo.cities

		--map previous state id to new state id
		UPDATE t set t.new_state_id = new_st.id
		from FOOTBALL_SIM.dbo._temp_cities t
			join FOOTBALL_SIM.dbo.states prev_st on t.prev_state_id = prev_st.id
			join Football_Simulator.dbo.states new_st on prev_st.abbreviation = new_st.abbreviation

		--INSERT INTO FINAL TABLE:
		INSERT INTO cities ([name], state_id)
		select [name], new_state_id
		from FOOTBALL_SIM.dbo._temp_cities t where t.new_state_id is not null
	END
	BEGIN
	PRINT N'Adding climate types...'
		INSERT INTO climate_types ([name], [description])
		SELECT 'temperate' AS [name], 'Have distinct seasons with moderate temperatures and are found in mid-latitudes.         Subtypes: Mediterranean (warm, dry summers and mild, wet winters), humid subtropical, and oceanic/marine west coast climates.   ' AS [description] UNION
		SELECT 'dry' AS [name], 'Characterized by low precipitation, with hot or cold variations.         Subtypes: Desert (arid) and semi-arid (stretching into steppe regions).   ' AS [description] UNION
		SELECT 'tropical' AS [name], 'These are warm, wet climates found near the equator.         Subtypes: Rainforest (consistently wet) and savanna (wet and dry seasons).   ' AS [description] UNION
		SELECT 'continental' AS [name], 'Feature large temperature differences between summer and winter, with cold winters and hot summers.         Subtypes: Humid continental and subarctic climates.   ' AS [description] UNION
		SELECT 'polar' AS [name], 'These are very cold climates found at the North and South poles.         Subtypes: Tundra (permafrost) and ice cap (ice-covered regions).   ' AS [description]
		--LOOKUP QUERY FROM OLD DB:
		--SELECT 'SELECT ''' + t.[name] + ''' AS [name], ' + '''' + t.[description] + ''' AS [description] UNION', * FROM FOOTBALL_SIM.dbo.climate_types t 
	END
	BEGIN
	PRINT N'Adding weather types...'
		INSERT INTO weather_types ([name])
		SELECT 'Clear' AS [name] UNION
		SELECT 'Cloudy' AS [name] UNION
		SELECT 'Rain' AS [name] UNION
		SELECT 'Heavy Rain' AS [name] UNION
		SELECT 'Snow' AS [name]
		--LOOKUP QUERY FROM OLD DB:
		--SELECT 'SELECT ''' + t.[name] + ''' AS [name] UNION', * FROM FOOTBALL_SIM.dbo.weather_types t 
	END
	BEGIN
	PRINT N'Adding climate weather types...'
		INSERT INTO climate_type_weather_types(climate_type_id,weather_type_id)
		SELECT (select cl.id from climate_types cl where cl.[name] = 'continental') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Clear') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'continental') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Cloudy') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'continental') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Heavy Rain') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'continental') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Rain') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'continental') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Snow') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'dry') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Clear') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'dry') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Cloudy') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'dry') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Rain') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'temperate') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Clear') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'temperate') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Cloudy') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'temperate') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Heavy Rain') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'temperate') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Rain') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'temperate') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Snow') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'tropical') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Clear') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'tropical') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Cloudy') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'tropical') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Heavy Rain') as weather_type_id UNION
		SELECT (select cl.id from climate_types cl where cl.[name] = 'tropical') as climate_type_id,(select w.id from weather_types w where w.[name] = 'Rain') as weather_type_id

		--LOOKUP QUERY FROM OLD DB:
		--SELECT DISTINCT 'SELECT (select cl.id from climate_types cl where cl.[name] = ''' + cl.[name] + ''') as climate_type_id,(select w.id from weather_types w where w.[name] = ''' + w.[name] + ''') as weather_type_id UNION', cl.[name] as climate_type, w.[name] as weather_type FROM FOOTBALL_SIM.dbo.stadium_weather_types swt join FOOTBALL_SIM.dbo.stadiums st on st.id = swt.stadium_id join FOOTBALL_SIM.dbo.climate_types cl on st.climate_type_id = cl.id join FOOTBALL_SIM.dbo.weather_types w on swt.weather_type_id = w.id
	END
	BEGIN
	PRINT N'Adding stadium types...'
		INSERT INTO stadium_types ([name])
		SELECT 'open-air' AS [name] UNION
		SELECT 'enclosed' AS [name] UNION
		SELECT 'retractable-roof' AS [name] UNION
		SELECT 'translucent' AS [name]
		--LOOKUP QUERY FROM OLD DB:
		--SELECT 'SELECT ''' + t.[name] + ''' AS [name] UNION', * FROM FOOTBALL_SIM.dbo.stadium_types t 
	END
	BEGIN
	PRINT N'Adding stadiums...'
		INSERT INTO stadiums (name, capacity, city_id, stadium_type_id, climate_type_id, is_super_bowl_candidate, is_international_match_candidate, broke_ground, opened, archive, created_date, created_user_id, updated_date, updated_user_id)
		SELECT 'Placeholder Stadium' as stadium_name, 50000 as capacity, (select ci.id from cities ci join states st on ci.state_id = st.id where ci.[name] = 'Honolulu' and st.abbreviation = 'HI') as city_id, (select st.id from stadium_types st where st.[name] = 'open-air') as stadium_type_id, (select ct.id from climate_types ct where ct.[name] = 'tropical') as climate_type_id, 1 as is_super_bowl_candidate, 0 as is_international_match_candidate, '1960-01-01 00:00:00.000' as broke_ground, '1965-09-12 00:00:00.000' as opened, 1 as archive, '2014-11-10 09:08:55.490' as created_date, 1 as created_user_id, getutcdate() as updated_date, 1 as updated_user_id UNION
		--COPY OTHER STATEMENTS GENERATED FROM BELOW QUERY
		--LOOKUP QUERY FROM OLD DB:
		SELECT DISTINCT 'SELECT ''' + REPLACE(s.[stadium_name],'''', '''''') + ''' as stadium_name, ' + 
			convert(nvarchar(10), s.max_capacity) + ' as capacity, (select ci.id from cities ci join states st on ci.state_id = st.id where ci.[name] = ''' + ci.[name] + ''' and st.abbreviation = ''' + states.abbreviation + ''') as city_id, 
			(select st.id from stadium_types st where st.[name] = ''' + st.[name] + ''') as stadium_type_id, ' + 
			'(select ct.id from climate_types ct where ct.[name] = ''' + ct.[name] + ''') as climate_type_id, ' + 
			convert(nvarchar(1),s.is_superbowl_candidate) + ' as is_super_bowl_candidate, ' + 
			convert(nvarchar(1),s.is_international) + ' as is_international_match_candidate, ' +
			'''' + convert(varchar(23), s.broke_ground_date, 121) + ''' as broke_ground, ' +
			'''' + convert(varchar(23), s.open_date, 121) + ''' as opened, ' +
			convert(nvarchar(1),s.archived) + ' as archive, ' +
			'''' + convert(varchar(23), s.date_created, 121) + ''' as created_date, 1 as created_user_id, getutcdate() as updated_date, 1 as updated_user_id ' +
			'UNION'
			,* 
		FROM FOOTBALL_SIM.dbo.stadiums s
			JOIN FOOTBALL_SIM.dbo.climate_types ct on s.climate_type_id = ct.id
			JOIN FOOTBALL_SIM.dbo.stadium_types st on s.stadium_type_id = st.id
			JOIN FOOTBALL_SIM.dbo.cities ci on s.city_id = ci.id
			JOIN FOOTBALL_SIM.dbo.states on ci.state_id = states.id
	END
END
ELSE
BEGIN
	PRINT 'System Administrator does not exist, script will not continue. Make sure the users table contains the user with [user_name]: admin.  If it exists, other troubleshooting is required.'
END
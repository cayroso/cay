use master
go

if exists(select 1 from sys.databases where name='db')
	drop database db
go

create database db
go

use db
go

if  exists (select * from sys.server_principals where name = N'sqlAdmin')
	drop login [sqlAdmin]
go
create login [sqlAdmin] with password = N'12345a$', check_policy = off;
go

if  exists (select * from sys.server_principals where name = N'sqlUser')
	drop login [sqlUser]
go
create login [sqlUser] with password = N'12345a$', check_policy = off;
go


create user [sqlAdmin] for login [sqlAdmin] --with default_schema=[dbo]
go
create user [sqlUser] for login [sqlUser] --with default_schema=[dbo]
go

create schema [core]  authorization [sqlAdmin]
grant insert, select, [update], delete on schema ::dbo to sqlAdmin
go
--grant insert, select, update, delete on schema ::core to sqlAdmin
go

--grant execute on schema ::core to sqlUser

grant alter on schema :: dbo to sqlAdmin
grant create table to sqlAdmin


--	//	start of database objects


create table core.Users(
	Id bigint not null identity(1,1) constraint pk_user primary key clustered
	, UserName nvarchar(64) not null	
)
go


--	// end of database objects

use master
go







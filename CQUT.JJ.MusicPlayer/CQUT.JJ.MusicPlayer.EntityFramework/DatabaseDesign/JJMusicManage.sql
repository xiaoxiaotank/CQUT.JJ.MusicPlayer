/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2012                    */
/* Created on:     2018/3/16 12:44:16                           */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Permission') and o.name = 'FK_PERMISSI_FK_PERMIS_ROLE')
alter table Permission
   drop constraint FK_PERMISSI_FK_PERMIS_ROLE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Permission') and o.name = 'FK_PERMISSI_FK_PERMIS_USER')
alter table Permission
   drop constraint FK_PERMISSI_FK_PERMIS_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('UserRole') and o.name = 'FK_USERROLE_FK_USERRO_ROLE')
alter table UserRole
   drop constraint FK_USERROLE_FK_USERRO_ROLE
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('UserRole') and o.name = 'FK_USERROLE_FK_USERRO_USER')
alter table UserRole
   drop constraint FK_USERROLE_FK_USERRO_USER
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Menu')
            and   type = 'U')
   drop table Menu
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Permission')
            and   type = 'U')
   drop table Permission
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Role')
            and   type = 'U')
   drop table Role
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"User"')
            and   type = 'U')
   drop table "User"
go

if exists (select 1
            from  sysobjects
           where  id = object_id('UserRole')
            and   type = 'U')
   drop table UserRole
go

/*==============================================================*/
/* Table: Menu                                                  */
/*==============================================================*/
create table Menu (
   Id                   int                  identity,
   Header               nvarchar(8)          not null,
   TargetUrl            varchar(128)         null,
   ParentId             int                  not null default 0,
   RequiredAuthorizeCode varchar(256)         null,
   Priority             smallint             not null,
   CreationTime         datetime             not null,
   LastModificationTime datetime             null,
   constraint PK_MENU primary key (Id)
)
go

/*==============================================================*/
/* Table: Permission                                            */
/*==============================================================*/
create table Permission (
   Id                   int                  identity,
   UserId               int                  null,
   RoleId               int                  null,
   Code                 varchar(256)         not null,
   CreationTime         datetime             not null,
   constraint PK_PERMISSION primary key (Id)
)
go

/*==============================================================*/
/* Table: Role                                                  */
/*==============================================================*/
create table Role (
   Id                   int                  identity,
   Name                 nvarchar(8)          not null,
   IsDefault            bit                  not null default 0,
   LastModificationTime datetime             null,
   CreationTime         datetime             not null,
   IsDeleted            bit                  not null default 0,
   DeletionTime         datetime             null,
   constraint PK_ROLE primary key (Id)
)
go

/*==============================================================*/
/* Table: "User"                                                */
/*==============================================================*/
create table "User" (
   Id                   int                  identity,
   UserName             varchar(32)          not null,
   Password             varchar(32)          not null,
   NickName             nvarchar(8)          not null,
   IsAdmin              bit                  not null default 0,
   LastModificationTime datetime             null,
   CreationTime         datetime             not null,
   IsDeleted            bit                  not null default 0,
   DeletionTime         datetime             null,
   constraint PK_USER primary key (Id)
)
go

/*==============================================================*/
/* Table: UserRole                                              */
/*==============================================================*/
create table UserRole (
   UserId               int                  not null,
   RoleId               int                  not null,
   constraint PK_USERROLE primary key (UserId, RoleId)
)
go

alter table Permission
   add constraint FK_PERMISSI_FK_PERMIS_ROLE foreign key (RoleId)
      references Role (Id)
go

alter table Permission
   add constraint FK_PERMISSI_FK_PERMIS_USER foreign key (UserId)
      references "User" (Id)
go

alter table UserRole
   add constraint FK_USERROLE_FK_USERRO_ROLE foreign key (RoleId)
      references Role (Id)
go

alter table UserRole
   add constraint FK_USERROLE_FK_USERRO_USER foreign key (UserId)
      references "User" (Id)
go


/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2012                    */
/* Created on:     2018/4/3 17:07:12                            */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Album') and o.name = 'FK_ALBUM_REFERENCE_USER1')
alter table Album
   drop constraint FK_ALBUM_REFERENCE_USER1
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Album') and o.name = 'FK_ALBUM_REFERENCE_USER4')
alter table Album
   drop constraint FK_ALBUM_REFERENCE_USER4
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Album') and o.name = 'FK_ALBUM_REFERENCE_USER3')
alter table Album
   drop constraint FK_ALBUM_REFERENCE_USER3
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Album') and o.name = 'FK_ALBUM_REFERENCE_USER2')
alter table Album
   drop constraint FK_ALBUM_REFERENCE_USER2
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Album') and o.name = 'FK_ALBUM_REFERENCE_SINGER')
alter table Album
   drop constraint FK_ALBUM_REFERENCE_SINGER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Music') and o.name = 'FK_MUSIC_REFERENCE_SINGER')
alter table Music
   drop constraint FK_MUSIC_REFERENCE_SINGER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Music') and o.name = 'FK_MUSIC_REFERENCE_USER2')
alter table Music
   drop constraint FK_MUSIC_REFERENCE_USER2
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Music') and o.name = 'FK_MUSIC_REFERENCE_USER3')
alter table Music
   drop constraint FK_MUSIC_REFERENCE_USER3
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Music') and o.name = 'FK_MUSIC_REFERENCE_USER1')
alter table Music
   drop constraint FK_MUSIC_REFERENCE_USER1
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Music') and o.name = 'FK_MUSIC_REFERENCE_USER4')
alter table Music
   drop constraint FK_MUSIC_REFERENCE_USER4
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Music') and o.name = 'FK_MUSIC_REFERENCE_ALBUM')
alter table Music
   drop constraint FK_MUSIC_REFERENCE_ALBUM
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('MusicAttach') and o.name = 'FK_MUSICATT_REFERENCE_MUSIC')
alter table MusicAttach
   drop constraint FK_MUSICATT_REFERENCE_MUSIC
go

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
   where r.fkeyid = object_id('Singer') and o.name = 'FK_SINGER_REFERENCE_USER2')
alter table Singer
   drop constraint FK_SINGER_REFERENCE_USER2
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Singer') and o.name = 'FK_SINGER_REFERENCE_USER1')
alter table Singer
   drop constraint FK_SINGER_REFERENCE_USER1
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Singer') and o.name = 'FK_SINGER_REFERENCE_USER4')
alter table Singer
   drop constraint FK_SINGER_REFERENCE_USER4
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('Singer') and o.name = 'FK_SINGER_REFERENCE_USER3')
alter table Singer
   drop constraint FK_SINGER_REFERENCE_USER3
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('SingerAttach') and o.name = 'FK_SINGERAT_REFERENCE_SINGER')
alter table SingerAttach
   drop constraint FK_SINGERAT_REFERENCE_SINGER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('UserLike') and o.name = 'FK_USERLIKE_REFERENCE_USER')
alter table UserLike
   drop constraint FK_USERLIKE_REFERENCE_USER
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('UserLike') and o.name = 'FK_USERLIKE_REFERENCE_ALBUM')
alter table UserLike
   drop constraint FK_USERLIKE_REFERENCE_ALBUM
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('UserLike') and o.name = 'FK_USERLIKE_REFERENCE_MUSIC')
alter table UserLike
   drop constraint FK_USERLIKE_REFERENCE_MUSIC
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('UserMusicList') and o.name = 'FK_USERMUSI_REFERENCE_USER')
alter table UserMusicList
   drop constraint FK_USERMUSI_REFERENCE_USER
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
           where  id = object_id('Album')
            and   type = 'U')
   drop table Album
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Menu')
            and   type = 'U')
   drop table Menu
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Music')
            and   type = 'U')
   drop table Music
go

if exists (select 1
            from  sysobjects
           where  id = object_id('MusicAttach')
            and   type = 'U')
   drop table MusicAttach
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
           where  id = object_id('Singer')
            and   type = 'U')
   drop table Singer
go

if exists (select 1
            from  sysobjects
           where  id = object_id('SingerAttach')
            and   type = 'U')
   drop table SingerAttach
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"User"')
            and   type = 'U')
   drop table "User"
go

if exists (select 1
            from  sysobjects
           where  id = object_id('UserLike')
            and   type = 'U')
   drop table UserLike
go

if exists (select 1
            from  sysobjects
           where  id = object_id('UserMusicList')
            and   type = 'U')
   drop table UserMusicList
go

if exists (select 1
            from  sysobjects
           where  id = object_id('UserRole')
            and   type = 'U')
   drop table UserRole
go

/*==============================================================*/
/* Table: Album                                                 */
/*==============================================================*/
create table Album (
   Id                   int                  identity,
   SingerId             int                  not null,
   CreatorId            int                  not null,
   MenderId             int                  null,
   PublisherId          int                  null,
   UnpublisherId        int                  null,
   Name                 nvarchar(32)         not null,
   CreationTime         datetime             not null,
   LastModificationTime datetime             null,
   IsPublished          bit                  not null default 0,
   PublishmentTime      datetime             null,
   IsDeleted            bit                  not null,
   DeletionTime         datetime             null,
   constraint PK_ALBUM primary key (Id)
)
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
/* Table: Music                                                 */
/*==============================================================*/
create table Music (
   Id                   int                  identity,
   SingerId             int                  not null,
   AlbumId              int                  not null,
   CreatorId            int                  not null,
   MenderId             int                  null,
   PublisherId          int                  null,
   UnpublisherId        int                  null,
   Name                 nvarchar(32)         not null,
   Duration             time                 not null,
   FileUrl              varchar(256)         not null,
   CreationTime         datetime             not null,
   LastModificationTime datetime             null,
   IsPublished          bit                  not null default 0,
   PublishmentTime      datetime             null,
   IsDeleted            bit                  not null,
   DeletionTime         datetime             null,
   constraint PK_MUSIC primary key (Id)
)
go

/*==============================================================*/
/* Table: MusicAttach                                           */
/*==============================================================*/
create table MusicAttach (
   Id                   int                  identity,
   MusicId              int                  not null,
   CoverUrl             varchar(256)         null,
   Passion              int                  not null default 0
      constraint CKC_PASSION_MUSICATT check (Passion >= 0),
   constraint PK_MUSICATTACH primary key (Id)
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
/* Table: Singer                                                */
/*==============================================================*/
create table Singer (
   Id                   int                  identity,
   CreatorId            int                  not null,
   MenderId             int                  null,
   PublisherId          int                  null,
   UnpublisherId        int                  null,
   Name                 nvarchar(32)         not null,
   ForeignName          varchar(32)          null,
   Nationality          nvarchar(32)         not null,
   CreationTime         datetime             not null,
   LastModificationTime datetime             null,
   IsPublished          bit                  not null default 0,
   PublishmentTime      datetime             null,
   IsDeleted            bit                  not null,
   DeletionTime         datetime             null,
   constraint PK_SINGER primary key (Id)
)
go

/*==============================================================*/
/* Table: SingerAttach                                          */
/*==============================================================*/
create table SingerAttach (
   Id                   int                  identity,
   SingerId             int                  not null,
   FansNumber           int                  not null default 0
      constraint CKC_FANSNUMBER_SINGERAT check (FansNumber >= 0),
   constraint PK_SINGERATTACH primary key (Id)
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
/* Table: UserLike                                              */
/*==============================================================*/
create table UserLike (
   Id                   int                  identity,
   UserId               int                  not null,
   MusicId              int                  null,
   MusicListId          int                  null,
   AlbumId              int                  null,
   constraint PK_USERLIKE primary key (Id)
)
go

/*==============================================================*/
/* Table: UserMusicList                                         */
/*==============================================================*/
create table UserMusicList (
   Id                   int                  identity,
   UserId               int                  not null,
   Name                 nvarchar(16)         not null,
   CreationTime         datetime             not null,
   LastModificationTime datetime             null,
   IsDeleted            bit                  not null default 0,
   DeletionTime         datetime             null,
   constraint PK_USERMUSICLIST primary key (Id)
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

alter table Album
   add constraint FK_ALBUM_REFERENCE_USER1 foreign key (PublisherId)
      references "User" (Id)
go

alter table Album
   add constraint FK_ALBUM_REFERENCE_USER4 foreign key (CreatorId)
      references "User" (Id)
go

alter table Album
   add constraint FK_ALBUM_REFERENCE_USER3 foreign key (UnpublisherId)
      references "User" (Id)
go

alter table Album
   add constraint FK_ALBUM_REFERENCE_USER2 foreign key (MenderId)
      references "User" (Id)
go

alter table Album
   add constraint FK_ALBUM_REFERENCE_SINGER foreign key (SingerId)
      references Singer (Id)
go

alter table Music
   add constraint FK_MUSIC_REFERENCE_SINGER foreign key (SingerId)
      references Singer (Id)
go

alter table Music
   add constraint FK_MUSIC_REFERENCE_USER2 foreign key (CreatorId)
      references "User" (Id)
go

alter table Music
   add constraint FK_MUSIC_REFERENCE_USER3 foreign key (MenderId)
      references "User" (Id)
go

alter table Music
   add constraint FK_MUSIC_REFERENCE_USER1 foreign key (UnpublisherId)
      references "User" (Id)
go

alter table Music
   add constraint FK_MUSIC_REFERENCE_USER4 foreign key (PublisherId)
      references "User" (Id)
go

alter table Music
   add constraint FK_MUSIC_REFERENCE_ALBUM foreign key (AlbumId)
      references Album (Id)
go

alter table MusicAttach
   add constraint FK_MUSICATT_REFERENCE_MUSIC foreign key (MusicId)
      references Music (Id)
go

alter table Permission
   add constraint FK_PERMISSI_FK_PERMIS_ROLE foreign key (RoleId)
      references Role (Id)
go

alter table Permission
   add constraint FK_PERMISSI_FK_PERMIS_USER foreign key (UserId)
      references "User" (Id)
go

alter table Singer
   add constraint FK_SINGER_REFERENCE_USER2 foreign key (CreatorId)
      references "User" (Id)
go

alter table Singer
   add constraint FK_SINGER_REFERENCE_USER1 foreign key (MenderId)
      references "User" (Id)
go

alter table Singer
   add constraint FK_SINGER_REFERENCE_USER4 foreign key (PublisherId)
      references "User" (Id)
go

alter table Singer
   add constraint FK_SINGER_REFERENCE_USER3 foreign key (UnpublisherId)
      references "User" (Id)
go

alter table SingerAttach
   add constraint FK_SINGERAT_REFERENCE_SINGER foreign key (SingerId)
      references Singer (Id)
go

alter table UserLike
   add constraint FK_USERLIKE_REFERENCE_USER foreign key (UserId)
      references "User" (Id)
go

alter table UserLike
   add constraint FK_USERLIKE_REFERENCE_ALBUM foreign key (AlbumId)
      references Album (Id)
go

alter table UserLike
   add constraint FK_USERLIKE_REFERENCE_MUSIC foreign key (MusicId)
      references Music (Id)
go

alter table UserMusicList
   add constraint FK_USERMUSI_REFERENCE_USER foreign key (UserId)
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


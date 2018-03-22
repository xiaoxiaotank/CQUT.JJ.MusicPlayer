/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2012                    */
/* Created on:     2018/3/21 15:29:05                           */
/*==============================================================*/


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
   where r.fkeyid = object_id('SingerAttach') and o.name = 'FK_SINGERAT_REFERENCE_SINGER')
alter table SingerAttach
   drop constraint FK_SINGERAT_REFERENCE_SINGER
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Album')
            and   type = 'U')
   drop table Album
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

/*==============================================================*/
/* Table: Album                                                 */
/*==============================================================*/
create table Album (
   Id                   int                  not null,
   SingerId             int                  not null,
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
/* Table: Music                                                 */
/*==============================================================*/
create table Music (
   Id                   int                  not null,
   SingerId             int                  not null,
   AlbumId              int                  not null,
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
   Id                   int                  not null,
   MusicId              int                  not null,
   CoverUrl             varchar(256)         null,
   Passion              int                  not null default 0
      constraint CKC_PASSION_MUSICATT check (Passion >= 0),
   constraint PK_MUSICATTACH primary key (Id)
)
go

/*==============================================================*/
/* Table: Singer                                                */
/*==============================================================*/
create table Singer (
   Id                   int                  identity,
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
   Id                   int                  not null,
   SingerId             int                  not null,
   FansNumber           int                  not null default 0
      constraint CKC_FANSNUMBER_SINGERAT check (FansNumber >= 0),
   constraint PK_SINGERATTACH primary key (Id)
)
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
   add constraint FK_MUSIC_REFERENCE_ALBUM foreign key (AlbumId)
      references Album (Id)
go

alter table MusicAttach
   add constraint FK_MUSICATT_REFERENCE_MUSIC foreign key (MusicId)
      references Music (Id)
go

alter table SingerAttach
   add constraint FK_SINGERAT_REFERENCE_SINGER foreign key (SingerId)
      references Singer (Id)
go


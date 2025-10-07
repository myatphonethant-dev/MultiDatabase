create database TestDb;
use TestDb;

create table dbo.Tbl_Blog
(
    BlogId      int not null
        constraint Tbl_Blog_pk
            primary key,
    BlogTitle   varchar(50),
    BlogAuthor  varchar(50),
    BlogContent varchar(50)
)
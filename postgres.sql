create database testdb;

create table public.tbl_blog
(
    blog_id      serial
        primary key,
    blog_title   varchar(50),
    blog_author  varchar(50),
    blog_content varchar(50)
);

alter table public.tbl_blog
    owner to postgres;
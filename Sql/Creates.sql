create database LibraryDb;

use LibraryDb;

create table Books (
    [Id] int primary key identity,
    [Name] nvarchar(50) not null,
    [Author] nvarchar(50) not null,
    [Price] money,
)

create table Users(
    [Id] int primary key identity,
    [UserName] nvarchar(50) not null,
    [Email] nvarchar(50) not null,
    [Password] nvarchar(20) not null,

)
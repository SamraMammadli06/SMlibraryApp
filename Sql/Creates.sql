create database LibraryDb;

use LibraryDb;

create table Books (
    [Id] int primary key identity,
    [Name] nvarchar(50) not null,
    [Author] nvarchar(50) not null,
    [Price] money,
)
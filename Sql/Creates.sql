create database LibaryDb;

use LibaryDb;

create table Book (
    [Id] int primary key identity,
    [Name] nvarchar(50) not null,
    [Author] nvarchar(50) not null,
    [Price] money,
)
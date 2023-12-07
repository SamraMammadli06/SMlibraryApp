create database LibaryDb;

use LibaryDb;

create table Book (
    [Id] int primary key identity,
    [Name] nvarchar(50) not null,
    [Author] nvarchar(50) not null,
    [Price] money,
)

create table Users(
	Id int primary key identity,
	FullName nvarchar(50) not null,
	Email nvarchar(30),
	BookId int FOREIGN KEY REFERENCES Books(Id)
)
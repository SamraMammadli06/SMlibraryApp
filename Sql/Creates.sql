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

create table Loggings(
    [logId] int primary key identity,
	[userId ] int,
	[url] nvarchar(200),
	[methodType] nvarchar(200),
	[statusCode] int,
	[requestBody] nvarchar(255),
	[responseBody] nvarchar(255)
)
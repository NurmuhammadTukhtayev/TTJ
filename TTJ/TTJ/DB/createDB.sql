CREATE DATABASE TTJ;

USE TTJ;

CREATE TABLE [dbo].[Faculty] (
    [Fid]   INT IDENTITY (1, 1) NOT NULL,
    [FName] NCHAR (50) NOT NULL,
    PRIMARY KEY CLUSTERED ([Fid] ASC)
);

CREATE TABLE [dbo].[Direction]
(
	[DID] INT NOT NULL PRIMARY KEY, 
    [FID] INT NOT NULL FOREIGN KEY REFERENCES Faculty(FID), 
    [DName] NCHAR(50) NOT NULL
)

CREATE TABLE [dbo].[Building]
(
	[BID] INT NOT NULL PRIMARY KEY, 
    [Rooms] INT NOT NULL, 
    [Address] NCHAR(60) NOT NULL
)

CREATE TABLE [dbo].[Rooms]
(
	[RNum] INT NOT NULL PRIMARY KEY, 
    [BNum] INT NOT NULL FOREIGN KEY REFERENCES Building(BID), 
    [IsBromed] BIT NOT NULL, 
    [Price] MONEY NOT NULL
)

CREATE TABLE [dbo].[Students]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Name] NCHAR(50) NOT NULL, 
    [Gender] NCHAR(10) NOT NULL, 
    [Address] NCHAR(60) NOT NULL,
    [DID] INT NOT NULL FOREIGN KEY REFERENCES Direction(DID)
)

CREATE TABLE [dbo].[Payment]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [SID] INT NOT NULL FOREIGN KEY REFERENCES Students(ID), 
    [RNum] INT NOT NULL FOREIGN KEY REFERENCES Rooms(RNum), 
    [StartDate] DATE NOT NULL, 
    [EndDate] DATE NOT NULL, 
    [Months] INT NOT NULL, 
    [Payed] MONEY NOT NULL
)
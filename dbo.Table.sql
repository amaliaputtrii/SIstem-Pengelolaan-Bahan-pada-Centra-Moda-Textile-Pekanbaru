CREATE TABLE [dbo].[AttTable]
(
	[AttID] INT IDENTITY (1, 1) NOT NULL, 
    [AttName] VARCHAR(50) NOT NULL, 
    [Age] INT NOT NULL, 
    [Number] INT NOT NULL, 
    [Password] VARCHAR(50) NOT NULL

	PRIMARY KEY CLUSTERED ([AttID] ASC)
);

CREATE TABLE [dbo].[Jobs] (
    [JobId]    INT           IDENTITY (1, 1) NOT NULL,
    [JobTitle] VARCHAR (200) NULL,
    [JobCode]  VARCHAR (50)  NULL,
    CONSTRAINT [PK_Jobs] PRIMARY KEY CLUSTERED ([JobId] ASC)
);


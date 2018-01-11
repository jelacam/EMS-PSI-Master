CREATE TABLE [dbo].[Delta] (
    [Id]    INT NOT NULL,
    [Time]  DATETIME        NOT NULL,
    [Delta] VARBINARY (MAX) NOT NULL, 
    CONSTRAINT [PK_Delta] PRIMARY KEY ([Id])
);


CREATE TABLE [dbo].[DbVersion]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Major] INT NOT NULL, 
    [Minor] INT NOT NULL, 
    [Build] INT NOT NULL, 
    [ReleaseDate] DATETIME NOT NULL
)

CREATE TABLE [dbo].[CapstoneProjects]
(
    [CapstoneClientID] INT NOT NULL , 
    [InstructorID] INT NOT NULL, 
    PRIMARY KEY ([InstructorID], [CapstoneClientID]), 
    CONSTRAINT [FK_CapstoneProjects_CapstoneClients] FOREIGN KEY ([CapstoneClientID]) REFERENCES [CapstoneClients]([Id]), 
    CONSTRAINT [FK_CapstoneProjects_Instructors] FOREIGN KEY ([InstructorID]) REFERENCES [Instructors]([InstructorID])
)

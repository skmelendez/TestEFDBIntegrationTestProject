CREATE TABLE [dbo].[UserDetails]
(
	[Id] INT NOT NULL IDENTITY,
	[UserId] INT NOT NULL,
	[FirstName] NVARCHAR(50) NOT NULL,
	[LastName] NVARCHAR(50) NOT NULL,
	CONSTRAINT [PK_UserDetail_Id] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_User_UserDetail] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([Id])
)

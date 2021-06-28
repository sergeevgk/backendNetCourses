CREATE TABLE [dbo].[Factory](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](max) NULL,
    [Description] [nvarchar](max) NULL,
    CONSTRAINT [PK_Statuses] PRIMARY KEY CLUSTERED ([Id] ASC)
)

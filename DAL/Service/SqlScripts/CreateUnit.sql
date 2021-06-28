CREATE TABLE [dbo].[Unit](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](max) NULL,
    [FactoryId] [int] NULL,
    CONSTRAINT [PK_Unit] PRIMARY KEY CLUSTERED ([Id] ASC)
)

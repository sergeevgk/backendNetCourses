CREATE TABLE [dbo].[Tank](
    [Id] [int] IDENTITY(1,1) NOT NULL,
    [Name] [nvarchar](max) NULL,
    [Volume] [int] NOT NULL,
    [MaxVolume] [int] NOT NULL,
    [UnitId] [int] NULL,
    CONSTRAINT [PK_Tank] PRIMARY KEY CLUSTERED ([Id] ASC)
)

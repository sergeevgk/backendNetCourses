ALTER TABLE [dbo].[Unit] WITH CHECK ADD CONSTRAINT [FK_Unit_Factory_FactoryId] FOREIGN KEY([FactoryId])
    REFERENCES [dbo].[Factory] ([Id])

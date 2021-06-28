ALTER TABLE [dbo].[Tank] WITH CHECK ADD CONSTRAINT [FK_Tank_Unit_UnitId] FOREIGN KEY([UnitId])
    REFERENCES [dbo].[Unit] ([Id])

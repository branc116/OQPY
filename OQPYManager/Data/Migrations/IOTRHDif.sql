ALTER TABLE [Reviews] ADD [Helpfulness] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Reservations] ADD [VenueId] nvarchar(450);

GO

CREATE INDEX [IX_Reservations_VenueId] ON [Reservations] ([VenueId]);

GO

ALTER TABLE [Reservations] ADD CONSTRAINT [FK_Reservations_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170509094204_ReviewHelpfulnes', N'1.1.1');

GO


IF OBJECT_ID(N'__EFMigrationsHistory') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [ConcurrencyStamp] nvarchar(max),
    [Name] nvarchar(256),
    [NormalizedName] nvarchar(256),
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max),
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name])
);

GO

CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [ConcurrencyStamp] nvarchar(max),
    [Email] nvarchar(256),
    [EmailConfirmed] bit NOT NULL,
    [LockoutEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset,
    [NormalizedEmail] nvarchar(256),
    [NormalizedUserName] nvarchar(256),
    [PasswordHash] nvarchar(max),
    [PhoneNumber] nvarchar(max),
    [PhoneNumberConfirmed] bit NOT NULL,
    [SecurityStamp] nvarchar(max),
    [TwoFactorEnabled] bit NOT NULL,
    [UserName] nvarchar(256),
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [ClaimType] nvarchar(max),
    [ClaimValue] nvarchar(max),
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [ClaimType] nvarchar(max),
    [ClaimValue] nvarchar(max),
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max),
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]);

GO

CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);

GO

CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);

GO

CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);

GO

CREATE INDEX [IX_AspNetUserRoles_UserId] ON [AspNetUserRoles] ([UserId]);

GO

CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);

GO

CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'00000000000000_CreateIdentitySchema', N'1.1.1');

GO

DROP INDEX [IX_AspNetUserRoles_UserId] ON [AspNetUserRoles];

GO

DROP INDEX [RoleNameIndex] ON [AspNetRoles];

GO

ALTER TABLE [AspNetUsers] ADD [Discriminator] nvarchar(max) NOT NULL DEFAULT N'';

GO

ALTER TABLE [AspNetUsers] ADD [Name] nvarchar(max);

GO

ALTER TABLE [AspNetUsers] ADD [Surname] nvarchar(max);

GO

ALTER TABLE [AspNetUsers] ADD [VenueId] nvarchar(450);

GO

CREATE TABLE [Locations] (
    [Id] nvarchar(450) NOT NULL,
    [Adress] nvarchar(max),
    [Latitude] float NOT NULL,
    [Longditude] float NOT NULL,
    CONSTRAINT [PK_Locations] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [Venues] (
    [Id] nvarchar(450) NOT NULL,
    [Description] nvarchar(max),
    [LocationId] nvarchar(450),
    [Name] nvarchar(max),
    [OwnerId] nvarchar(450),
    [VenueCreationDate] datetime2 NOT NULL,
    CONSTRAINT [PK_Venues] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Venues_Locations_LocationId] FOREIGN KEY ([LocationId]) REFERENCES [Locations] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_Venues_AspNetUsers_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [PriceTags] (
    [Id] nvarchar(450) NOT NULL,
    [ItemName] nvarchar(max),
    [Price] decimal(18, 2) NOT NULL,
    [VenueId] nvarchar(450),
    CONSTRAINT [PK_PriceTags] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PriceTags_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Resources] (
    [Id] nvarchar(450) NOT NULL,
    [Category] nvarchar(max),
    [StuffName] nvarchar(max),
    [VenueId] nvarchar(450),
    CONSTRAINT [PK_Resources] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Resources_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Reviews] (
    [Id] nvarchar(450) NOT NULL,
    [Comment] nvarchar(max),
    [Rating] int NOT NULL,
    [VenueId] nvarchar(450),
    CONSTRAINT [PK_Reviews] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Reviews_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Tags] (
    [Id] nvarchar(450) NOT NULL,
    [TagName] nvarchar(max),
    [VenueId] nvarchar(450),
    CONSTRAINT [PK_Tags] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Tags_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [VenueWorkHours] (
    [Id] nvarchar(450) NOT NULL,
    [IsWorking] bit NOT NULL,
    [VenueId] nvarchar(450),
    CONSTRAINT [PK_VenueWorkHours] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_VenueWorkHours_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Reservations] (
    [Id] nvarchar(450) NOT NULL,
    [EndReservationTime] datetime2 NOT NULL,
    [ResourceId] nvarchar(450),
    [SecretCode] nvarchar(max),
    [StartReservationTime] datetime2 NOT NULL,
    CONSTRAINT [PK_Reservations] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Reservations_Resources_ResourceId] FOREIGN KEY ([ResourceId]) REFERENCES [Resources] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [VenueTag] (
    [VenueId] nvarchar(450) NOT NULL,
    [TagId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_VenueTag] PRIMARY KEY ([VenueId], [TagId]),
    CONSTRAINT [FK_VenueTag_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [Tags] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_VenueTag_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [WorkTime] (
    [Id] nvarchar(450) NOT NULL,
    [EndTime] datetime2 NOT NULL,
    [StartTime] datetime2 NOT NULL,
    [WorkHoursId] nvarchar(450),
    CONSTRAINT [PK_WorkTime] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WorkTime_VenueWorkHours_WorkHoursId] FOREIGN KEY ([WorkHoursId]) REFERENCES [VenueWorkHours] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_AspNetUsers_VenueId] ON [AspNetUsers] ([VenueId]);

GO

CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;

GO

CREATE INDEX [IX_PriceTags_VenueId] ON [PriceTags] ([VenueId]);

GO

CREATE INDEX [IX_Reservations_ResourceId] ON [Reservations] ([ResourceId]);

GO

CREATE INDEX [IX_Resources_VenueId] ON [Resources] ([VenueId]);

GO

CREATE INDEX [IX_Reviews_VenueId] ON [Reviews] ([VenueId]);

GO

CREATE INDEX [IX_Tags_VenueId] ON [Tags] ([VenueId]);

GO

CREATE INDEX [IX_Venues_LocationId] ON [Venues] ([LocationId]);

GO

CREATE INDEX [IX_Venues_OwnerId] ON [Venues] ([OwnerId]);

GO

CREATE INDEX [IX_VenueTag_TagId] ON [VenueTag] ([TagId]);

GO

CREATE UNIQUE INDEX [IX_VenueWorkHours_VenueId] ON [VenueWorkHours] ([VenueId]) WHERE [VenueId] IS NOT NULL;

GO

CREATE INDEX [IX_WorkTime_WorkHoursId] ON [WorkTime] ([WorkHoursId]);

GO

ALTER TABLE [AspNetUsers] ADD CONSTRAINT [FK_AspNetUsers_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170328221344_InitalCreate', N'1.1.1');

GO

ALTER TABLE [AspNetUsers] DROP CONSTRAINT [FK_AspNetUsers_Venues_VenueId];

GO

ALTER TABLE [Tags] DROP CONSTRAINT [FK_Tags_Venues_VenueId];

GO

ALTER TABLE [Venues] DROP CONSTRAINT [FK_Venues_AspNetUsers_OwnerId];

GO

DROP TABLE [VenueTag];

GO

DROP TABLE [WorkTime];

GO

DROP INDEX [IX_AspNetUsers_VenueId] ON [AspNetUsers];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'AspNetUsers') AND [c].[name] = N'Discriminator');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [AspNetUsers] DROP COLUMN [Discriminator];

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'AspNetUsers') AND [c].[name] = N'VenueId');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AspNetUsers] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [AspNetUsers] DROP COLUMN [VenueId];

GO

EXEC sp_rename N'Tags.VenueId', N'BaseVenueId', N'COLUMN';

GO

EXEC sp_rename N'Tags.IX_Tags_VenueId', N'IX_Tags_BaseVenueId', N'INDEX';

GO

ALTER TABLE [AspNetUserRoles] ADD [BaseEmployeeId] nvarchar(450);

GO

ALTER TABLE [AspNetUserRoles] ADD [BaseOwnerId] nvarchar(450);

GO

ALTER TABLE [AspNetUserLogins] ADD [BaseEmployeeId] nvarchar(450);

GO

ALTER TABLE [AspNetUserLogins] ADD [BaseOwnerId] nvarchar(450);

GO

ALTER TABLE [AspNetUserClaims] ADD [BaseEmployeeId] nvarchar(450);

GO

ALTER TABLE [AspNetUserClaims] ADD [BaseOwnerId] nvarchar(450);

GO

ALTER TABLE [Venues] ADD [ImageUrl] nvarchar(max);

GO

CREATE TABLE [Employees] (
    [Id] nvarchar(450) NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [ConcurrencyStamp] nvarchar(max),
    [Email] nvarchar(max),
    [EmailConfirmed] bit NOT NULL,
    [LockoutEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset,
    [Name] nvarchar(max),
    [NormalizedEmail] nvarchar(max),
    [NormalizedUserName] nvarchar(max),
    [PasswordHash] nvarchar(max),
    [PhoneNumber] nvarchar(max),
    [PhoneNumberConfirmed] bit NOT NULL,
    [SecurityStamp] nvarchar(max),
    [Surname] nvarchar(max),
    [TwoFactorEnabled] bit NOT NULL,
    [UserName] nvarchar(max),
    [VenueId] nvarchar(450),
    CONSTRAINT [PK_Employees] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Employees_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Owners] (
    [Id] nvarchar(450) NOT NULL,
    [AccessFailedCount] int NOT NULL,
    [ConcurrencyStamp] nvarchar(max),
    [Email] nvarchar(max),
    [EmailConfirmed] bit NOT NULL,
    [LockoutEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset,
    [Name] nvarchar(max),
    [NormalizedEmail] nvarchar(max),
    [NormalizedUserName] nvarchar(max),
    [PasswordHash] nvarchar(max),
    [PhoneNumber] nvarchar(max),
    [PhoneNumberConfirmed] bit NOT NULL,
    [SecurityStamp] nvarchar(max),
    [Surname] nvarchar(max),
    [TwoFactorEnabled] bit NOT NULL,
    [UserName] nvarchar(max),
    CONSTRAINT [PK_Owners] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [BaseVenueTag] (
    [VenueId] nvarchar(450) NOT NULL,
    [TagId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_BaseVenueTag] PRIMARY KEY ([VenueId], [TagId]),
    CONSTRAINT [FK_BaseVenueTag_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [Tags] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_BaseVenueTag_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [BaseWorkTime] (
    [Id] nvarchar(450) NOT NULL,
    [EndTime] datetime2 NOT NULL,
    [StartTime] datetime2 NOT NULL,
    [WorkHoursId] nvarchar(450),
    CONSTRAINT [PK_BaseWorkTime] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_BaseWorkTime_VenueWorkHours_WorkHoursId] FOREIGN KEY ([WorkHoursId]) REFERENCES [VenueWorkHours] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_AspNetUserRoles_BaseEmployeeId] ON [AspNetUserRoles] ([BaseEmployeeId]);

GO

CREATE INDEX [IX_AspNetUserRoles_BaseOwnerId] ON [AspNetUserRoles] ([BaseOwnerId]);

GO

CREATE INDEX [IX_AspNetUserLogins_BaseEmployeeId] ON [AspNetUserLogins] ([BaseEmployeeId]);

GO

CREATE INDEX [IX_AspNetUserLogins_BaseOwnerId] ON [AspNetUserLogins] ([BaseOwnerId]);

GO

CREATE INDEX [IX_AspNetUserClaims_BaseEmployeeId] ON [AspNetUserClaims] ([BaseEmployeeId]);

GO

CREATE INDEX [IX_AspNetUserClaims_BaseOwnerId] ON [AspNetUserClaims] ([BaseOwnerId]);

GO

CREATE INDEX [IX_Employees_VenueId] ON [Employees] ([VenueId]);

GO

CREATE INDEX [IX_BaseVenueTag_TagId] ON [BaseVenueTag] ([TagId]);

GO

CREATE INDEX [IX_BaseWorkTime_WorkHoursId] ON [BaseWorkTime] ([WorkHoursId]);

GO

ALTER TABLE [AspNetUserClaims] ADD CONSTRAINT [FK_AspNetUserClaims_Employees_BaseEmployeeId] FOREIGN KEY ([BaseEmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [AspNetUserClaims] ADD CONSTRAINT [FK_AspNetUserClaims_Owners_BaseOwnerId] FOREIGN KEY ([BaseOwnerId]) REFERENCES [Owners] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [AspNetUserLogins] ADD CONSTRAINT [FK_AspNetUserLogins_Employees_BaseEmployeeId] FOREIGN KEY ([BaseEmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [AspNetUserLogins] ADD CONSTRAINT [FK_AspNetUserLogins_Owners_BaseOwnerId] FOREIGN KEY ([BaseOwnerId]) REFERENCES [Owners] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_Employees_BaseEmployeeId] FOREIGN KEY ([BaseEmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_Owners_BaseOwnerId] FOREIGN KEY ([BaseOwnerId]) REFERENCES [Owners] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Tags] ADD CONSTRAINT [FK_Tags_Venues_BaseVenueId] FOREIGN KEY ([BaseVenueId]) REFERENCES [Venues] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Venues] ADD CONSTRAINT [FK_Venues_Owners_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [Owners] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170329101016_ToBaseWeGo', N'1.1.1');

GO

ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_Employees_BaseEmployeeId];

GO

ALTER TABLE [AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_Owners_BaseOwnerId];

GO

ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_Employees_BaseEmployeeId];

GO

ALTER TABLE [AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_Owners_BaseOwnerId];

GO

ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_Employees_BaseEmployeeId];

GO

ALTER TABLE [AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_Owners_BaseOwnerId];

GO

ALTER TABLE [Tags] DROP CONSTRAINT [FK_Tags_Venues_BaseVenueId];

GO

DROP TABLE [BaseVenueTag];

GO

DROP TABLE [BaseWorkTime];

GO

EXEC sp_rename N'AspNetUserRoles.BaseOwnerId', N'OwnerId', N'COLUMN';

GO

EXEC sp_rename N'AspNetUserRoles.BaseEmployeeId', N'EmployeeId', N'COLUMN';

GO

EXEC sp_rename N'AspNetUserRoles.IX_AspNetUserRoles_BaseOwnerId', N'IX_AspNetUserRoles_OwnerId', N'INDEX';

GO

EXEC sp_rename N'AspNetUserRoles.IX_AspNetUserRoles_BaseEmployeeId', N'IX_AspNetUserRoles_EmployeeId', N'INDEX';

GO

EXEC sp_rename N'AspNetUserLogins.BaseOwnerId', N'OwnerId', N'COLUMN';

GO

EXEC sp_rename N'AspNetUserLogins.BaseEmployeeId', N'EmployeeId', N'COLUMN';

GO

EXEC sp_rename N'AspNetUserLogins.IX_AspNetUserLogins_BaseOwnerId', N'IX_AspNetUserLogins_OwnerId', N'INDEX';

GO

EXEC sp_rename N'AspNetUserLogins.IX_AspNetUserLogins_BaseEmployeeId', N'IX_AspNetUserLogins_EmployeeId', N'INDEX';

GO

EXEC sp_rename N'AspNetUserClaims.BaseOwnerId', N'OwnerId', N'COLUMN';

GO

EXEC sp_rename N'AspNetUserClaims.BaseEmployeeId', N'EmployeeId', N'COLUMN';

GO

EXEC sp_rename N'AspNetUserClaims.IX_AspNetUserClaims_BaseOwnerId', N'IX_AspNetUserClaims_OwnerId', N'INDEX';

GO

EXEC sp_rename N'AspNetUserClaims.IX_AspNetUserClaims_BaseEmployeeId', N'IX_AspNetUserClaims_EmployeeId', N'INDEX';

GO

EXEC sp_rename N'Tags.BaseVenueId', N'VenueId', N'COLUMN';

GO

EXEC sp_rename N'Tags.IX_Tags_BaseVenueId', N'IX_Tags_VenueId', N'INDEX';

GO

CREATE TABLE [VenueTag] (
    [VenueId] nvarchar(450) NOT NULL,
    [TagId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_VenueTag] PRIMARY KEY ([VenueId], [TagId]),
    CONSTRAINT [FK_VenueTag_Tags_TagId] FOREIGN KEY ([TagId]) REFERENCES [Tags] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_VenueTag_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [WorkTime] (
    [Id] nvarchar(450) NOT NULL,
    [EndTime] datetime2 NOT NULL,
    [StartTime] datetime2 NOT NULL,
    [WorkHoursId] nvarchar(450),
    CONSTRAINT [PK_WorkTime] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WorkTime_VenueWorkHours_WorkHoursId] FOREIGN KEY ([WorkHoursId]) REFERENCES [VenueWorkHours] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_VenueTag_TagId] ON [VenueTag] ([TagId]);

GO

CREATE INDEX [IX_WorkTime_WorkHoursId] ON [WorkTime] ([WorkHoursId]);

GO

ALTER TABLE [AspNetUserClaims] ADD CONSTRAINT [FK_AspNetUserClaims_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [AspNetUserClaims] ADD CONSTRAINT [FK_AspNetUserClaims_Owners_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [Owners] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [AspNetUserLogins] ADD CONSTRAINT [FK_AspNetUserLogins_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [AspNetUserLogins] ADD CONSTRAINT [FK_AspNetUserLogins_Owners_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [Owners] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_Employees_EmployeeId] FOREIGN KEY ([EmployeeId]) REFERENCES [Employees] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [AspNetUserRoles] ADD CONSTRAINT [FK_AspNetUserRoles_Owners_OwnerId] FOREIGN KEY ([OwnerId]) REFERENCES [Owners] ([Id]) ON DELETE NO ACTION;

GO

ALTER TABLE [Tags] ADD CONSTRAINT [FK_Tags_Venues_VenueId] FOREIGN KEY ([VenueId]) REFERENCES [Venues] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170402152207_BaseOut', N'1.1.1');

GO

ALTER TABLE [Resources] ADD [IOTEnabled] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Resources] ADD [OQPYed] bit NOT NULL DEFAULT 0;

GO

ALTER TABLE [Resources] ADD [SecreteCode] nvarchar(max);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170505182152_IOT', N'1.1.1');

GO


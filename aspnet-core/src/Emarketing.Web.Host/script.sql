
IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [AbpEditions] (
    [Id] int NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [DisplayName] nvarchar(64) NOT NULL,
    [IsDeleted] bit NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [Name] nvarchar(32) NOT NULL,
    CONSTRAINT [PK_AbpEditions] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpAuditLogs] (
    [Id] bigint NOT NULL IDENTITY,
    [BrowserInfo] nvarchar(256) NULL,
    [ClientIpAddress] nvarchar(64) NULL,
    [ClientName] nvarchar(128) NULL,
    [CustomData] nvarchar(2000) NULL,
    [Exception] nvarchar(2000) NULL,
    [ExecutionDuration] int NOT NULL,
    [ExecutionTime] datetime2 NOT NULL,
    [ImpersonatorTenantId] int NULL,
    [ImpersonatorUserId] bigint NULL,
    [MethodName] nvarchar(256) NULL,
    [Parameters] nvarchar(1024) NULL,
    [ServiceName] nvarchar(256) NULL,
    [TenantId] int NULL,
    [UserId] bigint NULL,
    CONSTRAINT [PK_AbpAuditLogs] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpUserAccounts] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [EmailAddress] nvarchar(450) NULL,
    [IsDeleted] bit NOT NULL,
    [LastLoginTime] datetime2 NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    [UserLinkId] bigint NULL,
    [UserName] nvarchar(450) NULL,
    CONSTRAINT [PK_AbpUserAccounts] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpUserLoginAttempts] (
    [Id] bigint NOT NULL IDENTITY,
    [BrowserInfo] nvarchar(256) NULL,
    [ClientIpAddress] nvarchar(64) NULL,
    [ClientName] nvarchar(128) NULL,
    [CreationTime] datetime2 NOT NULL,
    [Result] tinyint NOT NULL,
    [TenancyName] nvarchar(64) NULL,
    [TenantId] int NULL,
    [UserId] bigint NULL,
    [UserNameOrEmailAddress] nvarchar(255) NULL,
    CONSTRAINT [PK_AbpUserLoginAttempts] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpUserOrganizationUnits] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [OrganizationUnitId] bigint NOT NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_AbpUserOrganizationUnits] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpBackgroundJobs] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [IsAbandoned] bit NOT NULL,
    [JobArgs] nvarchar(max) NOT NULL,
    [JobType] nvarchar(512) NOT NULL,
    [LastTryTime] datetime2 NULL,
    [NextTryTime] datetime2 NOT NULL,
    [Priority] tinyint NOT NULL,
    [TryCount] smallint NOT NULL,
    CONSTRAINT [PK_AbpBackgroundJobs] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpLanguages] (
    [Id] int NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [DisplayName] nvarchar(64) NOT NULL,
    [Icon] nvarchar(128) NULL,
    [IsDeleted] bit NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [Name] nvarchar(10) NOT NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpLanguages] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpLanguageTexts] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [Key] nvarchar(256) NOT NULL,
    [LanguageName] nvarchar(10) NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [Source] nvarchar(128) NOT NULL,
    [TenantId] int NULL,
    [Value] nvarchar(max) NOT NULL,
    CONSTRAINT [PK_AbpLanguageTexts] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpNotifications] (
    [Id] uniqueidentifier NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [Data] nvarchar(max) NULL,
    [DataTypeName] nvarchar(512) NULL,
    [EntityId] nvarchar(96) NULL,
    [EntityTypeAssemblyQualifiedName] nvarchar(512) NULL,
    [EntityTypeName] nvarchar(250) NULL,
    [ExcludedUserIds] nvarchar(max) NULL,
    [NotificationName] nvarchar(96) NOT NULL,
    [Severity] tinyint NOT NULL,
    [TenantIds] nvarchar(max) NULL,
    [UserIds] nvarchar(max) NULL,
    CONSTRAINT [PK_AbpNotifications] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpNotificationSubscriptions] (
    [Id] uniqueidentifier NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [EntityId] nvarchar(96) NULL,
    [EntityTypeAssemblyQualifiedName] nvarchar(512) NULL,
    [EntityTypeName] nvarchar(250) NULL,
    [NotificationName] nvarchar(96) NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_AbpNotificationSubscriptions] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpTenantNotifications] (
    [Id] uniqueidentifier NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [Data] nvarchar(max) NULL,
    [DataTypeName] nvarchar(512) NULL,
    [EntityId] nvarchar(96) NULL,
    [EntityTypeAssemblyQualifiedName] nvarchar(512) NULL,
    [EntityTypeName] nvarchar(250) NULL,
    [NotificationName] nvarchar(96) NOT NULL,
    [Severity] tinyint NOT NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpTenantNotifications] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpUserNotifications] (
    [Id] uniqueidentifier NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [State] int NOT NULL,
    [TenantId] int NULL,
    [TenantNotificationId] uniqueidentifier NOT NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_AbpUserNotifications] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpOrganizationUnits] (
    [Id] bigint NOT NULL IDENTITY,
    [Code] nvarchar(95) NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [DisplayName] nvarchar(128) NOT NULL,
    [IsDeleted] bit NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [ParentId] bigint NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpOrganizationUnits] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpOrganizationUnits_AbpOrganizationUnits_ParentId] FOREIGN KEY ([ParentId]) REFERENCES [AbpOrganizationUnits] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AbpUsers] (
    [Id] bigint NOT NULL IDENTITY,
    [AccessFailedCount] int NOT NULL,
    [AuthenticationSource] nvarchar(64) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [EmailAddress] nvarchar(256) NOT NULL,
    [EmailConfirmationCode] nvarchar(328) NULL,
    [IsActive] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsEmailConfirmed] bit NOT NULL,
    [IsLockoutEnabled] bit NOT NULL,
    [IsPhoneNumberConfirmed] bit NOT NULL,
    [IsTwoFactorEnabled] bit NOT NULL,
    [LastLoginTime] datetime2 NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [LockoutEndDateUtc] datetime2 NULL,
    [Name] nvarchar(32) NOT NULL,
    [NormalizedEmailAddress] nvarchar(256) NOT NULL,
    [NormalizedUserName] nvarchar(32) NOT NULL,
    [Password] nvarchar(128) NOT NULL,
    [PasswordResetCode] nvarchar(328) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [Surname] nvarchar(32) NOT NULL,
    [TenantId] int NULL,
    [UserName] nvarchar(32) NOT NULL,
    CONSTRAINT [PK_AbpUsers] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpUsers_AbpUsers_CreatorUserId] FOREIGN KEY ([CreatorUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpUsers_AbpUsers_DeleterUserId] FOREIGN KEY ([DeleterUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpUsers_AbpUsers_LastModifierUserId] FOREIGN KEY ([LastModifierUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AbpFeatures] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [Value] nvarchar(2000) NOT NULL,
    [EditionId] int NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpFeatures] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpFeatures_AbpEditions_EditionId] FOREIGN KEY ([EditionId]) REFERENCES [AbpEditions] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpUserClaims] (
    [Id] bigint NOT NULL IDENTITY,
    [ClaimType] nvarchar(450) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_AbpUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpUserClaims_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpUserLogins] (
    [Id] bigint NOT NULL IDENTITY,
    [LoginProvider] nvarchar(128) NOT NULL,
    [ProviderKey] nvarchar(256) NOT NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_AbpUserLogins] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpUserLogins_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpUserRoles] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [RoleId] int NOT NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    CONSTRAINT [PK_AbpUserRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpUserRoles_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpUserTokens] (
    [Id] bigint NOT NULL IDENTITY,
    [LoginProvider] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [TenantId] int NULL,
    [UserId] bigint NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AbpUserTokens] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpUserTokens_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpSettings] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [Name] nvarchar(256) NOT NULL,
    [TenantId] int NULL,
    [UserId] bigint NULL,
    [Value] nvarchar(2000) NULL,
    CONSTRAINT [PK_AbpSettings] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpSettings_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AbpRoles] (
    [Id] int NOT NULL IDENTITY,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [DisplayName] nvarchar(64) NOT NULL,
    [IsDefault] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [IsStatic] bit NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [Name] nvarchar(32) NOT NULL,
    [NormalizedName] nvarchar(32) NOT NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpRoles] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpRoles_AbpUsers_CreatorUserId] FOREIGN KEY ([CreatorUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpRoles_AbpUsers_DeleterUserId] FOREIGN KEY ([DeleterUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpRoles_AbpUsers_LastModifierUserId] FOREIGN KEY ([LastModifierUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AbpTenants] (
    [Id] int NOT NULL IDENTITY,
    [ConnectionString] nvarchar(1024) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [EditionId] int NULL,
    [IsActive] bit NOT NULL,
    [IsDeleted] bit NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [Name] nvarchar(128) NOT NULL,
    [TenancyName] nvarchar(64) NOT NULL,
    CONSTRAINT [PK_AbpTenants] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpTenants_AbpUsers_CreatorUserId] FOREIGN KEY ([CreatorUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpTenants_AbpUsers_DeleterUserId] FOREIGN KEY ([DeleterUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpTenants_AbpEditions_EditionId] FOREIGN KEY ([EditionId]) REFERENCES [AbpEditions] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_AbpTenants_AbpUsers_LastModifierUserId] FOREIGN KEY ([LastModifierUserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION
);

GO

CREATE TABLE [AbpPermissions] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [Discriminator] nvarchar(max) NOT NULL,
    [IsGranted] bit NOT NULL,
    [Name] nvarchar(128) NOT NULL,
    [TenantId] int NULL,
    [RoleId] int NULL,
    [UserId] bigint NULL,
    CONSTRAINT [PK_AbpPermissions] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpPermissions_AbpRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AbpRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AbpPermissions_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpRoleClaims] (
    [Id] bigint NOT NULL IDENTITY,
    [ClaimType] nvarchar(450) NULL,
    [ClaimValue] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [RoleId] int NOT NULL,
    [TenantId] int NULL,
    [UserId] int NULL,
    CONSTRAINT [PK_AbpRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpRoleClaims_AbpRoles_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpRoles] ([Id]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_AbpFeatures_EditionId_Name] ON [AbpFeatures] ([EditionId], [Name]);

GO

CREATE INDEX [IX_AbpFeatures_TenantId_Name] ON [AbpFeatures] ([TenantId], [Name]);

GO

CREATE INDEX [IX_AbpAuditLogs_TenantId_ExecutionDuration] ON [AbpAuditLogs] ([TenantId], [ExecutionDuration]);

GO

CREATE INDEX [IX_AbpAuditLogs_TenantId_ExecutionTime] ON [AbpAuditLogs] ([TenantId], [ExecutionTime]);

GO

CREATE INDEX [IX_AbpAuditLogs_TenantId_UserId] ON [AbpAuditLogs] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpPermissions_TenantId_Name] ON [AbpPermissions] ([TenantId], [Name]);

GO

CREATE INDEX [IX_AbpPermissions_RoleId] ON [AbpPermissions] ([RoleId]);

GO

CREATE INDEX [IX_AbpPermissions_UserId] ON [AbpPermissions] ([UserId]);

GO

CREATE INDEX [IX_AbpRoleClaims_RoleId] ON [AbpRoleClaims] ([RoleId]);

GO

CREATE INDEX [IX_AbpRoleClaims_UserId] ON [AbpRoleClaims] ([UserId]);

GO

CREATE INDEX [IX_AbpRoleClaims_TenantId_ClaimType] ON [AbpRoleClaims] ([TenantId], [ClaimType]);

GO

CREATE INDEX [IX_AbpUserAccounts_EmailAddress] ON [AbpUserAccounts] ([EmailAddress]);

GO

CREATE INDEX [IX_AbpUserAccounts_UserName] ON [AbpUserAccounts] ([UserName]);

GO

CREATE INDEX [IX_AbpUserAccounts_TenantId_EmailAddress] ON [AbpUserAccounts] ([TenantId], [EmailAddress]);

GO

CREATE INDEX [IX_AbpUserAccounts_TenantId_UserId] ON [AbpUserAccounts] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpUserAccounts_TenantId_UserName] ON [AbpUserAccounts] ([TenantId], [UserName]);

GO

CREATE INDEX [IX_AbpUserClaims_UserId] ON [AbpUserClaims] ([UserId]);

GO

CREATE INDEX [IX_AbpUserClaims_TenantId_ClaimType] ON [AbpUserClaims] ([TenantId], [ClaimType]);

GO

CREATE INDEX [IX_AbpUserLogins_UserId] ON [AbpUserLogins] ([UserId]);

GO

CREATE INDEX [IX_AbpUserLogins_TenantId_UserId] ON [AbpUserLogins] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpUserLogins_TenantId_LoginProvider_ProviderKey] ON [AbpUserLogins] ([TenantId], [LoginProvider], [ProviderKey]);

GO

CREATE INDEX [IX_AbpUserLoginAttempts_UserId_TenantId] ON [AbpUserLoginAttempts] ([UserId], [TenantId]);

GO

CREATE INDEX [IX_AbpUserLoginAttempts_TenancyName_UserNameOrEmailAddress_Result] ON [AbpUserLoginAttempts] ([TenancyName], [UserNameOrEmailAddress], [Result]);

GO

CREATE INDEX [IX_AbpUserOrganizationUnits_TenantId_OrganizationUnitId] ON [AbpUserOrganizationUnits] ([TenantId], [OrganizationUnitId]);

GO

CREATE INDEX [IX_AbpUserOrganizationUnits_TenantId_UserId] ON [AbpUserOrganizationUnits] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpUserRoles_UserId] ON [AbpUserRoles] ([UserId]);

GO

CREATE INDEX [IX_AbpUserRoles_TenantId_RoleId] ON [AbpUserRoles] ([TenantId], [RoleId]);

GO

CREATE INDEX [IX_AbpUserRoles_TenantId_UserId] ON [AbpUserRoles] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpUserTokens_UserId] ON [AbpUserTokens] ([UserId]);

GO

CREATE INDEX [IX_AbpUserTokens_TenantId_UserId] ON [AbpUserTokens] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpBackgroundJobs_IsAbandoned_NextTryTime] ON [AbpBackgroundJobs] ([IsAbandoned], [NextTryTime]);

GO

CREATE INDEX [IX_AbpSettings_UserId] ON [AbpSettings] ([UserId]);

GO

CREATE INDEX [IX_AbpSettings_TenantId_Name] ON [AbpSettings] ([TenantId], [Name]);

GO

CREATE INDEX [IX_AbpLanguages_TenantId_Name] ON [AbpLanguages] ([TenantId], [Name]);

GO

CREATE INDEX [IX_AbpLanguageTexts_TenantId_Source_LanguageName_Key] ON [AbpLanguageTexts] ([TenantId], [Source], [LanguageName], [Key]);

GO

CREATE INDEX [IX_AbpNotificationSubscriptions_NotificationName_EntityTypeName_EntityId_UserId] ON [AbpNotificationSubscriptions] ([NotificationName], [EntityTypeName], [EntityId], [UserId]);

GO

CREATE INDEX [IX_AbpNotificationSubscriptions_TenantId_NotificationName_EntityTypeName_EntityId_UserId] ON [AbpNotificationSubscriptions] ([TenantId], [NotificationName], [EntityTypeName], [EntityId], [UserId]);

GO

CREATE INDEX [IX_AbpTenantNotifications_TenantId] ON [AbpTenantNotifications] ([TenantId]);

GO

CREATE INDEX [IX_AbpUserNotifications_UserId_State_CreationTime] ON [AbpUserNotifications] ([UserId], [State], [CreationTime]);

GO

CREATE INDEX [IX_AbpOrganizationUnits_ParentId] ON [AbpOrganizationUnits] ([ParentId]);

GO

CREATE INDEX [IX_AbpOrganizationUnits_TenantId_Code] ON [AbpOrganizationUnits] ([TenantId], [Code]);

GO

CREATE INDEX [IX_AbpRoles_CreatorUserId] ON [AbpRoles] ([CreatorUserId]);

GO

CREATE INDEX [IX_AbpRoles_DeleterUserId] ON [AbpRoles] ([DeleterUserId]);

GO

CREATE INDEX [IX_AbpRoles_LastModifierUserId] ON [AbpRoles] ([LastModifierUserId]);

GO

CREATE INDEX [IX_AbpRoles_TenantId_NormalizedName] ON [AbpRoles] ([TenantId], [NormalizedName]);

GO

CREATE INDEX [IX_AbpUsers_CreatorUserId] ON [AbpUsers] ([CreatorUserId]);

GO

CREATE INDEX [IX_AbpUsers_DeleterUserId] ON [AbpUsers] ([DeleterUserId]);

GO

CREATE INDEX [IX_AbpUsers_LastModifierUserId] ON [AbpUsers] ([LastModifierUserId]);

GO

CREATE INDEX [IX_AbpUsers_TenantId_NormalizedEmailAddress] ON [AbpUsers] ([TenantId], [NormalizedEmailAddress]);

GO

CREATE INDEX [IX_AbpUsers_TenantId_NormalizedUserName] ON [AbpUsers] ([TenantId], [NormalizedUserName]);

GO

CREATE INDEX [IX_AbpTenants_CreatorUserId] ON [AbpTenants] ([CreatorUserId]);

GO

CREATE INDEX [IX_AbpTenants_DeleterUserId] ON [AbpTenants] ([DeleterUserId]);

GO

CREATE INDEX [IX_AbpTenants_EditionId] ON [AbpTenants] ([EditionId]);

GO

CREATE INDEX [IX_AbpTenants_LastModifierUserId] ON [AbpTenants] ([LastModifierUserId]);

GO

CREATE INDEX [IX_AbpTenants_TenancyName] ON [AbpTenants] ([TenancyName]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170424115119_Initial_Migrations', N'3.1.4');

GO

ALTER TABLE [AbpRoleClaims] DROP CONSTRAINT [FK_AbpRoleClaims_AbpRoles_UserId];

GO

DROP INDEX [IX_AbpRoleClaims_UserId] ON [AbpRoleClaims];

GO

DECLARE @var0 sysname;
SELECT @var0 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpRoleClaims]') AND [c].[name] = N'UserId');
IF @var0 IS NOT NULL EXEC(N'ALTER TABLE [AbpRoleClaims] DROP CONSTRAINT [' + @var0 + '];');
ALTER TABLE [AbpRoleClaims] DROP COLUMN [UserId];

GO

ALTER TABLE [AbpLanguages] ADD [IsDisabled] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [AbpRoleClaims] ADD CONSTRAINT [FK_AbpRoleClaims_AbpRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AbpRoles] ([Id]) ON DELETE CASCADE;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170608053244_Upgraded_To_Abp_2_1_0', N'3.1.4');

GO

ALTER TABLE [AbpRoles] ADD [Description] nvarchar(max) NULL;

GO

ALTER TABLE [AbpRoles] ADD [IsActive] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170621153937_Added_Description_And_IsActive_To_Role', N'3.1.4');

GO

DECLARE @var1 sysname;
SELECT @var1 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpRoles]') AND [c].[name] = N'IsActive');
IF @var1 IS NOT NULL EXEC(N'ALTER TABLE [AbpRoles] DROP CONSTRAINT [' + @var1 + '];');
ALTER TABLE [AbpRoles] DROP COLUMN [IsActive];

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170703134115_Remove_IsActive_From_Role', N'3.1.4');

GO

ALTER TABLE [AbpUserOrganizationUnits] ADD [IsDeleted] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20170804083601_Upgraded_To_Abp_v2.2.2', N'3.1.4');

GO

DROP INDEX [IX_AbpUserClaims_TenantId_ClaimType] ON [AbpUserClaims];
DECLARE @var2 sysname;
SELECT @var2 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUserClaims]') AND [c].[name] = N'ClaimType');
IF @var2 IS NOT NULL EXEC(N'ALTER TABLE [AbpUserClaims] DROP CONSTRAINT [' + @var2 + '];');
ALTER TABLE [AbpUserClaims] ALTER COLUMN [ClaimType] nvarchar(256) NULL;
CREATE INDEX [IX_AbpUserClaims_TenantId_ClaimType] ON [AbpUserClaims] ([TenantId], [ClaimType]);

GO

DROP INDEX [IX_AbpUserAccounts_UserName] ON [AbpUserAccounts];
DROP INDEX [IX_AbpUserAccounts_TenantId_UserName] ON [AbpUserAccounts];
DECLARE @var3 sysname;
SELECT @var3 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUserAccounts]') AND [c].[name] = N'UserName');
IF @var3 IS NOT NULL EXEC(N'ALTER TABLE [AbpUserAccounts] DROP CONSTRAINT [' + @var3 + '];');
ALTER TABLE [AbpUserAccounts] ALTER COLUMN [UserName] nvarchar(32) NULL;
CREATE INDEX [IX_AbpUserAccounts_UserName] ON [AbpUserAccounts] ([UserName]);
CREATE INDEX [IX_AbpUserAccounts_TenantId_UserName] ON [AbpUserAccounts] ([TenantId], [UserName]);

GO

DROP INDEX [IX_AbpUserAccounts_EmailAddress] ON [AbpUserAccounts];
DROP INDEX [IX_AbpUserAccounts_TenantId_EmailAddress] ON [AbpUserAccounts];
DECLARE @var4 sysname;
SELECT @var4 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUserAccounts]') AND [c].[name] = N'EmailAddress');
IF @var4 IS NOT NULL EXEC(N'ALTER TABLE [AbpUserAccounts] DROP CONSTRAINT [' + @var4 + '];');
ALTER TABLE [AbpUserAccounts] ALTER COLUMN [EmailAddress] nvarchar(256) NULL;
CREATE INDEX [IX_AbpUserAccounts_EmailAddress] ON [AbpUserAccounts] ([EmailAddress]);
CREATE INDEX [IX_AbpUserAccounts_TenantId_EmailAddress] ON [AbpUserAccounts] ([TenantId], [EmailAddress]);

GO

DROP INDEX [IX_AbpRoleClaims_TenantId_ClaimType] ON [AbpRoleClaims];
DECLARE @var5 sysname;
SELECT @var5 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpRoleClaims]') AND [c].[name] = N'ClaimType');
IF @var5 IS NOT NULL EXEC(N'ALTER TABLE [AbpRoleClaims] DROP CONSTRAINT [' + @var5 + '];');
ALTER TABLE [AbpRoleClaims] ALTER COLUMN [ClaimType] nvarchar(256) NULL;
CREATE INDEX [IX_AbpRoleClaims_TenantId_ClaimType] ON [AbpRoleClaims] ([TenantId], [ClaimType]);

GO

CREATE TABLE [AbpEntityChangeSets] (
    [Id] bigint NOT NULL IDENTITY,
    [BrowserInfo] nvarchar(256) NULL,
    [ClientIpAddress] nvarchar(64) NULL,
    [ClientName] nvarchar(128) NULL,
    [CreationTime] datetime2 NOT NULL,
    [ExtensionData] nvarchar(max) NULL,
    [ImpersonatorTenantId] int NULL,
    [ImpersonatorUserId] bigint NULL,
    [Reason] nvarchar(256) NULL,
    [TenantId] int NULL,
    [UserId] bigint NULL,
    CONSTRAINT [PK_AbpEntityChangeSets] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpEntityChanges] (
    [Id] bigint NOT NULL IDENTITY,
    [ChangeTime] datetime2 NOT NULL,
    [ChangeType] tinyint NOT NULL,
    [EntityChangeSetId] bigint NOT NULL,
    [EntityId] nvarchar(48) NULL,
    [EntityTypeFullName] nvarchar(192) NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpEntityChanges] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpEntityChanges_AbpEntityChangeSets_EntityChangeSetId] FOREIGN KEY ([EntityChangeSetId]) REFERENCES [AbpEntityChangeSets] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpEntityPropertyChanges] (
    [Id] bigint NOT NULL IDENTITY,
    [EntityChangeId] bigint NOT NULL,
    [NewValue] nvarchar(512) NULL,
    [OriginalValue] nvarchar(512) NULL,
    [PropertyName] nvarchar(96) NULL,
    [PropertyTypeFullName] nvarchar(192) NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpEntityPropertyChanges] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpEntityPropertyChanges_AbpEntityChanges_EntityChangeId] FOREIGN KEY ([EntityChangeId]) REFERENCES [AbpEntityChanges] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_AbpEntityChanges_EntityChangeSetId] ON [AbpEntityChanges] ([EntityChangeSetId]);

GO

CREATE INDEX [IX_AbpEntityChanges_EntityTypeFullName_EntityId] ON [AbpEntityChanges] ([EntityTypeFullName], [EntityId]);

GO

CREATE INDEX [IX_AbpEntityChangeSets_TenantId_CreationTime] ON [AbpEntityChangeSets] ([TenantId], [CreationTime]);

GO

CREATE INDEX [IX_AbpEntityChangeSets_TenantId_Reason] ON [AbpEntityChangeSets] ([TenantId], [Reason]);

GO

CREATE INDEX [IX_AbpEntityChangeSets_TenantId_UserId] ON [AbpEntityChangeSets] ([TenantId], [UserId]);

GO

CREATE INDEX [IX_AbpEntityPropertyChanges_EntityChangeId] ON [AbpEntityPropertyChanges] ([EntityChangeId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180201051646_Upgraded_To_Abp_v3.4.0', N'3.1.4');

GO

DECLARE @var6 sysname;
SELECT @var6 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUserTokens]') AND [c].[name] = N'Value');
IF @var6 IS NOT NULL EXEC(N'ALTER TABLE [AbpUserTokens] DROP CONSTRAINT [' + @var6 + '];');
ALTER TABLE [AbpUserTokens] ALTER COLUMN [Value] nvarchar(512) NULL;

GO

DECLARE @var7 sysname;
SELECT @var7 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUserTokens]') AND [c].[name] = N'Name');
IF @var7 IS NOT NULL EXEC(N'ALTER TABLE [AbpUserTokens] DROP CONSTRAINT [' + @var7 + '];');
ALTER TABLE [AbpUserTokens] ALTER COLUMN [Name] nvarchar(128) NULL;

GO

DECLARE @var8 sysname;
SELECT @var8 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUserTokens]') AND [c].[name] = N'LoginProvider');
IF @var8 IS NOT NULL EXEC(N'ALTER TABLE [AbpUserTokens] DROP CONSTRAINT [' + @var8 + '];');
ALTER TABLE [AbpUserTokens] ALTER COLUMN [LoginProvider] nvarchar(64) NULL;

GO

DECLARE @var9 sysname;
SELECT @var9 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUsers]') AND [c].[name] = N'SecurityStamp');
IF @var9 IS NOT NULL EXEC(N'ALTER TABLE [AbpUsers] DROP CONSTRAINT [' + @var9 + '];');
ALTER TABLE [AbpUsers] ALTER COLUMN [SecurityStamp] nvarchar(128) NULL;

GO

DECLARE @var10 sysname;
SELECT @var10 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUsers]') AND [c].[name] = N'PhoneNumber');
IF @var10 IS NOT NULL EXEC(N'ALTER TABLE [AbpUsers] DROP CONSTRAINT [' + @var10 + '];');
ALTER TABLE [AbpUsers] ALTER COLUMN [PhoneNumber] nvarchar(32) NULL;

GO

DECLARE @var11 sysname;
SELECT @var11 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUsers]') AND [c].[name] = N'ConcurrencyStamp');
IF @var11 IS NOT NULL EXEC(N'ALTER TABLE [AbpUsers] DROP CONSTRAINT [' + @var11 + '];');
ALTER TABLE [AbpUsers] ALTER COLUMN [ConcurrencyStamp] nvarchar(128) NULL;

GO

DECLARE @var12 sysname;
SELECT @var12 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpRoles]') AND [c].[name] = N'ConcurrencyStamp');
IF @var12 IS NOT NULL EXEC(N'ALTER TABLE [AbpRoles] DROP CONSTRAINT [' + @var12 + '];');
ALTER TABLE [AbpRoles] ALTER COLUMN [ConcurrencyStamp] nvarchar(128) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180320131229_Upgraded_To_Abp_v3_5_0', N'3.1.4');

GO

DECLARE @var13 sysname;
SELECT @var13 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUserLoginAttempts]') AND [c].[name] = N'BrowserInfo');
IF @var13 IS NOT NULL EXEC(N'ALTER TABLE [AbpUserLoginAttempts] DROP CONSTRAINT [' + @var13 + '];');
ALTER TABLE [AbpUserLoginAttempts] ALTER COLUMN [BrowserInfo] nvarchar(512) NULL;

GO

DECLARE @var14 sysname;
SELECT @var14 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpEntityChangeSets]') AND [c].[name] = N'BrowserInfo');
IF @var14 IS NOT NULL EXEC(N'ALTER TABLE [AbpEntityChangeSets] DROP CONSTRAINT [' + @var14 + '];');
ALTER TABLE [AbpEntityChangeSets] ALTER COLUMN [BrowserInfo] nvarchar(512) NULL;

GO

DECLARE @var15 sysname;
SELECT @var15 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpAuditLogs]') AND [c].[name] = N'BrowserInfo');
IF @var15 IS NOT NULL EXEC(N'ALTER TABLE [AbpAuditLogs] DROP CONSTRAINT [' + @var15 + '];');
ALTER TABLE [AbpAuditLogs] ALTER COLUMN [BrowserInfo] nvarchar(512) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180509121141_Upgraded_To_Abp_v3_6_1', N'3.1.4');

GO

DECLARE @var16 sysname;
SELECT @var16 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUsers]') AND [c].[name] = N'UserName');
IF @var16 IS NOT NULL EXEC(N'ALTER TABLE [AbpUsers] DROP CONSTRAINT [' + @var16 + '];');
ALTER TABLE [AbpUsers] ALTER COLUMN [UserName] nvarchar(256) NOT NULL;

GO

DROP INDEX [IX_AbpUsers_TenantId_NormalizedUserName] ON [AbpUsers];
DECLARE @var17 sysname;
SELECT @var17 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUsers]') AND [c].[name] = N'NormalizedUserName');
IF @var17 IS NOT NULL EXEC(N'ALTER TABLE [AbpUsers] DROP CONSTRAINT [' + @var17 + '];');
ALTER TABLE [AbpUsers] ALTER COLUMN [NormalizedUserName] nvarchar(256) NOT NULL;
CREATE INDEX [IX_AbpUsers_TenantId_NormalizedUserName] ON [AbpUsers] ([TenantId], [NormalizedUserName]);

GO

DROP INDEX [IX_AbpUserAccounts_UserName] ON [AbpUserAccounts];
DROP INDEX [IX_AbpUserAccounts_TenantId_UserName] ON [AbpUserAccounts];
DECLARE @var18 sysname;
SELECT @var18 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUserAccounts]') AND [c].[name] = N'UserName');
IF @var18 IS NOT NULL EXEC(N'ALTER TABLE [AbpUserAccounts] DROP CONSTRAINT [' + @var18 + '];');
ALTER TABLE [AbpUserAccounts] ALTER COLUMN [UserName] nvarchar(256) NULL;
CREATE INDEX [IX_AbpUserAccounts_UserName] ON [AbpUserAccounts] ([UserName]);
CREATE INDEX [IX_AbpUserAccounts_TenantId_UserName] ON [AbpUserAccounts] ([TenantId], [UserName]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180726102703_Upgrade_ABP_3.8.0', N'3.1.4');

GO

DECLARE @var19 sysname;
SELECT @var19 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUserTokens]') AND [c].[name] = N'LoginProvider');
IF @var19 IS NOT NULL EXEC(N'ALTER TABLE [AbpUserTokens] DROP CONSTRAINT [' + @var19 + '];');
ALTER TABLE [AbpUserTokens] ALTER COLUMN [LoginProvider] nvarchar(128) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180731132139_Upgrade_ABP_3.8.1', N'3.1.4');

GO

ALTER TABLE [AbpUserTokens] ADD [ExpireDate] datetime2 NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20180927062608_Upgrade_ABP_3.8.3', N'3.1.4');

GO

DECLARE @var20 sysname;
SELECT @var20 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUsers]') AND [c].[name] = N'Surname');
IF @var20 IS NOT NULL EXEC(N'ALTER TABLE [AbpUsers] DROP CONSTRAINT [' + @var20 + '];');
ALTER TABLE [AbpUsers] ALTER COLUMN [Surname] nvarchar(64) NOT NULL;

GO

DECLARE @var21 sysname;
SELECT @var21 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUsers]') AND [c].[name] = N'Name');
IF @var21 IS NOT NULL EXEC(N'ALTER TABLE [AbpUsers] DROP CONSTRAINT [' + @var21 + '];');
ALTER TABLE [AbpUsers] ALTER COLUMN [Name] nvarchar(64) NOT NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20181013103914_Upgraded_To_Abp_v3_9_0', N'3.1.4');

GO

DECLARE @var22 sysname;
SELECT @var22 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUsers]') AND [c].[name] = N'LastLoginTime');
IF @var22 IS NOT NULL EXEC(N'ALTER TABLE [AbpUsers] DROP CONSTRAINT [' + @var22 + '];');
ALTER TABLE [AbpUsers] DROP COLUMN [LastLoginTime];

GO

DECLARE @var23 sysname;
SELECT @var23 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUserAccounts]') AND [c].[name] = N'LastLoginTime');
IF @var23 IS NOT NULL EXEC(N'ALTER TABLE [AbpUserAccounts] DROP CONSTRAINT [' + @var23 + '];');
ALTER TABLE [AbpUserAccounts] DROP COLUMN [LastLoginTime];

GO

ALTER TABLE [AbpAuditLogs] ADD [ReturnValue] nvarchar(max) NULL;

GO

CREATE TABLE [AbpOrganizationUnitRoles] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [TenantId] int NULL,
    [RoleId] int NOT NULL,
    [OrganizationUnitId] bigint NOT NULL,
    [IsDeleted] bit NOT NULL,
    CONSTRAINT [PK_AbpOrganizationUnitRoles] PRIMARY KEY ([Id])
);

GO

CREATE INDEX [IX_AbpOrganizationUnitRoles_TenantId_OrganizationUnitId] ON [AbpOrganizationUnitRoles] ([TenantId], [OrganizationUnitId]);

GO

CREATE INDEX [IX_AbpOrganizationUnitRoles_TenantId_RoleId] ON [AbpOrganizationUnitRoles] ([TenantId], [RoleId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190208051931_Upgrade_ABP_4_2_0', N'3.1.4');

GO

DROP INDEX [IX_AbpSettings_TenantId_Name] ON [AbpSettings];

GO

CREATE UNIQUE INDEX [IX_AbpSettings_TenantId_Name_UserId] ON [AbpSettings] ([TenantId], [Name], [UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190703062215_Upgraded_To_Abp_4_7_0', N'3.1.4');

GO

DROP INDEX [IX_AbpLanguageTexts_TenantId_Source_LanguageName_Key] ON [AbpLanguageTexts];
DECLARE @var24 sysname;
SELECT @var24 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpLanguageTexts]') AND [c].[name] = N'LanguageName');
IF @var24 IS NOT NULL EXEC(N'ALTER TABLE [AbpLanguageTexts] DROP CONSTRAINT [' + @var24 + '];');
ALTER TABLE [AbpLanguageTexts] ALTER COLUMN [LanguageName] nvarchar(128) NOT NULL;
CREATE INDEX [IX_AbpLanguageTexts_TenantId_Source_LanguageName_Key] ON [AbpLanguageTexts] ([TenantId], [Source], [LanguageName], [Key]);

GO

DROP INDEX [IX_AbpLanguages_TenantId_Name] ON [AbpLanguages];
DECLARE @var25 sysname;
SELECT @var25 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpLanguages]') AND [c].[name] = N'Name');
IF @var25 IS NOT NULL EXEC(N'ALTER TABLE [AbpLanguages] DROP CONSTRAINT [' + @var25 + '];');
ALTER TABLE [AbpLanguages] ALTER COLUMN [Name] nvarchar(128) NOT NULL;
CREATE INDEX [IX_AbpLanguages_TenantId_Name] ON [AbpLanguages] ([TenantId], [Name]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190719143908_Upgraded_To_Abp_4_8_0', N'3.1.4');

GO

DROP INDEX [IX_AbpUserLoginAttempts_TenancyName_UserNameOrEmailAddress_Result] ON [AbpUserLoginAttempts];
DECLARE @var26 sysname;
SELECT @var26 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpUserLoginAttempts]') AND [c].[name] = N'UserNameOrEmailAddress');
IF @var26 IS NOT NULL EXEC(N'ALTER TABLE [AbpUserLoginAttempts] DROP CONSTRAINT [' + @var26 + '];');
ALTER TABLE [AbpUserLoginAttempts] ALTER COLUMN [UserNameOrEmailAddress] nvarchar(256) NULL;
CREATE INDEX [IX_AbpUserLoginAttempts_TenancyName_UserNameOrEmailAddress_Result] ON [AbpUserLoginAttempts] ([TenancyName], [UserNameOrEmailAddress], [Result]);

GO

DECLARE @var27 sysname;
SELECT @var27 = [d].[name]
FROM [sys].[default_constraints] [d]
INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]
WHERE ([d].[parent_object_id] = OBJECT_ID(N'[AbpSettings]') AND [c].[name] = N'Value');
IF @var27 IS NOT NULL EXEC(N'ALTER TABLE [AbpSettings] DROP CONSTRAINT [' + @var27 + '];');
ALTER TABLE [AbpSettings] ALTER COLUMN [Value] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20191216011543_Upgraded_To_Abp_5_1_0', N'3.1.4');

GO

CREATE TABLE [AbpWebhookEvents] (
    [Id] uniqueidentifier NOT NULL,
    [WebhookName] nvarchar(max) NOT NULL,
    [Data] nvarchar(max) NULL,
    [CreationTime] datetime2 NOT NULL,
    [TenantId] int NULL,
    [IsDeleted] bit NOT NULL,
    [DeletionTime] datetime2 NULL,
    CONSTRAINT [PK_AbpWebhookEvents] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpWebhookSubscriptions] (
    [Id] uniqueidentifier NOT NULL,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [TenantId] int NULL,
    [WebhookUri] nvarchar(max) NOT NULL,
    [Secret] nvarchar(max) NOT NULL,
    [IsActive] bit NOT NULL,
    [Webhooks] nvarchar(max) NULL,
    [Headers] nvarchar(max) NULL,
    CONSTRAINT [PK_AbpWebhookSubscriptions] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpWebhookSendAttempts] (
    [Id] uniqueidentifier NOT NULL,
    [WebhookEventId] uniqueidentifier NOT NULL,
    [WebhookSubscriptionId] uniqueidentifier NOT NULL,
    [Response] nvarchar(max) NULL,
    [ResponseStatusCode] int NULL,
    [CreationTime] datetime2 NOT NULL,
    [LastModificationTime] datetime2 NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpWebhookSendAttempts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpWebhookSendAttempts_AbpWebhookEvents_WebhookEventId] FOREIGN KEY ([WebhookEventId]) REFERENCES [AbpWebhookEvents] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_AbpWebhookSendAttempts_WebhookEventId] ON [AbpWebhookSendAttempts] ([WebhookEventId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200220110527_Upgraded_To_Abp_5_2_0', N'3.1.4');

GO

DROP INDEX [IX_AbpOrganizationUnits_TenantId_Code] ON [AbpOrganizationUnits];

GO

CREATE TABLE [AbpDynamicParameters] (
    [Id] int NOT NULL IDENTITY,
    [ParameterName] nvarchar(450) NULL,
    [InputType] nvarchar(max) NULL,
    [Permission] nvarchar(max) NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpDynamicParameters] PRIMARY KEY ([Id])
);

GO

CREATE TABLE [AbpDynamicParameterValues] (
    [Id] int NOT NULL IDENTITY,
    [Value] nvarchar(max) NOT NULL,
    [TenantId] int NULL,
    [DynamicParameterId] int NOT NULL,
    CONSTRAINT [PK_AbpDynamicParameterValues] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpDynamicParameterValues_AbpDynamicParameters_DynamicParameterId] FOREIGN KEY ([DynamicParameterId]) REFERENCES [AbpDynamicParameters] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpEntityDynamicParameters] (
    [Id] int NOT NULL IDENTITY,
    [EntityFullName] nvarchar(450) NULL,
    [DynamicParameterId] int NOT NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpEntityDynamicParameters] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpEntityDynamicParameters_AbpDynamicParameters_DynamicParameterId] FOREIGN KEY ([DynamicParameterId]) REFERENCES [AbpDynamicParameters] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [AbpEntityDynamicParameterValues] (
    [Id] int NOT NULL IDENTITY,
    [Value] nvarchar(max) NOT NULL,
    [EntityId] nvarchar(max) NULL,
    [EntityDynamicParameterId] int NOT NULL,
    [TenantId] int NULL,
    CONSTRAINT [PK_AbpEntityDynamicParameterValues] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AbpEntityDynamicParameterValues_AbpEntityDynamicParameters_EntityDynamicParameterId] FOREIGN KEY ([EntityDynamicParameterId]) REFERENCES [AbpEntityDynamicParameters] ([Id]) ON DELETE CASCADE
);

GO

CREATE UNIQUE INDEX [IX_AbpOrganizationUnits_TenantId_Code] ON [AbpOrganizationUnits] ([TenantId], [Code]) WHERE [TenantId] IS NOT NULL;

GO

CREATE UNIQUE INDEX [IX_AbpDynamicParameters_ParameterName_TenantId] ON [AbpDynamicParameters] ([ParameterName], [TenantId]) WHERE [ParameterName] IS NOT NULL AND [TenantId] IS NOT NULL;

GO

CREATE INDEX [IX_AbpDynamicParameterValues_DynamicParameterId] ON [AbpDynamicParameterValues] ([DynamicParameterId]);

GO

CREATE INDEX [IX_AbpEntityDynamicParameters_DynamicParameterId] ON [AbpEntityDynamicParameters] ([DynamicParameterId]);

GO

CREATE UNIQUE INDEX [IX_AbpEntityDynamicParameters_EntityFullName_DynamicParameterId_TenantId] ON [AbpEntityDynamicParameters] ([EntityFullName], [DynamicParameterId], [TenantId]) WHERE [EntityFullName] IS NOT NULL AND [TenantId] IS NOT NULL;

GO

CREATE INDEX [IX_AbpEntityDynamicParameterValues_EntityDynamicParameterId] ON [AbpEntityDynamicParameterValues] ([EntityDynamicParameterId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200320114152_Upgraded_To_Abp_5_4_0', N'3.1.4');

GO

DROP INDEX [IX_AbpOrganizationUnits_TenantId_Code] ON [AbpOrganizationUnits];

GO

CREATE INDEX [IX_AbpOrganizationUnits_TenantId_Code] ON [AbpOrganizationUnits] ([TenantId], [Code]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200604091046_Upgraded_To_Abp_5_9', N'3.1.4');

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200607162236_createDatabase', N'3.1.4');

GO

CREATE TABLE [WithdrawRequests] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [IsDeleted] bit NOT NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [UserId] bigint NOT NULL,
    [Amount] decimal(18,2) NOT NULL,
    [WithdrawTypeId] int NOT NULL,
    [Status] bit NOT NULL,
    CONSTRAINT [PK_WithdrawRequests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_WithdrawRequests_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_WithdrawRequests_UserId] ON [WithdrawRequests] ([UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200628184414_AddedWithdrawRequestEntity', N'3.1.4');

GO

CREATE TABLE [UserReferralRequests] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [IsDeleted] bit NOT NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [UserId] bigint NOT NULL,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [UserName] nvarchar(max) NULL,
    [ReferralRequestStatusId] int NOT NULL,
    CONSTRAINT [PK_UserReferralRequests] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserReferralRequests_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE TABLE [UserReferrals] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [IsDeleted] bit NOT NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [UserId] bigint NOT NULL,
    [ReferralUserId] bigint NOT NULL,
    [ReferralAccountStatusId] int NOT NULL,
    [ReferralBonusStatusId] int NOT NULL,
    CONSTRAINT [PK_UserReferrals] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserReferrals_AbpUsers_ReferralUserId] FOREIGN KEY ([ReferralUserId]) REFERENCES [AbpUsers] ([Id]),
    CONSTRAINT [FK_UserReferrals_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id])
);

GO

CREATE INDEX [IX_UserReferralRequests_UserId] ON [UserReferralRequests] ([UserId]);

GO

CREATE INDEX [IX_UserReferrals_ReferralUserId] ON [UserReferrals] ([ReferralUserId]);

GO

CREATE INDEX [IX_UserReferrals_UserId] ON [UserReferrals] ([UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200628200033_AddedUserReferralEntities', N'3.1.4');

GO

CREATE TABLE [UserRequests] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [IsDeleted] bit NOT NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [FirstName] nvarchar(max) NULL,
    [LastName] nvarchar(max) NULL,
    [Email] nvarchar(max) NULL,
    [UserName] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [Password] nvarchar(max) NULL,
    CONSTRAINT [PK_UserRequests] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200708205136_AddedUserRequestEntity', N'3.1.4');

GO

CREATE TABLE [UserWithdrawDetails] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [IsDeleted] bit NOT NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [UserId] bigint NOT NULL,
    [WithdrawTypeId] int NOT NULL,
    [AccountTitle] nvarchar(max) NULL,
    [AccountIBAN] nvarchar(max) NULL,
    [JazzCashNumber] nvarchar(max) NULL,
    [EasyPaisaNumber] nvarchar(max) NULL,
    [IsPrimary] bit NOT NULL,
    CONSTRAINT [PK_UserWithdrawDetails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserWithdrawDetails_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_UserWithdrawDetails_UserId] ON [UserWithdrawDetails] ([UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200711151118_addedUserWithdrawDetailEntity', N'3.1.4');

GO

ALTER TABLE [UserRequests] ADD [PackageId] bigint NOT NULL DEFAULT CAST(0 AS bigint);

GO

ALTER TABLE [UserReferrals] ADD [PackageId] bigint NOT NULL DEFAULT CAST(0 AS bigint);

GO

ALTER TABLE [UserReferralRequests] ADD [PackageId] bigint NOT NULL DEFAULT CAST(0 AS bigint);

GO

CREATE TABLE [Packages] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [IsDeleted] bit NOT NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [Code] nvarchar(max) NULL,
    [Name] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
    [Price] decimal(18,2) NOT NULL,
    [ProfitValue] decimal(18,2) NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_Packages] PRIMARY KEY ([Id])
);

GO

CREATE INDEX [IX_UserRequests_PackageId] ON [UserRequests] ([PackageId]);

GO

CREATE INDEX [IX_UserReferrals_PackageId] ON [UserReferrals] ([PackageId]);

GO

CREATE INDEX [IX_UserReferralRequests_PackageId] ON [UserReferralRequests] ([PackageId]);

GO

ALTER TABLE [UserReferralRequests] ADD CONSTRAINT [FK_UserReferralRequests_Packages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [Packages] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [UserReferrals] ADD CONSTRAINT [FK_UserReferrals_Packages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [Packages] ([Id]) ON DELETE CASCADE;

GO

ALTER TABLE [UserRequests] ADD CONSTRAINT [FK_UserRequests_Packages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [Packages] ([Id]) ON DELETE CASCADE;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200712111502_AddedPackageEntityUpdatedRelatedEntities', N'3.1.4');

GO

CREATE TABLE [UserPackageSubscriptionDetails] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [IsDeleted] bit NOT NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [UserId] bigint NOT NULL,
    [PackageId] bigint NOT NULL,
    [StatusId] int NOT NULL,
    [StartDate] datetime2 NULL,
    [ExpiryDate] datetime2 NULL,
    CONSTRAINT [PK_UserPackageSubscriptionDetails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserPackageSubscriptionDetails_Packages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [Packages] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserPackageSubscriptionDetails_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_UserPackageSubscriptionDetails_PackageId] ON [UserPackageSubscriptionDetails] ([PackageId]);

GO

CREATE INDEX [IX_UserPackageSubscriptionDetails_UserId] ON [UserPackageSubscriptionDetails] ([UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200712122815_AddedUserPackageSubscriptionDetailEntity', N'3.1.4');

GO

CREATE TABLE [UserPersonalDetails] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [IsDeleted] bit NOT NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [UserId] bigint NOT NULL,
    [Gender] int NOT NULL,
    [NicNumber] nvarchar(max) NULL,
    [Birthday] datetime2 NOT NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [Address] nvarchar(max) NULL,
    [City] nvarchar(max) NULL,
    [State] nvarchar(max) NULL,
    [Country] nvarchar(max) NULL,
    [PostalCode] nvarchar(max) NULL,
    CONSTRAINT [PK_UserPersonalDetails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserPersonalDetails_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_UserPersonalDetails_UserId] ON [UserPersonalDetails] ([UserId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200725222037_addedUserPersonalDetailEntity', N'3.1.4');

GO

CREATE TABLE [PackageAds] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [IsDeleted] bit NOT NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [PackageId] bigint NOT NULL,
    [Title] nvarchar(max) NULL,
    [Url] nvarchar(max) NULL,
    [Price] decimal(18,2) NOT NULL,
    [IsActive] bit NOT NULL,
    CONSTRAINT [PK_PackageAds] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_PackageAds_Packages_PackageId] FOREIGN KEY ([PackageId]) REFERENCES [Packages] ([Id]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_PackageAds_PackageId] ON [PackageAds] ([PackageId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200725225452_addedpackageAdEntity', N'3.1.4');

GO

ALTER TABLE [Packages] ADD [DailyAdCount] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Packages] ADD [DurationInDays] int NOT NULL DEFAULT 0;

GO

ALTER TABLE [Packages] ADD [ReferralAmount] decimal(18,2) NOT NULL DEFAULT 0.0;

GO

ALTER TABLE [Packages] ADD [TotalEarning] decimal(18,2) NOT NULL DEFAULT 0.0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200727191656_UpdatedPackagesEntities', N'3.1.4');

GO

ALTER TABLE [Packages] ADD [PricePerAd] decimal(18,2) NOT NULL DEFAULT 0.0;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200808183054_UpdatePackageEntityFOrPricePerAd', N'3.1.4');

GO

ALTER TABLE [Packages] ADD [IsUnlimited] bit NOT NULL DEFAULT CAST(0 AS bit);

GO

ALTER TABLE [Packages] ADD [Limit] int NULL;

GO

ALTER TABLE [Packages] ADD [MaximumWithdraw] decimal(18,2) NULL;

GO

ALTER TABLE [Packages] ADD [MinimumWithdraw] decimal(18,2) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200809103133_UpdatedPackgeEntityForLimitValidation', N'3.1.4');

GO

CREATE TABLE [UserPackageAdDetails] (
    [Id] bigint NOT NULL IDENTITY,
    [CreationTime] datetime2 NOT NULL,
    [CreatorUserId] bigint NULL,
    [LastModificationTime] datetime2 NULL,
    [LastModifierUserId] bigint NULL,
    [IsDeleted] bit NOT NULL,
    [DeleterUserId] bigint NULL,
    [DeletionTime] datetime2 NULL,
    [UserId] bigint NOT NULL,
    [UserPackageSubscriptionDetailId] bigint NOT NULL,
    [PackageAdId] bigint NULL,
    [PackageId] bigint NOT NULL,
    [AdPrice] decimal(18,2) NOT NULL,
    [AdDate] datetime2 NOT NULL,
    [IsViewed] bit NOT NULL,
    CONSTRAINT [PK_UserPackageAdDetails] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_UserPackageAdDetails_PackageAds_PackageAdId] FOREIGN KEY ([PackageAdId]) REFERENCES [PackageAds] ([Id]) ON DELETE NO ACTION,
    CONSTRAINT [FK_UserPackageAdDetails_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserPackageAdDetails_UserPackageSubscriptionDetails_UserPackageSubscriptionDetailId] FOREIGN KEY ([UserPackageSubscriptionDetailId]) REFERENCES [UserPackageSubscriptionDetails] ([Id])
);

GO

CREATE INDEX [IX_UserPackageAdDetails_PackageAdId] ON [UserPackageAdDetails] ([PackageAdId]);

GO

CREATE INDEX [IX_UserPackageAdDetails_UserId] ON [UserPackageAdDetails] ([UserId]);

GO

CREATE INDEX [IX_UserPackageAdDetails_UserPackageSubscriptionDetailId] ON [UserPackageAdDetails] ([UserPackageSubscriptionDetailId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200809150340_AddedUserPackageAdDetailEntity', N'3.1.4');

GO

ALTER TABLE [UserRequests] ADD [UserId] bigint NULL;

GO

CREATE INDEX [IX_UserRequests_UserId] ON [UserRequests] ([UserId]);

GO

ALTER TABLE [UserRequests] ADD CONSTRAINT [FK_UserRequests_AbpUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200811202603_updatedUserRequestEntity', N'3.1.4');

GO

ALTER TABLE [UserReferralRequests] ADD [PhoneNumber] nvarchar(max) NULL;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200812083624_UpdatedUserReferralRequestEntityForPhoneNumber', N'3.1.4');

GO

ALTER TABLE [UserReferralRequests] ADD [UserReferralId] bigint NULL;

GO

CREATE INDEX [IX_UserReferralRequests_UserReferralId] ON [UserReferralRequests] ([UserReferralId]);

GO

ALTER TABLE [UserReferralRequests] ADD CONSTRAINT [FK_UserReferralRequests_AbpUsers_UserReferralId] FOREIGN KEY ([UserReferralId]) REFERENCES [AbpUsers] ([Id]) ON DELETE NO ACTION;

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200812115104_UpdatedUserReferralRequestEntity', N'3.1.4');

GO



IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [ClientTokens] (
    [Id] uniqueidentifier NOT NULL,
    [Token] nvarchar(4000) NOT NULL,
    [CreatedTs] datetime2 NOT NULL DEFAULT (GETUTCDATE()),
    CONSTRAINT [PK_ClientTokens] PRIMARY KEY ([Id])
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20200323085345_InitialCreate', N'2.1.11-servicing-32099');

GO


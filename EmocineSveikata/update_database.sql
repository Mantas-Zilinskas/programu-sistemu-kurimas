-- Create UserProfiles table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UserProfiles')
BEGIN
    CREATE TABLE [dbo].[UserProfiles] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [UserId] INT NOT NULL,
        [ProfilePicture] NVARCHAR(MAX) NULL,
        [SelectedTopics] NVARCHAR(MAX) NULL,
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [FK_UserProfiles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_UserProfiles_UserId] ON [dbo].[UserProfiles] ([UserId]);
END
ELSE
BEGIN
    -- Add missing columns to UserProfiles if they don't exist
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = 'ProfilePicture' AND object_id = OBJECT_ID('UserProfiles'))
    BEGIN
        ALTER TABLE [dbo].[UserProfiles] ADD [ProfilePicture] NVARCHAR(MAX) NULL;
    END

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = 'SelectedTopics' AND object_id = OBJECT_ID('UserProfiles'))
    BEGIN
        ALTER TABLE [dbo].[UserProfiles] ADD [SelectedTopics] NVARCHAR(MAX) NULL;
    END
END

-- Create SpecialistProfiles table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SpecialistProfiles')
BEGIN
    CREATE TABLE [dbo].[SpecialistProfiles] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [UserId] INT NOT NULL,
        [ProfilePicture] NVARCHAR(MAX) NULL,
        [Bio] NVARCHAR(MAX) NULL,
        [SelectedTopics] NVARCHAR(MAX) NULL,
        [UpdatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [FK_SpecialistProfiles_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE
    );

    CREATE INDEX [IX_SpecialistProfiles_UserId] ON [dbo].[SpecialistProfiles] ([UserId]);
END
ELSE
BEGIN
    -- Add missing columns to SpecialistProfiles if they don't exist
    IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = 'ProfilePicture' AND object_id = OBJECT_ID('SpecialistProfiles'))
    BEGIN
        ALTER TABLE [dbo].[SpecialistProfiles] ADD [ProfilePicture] NVARCHAR(MAX) NULL;
    END

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = 'Bio' AND object_id = OBJECT_ID('SpecialistProfiles'))
    BEGIN
        ALTER TABLE [dbo].[SpecialistProfiles] ADD [Bio] NVARCHAR(MAX) NULL;
    END

    IF NOT EXISTS (SELECT * FROM sys.columns WHERE name = 'SelectedTopics' AND object_id = OBJECT_ID('SpecialistProfiles'))
    BEGIN
        ALTER TABLE [dbo].[SpecialistProfiles] ADD [SelectedTopics] NVARCHAR(MAX) NULL;
    END
END

-- Create SpecialistTimeSlots table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'SpecialistTimeSlots')
BEGIN
    CREATE TABLE [dbo].[SpecialistTimeSlots] (
        [Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        [UserId] INT NOT NULL,
        [Date] DATETIME2 NOT NULL,
        [StartTime] TIME NOT NULL,
        [EndTime] TIME NOT NULL,
        [IsBooked] BIT NOT NULL DEFAULT 0,
        [BookedByUserId] INT NULL,
        [CreatedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
        CONSTRAINT [FK_SpecialistTimeSlots_Users_UserId] FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_SpecialistTimeSlots_Users_BookedByUserId] FOREIGN KEY ([BookedByUserId]) REFERENCES [dbo].[Users] ([Id])
    );

    CREATE INDEX [IX_SpecialistTimeSlots_UserId] ON [dbo].[SpecialistTimeSlots] ([UserId]);
    CREATE INDEX [IX_SpecialistTimeSlots_BookedByUserId] ON [dbo].[SpecialistTimeSlots] ([BookedByUserId]) WHERE [BookedByUserId] IS NOT NULL;
END

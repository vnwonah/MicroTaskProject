CREATE PROCEDURE [dbo].[sp_NewTeam]

    @Id  INT,
    @Name NVARCHAR(128),
    @LogoLink NVARCHAR(30) = 'default',
    @CreatedAt DATETIME,
    @IsDeleted SMALLINT = 0
AS
    IF @CreatedAt IS NULL
    BEGIN 
        RAISERROR ('Error. @CreatedAt must be specified', 11, 1)
        RETURN 1
    END
    IF @Id IS NULL
    BEGIN
        RAISERROR ('Error. @Id must be specified', 11, 1)
        RETURN 1
    END

    IF @Name IS NULL
    BEGIN
        RAISERROR ('Error. @Name must be specified', 11, 1)
        RETURN 1
    END
      
     --TODO: REMEMBER TO CHANGE DEFAULT VALUES!!!
     --Insert ID
    SET IDENTITY_INSERT [dbo].[Teams] ON
     -- Insert Team
    INSERT INTO [dbo].Teams
        ([Id],[Name],[LogoLink], [UTCCreatedAt], [IsDeleted], 
        [MaxUsers], [MaxRecord], [MaxRecordsPerMth], [RecordsThisMth], 
        [RecordsThisYear], [MaxForms], [DatePaid], [ResetDate],[NextSubscriptionDate],
        [LastApiTimestamp], [PaymentLastApiTimestamp], [DisplayCampaignTab], [DisplayReportTab])         
    VALUES
        (@Id, @Name,@LogoLink, @CreatedAt, @IsDeleted, 0, 0, 0, 0, 0, 0,
        '20120618 10:34:09 AM', '20120618 10:34:09 AM', 
        '20120618 10:34:09 AM','20120618 10:34:09 AM','20120618 10:34:09 AM',
        1,1 )

RETURN 0
--SET IDENTITY_INSERT [dbo].[Teams] ON
 
 
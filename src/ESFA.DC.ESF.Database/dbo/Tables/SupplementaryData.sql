CREATE TABLE [dbo].[SupplementaryDataModel] (
    [SupplementaryDataModelId]        INT            IDENTITY (1, 1) NOT NULL,
    [ConRefNumber]               VARCHAR (20)   NOT NULL,
    [DeliverableCode]            VARCHAR (10)   NOT NULL,
    [CalendarYear]               INT            NOT NULL,
    [CalendarMonth]              INT            NOT NULL,
    [CostType]                   VARCHAR (20)   NOT NULL,
    [StaffName]                  VARCHAR (100)  NULL,
    [ReferenceType]              VARCHAR (20)   NOT NULL,
    [Reference]                  VARCHAR (100)  NOT NULL,
    [ULN]                        BIGINT         NULL,
    [ProviderSpecifiedReference] VARCHAR (200)  NULL,
    [Value]                      DECIMAL (8, 2) NULL,
    [HourlyRate]                 DECIMAL (8, 2) NULL,
    [TotalHoursWorked]           DECIMAL (8, 2) NULL,
    [ProjectHours]               DECIMAL (8, 2) NULL,
    [OrgHours]                   DECIMAL (8, 2) NULL,
    [SourceFileId]               INT            NOT NULL,
    CONSTRAINT [PK_SupplementaryDataModel] PRIMARY KEY CLUSTERED ([ConRefNumber] ASC, [DeliverableCode] ASC, [CalendarYear] ASC, [CalendarMonth] ASC, [CostType] ASC, [ReferenceType] ASC, [Reference] ASC),
    CONSTRAINT [FK_SourceFile] FOREIGN KEY ([SourceFileId]) REFERENCES [dbo].[SourceFile] ([SourceFileId])
);


CREATE TABLE [dbo].[SupplementaryDataUnitCost] (
    [ConRefNumber]    VARCHAR (20)   NOT NULL,
    [DeliverableCode] VARCHAR (10)   NOT NULL,
    [CalendarYear]    INT            NOT NULL,
    [CalendarMonth]   INT            NOT NULL,
    [CostType]        VARCHAR (20)   NOT NULL,
    [StaffName]       VARCHAR (100)  NULL,
    [ReferenceType]   VARCHAR (20)   NOT NULL,
    [Reference]       VARCHAR (100)  NOT NULL,
    [Value]           DECIMAL (8, 2) NULL,
    CONSTRAINT [PK_SupplementaryDataUnitCost] PRIMARY KEY CLUSTERED ([ConRefNumber] ASC, [DeliverableCode] ASC, [CalendarYear] ASC, [CalendarMonth] ASC, [CostType] ASC, [ReferenceType] ASC, [Reference] ASC),
    CONSTRAINT [FK_SupplementaryDataUnitCost_SupplementaryData] FOREIGN KEY ([ConRefNumber], [DeliverableCode], [CalendarYear], [CalendarMonth], [CostType], [ReferenceType], [Reference]) REFERENCES [dbo].[SupplementaryData] ([ConRefNumber], [DeliverableCode], [CalendarYear], [CalendarMonth], [CostType], [ReferenceType], [Reference]) ON DELETE CASCADE
);


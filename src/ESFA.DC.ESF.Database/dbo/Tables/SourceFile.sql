CREATE TABLE [dbo].[SourceFile] (
    [SourceFileId]        INT           IDENTITY (1, 1) NOT NULL,
    [FileName]            NVARCHAR (60) NOT NULL,
    [FilePreparationDate] DATETIME      NOT NULL,
    [ConRefNumber]        VARCHAR (20)  NOT NULL,
    [UKPRN]               NVARCHAR (20) NOT NULL,
    [DateTime]            DATETIME      DEFAULT (getdate()) NULL,
    CONSTRAINT [PK_SourceFile] PRIMARY KEY CLUSTERED ([SourceFileId] ASC)
);


create procedure [dbo].[DeleteExistingRecords] (
	@ukprn int,
	@fileName varchar(100)
) as
begin
	delete sd
	from dbo.SupplementaryData sd
		inner join dbo.SourceFile sf on sf.SourceFileId = sd.SourceFileId
	where sf.UKPRN = @ukprn AND sf.Filename = @fileName
	
	DELETE ve
	FROM dbo.ValidationError ve
		INNER JOIN dbo.SourceFile sf ON sf.SourceFileId = ve.SourceFileId
	WHERE sf.UKPRN = @ukprn AND sf.Filename = @fileName
    
	delete from dbo.SourceFile where UKPRN = @ukprn and Filename = @fileName
end

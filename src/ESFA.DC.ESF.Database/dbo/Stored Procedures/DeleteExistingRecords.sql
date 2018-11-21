create procedure [dbo].[DeleteExistingRecords] (
	@ukprn int,
	@conRefNum varchar(20)
) as
begin
	delete sd
	from dbo.SupplementaryData sd
		inner join dbo.SourceFile sf on sf.SourceFileId = sd.SourceFileId
	where sf.UKPRN = @ukprn AND sf.ConRefNumber = @conRefNum
	
	DELETE ve
	FROM dbo.ValidationError ve
		INNER JOIN dbo.SourceFile sf ON sf.SourceFileId = ve.SourceFileId
	WHERE sf.UKPRN = @ukprn AND sf.ConRefNumber = @conRefNum
    
	delete from dbo.SourceFile where UKPRN = @ukprn and ConRefNumber = @conRefNum
end

create procedure [dbo].[DeleteExistingRecords] (
	@ukprn int,
	@conRefNum varchar(20)
) as
begin
	delete sd
	from dbo.SupplementaryData sd
		inner join dbo.SourceFile sf on sf.SourceFileId = sd.SourceFileId
	where sf.UKPRN = @ukprn and sf.ConRefNumber = @conRefNum;
	
	delete ve
	from dbo.ValidationError ve
		inner join dbo.SourceFile sf on sf.SourceFileId = ve.SourceFileId
	where sf.UKPRN = @ukprn and sf.ConRefNumber = @conRefNum;
    
	--delete from dbo.SourceFile where UKPRN = @ukprn and ConRefNumber = @conRefNum;
end
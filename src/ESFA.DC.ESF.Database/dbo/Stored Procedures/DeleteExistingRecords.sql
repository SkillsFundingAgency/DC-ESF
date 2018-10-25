create procedure [dbo].[DeleteExistingRecords] (
	@ukprn int,
	@fileName varchar(100)
) as
begin
	delete sd
	from dbo.SupplementaryData sd
		inner join dbo.SourceFile sf on sf.SourceFileId = sd.SourceFileId
	where sf.UKPRN = @ukprn

	delete ve
	from dbo.ValidationError ve
		inner join dbo.SourceFile sf on sf.SourceFileId = ve.SourceFileId
	where sf.UKPRN = @ukprn

	delete from dbo.SourceFile where UKPRN = @ukprn and Filename = @fileName
end

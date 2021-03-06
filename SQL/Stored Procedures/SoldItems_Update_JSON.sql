USE [Ebay]
GO

CREATE OR ALTER Proc [dbo].[SoldItems_Update_JSON]
(
	@json NVARCHAR(MAX) = null
)AS
/***********************************************************************************\
Name:		[SoldItems_Update_JSON]
Purpose:	Pass JSON and then iterate and insert/update records into Sold Items 
			table
			
			
Created:	2019.10.21
Creator:	David Sargent

			--exec SoldItems_Update_JSON
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.21	DS	Created

\***********************************************************************************/

BEGIN

CREATE TABLE #tmp_20190801_SoldItems
(
	Id					int PRIMARY KEY IDENTITY(1,1),
	EbayUser			varchar(50)		,
	EbayId				varchar(50)		,
	Title				varchar(180)	,
	LastDateSold		datetime 
)


Insert into #tmp_20190801_SoldItems(EbayUser,EbayId,Title,LastDateSold)

SELECT EbayUser,EbayId,Title,DateLastSold
FROM OPENJSON(@json)  
  WITH 
	(
		Id int,  
        EbayUser nvarchar(50),
		EbayId nvarchar(50),
		Title nvarchar(50),
		DateLastSold datetime
	) 


DECLARE @start int, @end int
select @start = 1,@end = count(*) from #tmp_20190801_SoldItems

WHILE(@start <= @end )
BEGIN
	DECLARE @EbayUser varchar(50),@EbayId varchar(50), @LastDateSold datetime, @Title varchar(180)

	select @EbayUser = EbayUser, @EbayId = EbayId, @LastDateSold = LastDateSold, @Title = Title from #tmp_20190801_SoldItems where id = @start

	IF NOT EXISTS( select * from SoldItems where EbayUser = @EbayUser AND EbayId = @EbayId )
	BEGIN
		Insert into SoldItems(EbayUser,EbayId,Title,LastDateSold)
		select @EbayUser,@EbayId,@Title,@LastDateSold

	END ELSE 
	IF NOT EXISTS( select * from SoldItems where EbayUser = @EbayUser AND EbayId = @EbayId AND LastDateSold = @LastDateSold )
	BEGIN
		UPDATE SoldItems
			set 
				LastDateSold = @LastDateSold,
				DateM = GETDATE()
			where 1=1
				and EbayUser = @EbayUser
				and EbayId = @EbayId

	END

select @start = @start + 1
END

	
END


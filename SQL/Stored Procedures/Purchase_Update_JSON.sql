USE [Ebay]
GO

CREATE OR ALTER proc [dbo].[Purchase_Update_JSON]
(
	@json NVARCHAR(MAX) = null
)AS
/***********************************************************************************\
Name:		[Purchase_Update_JSON]
Purpose:	Iterate and insert purchase records into the purchase table if they
			don't already exist

Created:	2019.10.01
Creator:	David Sargent
			
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/
Begin

CREATE TABLE #tmp_20190801_Purchase
(
	Id					int PRIMARY KEY IDENTITY(1,1)	,
	ItemId				varchar(50)						,
	Quantity			int								,
	Price				money							,
	DateOfPurchase		datetime						,
	EbayUser			varchar(50)						
)


	

Insert into #tmp_20190801_Purchase(ItemId,Quantity,Price,DateOfPurchase,EbayUser)

SELECT ItemId,Quantity,Price,DateOfPurchase,EbayUser
FROM OPENJSON(@json)  
  WITH 
	(
		Id					int 							,
		ItemId				varchar(50)						,
		Quantity			int								,
		Price				money							,
		DateOfPurchase		datetime						,
		EbayUser			varchar(50)			
	) 


DECLARE @start int, @end int
select @start = 1,@end = count(*) from #tmp_20190801_Purchase

WHILE(@start <= @end )
BEGIN
	DECLARE @EbayUser varchar(50),@ItemId varchar(50), @DateOfPurchase datetime, @Price money, @Quantity int

	select @EbayUser = EbayUser, @ItemId = ItemId, @DateOfPurchase = DateOfPurchase, @Quantity = Quantity, @Price = Price from #tmp_20190801_Purchase where id = @start

	IF NOT EXISTS( select * from Purchase where EbayUser = @EbayUser AND ItemId = @ItemId AND DateOfPurchase = @DateOfPurchase )
	BEGIN
		Insert into Purchase(ItemId,Quantity,Price,DateOfPurchase,EbayUser)
			select @ItemId,@Quantity,@Price, @DateOfPurchase, @EbayUser

	END

select @start = @start + 1
END

Update si
	set si.LastDetailRun = GETDATE()
	from SoldItems si
		inner join #tmp_20190801_Purchase p on (si.EbayId = p.ItemId) and (si.EbayUser = p.EbayUser)
	

END


USE [Ebay]
GO

CREATE OR ALTER Proc [dbo].[Purchase_Update]
(
	@ItemId				varchar(50),
	@Quantity			int ,
	@Price				money ,
	@DateOfPurchase		datetime,
	@EbayUser			varchar(50)
)AS
/***********************************************************************************\
Name:		[Purchase_Update]
Purpose:	Update or insert a purchase 
Created:	2019.10.01
Creator:	David Sargent
			
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/
BEGIN

IF NOT EXISTS( select * from dbo.Purchase where 1=1 AND ItemId = @ItemId and DateOfPurchase = @DateOfPurchase )
BEGIN
Insert into dbo.Purchase(ItemId,Quantity,Price,DateOfPurchase,EbayUser)
	select @ItemId,@Quantity,@Price,@DateOfPurchase,@EbayUser

END
ELSE IF NOT EXISTS ( select * from dbo.Purchase where 1=1 AND ItemId = @ItemId and DateOfPurchase = @DateOfPurchase and @Quantity = Quantity )
BEGIN
	Update dbo.Purchase
		set 
				Quantity = @Quantity,
				DateCreated = Getdate()
		where 1=1
			and ItemId = @ItemId
			and DateOfPurchase = @DateOfPurchase
END


END




USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[EbayListing_Update]
(
	@EbayId							varchar(20),
	@Quantity						int,
	@Title							varchar(80),
	@Price							money,
	@Views							int,
	@Watchers						int,
	@Shipping						money,
	@PromotedListing				decimal = 0,
	@TrendingPromotedListing		decimal = 0,
	@StoreId						int = 3
)AS
/***********************************************************************************\
Name:		[EbayListing_Update]
Purpose:	Update info about ebay listing
			
Created:	2019.10.01
Creator:	David Sargent


------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/AS
BEGIN
IF EXISTS(select * from EbayListings where 1=1 and EbayId = @EbayId)	BEGIN
update EbayListings
	set
		Quantity = @Quantity,
		Price = @Price,
		[Views] = @Views,
		Watchers = @Watchers,
		Title = @Title,
		DateM = Getdate(),
		Shipping = @Shipping,
		PromotedListing = @PromotedListing,
		TrendingPromotedListing = @TrendingPromotedListing,
		StoreId = @StoreId
	where 1=1
		and EbayId = @EbayId

END
ELSE		BEGIN
Insert into EbayListings(EbayId,Quantity,Price,[Views],Watchers,Title,Shipping,PromotedListing,TrendingPromotedListing,StoreId)
	select @EbayId,@Quantity,@Price,@Views,@Watchers,@Title,@Shipping,@PromotedListing,@TrendingPromotedListing,@StoreId
END

END



USE [Ebay]
GO

CREATE OR ALTER proc [dbo].[SoldItems_Update]
(
	@EbayUser varchar(50),
	@EbayId varchar(50),
	@Title varchar(180),
	@LastDateSold datetime = null
)AS
/***********************************************************************************\
Name:		[SoldItems_Update]
Purpose:	Insert/update record into Sold Items table
			
			
Created:	2019.10.21
Creator:	David Sargent

			
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.21	DS	Created

\***********************************************************************************/


Begin
IF NOT EXISTS (select * from SoldItems where EbayUser = @EbayUser and @EbayId = EbayId)
BEGIN
	Insert into SoldITems(EbayUser,EbayId,Title,LastDateSold)
		select @EbayUser,@EbayId,@Title,@LastDateSold

END
ELSE BEGIN
	Update SoldItems
		Set Title = @Title,
		LastDateSold = @LastDateSold
	where 1=1
		and EbayUser = @EbayUser
		and EbayId = @EbayId
END

END
USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[ZIK_Upate]
(
	@ImageUrl					varchar(50),
	@ProductName				varchar(100),
	@EbayUrl					varchar(100),
	@EbayId						bigint,
	@UploadDate					Datetime,
	@MonthSales					int,
	@TotalSold					int,
	@CurrentPrice				money,
	@LastSalePrice				money,
	@UPC						varchar(100)  = NULL,
	@EAN						varchar(100)  = NULL,
	@SourceURL					varchar(200)	= NULL,
	@SourceId					varchar(100)	= NULL,
	@SourcePrice				money	= NULL,
	@SourceFufill				varchar(50) =  NULL,
	@MatchCount					int
)AS
/***********************************************************************************\
Name:		[ZIK_Upate]
Purpose:	Update or create items scraped from Zik
			
Created:	2019.10.01
Creator:	David Sargent
			
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.21	DS	Created

\***********************************************************************************/
BEGIN
DECLARE @count int
select @count = count(*) from ZIK where EbayId = @EbayId

IF(@count = 0)		BEGIN
Insert into Zik(ImageUrl,ProductName,EbayUrl,EbayId,UploadDate,MonthSales,TotalSold,CurrentPrice,LastSalePrice,UPC,SourceURL,SourceId,SourcePrice,SourceFufill,EAN)
	select @ImageUrl,@ProductName,@EbayUrl,@EbayId,@UploadDate,@MonthSales,@TotalSold,@CurrentPrice,@LastSalePrice,@UPC,@SourceURL,@SourceId,@SourcePrice,@SourceFufill,@EAN

END
ELSE	BEGIN
	UPDATE ZIK
		set UPC = @UPC,
		EAN = @EAN,
		SourceURL = @SourceURL,
		SourceId = @SourceId,
		SourcePrice = @SourcePrice,
		SourceFufill = @SourceFufill,
		MatchCount = @MatchCount
		Where 1=1
			and EbayId = @EbayId
END


END


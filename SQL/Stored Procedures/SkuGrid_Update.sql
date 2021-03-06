USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[SkuGrid_Update]
(
	@EbayId varchar(20) = null,
	@SupplierId varchar(20),
	@SupplierName varchar(20),
	@EbayPrice money,
	@SupplierPrice money,
	@SupplierShipping money,
	@IsInStock bit,
	@DispatchTime int,
	@RetailPrice money,
	@UpdatedTimeSkuGrid datetime,
	@DateAddedSkuGrid datetime,
	@SupplierImageUrl varchar(200),
	@Reference varchar(20)
	--,@DatePriceUpdated datetime
)AS
/***********************************************************************************\
Name:		[SkuGrid_Update]
Purpose:	Update items that have changed from scraping SKUGrid and send an email	
			with the details of the change
			
Created:	2019.08.01
Creator:	David Sargent

			exec [SnipeList_Get] @end = '2019-10-01'
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.08.01	DS	Created

\***********************************************************************************/
BEGIN

IF(@EbayId is null) BEGIN
	select @ebayId = @reference
END

IF EXISTS(select * from SkuGrid where 1=1 and SupplierId = @SupplierId)	BEGIN

	Declare @SupplierPriceOld money
	Declare @messageBody varchar(1000)
	DECLARE @EbayIdD varchar(20)
	Declare @InstockOld bit
	
	select @SupplierPriceOld = SupplierPrice, @EbayIdD = EbayId, @InstockOld = IsInStock from SkuGrid where 1=1 and SupplierId = @SupplierId
	
	IF NOT EXISTS( select * from SkuGrid where 1=1 and SupplierId = @SupplierId and SupplierPrice = @SupplierPrice and IsInStock = @IsInStock /*and DispatchTime = @DispatchTime*/ ) BEGIN
	
		Update SkuGrid
			set 
				SupplierPrice = @SupplierPrice,
				SupplierShipping = @SupplierShipping,
				DispatchTime = @DispatchTime,
				DateUpdated = GETDATE(),
				IsInStock = @IsInStock,
				RetailPrice = @RetailPrice,
				Reference = @Reference,
				--SupplierImageUrl = @SupplierImageUrl,
				UpdatedTimeSkuGrid = @UpdatedTimeSkuGrid,
				DatePriceUpdated = GETDATE()
			where 1=1
				and SupplierId = @SupplierId

		select @messageBody = '<html><head></head><body>'
		select @messageBody = @messageBody + '<div><img src="https://thumbs.ebaystatic.com/pict/'+ @EbayIdD +'6464_1.jpg"/></div>'
		select @messageBody = @messageBody + '<h2>In Stock Old: ' + cast(@InstockOld as varchar(2)) + '.In Stock New: ' + cast(@IsInStock as varchar(2)) + '</h2>'
		
		
		
		select @messageBody = @messageBody + '<h2> Price Old: $' + cast(@SupplierPriceOld as varchar(20)) +'.Price new: $' +  cast(@SupplierPrice as varchar(20))

		--select @messageBody = @messageBody + '<h1>Cool</h1>'

		
		select @messageBody = @messageBody + '<div><a href="https://www.ebay.com/itm/' +  @EbayIdD + '" target="_blank">Ebay</a></div>'
		select @messageBody = @messageBody + '<div><a href="https://www.walmart.com/ip/' +  @SupplierId + '" target="_blank">Walmart</a></div>'
		select @messageBody =  @messageBody + '</body></html>'


		EXEC msdb.dbo.sp_send_dbmail  
			@profile_name = 'Gmail',  
			@recipients = 'dsargent9182@gmail.com',  
			@subject = 'Sku Grid price / stock changed detected --> PRICE UPDATE', 
			@body = @messageBody
			,@body_format='html'
			

	END
	ELSE	BEGIN
	
		Update SkuGrid
			set 
				UpdatedTimeSkuGrid = @UpdatedTimeSkuGrid,
				DateUpdated = GETDATE()
			where 1=1
				and SupplierId = @SupplierId
			
		select @messageBody = '<html><head></head><body>'
		select @messageBody = @messageBody + '<div><img src="https://thumbs.ebaystatic.com/pict/'+ @EbayIdD +'6464_1.jpg"/></div>'
		select @messageBody = @messageBody + '<h2>In Stock Old: ' + cast(@InstockOld as varchar(2)) + '.In Stock New: ' + cast(@IsInStock as varchar(2)) + '</h2>'
		
		
		
		select @messageBody = @messageBody + '<h2> Price Old: $' + cast(@SupplierPriceOld as varchar(20)) +'.Price new: $' +  cast(@SupplierPrice as varchar(20))

		--select @messageBody = @messageBody + '<h1>Cool</h1>'

		
		select @messageBody = @messageBody + '<div><a href="https://www.ebay.com/itm/' +  @EbayIdD + '" target="_blank">Ebay</a></div>'
		select @messageBody = @messageBody + '<div><a href="https://www.walmart.com/ip/' +  @SupplierId + '" target="_blank">Walmart</a></div>'
		select @messageBody =  @messageBody + '</body></html>'

		/*
		EXEC msdb.dbo.sp_send_dbmail  
			@profile_name = 'Gmail',  
			@recipients = 'dsargent9182@gmail.com',  
			@subject = 'Sku Grid price / stock changed detected', 
			@body = @messageBody
			,@body_format='html'
		*/

	END



END

ELSE	BEGIN

Insert into SkuGrid(EbayId,SupplierId,SupplierName,EbayPrice,SupplierPrice,SupplierShipping,IsInStock,DispatchTime,RetailPrice,UpdatedTimeSkuGrid,DateAddedSkuGrid,SupplierImageUrl,Reference)
select 
	@EbayId,
	@SupplierId,
	@SupplierName,
	@EbayPrice,
	@SupplierPrice,
	@SupplierShipping,
	@IsInStock,
	@DispatchTime,
	@RetailPrice,
	@UpdatedTimeSkuGrid,
	@DateAddedSkuGrid,
	@SupplierImageUrl,
	@Reference

END
END


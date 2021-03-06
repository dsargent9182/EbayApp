USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[SkuGrid_Get]
/***********************************************************************************\
Name:		[SkuGrid_Get]
Purpose:	Computes profit or loss from items listed in Ebay compared to their\
			current price on the supplies website from SkuGrid
			
			
Created:	2019.10.21
Creator:	David Sargent

			exec [SkuGrid_Get]
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.21	DS	Created

\***********************************************************************************/AS
BEGIN
select  
		sg.Id,
		ROW_NUMBER() OVER(ORDER BY sg.Id) 'RowNumber',
		ISNULL(el.Quantity,0)'Quantity',
		s.IsTaxExempt,
		sg.EbayId,
		SupplierId,
		sg.SupplierName,
		ISNULL(el.PromotedListing,0.0) 'PromotedListing',
		ISNULL(el.Price,0) 'EbayPrice',
		ISNULL(el.Shipping,0) 'EbayShipping',
		ISNULL(SupplierPrice * sg.Quantity,0) + ISNULL(SupplierShipping,0) 'SupplierPrice',
		((ISNULL(el.Price,0) * .029) + .30) + (ISNULL(el.Price,0) * .0915) + (ISNULL(el.Price,0) * .01) 'EbayFees',
		CASE 
			WHEN ISNULL(s.IsTaxExempt,0) = 0 then
				( (ISNULL(el.Price,0) + ISNULL(el.Shipping,0) ) -								--Ebay Price + Ebay Shipping
				(ISNULL(SupplierPrice* sg.Quantity,0) + ISNULL(SupplierShipping,0))) -			--Supplier Price + Supplier Shipping Price
				(
					( ( ( ISNULL(el.Price,0) + ISNULL(el.Shipping,0) ) * .029 ) + .30 ) +		--PayPal Fee
					( ( ISNULL(el.Price,0) + ISNULL(el.Shipping,0) ) * .0915 ) +				--Ebay Fee 
					( ( ISNULL(SupplierPrice,0) + ISNULL(SupplierShipping,0) ) * .07 ) +		--Avg Tax price from supplier price + shipping price
					( ( ( ISNULL(el.Price,0) + ISNULL(el.Shipping,0) ) * ( ISNULL(el.PromotedListing,0.0) / 100.00) ) )		--Promoted listing
				)			
			ELSE
				( (ISNULL(el.Price,0) + ISNULL(el.Shipping,0) ) -								--Ebay Price + Ebay Shipping
				(ISNULL(SupplierPrice * sg.Quantity,0) + ISNULL(SupplierShipping,0))) -						--Supplier Price + Supplier Shipping Price
				(
					( ( ( ISNULL(el.Price,0) + ISNULL(el.Shipping,0) ) * .029 ) + .30 ) +		--PayPal Fee
					( ( ISNULL(el.Price,0) + ISNULL(el.Shipping,0) ) * .0915 ) +				--Ebay Fee 
					( ( ( ISNULL(el.Price,0) + ISNULL(el.Shipping,0) ) * ( ISNULL(el.PromotedListing,0.0) / 100.00) ) )		--Promoted listing
				)			
		END 'Profit',
		(((ISNULL(SupplierPrice,0) + ISNULL(SupplierShipping,0)) * .07)) 'Tax',
		SupplierShipping,
		IsInStock,
		DispatchTime,
		RetailPrice,
		UpdatedTimeSkuGrid,
		DateAddedSkuGrid,
		SupplierImageUrl,
		Reference,
		sg.CreateDate,
		DateUpdated,
		DatePriceUpdated,
		el.title
	from skugrid sg
		left outer join EbayListings el on el.EbayId = sg.EbayId
		left outer join Supplier s on ( s.SupplierName = sg.SupplierName ) 

END

--select * FROM EbayListings order by Price desc





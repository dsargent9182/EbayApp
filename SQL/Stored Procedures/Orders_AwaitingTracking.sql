USE [Ebay]
GO


CREATE OR ALTER Proc [dbo].[Orders_AwaitingTracking]
/***********************************************************************************\
Name:		[Orders_AwaitingTracking]
Purpose:	Get orders that need their tracking numbers updated on Ebay
Created:	2019.10.01
Creator:	David Sargent
			exec Orders_AwaitingTracking
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/AS
BEGIN

SELECT 
		o.Id,
		o.IsOrdered,
		o.IsNoteInEbay,
		o.ItemId,
		o.TransactionId,
		o.AmountPaid,
		o.BuyerEmail,
		o.PaidStatus,
		o.ListingStatus,
		o.UPC,
		o.BuyerId,
		o.FirstName,
		o.LastName,
		o.Address1,
		o.Address2,
		o.City,
		o.State,
		o.ZipCode,
		o.PhoneNumber,
		o.OrderLineItemId,
		o.Title,
		o.Notes,
		o.HDOrderId,
		o.PaidTime,
		o.DateAdded,
		o.HDInternetOrderId,
		o.IsTrackingUploaded,
		o.IsCanceled,
		o.Quantity,
		o.BestOfferAmount,
		o.Tax,
		o.FullName,
		o.FVF,
		o.StoreId,
		t.Id,
		t.HDOrderId,
		t.HDItemId,
		t.TrackingNumber,
		t.Carrier,
		t.Address,
		t.DateInserted,
		t.SubTotal,
		t.Tax,
		t.Shipping,
		t.Total,
		r.Id,
		r.EbayId,
		ISNULL(r.HDId,s.SupplierId)'HDID',
		r.CreatedDate,
		r.URL,
		ISNULL(r.source,s.SupplierName) 'Source',
		s.Id,
		s.EbayId,
		s.SupplierId,
		s.SupplierName,
		s.EbayPrice,
		s.SupplierPrice,
		s.SupplierShipping,
		s.IsInStock,
		s.DispatchTime,
		s.RetailPrice,
		s.UpdatedTimeSkuGrid,
		s.DateAddedSkuGrid,
		s.SupplierImageUrl,
		s.Reference,
		s.CreateDate,
		s.DateUpdated,
		s.DatePriceUpdated	
	from orders o 
		left join Tracking t on ( o.HDOrderId = t.HDOrderId  )
		left join Repricer r on ( r.EbayId = o.ItemId ) 
		left join SkuGrid s on ( s.EbayId = o.ItemId ) 
	where 1=1
		and o.IsOrdered = 1
		and t.Id is null
		and IsCanceled = 0
		and o.Paidtime > '2018-08-07'
	order by o.id 
/**
select *
	from orders o 
		left join Tracking t on ( o.HDOrderId = t.HDOrderId and o.HDInternetOrderId = t.HDItemId )
		left join Repricer r on ( r.EbayId = o.ItemId ) 
	where 1=1
		and o.IsOrdered = 1
		and t.Id is null
		and o.Paidtime > '2018-08-01'
	order by o.id 
**/
END


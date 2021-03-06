USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[Orders_GetbyTranId]
(
	@Id int
)AS
/***********************************************************************************\
Name:		[Orders_GetbyTranId]
Purpose:	Get orders that need to be ordered from Walmart, Home Depot etc
Created:	2019.10.01
Creator:	David Sargent

			exec [Orders_GetbyTranId] @Id = 3641
			exec [Orders_GetbyTranId] @Id = 3649
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/AS


BEGIN

--If this is walmart join only on Order Id

DECLARE @supplier varchar(20)

select @supplier = SupplierName 
	from orders o 
		inner join Skugrid sg on ( sg.EbayId = o.ItemId )
	where 1=1
		and o.Id = @Id

--If Walmart is the supplier only join on Order Id
IF( @supplier = 'WALMART' )
BEGIN

select 
		o.Id,ISNULL(r.source,s.SupplierName) 'Source',o.IsOrdered,o.IsNoteInEbay,o.ItemId,o.TransactionId,o.AmountPaid,o.BuyerEmail,o.PaidStatus,o.ListingStatus,
		o.UPC,o.BuyerId,o.FirstName,o.LastName,o.Address1,o.Address2,o.City,o.State,o.ZipCode,o.PhoneNumber,o.OrderLineItemId,o.Title,o.Notes,
		o.HDOrderId,o.PaidTime,o.DateAdded,Coalesce(o.HDInternetOrderId,r.HDId,s.SupplierId) 'HDInternetOrderId',o.IsTrackingUploaded,o.IsCanceled,
		o.Quantity,o.BestOfferAmount,o.Tax,o.FullName,o.FVF,t.Id,t.HDOrderId,t.HDItemId,t.TrackingNumber,t.Carrier,t.Address 'CarrierAddress',t.DateInserted,
		t.SubTotal,t.Tax,t.Shipping,t.Total,el.Notes 'ListingNote',ss.[Name] 'StateLongName'
	from orders o
		left outer join Tracking t on ( o.HDOrderId = t.HDOrderId )
		left outer join Repricer r on ( r.EbayId = o.ItemId ) 
		left outer join SkuGrid s on ( s.EbayId = o.ItemId ) 
		left outer join EbayListings el on ( el.EbayId = o.ItemId )
		left outer join States ss on ( ss.Abbreviation = o.[State] )
	where 1=1
		and o.Id = @Id
END ELSE
--Home Depot should join on Order Id and Item Id
BEGIN

select 
		o.Id,ISNULL(r.source,s.SupplierName) 'Source',o.IsOrdered,o.IsNoteInEbay,o.ItemId,o.TransactionId,o.AmountPaid,o.BuyerEmail,o.PaidStatus,o.ListingStatus,
		o.UPC,o.BuyerId,o.FirstName,o.LastName,o.Address1,o.Address2,o.City,o.State,o.ZipCode,o.PhoneNumber,o.OrderLineItemId,o.Title,o.Notes,
		o.HDOrderId,o.PaidTime,o.DateAdded,Coalesce(o.HDInternetOrderId,r.HDId,s.SupplierId) 'HDInternetOrderId',o.IsTrackingUploaded,o.IsCanceled,
		o.Quantity,o.BestOfferAmount,o.Tax,o.FullName,o.FVF,t.Id,t.HDOrderId,t.HDItemId,t.TrackingNumber,t.Carrier,t.Address 'CarrierAddress',t.DateInserted,
		t.SubTotal,t.Tax,t.Shipping,t.Total,el.Notes 'ListingNote',ss.[Name] 'StateLongName'
	from orders o
		left join Tracking t on ( o.HDOrderId = t.HDOrderId and o.HDInternetOrderId = t.HDItemId )
		left outer join Repricer r on ( r.EbayId = o.ItemId ) 
		left outer join SkuGrid s on ( s.EbayId = o.ItemId ) 
		left outer join EbayListings el on ( el.EbayId = o.ItemId )
		left outer join States ss on ( ss.Abbreviation = o.[State] )
	where 1=1
		and o.Id = @Id
END

END


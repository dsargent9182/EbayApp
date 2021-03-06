USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[Orders_Get_All]
/***********************************************************************************\
Name:		[Orders_Get_All]
Purpose:	Get orders that need to be ordered from Walmart, Home Depot etc
			including supplier details
Created:	2019.10.01
Creator:	David Sargent
			exec [Orders_Get_All]
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/AS
BEGIN

select 
	o.title 'Title2',
	o.notes 'Notes2',
	sr.IsTaxExempt,
	ISNULL(r.source,s.SupplierName) 'Source',
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
	Coalesce(o.HDInternetOrderId,r.HDId,s.SupplierId) 'HDInternetOrderId',
	o.IsTrackingUploaded,
	o.IsCanceled,
	o.Quantity,
	o.BestOfferAmount,
	o.Tax,
	o.FullName,
	o.FVF,
	o.StoreId,
	o.SalesRecord,
	el.Notes 'ListingNote'
	from Orders o 
		left outer join repricer r on r.EbayId = o.ItemId
		left outer join SkuGrid s on s.EbayId = o.ItemId
		left outer join StateReseller sr on sr.StateShortName = o.State
		left outer join EbayListings el on el.EbayId = o.ItemId
	where 1=1
		and o.notes is null
		and IsOrdered = 0
		--and UPC not like ('%apply%')
	order by id,PaidTime


END
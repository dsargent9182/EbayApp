USE [Ebay]
GO

CREATE OR ALTER Proc [dbo].[EbayExported_Get]
/***********************************************************************************\
Name:		[EbayExported_Get]
Purpose:	Generate file to import into Ebay to update tracking information
			from email updates
			
Created:	2019.10.01
Creator:	David Sargent


------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/AS
BEGIN
select * into #tmp_20190804_Export from 
(
--Home Depot
--Orders with 2 with the same Order Id
select BuyerId,BuyerEmail,o.Title,Address1,Address2,City,State,ZipCode,T.Carrier 'ShippingCarrierUsed',
		T.TrackingNumber 'ShipmentTrackingNumber', o.ItemId,o.TransactionId,o.PaidTime,ISNULL(el.StoreId,3)'StoreId'
	from orders o
		left outer join EbayExported e on ( e.OrderId = o.Id )
		left outer join Tracking t on ( t.HDOrderId = o.HDOrderId  and t.HDItemId = o.HDInternetOrderId)
		left outer join EbayListings el on ( el.EbayId = o.ItemId )
	where 1=1
		and e.OrderId is null
		and o.IsCanceled != 1
		and o.IsTrackingUploaded = 1 -- Really should be tracking downloaded / scraped from email
		and o.HDOrderId in (select HDOrderId from Orders group by HDOrderId having count(*) = 2 )

union all
--Home Depot 1 
select BuyerId,BuyerEmail,o.Title,Address1,Address2,City,State,ZipCode,T.Carrier 'ShippingCarrierUsed',
		T.TrackingNumber 'ShipmentTrackingNumber', o.ItemId,o.TransactionId,o.PaidTime,ISNULL(el.StoreId,3)'StoreId'
	from orders o
		left outer join EbayExported e on ( e.OrderId = o.Id )
		left outer join Tracking t on ( t.HDOrderId = o.HDOrderId )
		left outer join EbayListings el on ( el.EbayId = o.ItemId )
		left outer join SkuGrid sg on ( sg.EbayId = o.ItemId )
	where 1=1
		and e.OrderId is null
		and o.IsCanceled != 1
		and o.IsTrackingUploaded = 1 -- Really should be tracking downloaded / scraped from email
		and o.HDOrderId not in (select HDOrderId from Orders group by HDOrderId having count(*) = 2 )
		--and ISNULL(sg.SupplierName,'HomeDepot') = 'HomeDepot'

) as q

select * from #tmp_20190804_Export

select 
		'Status' 'Action(SiteID=US|Country=US|Currency=USD|Version=585|CC=UTF-8)',
		'' 'OrderId',
		ItemId,
		TransactionId,
		'1' 'ShippingStatus',
		ShippingCarrierUsed 'ShippingCarrierUsed',
		ShipmentTrackingNumber 'ShipmentTrackingNumber',
		StoreId
	from #tmp_20190804_Export
	where 1=1
	order by PaidTime desc



Insert into EbayExported(OrderId,ExportedDate)
select o.Id,Getdate()
	from orders o
		left outer join EbayExported e on ( e.OrderId = o.Id )
		left outer join Tracking t on ( t.HDOrderId = o.HDOrderId )
	where 1=1
		and e.OrderId is null
		and o.IsCanceled != 1
		and o.IsTrackingUploaded = 1 -- Really should be tracking downloaded / scraped from email
	order by PaidTime desc


END



--select top 10 * from EbayExported order by id desc



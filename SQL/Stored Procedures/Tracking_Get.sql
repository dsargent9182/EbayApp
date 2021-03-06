USE [Ebay]
GO

CREATE OR ALTER Proc [dbo].Tracking_Get
/***********************************************************************************\
Name:		[Tracking_Get]
Purpose:	Create or Update tracking information including totals for shipping,
			tax and Supplier Ids
			
Created:	2019.10.01
Creator:	David Sargent

			exec Tracking_Get
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.21	DS	Created

\***********************************************************************************/ AS
BEGIN
select 
		T.TrackingNumber 'TrackingT',* 
	from Orders o
		--inner join Tracking t on (o.HDOrderId = t.HDOrderId AND t.HDItemId = o.HDInternetOrderId) For HomeDepot
		inner join Tracking t on (o.HDOrderId = t.HDOrderId )
	where 1=1
		and o.IsTrackingUploaded = 0

END
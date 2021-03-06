USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[Orders_Get_Shipped]
(
	@date datetime = null,
	@months int = 6
)AS
/***********************************************************************************\
Name:		[Orders_Get_Shipped]
Purpose:	Get orders that have been shipped with tracking numbers updated in Ebay
			and that have not been canceled
Created:	2019.10.01
Creator:	David Sargent
			exec [Orders_Get_Shipped] @date = '2019-10-01'
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/
BEGIN

IF( @date is null )
BEGIN
	select @date = GETDATE()
END


SELECT o.*,t.TrackingNumber 'TrackNumber',t.Carrier
	FROM Orders o
		left outer join Tracking t on ( o.HDOrderId = t.HDOrderId and t.HDItemId = o.HDInternetOrderId )
	WHERE 1=1
		AND (
				( IsOrdered = 1 and IsTrackingUploaded = 1 )
				OR IsCanceled = 1
			)
		AND PaidTime > DATEADD(month,-@months,@date) 
	order by PaidTime

END



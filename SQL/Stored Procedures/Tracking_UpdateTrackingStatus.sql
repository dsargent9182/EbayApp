USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[Tracking_UpdateTrackingStatus]
(
	@TransactionId		varchar(20),
	@ItemId				varchar(20)
)AS
/***********************************************************************************\
Name:		[Tracking_UpdateTrackingStatus]
Purpose:	Update tracking status to uploaded
			
Created:	2019.10.01
Creator:	David Sargent

------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.21	DS	Created

\***********************************************************************************/

BEGIN

Update Orders
	set IsTrackingUploaded = 1
	where 1=1
		and TransactionId = @TransactionId
		and ItemId = @ItemId

END
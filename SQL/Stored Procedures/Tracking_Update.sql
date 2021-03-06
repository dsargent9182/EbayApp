USE [Ebay]
GO

CREATE OR ALTER Proc [dbo].[Tracking_Update]
(
	@HDOrderId				varchar(30),
	@HDItemId				varchar(20) = null, ---This is required for Home Depot but not for Walmart
	@TrackingNumber			varchar(30),
	@Carrier				varchar(20),
	@Address				varchar(60) = null,
	@Shipping				money,
	@Tax					money,
	@SubTotal				money,
	@Total					money,
	@IsWalmart				bit = 1
)AS
/***********************************************************************************\
Name:		[Tracking_Update]
Purpose:	Create or Update tracking information including totals for shipping,
			tax and Supplier Ids
			
Created:	2019.10.01
Creator:	David Sargent

			exec Tracking_Update
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/
BEGIN
DECLARE @count int = 0

IF(@IsWalmart = 1)		BEGIN
	select @count = count(*) 
		from Tracking 
		where 1=1
			and HDOrderId = @HDOrderId


	IF(@count < 1)		BEGIN
		Insert into Tracking(HDOrderId,HDItemId,TrackingNumber,Carrier,[Address],DateInserted,Shipping,Total,Tax,SubTotal)
			select @HDOrderId,@HDItemId,@TrackingNumber,@Carrier,@Address,getdate(),@Shipping,@Total,@Tax,@SubTotal

	END

	ELSE	BEGIN
		UPDATE Tracking
			set Total = @Total,
				Shipping = @Shipping,
				Tax = @Tax,
				SubTotal = @SubTotal
			WHERE 1=1
				and HDOrderId = @HDOrderId
	END

END


ELSE		BEGIN
select @count = count(*) 
	from Tracking 
	where 1=1
		and HDOrderId = @HDOrderId
		and HDItemId = @HDItemId

IF(@count < 1)		BEGIN
Insert into Tracking(HDOrderId,HDItemId,TrackingNumber,Carrier,[Address],DateInserted,Shipping,Total,Tax,SubTotal)
	select @HDOrderId,@HDItemId,@TrackingNumber,@Carrier,@Address,getdate(),@Shipping,@Total,@Tax,@SubTotal

END

ELSE	BEGIN
	UPDATE Tracking
		set Total = @Total,
			Shipping = @Shipping,
			Tax = @Tax,
			SubTotal = @SubTotal
		WHERE 1=1
			and HDOrderId = @HDOrderId
			and HDItemId = @HDItemId 
END


END

END



USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[Orders_Update]
(
		@ItemId					varchar(20),
		@TransactionId			varchar(20),
		@HDOrderId				varchar(30)		=	null,
		--@TrackingNumber			varchar(30)		=	null,
		--@TrackingCompany		varchar(30)		=	null,
		@Title					varchar(80),
		@Notes					varchar(100)	=	null,
		@AmountPaid				float,
		@BuyerEmail				varchar(100),
		@PaidStatus				varchar(20),
		@ListingStatus			varchar(20)		=	null,
		@UPC					varchar(30),
		@BuyerId				varchar(30),
		@FirstName				varchar(20),
		@LastName				varchar(30),
		@Address1				varchar(100),
		@Address2				varchar(100),
		@City					varchar(20),
		@State					varchar(20),
		@ZipCode				varchar(15),
		@PhoneNumber			varchar(30),
		@OrderLineItemId		varchar(30),
		@PaidTime				Datetime,
		@IsOrdered				bit = 0,
		@Quantity				int = 1,
		@BestOfferAmount		money = null,
		@Tax					money = null,
		@FullName				varchar(500) = null,
		@FVF					money = null,
		@StoreId				int = null,
		@SalesRecord			int = null
)AS
/***********************************************************************************\
Name:		[Orders_Update]
Purpose:	Insert or update order
Created:	2019.10.01
Creator:	David Sargent
			
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/
BEGIN

Declare @count int
DECLARE @IsCanceled bit

select @count = count(*) from orders where 1=1 and TransactionId = @TransactionId
select @IsCanceled = IsCanceled from orders where 1=1 and TransactionId = @TransactionId

/*
IF(@ItemId = '202235722167') BEGIN
	select @UPC = '203066067'
END
ELSE IF(@ItemId = '202236331686' ) BEGIN
	select @UPC = '202654978'
END
ELSE IF(@ItemId = '202238149711' ) BEGIN
	select @UPC = '100553490'
END
ELSE IF(@ItemId = '202313894033' ) BEGIN
	select @UPC = '1000331252'
END

--FiberGlass Claw Hammer
ELSE IF(@ItemId = '202293896943' ) BEGIN
	select @UPC = '205386272'
END

--Flushmount Light
ELSE IF(@ItemId = '202233101737' ) BEGIN
	select @UPC = '202786700'
END
*/

DECLARE @HDId varchar(30)
select @HDId = SupplierId from Skugrid where EbayId = @ItemId

IF(@count = 0) Begin

Insert into Orders(ItemId,TransactionId,HDOrderId,Title,Notes,BuyerEmail,PaidStatus,ListingStatus,UPC,BuyerId,FirstName,LastName,Address1,
Address2,City,State,ZipCode,PhoneNumber,OrderLineItemId,PaidTime,AmountPaid,IsOrdered,Dateadded,Quantity,BestOfferAmount,Tax,FullName,FVF,StoreId,SalesRecord)

	select @ItemId,@TransactionId,@HDOrderId,@Title,@Notes,@BuyerEmail,@PaidStatus,@ListingStatus,@UPC,@BuyerId,@FirstName,@LastName,@Address1,
			@Address2,@City,@State,@ZipCode,@PhoneNumber,@OrderLineItemId,@PaidTime,@AmountPaid,@IsOrdered,Getdate(),@Quantity,@BestOfferAmount,@Tax,@FullName,@FVF,@StoreId,@SalesRecord


update Orders
	set HDInternetOrderId = @HDId
	where 1=1
		and TransactionId = @TransactionId
		and ItemId = @ItemId

End

ELSE IF ( @IsCanceled = 0 ) 

BEGIN
Update orders 
	set Notes = @Notes
	where 1=1
		and TransactionId = @TransactionId
END

END



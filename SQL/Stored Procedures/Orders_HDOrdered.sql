USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[Orders_HDOrdered]
(
	@HdOrderId				varchar(30),
	@HDInternetOrderId		varchar(20) = null,
	@IsError				bit = 0,
	@IsCanceled				bit = 0,
	@Id						int
)AS
/***********************************************************************************\
Name:		[Orders_HDOrdered]
Purpose:	Update the status of an order if it is ordered,canceled or waiting
			for the itme to come in stock
Created:	2019.10.01
Creator:	David Sargent
			
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/
BEGIN

	IF(@IsError = 1 AND @IsCanceled = 0)		BEGIN
		Update Orders
		set 
			--Notes = 'OutOfStock'
			Notes = @HdOrderId
		where 1=1
			--and TransactionId = @TransactionId
			and Id = @Id

	END

	ELSE IF(@IsError = 1 AND @IsCanceled = 1)		BEGIN
		Update Orders
		set 
			Notes = @HdOrderId
			,IsCanceled = 1
		where 1=1
			--and TransactionId = @TransactionId
			and Id = @Id

	END

	ELSE	BEGIN
		Update Orders
		set 
			HDOrderId = @HdOrderId,
			IsOrdered = 1,
			Notes = @HdOrderId,
			OrderedDate = GETDATE()
			--,HDInternetOrderId = @HDInternetOrderId
		where 1=1
			--and TransactionId = @TransactionId
			and Id = @Id

	END
	
	
END
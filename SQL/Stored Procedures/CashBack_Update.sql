USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[CashBack_Update]
(
	@OrderDate				Datetime,
	@LockDate				Datetime,
	@Store					varchar(100),
	@OrderId				varchar(100),
	@Amount					money,
	@Website				varchar(100) = 'BeFrugal'
)AS
/***********************************************************************************\
Name:		[CashBack_Update]
Purpose:	Update Cash back received from BeFrugal
			
Created:	2019.10.01
Creator:	David Sargent


------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/
BEGIN

IF NOT EXISTS(select * from CashBack where OrderId = @OrderId)
BEGIN
	Insert into CashBack(OrderDate,LockDate,Store,OrderId,Amount,Website)
		select @OrderDate,@LockDate,@Store,@OrderId,@Amount,@Website
END

END
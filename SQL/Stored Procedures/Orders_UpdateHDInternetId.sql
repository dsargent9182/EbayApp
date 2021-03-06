USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[Orders_UpdateHDInternetId]
(
	@HDInternetItemId		varchar(20),
	@UPC					varchar(30)
)AS
/***********************************************************************************\
Name:		[Orders_UpdateHDInternetId]
Purpose:	Update orders with UPC codes
Created:	2019.10.01
Creator:	David Sargent
			
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/
BEGIN

update
	Orders
	set HDInternetOrderId = @HDInternetItemId
	where 1=1
		and UPC = @UPC

END
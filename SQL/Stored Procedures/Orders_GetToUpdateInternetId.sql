USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[Orders_GetToUpdateInternetId]
/***********************************************************************************\
Name:		[Orders_GetToUpdateInternetId]
Purpose:	Get order details where the HDInternetOrderId field is null
Created:	2019.10.01
Creator:	David Sargent

			exec [Orders_GetToUpdateInternetId]
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/ AS
BEGIN

select *
	from Orders
	where 1=1
		and HDInternetOrderId is null
		and UPC not like ('%apply%')

END
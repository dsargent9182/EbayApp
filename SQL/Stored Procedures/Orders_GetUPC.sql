USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[Orders_GetUPC]
/***********************************************************************************\
Name:		[Orders_GetUPC]
Purpose:	Get orders that were paid within the last 21 days
Created:	2019.10.01
Creator:	David Sargent
			exec [Orders_GetUPC]
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/AS
BEGIN

select ItemId,MAX(UPC)'UPC'
	from Orders
	group by itemid
	order by count(*) desc

END
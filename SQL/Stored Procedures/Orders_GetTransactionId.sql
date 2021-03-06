USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[Orders_GetTransactionId]
/***********************************************************************************\
Name:		[Orders_GetTransactionId]
Purpose:	Get orders that were paid within the last 21 days
Created:	2019.10.01
Creator:	David Sargent
			exec [Orders_GetTransactionId]
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/AS
BEGIN

select TransactionId,Title 
	from orders
	where 1=1
		and PaidTime > DATEADD(day,-21,GETDATE())
END
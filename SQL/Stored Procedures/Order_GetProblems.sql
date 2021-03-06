USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[Order_GetProblems]
/***********************************************************************************\
Name:		[Order_GetProblems]
Purpose:	Get orders that have problems
Created:	2019.10.01
Creator:	David Sargent
			exec [Order_GetProblems]
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/AS
BEGIN

Select notes 'Notes1',* 
	from Orders
	where 1 = 1 
		and IsOrdered = 0
		and iscanceled = 0
		and notes is not null
	order by PaidTime 

END



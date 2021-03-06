USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[Purchase_Get]
(
	@EbayUser				varchar(50),
	@Days					int  = 30
)AS
/***********************************************************************************\
Name:		[Purchase_Get]
Purpose:	Get purchases from an ebay user name
Created:	2019.10.01
Creator:	David Sargent
			
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/
BEGIN

select *
	from Purchase
	where 1=1
		and EbayUser = @EbayUser

END
USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[RepricerADD]
(
	@EbayId varchar(30),
	@HDId	varchar(30)
)AS 
/***********************************************************************************\
Name:		[RepricerADD]
Purpose:	Add repricer record if it doesn't exist already

Created:	2019.10.01
Creator:	David Sargent
			
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/
BEGIN

Declare @i int
Select @i = 0

Select @i = count(*) from Repricer where 1=1 and EbayId = @EbayId and HDId = @HDId

IF(@i = 0 ) BEGIN

Insert into Repricer(EbayId,HdId)
	select @EbayId, @HDId

END

END
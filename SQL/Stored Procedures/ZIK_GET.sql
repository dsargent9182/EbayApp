USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[ZIK_GET]
(
	@IsWalmartScrape bit = 0 
)AS
/***********************************************************************************\
Name:		[ZIK_GET]
Purpose:	Get items scraped from Zik
			
Created:	2019.10.01
Creator:	David Sargent
			exec [ZIK_GET] @IsWalmartScrape = 1
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.21	DS	Created

\***********************************************************************************/


BEGIN


IF( @IsWalmartScrape = 0 )
BEGIN
	select top 50 *
		from zik
		where 1=1
			and UPC is null
END

ELSE	BEGIN

select top 25 *
	from zik
	where 1=1
		--and UPC is not null
		and SourceURL is null
		AND id > 709
END

END


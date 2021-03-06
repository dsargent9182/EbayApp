USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[SoldItems_GetByUser]
(
	@EbayUser				varchar(50),
	@Days					int  = 30
)AS
/***********************************************************************************\
Name:		[SoldItems_GetByUser]
Purpose:	Insert/update record into Sold Items table
			
			
Created:	2019.10.21
Creator:	David Sargent

			exec SoldItems_GetByUser @EbayUser = 'bf-market'		
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.21	DS	Created

\***********************************************************************************/

BEGIN

DECLARE @end datetime, @start datetime

select @end = convert(date,getdate())

select @start = convert(date,@end - 30)

print @start
print @end


select *
	from SoldItems 
	where 1=1
		and EbayUser = @EbayUser 
		and LastDateSold >= @start
		and LastDateSold > ISNULL(LastDetailRun,'1900-01-01')
		and ISNULL(Convert(date,LastDetailRun),'1900-01-01') < Convert(date,GETDATE())  --Update once a day only
	order by LastDateSold desc
END



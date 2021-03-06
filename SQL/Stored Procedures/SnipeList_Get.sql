USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[SnipeList_Get]
(
	@EbayUser			varchar(50)			= null,
	@Days				int					= null,
	@end				datetime			= null --'2019-10-01'
) AS
/***********************************************************************************\
Name:		[SnipeList_Get]
Purpose:	Get a listing of items from stores that have recently sold items
			
Created:	2019.08.01
Creator:	David Sargent

			exec [SnipeList_Get] @end = '2019-10-01'
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.08.01	DS	Created

\***********************************************************************************/


/*
greathomesales
omgstore955
exec SnipeList_Get @EbayUser = 'tiashop'
--2019-09-03 05:56:00.000
*/
BEGIN

DECLARE @start datetime --@end datetime, 

IF (@end IS NULL)
BEGIN
	select @end = convert(date,getdate())
END

IF(@Days is null)
BEGIN
	SET @Days = 30
END

select @start = convert(date,@end - @Days)

print @end
print @start

select 
		EbayId,
		Title,
		count(*) 'SoldCount',
		sum(p.Quantity) 'TotalSold',
		sum(p.Price) 'SoldAmount',
		max(p.Price) 'Price',
		max(p.EbayUser) 'EbayUser',
		max(si.LastDateSold) 'LastDateSold'
	from SoldItems si
		left outer join Purchase p on si.EbayId = p.ItemId
	where 1=1
		and si.EbayUser = ISNULL(@EbayUser,p.EbayUser)
		and  p.DateOfPurchase >= @start
	group by si.EbayId,si.Title
	having sum(p.Quantity) is not null
	
	order by count(*) desc

END
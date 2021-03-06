USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[SoldItems_Get]
(
	@Days		int				= 30,
	@start		datetime		= null,
	@end		datetime		= null
)AS
/***********************************************************************************\
Name:		[SoldItems_Get]
Purpose:	Get a summary of ebay sellers with a listing of total sales and total
			unique orders
			
			
Created:	2019.10.21
Creator:	David Sargent

			exec [SoldItems_Get] @end = '2019-10-01'
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.21	DS	Created

\***********************************************************************************/

BEGIN

IF( @end is null )
BEGIN
	select @end = convert(date,getdate())
END

IF( @start is null )
BEGIN
	select @start = convert(date,@end - 30)
END

print @end
print @start


select 
		EbayUser,
		Sum(Quantity) 'Quantity',
		Sum(Price) 'TotalSold'
			into #tmp_20190717_Purchase
		from Purchase
		where 1=1
			and DateOfPurchase >= @start
			and DateOfPurchase <= @end
		group by EbayUser


--select * from #tmp_20190717_Purchase


select 
		s.EbayUser,
		Count(*)						'UniqueItemsSoldCount',
		ISNULL(MAX(p.Quantity),0.0)		'TotalQuantity',
		ISNULL(MAX(p.TotalSold),0.00)	'TotalAmount'
			into #tmp_20190920_Final
		from SoldItems s
			left outer join #tmp_20190717_Purchase p on ( p.EbayUser = s.EbayUser )
		where 1=1
			and LastDateSold >= @start
		group by s.EbayUser
		order by Count(*) desc


select * from #tmp_20190920_Final order by TotalAmount desc


drop Table #tmp_20190717_Purchase
drop Table #tmp_20190920_Final
END

--select top 10 * from Purchase

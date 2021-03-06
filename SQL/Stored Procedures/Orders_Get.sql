USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[Orders_Get]
/***********************************************************************************\
Name:		[Orders_Get]
Purpose:	Get orders that need to be ordered from Walmart, Home Depot etc
Created:	2019.10.01
Creator:	David Sargent
			exec [Orders_Get]
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/AS
BEGIN

select top 20 r.source,r.HDId'SourceId',title 'Title2',notes 'Notes2',o.*
	into #tmpOrders
	from Orders o 
		left join Repricer r on o.ItemId = r.EbayId
	where 1=1
		and notes is null
		and IsOrdered = 0
		--and UPC not like ('%apply%')
	order by PaidTime


select * into #tmpResults from( 
select * 
	from #tmpOrders
	where 1=1
		and ItemId not in (select ItemId from #tmpOrders group by ItemId having count(*) > 1)
union
select * 
	from #tmpOrders
	where 1=1
		and TransactionId in (
			select min(transactionId)
				from #tmpOrders
				group by ItemId
				having count(*) > 1	
		)
)as q


select top 10 * from #tmpResults order by PaidTime


drop table #tmpOrders
END

USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[Orders_Profit_Get]
(
	@date datetime = null
)AS
/***********************************************************************************\
Name:		[Orders_Profit_Get]
Purpose:	Used in graph for admin dashboard for profit
			
Created:	2019.10.25
Creator:	David Sargent

			exec Orders_Profit_Get @date = '2019-09-30'
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.25	DS	Created
2021.12.18	DS	Add @date parameter

\***********************************************************************************/
BEGIN

IF(@date is null) BEGIN
	select @date = GETDATE()
END


select	o.HDInternetOrderId,
		o.HDOrderId,
		o.AmountPaid * o.Quantity 'T1',
		SUM((o.AmountPaid * o.Quantity)) OVER (Partition by o.HDOrderId ORDER by cb.id desc) 'Total',
		cb.Amount
		into #tmp_20190804_CashBack
	from CashBack cb
		inner join Orders o on o.HDOrderId = cb.OrderId
	where 1=1
		and o.HDOrderId in (select HDOrderId from Orders group by HDOrderId having count(*) = 2)
		and o.PaidTime > '2019-01-01'
	order by cb.id desc


--select * from #tmp_20190804_CashBack

select HDOrderId,HDInternetOrderId,Round(convert(money,(t1/Total) * Amount),2) 'Amount',Amount 'TotalAmount' into #tmp_20190804_CashBackFinal from #tmp_20190804_CashBack

--select * from #tmp_20190804_CashBackFinal


select * 
into #tmpTotals from (

--Any Orders with 2 from the same order
--HD orders with 2

select 
		ROW_NUMBER() OVER (ORDER by o.PaidTime desc) 'Number',
		t.Total,
		ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00)  'EbayPrice',
		( (ISNULL(o.AmountPaid,0.00) * .029 * ISNULL(o.Quantity,0.00)) + .30) 'PayPal Fees',
		ROUND( ( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) ) - ( ( (ISNULL(o.AmountPaid,0.00) * .029) * ISNULL(o.Quantity,0.00) ) + .30),2) 'NetPP',
		o.Quantity,
		( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) )- ISNULL(t.Total,0.00) 'GrossProfit',
		(ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .0915)  'EbayFVF',
		(ISNULL(o.AmountPaid,0.00) * .01 * ISNULL(o.Quantity,0.00))  'EbayPLF',
		
		(ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .1015) + ( (ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .029) + .30) 'Fees' ,
		COALESCE(c.Amount,/*t.Total * .03,*/0.00)  'FrontEndProfit',
		( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) - ISNULL(t.Total,0.00) )  - ( ( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .1015 ) + ( (ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .029) + .30 ) ) + ISNULL(pm.Amount,0) 'NetProfit',
		--( (ISNULL(o.AmountPaid,0.00) * .1115) + ( (ISNULL(o.AmountPaid,0.00) * .029) + .30) ) 'NetProfit' ,
		( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) - ISNULL(t.Total,0.00) )  - ( ( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .1015 ) + ( (ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .029) + .30 ) ) +  COALESCE(c.Amount,/*t.Total * .03,*/0.00) + ISNULL(pm.Amount,0) 'TotalProfit',
		o.Title,
		o.PaidTime,
		o.Id,
		t.Total 'HDPrice',
		o.BestOfferAmount,
		o.HDOrderId,
		o.UPC,
		o.TransactionId,
		ISNULL(c.Amount,0.00) 'CashBack',
		case when c.Amount is not null then 1 else 0 end 'IsCashBack' ,
		ISNULL(t.Tax,0.00) 'Tax'
	from  orders o
		left outer join tracking t on o.HDInternetOrderId = t.HDItemId and o.HDOrderId = t.HDOrderId
		--left outer join tracking t on o.HDOrderId = t.HDOrderId
		left outer join #tmp_20190804_CashBackFinal c on ( o.HDOrderId = c.HDOrderId and o.HDInternetOrderId = c.HDInternetOrderId )
		left outer join PriceMatch pm on ( pm.SupplierId = o.HDOrderId and pm.HdItemId = o.HDInternetOrderId )
	where 1=1
		and o.IsOrdered = 1
		and o.IsCanceled = 0
		and t.Total is not null
		and o.PaidTime > '2018-08-06'
		and O.HDOrderId  in (select HDOrderId from Orders where HDOrderId is not null group by HDOrderId having count(*) = 2)
union 
--Home Depot Orders with only 1 item in the order
select 
		ROW_NUMBER() OVER (ORDER by o.PaidTime desc) 'Number',
		t.Total,
		ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00)  'EbayPrice',
		( (ISNULL(o.AmountPaid,0.00) * .029 * ISNULL(o.Quantity,0.00)) + .30) 'PayPal Fees',
		ROUND( ( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) ) - ( ( (ISNULL(o.AmountPaid,0.00) * .029) * ISNULL(o.Quantity,0.00) ) + .30),2) 'NetPP',
		o.Quantity,
		( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) )- ISNULL(t.Total,0.00) 'GrossProfit',
		(ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .0915)  'EbayFVF',
		(ISNULL(o.AmountPaid,0.00) * .01 * ISNULL(o.Quantity,0.00))  'EbayPLF',
		
		(ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .1015) + ( (ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .029) + .30) 'Fees' ,
		COALESCE(c.Amount,/*t.Total * .03,*/0.00)  'FrontEndProfit',
		( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) - ISNULL(t.Total,0.00) )  - ( ( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .1015 ) + ( (ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .029) + .30 ) ) + ISNULL(pm.Amount,0) 'NetProfit',
		--( (ISNULL(o.AmountPaid,0.00) * .1115) + ( (ISNULL(o.AmountPaid,0.00) * .029) + .30) ) 'NetProfit' ,
		( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) - ISNULL(t.Total,0.00) )  - ( ( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .1015 ) + ( (ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .029) + .30 ) ) +  COALESCE(c.Amount,/*t.Total * .03,*/0.00) + ISNULL(pm.Amount,0) 'TotalProfit',
		o.Title,
		o.PaidTime,
		o.Id,
		t.Total 'HDPrice',
		o.BestOfferAmount,
		o.HDOrderId,
		o.UPC,
		o.TransactionId,
		ISNULL(c.Amount,0.00) 'CashBack',
		case when c.Amount is not null then 1 else 0 end 'IsCashBack' ,
		ISNULL(t.Tax,0.00) 'Tax'
	from  orders o
		left outer join tracking t on o.HDInternetOrderId = t.HDItemId and o.HDOrderId = t.HDOrderId
		left outer join PriceMatch pm on ( pm.SupplierId = o.HDOrderId and pm.HdItemId = o.HDInternetOrderId )
		left outer join CashBack c on o.HDOrderId = c.OrderId
	where 1=1
		and o.IsOrdered = 1
		and t.Total is not null
		and o.IsCanceled = 0
		and o.PaidTime > '2018-08-06'
		and O.HDOrderId  in (select HDOrderId from Orders where HDOrderId is not null group by HDOrderId having count(*) = 1)
union 
--Walmart Orders
select 
		ROW_NUMBER() OVER (ORDER by o.PaidTime desc) 'Number',
		t.Total,
		ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00)  'EbayPrice',
		( (ISNULL(o.AmountPaid,0.00) * .029 * ISNULL(o.Quantity,0.00)) + .30) 'PayPal Fees',
		ROUND( ( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) ) - ( ( (ISNULL(o.AmountPaid,0.00) * .029) * ISNULL(o.Quantity,0.00) ) + .30),2) 'NetPP',
		o.Quantity,
		( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) )- ISNULL(t.Total,0.00) 'GrossProfit',
		(ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .0915)  'EbayFVF',
		(ISNULL(o.AmountPaid,0.00) * .01 * ISNULL(o.Quantity,0.00))  'EbayPLF',
		
		(ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .1015) + ( (ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .029) + .30) 'Fees' ,
		COALESCE(c.Amount,/*t.Total * .03,*/0.00)  'FrontEndProfit',
		( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) - ISNULL(t.Total,0.00) )  - ( ( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .1015 ) + ( (ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .029) + .30 ) ) 'NetProfit',
		--( (ISNULL(o.AmountPaid,0.00) * .1115) + ( (ISNULL(o.AmountPaid,0.00) * .029) + .30) ) 'NetProfit' ,
		( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) - ISNULL(t.Total,0.00) )  - ( ( ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .1015 ) + ( (ISNULL(o.AmountPaid,0.00) * ISNULL(o.Quantity,0.00) * .029) + .30 ) ) +  COALESCE(c.Amount,/*t.Total * .03,*/0.00) 'TotalProfit',
		o.Title,
		o.PaidTime,
		o.Id,
		t.Total 'HDPrice',
		o.BestOfferAmount,
		o.HDOrderId,
		o.UPC,
		o.TransactionId,
		ISNULL(c.Amount,0.00) 'CashBack',
		case when c.Amount is not null then 1 else 0 end 'IsCashBack' ,
		ISNULL(t.Tax,0.00) 'Tax'
	from  orders o
		--left outer join tracking t on o.HDInternetOrderId = t.HDItemId and o.HDOrderId = t.HDOrderId
		left outer join tracking t on o.HDOrderId = t.HDOrderId
		left outer join CashBack c on o.HDOrderId = c.OrderId
		left outer join SkuGrid sg on sg.EbayId = o.ItemId
	where 1=1
		and o.IsOrdered = 1
		--and o.AmountPaid is not null
		and o.IsCanceled = 0
		and t.Total is not null
		and o.PaidTime > '2018-08-06'
		and ISNULL(sg.SupplierName,'WALMART') = 'WALMART'

)as q






select t.*,(ISNULL(o.SalesRecord,o.Id))'SalesRecord'
	from #tmpTotals t
		left join Orders o on t.Id = o.Id
	order by o.Id desc

--return;

select	sum([NetProfit])'FrontEndProfit',
		sum(CashBack) 'BackendProfit',
		sum([NetProfit]) + (sum(CashBack)) 'TotalProfit',
		DATEPart(mm,PaidTime)'Month',
		DATEPart(yy,PaidTime)'Year',
		sum(EbayPrice)'EbaySalesTotal',
		sum(HDPrice)'HomeDepotSalesTotal',
		count(TransactionId) 'Order totals',
		sum([NetProfit]) + (sum(CashBack)) + ( sum(HDPrice) * .06 ) 'ProfitWith6%CB',
		sum([NetProfit]) + (sum(CashBack)) +(sum(HDPrice) * .10) 'ProfitWith10%CB'
	from #tmpTotals
	group by DATEPart(yy,PaidTime),DATEPart(mm,PaidTime)
	order by DATEPart(yy,PaidTime) desc,DATEPart(mm,PaidTime) desc

--week
select 
		top 7
		ROUND(sum([NetProfit]) + (sum(CashBack)),2) 'TotalProfit',
		Round(sum([NetProfit]),2)'FrontEndProfit',
		Round(sum(CashBack),2) 'BackendProfit',
		convert(date,max(PaidTime)) 'Date'
		/*
		sum([NetProfit])'FrontEndProfit',
		sum(CashBack) 'BackendProfit',
		sum(EbayPrice)'EbaySalesTotal',
		sum(HDPrice)'HomeDepotSalesTotal',
		count(*)'OrderCount',
		DATEPart(mm,PaidTime)'Month',
		DATEPart(DD,PaidTime)'Day',
		DATEPart(yy,PaidTime)'Year',
		(sum(Hdprice) * .06 ) + sum(NetProfit) + sum(Cashback) 'ProfitWith6%CB',
		(sum(Hdprice) * .1 ) + sum(NetProfit) + sum(Cashback) 'ProfitWith10%CB'
		*/
		into #tmpTotalPerWeek
	from #tmpTotals
	where 1=1
		and PaidTime > @date - 30
	group by DATEPart(yy,PaidTime),DATEPart(mm,PaidTime),DATEPart(DD,PaidTime)
	order by DATEPart(yy,PaidTime) desc,DATEPart(mm,PaidTime) desc,DATEPart(DD,PaidTime) desc

select * from #tmpTotalPerWeek order by [Date] 

--Month
select 
		sum([NetProfit]) + (sum(CashBack)) 'TotalProfit',
		sum([NetProfit])'FrontEndProfit',
		sum(CashBack) 'BackendProfit',
		sum(EbayPrice)'EbaySalesTotal',
		sum(HDPrice)'HomeDepotSalesTotal',
		count(*)'OrderCount',
		DATEPart(mm,PaidTime)'Month',
		DATEPart(DD,PaidTime)'Day',
		DATEPart(yy,PaidTime)'Year',
		
		(sum(Hdprice) * .06 ) + sum(NetProfit) + sum(Cashback) 'ProfitWith6%CB',
		(sum(Hdprice) * .1 ) + sum(NetProfit) + sum(Cashback) 'ProfitWith10%CB'
		
	from #tmpTotals
	where 1=1
		and PaidTime > @date - 30
	group by DATEPart(yy,PaidTime),DATEPart(mm,PaidTime),DATEPart(DD,PaidTime)
	order by DATEPart(yy,PaidTime) desc,DATEPart(mm,PaidTime) desc,DATEPart(DD,PaidTime) desc



DECLARE @ordersToProcess int, @awaitingTracking int,@orderMTD int
DECLARE @today datetime, @firstDay datetime

select @today = convert(date,@date)

select @firstDay = Dateadd(day,1,EOMONTH(DateAdd(month,-1,@date)))


--select @today,@firstDay

select @orderMTD =count(*) from orders where PaidTime >= @firstDay and PaidTime <= @today

--'OrdersToProcess'
select @ordersToProcess = count(*) from Orders where 1=1 and IsOrdered = 0 and IsNoteInEbay = 0

--Awaiting Tracking
select @awaitingTracking = count(*) from Orders where 1=1 and IsTrackingUploaded = 0 and IsCanceled = 0 and IsOrdered = 1


Declare @totalProfit money
-- TotalProfit
select @totalProfit = Round( (sum(NetProfit) + sum(CashBack)) ,2) 
	from #tmpTotals t
		inner join Calendar c on c.[Date] = t.PaidTime
	where 1=1
		and MMYYYY = ( select MMYYYY from Calendar where 1=1 and [Date] = convert(date,@date) )


select @ordersToProcess 'OrdersToProcessCount',@awaitingTracking 'AwaitingTrackingCount',@orderMTD 'OrdersMTD',@totalProfit 'ProfitMTD'

drop table #tmpTotals
END
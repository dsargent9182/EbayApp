USE [Ebay]
GO

CREATE OR ALTER Proc [dbo].[GiftCard_Get]
(
	@Id int = null
)AS
/***********************************************************************************\
Name:		[GiftCard_Get]
Purpose:	Get Available Gift Cards including current balance
			
Created:	2019.10.01
Creator:	David Sargent

			exec GiftCard_Get
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/


BEGIN

IF( ISNULL(@Id,0) = 0 ) 
BEGIN


select * from (

	select Id,Store,Number,PIN,Amount,Balance,CreatedDate,DateM
		from GiftCard
		where 1=1
			--and Balance != 0
			and Id = ISNULL(@Id,Id)
			and ( CreatedDate > getdate() - 5 or
				Balance != 0 )
	union all
	select 0,Max(Store),'Total',null,0,Sum(Balance),GETDATE(),null
		from GiftCard
	group by Store

) as q
order by q.Store,q.Balance,q.CreatedDate

END ELSE
BEGIN

select Id,Store,Number,PIN,Amount,Balance,CreatedDate,DateM
		from GiftCard
		where 1=1
			--and Balance != 0
			and Id = ISNULL(@Id,Id)

END


END

sp_help giftcard
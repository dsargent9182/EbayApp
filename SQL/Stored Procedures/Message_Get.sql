USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[Message_Get]
(
	@ItemId varchar(20),
	@BuyerUserName varchar(100)
)AS
/***********************************************************************************\
Name:		[Message_Get]
Purpose:	Retrieve message from Ebay buyers
Created:	2019.10.01
Creator:	David Sargent

exec Message_Get @ItemId = '183564343560', @BuyerUserName = 'hahahaitsmine'
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/


BEGIN

select * 
	from Message
	where 1=1
		and ItemId = @ItemId
		and BuyerUserName = @BuyerUserName
	order by Received asc

END

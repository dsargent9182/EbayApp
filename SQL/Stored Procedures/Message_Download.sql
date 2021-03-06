USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[Message_Download]
(
	@StoreName varchar(30)	,
	@BuyerUserName varchar(100),
	@Subject varchar(500),
	@ItemId varchar(38),	
	@ItemTitle varchar(100),
	@Received  DateTime,
	@MessageId varchar(38),
	@Content varchar(max),
	@Sender varchar(30),
	@SendToName varchar(30),
	@ExternalId	varchar(38),
	@Text varchar(max),
	@photo1 varchar(200) = null,
	@photo2 varchar(200)= null,
	@photo3 varchar(200)= null,
	@photo4 varchar(200)= null,
	@photo5 varchar(200)= null
)AS
/***********************************************************************************\
Name:		[Message_Download]
Purpose:	Saves the results of scraping ebay messages after 
			logging in to eBay portal
			
Created:	2019.10.01
Creator:	David Sargent


------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/

BEGIN

Insert into [Message](StoreName,BuyerUserName,[Subject],ItemId,Received,MessageId,Content,ItemTitle,Sender,SendToName,ExternalId,[Text],photo1,photo2,photo3,photo4,photo5)
	select @StoreName,@BuyerUserName,@Subject,@ItemId,@Received,@MessageId,@Content,@ItemTitle,@Sender,@SendToName,@ExternalId,@Text,@photo1,@photo2,@photo3,@photo4,@photo5
END
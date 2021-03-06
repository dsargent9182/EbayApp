USE [Ebay]
GO


CREATE OR ALTER Proc [dbo].[Orders_NotesRemove]
(
	@TransactionId			varchar(20) = null,
	@ItemId					varchar(20) = null,
	@Id						int = null
)AS
/***********************************************************************************\
Name:		[Orders_NotesRemove]
Purpose		Set the note field to null 
Created:	2019.10.01
Creator:	David Sargent
			
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/
BEGIN


IF( (@TransactionId is null OR @ItemId is null) AND (@ID is null)) BEGIN

RAISERROR('Transaction Id and Item Id must be provided or Id must be provided',11,1)

END
ELSE	BEGIN
Update Orders
	set 
		IsNoteInEbay = 0,
		Notes = null
	where 1=1
		and TransactionId = ISNULL(@TransactionId,TransactionId)
		and ItemId = ISNULL(@ItemId,ItemId)
		and Id = ISNULL(@Id,Id)



END





END
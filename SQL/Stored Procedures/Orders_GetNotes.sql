USE [Ebay]
GO

CREATE OR ALTER PROC [dbo].[Orders_GetNotes]
/***********************************************************************************\
Name:		[Orders_GetNotes]
Purpose:	Get order details where the notes field is null
Created:	2019.10.01
Creator:	David Sargent

			exec [Orders_GetNotes]
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.01	DS	Created

\***********************************************************************************/ AS
BEGIN

select notes 'notes2',*
	from Orders
	where 1=1
		and IsNoteInEbay = 0
		and notes is not null
	order by PaidTime

END
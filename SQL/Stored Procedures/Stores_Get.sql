USE [Ebay]
GO


CREATE OR ALTER PROC [dbo].[Stores_Get]
/***********************************************************************************\
Name:		[Stores_Get]
Purpose:	Simple query to get Ebay store
			
Created:	2019.10.01
Creator:	David Sargent

			exec [Stores_Get]
------------------------------------------------------------------------------------
Change Log:	
----------	---	--------------------------------------------------------------------
2019.10.21	DS	Created

\***********************************************************************************/ AS
BEGIN
	select * from [Store]
END
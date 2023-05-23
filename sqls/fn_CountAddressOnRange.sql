USE [SalesforceStaging]
GO

/****** Object:  UserDefinedFunction [dbo].[CountAddressOnRange]    Script Date: 5/22/2023 11:39:31 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[CountAddressOnRange] 
(	
	-- Add the parameters for the function here
	-- <@param1, sysname, @p1> <Data_Type_For_Param1, , int>, 
	-- <@param2, sysname, @p2> <Data_Type_For_Param2, , char>
	@Min INT,
	@Max INT
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	/****** Script for SelectTopNRows command from SSMS  ******/
	SELECT COUNT([Id]) AS AddressesOnRange
	FROM [SalesforceStaging].[dbo].[ADV1_AddressesInvolvedInDupFinal]
	WHERE BatchId >= @Min AND BatchId < @Max
)
GO

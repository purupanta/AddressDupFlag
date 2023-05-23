USE [SalesforceStaging]
GO

/****** Object:  UserDefinedFunction [dbo].[DistHaving_x_OrMoreCounts]    Script Date: 5/22/2023 11:38:30 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER FUNCTION [dbo].[DistHaving_x_OrMoreCounts] 
(	
	-- Add the parameters for the function here
	-- <@param1, sysname, @p1> <Data_Type_For_Param1, , int>, 
	-- <@param2, sysname, @p2> <Data_Type_For_Param2, , char>
	@DupkeyCount as INT
)
RETURNS TABLE 
AS
RETURN 
(
	-- Add the SELECT statement with parameter references here
	SELECT DISTINCT DupKey 
	FROM dbo.ADT0_Addresses AS t1 
	WHERE (DupKey IS NOT NULL) 
	GROUP BY DupKey 
	HAVING (COUNT(DupKey) >= @DupkeyCount)
)
GO



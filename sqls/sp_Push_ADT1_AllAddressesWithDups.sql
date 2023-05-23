USE [SalesforceStaging]
GO

/****** Object:  StoredProcedure [dbo].[Push_ADT1_AllAddressesWithDups]    Script Date: 5/22/2023 11:41:20 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Puru,,Panta>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE OR ALTER   PROCEDURE [dbo].[Push_ADT1_AllAddressesWithDups]
	-- Add the parameters for the stored procedure here
	-- <@Param1, sysname, @p1> <Datatype_For_Param1, , int> = <Default_Value_For_Param1, , 0>, 
	-- <@Param2, sysname, @p2> <Datatype_For_Param2, , int> = <Default_Value_For_Param2, , 0>
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	-- SELECT <@Param1, sysname, @p1>, <@Param2, sysname, @p2>
	--
	--
	-- YOU MAY CHANGE THE TABLE NAME HERE ...
	--

INSERT INTO [dbo].[ADT1_AllAddressesWithDups] 
(
	[ADT0_Id]
	,[Name]
	,[DupKey]
	,[Address_SAP_Id__c]
	,[hed__Address__c_Id]
	,[hed__Parent_Account__c]
	,[hed__Parent_Contact__c]
	,[hed__MailingStreet__c]
	,[hed__MailingStreet2__c]
	,[hed__MailingCity__c]
	,[hed__MailingState__c]
	,[hed__MailingPostalCode__c]
	,[hed__MailingCountry__c]
	,[hed__Address_Type__c]
	,[hed__Default_Address__c]
	,[hed__Seasonal_Start_Day__c]
	,[hed__Seasonal_End_Day__c]
	,[hed__Seasonal_Start_Month__c]
	,[hed__Seasonal_End_Month__c]
	,[hed__Seasonal_Start_Year__c]
	,[hed__Seasonal_End_Year__c]
	,[hed__Formula_MailingAddress__c]
	,[hed__Formula_MailingStreetAddress__c]
	,[hed__Latest_Start_Date__c]
	,[hed__Latest_End_Date__c]
	,[Mailing_State__c]
	,[Zip_Code_Length__c]
	,[OwnerId]
	,[CreatedDate]
	,[CreatedById]
	,[LastModifiedDate]
	,[LastModifiedById]
	,[LastViewedDate]
	,[LastReferencedDate]
	,[SystemModstamp]
	,[IsDeleted]
)
SELECT [Id]
      ,[Name]
      ,[DupKey]
      ,[Address_SAP_Id__c]
      ,[hed__Address__c_Id]
      ,[hed__Parent_Account__c]
      ,[hed__Parent_Contact__c]
      ,[hed__MailingStreet__c]
      ,[hed__MailingStreet2__c]
      ,[hed__MailingCity__c]
      ,[hed__MailingState__c]
      ,[hed__MailingPostalCode__c]
      ,[hed__MailingCountry__c]
      ,[hed__Address_Type__c]
      ,[hed__Default_Address__c]
      ,[hed__Seasonal_Start_Day__c]
      ,[hed__Seasonal_End_Day__c]
      ,[hed__Seasonal_Start_Month__c]
      ,[hed__Seasonal_End_Month__c]
      ,[hed__Seasonal_Start_Year__c]
      ,[hed__Seasonal_End_Year__c]
      ,[hed__Formula_MailingAddress__c]
      ,[hed__Formula_MailingStreetAddress__c]
      ,[hed__Latest_Start_Date__c]
      ,[hed__Latest_End_Date__c]
      ,[Mailing_State__c]
      ,[Zip_Code_Length__c]
      ,[OwnerId]
      ,[CreatedDate]
      ,[CreatedById]
      ,[LastModifiedDate]
      ,[LastModifiedById]
      ,[LastViewedDate]
      ,[LastReferencedDate]
      ,[SystemModstamp]
      ,[IsDeleted]
FROM [dbo].[ADT0_Addresses]
WHERE [DupKey] IN (
SELECT        [DupKey]
FROM            [dbo].[ADT0_Addresses]
WHERE        ([DupKey] IS NOT NULL)
GROUP BY [DupKey]
HAVING        (COUNT([DupKey]) >= 2)
)

END
GO

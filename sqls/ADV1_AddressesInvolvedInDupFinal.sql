USE [SalesforceStaging]
GO

-- attempt to run DROP TABLE only if it exists 
DROP VIEW IF EXISTS [SalesforceStaging].[dbo].[ADV1_AddressesInvolvedInDupFinal];
GO

/****** Object:  View [dbo].[ADV1_AddressesInvolvedInDupFinal]    Script Date: 5/22/2023 11:32:01 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[ADV1_AddressesInvolvedInDupFinal]
AS
/****** Script for SelectTopNRows command from SSMS  ******/ 

SELECT 
      t1.[Id],
      t1.[ADT0_Id], 
      t1.[Name], 
      t1.[DupKey], 
      t1.[Address_SAP_Id__c], 
      t1.[hed__Address__c_Id], 
      t1.[hed__Parent_Account__c], 
      t1.[hed__Parent_Contact__c], 
      BatchId = DENSE_RANK() OVER(ORDER BY t1.[hed__Parent_Contact__c]), t1.[hed__MailingStreet__c],
      t1.[hed__MailingStreet2__c], 
      t1.[hed__MailingCity__c], 
      t1.[hed__MailingState__c],
      t1.[hed__MailingPostalCode__c], 
      t1.[hed__MailingCountry__c],
      t1.[hed__Address_Type__c], 
      t1.[hed__Default_Address__c], 
      t1.[hed__Seasonal_Start_Day__c], 
      t1.[hed__Seasonal_End_Day__c], 
      t1.[hed__Seasonal_Start_Month__c], 
      t1.[hed__Seasonal_End_Month__c], 
      t1.[hed__Seasonal_Start_Year__c], 
      t1.[hed__Seasonal_End_Year__c], 
      t1.[hed__Formula_MailingAddress__c], 
      t1.[hed__Formula_MailingStreetAddress__c], 
      t1.[hed__Latest_Start_Date__c], 
      t1.[hed__Latest_End_Date__c], 
      t1.[Mailing_State__c], 
      t1.[Zip_Code_Length__c], 
      t1.[OwnerId], 
      t1.[CreatedDate], 
      t1.[LastModifiedDate], 
      t1.[CreatedById], 
      t1.[LastModifiedById], 
      t1.[LastViewedDate], 
      t1.[LastReferencedDate], 
      t1.[SystemModstamp], 
      t1.[IsDeleted]
FROM [SalesforceStaging].[dbo].[ADT1_AddressesInvolvedInDups] AS t1
GO

IF NOT EXISTS (SELECT * FROM sys.fn_listextendedproperty(N'MS_DiagramPane1' , N'SCHEMA',N'dbo', N'VIEW',N'ADV1_AddressesInvolvedInDupFinal', NULL,NULL))
	EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
USE [SalesforceStaging]
GO

-- attempt to run DROP TABLE only if it exists 
DROP VIEW IF EXISTS [SalesforceStaging].[dbo].[ADV0_AddressesInvolvedInDup];
GO

/****** Object:  View [dbo].[ADV0_AddressesInvolvedInDup]    Script Date: 5/22/2023 11:28:52 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER VIEW [dbo].[ADV0_AddressesInvolvedInDup]
AS
SELECT        Id, Name, DupKey, Address_SAP_Id__c, hed__Address__c_Id, hed__Parent_Account__c, hed__Parent_Contact__c, hed__MailingStreet__c, hed__MailingStreet2__c, hed__MailingCity__c, hed__MailingState__c, 
                         hed__MailingPostalCode__c, hed__MailingCountry__c, hed__Address_Type__c, hed__Default_Address__c, hed__Seasonal_Start_Day__c, hed__Seasonal_End_Day__c, hed__Seasonal_Start_Month__c, 
                         hed__Seasonal_End_Month__c, hed__Seasonal_Start_Year__c, hed__Seasonal_End_Year__c, hed__Formula_MailingAddress__c, hed__Formula_MailingStreetAddress__c, hed__Latest_Start_Date__c, 
                         hed__Latest_End_Date__c, Mailing_State__c, Zip_Code_Length__c, OwnerId, CreatedDate, CreatedById, LastModifiedDate, LastModifiedById, LastViewedDate, LastReferencedDate, SystemModstamp, IsDeleted
FROM            dbo.ADT0_Addresses
WHERE        (DupKey IN
                             (SELECT        DupKey
                               FROM            dbo.DistHaving_x_OrMoreCounts(2) AS DistHaving_x_OrMoreCounts_1))
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]

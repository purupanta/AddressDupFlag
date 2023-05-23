/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO

-- use database
USE [SalesforceStaging]
GO

-- attempt to run DROP TABLE only if it exists 
DROP TABLE IF EXISTS [SalesforceStaging].[dbo].[ADT0_Addresses];
GO

CREATE TABLE dbo.ADT0_Addresses
	(
	Id int NOT NULL,
	Name nvarchar(100) NULL,
	DupKey nvarchar(2000) NULL,
	Address_SAP_Id__c nvarchar(255) NULL,
	hed__Address__c_Id nvarchar(50) NOT NULL,
	hed__Parent_Account__c nvarchar(50) NULL,
	hed__Parent_Contact__c nvarchar(50) NULL,
	hed__MailingStreet__c nvarchar(255) NULL,
	hed__MailingStreet2__c nvarchar(255) NULL,
	hed__MailingCity__c nvarchar(255) NULL,
	hed__MailingState__c nvarchar(255) NULL,
	hed__MailingPostalCode__c nvarchar(255) NULL,
	hed__MailingCountry__c nvarchar(255) NULL,
	hed__Address_Type__c nvarchar(255) NULL,
	hed__Default_Address__c nvarchar(50) NULL,
	hed__Seasonal_Start_Day__c nvarchar(255) NULL,
	hed__Seasonal_End_Day__c nvarchar(255) NULL,
	hed__Seasonal_Start_Month__c nvarchar(255) NULL,
	hed__Seasonal_End_Month__c nvarchar(255) NULL,
	hed__Seasonal_Start_Year__c nvarchar(50) NULL,
	hed__Seasonal_End_Year__c nvarchar(50) NULL,
	hed__Formula_MailingAddress__c nvarchar(1300) NULL,
	hed__Formula_MailingStreetAddress__c nvarchar(1300) NULL,
	hed__Latest_Start_Date__c nvarchar(50) NULL,
	hed__Latest_End_Date__c nvarchar(50) NULL,
	Mailing_State__c nvarchar(255) NULL,
	Zip_Code_Length__c nvarchar(50) NULL,
	OwnerId nvarchar(50) NULL,
	CreatedDate nvarchar(50) NULL,
	CreatedById nvarchar(50) NULL,
	LastModifiedDate nvarchar(50) NULL,
	LastModifiedById nvarchar(50) NULL,
	LastViewedDate nvarchar(50) NULL,
	LastReferencedDate nvarchar(50) NULL,
	SystemModstamp nvarchar(50) NULL,
	IsDeleted nvarchar(50) NULL
	)  ON [PRIMARY]
GO
ALTER TABLE dbo.ADT0_Addresses SET (LOCK_ESCALATION = TABLE)
GO
COMMIT

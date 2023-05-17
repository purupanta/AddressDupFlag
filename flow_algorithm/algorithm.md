
<b>Input:</b> All addresses from hed__Address__c <br />
<b>Output:</b> A subset of hed__Address__c, who are involved in duplicates <br />
	This subset of address will have following three properties added on them in output:
	a. DupCount: Total number of duplicates for each address item.
	b. DupAddressIds: Other identical addresses (max 50), saperated by comma.
	c. DupFlag: "YES" = YES to delete; "NO" = NO to delete as we keep one address from the list of duplicate addresses.

	Condition: The addresses are said to be duplicates if they are in:
		a. Same contact (Or Account if Contact is not associated with Address) And,
		b. Have all of the following identical: 
		hed__MailingStreet__c, hed__MailingStreet2__c, hed__MailingCity__c, hed__MailingState__c, hed__MailingPostalCode__c, hed__MailingCounty__c
		
<hr />
Data_Preprocessing:

Foreach Address in hed__Address__c:
	Build_DupKey()
	Push to table, "ADT0_Addresses"

Group By "DupKey" and Count Addresses in each group
If Count >= 2:
	Move to a new table, "ADT1_AddressesInvolvedInDups" for further processing


For each Address in new table, "ADT1_AddressesInvolvedInDups":

	For each Distinct Contact in this table
		BatchId = a serial number
		(So, now each distinct contact now has a unique BatchId)


For each Address in new table, "ADT1_AddressesInvolvedInDups":
	Assign the BatchId Based on its ContactId


<strong>Input:</strong> All addresses from hed__Address__c
<strong>Output:</strong> A subset of hed__Address__c, who are involved in duplicates.
	This subset of address will have following three prperties added on them in output:
	      a. DupCount: Total number of duplicates for each address item.
	      b. DupAddressIds: Other identical addresses (max 50), saperated by comma.
	      c. DupFlag: "YES" = YES to delete; "NO" = NO to delete as we keep one address from the list of duplicate addresses.
<strong>Condition:</strong> The addresses are said to be duplicates if they are in:
	a. Same contact (Or Account if Contact is not associated with Address) And,
	b. Have all of the following identical: 
		hed__MailingStreet__c, hed__MailingStreet2__c, hed__MailingCity__c, hed__MailingState__c, hed__MailingPostalCode__c, hed__MailingCounty__c


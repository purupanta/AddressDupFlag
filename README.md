# AddressDupFlag: Synopsis <hr />

Issue: The salesforce HEDA address object, "hed__Address__c" is currently containing a bulk of duplicate addresses. We are trying to get rid of same addresses for same contact (or account if contact is not present) are taking space in salesforce address object.
<hr />

We have built a process that flags all the duplicate address either "YES" (to be deleted) or "NO" (not to be deleted). Following is the basic description of how the process works:

1. The process basically pulls all the address data from the "hed__Address__c" object.
2. Assigns a unique "DupKey" for each of the addresses and push to table, "ADT0_Addresses"
3. Group by "DupKey" then count >= 2 in each group, then gives us only those addresses involved in duplications.
4. We pull the set of these addresses involved in duplications and push this dataset to the other table, "ADT1_AddressesInvolvedInDups" for further processing.
5. Now for each of the Distinct Contact id, "hed__Parent_Contact__c", we assign a unique Id (A serial number) and we call it BatchId. It guarantees the all addresses being involved in duplication will be pulled for batch processing, when we pull a set of batch items.<br />
(Ex: Pull addresses where BatchId >= BatchId_Min AND BatchId <= BatchSize), where we pick the BatchSize optimal based on our allocated buffer in the OS. So it processes the whole set of addresses in the chunk of our defined BatchSize, then avoids potential buffer overflow and guarantees correctness in the outcome.
7. The process runs in the loop until it reaches the BatchId_Max.
8. The final outcome will be stored in the table, "ADT2_AddressesInvolvedInDupsFinal" 

<hr /><b>
A detailed algorithm can be found [Contribution guidelines for this project](flow_algorithm/algorithm.md) <br />
The control flow can be found [here](https://github.com/purupanta/AddressDupFlag/blob/main/flow_algorithm/CFlow%20AddressDup%20Flag.pdf)).
</b>

[GitHub Pages]([https://pages.github.com/](https://github.com/purupanta/AddressDupFlag/blob/main/flow_algorithm/CFlow%20AddressDup%20Flag.pdf))

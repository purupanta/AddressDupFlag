#region Help:  Introduction to the Script Component
/* The Script Component allows you to perform virtually any operation that can be accomplished in
 * a .Net application within the context of an Integration Services data flow.
 *
 * Expand the other regions which have "Help" prefixes for examples of specific ways to use
 * Integration Services features within this script component. */
#endregion

#region Namespaces
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Dts.Pipeline.Wrapper;
using Microsoft.SqlServer.Dts.Runtime.Wrapper;
#endregion

/// <summary>
/// This is the class to which to add your code.  Do not change the name, attributes, or parent
/// of this class.
/// </summary>
[Microsoft.SqlServer.Dts.Pipeline.SSISScriptComponentEntryPointAttribute]
public class ScriptMain : UserComponent
{
    #region Help:  Using Integration Services variables and parameters
    /* To use a variable in this script, first ensure that the variable has been added to
     * either the list contained in the ReadOnlyVariables property or the list contained in
     * the ReadWriteVariables property of this script component, according to whether or not your
     * code needs to write into the variable.  To do so, save this script, close this instance of
     * Visual Studio, and update the ReadOnlyVariables and ReadWriteVariables properties in the
     * Script Transformation Editor window.
     * To use a parameter in this script, follow the same steps. Parameters are always read-only.
     *
     * Example of reading from a variable or parameter:
     *  DateTime startTime = Variables.MyStartTime;
     *
     * Example of writing to a variable:
     *  Variables.myStringVariable = "new value";
     */
    #endregion

    #region Help:  Using Integration Services Connnection Managers
    /* Some types of connection managers can be used in this script component.  See the help topic
     * "Working with Connection Managers Programatically" for details.
     *
     * To use a connection manager in this script, first ensure that the connection manager has
     * been added to either the list of connection managers on the Connection Managers page of the
     * script component editor.  To add the connection manager, save this script, close this instance of
     * Visual Studio, and add the Connection Manager to the list.
     *
     * If the component needs to hold a connection open while processing rows, override the
     * AcquireConnections and ReleaseConnections methods.
     * 
     * Example of using an ADO.Net connection manager to acquire a SqlConnection:
     *  object rawConnection = Connections.SalesDB.AcquireConnection(transaction);
     *  SqlConnection salesDBConn = (SqlConnection)rawConnection;
     *
     * Example of using a File connection manager to acquire a file path:
     *  object rawConnection = Connections.Prices_zip.AcquireConnection(transaction);
     *  string filePath = (string)rawConnection;
     *
     * Example of releasing a connection manager:
     *  Connections.SalesDB.ReleaseConnection(rawConnection);
     */
    #endregion

    #region Help:  Firing Integration Services Events
    /* This script component can fire events.
     *
     * Example of firing an error event:
     *  ComponentMetaData.FireError(10, "Process Values", "Bad value", "", 0, out cancel);
     *
     * Example of firing an information event:
     *  ComponentMetaData.FireInformation(10, "Process Values", "Processing has started", "", 0, fireAgain);
     *
     * Example of firing a warning event:
     *  ComponentMetaData.FireWarning(10, "Process Values", "No rows were received", "", 0);
     */
    #endregion

    List<InputDataRow> InputDataTable;
    /// <summary>
    /// This method is called once, before rows begin to be processed in the data flow.
    ///
    /// You can remove this method if you don't need to do anything here.
    /// </summary>
    public override void PreExecute()
    {
        base.PreExecute();
        /*
         * Add your code here
         */
        this.InputDataTable = new List<InputDataRow>();
    }

    /// <summary>
    /// This method is called after all the rows have passed through this component.
    ///
    /// You can delete this method if you don't need to do anything here.
    /// </summary>
    public override void PostExecute()
    {
        base.PostExecute();
        /*
         * Add your code here
         */
    }

    /// <summary>
    /// This method is called once the last row has hit.
    /// Since we will can only find the highest OneToManyDataId
    /// after receiving all the rows, this the only time we can
    /// send rows to the output buffer.
    /// </summary>
    public override void FinishOutputs()
    {
        base.FinishOutputs();
        CreateNewOutputRows();
    }


    /*
    public override void Input0_ProcessInput(Input0Buffer Buffer)
    {
        while (Buffer.NextRow())
        {
            Input0_ProcessInputRow(Buffer);
        }

        if (Buffer.EndOfRowset())
        {
            Output0Buffer.SetEndOfRowset();
        }
    }
    */

    /// <summary>
    /// This method is called once for every row that passes through the component from Input0.
    ///
    /// Example of reading a value from a column in the the row:
    ///  string zipCode = Row.ZipCode
    ///
    /// Example of writing a value to a column in the row:
    ///  Row.ZipCode = zipCode
    /// </summary>
    /// <param name="Row">The row that is currently passing through the component</param>
    public override void Input0_ProcessInputRow(Input0Buffer Row)
    {
        /*
         * Add your code here
         */
        InputDataRow CurrentRow = new InputDataRow();
        CurrentRow = MapInputRow(Row);

        //Building the LINQ-able 'data' table
        InputDataTable.Add(CurrentRow);


        //Flushing out the "InputDataRow" object
        CurrentRow = new InputDataRow();

    }

    private RunTimeInfo RTI = new RunTimeInfo();

    public override void CreateNewOutputRows()
    {
        /*
          Add rows by calling the AddRow method on the member variable named "<Output Name>Buffer".
          For example, call MyOutputBuffer.AddRow() if your output was named "MyOutput".
        */
        try
        {
            RTI.ContactCounter = 0;
            foreach (var EachContactGrp in this.InputDataTable.GroupBy(d => d.hed__Parent_Contact__c))
            {
                RTI.DupAddressCounter = 0;
                foreach (var EachAddressGrp in EachContactGrp.GroupBy(c => c.DupKey))
                {
                    if (EachAddressGrp.Count() >= 2)
                    {
                        List<string> CurrentGroupAddressIdList = new List<string>();
                        RTI.DupAddressInEachGrpCounter = 0;
                        foreach (var AddressItem in EachAddressGrp)
                        {
                            InputDataRow CurrentRow = (InputDataRow)AddressItem;
                            CurrentGroupAddressIdList.Add(CurrentRow.hed__Address__c_Id);

                            //Take only 50 AddressIds if found more than 50 Addresses being duplicate.
                            if (RTI.DupAddressInEachGrpCounter > 50) break;

                            RTI.DupAddressInEachGrpCounter++;
                        }
                        string CurrentGroupAddressIds = string.Join(",", CurrentGroupAddressIdList);


                        RTI.DupAddressInEachGrpCounter = 0;
                        foreach (var AddressItem in EachAddressGrp)
                        {
                            if (RTI.DupAddressInEachGrpCounter == 0)
                            {
                                RTI.DupFlag = "NO";
                                //AddressItem.hed__Default_Address__c = "true"; //SET THIS VALUE TRUE TO MAKE THE ADDRESS WE KEEP THE DEFAULT ADDRESS
                            }
                            if (RTI.DupAddressInEachGrpCounter > 0)
                            {
                                RTI.DupFlag = "YES";
                            }
                            RTI.DupAddressIds = CurrentGroupAddressIds;
                            RTI.DupCount = EachAddressGrp.Count();

                            Output0Buffer.AddRow();
                            Output0Buffer.Id = AddressItem.Id;
                            Output0Buffer.ADT0Id = AddressItem.ADT0_Id;
                            Output0Buffer.BatchId = AddressItem.BatchId;
                            Output0Buffer.Name = AddressItem.Name;
                            Output0Buffer.DupCount = RTI.DupCount;
                            Output0Buffer.DupAddressIds = RTI.DupAddressIds;
                            Output0Buffer.DupFlag = RTI.DupFlag;
                            Output0Buffer.DupKey = AddressItem.DupKey;
                            Output0Buffer.AddressSAPIdc = AddressItem.Address_SAP_Id__c;
                            Output0Buffer.hedAddresscId = AddressItem.hed__Address__c_Id;
                            Output0Buffer.hedParentAccountc = AddressItem.hed__Parent_Account__c;
                            Output0Buffer.hedParentContactc = AddressItem.hed__Parent_Contact__c;
                            Output0Buffer.hedMailingStreetc = AddressItem.hed__MailingStreet__c;
                            Output0Buffer.hedMailingStreet2c = AddressItem.hed__MailingStreet2__c;
                            Output0Buffer.hedMailingCityc = AddressItem.hed__MailingCity__c;
                            Output0Buffer.hedMailingStatec = AddressItem.hed__MailingState__c;
                            Output0Buffer.hedMailingPostalCodec = AddressItem.hed__MailingPostalCode__c;
                            Output0Buffer.hedMailingCountryc = AddressItem.hed__MailingCountry__c;
                            Output0Buffer.hedAddressTypec = AddressItem.hed__Address_Type__c;
                            Output0Buffer.hedDefaultAddressc = AddressItem.hed__Default_Address__c;
                            Output0Buffer.hedSeasonalStartDayc = AddressItem.hed__Seasonal_Start_Day__c;
                            Output0Buffer.hedSeasonalEndDayc = AddressItem.hed__Seasonal_End_Day__c;
                            Output0Buffer.hedSeasonalStartMonthc = AddressItem.hed__Seasonal_Start_Month__c;
                            Output0Buffer.hedSeasonalEndMonthc = AddressItem.hed__Seasonal_End_Month__c;
                            Output0Buffer.hedSeasonalStartYearc = AddressItem.hed__Seasonal_Start_Year__c;
                            Output0Buffer.hedSeasonalEndYearc = AddressItem.hed__Seasonal_End_Year__c;
                            Output0Buffer.hedFormulaMailingAddressc = AddressItem.hed__Formula_MailingAddress__c;
                            Output0Buffer.hedFormulaMailingStreetAddressc = AddressItem.hed__Formula_MailingStreetAddress__c;
                            Output0Buffer.hedLatestStartDatec = AddressItem.hed__Latest_Start_Date__c;
                            Output0Buffer.hedLatestEndDatec = AddressItem.hed__Latest_End_Date__c;
                            Output0Buffer.MailingStatec = AddressItem.Mailing_State__c;
                            Output0Buffer.ZipCodeLengthc = AddressItem.Zip_Code_Length__c;
                            Output0Buffer.OwnerId = AddressItem.OwnerId;
                            Output0Buffer.CreatedDate = AddressItem.CreatedDate;
                            Output0Buffer.CreatedById = AddressItem.CreatedById;
                            Output0Buffer.LastModifiedDate = AddressItem.LastModifiedDate;
                            Output0Buffer.LastModifiedById = AddressItem.LastModifiedById;
                            Output0Buffer.LastViewedDate = AddressItem.LastViewedDate;
                            Output0Buffer.LastReferencedDate = AddressItem.LastReferencedDate;
                            Output0Buffer.SystemModstamp = AddressItem.SystemModstamp;
                            Output0Buffer.IsDeleted = AddressItem.IsDeleted;

                            RTI.DupAddressInEachGrpCounter++;

                        }
                        RTI.DupAddressCounter++;
                    }
                }

                RTI.ContactCounter++;
            }

        }
        catch (Exception Ex)
        {
            Console.WriteLine("####EXCEPTION #2: " + Ex.ToString());
        }
       

        /* NOT NEEDED AT THIS POINT
        //LOOP THROUGH DATA
        foreach (InputDataRow CurrentRow IN InputDataTable){ } */

    }


    //DEPRECIATED LIBRARIES

    //Mapping the current input row to the InputDataRow object
    private InputDataRow MapInputRow(Input0Buffer Row)
    {
        //Format: Object Item = Input data row
        InputDataRow CurrentRow = new InputDataRow();
        CurrentRow.Id = Row.Id;
        CurrentRow.ADT0_Id = Row.ADT0Id;
        CurrentRow.Name = Row.Name;
        CurrentRow.BatchId = Row.BatchId;
        CurrentRow.DupKey = Row.DupKey;
        CurrentRow.Address_SAP_Id__c = Row.AddressSAPIdc;
        CurrentRow.hed__Address__c_Id = Row.hedAddresscId;
        CurrentRow.hed__Parent_Account__c = Row.hedParentAccountc;
        CurrentRow.hed__Parent_Contact__c = Row.hedParentContactc;
        CurrentRow.hed__MailingStreet__c = Row.hedMailingStreetc;
        CurrentRow.hed__MailingStreet2__c = Row.hedMailingStreet2c;
        CurrentRow.hed__MailingCity__c = Row.hedMailingCityc;
        CurrentRow.hed__MailingState__c = Row.hedMailingStatec;
        CurrentRow.hed__MailingPostalCode__c = Row.hedMailingPostalCodec;
        CurrentRow.hed__MailingCountry__c = Row.hedMailingCountryc;
        CurrentRow.hed__Address_Type__c = Row.hedAddressTypec;
        CurrentRow.hed__Default_Address__c = Row.hedDefaultAddressc;
        CurrentRow.hed__Seasonal_Start_Day__c = Row.hedSeasonalStartDayc;
        CurrentRow.hed__Seasonal_End_Day__c = Row.hedSeasonalEndDayc;
        CurrentRow.hed__Seasonal_Start_Month__c = Row.hedSeasonalStartMonthc;
        CurrentRow.hed__Seasonal_End_Month__c = Row.hedSeasonalEndMonthc;
        CurrentRow.hed__Seasonal_Start_Year__c = Row.hedSeasonalStartYearc;
        CurrentRow.hed__Seasonal_End_Year__c = Row.hedSeasonalEndYearc;
        CurrentRow.hed__Formula_MailingAddress__c = Row.hedFormulaMailingAddressc;
        CurrentRow.hed__Formula_MailingStreetAddress__c = Row.hedFormulaMailingStreetAddressc;
        CurrentRow.hed__Latest_Start_Date__c = Row.hedLatestStartDatec;
        CurrentRow.hed__Latest_End_Date__c = Row.hedLatestEndDatec;
        CurrentRow.Mailing_State__c = Row.MailingStatec;
        CurrentRow.Zip_Code_Length__c = Row.ZipCodeLengthc;
        CurrentRow.OwnerId = Row.OwnerId;
        CurrentRow.CreatedDate = Row.CreatedDate;
        CurrentRow.CreatedById = Row.CreatedById;
        CurrentRow.LastModifiedDate = Row.LastModifiedDate;
        CurrentRow.LastModifiedById = Row.LastModifiedById;
        CurrentRow.LastViewedDate = Row.LastViewedDate;
        CurrentRow.LastReferencedDate = Row.LastReferencedDate;
        CurrentRow.SystemModstamp = Row.SystemModstamp;
        CurrentRow.IsDeleted = Row.IsDeleted;

        return CurrentRow;
    }
    private string BuildKey(Input0Buffer Row)
    {
        string op = null;

        Row.hedParentContactc = CleanStr(Row.hedParentContactc);
        Row.hedMailingStreetc = CleanStr(Row.hedMailingStreetc);
        Row.hedMailingStreet2c = CleanStr(Row.hedMailingStreet2c);
        Row.hedMailingCityc = CleanStr(Row.hedMailingCityc);
        Row.hedMailingStatec = CleanStr(Row.hedMailingStatec);
        Row.hedMailingPostalCodec = CleanStr(Row.hedMailingPostalCodec);
        Row.hedMailingCountryc = CleanStr(Row.hedMailingCountryc);

        op = Row.hedParentContactc + Row.hedMailingStreetc + Row.hedMailingStreet2c + Row.hedMailingCityc + Row.hedMailingStatec + Row.hedMailingPostalCodec + Row.hedMailingCountryc;

        return op;
    }

    private string CleanStr(string s1)
    {
        if (!string.IsNullOrEmpty(s1) && !string.IsNullOrWhiteSpace(s1) && s1 != "") s1.Trim();
        return s1;
    }

}

//Object

public class RunTimeInfo
{
    public int ContactCounter { get; set; }
    public int DupAddressCounter { get; set; }
    public int DupAddressInEachGrpCounter { get; set; }

    public int DupCount { get; set; }
    public string DupAddressIds { get; set; }
    public string DupFlag { get; set; }

    //Constructor
    public RunTimeInfo()
    {
        this.ContactCounter = 0;
        this.DupAddressCounter = 0;
        this.DupAddressInEachGrpCounter = 0;
        this.DupCount = 0;
        this.DupAddressIds = null;
        this.DupFlag = null;
    }

}
public class InputDataRow
{
    public int Id { get; set; }
    public int ADT0_Id { get; set; }
    public string Name { get; set; }
    public long BatchId { get; set; }
    public string DupKey { get; set; }
    public string Address_SAP_Id__c { get; set; }
    public string hed__Address__c_Id { get; set; }
    public string hed__Parent_Account__c { get; set; }
    public string hed__Parent_Contact__c { get; set; }
    public string hed__MailingStreet__c { get; set; }
    public string hed__MailingStreet2__c { get; set; }
    public string hed__MailingCity__c { get; set; }
    public string hed__MailingState__c { get; set; }
    public string hed__MailingPostalCode__c { get; set; }
    public string hed__MailingCountry__c { get; set; }
    public string hed__Address_Type__c { get; set; }
    public string hed__Default_Address__c { get; set; }
    public string hed__Seasonal_Start_Day__c { get; set; }
    public string hed__Seasonal_End_Day__c { get; set; }
    public string hed__Seasonal_Start_Month__c { get; set; }
    public string hed__Seasonal_End_Month__c { get; set; }
    public string hed__Seasonal_Start_Year__c { get; set; }
    public string hed__Seasonal_End_Year__c { get; set; }
    public string hed__Formula_MailingAddress__c { get; set; }
    public string hed__Formula_MailingStreetAddress__c { get; set; }
    public string hed__Latest_Start_Date__c { get; set; }
    public string hed__Latest_End_Date__c { get; set; }
    public string Mailing_State__c { get; set; }
    public string Zip_Code_Length__c { get; set; }
    public string OwnerId { get; set; }
    public string CreatedDate { get; set; }
    public string CreatedById { get; set; }
    public string LastModifiedDate { get; set; }
    public string LastModifiedById { get; set; }
    public string LastViewedDate { get; set; }
    public string LastReferencedDate { get; set; }
    public string SystemModstamp { get; set; }
    public string IsDeleted { get; set; }
  
}

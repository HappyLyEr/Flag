using System;
using openwfe.workitem;
using log4net;
using log4net.Config;
using GASystem.DataModel;
using GASystem;
using GASystem.AppUtils;
using GASystem.BusinessLayer;
using GASystem.DataAccess;
using GASystem.DataAccess.Utils;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;


namespace GASystem.DotNetApre
{
        class UpdateNotifier : impl.SimpleConsumer
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(WorkflowStarter));

        /// <summary>
        /// The main entry point for the application. Send e-mail notifications from GARecordHistoryLog, then delete reported GARecordHistoryLog records
        /// </summary>
        /// private
        
            private static int currentPersonnelRowId=0;
            private static int currentRoleListsRowId=0;
            private static int personnelRowId=0;
            private static int roleListsRowId=0;
            private static string Subject = "Flag form update notification";
            private static string TextBody = string.Empty;
            // Tor 20151215 change to HTMP message
            // private static bool sendInHTLMFormat = false;
            private static bool sendInHTLMFormat = true;
            private static string ownerClass = string.Empty;
            private static int ownerClassRowId = 0;
            private static string currentOwnerClass = string.Empty;
            private static int currentOwnerClassRowId = 0;
            private static string reportOnClass = string.Empty;
            private static int reportOnClassRowId = 0;
            private static string currentReportOnClass = string.Empty;
            private static int currentReportOnClassRowId = 0;
            private static string memberClass = string.Empty;
            private static int memberClassRowId = 0;
            private static string currentMemberClass = string.Empty;
            private static int currentMemberClassRowId = 0;
            private static bool messageSent = false;
            private static string fieldId = string.Empty;
            private static string oldFieldValue = string.Empty;
            private static string newFieldValue = string.Empty;
            private static string currentFieldId = string.Empty;
            private static string currentOldFieldValue = string.Empty;
            private static string currentNewFieldValue = string.Empty;
            private static DateTime DateTimeFieldIdChanged = DateTime.MinValue;
            private static DateTime currentDateTimeFieldIdChanged = DateTime.MinValue;
            private static int ChangedByPersonnelRowId = 0;
            private static string newRecordHeadingValue = string.Empty;
            private static int currentChangedByPersonnelRowId = 0;
            private static string currentNewRecordHeadingValue = string.Empty;
            private static bool isNewRecord = false;
            private static bool currentIsNewRecord = false;

            [STAThread]

            static void Main(string[] args)
            {
                InitializeVariables();
                GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
                ReportOnAllPersons(transaction);
                transaction.CommitAndClose();
                InitializeVariables();
                transaction = GADataTransaction.StartGADataTransaction();
                ReportOnAllRoles(transaction);
                // commit transaction
                transaction.CommitAndClose();

            }
            private static void ReportOnAllPersons(GADataTransaction transaction)
            {              
                RecordHistoryLog historylog = new RecordHistoryLog();
//                GADataTransaction transaction = GADataTransaction.StartGADataTransaction();

                // read all GARecordHistoryLogs to Person _selectPersonnelSqlSorted = @"SELECT * FROM GARecordHistoryLog order by reportToPersonnelRowId,reportOnClass,reportOnClassRowId,DateTimeFieldIdChanged,classMember,classMemberRowId,classOwner,classOwnerRowId where reportToPersonnelRowId is not null";
                // read all GARecordHistoryLogs to Role:  _selectRolesSqlSorted = @"SELECT * FROM GARecordHistoryLog order by reportToRoleListsRowId,reportOnClass,reportOnClassRowId,DateTimeFieldIdChanged where reportToRoleListsRowId is not null";
        		
                //// read all GAHistory records ordered by reportToPersonnelRowId,reportToRoleListsRowId,reportOnClass,reportOnClassRowId,DateTimeFieldIdChanged,classMember,classMemberRowId,classOwner,classOwnerRowId
                
                RecordHistoryLogDS ds = RecordHistoryLog.GetAllRecordHistoryLogsToPersonSorted();
                int i = -1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    i++;
                    currentPersonnelRowId = (int)ds.Tables[0].Rows[i]["reportToPersonnelRowId"];
                    if (!ds.Tables[0].Rows[i]["reportToPersonnelRowId"].Equals(DBNull.Value))
                        currentPersonnelRowId = (int)ds.Tables[0].Rows[i]["reportToPersonnelRowId"];
                    else currentPersonnelRowId = 0;
                    if (currentPersonnelRowId > 0) // report on person
                    {
                        if(currentPersonnelRowId!=personnelRowId)
                        {
                            if (personnelRowId > 0) // new person
                            {
                                // then send previous build e-mail;
                                GASystem.BusinessLayer.FlagSMTP.SendMessage(personnelRowId, Subject, TextBody, sendInHTLMFormat);
                                messageSent = true;
                                personnelRowId=currentPersonnelRowId;
                                // build new e-mail message header
                                TextBody = string.Empty;
                                System.Console.WriteLine("build e-mail body part for person");
                            }
                            else
                            { // first person
                                System.Console.WriteLine("first person");
                                if (TextBody != string.Empty && roleListsRowId /*currentRoleListsRowId*/ > 0)
                                {
//                                    SendEmailToRoleId(Subject, TextBody, sendInHTLMFormat, memberClass, memberClassRowId, currentRoleListsRowId);
                                    SendEmailToRoleId(Subject, TextBody, sendInHTLMFormat, reportOnClass, reportOnClassRowId, roleListsRowId /* currentRoleListsRowId*/);
                                    messageSent = true;
                                }
                                TextBody = string.Empty;
                                personnelRowId= currentPersonnelRowId;
                            }
                        }
                    }
                    //                 buid e-mail body part
            

                    currentOwnerClass = ds.Tables[0].Rows[i]["classOwner"].ToString();
                    currentOwnerClassRowId = (int)ds.Tables[0].Rows[i]["classOwnerRowId"];
                    currentReportOnClass = ds.Tables[0].Rows[i]["reportOnClass"].ToString() ;
                    currentReportOnClassRowId = (int)ds.Tables[0].Rows[i]["reportOnClassRowId"];
                    currentMemberClass = ds.Tables[0].Rows[i]["classMember"].ToString();
                    currentMemberClassRowId = (int)ds.Tables[0].Rows[i]["classMemberRowId"];
                    currentFieldId = ds.Tables[0].Rows[i]["FieldId"].ToString();
                    currentOldFieldValue = ds.Tables[0].Rows[i]["OldAttributeValue"].ToString();
                    currentNewFieldValue = ds.Tables[0].Rows[i]["NewAttributeValue"].ToString();
                    currentChangedByPersonnelRowId = (int)ds.Tables[0].Rows[i]["ChangedByPersonnelRowId"];
                    currentNewRecordHeadingValue = ds.Tables[0].Rows[i]["TextFree1"].ToString();
                    currentIsNewRecord = (bool)ds.Tables[0].Rows[i]["isNewRecord"];
                    currentDateTimeFieldIdChanged = (DateTime)ds.Tables[0].Rows[i]["DateTimeFieldIdChanged"];

                    buildBodyAndDetails(ds);
                    
                    row.Delete();
                }
                // SEND LAST MESSAGE
                System.Console.WriteLine("SEND LAST MESSAGE TO Person");
                if (TextBody != string.Empty) GASystem.BusinessLayer.FlagSMTP.SendMessage(personnelRowId, Subject, TextBody, sendInHTLMFormat);

                // delete dataset
                // Tor 20170328 historylog.Update(ds,transaction);
                UpdateGARecordHistoryLogDataset(ds, transaction);
            }

            private static void ReportOnAllRoles(GADataTransaction transaction)
            {
                RecordHistoryLog historylog = new RecordHistoryLog();
                //                GADataTransaction transaction = GADataTransaction.StartGADataTransaction();

                // read all GARecordHistoryLogs to Person _selectPersonnelSqlSorted = @"SELECT * FROM GARecordHistoryLog order by reportToPersonnelRowId,reportOnClass,reportOnClassRowId,DateTimeFieldIdChanged,classMember,classMemberRowId,classOwner,classOwnerRowId where reportToPersonnelRowId is not null";
                // read all GARecordHistoryLogs to Role:  _selectRolesSqlSorted = @"SELECT * FROM GARecordHistoryLog order by reportToRoleListsRowId,reportOnClass,reportOnClassRowId,DateTimeFieldIdChanged where reportToRoleListsRowId is not null";

                //// read all GAHistory records ordered by reportToPersonnelRowId,reportToRoleListsRowId,reportOnClass,reportOnClassRowId,DateTimeFieldIdChanged,classMember,classMemberRowId,classOwner,classOwnerRowId

                RecordHistoryLogDS ds = RecordHistoryLog.GetAllRecordHistoryLogsToRoleSorted();
                int i = -1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    i++;
                    if (!ds.Tables[0].Rows[i]["reportToRoleListsRowId"].Equals(DBNull.Value))
                        currentRoleListsRowId = (int)ds.Tables[0].Rows[i]["reportToRoleListsRowId"];
                    else currentRoleListsRowId = 0;

                    if (currentRoleListsRowId > 0) // report on role
                    {
                        if (currentRoleListsRowId != roleListsRowId)
                        {
                            if (roleListsRowId > 0) // new role
                            { // send previous build e-mail to previous role;
                                SendEmailToRoleId(Subject, TextBody, sendInHTLMFormat, reportOnClass, reportOnClassRowId, roleListsRowId);
                                messageSent = true;
                                TextBody = string.Empty;
                                roleListsRowId = currentRoleListsRowId ;
                            }
                            else
                            { // first role
                                if (TextBody != string.Empty && personnelRowId > 0)
                                { // send last message built to Person if TextBody not empty and currentPersonnel > 0
                                    GASystem.BusinessLayer.FlagSMTP.SendMessage(personnelRowId, Subject, TextBody, sendInHTLMFormat);
                                    messageSent = true;
                                    TextBody = string.Empty;
                                }
                                System.Console.WriteLine("first role");
                                roleListsRowId=currentRoleListsRowId ;
                            }
                        }
                    }
                    currentOwnerClass = ds.Tables[0].Rows[i]["classOwner"].ToString();
                    currentOwnerClassRowId = (int)ds.Tables[0].Rows[i]["classOwnerRowId"];
                    currentReportOnClass = ds.Tables[0].Rows[i]["reportOnClass"].ToString();
                    currentReportOnClassRowId = (int)ds.Tables[0].Rows[i]["reportOnClassRowId"];
                    currentMemberClass = ds.Tables[0].Rows[i]["classMember"].ToString();
                    currentMemberClassRowId = (int)ds.Tables[0].Rows[i]["classMemberRowId"];
                    currentFieldId = ds.Tables[0].Rows[i]["FieldId"].ToString();
                    currentOldFieldValue = ds.Tables[0].Rows[i]["OldAttributeValue"].ToString();
                    currentNewFieldValue = ds.Tables[0].Rows[i]["NewAttributeValue"].ToString();
                    currentChangedByPersonnelRowId = (int)ds.Tables[0].Rows[i]["ChangedByPersonnelRowId"];
                    currentNewRecordHeadingValue = ds.Tables[0].Rows[i]["TextFree1"].ToString();
                    currentIsNewRecord = (bool)ds.Tables[0].Rows[i]["isNewRecord"];
                    currentDateTimeFieldIdChanged = (DateTime)ds.Tables[0].Rows[i]["DateTimeFieldIdChanged"];

                    buildBodyAndDetails(ds);

                    row.Delete();
                }
                // SEND LAST MESSAGE
                System.Console.WriteLine("SEND LAST MESSAGE TO Roles");
                if (TextBody != string.Empty) if (roleListsRowId > 0) SendEmailToRoleId(Subject, TextBody, sendInHTLMFormat, reportOnClass, reportOnClassRowId, roleListsRowId);
                // delete dataset
                // Tor 20170328 historylog.Update(ds,transaction);
                UpdateGARecordHistoryLogDataset(ds, transaction);
            }

            
            private static void buildBodyAndDetails(RecordHistoryLogDS ds)
            {
                System.Console.WriteLine("build e-mail body part");
                int myMemberClassRowId; // is dbNull for new records
                if (currentIsNewRecord) myMemberClassRowId = 0;
                else myMemberClassRowId = currentMemberClassRowId;// (int)ds.Tables[0].Rows[i]["classMemberRowId"];

                if (currentReportOnClass!= reportOnClass
                    || currentReportOnClassRowId != reportOnClassRowId
                    || messageSent)
                { // get owner and member names
                    TextBody = buildBody(TextBody,currentIsNewRecord,currentReportOnClass,currentReportOnClassRowId,currentOwnerClass,currentOwnerClassRowId,currentMemberClass,
                        myMemberClassRowId);
                    messageSent = false;
                }
                // get record changes
                if (currentOwnerClass != ownerClass
                    || currentOwnerClassRowId != ownerClassRowId
                    || currentMemberClass != memberClass
                    || currentMemberClassRowId != memberClassRowId
                    || myMemberClassRowId != memberClassRowId
                    || reportOnClass != currentReportOnClass // ds.Tables[0].Rows[i]["reportOnClass"].ToString()
                    || reportOnClassRowId != currentReportOnClassRowId // (int)ds.Tables[0].Rows[i]["reportOnClassRowId"]
                    || (currentIsNewRecord && isNewRecord)
                    || currentFieldId != fieldId
                    || currentOldFieldValue != oldFieldValue
                    || currentNewFieldValue != newFieldValue
                    || currentNewRecordHeadingValue != newRecordHeadingValue
                    || currentDateTimeFieldIdChanged != DateTimeFieldIdChanged
                    || currentChangedByPersonnelRowId != ChangedByPersonnelRowId)
                {
                    ownerClass = currentOwnerClass; // ds.Tables[0].Rows[i]["classOwner"].ToString();
                    ownerClassRowId = currentOwnerClassRowId; // (int)ds.Tables[0].Rows[i]["classOwnerRowId"];
                    memberClass = currentMemberClass; // ds.Tables[0].Rows[i]["classMember"].ToString();
                    memberClassRowId = myMemberClassRowId;
                    reportOnClass = currentReportOnClass; // ds.Tables[0].Rows[i]["reportOnClass"].ToString();
                    reportOnClassRowId = currentReportOnClassRowId; // (int)ds.Tables[0].Rows[i]["reportOnClassRowId"];
                    isNewRecord = currentIsNewRecord; // (bool)ds.Tables[0].Rows[i]["isNewRecord"];
                    fieldId = currentFieldId; // ds.Tables[0].Rows[i]["FieldId"].ToString();
                    oldFieldValue = currentOldFieldValue; // ds.Tables[0].Rows[i]["oldAttributeValue"].ToString();
                    newFieldValue = currentNewFieldValue; // ds.Tables[0].Rows[i]["newAttributeValue"].ToString();
                    newRecordHeadingValue = currentNewRecordHeadingValue; // ds.Tables[0].Rows[i]["TextFree1"].ToString();
                    DateTimeFieldIdChanged = currentDateTimeFieldIdChanged; // (DateTime)ds.Tables[0].Rows[i]["DateTimeFieldIdChanged"];
                    ChangedByPersonnelRowId = currentChangedByPersonnelRowId; // (int)ds.Tables[0].Rows[i]["ChangedByPersonnelRowId"];

                    TextBody = buildBodyDetails(TextBody, memberClass, memberClassRowId, fieldId, oldFieldValue, newFieldValue,
                        newRecordHeadingValue, isNewRecord, DateTimeFieldIdChanged, ChangedByPersonnelRowId);
                }
            }

            private static string buildBody(string body, bool isNewRecord, string reportOnClass, int ReportOnClassRowId, string ownerClass, int ownerClassRowId, string memberClass, int memberClassRowId)
            {
                string httpReference = "http://intranet/flag/Default.aspx?tabId=238&";
                if (isNewRecord)
                {
                    httpReference = httpReference + reportOnClass.ToString().Substring(2) + "RowId=" + ReportOnClassRowId.ToString() + "&dataclass=" + reportOnClass.ToString();
                    string reportOnClassName = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("GEN", reportOnClass));
                    string reportOnClassRecordName = GASystem.BusinessLayer.Utils.RecordSetUtils.GetDataRecordName(reportOnClass, ReportOnClassRowId);
                    // get reportOnClass owner record 
                    GADataRecord reportOnClassOwner=GASystem.BusinessLayer.DataClassRelations.GetOwner(new GADataRecord(ReportOnClassRowId,GASystem.DataModel.GADataRecord.ParseGADataClass(reportOnClass)));
                    // get reportOnClass owner header
                    string reportOnClassOwnerName = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("GEN", reportOnClassOwner.DataClass.ToString()));
                    string reportOnClassOwnerRecordName = GASystem.BusinessLayer.Utils.RecordSetUtils.GetDataRecordName(reportOnClassOwner.DataClass.ToString(),reportOnClassOwner.RowId);
                    // Tor 20151215 change to HTML text
                    // return body + "\n\n" + reportOnClassName + ": '" + reportOnClassRecordName + "' under: " + reportOnClassOwnerName + " '" + reportOnClassOwnerRecordName + "' at " + httpReference;
                    return body + "<p>" + reportOnClassName + ": <span style=\"color: rgb(0, 0, 255);\"><strong>"
                        + reportOnClassRecordName + "</strong></span> under: "
                        + reportOnClassOwnerName + ": <span style=\"color: rgb(0, 0, 255);\"><strong> "
                        + reportOnClassOwnerRecordName + "</strong></span> at "
                        + httpReference + "<br />";
                }
                else
                {
                    httpReference = httpReference + memberClass.ToString().Substring(2) + "RowId=" + memberClassRowId.ToString() + "&dataclass=" + memberClass.ToString();
                    string ownerClassName = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("GEN", ownerClass));
                    string ownerRecordName = GASystem.BusinessLayer.Utils.RecordSetUtils.GetDataRecordName(ownerClass, ownerClassRowId);
                    string memberClassName = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("GEN", memberClass));
                    string memberRecordName = GASystem.BusinessLayer.Utils.RecordSetUtils.GetDataRecordName(memberClass, memberClassRowId);
                    // Tor 20151215 change to HTML text
                    // return body + "\n\n" + memberClassName + ": '" + memberRecordName + "' under: " + ownerClassName + " '" + ownerRecordName + "' at " + httpReference;
                    return body+"<p>" + memberClassName + ": <span style=\"color: rgb(0, 0, 255);\"><strong>" 
                        + memberRecordName + "</strong></span> under: " 
                        + ownerClassName + ": <span style=\"color: rgb(0, 0, 255);\"><strong> " 
                        + ownerRecordName + "</strong></span> at " 
                        + httpReference+"<br />";
                }
            }

            // Tor 20150409 added parameters new record heading and isnewrecord parameters
            private static string buildBodyDetails(
                string body, string memberClass, int memberClassRowId, string fieldId, string oldValue, string newValue
                ,string newRecordHeader, bool isNewRecord, DateTime timeOfChange, int ChangedByPersonnelRowId)
            {
                // Tor 20151125 Check if ChangedByPersonnelRowId is > 0
                string personName = "";
                if (ChangedByPersonnelRowId > 0)
                {
                    PersonnelDS pds = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelRowId(ChangedByPersonnelRowId);
                    if (pds.Tables[0].Rows.Count > 0) personName = pds.Tables[0].Rows[0]["FamilyName"] + " " + pds.Tables[0].Rows[0]["GivenName"];
                }
                if (!isNewRecord)
                {
                    string memberRecordName = GASystem.BusinessLayer.Utils.RecordSetUtils.GetDataRecordName(memberClass, memberClassRowId);
                    string fieldName = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("CEN", fieldId));
                    // if Table/Column is dropdownfield
                    string oldValueText = string.Empty;
                    string newValueText = string.Empty;
                    FieldDescription fd = GASystem.AppUtils.FieldDefintion.GetFieldDescription(fieldId, memberClass);
                    if (fd.ListCategory != string.Empty && fd.ControlType.ToString().ToLower() == "dropdownlist")
                    {
                        oldValueText = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(int.Parse(oldValue));
                        newValueText = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(int.Parse(newValue));
                    }
                    else
                        if (fd.LookupTable != string.Empty && fd.ControlType.ToString().ToLower() == "lookupfield")
                        {
                            // get lookupfield value from lookuptable.LookupTableDisplayValue with key old and new values
                            oldValueText = GASystem.BusinessLayer.Utils.RecordSetUtils.GetDataRecordName(fd.LookupTable.ToString(), int.Parse(oldValue));
                            newValueText = GASystem.BusinessLayer.Utils.RecordSetUtils.GetDataRecordName(fd.LookupTable.ToString(), int.Parse(newValue));
                        }
                        else
                        {
                            oldValueText = oldValue;
                            newValueText = newValue;
                        }
                    
                    // Tor 20151202 different text if personName is empty
                    if (personName != "")
                    {
                        // Tor 20151215 change to HTML text
                        //return body + "\n\nField: '" + fieldName + "' was changed\nfrom value: '" + oldValueText + "'\nto value: '" + newValueText + "'\nby: '" + personName + "' " + timeOfChange.ToString() + " (UTC time)";
                        return body
                            + "<br /><strong>Field</strong>: <span style=\"color: rgb(0, 0, 255);\"><strong>"
                            + fieldName + "</strong></span> was changed<br /><strong>from</strong>: <span style=\"color: rgb(0, 0, 255);\"><strong>"
                            + oldValueText + "</strong></span><br /><strong>to: </strong><span style=\"color: rgb(0, 0, 255);\"><strong>"
                            + newValueText + "</strong></span><br /><strong>by</strong>: <span style=\"color: rgb(0, 0, 255);\"><strong>"
                            + personName + "</strong></span> " + timeOfChange.ToString() + " (UTC time)</p>";
                    }
                    else
                    {
                        // Tor 20151215 change to HTML text
                        // return body + "\n\nField: '" + fieldName + "' was changed\nfrom value: '" + oldValueText + "'\nto value: '" + newValueText + "'\n'" + timeOfChange.ToString() + " (UTC time)";
                        return body
                            + "<strong>Field</strong>: <span style=\"color: rgb(0, 0, 255);\"><strong>"
                            + fieldName + "</strong></span> was changed<br /><strong>from</strong>: <span style=\"color: rgb(0, 0, 255);\"><strong>"
                            + oldValueText + "</strong></span><br /><strong>to: </strong><span style=\"color: rgb(0, 0, 255);\"><strong>"
                            + newValueText + "</strong></span><br />"
                            + timeOfChange.ToString() + " (UTC time)</p>";
                    }
                }
                else
                {// new record
                    // get new record GAClass.memberClass ObjectName and ObjectName fieldDescription
                    ClassDescription mcd = ClassDefinition.GetClassDescriptionByGADataClass(memberClass);
                    FieldDescription fd = GASystem.AppUtils.FieldDefintion.GetFieldDescription(mcd.ObjectName.ToString(), memberClass);
                    string myNewRecordHeader = string.Empty;                                
                    if (fd.ListCategory != string.Empty && fd.ControlType.ToString().ToLower() == "dropdownlist")
                    {
                        myNewRecordHeader = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(int.Parse(newRecordHeader));
                    }
                    else
                        if (fd.LookupTable != string.Empty && fd.ControlType.ToString().ToLower() == "lookupfield")
                        {
                            // get lookupfield value from lookuptable.LookupTableDisplayValue with key old and new values
                            myNewRecordHeader = GASystem.BusinessLayer.Utils.RecordSetUtils.GetDataRecordName(fd.LookupTable.ToString(), int.Parse(newRecordHeader));
                        }
                        else
                        {
                            myNewRecordHeader = newRecordHeader;
                        }
                    // Tor 20151202 different text if personName is empty
                    if (personName != "")
                    {
                        // Tor 20151215 change to HTML text
                        //return body + "\n\nNew '" + GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("GEN", memberClass))
                        //    + "' with heading: '" + myNewRecordHeader + "' was added" + "\nby: '" + personName + "' " + timeOfChange.ToString() + " (UTC time)";
                        return body
    + "<strong>New : '"+ GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("GEN", memberClass))
    + "</strong>' with heading: <span style=\"color: rgb(0, 0, 255);\"><strong>"
    + myNewRecordHeader + "</strong></span> was added<br />by: <span style=\"color: rgb(0, 0, 255);\"><strong>"
    + personName + "</strong></span> "+ timeOfChange.ToString() + " (UTC time)</p>";
                    }
                    else
                    {
                        // Tor 20151215 change to HTML text
                        //return body + "\n\nNew '" + GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("GEN", memberClass))
                        //    + "' with heading: '" + myNewRecordHeader + "' was added" + "\n" + timeOfChange.ToString() + " (UTC time)";
                        return body
    + "<strong>New : '" + GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("GEN", memberClass))
    + "</strong>' with heading: <span style=\"color: rgb(0, 0, 255);\"><strong>"
    + myNewRecordHeader + "</strong></span> was added "
    + timeOfChange.ToString() + " (UTC time)</p>";
                    }

                    //New record: 'Notes' with heading: 'xx' was added by: 'Heskje Tor' 09/04/2015 14:54:07 (UTC time)
                }
            }
            
            private static void SendEmailToRoleId(string Subject, string body, bool html, string Class, int ClassRowId, int RoleListsRowId)
            {
                int personnelRowId = 0;
                string toEmailAddresses = string.Empty;
                string toAddress = string.Empty;
                int receivers = 0;
                bool firstAddress = true;
                bool lastAddress = false;
                MailMessage messageObject = new MailMessage();
                // get receivers
                System.Collections.Generic.List<GADataRecord> dataRecordList = 
                    GASystem.BusinessLayer.DataClassRelations.GetCurrentParentLevelDataRecords
                    (new GADataRecord(ClassRowId, GADataRecord.ParseGADataClass(Class)));
// Tor 20170325                EmploymentDS ds = GASystem.BusinessLayer.Employment.SearchEmploymentsByOwnerRecordsDateAndRoleId(dataRecordList, System.DateTime.Now, roleListsRowId);
                EmploymentDS ds = GASystem.BusinessLayer.Employment.SearchEmploymentsByOwnerRecordsDateAndJobTitle(dataRecordList, System.DateTime.UtcNow, roleListsRowId);
                int i = -1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    i++;
                    if (!ds.Tables[0].Rows[i]["Personnel"].Equals(DBNull.Value))
                    {
                        personnelRowId = (int)ds.Tables[0].Rows[i]["Personnel"];
                        toAddress = GASystem.BusinessLayer.FlagSMTP.GetEmailForPersonnelRowId(personnelRowId);
                        /*add address if not already there */
                        if (toEmailAddresses.IndexOf(toAddress) < 0)
                        {
                            toEmailAddresses += toAddress + ',';
                            receivers += 1;
                            //                      GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(firstAddress, lastAddress, toAddress, subject, message, sendInHTLMFormat);
                            GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(messageObject, firstAddress, lastAddress, toAddress, Subject, body, html);
                            firstAddress = false;
                        }
                    }
                }
                if (toEmailAddresses != string.Empty)
                {
//                    firstAddress = true;
                    lastAddress = true;
                    GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(messageObject, firstAddress, lastAddress,toAddress, Subject, body, html);
                    System.Console.WriteLine("Sent mail: " + Subject + " to recipient(s) " + toEmailAddresses);
                }
                else
                {
                    System.Console.WriteLine("No e-mail receiver for: " + Subject);
                }
            }
            private static void InitializeVariables()
            { 
                currentPersonnelRowId=0;
                currentRoleListsRowId=0;
                TextBody = string.Empty;
                ownerClass = string.Empty;
                ownerClassRowId = 0;
                currentOwnerClass = string.Empty;
                currentOwnerClassRowId = 0;
                reportOnClass = string.Empty;
                reportOnClassRowId = 0;
                currentReportOnClass = string.Empty;
                currentReportOnClassRowId = 0;
                memberClass = string.Empty;
                memberClassRowId = 0;
                currentMemberClass = string.Empty;
                currentMemberClassRowId = 0;
                messageSent = false;
                fieldId = string.Empty;
                oldFieldValue = string.Empty;
                newFieldValue = string.Empty;
                currentFieldId = string.Empty;
                currentOldFieldValue = string.Empty;
                currentNewFieldValue = string.Empty;
                DateTimeFieldIdChanged = DateTime.MinValue;
                currentDateTimeFieldIdChanged = DateTime.MinValue;
                ChangedByPersonnelRowId = 0;
                newRecordHeadingValue = string.Empty;
                currentChangedByPersonnelRowId = 0;
                currentNewRecordHeadingValue = string.Empty;
                isNewRecord = false;
                currentIsNewRecord = false;
            }

            private static void UpdateGARecordHistoryLogDataset(DataSet ds, GADataTransaction transaction)
            {

                DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(GADataRecord.ParseGADataClass("GARecordHistoryLog"), transaction);
                DataSet dsresult = gada.Update(ds);
                
            }
    }
}

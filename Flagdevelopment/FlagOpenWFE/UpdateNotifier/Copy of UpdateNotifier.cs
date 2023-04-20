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

            [STAThread]

            static void Main(string[] args)
            {
                int currentPersonnelRowId=0;
                int currentRoleListsRowId=0;
                int dsPersonnelRowId;
                int dsRoleListsRowId;
                string Subject = "Flag form update notification";
                string TextBody = string.Empty;
                bool sendInHTLMFormat = false;
                string ownerClass = string.Empty;
                int ownerClassRowId=0;
                string reportOnClass = string.Empty;
                int reportOnClassRowId = 0;
                string memberClass = string.Empty;
                int memberClassRowId=0;
                bool messageSent = false;
                string fieldId=string.Empty;
                string oldFieldValue=string.Empty;
                string newFieldValue=string.Empty;
                DateTime DateTimeFieldIdChanged=DateTime.MinValue;
                int ChangedByPersonnelRowId=0;
                string newRecordHeadingValue = string.Empty;
                bool isNewRecord=false;

                RecordHistoryLog historylog = new RecordHistoryLog();
                GADataTransaction transaction = GADataTransaction.StartGADataTransaction();

                // read all GAHistory records ordered by reportToPersonnelRowId,reportToRoleListsRowId,reportOnClass,reportOnClassRowId,DateTimeFieldIdChanged,classMember,classMemberRowId,classOwner,classOwnerRowId
                RecordHistoryLogDS ds = RecordHistoryLog.GetAllRecordHistoryLogsSorted();
                int i = -1;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    i++;
                    if (!ds.Tables[0].Rows[i]["reportToPersonnelRowId"].Equals(DBNull.Value))
                        dsPersonnelRowId = (int)ds.Tables[0].Rows[i]["reportToPersonnelRowId"];
                    else dsPersonnelRowId = 0; 
                    if (dsPersonnelRowId>0) // report on person
                    {
                        if(currentPersonnelRowId!=dsPersonnelRowId)
                        {
                            if (currentPersonnelRowId > 0) // new person
                            {
                                // then send previous build e-mail;
                                GASystem.BusinessLayer.FlagSMTP.SendMessage(currentPersonnelRowId, Subject, TextBody, sendInHTLMFormat);
                                messageSent = true;
                                currentPersonnelRowId = dsPersonnelRowId;
                                // build new e-mail message header
                                TextBody = string.Empty;
                                System.Console.WriteLine("build e-mail body part for person");
                            }
                            else
                            { // first person
                                System.Console.WriteLine("first person");
                                if (TextBody != string.Empty && currentRoleListsRowId > 0)
                                {
//                                    SendEmailToRoleId(Subject, TextBody, sendInHTLMFormat, memberClass, memberClassRowId, currentRoleListsRowId);
                                    SendEmailToRoleId(Subject, TextBody, sendInHTLMFormat, reportOnClass, reportOnClassRowId, currentRoleListsRowId);
                                    messageSent = true;
                                }
                                TextBody = string.Empty;
                                currentPersonnelRowId = dsPersonnelRowId;
                                currentRoleListsRowId = 0;
                            }
                        }
                    }
                    else
                    { // report on role
                        if (!ds.Tables[0].Rows[i]["reportToRoleListsRowId"].Equals(DBNull.Value))
                            dsRoleListsRowId = (int)ds.Tables[0].Rows[i]["reportToRoleListsRowId"];
                        else dsRoleListsRowId = 0;

                        if (dsRoleListsRowId >0) // report on role
                        {
                            if(currentRoleListsRowId!=dsRoleListsRowId)
                            {
                                if (currentRoleListsRowId>0) // new role
                                { // send previous build e-mail to previous role;
                                    SendEmailToRoleId(Subject, TextBody, sendInHTLMFormat, memberClass, memberClassRowId, currentRoleListsRowId);
                                    messageSent = true;
                                    TextBody = string.Empty;
                                    currentRoleListsRowId = dsRoleListsRowId;
                                }
                                else
                                { // first role
                                    if (TextBody != string.Empty && currentPersonnelRowId>0)
                                    { // send last message built to Person if TextBody not empty and currentPersonnel > 0
                                        GASystem.BusinessLayer.FlagSMTP.SendMessage(currentPersonnelRowId, Subject, TextBody, sendInHTLMFormat);
                                        messageSent = true;
                                        TextBody = string.Empty;
                                    }
                                    System.Console.WriteLine("first role");
                                    currentRoleListsRowId = dsRoleListsRowId;
                                    currentPersonnelRowId = 0;
                                }
                            }
                        }
                    }
                    //                 buid e-mail body part

                    System.Console.WriteLine("build e-mail body part");
                    int myMemberClassRowId; // is dbNull for new records
                    if ((bool)ds.Tables[0].Rows[i]["isNewRecord"]) myMemberClassRowId = 0;
                    else myMemberClassRowId=(int)ds.Tables[0].Rows[i]["classMemberRowId"];

                    if (ds.Tables[0].Rows[i]["reportOnClass"].ToString() != reportOnClass
                        || (int)ds.Tables[0].Rows[i]["reportOnClassRowId"] != reportOnClassRowId
                        || messageSent)
                    //if (ds.Tables[0].Rows[i]["classOwner"].ToString() != ownerClass
                    //    || ds.Tables[0].Rows[i]["classMember"].ToString() != memberClass
                    //    || (int)ds.Tables[0].Rows[i]["classMemberRowId"] != memberClassRowId
                    //    || (int)ds.Tables[0].Rows[i]["classOwnerRowId"] != ownerClassRowId
                    //    || messageSent)
                    { // get owner and member names
                        TextBody = buildBody(TextBody,
                            (bool)ds.Tables[0].Rows[i]["isNewRecord"],
                            ds.Tables[0].Rows[i]["reportOnClass"].ToString(),
                            (int)ds.Tables[0].Rows[i]["reportOnClassRowId"],
                            ds.Tables[0].Rows[i]["classOwner"].ToString(),
                            (int)ds.Tables[0].Rows[i]["classOwnerRowId"],
                            ds.Tables[0].Rows[i]["classMember"].ToString(),
                            myMemberClassRowId);
//                            (int)ds.Tables[0].Rows[i]["classMemberRowId"]);
                        messageSent = false;
                    }
                    // get record changes
                    if (ds.Tables[0].Rows[i]["classOwner"].ToString() != ownerClass
                        || (int)ds.Tables[0].Rows[i]["classOwnerRowId"] != ownerClassRowId
                        || ds.Tables[0].Rows[i]["classMember"].ToString() != memberClass
                        || (int)ds.Tables[0].Rows[i]["classMemberRowId"] != memberClassRowId
//                        || myMemberClassRowId (int)ds.Tables[0].Rows[i]["classMemberRowId"] != memberClassRowId
                        || myMemberClassRowId != memberClassRowId
                        || reportOnClass != ds.Tables[0].Rows[i]["reportOnClass"].ToString()
                        || reportOnClassRowId != (int)ds.Tables[0].Rows[i]["reportOnClassRowId"]
                        || ((bool)ds.Tables[0].Rows[i]["isNewRecord"] && isNewRecord)
                        || ds.Tables[0].Rows[i]["FieldId"].ToString() != fieldId
                        || ds.Tables[0].Rows[i]["oldAttributeValue"].ToString() != oldFieldValue
                        || ds.Tables[0].Rows[i]["newAttributeValue"].ToString() != newFieldValue
                        || ds.Tables[0].Rows[i]["TextFree1"].ToString() != newRecordHeadingValue
                        || (DateTime)ds.Tables[0].Rows[i]["DateTimeFieldIdChanged"] != DateTimeFieldIdChanged
                        || (int)ds.Tables[0].Rows[i]["ChangedByPersonnelRowId"] != ChangedByPersonnelRowId)
                    {
                        ownerClass = ds.Tables[0].Rows[i]["classOwner"].ToString();
                        ownerClassRowId = (int)ds.Tables[0].Rows[i]["classOwnerRowId"];
                        memberClass = ds.Tables[0].Rows[i]["classMember"].ToString();
                        memberClassRowId=myMemberClassRowId;
                        //if (!ds.Tables[0].Rows[i]["classMemberRowId"].Equals(DBNull.Value)) memberClassRowId = (int)ds.Tables[0].Rows[i]["classMemberRowId"];
                        //else memberClassRowId = 0;
                        reportOnClass = ds.Tables[0].Rows[i]["reportOnClass"].ToString();
                        reportOnClassRowId = (int)ds.Tables[0].Rows[i]["reportOnClassRowId"];
                        isNewRecord = (bool)ds.Tables[0].Rows[i]["isNewRecord"];
                        fieldId = ds.Tables[0].Rows[i]["FieldId"].ToString();
                        oldFieldValue= ds.Tables[0].Rows[i]["oldAttributeValue"].ToString();
                        newFieldValue= ds.Tables[0].Rows[i]["newAttributeValue"].ToString();
                        newRecordHeadingValue= ds.Tables[0].Rows[i]["TextFree1"].ToString();
                        DateTimeFieldIdChanged = (DateTime)ds.Tables[0].Rows[i]["DateTimeFieldIdChanged"];
                        ChangedByPersonnelRowId= (int)ds.Tables[0].Rows[i]["ChangedByPersonnelRowId"];

                        //TextBody = buildBodyDetails(TextBody, memberClass, memberClassRowId
                        //, ds.Tables[0].Rows[i]["FieldId"].ToString()
                        //, ds.Tables[0].Rows[i]["oldAttributeValue"].ToString()
                        //, ds.Tables[0].Rows[i]["newAttributeValue"].ToString()
                        //, ds.Tables[0].Rows[i]["TextFree1"].ToString()
                        //, ds.Tables[0].Rows[i]["isNewRecord"]
                        //, (DateTime)ds.Tables[0].Rows[i]["DateTimeFieldIdChanged"]
                        //, (int)ds.Tables[0].Rows[i]["ChangedByPersonnelRowId"]);
                        TextBody = buildBodyDetails(TextBody, memberClass, memberClassRowId, fieldId, oldFieldValue, newFieldValue,
                            newRecordHeadingValue, isNewRecord, DateTimeFieldIdChanged, ChangedByPersonnelRowId);
                    }
                    
                    row.Delete();
                }
                // SEND LAST MESSAGE
                System.Console.WriteLine("SEND LAST MESSAGE");
                if (TextBody != string.Empty)
                {
                    if (currentPersonnelRowId > 0)
                    {
                        GASystem.BusinessLayer.FlagSMTP.SendMessage(currentPersonnelRowId, Subject, TextBody, sendInHTLMFormat);
                    }
                    else
                        if (currentRoleListsRowId > 0) SendEmailToRoleId(Subject, TextBody, sendInHTLMFormat, memberClass, memberClassRowId, currentRoleListsRowId);
                }

                // delete dataset
                historylog.Update(ds,transaction);
                // commit transaction
                transaction.CommitAndClose();
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
                    return body + "\n\n" +reportOnClassName + ": '" + reportOnClassRecordName + "' under: " + reportOnClassOwnerName+ " '"+reportOnClassOwnerRecordName+"' at " + httpReference;
                }
                else
                {
                    httpReference = httpReference + memberClass.ToString().Substring(2) + "RowId=" + memberClassRowId.ToString() + "&dataclass=" + memberClass.ToString();
                    string ownerClassName = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("GEN", ownerClass));
                    string ownerRecordName = GASystem.BusinessLayer.Utils.RecordSetUtils.GetDataRecordName(ownerClass, ownerClassRowId);
                    string memberClassName = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("GEN", memberClass));
                    string memberRecordName = GASystem.BusinessLayer.Utils.RecordSetUtils.GetDataRecordName(memberClass, memberClassRowId);
                    return body + "\n\n" + memberClassName + ": '" + memberRecordName + "' under: " + ownerClassName + " '" + ownerRecordName + "' at " + httpReference;
                }
            }

            // Tor 20150409 added parameters new record heading and isnewrecord parameters
            private static string buildBodyDetails(
                string body, string memberClass, int memberClassRowId, string fieldId, string oldValue, string newValue
                ,string newRecordHeader, bool isNewRecord, DateTime timeOfChange, int ChangedByPersonnelRowId)
            {
                PersonnelDS pds = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelRowId(ChangedByPersonnelRowId);
                string personName = pds.Tables[0].Rows[0]["FamilyName"] + " " + pds.Tables[0].Rows[0]["GivenName"];
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
                    return body + "\n\nField: '" + fieldName + "' was changed\nfrom value: '" + oldValueText + "'\nto value: '" + newValueText + "'\nby: '" + personName + "' " + timeOfChange.ToString() + " (UTC time)";
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
                    return body + "\n\nNew '" +GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("GEN", memberClass))
                        + "' with heading: '" + myNewRecordHeader+ "' was added" + "\nby: '" + personName + "' " + timeOfChange.ToString() + " (UTC time)";
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

                GADataRecord member = new GADataRecord(ClassRowId, GADataRecord.ParseGADataClass(Class));
                EmploymentDS ds = Employment.SearchForEmploymentsByOwnerDateAndRoleId(member, DateTime.UtcNow, RoleListsRowId);
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
    }
}

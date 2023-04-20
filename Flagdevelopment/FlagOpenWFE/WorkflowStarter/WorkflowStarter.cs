using System;
using openwfe.workitem;
using log4net;
using log4net.Config;
using GASystem.DataModel;
using GASystem;
using GASystem.BusinessLayer;
using GASystem.DataAccess;
using GASystem.DataAccess.Utils;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Data;
using System.Net.Mail;

namespace GASystem.DotNetApre
// starts workflows by reading all records from class GAWorkflowStarter into dataset
// foreach record
// { select * from GAWorkflowStarter where getdate() >= WorkflowNextStartDateTime and WorkFlowStatus="Completed"
// /* if getdate() >= WorkflowNextStartDateTime and WorkFlowStatus="Completed"*/
//      update record set WorkFlowStatus="Running"
//      call ClassMethod (record)
//          two method types:
//              - send e-mail notification (example passport reminder)
//              - start workflow (example document control)
//      update record set
//              WorkflowLastStartedDateTime=getdate(), 
//              WorkflowNextStartDateTime=getdate()+WorkflowStarterRunFrequencyListsRowId
//              WorkFlowStatus="Completed"
// }
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    class WorkflowStarter : impl.SimpleConsumer
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(WorkflowStarter));

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            //
            // TODO: Add code to start application here
            //
            log4net.Config.XmlConfigurator.Configure();  // BasicConfigurator.Configure();

            _logger.Info("starting WorkflowStarter");

            System.Console.WriteLine("--starting--");
            System.Console.WriteLine("Read all WorkflowStarter records to exceute ");

            // Tor 20150501 start
            // read and update records using methods in GASystem.BusinessLayer.WorkflowStarter.cs som benytter GASystem.DataAccess.WorkflowStarterDb.cs
            // 
            string StatusCategory = "WSS";
            string StatusRunning = "Running";
            string StatusCompleted = "Completed";
            string currentClass = string.Empty;
            string myCurrentWorkflowClass = string.Empty;            

            int StatusCompletedRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey(StatusCategory, StatusCompleted);
            int StatusRunningRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey(StatusCategory,StatusRunning);

            // Get all GAWorkflowStarter records where current dateTime >= nextRun date time and workflowstarter status is completed
            WorkflowStarterDS wds = GASystem.BusinessLayer.WorkflowStarter.GetAllCurrentWorkflowStarters();

            int numberOfRecordsRead = wds.GAWorkflowStarter.Rows.Count;
            System.Console.WriteLine("Number of WorkflowStarter records read: " + numberOfRecordsRead.ToString());

            foreach (WorkflowStarterDS.GAWorkflowStarterRow row in wds.GAWorkflowStarter)
            {
                System.Console.WriteLine("Process WorkflowStarter record: "+row.WorkflowDefaultSubject+" with default description: "+row.WorkflowDefaultDescription);
                int rowId = row.WorkflowStarterRowId;
                string myCurrentWorkflowClassDateField = row.ClassDateFieldId;
                System.Console.WriteLine("Transaction Start");
                GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
                try
                {
                    //      update record set WorkFlowStatus="Running" to avoid other processes trying to start the same workflow 
                    GASystem.BusinessLayer.WorkflowStarter.SetWorkflowStarterRunning(rowId, StatusRunningRowId, transaction);
                    // get class name by rowId
                    ClassDS cds = GASystem.BusinessLayer.Class.GetClassByClassRowId(row.ClassRowId);
                    if (cds.GAClass.Rows.Count > 0)
                    {
                        myCurrentWorkflowClass = cds.GAClass[0].Class;
                        row.WorkflowLastStartedDateTime = DateTime.UtcNow;
                        row.WorkflowNextStartDateTime = GASystem.BusinessLayer.WorkflowStarter.GetNextDate(row.WorkflowStarterRunFrequencyListsRowId, row.WorkflowLastStartedDateTime, "add");
                        if (!row.IsTextFree3Null() && row.TextFree3 == "DefaultNotifier")
                        {
                            DefaultNotifier notifier = new DefaultNotifier(row, cds);
                            notifier.Notify();
                        }
                        else
                        {
                            switch (myCurrentWorkflowClass)
                            {
                                case "GADocumentControl":
                                    doDocumentControlWorkflowStart(myCurrentWorkflowClass, transaction);
                                    break;
                                case "GASubcontractorView":
                                    doSubcontractorViewWorkflowStart(myCurrentWorkflowClass, transaction);
                                    break;
                                case "GAPassportVisaView":
                                    doHealthPassportVisaViewEmailNotifications(myCurrentWorkflowClass, transaction);
                                    break;
                                case "GACoursePersonListView":
                                    doCoursePersonListViewEmailNotifications(myCurrentWorkflowClass, transaction);
                                    break;
                                case "GARisk":
                                    if (row.WorkflowDefaultSubject == "RA verification process")
                                    {
                                        doRiskVerifyWorkflowStart(myCurrentWorkflowClass, transaction);
                                        break;
                                    }
                                    else 
                                    {
                                        if (row.WorkflowDefaultSubject == "New Hazard Assessment Revision")
                                        {
                                            doRiskWorkflowStart(myCurrentWorkflowClass, transaction);
                                            break;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }                                                                        
                                case "GAPersonnel":
                                    doPersonnelEmailNotifications(myCurrentWorkflowClass, transaction);
                                    break;
                                case "GAManageChange":
                                    doMOCEmailNotifications(myCurrentWorkflowClass, transaction);
                                    break;
                                case "GAVendor":
                                    doVendorWorkflowStart(myCurrentWorkflowClass, transaction);
                                    break;
                                case "GAProjectRisk":
                                    doProjectRiskWorkflowStart(myCurrentWorkflowClass, transaction);
                                    break;
                                //case "GAPassportVisaView":
                                //    doPassportVisaViewWorkflowStart();
                                //    break;
                                default:
                                    break;
                            }                            
                        }
                        // updates record: sets WorkflowLastStartedDateTime=getdate(), WorkflowNextStartDateTime=getdate()+WorkflowStarterRunFrequencyListsRowId, WorkFlowStatus="Completed"
                        GASystem.BusinessLayer.WorkflowStarter.SetWorkflowStarterCompleted(rowId, StatusCompletedRowId, transaction);
                        System.Console.WriteLine("Transaction Commit");
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    System.Console.WriteLine("Transaction Rollback");
                    transaction.Rollback();
                    throw ex;
                }
                finally
                {
                    transaction.Connection.Close();
                }
            }        
                // Tor 20140510 Og her burde vi vel sette end transaction ?
            System.Console.WriteLine("Run Complete, program terminates");
        }
        

        // Tor 20150502
        private static void doSubcontractorViewWorkflowStart(string myCurrentWorkflowClass)
        {
            doSubcontractorViewWorkflowStart(myCurrentWorkflowClass, null);
        }

        private static void doSubcontractorViewWorkflowStart(string myCurrentWorkflowClass, GADataTransaction transaction)
        {
            ProcedureDS pds = GASystem.BusinessLayer.Procedure.GetProcedureForGADataClass(myCurrentWorkflowClass);
            if (pds.Tables.Count < 1) 
            {
                System.Console.WriteLine("No procedure for class " + myCurrentWorkflowClass + ". Processing workflows for this class stops.");
                return;
            }
            SubcontractorViewDS sds = GASystem.BusinessLayer.SubcontractorView.GetAllSubcontractorViewToStart(myCurrentWorkflowClass, transaction);
            System.Console.WriteLine("Number of SubcontractorView read is: " + sds.GASubcontractorView.Rows.Count.ToString());
            if (sds!=null)
            {
                sds = GASystem.BusinessLayer.SubcontractorView.UpdateSubcontractorsAndCreateGAAction(sds, pds, transaction);
            }
        }

        // Added by Gao Peng for ProjectRisk WorkflowStarter 20230321
        private static void doProjectRiskWorkflowStart(string myCurrentWorkflowClass)
        { 
            doProjectRiskWorkflowStart(myCurrentWorkflowClass, null);
        }

        // Added by Gao Peng for ProjectRisk WorkflowStarter 20230321
        private static void doProjectRiskWorkflowStart(string myCurrentWorkflowClass, GADataTransaction transaction)
        {
            // returns all Project Risks where workflow is not running, the sql script will filter 
            ProjectRiskDS ds = GASystem.BusinessLayer.ProjectRisk.GetAllProjectRiskToStart(transaction);

            System.Console.WriteLine("Number of GAProjectRisk  read is: " + ds.GAProjectRisk.Rows.Count.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                //There is no status for PRA Verification so comment out below part
                //string StatusCategory = "DCDS";
                //string StatusNextRevisionStarted = "NextRevisionStarted";   
                string userName = string.Empty;
                

                // get Project Risk workflow Procedure dataset
                ProcedureDS pds = GASystem.BusinessLayer.Procedure.GetProcedureForGADataClass(myCurrentWorkflowClass);
                if (pds.Tables.Count < 1)
                {
                    System.Console.WriteLine("No procedure for class " + myCurrentWorkflowClass + ". Processing workflows for this class stops.");
                    return;
                }

                // get procedure priority
                int PriorityRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("PR", "Normal priority");
                int ActionStatusRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ST", "Open");
                // get person with default role from web.config if there are no current assignments in with projectrisk.revisionresponsibelerole
                string defaultRole = GetCoordinatorWorkitemRole();
                // Tor 20170325 Job Title (Role) category moved from ER to TITL
                //int defaultRoleRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ER", defaultRole);
                int defaultRoleRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", defaultRole);

                int defaultPersonnelRowId = GetCoordinatorWorkitemPersonnelRowId();

                foreach (ProjectRiskDS.GAProjectRiskRow row in ds.GAProjectRisk)
                {
                    // set conditions or exceptions for workflow being disabled
                    GADataRecord Project = DataClassRelations.GetOwner(new GADataRecord(row.ProjectRiskRowId, GADataClass.GAProjectRisk));
                    BusinessClass ProjectBC = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(Project.DataClass);
                    System.Data.DataSet dsProject = ProjectBC.GetByRowId(Project.RowId);

                    DateTime toDate = DateTime.MaxValue;

                    // System.Console.WriteLine(DateTime.Parse(DateTime.UtcNow.Date.ToString("D")));

                    // get the value of Project Phase
                    int ProjectStatus = Convert.ToInt32(dsProject.Tables[0].Rows[0]["CreatedBy"]);
                    int ProjectRowId = Convert.ToInt32(dsProject.Tables[0].Rows[0]["ProjectRowId"]);
                    int ProjectRiskRowId = row.ProjectRiskRowId;
                    // System.Console.WriteLine("Project Status:" + ProjectStatus.ToString() + ". with its rowid as " + ProjectRowId.ToString());
                    // System.Console.WriteLine("Project Risk Rowid:" + ProjectRiskRowId.ToString());
                    int MobPhase = 9029; // 9029 is the listsrowid of Mobilization phase in GALists
                    int MobTrigger = 0;
                    string nextrun = "6M";

                    // Gao 20230408 start workflow by checking projectphase and projectrisk.date and projectrisk.intfree1
                    if (ProjectStatus != MobPhase)
                    {
                        if (row.IsDateNull())
                        {
                            continue;
                        }
                        else
                        {
                            if (GASystem.BusinessLayer.WorkflowStarter.GetNextDate(nextrun, row.Date, "add") > DateTime.UtcNow.Date)
                            {
                                continue;
                            }
                        }
                    }
                    else
                    {
                        if (!row.IsIntFree1Null() && row.IntFree1 == 1)
                        {
                            continue;
                        }
                    }


                    // Gao 20230408 if workflow is trigger by phase change (to mob), update intfree to 1 so that it will be activated again
                    if( ProjectStatus == MobPhase)
                    {
                        row.IntFree1 = 1;
                    }

                    GADataTransaction Sctransaction = GADataTransaction.StartGADataTransaction();
                    try
                    {
                        System.Console.WriteLine("Create Workflow for GAPojectRisk verification Assessment with refence id: " + row.ReferenceId);

                        int PersonnelRowId = 0;
                        int myResponsibleRoleListsRowId = 0;
                        // get first person with Responsible Job Title, default
                        if (!row.IsCMVbyJobTitleListsRowIdNull())
                        {
                            PersonnelRowId = GetPersonnelRowIdWithTitle(row.CMVbyJobTitleListsRowId, row.ProjectRiskRowId, myCurrentWorkflowClass);
                            if (PersonnelRowId != 0)
                            {
                                myResponsibleRoleListsRowId = row.CMVbyJobTitleListsRowId;
                            }
                        }
                        PersonnelRowId = defaultPersonnelRowId;
                        userName = GetUserName(PersonnelRowId);
                        //UserDS uds = new UserDS();
                        //uds = GASystem.BusinessLayer.User.GetUserByPersonnelRowId(PersonnelRowId);
                        //if (uds.Tables.Count > 0) userName = uds.GAUser[0].DNNUserId;

                        GADataRecord owner = new GADataRecord(row.ProjectRiskRowId, GADataRecord.ParseGADataClass(myCurrentWorkflowClass));
                        Action action = new Action();
                        ActionDS ads = (ActionDS)action.GetNewRecord(owner, Sctransaction);
                        ActionDS.GAActionRow newRow = (ActionDS.GAActionRow)ads.GAAction.Rows[0];

                        GASystem.BusinessLayer.Utils.IdGenerator idgen = new GASystem.BusinessLayer.Utils.IdGenerator(GADataClass.GAAction, owner, Sctransaction);
                        idgen.ApplyReferenceId(ads);

                        newRow.ActionName = pds.GAProcedure[0].Shortname;
                        newRow.ProcedureRowId = pds.GAProcedure[0].ProcedureRowId;
                        newRow.Description = pds.GAProcedure[0].Description;
                        newRow.Subject = pds.GAProcedure[0].Shortname;
                        newRow.ActionStatusListsRowId = ActionStatusRowId;
                        newRow.PriorityListsRowId = PriorityRowId;
                        newRow.ResponsibleRoleListsRowId = myResponsibleRoleListsRowId > 0 ? myResponsibleRoleListsRowId : defaultRoleRowId;
                        newRow.RegisteredByPersonnelRowId = defaultPersonnelRowId;
                        newRow.WorkflowId = "StartPending:" + userName;
                        newRow.DateEndEstimated = row.IsDateNull() ? DateTime.Now.Date.AddDays(7) : row.Date.AddDays(7);
                        // if (newRow.DateEndEstimated < DateTime.Now.Date.AddDays(1)) newRow.DateEndEstimated = DateTime.Now.Date.AddDays(1); 
                        DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(GADataClass.GAAction, Sctransaction);
                        DataSet dsresult = gada.Update(ads);
                        DataClassRelations.CreateDataClassRelation(owner.RowId, owner.DataClass, (int)dsresult.Tables["GAAction"].Rows[0]["ActionRowId"], GADataClass.GAAction, Sctransaction);
                        
                        // update dataset set Current date as Verified Date in PRA
                        row.Date = DateTime.UtcNow.Date;
                        
                        GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAProjectRisk);
                        bc.UpdateDataSet(ds, Sctransaction);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Transaction Rollback for " + row.ReferenceId + " RowId: " + row.ProjectRiskRowId.ToString());
                        Sctransaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        Sctransaction.Commit();
                        Sctransaction.Connection.Close();
                    }
                }
            }

        }

        // Added by Gao Peng for Vendor Workflowstarter 20210927
        private static void doVendorWorkflowStart(string myCurrentWorkflowClass)
        {
            doVendorWorkflowStart(myCurrentWorkflowClass, null);
        }
        
        // Added by Gao Peng for Vendor Workflowstarter 20210927
        private static void doVendorWorkflowStart(string myCurrentWorkflowClass, GADataTransaction transaction)
        {
            // returns all Vendors where workflow is not running and last or next revision dates are not null
            VendorDS ds = GASystem.BusinessLayer.Vendor.GetAllVendorsToStart(transaction);
            
            System.Console.WriteLine("Number of Vendors read is: " + ds.GAVendor.Rows.Count.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                string userName = string.Empty;
                
                // get Vendor Procedure dataset
                ProcedureDS pds = GASystem.BusinessLayer.Procedure.GetProcedureForGADataClass(myCurrentWorkflowClass);
                if (pds.Tables.Count < 1)
                { 
                    System.Console.WriteLine("No procedure for class " + myCurrentWorkflowClass + ". Processing workflows for this class stops.");
                    return;
                }
                
                // get procedure priority
                int PriorityRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("PR", "Normal priority");
                int ActionStatusRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ST", "Open");
                // get defaultrole who is the owner of the workitem, now is Chief Legal Officer      
                string defaultRole = GetCoordinatorWorkitemRole();
                int defaultRoleRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", defaultRole);
                int defaultPersonnelRowId = GetCoordinatorWorkitemPersonnelRowId();
                string nextrun = "M";

                foreach (VendorDS.GAVendorRow row in ds.GAVendor)
                {
                    // start workflow if renewal date - 1month <= today
                    if (!row.IsDateChangedNull() && GASystem.BusinessLayer.WorkflowStarter.GetNextDate(nextrun, row.DateChanged, "subtract") < DateTime.UtcNow.Date)
                    {
                        GADataTransaction Sctransaction = GADataTransaction.StartGADataTransaction();
                        try
                        {
                            System.Console.WriteLine("Create Workflow for GAVendor record with name: " + row.VendorName);
                            row.DateChanged = DateTime.UtcNow.Date;
                            int PersonnelRowId = 0;

                            // get first person with role:  BGPO Responsible
                            if (!row.IsBGPOContactListsRowIdNull()) //BGPO Contact
                            {
                                PersonnelRowId = row.BGPOContactListsRowId;
                            }
                            else
                            {
                                PersonnelRowId = defaultPersonnelRowId;
                            }

                            userName = GetUserName(PersonnelRowId);

                            GADataRecord owner = new GADataRecord(row.VendorRowId, GADataRecord.ParseGADataClass(myCurrentWorkflowClass));
                            Action action = new Action();
                            ActionDS ads = (ActionDS)action.GetNewRecord(owner, Sctransaction);
                            ActionDS.GAActionRow newRow = (ActionDS.GAActionRow)ads.GAAction.Rows[0];

                            newRow.ActionName = pds.GAProcedure[0].Shortname;

                            GASystem.BusinessLayer.Utils.IdGenerator idgen = new GASystem.BusinessLayer.Utils.IdGenerator(GADataClass.GAAction, owner, Sctransaction);
                            idgen.ApplyReferenceId(ads);

                            newRow.ProcedureRowId = pds.GAProcedure[0].ProcedureRowId;
                            newRow.Description = pds.GAProcedure[0].Description;
                            newRow.Subject = pds.GAProcedure[0].Shortname;
                            newRow.ActionStatusListsRowId = ActionStatusRowId;
                            newRow.PriorityListsRowId = PriorityRowId;
                            newRow.Responsible = PersonnelRowId > 0 ? PersonnelRowId : defaultPersonnelRowId;

                            newRow.WorkflowId = "StartPending:" + userName;                          

                            DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(GADataClass.GAAction, Sctransaction);
                            DataSet dsresult = gada.Update(ads);
                            DataClassRelations.CreateDataClassRelation(owner.RowId, owner.DataClass, (int)dsresult.Tables["GAAction"].Rows[0]["ActionRowId"], GADataClass.GAAction, Sctransaction); 

                            row.DateChanged = DateTime.UtcNow.Date.AddYears(1);
                       
                            //update dataset
                            GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAVendor);
                            //bc.UpdateDataSt(ds, Sctransaction);
                            bc.UpdateDataSet(ds, Sctransaction);
                        }
                        catch(Exception ex)
                        {
                            System.Console.WriteLine("Transaction Rollback for " + row.VendorReferenceId + " RowId: " + row.VendorRowId.ToString()
                                + " ex.Message: " + ex.Message);
                            Sctransaction.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            Sctransaction.Commit();
                            Sctransaction.Connection.Close();
                        }
                    }
                }

            }
            
        }       


        private static void doDocumentControlWorkflowStart(string myCurrentWorkflowClass)
        {
            doDocumentControlWorkflowStart(myCurrentWorkflowClass, null);
        }

        private static void doDocumentControlWorkflowStart(string myCurrentWorkflowClass, GADataTransaction transaction)
        {
            // returns all documents where workflow is not running and last or next revision dates are not null
            DocumentControlDS ds = GASystem.BusinessLayer.DocumentControl.GetAllDocumentControlsToStart(transaction);
            
            System.Console.WriteLine("Number of DocumentControl documents read is: " + ds.GADocumentControl.Rows.Count.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                string StatusCategory = "DCDS";
                string StatusNextRevisionStarted = "NextRevisionStarted";
                string userName = string.Empty;

                // get Document Control Procedure dataset
                ProcedureDS pds = GASystem.BusinessLayer.Procedure.GetProcedureForGADataClass(myCurrentWorkflowClass);
                if (pds.Tables.Count < 1)
                {
                    System.Console.WriteLine("No procedure for class " + myCurrentWorkflowClass + ". Processing workflows for this class stops.");
                    return;
                }
                // get procedure priority
                int PriorityRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("PR", "Normal priority");
                int ActionStatusRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ST", "Open");
                // get person with default role from web.config if there are no current assignments in with documentcontrol.revisionresponsibelerole
                string defaultRole = GetCoordinatorWorkitemRole();
                // Tor 20170325 Job Title (Role) category moved from ER to TITL
                //int defaultRoleRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ER", defaultRole);
                int defaultRoleRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", defaultRole);

                int defaultPersonnelRowId = GetCoordinatorWorkitemPersonnelRowId();
                foreach (DocumentControlDS.GADocumentControlRow row in ds.GADocumentControl)
                {
                    // start workflow if next - prior or last + frequency <= today
                    if ((!row.IsNextRevisionDateNull() && GASystem.BusinessLayer.WorkflowStarter.GetNextDate(row.AutoWorkflowStartPriorToNextRevisionListsRowId, row.NextRevisionDate, "subtract") <= DateTime.UtcNow.Date)
                        || (!row.IsLastRevisionStartedDateNull() && row.IsNextRevisionDateNull() && GASystem.BusinessLayer.WorkflowStarter.GetNextDate(row.RevisionFrequencyListsRowId, row.LastRevisionStartedDate, "add") <= DateTime.UtcNow.Date))
                    {
                        GADataTransaction Sctransaction = GADataTransaction.StartGADataTransaction();
                        try
                        {
                            System.Console.WriteLine("Create Workflow for GADocumentControl document with name: " + row.DocumentName);
                            row.LastRevisionStartedDate = GASystem.BusinessLayer.WorkflowStarter.GetNextDate(row.AutoWorkflowStartPriorToNextRevisionListsRowId, DateTime.UtcNow.Date, "add");
                            row.LastRevisionWorkflowStartDate = DateTime.UtcNow.Date;
                            row.NextRevisionDate = GASystem.BusinessLayer.WorkflowStarter.GetNextDate(row.RevisionFrequencyListsRowId, row.LastRevisionStartedDate, "add");

                            int PersonnelRowId = 0;
                            int myResponsibleRoleListsRowId = 0;
                            // get first person with role:  ProcessResponsible, RevisionResponsible, ApprovalResponsible, default
                            if (!row.IsIntFree1Null()) //Document Review Manager
                            {
                                PersonnelRowId = GetPersonnelRowIdWithTitle(row.IntFree1, row.DocumentControlRowId, myCurrentWorkflowClass);
                                if (PersonnelRowId != 0) myResponsibleRoleListsRowId = row.IntFree1;
                            }
                            if ((PersonnelRowId == 0) && !row.IsRevisionResponsibleRoleListsRowIdNull()) //Document Owner
                            {
                                PersonnelRowId = GetPersonnelRowIdWithTitle(row.RevisionResponsibleRoleListsRowId, row.DocumentControlRowId, myCurrentWorkflowClass);
                                if (PersonnelRowId != 0) myResponsibleRoleListsRowId = row.RevisionResponsibleRoleListsRowId;
                            }
                            if (PersonnelRowId == 0 && !row.IsApprovalResponsibleRoleListsRowIdNull()) //Document Approver
                            {
                                PersonnelRowId = GetPersonnelRowIdWithTitle(row.ApprovalResponsibleRoleListsRowId, row.DocumentControlRowId, myCurrentWorkflowClass);
                                if (PersonnelRowId != 0) myResponsibleRoleListsRowId = row.ApprovalResponsibleRoleListsRowId;
                            }
                            if (PersonnelRowId == 0) PersonnelRowId = defaultPersonnelRowId;

                            // Tor 20191209
                            //UserDS uds = new UserDS();
                            //uds = GASystem.BusinessLayer.User.GetUserByPersonnelRowId(PersonnelRowId);
                            //if (uds.Tables.Count > 0) userName = uds.GAUser[0].DNNUserId;
                            userName = GetUserName(PersonnelRowId);

                            GADataRecord owner = new GADataRecord(row.DocumentControlRowId, GADataRecord.ParseGADataClass(myCurrentWorkflowClass));
                            Action action = new Action();
                            ActionDS ads = (ActionDS)action.GetNewRecord(owner, Sctransaction);
                            ActionDS.GAActionRow newRow = (ActionDS.GAActionRow)ads.GAAction.Rows[0];

                            string myRevisionResponsible = GASystem.BusinessLayer.Lists.GetListValueByRowId(row.RevisionResponsibleRoleListsRowId);
                            string myRevisionFrequency = GASystem.BusinessLayer.Lists.GetListValueByRowId(row.RevisionFrequencyListsRowId);

                            newRow.ActionName = pds.GAProcedure[0].Shortname;

                            GASystem.BusinessLayer.Utils.IdGenerator idgen = new GASystem.BusinessLayer.Utils.IdGenerator(GADataClass.GAAction, owner,Sctransaction);
                            idgen.ApplyReferenceId(ads);

                            newRow.ProcedureRowId = pds.GAProcedure[0].ProcedureRowId;
                            newRow.Description = pds.GAProcedure[0].Description;
                            newRow.Subject = pds.GAProcedure[0].Shortname;
                            newRow.ActionStatusListsRowId = ActionStatusRowId;
                            newRow.PriorityListsRowId = PriorityRowId;
                            //newRow.ResponsibleRoleListsRowId = myResponsibleRoleListsRowId;
                            newRow.ResponsibleRoleListsRowId = myResponsibleRoleListsRowId > 0 ? myResponsibleRoleListsRowId : defaultRoleRowId;

                            newRow.RegisteredByPersonnelRowId = PersonnelRowId; 
                            newRow.WorkflowId = "StartPending:" + userName;
                            newRow.DateEndEstimated = row.LastRevisionStartedDate;

                            DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(GADataClass.GAAction, Sctransaction);
                            DataSet dsresult = gada.Update(ads);
                            DataClassRelations.CreateDataClassRelation(owner.RowId, owner.DataClass, (int)dsresult.Tables["GAAction"].Rows[0]["ActionRowId"], GADataClass.GAAction, Sctransaction);
                            row.DocumentRevisionStatusListsRowId = GASystem.BusinessLayer.WorkflowStarter.GetListsRowId(StatusCategory, StatusNextRevisionStarted);
                            // update dataset
                            GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GADocumentControl);
                            bc.UpdateDataSet(ds, Sctransaction);
                        }
                        catch (Exception ex)
                        {
                            System.Console.WriteLine("Transaction Rollback for " + row.DocumentControlReferenceId + " RowId: " + row.DocumentControlRowId.ToString()
                                +" ex.Message: "+ex.Message);
                            Sctransaction.Rollback();
                            throw ex;
                        }
                        finally
                        {
                            Sctransaction.Commit();
                            Sctransaction.Connection.Close();
                        }
                    }
                }
            }
        }


        //  gao 20230404 another workflow starter program for table GARisk, used for RA verification process
        private static void doRiskVerifyWorkFlowStart(string myCurrentWorkflowClass)
        {
            doRiskVerifyWorkflowStart(myCurrentWorkflowClass, null);
        }

        //  gao 20230404 another workflow starter program for table GARisk, used for RA verification process
        private static void doRiskVerifyWorkflowStart(string myCurrentWorkflowClass, GADataTransaction transaction)
        {
            RiskDS ds = GASystem.BusinessLayer.Risk.GetAllRiskVerifyToStart(transaction);
            // returns all GARisk records where verified date is not null and verify job title is not null and owner location is not archived and its to date is current
            System.Console.WriteLine("Number of Risk Assessment to be verified read is: " + ds.GARisk.Rows.Count.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {

                string userName = string.Empty;

                // get Risk Verify Procedure dataset 20230403
                ProcedureDS pds = GASystem.BusinessLayer.Procedure.GetSecondProcedureForGADataClass(myCurrentWorkflowClass);
                if (pds.Tables.Count < 1)
                {
                    System.Console.WriteLine("No procedure for class " + myCurrentWorkflowClass + ". Processing workflows for this class stops.");
                    return; // stop this method doRiskVerifyWorkflowStart()
                }
                // get procedure priority
                int PriorityRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("PR", "Normal priority");
                int ActionStatusRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ST", "Open");
                // get person with default role from web.config if there are no current assignments in with documentcontrol.revisionresponsibelerole
                string defaultRole = GetCoordinatorWorkitemRole();
                // Tor 20170325 Job Title (Role) category moved from ER to TITL
                int defaultRoleRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", defaultRole);

                int defaultPersonnelRowId = GetCoordinatorWorkitemPersonnelRowId();

                foreach (RiskDS.GARiskRow row in ds.GARisk)
                {
                    string nextrun = "6M";                    
                    DateTime verifydate = DateTime.MaxValue;
                    int marker = 0;
                    int projectrowid = 0;
                    int projectphase = 0;
                    int locationincrewrowid = 0;
                    int crewinprojectrowid = 0;

                    //  System.Console.WriteLine("Risk Refid:" + row.RiskReferenceId);
                    //  System.Console.WriteLine("Risk rowid:" + row.RiskRowId);
                    //  System.Console.WriteLine("verify date:" + verifydate);

                    //  project which owns this location enters Mobilizatino will 发起
                    //  get owner location and its rowid
                    GADataRecord location = DataClassRelations.GetOwner(new GADataRecord(row.RiskRowId, GADataClass.GARisk));
                    BusinessClass locationBC = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(location.DataClass);
                    System.Data.DataSet dsLocation = locationBC.GetByRowId(location.RowId);
                    int locationrowid = (int)dsLocation.Tables[0].Rows[0]["LocationRowId"];
                    System.Console.WriteLine("Location Id:" + locationrowid);

                    //  gao 20240404 get crew rowid which this location is currently assigned to
                    locationincrewrowid = GASystem.BusinessLayer.LocationInCrew.GetAllLocationInCrew(locationrowid);
                    if (locationincrewrowid != 0)
                    {
                        GADataRecord crew = DataClassRelations.GetOwner(new GADataRecord(locationincrewrowid, GADataClass.GALocationInCrew));
                        BusinessClass crewBC = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(crew.DataClass);
                        System.Data.DataSet dsCrew = crewBC.GetByRowId(crew.RowId);
                        int crewrowid = (int)dsCrew.Tables[0].Rows[0]["CrewRowId"];
                        System.Console.WriteLine("Crew Id:" + crewrowid);

                        //  gao 20240404 get project rowid which this crew is currently assigned to
                        crewinprojectrowid = GASystem.BusinessLayer.CrewInProject.GetAllCrewInProject(crewrowid);
                        if (crewinprojectrowid != 0)
                        {
                            GADataRecord project = DataClassRelations.GetOwner(new GADataRecord(crewinprojectrowid, GADataClass.GACrewInProject));
                            BusinessClass projectBC = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(project.DataClass);
                            System.Data.DataSet dsProject = projectBC.GetByRowId(project.RowId);
                            projectrowid = (int)dsProject.Tables[0].Rows[0]["ProjectRowId"];
                            // System.Console.WriteLine("Project Id:" + projectrowid);
                            if (dsProject.Tables[0].Rows[0]["CreatedBy"] != null)
                            {
                                projectphase = (int)dsProject.Tables[0].Rows[0]["CreatedBy"];
                            }
                            else
                            {
                                projectphase = 0;
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("crew is assigned to no project");
                        }
                    }
                    else
                    {
                        System.Console.WriteLine("location is assigned to no crew");
                    }




                    // gao 20240404 get project phase based on its rowid

                    System.Console.WriteLine("Project phase:" + projectphase.ToString());

                    // start workflow if Verified Date + 6M != today 不发起
                    // 
                    

                    //DateTime currentdate = Convert.ToDateTime(DateTime.UtcNow.ToString("yyyy-mm-dd"));
                    //DateTime verifydatecompare = Convert.ToDateTime(verifydate.ToString("yyyy-mm-dd"));

                    if (projectphase != 9029 )
                    {
                        if (!row.IsDateTimeFree2Null())
                        {
                            //  Verified Date + 6M = today then start
                            if (DateTime.UtcNow.Date > GASystem.BusinessLayer.WorkflowStarter.GetNextDate(nextrun, row.DateTimeFree2, "add"))
                            {
                                System.Console.WriteLine("workflow starts due to date time match");
                            }
                            else
                            {
                                continue;
                            }
                        }
                        else
                        {
                            continue;
                        }                       
                    }
                    else
                    {
                        // gao 20230410 if it has been trigger by projectphase entering Mob, skip
                        if (!row.IsMasterRowIdNull() && row.MasterRowId == 1)
                        {
                            continue;
                        }
                        else
                        {
                            System.Console.WriteLine("workflow starts due to mob phase");
                            marker = 1;
                        }
                    }                   
                                       

                    GADataTransaction Sctransaction = GADataTransaction.StartGADataTransaction();
                    try
                    {
                        System.Console.WriteLine("Create Workflow for Risk Assessment verification with refence id: " + row.RiskReferenceId);


                        int PersonnelRowId = 0;
                        int myResponsibleRoleListsRowId = 0;
                        // get first person with Responsible Job Title, default
                        if (!row.IsCreatedByNull())
                        {
                            PersonnelRowId = GetPersonnelRowIdWithTitle(row.CreatedBy, row.RiskRowId, myCurrentWorkflowClass);
                            if (PersonnelRowId != 0) myResponsibleRoleListsRowId = row.CreatedBy;
                        }
                        if (PersonnelRowId == 0) PersonnelRowId = defaultPersonnelRowId;

                        userName = GetUserName(PersonnelRowId);

                        GADataRecord owner = new GADataRecord(row.RiskRowId, GADataRecord.ParseGADataClass(myCurrentWorkflowClass));
                        Action action = new Action();
                        ActionDS ads = (ActionDS)action.GetNewRecord(owner, Sctransaction);
                        ActionDS.GAActionRow newRow = (ActionDS.GAActionRow)ads.GAAction.Rows[0];

                        string myRevisionResponsible = GASystem.BusinessLayer.Lists.GetListValueByRowId(row.ResponsibleRoleListsRowId);

                        newRow.ActionName = pds.GAProcedure[0].Shortname;

                        GASystem.BusinessLayer.Utils.IdGenerator idgen = new GASystem.BusinessLayer.Utils.IdGenerator(GADataClass.GAAction, owner, Sctransaction);
                        idgen.ApplyReferenceId(ads);

                        newRow.ProcedureRowId = pds.GAProcedure[0].ProcedureRowId;
                        newRow.Description = pds.GAProcedure[0].Description;
                        newRow.Subject = pds.GAProcedure[0].Shortname;
                        newRow.ActionStatusListsRowId = ActionStatusRowId;
                        newRow.PriorityListsRowId = PriorityRowId;
                        newRow.ResponsibleRoleListsRowId = myResponsibleRoleListsRowId;
                        newRow.RegisteredByPersonnelRowId = defaultPersonnelRowId;
                        newRow.WorkflowId = "StartPending:" + userName;
                        newRow.DateEndEstimated = (!row.IsDateTimeFree2Null()) ? row.DateTimeFree2.AddDays(7) : DateTime.Now.Date.AddDays(7);
                        if (newRow.DateEndEstimated < DateTime.Now.Date.AddDays(1)) newRow.DateEndEstimated = DateTime.Now.Date.AddDays(1);
                        DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(GADataClass.GAAction, Sctransaction);
                        DataSet dsresult = gada.Update(ads);
                        DataClassRelations.CreateDataClassRelation(owner.RowId, owner.DataClass, (int)dsresult.Tables["GAAction"].Rows[0]["ActionRowId"], GADataClass.GAAction, Sctransaction);

                        // set date next revision
                        row.DateTimeFree2 = DateTime.Now.Date;
                        if (marker == 1)
                        {
                            row.MasterRowId = 1;
                        }

                        // update dataset
                        GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GARisk);
                        bc.UpdateDataSet(ds, Sctransaction);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Transaction Rollback for " + row.RiskReferenceId + " RowId: " + row.RiskRowId.ToString());
                        Sctransaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        Sctransaction.Commit();
                        Sctransaction.Connection.Close();
                    }
                }
            }
        }

        private static void doRiskflowStart(string myCurrentWorkflowClass)
        {
            doRiskWorkflowStart(myCurrentWorkflowClass, null);
        }

        // Tor 20190222 Added method
        //  Gao 20230411 amended method
        private static void doRiskWorkflowStart(string myCurrentWorkflowClass, GADataTransaction transaction)
        {
            RiskDS ds=GASystem.BusinessLayer.Risk.GetAllRiskToStart(transaction);
            // returns all GARisk records where workflow is not running and last revision dates are not null
            System.Console.WriteLine("Number of Hazards  read is: " +ds.GARisk.Rows.Count.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                string StatusCategory = "DCDS";
                string StatusNextRevisionStarted = "NextRevisionStarted";
                string userName = string.Empty;

                // get Document Control Procedure dataset
                ProcedureDS pds = GASystem.BusinessLayer.Procedure.GetProcedureForGADataClass(myCurrentWorkflowClass);
                if (pds.Tables.Count < 1)
                {
                    System.Console.WriteLine("No procedure for class " + myCurrentWorkflowClass + ". Processing workflows for this class stops.");
                    return;
                }
                // get procedure priority
                int PriorityRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("PR", "Normal priority");
                int ActionStatusRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ST", "Open");
                // get person with default role from web.config if there are no current assignments in with documentcontrol.revisionresponsibelerole
                string defaultRole = GetCoordinatorWorkitemRole();
                // Tor 20170325 Job Title (Role) category moved from ER to TITL
                //int defaultRoleRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ER", defaultRole);
                int defaultRoleRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", defaultRole);

                int defaultPersonnelRowId = GetCoordinatorWorkitemPersonnelRowId();
                

                foreach (RiskDS.GARiskRow row in ds.GARisk)
                {

                    //  starts workflow if next revision date is null (the script will pick up GARisk approaching its first revision date, last review date + revision frequency )
                    //  System.Console.WriteLine("RiskRowId is:" + row.RiskRowId);
                    //  System.Console.WriteLine("RiskRefId is:" + row.RiskReferenceId);
                    if (!row.IsDateNextAssessmentNull())
                    {
                        // stop workflow if next revision date later than location.ToDate
                        GADataRecord location = DataClassRelations.GetOwner(new GADataRecord(row.RiskRowId, GADataClass.GARisk));
                        BusinessClass locationBC = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(location.DataClass);
                        System.Data.DataSet dsLocation = locationBC.GetByRowId(location.RowId);
                        DateTime toDate = DateTime.MaxValue;
                        //  System.Console.WriteLine("Location Rowid:" +dsLocation.Tables[0].Rows[0]["LocationRowId"].ToString());
                        //  System.Console.WriteLine(dsLocation.Tables[0].Rows[0]["DateTimeFree2"].ToString());

                        if (DateTime.TryParse(dsLocation.Tables["GALocation"].Rows[0]["DateTimeFree2"].ToString(), out toDate))
                        {
                            //  Gao added 20230321 pass the value of Location.DateTimeFree2 to parameter toDate
                            toDate = Convert.ToDateTime(dsLocation.Tables[0].Rows[0]["DateTimeFree2"]);
                            //  System.Console.WriteLine("Date is not null:" + toDate.ToString());
                        }
                        else
                        {
                            toDate = DateTime.MaxValue;
                            //  System.Console.WriteLine("Date is null:" + toDate.ToString());
                        }

                        if (toDate < row.DateNextAssessment)
                        {
                            continue;
                        }
                        else
                        {
                            if (row.DateNextAssessment > DateTime.UtcNow.Date)
                            {
                                continue;
                            }
                        }

                    }
                    else 
                    {

                        if (!row.IsPotentialListsRowIdNull() && (GASystem.BusinessLayer.WorkflowStarter.GetNextDate(row.PotentialListsRowId, row.DateTimeFree1, "add") <= DateTime.UtcNow.Date))
                        {
                            System.Console.WriteLine("start workflow with Risk Reference Id: " + row.RiskReferenceId);
                        }
                        else
                        {

                            continue;
                        }                        
                    }
                   
                     
                    GADataTransaction Sctransaction = GADataTransaction.StartGADataTransaction();
                    try
                    {
                        System.Console.WriteLine("Create Workflow for GARisk Hazard Assessment with refence id: " + row.RiskReferenceId);

                        //                            row.DateTimeFree2=GASystem.BusinessLayer.WorkflowStarter.GetNextDate(row.DeletedBy,DateTime.UtcNow.Date, "add");
                        // set Last Revision Started Date			  
                        // Tor 20190807 row.DateTimeFree2 = DateTime.UtcNow.Date;
                        // set Status next Revision Started 
                        row.StatusListsRowId = GASystem.BusinessLayer.WorkflowStarter.GetListsRowId(StatusCategory, StatusNextRevisionStarted);

                        int PersonnelRowId = 0;
                        int myResponsibleRoleListsRowId = 0;
                        // get first person with Responsible Job Title, default
                        if (!row.IsResponsibleRoleListsRowIdNull())
                        {
                            PersonnelRowId = GetPersonnelRowIdWithTitle(row.ResponsibleRoleListsRowId, row.RiskRowId, myCurrentWorkflowClass);
                            if (PersonnelRowId != 0) myResponsibleRoleListsRowId = row.ResponsibleRoleListsRowId;
                        }
                        if (PersonnelRowId == 0) PersonnelRowId = defaultPersonnelRowId;

                        // Tor 20191209
                        userName = GetUserName(PersonnelRowId);
                        //UserDS uds = new UserDS();
                        //uds = GASystem.BusinessLayer.User.GetUserByPersonnelRowId(PersonnelRowId);
                        //if (uds.Tables.Count > 0) userName = uds.GAUser[0].DNNUserId;

                        GADataRecord owner = new GADataRecord(row.RiskRowId, GADataRecord.ParseGADataClass(myCurrentWorkflowClass));
                        Action action = new Action();
                        ActionDS ads = (ActionDS)action.GetNewRecord(owner, Sctransaction);
                        ActionDS.GAActionRow newRow = (ActionDS.GAActionRow)ads.GAAction.Rows[0];

                        string myRevisionResponsible = GASystem.BusinessLayer.Lists.GetListValueByRowId(row.ResponsibleRoleListsRowId);
                        string myRevisionFrequency = GASystem.BusinessLayer.Lists.GetListValueByRowId(row.PotentialListsRowId);

                        newRow.ActionName = pds.GAProcedure[0].Shortname;

                        GASystem.BusinessLayer.Utils.IdGenerator idgen = new GASystem.BusinessLayer.Utils.IdGenerator(GADataClass.GAAction, owner, Sctransaction);
                        idgen.ApplyReferenceId(ads);

                        newRow.ProcedureRowId = pds.GAProcedure[0].ProcedureRowId;
                        newRow.Description = pds.GAProcedure[0].Description;
                        newRow.Subject = pds.GAProcedure[0].Shortname;
                        newRow.ActionStatusListsRowId = ActionStatusRowId;
                        newRow.PriorityListsRowId = PriorityRowId;
                        newRow.ResponsibleRoleListsRowId = myResponsibleRoleListsRowId;
                        newRow.RegisteredByPersonnelRowId = PersonnelRowId;
                        newRow.WorkflowId = "StartPending:" + userName;
                        newRow.DateEndEstimated = (!row.IsDateNextAssessmentNull()) ? row.DateNextAssessment.AddDays(7) : DateTime.Now.Date.AddDays(7);
                        if (newRow.DateEndEstimated < DateTime.Now.Date.AddDays(1)) newRow.DateEndEstimated = DateTime.Now.Date.AddDays(1);
                        DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(GADataClass.GAAction, Sctransaction);
                        DataSet dsresult = gada.Update(ads);
                        DataClassRelations.CreateDataClassRelation(owner.RowId, owner.DataClass, (int)dsresult.Tables["GAAction"].Rows[0]["ActionRowId"], GADataClass.GAAction, Sctransaction);

                        // set date next revision
                        if (!row.IsPotentialListsRowIdNull())
                        {
                            row.DateNextAssessment = GASystem.BusinessLayer.WorkflowStarter.GetNextDate(row.PotentialListsRowId, DateTime.UtcNow.Date, "add");
                        }
                        else
                        {
                            row.DateNextAssessment = DateTime.Now.Date.AddYears(1);
                        }
                        /*
                        if (row.DateNextAssessment < DateTime.Now.Date.AddDays(1))
                        {
                            row.DateNextAssessment = GASystem.BusinessLayer.WorkflowStarter.GetNextDate(row.PotentialListsRowId, DateTime.UtcNow.Date, "add");
                        }
                        */
                        // set last revision date = today
                        row.DateTimeFree1 = DateTime.UtcNow.Date;

                        // update dataset
                        GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GARisk);
                        bc.UpdateDataSet(ds, Sctransaction);
                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Transaction Rollback for " + row.RiskReferenceId + " RowId: " + row.RiskRowId.ToString());
                        Sctransaction.Rollback();
                        throw ex;
                    }
                    finally
                    {
                        Sctransaction.Commit();
                        Sctransaction.Connection.Close();
                    }
                }
            }
        }



        // Tor 20160611 added method
        private static void doCoursePersonListViewEmailNotifications(string myCurrentWorkflowClass, GADataTransaction transaction)
        {
            // read all entries with sql query stored in config file (GALists SYS GAListValue EMNSQL
            Console.WriteLine("Start sending Offshore Training expiry E-mail reminders");

            string updateSQL = "Update {0} set DateTimeFree3= {1} where {2} = {3}";
            string SMTPFromAddress = new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPFromAddress");
            if (SMTPFromAddress == null || SMTPFromAddress == string.Empty)
            {
                Console.WriteLine("SMTPFromAddress could not be found in config with key SMTPFromAddress. Job terminates.");
                return;
            }
            string sql = new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNSQL"+myCurrentWorkflowClass);
            if (sql == null || sql == string.Empty)
            {
                Console.WriteLine("No SQL select statement found in config with key EMNSQL"+myCurrentWorkflowClass+". Job terminates.");
                return;
            }
            // SqlDataReader result = GASystem.DataAccess.DataUtils.executeSelectSpecial(sql, DataUtils.getConnectionString());

            // Tor 20160226 code copied from the commented code above from C:\gadev2\gamain\GASystem\DataAccess\DataUtils.cs method: public static SqlDataReader executeSelectSpecial(String sql, String  connectionString)
            // because it failed because the connection was closed before the data records were read

			SqlConnection myConnection;
			SqlCommand myCommand;
			SqlDataReader result;
            myConnection = new SqlConnection(DataUtils.getConnectionString());
            try
            {
                myCommand = new SqlCommand(sql, myConnection);
                myConnection.Open();
                result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                // Tor 20151111 moved to finally myConnection.Close();
                throw e;
            }

            if (!result.HasRows)
            {
                Console.WriteLine("No records returned from select statement");
                myConnection.Close();
                return;
            }
            bool useHTML=true;
            string subject=string.Empty;
            string body=string.Empty;
            string myCertificateIdforSubjectField=string.Empty;

            // Tor 20160814 Fails when vertical is null
            //int VerticalListsRowId	;
            int? VerticalListsRowId	;
            int Personnel;
            int RoleListsRowId	;
            string eMailCopyTo=string.Empty;
            string eMailTo	=string.Empty;
            string Path	=string.Empty;
            int LocationRowId	;
            int OffshoreCertificateViewRowId	;
            string Hyperlink	=String.Empty;
            int PersonnelRowId	;
            string ReferenceId	=string.Empty;
            DateTime DateExpire	;
            DateTime? DateLastEmailReminder	;
            int TypeOfCourseListsRowId	;
            string TypeOfCource	=string.Empty;
            string TypeOfCourseGroup	=string.Empty;
            string CourseName	=string.Empty;

            string EmailNotificationGAListValuePrefix = "EMNTOT";
            string EmailNotificationGAListValue = string.Empty;
            string EmailNotificationBody = string.Empty;
            string EmailNotificationExpiryDueText = "is due to expire in {0} days";
            string EmailNotificationExpiredText = "expired {0} days ago";
            string EmailNotificationExpiryActualText = string.Empty;
            // Tor 20160328 Add role for CC role on e-mail reminder
            string EmailNotificationReminderCCRoleGAListValuePrefix = "EmnCcRmndrRole-";
            string EmailNotificationReminderCCRoleGAListValue = string.Empty;
            string EmailNotificationReminderCCRole = string.Empty;
            string EmailNotificationReminderCCEmailAddress = string.Empty;
            //string[] EmailNotificationReminderCCRoles; //=string.Empty;
            int numberOfEmailSent = 0;

            DateTime myDateLastEmailReminder;

            while (result.Read())
            {
                DateLastEmailReminder = null;
                // Tor 20160814 Fails when vertical is null
                VerticalListsRowId = null;
                if (result["VerticalListsRowId"] != DBNull.Value) VerticalListsRowId = result.GetInt32(0);
                //VerticalListsRowId = result.GetInt32(0);
                
                Personnel	= result.GetInt32(1);
                RoleListsRowId	= result.GetInt32(2);
                eMailCopyTo=result.GetString(3);
                eMailTo	=result.GetString(4);
                Path	=result.GetString(5);
                LocationRowId	= result.GetInt32(6);
                OffshoreCertificateViewRowId	= result.GetInt32(7);
                Hyperlink	=result.GetString(8);
                PersonnelRowId	= result.GetInt32(9);
                ReferenceId	=result.GetString(10);
                DateExpire	=result.GetDateTime(11);
                if (result["DateLastEmailReminder"]!=DBNull.Value) DateLastEmailReminder = result.GetDateTime(12);
                TypeOfCourseListsRowId	= result.GetInt32(13);
                TypeOfCource	=result.GetString(14);
                TypeOfCourseGroup	=result.GetString(15);
                CourseName	=result.GetString(16);

                EmailNotificationGAListValue = EmailNotificationGAListValuePrefix + "-" + LocationRowId.ToString();
                EmailNotificationBody = new GASystem.AppUtils.FlagSysResource().GetResourceString(EmailNotificationGAListValue);
                if (EmailNotificationBody == null || EmailNotificationBody == string.Empty)
                {
                    // Tor 20160620 get default body
                    EmailNotificationGAListValue = EmailNotificationGAListValuePrefix + "-Default";
                    EmailNotificationBody = new GASystem.AppUtils.FlagSysResource().GetResourceString(EmailNotificationGAListValue);
                }
                if (EmailNotificationBody == null || EmailNotificationBody == string.Empty)
                {
                    Console.WriteLine("E-mail body text for " + EmailNotificationGAListValue + " not found in config file. Skipping reminder to " + eMailTo + " ");
                }
                else
                {
                    GADataTransaction eMailtransaction = GADataTransaction.StartGADataTransaction();
                    try
                    {
                        //      create and send e-mail
                        //          get e-mail text body from GALists based on certificate type (SYS GAListValue=EMNTOT-locationRowId
                        //          Build subject field

                        // Tor 20160328 Add role for CC role on e-mail reminder
                        EmailNotificationReminderCCRole = string.Empty;
                        myCertificateIdforSubjectField = string.Empty;
                        System.Collections.ArrayList CCrecipentsList = new System.Collections.ArrayList();
                        // if (CertificateId != string.Empty) myCertificateIdforSubjectField = "with certificate id: " + CertificateId;
                        
                        // Tor 20170414 Add Person name to message header
                        string myPersonnelFullName = GetPersonFullName(Personnel);
                        if (DateExpire > DateTime.UtcNow) // different text depending on expires in future or past, and number of days until or past expiry date
                        {
                            // Tor 20170414 subject = "Your '" + TypeOfCource + "' expires " + string.Format("{0:d MMMM yyyy}", DateExpire);
                            subject = myPersonnelFullName+": Your '" + TypeOfCource + "' expires " + string.Format("{0:d MMMM yyyy}", DateExpire);
                        }
                        else
                        {
                            // Tor 20170414 subject = "Your '" + TypeOfCource+ "' expired "+ string.Format("{0:d MMMM yyyy}", DateExpire);
                            subject = myPersonnelFullName+": Your '" + TypeOfCource + "' expired " + string.Format("{0:d MMMM yyyy}", DateExpire);
                        }
                        
                        if (DateLastEmailReminder != null)
                        {
                            // reminder has been sent
                            myDateLastEmailReminder = result.GetDateTime(12);
                            TimeSpan ts = DateExpire - myDateLastEmailReminder.Date;
                            TimeSpan tsNow = DateTime.UtcNow - myDateLastEmailReminder.Date;

                            if ((ts.Days < 31 && ts.Days>0) || (tsNow.Days > 30)) 
                                // if (less than 31 days and more than 0 until cert expires) or more than 30 days since last reminder 
                            {
                                subject = "REMINDER! : " + subject;
                                // Tor 20160328 add cc e-mail addresses 
                            }
                        }
                        
                        body = string.Format(EmailNotificationBody
                            , GetEmailNotificationsExpiryText(DateExpire, EmailNotificationExpiryDueText, EmailNotificationExpiredText)
                            , eMailCopyTo);

                        // add e-mail addresses to arrays
                        AddElementToArrayIfNotAlreadyThere(ref CCrecipentsList, eMailCopyTo);

                        EmailNotificationReminderCCRoleGAListValue = EmailNotificationReminderCCRoleGAListValuePrefix + LocationRowId.ToString();
                        EmailNotificationReminderCCRole = new GASystem.AppUtils.FlagSysResource().GetResourceString(EmailNotificationReminderCCRoleGAListValue);
                        if (EmailNotificationReminderCCRole != null && EmailNotificationReminderCCRole != string.Empty)

                        {
                            // get e-mail address for all assigned personnel with role under location
                            GetEmailAddressesFromRolesString(ref CCrecipentsList, LocationRowId, EmailNotificationReminderCCRole, eMailtransaction);
                            // split into individual recipients
                            //EmailNotificationReminderCCRoles = EmailNotificationReminderCCRole.Split(';');
                            //foreach (string smtpCCRecipient in EmailNotificationReminderCCRoles)
                            //{
                            //    if (smtpCCRecipient != string.Empty)
                            //    {
                            //        int roleListsRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ER", smtpCCRecipient);
                            //        if (roleListsRowId > 0)
                            //        {
                            //            EmploymentDS eds = GASystem.BusinessLayer.Employment.GetEmploymentsByOwnerDateAndRoleId
                            //            (LocationRowId,
                            //            GADataClass.GALocation, DateTime.UtcNow, roleListsRowId, eMailtransaction);
                            //            foreach (EmploymentDS.GAEmploymentRow row in eds.GAEmployment)
                            //            {
                            //                string emailaddress = MeansOfContact.GetEmailContactAddressByPersonnelRowId(row.Personnel, eMailtransaction);
                            //                AddElementToArrayIfNotAlreadyThere(ref CCrecipentsList, emailaddress);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            Console.WriteLine("GAListValue role " + smtpCCRecipient + " was not found in GALists, process proceeds without CC email to this role");
                            //        }
                            //    }
                            //}
                        }

                        System.Collections.ArrayList TOrecipentsList = new System.Collections.ArrayList();
                        AddElementToArrayIfNotAlreadyThere(ref TOrecipentsList, eMailTo);

                        // send e-mail
                        GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(TOrecipentsList, CCrecipentsList, null, subject, body, useHTML);

                        //      update last e-mail notification sent in the actual certificate
                        //      date last e-mail GAHealthCertificateView.DateTimeFree2
                        //      date last e-mail GAPassportVisaView.DateTimeFree2
                        Console.WriteLine("Email sent to: " + eMailTo);
                        numberOfEmailSent += 1;
                        int Result = GASystem.DataAccess.DataUtils.executeNoneQuery(string.Format(updateSQL, myCurrentWorkflowClass, "'" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm") + "'", myCurrentWorkflowClass.Substring(2) + "RowId", OffshoreCertificateViewRowId.ToString()), eMailtransaction);
                        eMailtransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        eMailtransaction.Rollback();
                        Console.WriteLine("Transaction failed for class: " + myCurrentWorkflowClass + " with RowId: " + OffshoreCertificateViewRowId.ToString() + " Email might have been sent, but last email notification date was not updated");
                        throw ex;
                    }
                    finally
                    {
                        eMailtransaction.Connection.Close();
                    }
                }
            }
            Console.WriteLine("Number of Offshore Training Email reminders sent: " + numberOfEmailSent.ToString());
            myConnection.Close();
        }


        private static void doHealthPassportVisaViewEmailNotifications(string myCurrentWorkflowClass, GADataTransaction transaction)
        {
            // read all entries with sql query stored in config file (GALists SYS GAListValue EMNSQL
            Console.WriteLine("Start sending Health and Passport/Visa expiry E-mail reminders");
            string updateSQL = "Update {0} set DateTimeFree2= {1} where {2} = {3}";
            string SMTPFromAddress = new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPFromAddress");
            if (SMTPFromAddress == null || SMTPFromAddress == string.Empty)
            {
                Console.WriteLine("SMTPFromAddress could not be found in config with key SMTPFromAddress. Job terminates.");
                return;
            }
            string sql = new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNSQL");
            if (sql == null || sql == string.Empty)
            {
                Console.WriteLine("No SQL select statement found in config with key EMNSQL. Job terminates.");
                return;
            }
            // SqlDataReader result = GASystem.DataAccess.DataUtils.executeSelectSpecial(sql, DataUtils.getConnectionString());

            // Tor 20160226 code copied from the commented code above from C:\gadev2\gamain\GASystem\DataAccess\DataUtils.cs method: public static SqlDataReader executeSelectSpecial(String sql, String  connectionString)
            // because it failed because the connection was closed before the data records were read

			SqlConnection myConnection;
			SqlCommand myCommand;
			SqlDataReader result;
            myConnection = new SqlConnection(DataUtils.getConnectionString());
            try
            {
                myCommand = new SqlCommand(sql, myConnection);
                myConnection.Open();
                result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                // Tor 20151111 moved to finally myConnection.Close();
                throw e;
            }
            // Tor 20151111 added finally to close connection that was opened at the beginning of the method
            //finally 
            //{
            //    myConnection.Close();
            //}


            if (!result.HasRows)
            {
                Console.WriteLine("No records returned from select statement");
                myConnection.Close();
                return;
            }
            bool useHTML=true;
            string subject=string.Empty;
            string body=string.Empty;
            string myCertificateIdforSubjectField=string.Empty;
            string Class = string.Empty;                    //0 Class string
            int RowId;                                      //1 RowId int
            int PersonnelRowId;                             //2 PersonnelRowId int
            DateTime DateExpire;                            //3 DateExpire datetime
            int DaysBeforePassport;                         //4 DaysBeforePassport int
            int DaysBeforeOther;                            //5 DaysBeforeOther int
            string LocationeMailAddress = string.Empty;     //6 LocationeMailAddress string
            string PersonnelEmailAddress = string.Empty;    //7 PersonnelEmailAddress string
            string CertificateId = string.Empty;            //8 CertificateId string
            string TypeOfCertificate = string.Empty;        //9 TypeOfCertificate string
            string Country = string.Empty;                  //10 Country string
//            DateTime DateLastEmailReminder;                 //11 DateLastEmailReminder datetime
            DateTime? DateLastEmailReminder;                //11 DateLastEmailReminder datetime
            int LocationRowId;                              //12 LocationRowId int
            string CertificateGroup = string.Empty;         //13 CertificateGroup string
            string EmailNotificationGAListValuePrefix = "EMNT";
            string EmailNotificationGAListValue = string.Empty;
            string EmailNotificationBody = string.Empty;
            string EmailNotificationExpiryDueText = "is due to expire in {0} days";
            string EmailNotificationExpiredText = "expired {0} days ago";
            string EmailNotificationExpiryActualText = string.Empty;
            // Tor 20160328 Add role for CC role on e-mail reminder
            string EmailNotificationReminderCCRoleGAListValuePrefix = "EmnCcRmndrRole-";
            string EmailNotificationReminderCCRoleGAListValue = string.Empty;
            string EmailNotificationReminderCCRole = string.Empty;
            string EmailNotificationReminderCCEmailAddress = string.Empty;
            //string[] EmailNotificationReminderCCRoles; //=string.Empty;
            int numberOfEmailSent = 0;

            DateTime myDateLastEmailReminder;
            /* CertificateGroup 20160224:
                Medical
                Passport
                Vaccination
                Visa
                Other
             */

            while (result.Read())
            {
                // for each entry
                //      begin tran
                // get e-mail body text
                DateLastEmailReminder = null;
                Class = result.GetString(0);
                RowId = result.GetInt32(1);
                PersonnelRowId = result.GetInt32(2);
                DateExpire = result.GetDateTime(3);
                DaysBeforePassport = result.GetInt32(4);
                DaysBeforeOther = result.GetInt32(5);
                LocationeMailAddress = result.GetString(6);
                PersonnelEmailAddress = result.GetString(7);
                CertificateId = result.GetString(8);
                TypeOfCertificate = result.GetString(9);
                Country = result.GetString(10);

                // if result["fieldname"]!=....
                if (result["DateLastEmailReminder"]!=DBNull.Value) DateLastEmailReminder = result.GetDateTime(11);
                
                LocationRowId = result.GetInt32(12);
                CertificateGroup = result.GetString(13);
                EmailNotificationGAListValue = EmailNotificationGAListValuePrefix+CertificateGroup + "-" + LocationRowId.ToString();
                EmailNotificationBody = new GASystem.AppUtils.FlagSysResource().GetResourceString(EmailNotificationGAListValue);
                if (EmailNotificationBody == null || EmailNotificationBody == string.Empty)
                {
                    // Tor 20160620 get default text
                    EmailNotificationGAListValue = EmailNotificationGAListValuePrefix + CertificateGroup + "-Default";
                    EmailNotificationBody = new GASystem.AppUtils.FlagSysResource().GetResourceString(EmailNotificationGAListValue);
                }
                if (EmailNotificationBody == null || EmailNotificationBody == string.Empty)
                {
                    Console.WriteLine("E-mail body text for " + EmailNotificationGAListValue + " not found in config file. Skipping " + CertificateGroup + " reminder to " + PersonnelEmailAddress + " ");
                }
                else
                {
                    GADataTransaction eMailtransaction = GADataTransaction.StartGADataTransaction();
                    try
                    {
                        //      create and send e-mail
                        //          get e-mail text body from GALists based on certificate type (SYS GAListValue=EMNT<type>-locationRowId
                        //          Build subject field

                        // Tor 20160328 Add role for CC role on e-mail reminder
                        EmailNotificationReminderCCRole = string.Empty;
                        myCertificateIdforSubjectField = string.Empty;
                        System.Collections.ArrayList CCrecipentsList = new System.Collections.ArrayList();
                        if (CertificateId != string.Empty) myCertificateIdforSubjectField = "with certificate id: " + CertificateId;
                        
                        // Tor 20170414 Add Person's name to message header
                        string myPersonnelName = GetPersonFullName(PersonnelRowId);
                        if (DateExpire > DateTime.UtcNow) // different text depending on expires in future or past, and number of days until or past expiry date
                        {
                            // Tor 20170414 subject = "Your " + TypeOfCertificate + " " + myCertificateIdforSubjectField + " issued in " + Country + " expires " + string.Format("{0:d MMMM yyyy}", DateExpire);
                            subject = myPersonnelName+": Your " + TypeOfCertificate + " " + myCertificateIdforSubjectField + " issued in " + Country + " expires " + string.Format("{0:d MMMM yyyy}", DateExpire);
                        }
                        else
                        {
                            // Tor 20170414 subject = "Your " + TypeOfCertificate + " " + myCertificateIdforSubjectField + " issued in " + Country + " expired "+ string.Format("{0:d MMMM yyyy}", DateExpire);
                            subject = myPersonnelName+": Your " + TypeOfCertificate + " " + myCertificateIdforSubjectField + " issued in " + Country + " expired " + string.Format("{0:d MMMM yyyy}", DateExpire);

                        }
                        
                        if (DateLastEmailReminder != null)
                        {
                            // reminder has been sent
                            myDateLastEmailReminder = result.GetDateTime(11);
                            TimeSpan ts = DateExpire - myDateLastEmailReminder.Date;
                            TimeSpan tsNow = DateTime.UtcNow - myDateLastEmailReminder.Date;

                            if ((ts.Days < 31 && ts.Days>0) || (tsNow.Days > 30)) 
                                // if (less than 31 days and more than 0 until cert expires) or more than 30 days since last reminder 
                            {
                                subject = "REMINDER! : " + subject;
                                // Tor 20160328 add cc e-mail addresses 
                             
                            }
                            
                        }
                        
                        body = string.Format(
                            EmailNotificationBody, 
                            GetEmailNotificationsExpiryText(DateExpire, EmailNotificationExpiryDueText, EmailNotificationExpiredText)
                            , LocationeMailAddress);

                        // add e-mail addresses to arrays
                        AddElementToArrayIfNotAlreadyThere(ref CCrecipentsList, LocationeMailAddress);

                        EmailNotificationReminderCCRoleGAListValue = EmailNotificationReminderCCRoleGAListValuePrefix + LocationRowId.ToString();
                        EmailNotificationReminderCCRole = new GASystem.AppUtils.FlagSysResource().GetResourceString(EmailNotificationReminderCCRoleGAListValue);
                        if (EmailNotificationReminderCCRole != null && EmailNotificationReminderCCRole != string.Empty )
                        {
                            // get e-mail address for all assigned personnel with role under location
                            GetEmailAddressesFromRolesString(ref CCrecipentsList, LocationRowId, EmailNotificationReminderCCRole, eMailtransaction);
                            // split into individual recipients
                            //EmailNotificationReminderCCRoles = EmailNotificationReminderCCRole.Split(';');
                            //foreach (string smtpCCRecipient in EmailNotificationReminderCCRoles)
                            //{
                            //    if (smtpCCRecipient != string.Empty)
                            //    {
                            //        int roleListsRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ER", smtpCCRecipient);
                            //        if (roleListsRowId > 0)
                            //        {
                            //            EmploymentDS eds = GASystem.BusinessLayer.Employment.GetEmploymentsByOwnerDateAndRoleId
                            //            (LocationRowId,
                            //            GADataClass.GALocation, DateTime.UtcNow, roleListsRowId, eMailtransaction);
                            //            foreach (EmploymentDS.GAEmploymentRow row in eds.GAEmployment)
                            //            {
                            //                string emailaddress = MeansOfContact.GetEmailContactAddressByPersonnelRowId(row.Personnel, eMailtransaction);
                            //                AddElementToArrayIfNotAlreadyThere(ref CCrecipentsList, emailaddress);
                            //            }
                            //        }
                            //        else
                            //        {
                            //            Console.WriteLine("GAListValue role " + smtpCCRecipient + " was not found in GALists, process proceeds without CC email to this role");
                            //        }
                            //    }
                            //}
                        }

                        System.Collections.ArrayList TOrecipentsList = new System.Collections.ArrayList();
                        AddElementToArrayIfNotAlreadyThere(ref TOrecipentsList, PersonnelEmailAddress);

                        // send e-mail
                        GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(TOrecipentsList, CCrecipentsList, null, subject, body, useHTML);
                        Console.WriteLine("expiry eMail notification has been sent to " + PersonnelEmailAddress);
                        numberOfEmailSent += 1;

                        //      update last e-mail notification sent in the actual certificate
                        //      date last e-mail GAHealthCertificateView.DateTimeFree2
                        //      date last e-mail GAPassportVisaView.DateTimeFree2
                        int Result = GASystem.DataAccess.DataUtils.executeNoneQuery(string.Format(updateSQL, Class, "'" + DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm") + "'", Class.Substring(2) + "RowId", RowId.ToString()), eMailtransaction);
                        eMailtransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        eMailtransaction.Rollback();
                        Console.WriteLine("Transaction failed for class: "+Class+" with RowId: "+RowId.ToString()+" Email might have been sent, but last email notification date was not updated");
                        throw ex;
                    }
                    finally
                    {
                        eMailtransaction.Connection.Close();
                    }
                }
            }
            myConnection.Close();
            Console.WriteLine("eMail notification sent: " + numberOfEmailSent.ToString());

        }

        private static void doPersonnelEmailNotifications(string myCurrentWorkflowClass, GADataTransaction transaction)
        {
            // read all entries with sql query stored in config file (GALists SYS GAListValue EMNSQL
            Console.WriteLine("Start sending Personnel Security Expiry E-mail reminders");

            string SMTPFromAddress = new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPFromAddress");
            if (string.IsNullOrEmpty(SMTPFromAddress))
            {
                Console.WriteLine("SMTPFromAddress could not be found in config with key SMTPFromAddress. Job terminates.");
                return;
            }
            string sql = new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNSQL" + myCurrentWorkflowClass);
            if (string.IsNullOrEmpty(sql))
            {
                Console.WriteLine("No SQL select statement found in config with key EMNSQL" + myCurrentWorkflowClass + ". Job terminates.");
                return;
            }

            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            SqlCommand myCommand = new SqlCommand(sql, myConnection);
            SqlDataReader result;            
            try
            {                
                myConnection.Open();
                result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                throw e;
            }

            if (!result.HasRows)
            {
                Console.WriteLine("No records returned from select statement");
                myConnection.Close();
                return;
            }
            //result is the result of sql statements defined in nTextFree2 or GAListDescription
            //when category='sys'

            string subject = "Security Expiry Reminder";
            string body = string.Empty;
            int Personnel;
            string eMailTo = string.Empty;

            DateTime DateExpire;
            string EmailNotificationBody = string.Empty; 
            int numberOfEmailSent = 0;

            while (result.Read())
            {
                Personnel = result.GetInt32(0);
                eMailTo = result.GetString(1);
                DateExpire = result.GetDateTime(2);

                EmailNotificationBody = new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNSecurityExpiryReminder");
                if (EmailNotificationBody == null || EmailNotificationBody == string.Empty)
                {
                    Console.WriteLine("E-mail body text for EMNSecurityExpiryReminder not found in config file. Skipping reminder to " + eMailTo + " ");
                }
                else
                {
                    GADataTransaction eMailtransaction = GADataTransaction.StartGADataTransaction();
                    try
                    {
                        string myPersonnelFullName = GetPersonFullName(Personnel);
                        body = string.Format(EmailNotificationBody, myPersonnelFullName);

                        System.Collections.ArrayList toList = new System.Collections.ArrayList();
                        AddElementToArrayIfNotAlreadyThere(ref toList, eMailTo);
                        // send e-mail
                        GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(toList, null, null, subject, body, true);
                        System.Collections.ArrayList toList2 = new System.Collections.ArrayList();
                        //toList2.Add("zhanghaipeng1@cnpc.com.cn");
                        GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(toList2, null, null, subject, body, true);
                        Console.WriteLine("Email sent to: " + eMailTo);
                        numberOfEmailSent += 1;
                        eMailtransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        eMailtransaction.Rollback();
                        Console.WriteLine("Transaction failed for class: " + myCurrentWorkflowClass + " with RowId: " + Personnel.ToString() + " Email might have been sent, but last email notification date was not updated");
                        throw ex;
                    }
                    finally
                    {
                        eMailtransaction.Connection.Close();
                    }
                }
            }
            Console.WriteLine("Number of Offshore Training Email reminders sent: " + numberOfEmailSent.ToString());
            myConnection.Close();
        }

        private static void doMOCEmailNotifications(string myCurrentWorkflowClass, GADataTransaction transaction)
        {
            // read all entries with sql query stored in config file (GALists SYS GAListValue EMNSQL
            Console.WriteLine("Start sending Removal of Temp. MoC EMail Notification reminders ");

            string SMTPFromAddress = new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPFromAddress");
            if (string.IsNullOrEmpty(SMTPFromAddress))
            {
                Console.WriteLine("SMTPFromAddress could not be found in config with key SMTPFromAddress. Job terminates.");
                return;
            }
            string sql = new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNSQL" + myCurrentWorkflowClass);
            if (string.IsNullOrEmpty(sql))
            {
                Console.WriteLine("No SQL select statement found in config with key EMNSQL" + myCurrentWorkflowClass + ". Job terminates.");
                return;
            }

            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            SqlCommand myCommand = new SqlCommand(sql, myConnection);
            SqlDataReader result;
            try
            {
                myConnection.Open();
                result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                throw e;
            }

            if (!result.HasRows)
            {
                Console.WriteLine("No records returned from select statement");
                myConnection.Close();
                return;
            }

            string subject = "Removal of Temp. MoC: {0}";
            string body = string.Empty;
            string ManageChangereferenceid = string.Empty;
            string eMailTo = string.Empty;

            DateTime DateExpire;
            string EmailNotificationBody = string.Empty;
            int numberOfEmailSent = 0;

            while (result.Read())
            {
                ManageChangereferenceid = result.GetString(0);
                eMailTo = result.GetString(2);

                subject = string.Format(subject, ManageChangereferenceid);
                EmailNotificationBody = new GASystem.AppUtils.FlagSysResource().GetResourceString("EMNMoCEMailNotification");
                if (EmailNotificationBody == null || EmailNotificationBody == string.Empty)
                {
                    Console.WriteLine("E-mail body text for EMNSecurityExpiryReminder not found in config file. Skipping reminder to " + eMailTo + " ");
                }
                else
                {
                    GADataTransaction eMailtransaction = GADataTransaction.StartGADataTransaction();
                    try
                    {
                        body = string.Format(EmailNotificationBody, ManageChangereferenceid);

                        System.Collections.ArrayList toList = new System.Collections.ArrayList();
                        AddElementToArrayIfNotAlreadyThere(ref toList, eMailTo);
                        // send e-mail
                        GASystem.BusinessLayer.FlagSMTP.SendEmailMsg(toList, null, null, subject, body, true);

                        numberOfEmailSent += 1;
                        eMailtransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        eMailtransaction.Rollback();
                        Console.WriteLine("Transaction failed for class: " + myCurrentWorkflowClass + " with RowId: " + ManageChangereferenceid.ToString() + " Email might have been sent, but last email notification date was not updated");
                        throw ex;
                    }
                    finally
                    {
                        eMailtransaction.Connection.Close();
                    }
                }
            }
            Console.WriteLine("eMail notification sent: " + numberOfEmailSent.ToString());
            myConnection.Close();
        }
        //private static string GetHealthPassportVisaViewEmailNotificationsExpiryText(DateTime DateExpire
        //    , string EmailNotificationExpiryDueText, string EmailNotificationExpiredText)
        //{
        //    int myDays;
        //    if (DateExpire > DateTime.UtcNow)
        //    {
        //        TimeSpan ets = DateExpire.Date - DateTime.UtcNow.Date;
        //        myDays = ets.Days;
        //        //EmailNotificationExpiryActualText = string.Format(EmailNotificationExpiryDueText, myDays.ToString());
        //        return string.Format(EmailNotificationExpiryDueText, myDays.ToString());
        //    }
        //    else
        //    {
        //        TimeSpan ets = DateTime.UtcNow.Date - DateExpire.Date;
        //        myDays = ets.Days;
        //        //EmailNotificationExpiryActualText = string.Format(EmailNotificationExpiredText, myDays.ToString());
        //        return string.Format(EmailNotificationExpiredText, myDays.ToString());
        //    }
        //}

        // Tor 20160611 added method
        private static void GetEmailAddressesFromRolesString(ref System.Collections.ArrayList eMailAddresses
            ,int LocationRowId,string roles, GADataTransaction eMailtransaction)
        {

            // get e-mail address for all assigned personnel with role under location
            // split into individual recipients
            string[] roleList = roles.Split(';');
            foreach (string role in roleList)
            {
                if (role != string.Empty)
                {
                    // Tor 20170325 Job Title (Role) category moved from ER to TITL
                    //int roleListsRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ER", role);
                    int roleListsRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", role);
                    if (roleListsRowId > 0)
                    {
                        // Tor 20170327 EmploymentDS eds = GASystem.BusinessLayer.Employment.GetEmploymentsByOwnerDateAndRoleId
                        EmploymentDS eds = GASystem.BusinessLayer.Employment.GetEmploymentsByOwnerDateAndJobTitle
                        (LocationRowId,
                        GADataClass.GALocation, DateTime.UtcNow, roleListsRowId, eMailtransaction);
                        foreach (EmploymentDS.GAEmploymentRow row in eds.GAEmployment)
                        {
                            // Tor 20170327 Get eMail address from GAPersonnel string emailaddress = MeansOfContact.GetEmailContactAddressByPersonnelRowId(row.Personnel, eMailtransaction);
                            string emailaddress = Personnel.GetPersonnelEmailAddress(row.Personnel);
                            AddElementToArrayIfNotAlreadyThere(ref eMailAddresses, emailaddress);
                        }
                    }
                    else
                    {
                        Console.WriteLine("GAListValue role " + role + " was not found in GALists, process proceeds without email to this role");
                    }
                }
            }
        }

        private static void AddElementToArrayIfNotAlreadyThere(ref System.Collections.ArrayList list, string element)
        {
            if (element != string.Empty)
            {
                // check if email address already in list before adding
                bool found = false;
                foreach (string a in list)
                {
                    if (a == element) found = true;
                }
                if (!found) list.Add(element);
            }
            return;
        }

        private static string GetEmailNotificationsExpiryText(DateTime DateExpire
            , string EmailNotificationExpiryDueText, string EmailNotificationExpiredText)
        {
            int myDays;
            if (DateExpire > DateTime.UtcNow)
            {
                TimeSpan ets = DateExpire.Date - DateTime.UtcNow.Date;
                myDays = ets.Days;
                //EmailNotificationExpiryActualText = string.Format(EmailNotificationExpiryDueText, myDays.ToString());
                return string.Format(EmailNotificationExpiryDueText, myDays.ToString());
            }
            else
            {
                TimeSpan ets = DateTime.UtcNow.Date - DateExpire.Date;
                myDays = ets.Days;
                //EmailNotificationExpiryActualText = string.Format(EmailNotificationExpiredText, myDays.ToString());
                return string.Format(EmailNotificationExpiredText, myDays.ToString());
            }

        }
        // Tor 20190809 - unreferenced method
        //private static int GetInitiatorPersonnelRowId(int RowId, GADataClass owner, DateTime date, int ResponsibleRoleRowId, GADataTransaction transaction)
        //{

        //    // get person with current role RevisionResponsibleRoleListsRowId
        //    // Tor 20170327 EmploymentDS eds = GASystem.BusinessLayer.Employment.GetEmploymentsByOwnerDateAndRoleId(RowId, owner, date, ResponsibleRoleRowId, transaction);
        //    EmploymentDS eds = GASystem.BusinessLayer.Employment.GetEmploymentsByOwnerDateAndJobTitle(RowId, owner, date, ResponsibleRoleRowId, transaction);

        //    if (eds.Tables[0].Rows.Count > 0) return eds.GAEmployment[0].Personnel;
        //    // get person by using role from config file
        //    // Tor 20170325 Job Title (Role) category moved from ER to TITL
        //    //eds = GASystem.BusinessLayer.Employment.GetEmploymentsByOwnerDateAndRoleId(RowId, owner, date, GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ER", GetCoordinatorWorkitemRole()),transaction);
        //    // Tor 20170327 eds = GASystem.BusinessLayer.Employment.GetEmploymentsByOwnerDateAndRoleId(RowId, owner, date, GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", GetCoordinatorWorkitemRole()), transaction);
        //    eds = GASystem.BusinessLayer.Employment.GetEmploymentsByOwnerDateAndJobTitle(RowId, owner, date, GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", GetCoordinatorWorkitemRole()), transaction);
        //    if (eds.Tables[0].Rows.Count > 0) return eds.GAEmployment[0].Personnel;
        //    // get person from by using config given and familyname or personnelrowid 
        //    return GetCoordinatorWorkitemPersonnelRowId();
        //}

        //private static int GetProcessCoordinatorPersonnelRowId(int RoleListsRowId, DocumentControlDS.GADocumentControlRow row, string myCurrentWorkflowClass)
        //{
        //    int personnelRowId = 0;
        //    EmploymentDS eds = GASystem.BusinessLayer.Employment.SearchForEmploymentsByOwnerDateAndJobTitle(
        //    // Tor 20170327 EmploymentDS eds = GASystem.BusinessLayer.Employment.SearchForEmploymentsByOwnerDateAndRoleId(
        //        new GADataRecord(row.DocumentControlRowId, GADataRecord.ParseGADataClass(myCurrentWorkflowClass))
        //        ,DateTime.UtcNow,RoleListsRowId);
        //    if (eds.Tables[0].Rows.Count > 0)
        //    {
        //        // Tor 20141029 Get first personnelRowId with Flag user Id among the current assigned personnel
        //        int i = 0;
        //        UserDS uds = new UserDS();
        //        while (i < (eds.Tables[0].Rows.Count) )
        //        { 
        //            uds = GASystem.BusinessLayer.User.GetUserByPersonnelRowId(eds.GAEmployment[i].Personnel);
        //            if (uds.Tables.Count > 0)
        //            {
        //                return eds.GAEmployment[i].Personnel;
        //            }
        //            i++;
        //        }
        //    }
        //    return personnelRowId;
        //}

        private static int GetPersonnelRowIdWithTitle(int RoleListsRowId, int rowId, string myCurrentWorkflowClass)
        {
            int personnelRowId = 0;
            EmploymentDS eds = GASystem.BusinessLayer.Employment.SearchForEmploymentsByOwnerDateAndJobTitle(
                // Tor 20170327 EmploymentDS eds = GASystem.BusinessLayer.Employment.SearchForEmploymentsByOwnerDateAndRoleId(
                new GADataRecord(rowId, GADataRecord.ParseGADataClass(myCurrentWorkflowClass))
                , DateTime.UtcNow, RoleListsRowId);
            if (eds.Tables[0].Rows.Count > 0)
            {
                // Tor 20141029 Get first personnelRowId with Flag user Id among the current assigned personnel
                int i = 0;
                UserDS uds = new UserDS();
                while (i < (eds.Tables[0].Rows.Count))
                {
                    uds = GASystem.BusinessLayer.User.GetUserByPersonnelRowId(eds.GAEmployment[i].Personnel);
                    if (uds.Tables.Count > 0)
                    {
                        return eds.GAEmployment[i].Personnel;
                    }
                    i++;
                }
            }
            return personnelRowId;
        }
        //private static DateTime GetNextDateAfterPrior(DateTime myBaseDate, int myAutoWorkflowStartPriorToNextRevisionListsRowId, int myRevisionFrequencyListsRowId)
        //{
        //    return GASystem.BusinessLayer.WorkflowStarter.GetNextDate(
        //        myAutoWorkflowStartPriorToNextRevisionListsRowId,
        //        GASystem.BusinessLayer.WorkflowStarter.GetNextDate(myRevisionFrequencyListsRowId,myBaseDate,"add"),
        //        "subtract");
        //}
                
        private static string GetCoordinatorWorkitemRole()
        {
                // Tor 20160303 return System.Configuration.ConfigurationManager.AppSettings.Get("CoordinatorWorkitemRole");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("CoordinatorWorkitemRole");

        }

        private static int GetCoordinatorWorkitemPersonnelRowId()
        {
                PersonnelDS pds = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelGivenNameAndFamilyName(
                    // Tor 20160303 System.Configuration.ConfigurationManager.AppSettings.Get("CoordinatorWorkitemGivenName"),
                    new GASystem.AppUtils.FlagSysResource().GetResourceString("CoordinatorWorkitemGivenName"),
                    // Tor 20160303 System.Configuration.ConfigurationManager.AppSettings.Get("CoordinatorWorkitemFamilyName")
                    new GASystem.AppUtils.FlagSysResource().GetResourceString("CoordinatorWorkitemFamilyName"));
                
                if (pds.Tables[0].Rows.Count==0) 
                {
                    // Tor 20160303 return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("CoordinatorWorkitemPersonnelRowId"));
                    return int.Parse(new GASystem.AppUtils.FlagSysResource().GetResourceString("CoordinatorWorkitemPersonnelRowId"));
                }
                return pds.GAPersonnel[0].PersonnelRowId;
        }

        private static string GetUserName(int PersonnelRowId)
        {
            UserDS uds = GASystem.BusinessLayer.User.GetUserByPersonnelRowId(PersonnelRowId);
            return (uds.Tables[0].Rows.Count > 0) ? uds.GAUser[0].DNNUserId : new GASystem.AppUtils.FlagSysResource().GetResourceString("WSDefaultDnnUsername");
            // Tor 20191209 return (uds.Tables[0].Rows.Count > 0) ? uds.GAUser[0].DNNUserId : string.Empty;
            //if (uds.Tables[0].Rows.Count > 0)
            //{
            //    return uds.GAUser[0].DNNUserId;
            //}
            //return string.Empty;
        }

        // Tor 20170414
        private static string GetPersonFullName(int PersonnelRowId)
        {
            PersonnelDS ds = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelRowId(PersonnelRowId);
            return (ds.Tables[0].Rows.Count > 0) ? ds.GAPersonnel[0].GivenName+" "+ds.GAPersonnel[0].FamilyName : string.Empty;
        }
    }
}
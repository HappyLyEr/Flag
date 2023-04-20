using System;
using System.Data;
using GASystem.DataModel;
using GASystem.DataAccess;
using log4net;
using System.Collections;
using GASystem.DataAccess.Security;
using GASystem.BusinessLayer.Utils;
using System.Collections.Generic;
using System.Text;

namespace GASystem.BusinessLayer
{
    public class SubcontractorView : BusinessClass
    {
        public static SubcontractorViewDS GetAllSubcontractorViewToStart(string myDataClass) // returns all subcontractors where workflow is not running
        {
            return GetAllSubcontractorViewToStart(myDataClass, null);
        }
        public static SubcontractorViewDS GetAllSubcontractorViewToStart(string myDataClass, GADataTransaction transaction) // returns all subcontractors where workflow is not running
        {
            return GASystem.DataAccess.SubcontractorViewDb.GetAllSubcontractorViewsToStart(myDataClass,transaction);
        }
        public static SubcontractorViewDS UpdateSubcontractorsAndCreateGAAction(SubcontractorViewDS ds, ProcedureDS pds, GADataTransaction transaction) // returns all subcontractors where workflow is not running
            // get Subcontractor Control Procedure dataset
        {
            string StatusCategory = "ScWS";
            string StatusNextRevisionStarted = "AuditInProgress";
            int myVendorLevel1ListsRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("VC", "1Ma");
            int myVendorLevel2ListsRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("VC", "2Mo");
            int myFrequencyEveryYearListsRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("WSRF", "Y");
            int myFrequencyEvery2ndYearListsRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("WSRF", "2Y");
            string userName = string.Empty;
            string myCurrentWorkflowClass = ds.Tables[0].TableName.ToString();
            // get procedure priority
            int PriorityRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("PR", "Normal priority");
            int ActionStatusRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ST", "Open");
            // get person with default role from web.config if there are no current assignments in with documentcontrol.revisionresponsibelerole
            string defaultRole = GetCoordinatorWorkitemRole();
            // Tor 20170313 Responsible changed from Role ER to Title TITL int defaultRoleRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("ER", defaultRole);
            int defaultRoleRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("TITL", defaultRole);
            int defaultPersonnelRowId = GetCoordinatorWorkitemPersonnelRowId();
            string defaultUserId=GetDefaultUserId();
//            int myAutoWorkflowStartPriorToNextAuditListsRowId = GetSubcontractorStartWorkflowPriorToAuditDate();

            foreach(SubcontractorViewDS.GASubcontractorViewRow row in ds.GASubcontractorView)
            {
                // set CategoryLevel and ClassificationMatrixValue from risk probability and consequence
                int myClassificationMatrixValue = ComputeClassificationMatrixValue(row.SubcontractorViewRowId);
                int myCategoryLevelListsRowId = ComputeCategoryLevelListsRowId(row.SubcontractorViewRowId);
                int myAuditPersonnelRowId = 0;
                int myAuditResponsibleRoleListsRowId = 0;
                // check if audit frequency is set according to rules:
                // GALists CategoryLevel:
                //  Group2=Y if automatic audit workflow is to start for this categorylevel
                //  Group4 has audit frequency for this categorylevel = GAListsValue for frequency lookup
                //  Group3 has workflow start prior to next audit

                // set audit frequency defined in CategoryLevel . Group4
                string isRunAutomaticWorkflow = (row.IsCategoryLevelListsRowIdNull()) ? string.Empty : GASystem.DataAccess.ListsDb.GetStringColumnValueByListsRowId(myCategoryLevelListsRowId, "Group2");
                int myAuditFrequencyListsRowId = GASystem.DataAccess.ListsDb.GetListsRowIdByCategoryAndKey(
                    GASystem.DataAccess.ListCategoryDb.GetListCategoryRowIdByName("WSRF")
                    , GASystem.DataAccess.ListsDb.GetStringColumnValueByListsRowId(myCategoryLevelListsRowId, "Group4"));

                // set prior to next audit
                int myStartWorkflowPriorToAuditListsRowId = GASystem.DataAccess.ListsDb.GetListsRowIdByCategoryAndKey(
                    GASystem.DataAccess.ListCategoryDb.GetListCategoryRowIdByName("WSRF")
                    , GASystem.DataAccess.ListsDb.GetStringColumnValueByListsRowId(myCategoryLevelListsRowId, "Group3"));

                // set if audit is overdue
                //  når er audit overdue ?
                // start workflow if next - prior or last + frequency <= today and CategoryLevelListsRowId != null

                // Tor 201611 Security 20161113 if (myCategoryLevelListsRowId != null && isRunAutomaticWorkflow == "Y" && myAuditFrequencyListsRowId != null
                //    && myStartWorkflowPriorToAuditListsRowId != null
                    // Tor 20170328           if (myCategoryLevelListsRowId == -1 && isRunAutomaticWorkflow == "Y" && myAuditFrequencyListsRowId == -1 && myStartWorkflowPriorToAuditListsRowId == -1 && ((!row.IsDateNextAuditNull() && 

                
                if (myCategoryLevelListsRowId != -1 && isRunAutomaticWorkflow == "Y" && myAuditFrequencyListsRowId != -1
                    && myStartWorkflowPriorToAuditListsRowId != -1
                    && ((!row.IsDateNextAuditNull() && 
                    GASystem.BusinessLayer.WorkflowStarter.GetNextDate(myStartWorkflowPriorToAuditListsRowId, row.DateNextAudit, "subtract") <= DateTime.UtcNow.Date)
                    || (!row.IsDateLastAuditNull() && row.IsDateNextAuditNull() && GASystem.BusinessLayer.WorkflowStarter.GetNextDate(
                    myAuditFrequencyListsRowId, row.DateLastAudit, "add") <= DateTime.UtcNow.Date)))
                {
                    // Tor 20150704 added
                    System.Console.WriteLine("Transaction Start "+row.Title+" RowId: "+row.SubcontractorViewRowId.ToString());
                    GADataTransaction Sctransaction = GADataTransaction.StartGADataTransaction();

                    try
                    {
                        // Tor 20150704 added end

                        row.CategoryLevelListsRowId = myCategoryLevelListsRowId;
                        row.DeletedBy = myClassificationMatrixValue;
                        row.SubContractorAuditRequiredListsRowId = myAuditFrequencyListsRowId;
                        row.DateLastAudit = GASystem.BusinessLayer.WorkflowStarter.GetNextDate(myStartWorkflowPriorToAuditListsRowId, DateTime.UtcNow.Date, "add");
                        row.DateNextAudit = GASystem.BusinessLayer.WorkflowStarter.GetNextDate(myAuditFrequencyListsRowId, row.DateLastAudit, "add");

                        myAuditPersonnelRowId = 0;
                        myAuditResponsibleRoleListsRowId = 0;

                        // set contract owner role if empty
                        if (row.IsRXTContractholderPersonnelRowIdNull()) row.RXTContractholderPersonnelRowId = defaultRoleRowId;
                        // get first person with role:  (RXTContractholderPersonnelRowId "OceanGeo Contract Owner" ER), default
                        if (!row.IsRXTContractholderPersonnelRowIdNull())
                        {
                            myAuditResponsibleRoleListsRowId = row.RXTContractholderPersonnelRowId;
                            myAuditPersonnelRowId = GetProcessCoordinatorPersonnelRowId(row.RXTContractholderPersonnelRowId, row, myCurrentWorkflowClass);
                        }

                        if (myAuditPersonnelRowId == 0)
                        {
                            myAuditResponsibleRoleListsRowId = defaultRoleRowId;
                            row.RXTContractholderPersonnelRowId = defaultRoleRowId;
                            myAuditPersonnelRowId = GetProcessCoordinatorPersonnelRowId(row.RXTContractholderPersonnelRowId, row, myCurrentWorkflowClass);
                            if (myAuditPersonnelRowId == 0) myAuditPersonnelRowId = defaultPersonnelRowId;
                        }

                        UserDS uds1 = new UserDS();
                        uds1 = GASystem.BusinessLayer.User.GetUserByPersonnelRowId(myAuditPersonnelRowId);
                        userName = (uds1.Tables.Count > 0) ? userName = uds1.GAUser[0].DNNUserId : defaultUserId;

                        // IntFree2 Subcontractor Audit Status DCDS

                        System.Console.WriteLine("Create Workflow for GASubcontractorview for Vendor: " + row.Title);
                        GADataRecord owner = new GADataRecord(row.SubcontractorViewRowId, GADataRecord.ParseGADataClass(myCurrentWorkflowClass));
                        Action action = new Action();
                        ActionDS ads = (ActionDS)action.GetNewRecord(owner, transaction);
                        ActionDS.GAActionRow newRow = (ActionDS.GAActionRow)ads.GAAction.Rows[0];
                        string myAuditResponsibleRole = GASystem.BusinessLayer.Lists.GetListValueByRowId(row.RXTContractholderPersonnelRowId);
                        newRow.ActionName = pds.GAProcedure[0].Shortname;
                        GASystem.BusinessLayer.Utils.IdGenerator idgen = new GASystem.BusinessLayer.Utils.IdGenerator(GADataClass.GAAction, owner, Sctransaction);
                        idgen.ApplyReferenceId(ads);

                        newRow.ProcedureRowId = pds.GAProcedure[0].ProcedureRowId;
                        newRow.Description = pds.GAProcedure[0].Description;
                        newRow.Subject = pds.GAProcedure[0].Shortname;
                        newRow.ActionStatusListsRowId = ActionStatusRowId;
                        newRow.PriorityListsRowId = PriorityRowId;
                        newRow.ResponsibleRoleListsRowId = myAuditResponsibleRoleListsRowId;
                        newRow.RegisteredByPersonnelRowId = myAuditPersonnelRowId; //GetInitiatorPersonnelRowId(row.DocumentControlRowId,GADataRecord.ParseGADataClass(myCurrentWorkflowClass),DateTime.Now,row.RevisionResponsibleRoleListsRowId, transaction);
                        newRow.WorkflowId = "StartPending:" + userName;
                        newRow.DateEndEstimated = row.DateLastAudit;

                        //DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(GADataClass.GAAction, transaction);
                        DataAccess.DataAccess gada = new GASystem.DataAccess.DataAccess(GADataClass.GAAction, Sctransaction);
                        DataSet dsresult = gada.Update(ads);
                        //DataClassRelations.CreateDataClassRelation(owner.RowId, owner.DataClass, (int)dsresult.Tables["GAAction"].Rows[0]["ActionRowId"], GADataClass.GAAction, transaction);
                        DataClassRelations.CreateDataClassRelation(owner.RowId, owner.DataClass, (int)dsresult.Tables["GAAction"].Rows[0]["ActionRowId"], GADataClass.GAAction, Sctransaction);

                        row.IntFree2 = GASystem.BusinessLayer.WorkflowStarter.GetListsRowId(StatusCategory, StatusNextRevisionStarted);
// Tor 20150704 Moved here from below
                        GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GASubcontractorView);
                        bc.UpdateDataSet(ds, Sctransaction);
// Tor 20150704 Moved here from below end

                    }
                    catch (Exception ex)
                    {
                        System.Console.WriteLine("Transaction Rollback "+row.Title+" RowId: "+row.SubcontractorViewRowId.ToString());
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
            // update dataset
            //GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GASubcontractorView);
            //bc.UpdateDataSet(ds, transaction);
            return ds;
        }

        private static int GetProcessCoordinatorPersonnelRowId(int RoleListsRowId,SubcontractorViewDS.GASubcontractorViewRow row, string myCurrentWorkflowClass)
        {
            int personnelRowId = 0;

            // Tor 20170328 EmploymentDS eds = GASystem.BusinessLayer.Employment.SearchForEmploymentsByOwnerDateAndRoleId(
            EmploymentDS eds =GASystem.BusinessLayer.Employment.SearchForEmploymentsByOwnerDateAndJobTitle(
                new GADataRecord(row.SubcontractorViewRowId, GADataRecord.ParseGADataClass(myCurrentWorkflowClass))
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
                        return eds.GAEmployment[i].Personnel;
                    i++;
                }
            }
            return personnelRowId;
        }

        private static string GetCoordinatorWorkitemRole()
        {
            // Tor 20160303 return System.Configuration.ConfigurationManager.AppSettings.Get("SubContractorCoordinatorWorkitemRole");
            return new GASystem.AppUtils.FlagSysResource().GetResourceString("SubContractorCoordinatorWorkitemRole"); 

        }

        private static string GetDefaultUserId()
        {
            // Tor 20160303 return System.Configuration.ConfigurationManager.AppSettings.Get("SubContractorCoordinatorWorkitemUserId");
            return new GASystem.AppUtils.FlagSysResource().GetResourceString("SubContractorCoordinatorWorkitemUserId");
        }

        private static int GetCoordinatorWorkitemPersonnelRowId()
        {
            PersonnelDS pds = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelGivenNameAndFamilyName(
                // Tor 20160303 System.Configuration.ConfigurationManager.AppSettings.Get("SubContractorCoordinatorWorkitemGivenName"),
                new GASystem.AppUtils.FlagSysResource().GetResourceString("SubContractorCoordinatorWorkitemGivenName"),
                // Tor 20160303 System.Configuration.ConfigurationManager.AppSettings.Get("SubContractorCoordinatorWorkitemFamilyName"));
                new GASystem.AppUtils.FlagSysResource().GetResourceString("SubContractorCoordinatorWorkitemFamilyName"));
            if (pds.Tables[0].Rows.Count == 0)
            {
                // Tor 20160303 return int.Parse(System.Configuration.ConfigurationManager.AppSettings.Get("SubContractorCoordinatorWorkitemPersonnelRowId"));
                return int.Parse(new GASystem.AppUtils.FlagSysResource().GetResourceString("SubContractorCoordinatorWorkitemPersonnelRowId"));
            }
            return pds.GAPersonnel[0].PersonnelRowId;
        }

        private static DateTime GetNextDateAfterPrior(DateTime myBaseDate, int myAutoWorkflowStartPriorToNextAuditListsRowId, int myRevisionFrequencyListsRowId)
        {
            return GASystem.BusinessLayer.WorkflowStarter.GetNextDate(
                myAutoWorkflowStartPriorToNextAuditListsRowId,
                GASystem.BusinessLayer.WorkflowStarter.GetNextDate(myRevisionFrequencyListsRowId, myBaseDate, "add"),
                "subtract");
        }

        private static int ComputeClassificationMatrixValue(int rowId) // returns compute ClassificationMatrixValue from Audit result and probability
        {
            return GASystem.DataAccess.SubcontractorViewDb.ComputeClassificationMatrixValue(rowId);
        }

        private static int ComputeCategoryLevelListsRowId(int rowId) // returns compute ClassificationMatrixValue from Audit result and probability
        {
            return GASystem.DataAccess.SubcontractorViewDb.ComputeCategoryLevelListsRowId(rowId);
        }
    }
}

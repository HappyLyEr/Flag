using System;
using System.Collections;
using GASystem.DataModel;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;
using SautinSoft.HtmlToRtf;
using GASystem.AppUtils;
using GASystem.DataModel.View;
using System.Data.SqlClient;
using GASystem.DataAccess;
using System.Collections.Generic;


namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for GAReport.
	/// </summary>
	public class GAReport
	{
		private int reportId;
		private int reportInstanceId;
		private string reportDocumentPath;
		//		private System.Collections.Hashtable reportAttributes;
		private GADataRecord _ownerRecord;
		private GADataRecord _dataRecord;
		private GADataClass _reportDataClass;
		private GADataRecord _reportContext;
		private bool IsDataRecordTopLevelClass = false;
        private string tmpPath; 

		private GAReport()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        /// <summary>
        /// GA Report constructor
        /// </summary>
        /// <param name="ReportId">Report definition id</param>
        /// <param name="ReportDocumentPath">Absolutt path to report definition folder</param>
		public GAReport(int ReportId, string ReportDocumentPath)
		{
			this.reportId = ReportId;
			reportDocumentPath = ReportDocumentPath;
			//add backslash to end of path if missing
			if (!reportDocumentPath.EndsWith("\\"))
				reportDocumentPath = reportDocumentPath + "\\";
		}

		

		public int ReportInstanceId 
		{
			set {reportInstanceId = value;}
			get {return reportInstanceId;}
		}
		

		public GADataRecord Owner 
		{
			get {return _ownerRecord;}
			set {_ownerRecord = value;}
		}
	
		public GADataRecord DataRecord 
		{
			get {return _dataRecord;}
			set {_dataRecord = value;}
		}

		public GADataClass ReportDataClass 
		{
			get {return _reportDataClass;}
			set {_reportDataClass = value;}
		}

		public GADataRecord ReportContext 
		{
			get {return _reportContext;}
			set {_reportContext = value;}
		}

        /// <summary>
        /// Generate a crystal report. Loads defnition, data and parameters
        /// </summary>
        /// <returns></returns>
		public ReportDocument GenerateCrystalReportDocument() 
		{
			CrystalDecisions.CrystalReports.Engine.ReportDocument reportDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
			
			if (DataRecord.DataClass != GADataClass.GAReportInstance) 
			{
				//load data for a single class report.  TODO refactor into a seperate class
                reportDoc.Load(reportDocumentPath + DataRecord.DataClass + ".rpt");
				
				//get data for the datarecord reported upon.
				DataSet ds = DataAccess.ReportView.GetRecordSetAllDetailsByDataRecord(DataRecord);
                this.formatDataForCrystal(ds, DataRecord.DataClass);
				GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataRecord.DataClass);
				
				if (cd != null)
					IsDataRecordTopLevelClass = cd.IsTop;



				foreach(Table reportTable in reportDoc.Database.Tables) 
				{
					if (reportTable.Name != DataRecord.DataClass.ToString() )   //&& reportTable.Name != GADataClass.GAUsers.ToString()) 
					{
						ds.Merge(GetDataForReportTable(reportTable, DataRecord));
					}
				}
				
				reportDoc.SetDataSource(ds);
				
				AppUtils.DateRangeGenerator.AbstractDateRange dateRange;
				if (DataRecord.DataClass == GADataClass.GAReportInstance)
					dateRange = BusinessLayer.ReportInstance.GetDateRange(ReportInstanceId);
				else
					dateRange = AppUtils.DateRangeGenerator.DateRangeFactory.Make(AppUtils.DateRangeGenerator.DateRangeEnum.AdHoc, System.DateTime.Now, System.DateTime.Now);
			

				//load data for sub reports
				
				
				//moved the definition for subDS here, because of bug when adding data for multiple subreports. setting subreport data for the second etc. subreport seams to 
				//override data for pevious sub reports reports. Could be because the when setting data for subsequent subreport the ReportView contains empty tables to other data 
				//then the one in the current subreport. Works now that I keep filling data to the same dataset.
				GASystem.DataModel.View.ReportView allDS = new GASystem.DataModel.View.ReportView();
				//need to add main data too
				allDS.Merge(ds);


                //ArrayList visitedTables = new ArrayList();
                System.Collections.Generic.List<string> visitedTables = new System.Collections.Generic.List<string>();
                foreach (CrystalDecisions.CrystalReports.Engine.ReportObject repObject in reportDoc.ReportDefinition.ReportObjects) 
				{
					if (typeof(CrystalDecisions.CrystalReports.Engine.SubreportObject) == repObject.GetType()) 
					{
						ReportDocument subReportDoc = reportDoc.OpenSubreport(((SubreportObject)repObject).SubreportName);
						//DataSet subDS = new DataSet();
						
						foreach(Table subReportTable in subReportDoc.Database.Tables) 
						{
                            if (!visitedTables.Contains(subReportTable.Location))
                            {
                                DataSet subDS;
                                switch (subReportTable.Location)
                                {
                                    case "GAMeetingToolboxView":
                                    case "GAClientFeedbackViewEvaluationForm":
                                    case "GAClientFeedbackViewProject":
                                        subDS = GetSubDSForReportTable(subReportTable, DataRecord);
                                        break;
                                    default:
                                        subDS = GetDataForReportTable(subReportTable, DataRecord);
                                        break;
                                }
                                
                                allDS.Merge(subDS);
                                visitedTables.Add(subReportTable.Location);
                            }
						}
						reportDoc.OpenSubreport(((SubreportObject)repObject).SubreportName).SetDataSource(allDS);
					}
	
				}

                //check for parameters
                //reportDoc.DataDefinition.ParameterFields[1].
                //set default params for data
                foreach (CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinition param in reportDoc.DataDefinition.ParameterFields)
                {
                    object paramValue = string.Empty;
                    if (param.Name == "OwnerName")
                    {
                        DataSet dsOwner = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(Owner);
                        if (dsOwner.Tables[0].Columns.Contains("name"))
                        {
                            paramValue = dsOwner.Tables[0].Rows[0]["name"].ToString();
                        }
                    }

                    else if (param.Name == "StartDate")
                    {
                        if (param.ParameterValueKind == CrystalDecisions.Shared.ParameterValueKind.DateParameter ||
                                    param.ParameterValueKind == CrystalDecisions.Shared.ParameterValueKind.DateTimeParameter)
                            paramValue = dateRange.GetDateFrom();
                        else
                            paramValue = dateRange.GetDateFrom().ToLongDateString();

                    }
                    else if (param.Name == "EndDate")
                    {
                        if (param.ParameterValueKind == CrystalDecisions.Shared.ParameterValueKind.DateParameter ||
                                       param.ParameterValueKind == CrystalDecisions.Shared.ParameterValueKind.DateTimeParameter)
                            paramValue = dateRange.GetDateTo();
                        else
                            paramValue = dateRange.GetDateTo().ToLongDateString();
                    }

                    else if (param.Name == "ReportPath")
                    {
                        paramValue = reportDocumentPath;
                    }
                        
                    // Tor 20161023 Added paramenter option in Crystal Report
                    else if (param.Name == "ReportOwnerClass")
                    {
                        paramValue = this._dataRecord.DataClass.ToString();
                    }
                    else if (param.Name == "ReportOwnerOwnerClass")
                    {
                        paramValue = this._ownerRecord.DataClass.ToString();
                    }

                    reportDoc.SetParameterValue(param.Name, paramValue);

                }
				

			} 
			else 
			{
				return generateReportInstanceReport();
				
			}

	
			return reportDoc;
		}

        private DataSet GetSubDSForReportTable(Table subReportTable, GADataRecord DataRecord)
        {
            DataSet mainDS = new DataSet();
            string ownerRowIdName = DataRecord.DataClass.ToString().Substring(2) + "rowid";
            string sql = string.Format("select * from {0} where {1}={2}",
                subReportTable.Location,
                ownerRowIdName,
                DataRecord.RowId);
            SqlConnection myConnection = DataUtils.GetConnection(null);
            SqlDataAdapter da = new SqlDataAdapter(sql, myConnection);
            da.Fill(mainDS, subReportTable.Location);
            if (subReportTable.Location == "GAMeetingToolboxView")
            {
                string notIn = "0,";
                foreach (DataRow row in mainDS.Tables["GAMeetingToolboxView"].Rows)
                {
                    notIn += row["ListsRowId"].ToString() + ",";
                }
                DataSet naDS = new DataSet();
                string naSql = string.Format(@"
Select case
           when a.GAListValue in ('TheFirstOperation', 'NewCrewMemebet', 'DifferentRole', 'UnknowedOfTasks',
                                  'AnyChanges', 'Affections', 'SmallBoatMOPO', 'UnawareOfRisks') then 'PART1'
           when a.GAListValue in ('IsReviewedTRA', 'IsReadSafe', 'HasUndertakenTRA', 'IsCompletedMoC') then 'PART2'
           else 'PART3' end as Part,
       {0}      as MeetingRowId,
       a.ListsRowId,
       N'N/A' as nTextFree1,
       a.GAListDescription,
       a.Sort1
from GALists a
where GAListCategory = 'TBWC' and ListsRowId not in ({1})", DataRecord.RowId, notIn.TrimEnd(','));
                SqlDataAdapter naAdapter = new SqlDataAdapter(naSql, myConnection);
                naAdapter.Fill(naDS, subReportTable.Location);
                mainDS.Merge(naDS);
            }
            if (myConnection.State != ConnectionState.Closed)
                myConnection.Close();
            return mainDS;
        }

		/// <summary>
		/// Get data for a table definition in Crystal Report
		/// </summary>
		/// <param name="ReportTable">Crystal report definition table</param>
		/// <param name="Owner">Owner datarecord to get data for</param>
		/// <returns></returns>
		private DataSet 
            GetDataForReportTable(Table ReportTable, GADataRecord Owner) 
		{
			DataSet lds = new DataSet();
		//	if (ReportTable.Location.ToUpper().IndexOf("VIEW") > -1)
            if (ReportTable.Location.ToUpper().IndexOf("GAACTIONWORKITEMVIEW") > -1) 

			{
                ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(ReportTable.Location);


                //  ----- if not virtual view   -- DO THIS
                if (!cd.hasViewSQL())
                {
                    //table is a view. Return data from a ga view
                    GASystem.DataModel.FieldDefinitionDS fds = GASystem.DataAccess.FieldDefinitionDb.GetLookupTableFieldsForTable(ReportTable.Location, DataRecord.DataClass.ToString());
                    if (fds.GAFieldDefinitions.Count > 0)
                    {
                        //create filter
                        // added tablename to string filter = fds.GAFieldDefinitions[0].FieldId + " = " + DataRecord.RowId;
                        string filter = fds.GAFieldDefinitions[0].TableId + "." + fds.GAFieldDefinitions[0].FieldId + " = " + DataRecord.RowId;
                        //get context
                        //GADataRecord CurrentContext = GASystem.AppUtils.SessionManagement.GetCurrentDataContext().InitialContextRecord;


                        //get data
                        //ds = Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(reportTableDataClass, CurrentContext, filter);
                        lds = DataAccess.ReportView.GetRecordSetAllDetails(ReportTable.Location, filter);
                    }

                }
                else
                {
                    //  -----------  if virtual view   -- DO THIS

                    GASystem.DataAccess.Utils.SQLView.FlagClassDefinedSQLView virtView = new GASystem.DataAccess.Utils.SQLView.FlagClassDefinedSQLView(cd, Owner);

                    String sql = virtView.GetSQLViewQuery();


                    System.Data.SqlClient.SqlConnection myConnection = new System.Data.SqlClient.SqlConnection(GASystem.DataAccess.DataUtils.getConnectionString());
                    System.Data.SqlClient.SqlDataAdapter da = new System.Data.SqlClient.SqlDataAdapter(sql, myConnection);


                    //DataSet ds = new DataSet();
                    DataModel.View.ReportView ds = new GASystem.DataModel.View.ReportView();
                    //adjust from and to date. SqlServer dates must be between 1753 and 9999
                        DateTime startDate = new DateTime(1754, 1, 1);

                        DateTime endDate = new DateTime(9999, 1, 1);


                    da.SelectCommand.Parameters.AddWithValue("@dateFrom", startDate);
                    da.SelectCommand.Parameters.AddWithValue("@DateTo", endDate);


                    da.Fill(lds, ReportTable.Location);
                    myConnection.Close();

                }


                ////table is a view. Return data from a ga view
                //GASystem.DataModel.FieldDefinitionDS fds = GASystem.DataAccess.FieldDefinitionDb.GetLookupTableFieldsForTable(ReportTable.Location, DataRecord.DataClass.ToString());
                //if (fds.GAFieldDefinitions.Rows.Count > 0) 
                //{
                //    //create filter
                //    string filter = fds.GAFieldDefinitions[0].FieldId + " = " + DataRecord.RowId;
                //    //get context
                //    //GADataRecord CurrentContext = GASystem.AppUtils.SessionManagement.GetCurrentDataContext().InitialContextRecord;

					
                //    //get data
                //    //ds = Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(reportTableDataClass, CurrentContext, filter);
                //    lds = DataAccess.ReportView.GetRecordSetAllDetails(ReportTable.Location, filter);
                //}	
			} 
			else 
			{		
				lds = GetDataForSingelRecordReportTable(GADataRecord.ParseGADataClass(ReportTable.Location), IsDataRecordTopLevelClass);
				if (ReportTable.Location != ReportTable.Name) 
				{
					//report alias and table name is different. get table and rename it in the dataset
					lds.Tables[ReportTable.Location].TableName = ReportTable.Name;
				} 
			}
			
			
            //patch lds for html data
            patchHTMLData(lds, GADataRecord.ParseGADataClass(ReportTable.Location));



            //preformatting of data for crystal reports
            formatDataForCrystal(lds, GADataRecord.ParseGADataClass(ReportTable.Location));



			return lds;



		}

        private void formatDataForCrystal(DataSet lds, GADataClass dataClass)
        {
            //TODO refactor into a factory solution

            
            //format selectlistcolumns)
            FieldDescription[] fds = GASystem.AppUtils.FieldDefintion.GetFieldDescriptions(dataClass);

            foreach (FieldDescription fd in fds)
            {
                if (fd.ControlType.ToUpper() == "SELECTLIST" && lds.Tables[dataClass.ToString()].Columns.Contains(fd.FieldId))
                {
                    GASystem.BusinessLayer.BusinessClass bcListsSelected = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAListsSelected);
                    foreach (DataRow row in lds.Tables[dataClass.ToString()].Rows)
                    {
                        string selectListValue = string.Empty;
                        DataSet dsListsSelected = bcListsSelected.GetByOwner(new GADataRecord((int)row[dataClass.ToString().Substring(2) + "rowid"],dataClass) , null);
                        foreach (ListsSelectedDS.GAListsSelectedRow slRow in dsListsSelected.Tables[0].Rows)
                        {
                            if(fd.FieldId.ToUpper() == slRow.FieldId.ToUpper())
                                selectListValue += Lists.GetListDescriptionByRowId(slRow.ListsRowId) + System.Environment.NewLine;
                        }
                        row[fd.FieldId] = selectListValue;
                    }
                }
                
            }


            lds.AcceptChanges();

        }




		/// <summary>
		/// Return data for a table used in a single record report.
		/// </summary>
		/// <returns></returns>
		private DataSet GetDataForSingelRecordReportTable(GADataClass reportTableDataClass, bool isTopLevel)
		{
//			DataSet ds = DataAccess.ReportView.GetRecordSetAllDetailsForDataClassByOwner(
//				reportTableDataClass,
//				DataRecord, string.Empty);
//			
			DataSet ds;

			if (DataRecord.DataClass != GADataClass.GAAudit)  //HACK JOF 02.05.06  documents attached to audits may have actions, need to add these to the report
				ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(reportTableDataClass, DataRecord);
			else
				ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(reportTableDataClass, DataRecord, string.Empty);
			
			
			if (ds.Tables[reportTableDataClass.ToString()].Rows.Count == 0 && isTopLevel) 
			{
				//found no data as children, try to find data where DataRecord is referenced

				//find reference field
				GASystem.DataModel.FieldDefinitionDS fds = GASystem.DataAccess.FieldDefinitionDb.GetLookupTableFieldsForTable(reportTableDataClass.ToString(), DataRecord.DataClass.ToString());
				if (fds.GAFieldDefinitions.Rows.Count > 0) 
				{
					//create filter
					string filter = reportTableDataClass.ToString() + "." + fds.GAFieldDefinitions[0].FieldId + " = " +  DataRecord.RowId;
					//get context
					GADataRecord CurrentContext = GASystem.AppUtils.SessionManagement.GetCurrentDataContext().InitialContextRecord;

					
					//get data
					ds = Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(reportTableDataClass, CurrentContext, filter);
				}
			}

			return ds;
		}

		/// <summary>
		/// Generate report based on a report instance
		/// </summary>
		private ReportDocument generateReportInstanceReport() 
		{
			CrystalDecisions.CrystalReports.Engine.ReportDocument reportDoc = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
			
			ReportsDS gaReport = GASystem.BusinessLayer.Reports.GetReportsByReportsRowId(reportId);
			ReportsDS.GAReportsRow  gaReportRow = gaReport.GAReports[0];
			
			//load report document
			reportDoc.Load(reportDocumentPath + gaReportRow.ReportFile);

			//report context holds owner of reportinstance. include data for this datarecord
			DataSet ds = DataAccess.ReportView.GetRecordSetAllDetailsByDataRecord(this.ReportContext);
				
			foreach(Table reportTable in reportDoc.Database.Tables) 
			{
				//				ReportBuilder rb = new ReportBuilder();
				//
                System.Collections.Generic.List<string> foundTables = new System.Collections.Generic.List<string>();
                

				if (reportTable.Location != ReportContext.DataClass.ToString() && reportTable.Name != GADataClass.GAUsers.ToString()) 
				{
                    if (!foundTables.Contains(reportTable.Location))
                    {

                        ds.Merge(GetDataForReportTableByReportInstance(reportTable, ReportContext, ReportInstanceId));
                        foundTables.Add(reportTable.Location);

                    }
				}
			}
			
			reportDoc.SetDataSource(ds);

		

            //get daterange. used for setting parameters
            AppUtils.DateRangeGenerator.AbstractDateRange dateRange;
            if (DataRecord.DataClass == GADataClass.GAReportInstance)
                dateRange = BusinessLayer.ReportInstance.GetDateRange(ReportInstanceId);
            else
                dateRange = AppUtils.DateRangeGenerator.DateRangeFactory.Make(AppUtils.DateRangeGenerator.DateRangeEnum.AdHoc, System.DateTime.Now, System.DateTime.Now);
            




			//moved the definition for subDS here, because of bug when adding data for multiple subreports. setting subreport data for the second etc. subreport seams to 
			//override data for pevious sub reports reports. Could be because the when setting data for subsequent subreport the ReportView contains empty tables to other data 
			//then the one in the current subreport. Works now that I keep filling data to the same dataset.
			GASystem.DataModel.View.ReportView subDS = new GASystem.DataModel.View.ReportView();
			//need to add main data too
			subDS.Merge(ds);
			foreach (CrystalDecisions.CrystalReports.Engine.ReportObject repObject in reportDoc.ReportDefinition.ReportObjects) 
			{
				if (typeof(CrystalDecisions.CrystalReports.Engine.SubreportObject) == repObject.GetType()) 
				{
					ReportDocument subReportDoc = reportDoc.OpenSubreport(((SubreportObject)repObject).SubreportName);
					//DataSet subDS = new DataSet();
                    foreach (Table subReportTable in subReportDoc.Database.Tables)
                    {
                        DataSet tmpDS = GetDataForReportTableByReportInstance(subReportTable, ReportContext, ReportInstanceId);
                        if (subReportTable.Location == GADataClass.GALocation.ToString()
                            || subReportTable.Location == GADataClass.GAMeeting.ToString()
                            || subReportTable.Location == GADataClass.GACourse.ToString())
                        {
                            DataTable uniqueTable = ReturnUniqueTable(tmpDS);
                            subDS.Merge(uniqueTable);
                        }
                        else
                        {
                            subDS.Merge(tmpDS);
                        }
                    }
                    reportDoc.OpenSubreport(((SubreportObject)repObject).SubreportName).SetDataSource(subDS);
				}
	
			}

             setParameters(reportDoc, dateRange);


			return reportDoc;
		}

        private DataTable ReturnUniqueTable(DataSet ds)
        {
            DataTable defautTable = ds.Tables[0];
            List<string> columns = new List<string>();
            foreach (DataColumn col in defautTable.Columns)
            {
                columns.Add(col.ColumnName);
            }
            DataTable uniqueTable = defautTable.DefaultView.ToTable(true, columns.ToArray());
            return uniqueTable;
        }

        /// <summary>
        /// Set parameters for a reportdefinition. Gets a list of parameters from the reportdefinition
        /// </summary>
        /// <param name="reportDoc">Reportdefinition</param>
        /// <param name="dateRange">Daterange. Used for setting datefrom and dateto parameters</param>
        private void setParameters(ReportDocument reportDoc, AppUtils.DateRangeGenerator.AbstractDateRange dateRange)
        {
            ////set parameters
            foreach (CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinition param in reportDoc.DataDefinition.ParameterFields)
            {
                if (param.Name == "OwnerName")
                {
                    string paramValue = string.Empty;
                    DataSet dsOwner = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(Owner);
                    if (dsOwner.Tables[0].Columns.Contains("name"))
                    {
                        paramValue = dsOwner.Tables[0].Rows[0]["name"].ToString();
                    }
                    reportDoc.SetParameterValue("OwnerName", paramValue);


                }
                else if (param.Name == "StartDate")
                    if (param.ParameterValueKind == CrystalDecisions.Shared.ParameterValueKind.DateParameter
                            || param.ParameterValueKind == CrystalDecisions.Shared.ParameterValueKind.DateTimeParameter)
                        reportDoc.SetParameterValue("StartDate", dateRange.GetDateFrom());
                    else
                        reportDoc.SetParameterValue("StartDate", dateRange.GetDateFrom().ToLongDateString());
                else if (param.Name == "EndDate")
                    if (param.ParameterValueKind == CrystalDecisions.Shared.ParameterValueKind.DateParameter
                            || param.ParameterValueKind == CrystalDecisions.Shared.ParameterValueKind.DateTimeParameter)
                        reportDoc.SetParameterValue("EndDate", dateRange.GetDateTo());
                    else
                        reportDoc.SetParameterValue("EndDate", dateRange.GetDateTo().ToLongDateString());
                else if (param.Name == "TotalExposedHoursLocation")
                {

                    DailyEmployeeCount eCount = new DailyEmployeeCount();
                    if (Owner.DataClass == GADataClass.GALocation || Owner.DataClass == GADataClass.GAProject)
                        reportDoc.SetParameterValue("TotalExposedHoursLocation", eCount.GetTotalExsposedHoursToDate(Owner, dateRange.GetDateFrom()));
                    else
                        reportDoc.SetParameterValue("TotalExposedHoursLocation", 0);

                }
                else if (param.Name == "TotalExposedHoursProject")
                {
                    DailyEmployeeCount eCount = new DailyEmployeeCount();
                    if (Owner.DataClass == GADataClass.GAProject)
                        reportDoc.SetParameterValue("TotalExposedHoursProject", eCount.GetTotalExsposedHoursToDate(Owner, dateRange.GetDateFrom()));
                    else
                        try
                        {
                            GADataRecord ownerProject = getLinkedOwner(GADataClass.GAProject, dateRange.GetDateFrom());
                            reportDoc.SetParameterValue("TotalExposedHoursProject", eCount.GetTotalExsposedHoursToDate(ownerProject, dateRange.GetDateFrom()));
                        }
                        catch (GAExceptions.GAException gaex)
                        {
                            //gaexpetion is most likely due to not finding linked owner fo type project. set total to 0
                            //TODO: write to log
                            reportDoc.SetParameterValue("TotalExposedHoursProject", 0);
                        }
                }
                else if (param.Name == "ReportPath")
                {
                    reportDoc.SetParameterValue(param.Name, reportDocumentPath); 
                }
                else if (param.Name == "Vertical")
                {
                    //reportDoc.
                    ReportInstanceDS dsi = BusinessLayer.ReportInstance.GetReportInstanceByReportInstanceRowId(this.ReportInstanceId);

                    if (dsi.Tables[0].Rows[0]["IntFree1"] != DBNull.Value)
                    {
                        int verticalid = (int)dsi.Tables[0].Rows[0]["IntFree1"];
//                        reportDoc.SetParameterValue("Vertical", BusinessLayer.Lists.GetListValueByRowId(verticalid));
                        reportDoc.SetParameterValue("Vertical", BusinessLayer.Lists.GetListDescriptionByRowId(verticalid));
                    }
                    else 
                    {
                        reportDoc.SetParameterValue("Vertical", "");
                    }

                }   
                else
                    if (param.ParameterValueKind == CrystalDecisions.Shared.ParameterValueKind.StringParameter)
                        reportDoc.SetParameterValue(param.Name, "");

                //reportDoc.
                //ReportInstanceDS dsi2 = BusinessLayer.ReportInstance.GetReportInstanceByReportInstanceRowId(this.ReportInstanceId);

                //int verticalid2 = (int)dsi2.Tables[0].Rows[0]["IntFree1"];
                //reportDoc.SetParameterValue("Vertical", BusinessLayer.Lists.GetListValueByRowId(verticalid2));


            }
        }



		private GADataRecord getLinkedOwner(GADataClass DataClass, DateTime date) 
		{
			BusinessClass bc = Utils.RecordsetFactory.Make(this.Owner.DataClass);
			DataSet ds = bc.GetManyToManyOwnerRecords(DataClass, date, Owner.RowId);
			if (ds.Tables[0].Rows.Count == 0)
				throw new GAExceptions.GAException("Could not find a linked owner to this report of type " + AppUtils.Localization.GetGuiElementText(DataClass.ToString()));

			return new GADataRecord((int)ds.Tables[0].Rows[0][DataClass.ToString().Substring(2) + "rowid"], DataClass);
		}

		private DataSet GetDataForReportTableByReportInstance(Table ReportTable, GADataRecord Owner, int ReportInstance)  
		{
			ReportBuilder rb = new ReportBuilder();
			GADataClass reportDataClass;
					
			//start test						//

           // GASystem.DataModel.View.ReportView rds = new ReportView();
            
			DataSet rds = new DataSet();
//			if (ReportTable.Location.ToUpper().IndexOf("VIEW") > -1) 
//			{
//				rds = DataAccess.ReportView.GetRecordSetAllDetailsWithinOwnerByReportInstance(ReportTable.Location, ReportContext, string.Empty, ReportInstanceId);
//			}
//			else 
//			{
					
				//end test
				reportDataClass = GADataRecord.ParseGADataClass(ReportTable.Location);
				//rb.GenerateDateFilter(ReportInstanceId, reportDataClass);
					
				//report alias and table names are equal, just add the table as it is defined in ga

                rds = rb.FindAllMembersByDate(ReportContext, reportDataClass, ReportInstanceId);
//				GASystem.BusinessLayer.BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(reportDataClass);
//
//				rds = bc.GetAllRecordsWithinOwnerAndLinkedRecords(ReportContext);// BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassWithinOwner(myDataClass, myOwner, filter);//rb.FindAllMembers(myOwner, myDataClass);
//				
				//rds = GASystem.BusinessLayer.Utils.RecordsetFactory.
//			}

			if (ReportTable.Location != ReportTable.Name) 
			{
				rds.Tables[ReportTable.Location].TableName = ReportTable.Name;
			} 
			//					else 
			//					{
			//						//report alias and table names are not equal, just add the table and rename to alias
			//						reportDataClass = GADataRecord.ParseGADataClass(reportTable.Location);
			//						//rb.GenerateDateFilter(ReportInstanceId, reportDataClass);
			//						DataSet dsReportTable = rb.FindAllMembersByDate(ReportContext, reportDataClass, ReportInstanceId);
			//						

          //  formatDataForCrystal(rds, reportDataClass);

			return rds;
						
			//					}

		}



        protected string generateFileName(string TempPath) 
		{
			string newName = DataRecord.DataClass.ToString() + "_" + DataRecord.RowId.ToString();
			
			//add datetag
			DateTime dateTag = DateTime.Now;
			string dateTagString = dateTag.Ticks.ToString();

		
			return TempPath + newName + dateTagString + ".tmp";
		}

        /// <summary>
        /// Metod for cleaning html data in dataset so that active reports can display it correctly
        /// </summary>
        /// <param name="ds"></param>
        protected void patchHTMLData(System.Data.DataSet ds, GADataClass DataClass)
        {
            if (DataClass == GADataClass.GAMeetingText)
            {
                //try
                //{
                if (ds.Tables[GADataClass.GAMeetingText.ToString()].Columns.Contains("Text"))
                {
                    foreach (System.Data.DataRow row in ds.Tables[GADataClass.GAMeetingText.ToString()].Rows)
                    {
                        if (row["Text"] != DBNull.Value)
                        {
                            //commented out d.t test of HTML2RTF

                            //string rowValue = row["Text"].ToString();

                            string rowValue = row["Text"].ToString();

                            if (rowValue.Trim() != string.Empty)
                            {

                                string convertFileName = generateFileName(this.TempPath) + "mt";

                                //row["MeetingTextRowId"].ToString() + ;
                                string outFileName = convertFileName.Replace("tmpmt", "rtf");



                                Converter htlmrtf = new Converter();
                                htlmrtf.OutputTextFormat = eOutputTextFormat.Rtf;
                                htlmrtf.PageSize = ePageSize.A4;
                                htlmrtf.PreserveImages = 0;
                                System.IO.File.WriteAllText(convertFileName, rowValue);
                                htlmrtf.ConvertFile(convertFileName, this.TempPath);
                                rowValue = System.IO.File.ReadAllText(outFileName);
                                //rowValue = htlmrtf.ConvertString(rowValue);

                                try
                                {
                                    System.IO.File.Delete(outFileName);
                                    System.IO.File.Delete(outFileName);
                                }
                                catch (System.IO.IOException ex)
                                {
                                    //ignore file deletion in this case
                                    //TODO log
                                }

                                row["Text"] = rowValue;
                            }
                        }
                    }
                }

            }
        }

        public string TempPath
        {
            get
            {
                return tmpPath;
            }
            set
            {
                tmpPath = value;
            }
        }

	}






}
using System;
using GASystem.DataModel;
using GASystem.DataAccess;
using System.Data;

namespace GASystem.BusinessLayer
{
    public enum ExposedHoursSource { GATimeAndAttendance, GAExposedHoursGroupView }


	/// <summary>
	/// Summary description for DailyEmployeeCount.
	/// </summary>
	public class DailyEmployeeCount : BusinessClass
	{
		public DailyEmployeeCount()
		{
			this.DataClass = GADataClass.GADailyEmployeeCount;
			//
			// TODO: Add constructor logic here
			//
		}

		
		/// <summary>
		/// Get total exposed hours for owner until specified date. With optional record filter
		/// </summary>
		/// <param name="Owner">GADataRecord for which to get exposed hours. Must be a Location or a Project</param>
		/// <param name="ToDate">DateTime, To date for calculation</param>
		/// <param name="Filter">GADataRecord specifing a record to filter by</param>
		/// <returns>Integer, total numver of hours</returns>
		public int GetTotalExsposedHoursToDate(GADataRecord Owner, System.DateTime ToDate, GADataRecord Filter) 
		{
			string sqlFilter = string.Empty;
			if (Filter != null)
				sqlFilter = " path like '%" + Filter.DataClass.ToString() + "-" + Filter.RowId.ToString()  + "/%'";

            /*  sqlFilter = "(GASuperClass.ReadRoles = '" + Filter.DataClass.ToString() + "-" + Filter.RowId.ToString() + "'"
                           +" OR GASuperClass.UpdateRoles = '" + Filter.DataClass.ToString() + "-" + Filter.RowId.ToString() + "'"
                           +" OR GASuperClass.CreateRoles = '" + Filter.DataClass.ToString() + "-" + Filter.RowId.ToString() + "'"
                           +" OR GASuperClass.DeleteRoles = '" + Filter.DataClass.ToString() + "-" + Filter.RowId.ToString() + "'"
                           +" OR GASuperClass.TextFree1 = '" + Filter.DataClass.ToString() + "-" + Filter.RowId.ToString() + "'"
                           +" OR GASuperClass.TextFree2 = '" + Filter.DataClass.ToString() + "-" + Filter.RowId.ToString() + "';"
            */

            //set startdate to 1.1.1900
			System.DateTime startDate = new DateTime(1900,1,1);
			//get start date if present
			
			AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(Owner.DataClass);
			if (cd.hasDateFromField()) {
				BusinessClass bc = Utils.RecordsetFactory.Make(Owner.DataClass);
				DataSet dds = bc.GetByRowId(Owner.RowId);
				if (dds.Tables[0].Columns.Contains(cd.DateFromField))
					if (dds.Tables[0].Rows[0][cd.DateFromField] != DBNull.Value)
						startDate = (DateTime)dds.Tables[0].Rows[0][cd.DateFromField];

			}
		
			DailyEmployeeCountDS  ds =  (DailyEmployeeCountDS) GetAllRecordsWithinOwnerAndLinkedRecords(Owner, startDate, ToDate, sqlFilter);
			
			int totalHours = 0;
			// get hours from daily emp count
			foreach (DailyEmployeeCountDS.GADailyEmployeeCountRow row in ds.GADailyEmployeeCount) 
				totalHours += row.ManHours;
			//add  start values from owner 
			if (Owner.DataClass == GADataClass.GALocation || Owner.DataClass == GADataClass.GAProject) 
			{
				BusinessClass bc = Utils.RecordsetFactory.Make(Owner.DataClass);
				DataSet ods = bc.GetByRowId(Owner.RowId);
                // Tor 20070812 : changed from "IntFree1" to "LocationHoursCount"
                if (ods.Tables[Owner.DataClass.ToString()].Rows.Count > 0 && ods.Tables[Owner.DataClass.ToString()].Columns.Contains("LocationHoursCount")) 
				{
                    if (ods.Tables[Owner.DataClass.ToString()].Rows[0]["LocationHoursCount"] != DBNull.Value)
                        totalHours += (int)ods.Tables[Owner.DataClass.ToString()].Rows[0]["LocationHoursCount"];
				}

			}
				
			return totalHours;
		}

		/// <summary>
		/// Get total exposed hours for owner until specified date
		/// </summary>
		/// <param name="Owner">GADataRecord for which to get exposed hours. Must be a Location or a Project</param>
		/// <param name="ToDate">DateTime, To date for calculation</param>
		/// <returns>Integer, total numver of hours</returns>
		public int GetTotalExsposedHoursToDate(GADataRecord Owner, System.DateTime ToDate) 
		{
			return GetTotalExsposedHoursToDate(Owner, ToDate, null);
		}

	
		/// <summary>
		/// Get all DailyEmployeeCount data within a specified owner. Overrided from BussinessClass.GetAllRecordsWithinOwnerAndLinkedRecords()
		/// </summary>
		/// <param name="Owner">GADataRecord</param>
		/// <param name="StartDate">StartDate</param>
		/// <param name="EndDate">EndDate</param>
		/// <returns>DailyEmployeeCount dataset</returns>
		public override DataSet GetAllRecordsWithinOwnerAndLinkedRecords(GADataRecord Owner, DateTime StartDate, DateTime EndDate, string Filter)
		{
			
			EndDate = EndDate.Date + new TimeSpan(0, 23,59,59,0);    //set enddate to end of day

			//create a dailyemployee count record
			DailyEmployeeCountDS dds = (DailyEmployeeCountDS)this.GetNewRecord(Owner);

			//set values to 0
			dds.GADailyEmployeeCount[0].MarinePersonnelCount = 0;
			dds.GADailyEmployeeCount[0].ClientPersonnelCount = 0;
			dds.GADailyEmployeeCount[0].SeismicPersonnelCount = 0;
			dds.GADailyEmployeeCount[0].ThirdPartyPersonnelCount = 0;
			dds.GADailyEmployeeCount[0].ManHours = 0;
			
			GenerateDailyEmp(dds,StartDate,EndDate , Owner, Filter);

			return dds;
		}




	    /// <summary>
	    /// generate time and attendance for a single date
	    /// </summary>
	    /// <param name="ds"></param>
	    /// <param name="StartDate"></param>
	    /// <param name="EndDate"></param>
	    /// <param name="Owner"></param>
	    /// <param name="Filter"></param>
		private void GenerateDailyEmp(DailyEmployeeCountDS ds, DateTime StartDate, DateTime EndDate, GADataRecord Owner, string Filter) 
		{
			try 
			{
                // Tor 20071010 : added code to enable reading exposed hours from GATimeAndAttendance or from GAExposedHoursGroupView
                //               switch should be set in web.config
                int marineCount, ClientCount, SeismicCount, thirdCount, marineId, clientId, seismicId, thirdId, depId, nWorkDays ;
                marineId = Lists.GetListsRowIdByCategoryAndKey("DP", "Marine");
				clientId = Lists.GetListsRowIdByCategoryAndKey("DP", "Client");
				seismicId = Lists.GetListsRowIdByCategoryAndKey("DP", "Seismic");
				thirdId = Lists.GetListsRowIdByCategoryAndKey("DP", "Third party");

			 //   int exposedHoursSource = 1; // Tor 20071010 : Should be replaced by reading from web.config TODO
				if (exposedHoursSource == ExposedHoursSource.GATimeAndAttendance)
                {
				    //get employments

                   
    		
				    BusinessClass bc = Utils.RecordsetFactory.Make(GADataClass.GATimeAndAttendance);
				    TimeAndAttendanceDS tds = (TimeAndAttendanceDS)bc.GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(Owner, StartDate, EndDate, Filter);
                    
				    marineCount = 0; ClientCount = 0; SeismicCount = 0; thirdCount = 0;
    			
				    try 
				    {
                        foreach (TimeAndAttendanceDS.GATimeAndAttendanceRow row in tds.GATimeAndAttendance.Rows)
                        {
                            if (!row.IsEmploymentRowIdNull())    //ignore row if employmentid is missing, invalid row
						    {
    							
							    //get number of days
							    nWorkDays = GetNumberOfDaysInPeriode(StartDate, EndDate, 
												    row.IsDateTimeFromNull() ? DateTime.MinValue : row.DateTimeFrom, 
												    row.IsDateTimeToNull() ? DateTime.MaxValue : row.DateTimeTo);
    							
    							
    							
							    //get employment for taa
							    //EmploymentDS eds = Employment.GetEmploymentByEmploymentRowId(row.EmploymentRowId);
                        
                                               	
                                EmploymentDS eds = Employment.GetEmploymentByPersonnelIdOwnerAndStartDate(row.EmploymentRowId, Owner, row.DateTimeFrom);
							    //if no employments found. Check againt all of the persons employments
							    if (eds.GAEmployment.Rows.Count == 0)
								    eds = Employment.GetEmploymentsByPersonnelIdAndDate(row.EmploymentRowId, row.DateTimeFrom);

                                if (eds.GAEmployment.Rows.Count > 0 && !eds.GAEmployment[0].IsDepartmentListsRowIdNull())
							    {
								    depId = eds.GAEmployment[0].DepartmentListsRowId;
								    if (depId == marineId)
									    marineCount += nWorkDays;
								    else if (depId == clientId)
									    ClientCount += nWorkDays;
								    else if (depId == seismicId)
									    SeismicCount += nWorkDays;
								    else if (depId == thirdId)
									    thirdCount += nWorkDays;
								    else 
									    thirdCount += nWorkDays;
                                } else
                                {
                            	    //if personnel does not have a employment on the vessel, count as thirdcount
                            	    thirdCount += nWorkDays;
                                }       
                            }
					    }
				    } 
				    catch (Exception ex) 
				    {
    //					lblGenerateError.Text = "Error getting Time and Attendance. " + ex.Message;  //TODO use gaerror
    //					lblGenerateError.Visible = true;
                        throw new GAExceptions.GAException("Error getting Time and attendace date: " + ex.Message);
				    }

				    ds.GADailyEmployeeCount[0].MarinePersonnelCount += marineCount;
				    ds.GADailyEmployeeCount[0].ClientPersonnelCount += ClientCount;
				    ds.GADailyEmployeeCount[0].SeismicPersonnelCount += SeismicCount;
				    ds.GADailyEmployeeCount[0].ThirdPartyPersonnelCount += thirdCount;

				    ds.GADailyEmployeeCount[0].ManHours += (marineCount + ClientCount + SeismicCount + thirdCount) * 24;
                }
                else
                {
				// Tor 20071010 : added code
                //               get exposed hours from ExposedHoursGroupViewDS
		
				    BusinessClass bc = Utils.RecordsetFactory.Make(GADataClass.GAExposedHoursGroupView);
                    ExposedHoursGroupViewDS expds = (ExposedHoursGroupViewDS)bc.GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(Owner, StartDate, EndDate, Filter);

				    marineCount = 0; ClientCount = 0; SeismicCount = 0; thirdCount = 0;
    			
				    try 
				    {
					    foreach (ExposedHoursGroupViewDS.GAExposedHoursGroupViewRow row in expds.GAExposedHoursGroupView.Rows) 
					    {
                        //get number of days
					    nWorkDays = GetNumberOfDaysInPeriode(StartDate, EndDate, 
												    row.IsDateTimeFromNull() ? DateTime.MinValue : row.DateTimeFrom, 
												    row.IsDateTimeToNull() ? DateTime.MaxValue : row.DateTimeTo);

                        // Tor 20080411 : attribute IntFree1 renamed to DepartmentCategoryListsRowId - depId = row.IntFree1;
                        depId = row.DepartmentCategoryListsRowId;
                        // Tor 20080411 : attribute IntFree3 renamed to NumberOfPersons - int numberOfPersonnel = row.IntFree3;
                        int numberOfPersonnel = row.NumberOfPersons;
						if (depId == marineId)
						    marineCount += nWorkDays * numberOfPersonnel ;
						else if (depId == clientId)
						    ClientCount += nWorkDays * numberOfPersonnel ;
						else if (depId == seismicId)
						    SeismicCount += nWorkDays * numberOfPersonnel ;
						else if (depId == thirdId)
						    thirdCount += nWorkDays * numberOfPersonnel ;
						else 
						    thirdCount += nWorkDays * numberOfPersonnel ;
					    }
				    } 
				    catch (Exception ex) 
				    {
                        throw new GAExceptions.GAException("Error getting Exposed Hours data: " + ex.Message);
				    }

				    ds.GADailyEmployeeCount[0].MarinePersonnelCount += marineCount;
				    ds.GADailyEmployeeCount[0].ClientPersonnelCount += ClientCount;
				    ds.GADailyEmployeeCount[0].SeismicPersonnelCount += SeismicCount;
				    ds.GADailyEmployeeCount[0].ThirdPartyPersonnelCount += thirdCount;

				    ds.GADailyEmployeeCount[0].ManHours += (marineCount + ClientCount + SeismicCount + thirdCount) * 24;

                }

			} 
			catch (Exception ex) 
			{
//				lblGenerateError.Text = "Error generating values " + ex.Message;  //TODO use gaerror
//				lblGenerateError.Visible = true;
                // Tor 20071010 : Replace TimeAndAttendanceDS with ExposedHoursGroupViewDS 
                // throw new GAExceptions.GAException("Error getting Time and attendace date: " + ex.Message);
                throw new GAExceptions.GAException("Error getting Exposed Hours data: " + ex.Message);
            }
        }

		private int GetNumberOfDaysInPeriode(DateTime PeriodeStart, DateTime PeriodeEnd, DateTime PersonTimeStart, DateTime PersonTimeEnd) 
		{
			//check if person end time is during the first day, return 0 days

			// Tor 20071010 : Include the date when the person leaves the owner
            // if (PersonTimeEnd < new DateTime(PeriodeStart.Year, PeriodeStart.Month,PeriodeStart.Day, 23, 59, 59))
			if (PersonTimeEnd < PeriodeStart  || PersonTimeStart > PeriodeEnd)
				return 0;

            // Tor 20071014 : Remove time from from object from and to date - computes whole days only when computing number of days
            PersonTimeStart = new DateTime(PersonTimeStart.Year, PersonTimeStart.Month, PersonTimeStart.Day, 0, 0, 0);
            PersonTimeEnd   = new DateTime(PersonTimeEnd.Year,   PersonTimeEnd.Month,   PersonTimeEnd.Day,   0, 0, 0);

            PeriodeStart = new DateTime(PeriodeStart.Year, PeriodeStart.Month, PeriodeStart.Day, 0, 0, 0);
            PeriodeEnd = new DateTime(PeriodeEnd.Year, PeriodeEnd.Month, PeriodeEnd.Day, 0, 0, 0);



			
            DateTime startDate = PeriodeStart < PersonTimeStart ? PersonTimeStart : PeriodeStart;

			//adjust startDate to start of day
            // 14/10-07 Tor: removed time adjustment - see 14/10 above
            // startDate = new DateTime(startDate.Year, startDate.Month, startDate.Day, 0, 0, 0);
			
			DateTime endDate = PeriodeEnd > PersonTimeEnd ? PersonTimeEnd : PeriodeEnd;

            // Tor 20071010 : Include startdate and enddate in number of days
			// int numberOfDays = ((TimeSpan)endDate.Subtract(startDate)).Days;
			int numberOfDays = ((TimeSpan)endDate.Subtract(startDate)).Days + 1;
			
			//if persons end date is  after the periode end, add one day in order to count the last day of the periode
            // Tor 20071014 : removed adding one day because both start and end date is allways included in number of days. See computation above
            // if (PersonTimeEnd > PeriodeEnd)
			//	numberOfDays++;

			return numberOfDays;
		}


        /// <summary>
        /// Get ExposedHoursSource to use in calculating. Get this value for the setting ExposedHoursSource in web.config. 
        /// Uses ExposedHoursSource.GAExposedHoursGroupView as default if value is missing.
        /// </summary>
        private static ExposedHoursSource exposedHoursSource
        {
            get
            {
// Tor 20160127                string exposedSource = System.Configuration.ConfigurationManager.AppSettings.Get("ExposedHoursSource");
                string exposedSource = new GASystem.AppUtils.FlagSysResource().GetResourceString("ExposedHoursSource");

                if (exposedSource == null)
                    return ExposedHoursSource.GAExposedHoursGroupView;  //return GAExposedHoursGroupView as default source type;

                try 
                {
                    return (ExposedHoursSource)Enum.Parse(typeof(ExposedHoursSource), exposedSource);
                } catch (System.ArgumentException ex)
                {
                    throw new GAExceptions.GAException("Invalid value for ExposedHoursSource in web.config. Valid values are  GATimeAndAttendance, GAExposedHoursGroupView");
                }
            }
        }

    }
}

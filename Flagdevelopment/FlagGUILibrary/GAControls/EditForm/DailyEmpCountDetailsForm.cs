using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using GASystem.GAControls.UtilityControls;

namespace GASystem.GAControls.EditForm
{
	/// <summary>
	/// Summary description for GeneralDetailsForm.
	/// </summary>
	public class DailyEmpCountDetailsForm : GeneralDetailsForm
	{
		
		protected System.Web.UI.WebControls.Button btnGenerateValues;
		protected Label lblGenerateError;
		//private GASystem.GAControls.UtilityControls.ProcedureSelector MyProcedureSelector;

		public DailyEmpCountDetailsForm(GADataRecord DataRecord) : base(DataRecord)
		{
			//
			// TODO: Add constructor logic here
			//
			btnGenerateValues = new Button();
			btnGenerateValues.Visible = DataRecord.RowId == 0;    //oshow generate only for new records
			lblGenerateError = new Label();
			lblGenerateError.EnableViewState = false;

			
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			btnGenerateValues.Text = GASystem.AppUtils.Localization.GetCaptionText("GenereateDailyEmp");
			btnGenerateValues.CausesValidation = false;
			btnGenerateValues.Click += new EventHandler(btnGenerateValues_Click);
			this.PlaceHolderTop.Controls.Add(btnGenerateValues);
			lblGenerateError.Visible = false;
			lblGenerateError.ForeColor = System.Drawing.Color.Red;
			this.PlaceHolderTop.Controls.Add(lblGenerateError);

		}

		private void btnGenerateValues_Click(object sender, EventArgs e)
		{
			GenerateDailyEmp();
		}

		private void GenerateDailyEmp() 
		{
			try 
			{
				this.myEditDataRecord.UpdateRecordSet();
				DailyEmployeeCountDS ds = (DailyEmployeeCountDS)this.myEditDataRecord.RecordDataSet;
			
				//test for date to use

				if (ds.GADailyEmployeeCount[0].IsReportDateNull()) 
				{
					lblGenerateError.Text = "No report date is selected. Please select a date";
					lblGenerateError.Visible = true;
				}

				//get employments
				TimeAndAttendanceDS tds = TimeAndAttendance.GetTimeAndAttendancesByOwnerAndDate(this.OwnerRecord.RowId,  this.OwnerRecord.DataClass, ds.GADailyEmployeeCount[0].ReportDate);
			
				int marineCount = 0, ClientCount = 0, SeismicCount = 0, thirdCount = 0;
			
				try 
				{
					int marineId = Lists.GetListsRowIdByCategoryAndKey("DP", "Marine");
					int clientId = Lists.GetListsRowIdByCategoryAndKey("DP", "Client");
					int seismicId = Lists.GetListsRowIdByCategoryAndKey("DP", "Seismic");
					int thirdId = Lists.GetListsRowIdByCategoryAndKey("DP", "Third party");

			
					foreach(TimeAndAttendanceDS.GATimeAndAttendanceRow row in tds.GATimeAndAttendance.Rows) 
					{
						if (!row.IsEmploymentRowIdNull())    //ignore row if employmentid is missing, invalid row
						{
							//get employment for taa
							EmploymentDS eds = Employment.GetEmploymentByEmploymentRowId(row.EmploymentRowId);
							if (eds.GAEmployment.Rows.Count > 0 && !eds.GAEmployment[0].IsDepartmentListsRowIdNull())
							{
								int depId = eds.GAEmployment[0].DepartmentListsRowId;
								if (depId == marineId)
									marineCount++;
								if (depId == clientId)
									ClientCount++;
								if (depId == seismicId)
									SeismicCount++;
								if (depId == thirdId)
									thirdCount++;
							}
						}
					}
				} 
				catch (Exception ex) 
				{
					lblGenerateError.Text = "Error getting Time and Attendance. " + ex.Message;  //TODO use gaerror
					lblGenerateError.Visible = true;
				}

				ds.GADailyEmployeeCount[0].MarinePersonnelCount = marineCount;
				ds.GADailyEmployeeCount[0].ClientPersonnelCount = ClientCount;
				ds.GADailyEmployeeCount[0].SeismicPersonnelCount = SeismicCount;
				ds.GADailyEmployeeCount[0].ThirdPartyPersonnelCount = thirdCount;

				ds.GADailyEmployeeCount[0].ManHours = (marineCount + ClientCount + SeismicCount + thirdCount) * 24;

				this.myEditDataRecord.RecordDataSet = ds;
				this.myEditDataRecord.SetupForm();
			} 
			catch (Exception ex) 
			{
				lblGenerateError.Text = "Error generating values " + ex.Message;  //TODO use gaerror
				lblGenerateError.Visible = true;
			}
		}
	}
}

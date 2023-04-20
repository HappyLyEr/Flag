using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

namespace ActiveReportBuilder
{
	/// <summary>
	/// Summary description for GAIncidentReport.
	/// </summary>
	public class GAIncidentReport : DataDynamics.ActiveReports.ActiveReport3
	{
		private DataDynamics.ActiveReports.PageHeader pageHeader;
		private DataDynamics.ActiveReports.Detail detail;
		private DataDynamics.ActiveReports.PageFooter pageFooter;
		private GASystem.DataModel.View.ReportView reportView1;
		private DataDynamics.ActiveReports.Parameter owner;
		private DataDynamics.ActiveReports.Parameter startdate;
		private DataDynamics.ActiveReports.Parameter enddate;
		private DataDynamics.ActiveReports.TextBox txtTitle1;
		private DataDynamics.ActiveReports.TextBox txtDateAndTimeOfIncident1;
		private DataDynamics.ActiveReports.TextBox txtShortDescription1;
		private DataDynamics.ActiveReports.TextBox txtWitness1;
		private DataDynamics.ActiveReports.TextBox txtHasMaritimeReport1;
		private DataDynamics.ActiveReports.TextBox txtWitnessPersonnelRowId1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GAIncidentReport()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Report Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GAIncidentReport));
			this.pageHeader = new DataDynamics.ActiveReports.PageHeader();
			this.txtTitle1 = new DataDynamics.ActiveReports.TextBox();
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.txtDateAndTimeOfIncident1 = new DataDynamics.ActiveReports.TextBox();
			this.txtShortDescription1 = new DataDynamics.ActiveReports.TextBox();
			this.txtWitness1 = new DataDynamics.ActiveReports.TextBox();
			this.txtHasMaritimeReport1 = new DataDynamics.ActiveReports.TextBox();
			this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
			this.reportView1 = new GASystem.DataModel.View.ReportView();
			this.owner = new DataDynamics.ActiveReports.Parameter();
			this.startdate = new DataDynamics.ActiveReports.Parameter();
			this.enddate = new DataDynamics.ActiveReports.Parameter();
			this.txtWitnessPersonnelRowId1 = new DataDynamics.ActiveReports.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.txtTitle1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDateAndTimeOfIncident1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtShortDescription1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtWitness1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtHasMaritimeReport1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtWitnessPersonnelRowId1)).BeginInit();
			// 
			// pageHeader
			// 
			this.pageHeader.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																							 this.txtTitle1});
			this.pageHeader.Height = 0.25F;
			this.pageHeader.Name = "pageHeader";
			// 
			// txtTitle1
			// 
			this.txtTitle1.DataField = "Title";
			this.txtTitle1.DistinctField = null;
			this.txtTitle1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtTitle1.Location = ((System.Drawing.PointF)(resources.GetObject("txtTitle1.Location")));
			this.txtTitle1.Name = "txtTitle1";
			this.txtTitle1.OutputFormat = null;
			this.txtTitle1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtTitle1.Size")));
			this.txtTitle1.Text = "txtTitle1";
			// 
			// detail
			// 
			this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																						 this.txtDateAndTimeOfIncident1,
																						 this.txtShortDescription1,
																						 this.txtWitness1,
																						 this.txtHasMaritimeReport1,
																						 this.txtWitnessPersonnelRowId1});
			this.detail.Height = 2F;
			this.detail.Name = "detail";
			// 
			// txtDateAndTimeOfIncident1
			// 
			this.txtDateAndTimeOfIncident1.DataField = "DateAndTimeOfIncident";
			this.txtDateAndTimeOfIncident1.DistinctField = null;
			this.txtDateAndTimeOfIncident1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtDateAndTimeOfIncident1.Location = ((System.Drawing.PointF)(resources.GetObject("txtDateAndTimeOfIncident1.Location")));
			this.txtDateAndTimeOfIncident1.Name = "txtDateAndTimeOfIncident1";
			this.txtDateAndTimeOfIncident1.OutputFormat = null;
			this.txtDateAndTimeOfIncident1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtDateAndTimeOfIncident1.Size")));
			this.txtDateAndTimeOfIncident1.Text = "txtDateAndTimeOfIncident1";
			// 
			// txtShortDescription1
			// 
			this.txtShortDescription1.DataField = "ShortDescription";
			this.txtShortDescription1.DistinctField = null;
			this.txtShortDescription1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtShortDescription1.Location = ((System.Drawing.PointF)(resources.GetObject("txtShortDescription1.Location")));
			this.txtShortDescription1.Name = "txtShortDescription1";
			this.txtShortDescription1.OutputFormat = null;
			this.txtShortDescription1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtShortDescription1.Size")));
			this.txtShortDescription1.Text = "txtShortDescription1";
			// 
			// txtWitness1
			// 
			this.txtWitness1.DataField = "Witness";
			this.txtWitness1.DistinctField = null;
			this.txtWitness1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtWitness1.Location = ((System.Drawing.PointF)(resources.GetObject("txtWitness1.Location")));
			this.txtWitness1.Name = "txtWitness1";
			this.txtWitness1.OutputFormat = null;
			this.txtWitness1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtWitness1.Size")));
			this.txtWitness1.Text = "txtWitness1";
			// 
			// txtHasMaritimeReport1
			// 
			this.txtHasMaritimeReport1.DataField = "HasMaritimeReport";
			this.txtHasMaritimeReport1.DistinctField = null;
			this.txtHasMaritimeReport1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtHasMaritimeReport1.Location = ((System.Drawing.PointF)(resources.GetObject("txtHasMaritimeReport1.Location")));
			this.txtHasMaritimeReport1.Name = "txtHasMaritimeReport1";
			this.txtHasMaritimeReport1.OutputFormat = null;
			this.txtHasMaritimeReport1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtHasMaritimeReport1.Size")));
			this.txtHasMaritimeReport1.Text = "txtHasMaritimeReport1";
			// 
			// pageFooter
			// 
			this.pageFooter.Height = 0.25F;
			this.pageFooter.Name = "pageFooter";
			// 
			// reportView1
			// 
			this.reportView1.DataSetName = "ReportView";
			this.reportView1.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// owner
			// 
			this.owner.DefaultValue = "";
			this.owner.Key = "owner";
			this.owner.Prompt = null;
			this.owner.PromptUser = true;
			this.owner.QueryCreated = false;
			this.owner.Tag = null;
			this.owner.Type = DataDynamics.ActiveReports.Parameter.DataType.String;
			// 
			// startdate
			// 
			this.startdate.DefaultValue = "";
			this.startdate.Key = "startdate";
			this.startdate.Prompt = null;
			this.startdate.PromptUser = true;
			this.startdate.QueryCreated = false;
			this.startdate.Tag = null;
			this.startdate.Type = DataDynamics.ActiveReports.Parameter.DataType.String;
			// 
			// enddate
			// 
			this.enddate.DefaultValue = "";
			this.enddate.Key = "enddate";
			this.enddate.Prompt = null;
			this.enddate.PromptUser = true;
			this.enddate.QueryCreated = false;
			this.enddate.Tag = null;
			this.enddate.Type = DataDynamics.ActiveReports.Parameter.DataType.String;
			// 
			// txtWitnessPersonnelRowId1
			// 
			this.txtWitnessPersonnelRowId1.DataField = "WitnessPersonnelRowId";
			this.txtWitnessPersonnelRowId1.DistinctField = null;
			this.txtWitnessPersonnelRowId1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtWitnessPersonnelRowId1.Location = ((System.Drawing.PointF)(resources.GetObject("txtWitnessPersonnelRowId1.Location")));
			this.txtWitnessPersonnelRowId1.Name = "txtWitnessPersonnelRowId1";
			this.txtWitnessPersonnelRowId1.OutputFormat = null;
			this.txtWitnessPersonnelRowId1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtWitnessPersonnelRowId1.Size")));
			this.txtWitnessPersonnelRowId1.Text = "txtWitnessPersonnelRowId1";
			// 
			// GAIncidentReport
			// 
			this.DataMember = "GAIncidentReport";
			this.DataSource = this.reportView1;
			this.PageSettings.PaperHeight = 11F;
			this.PageSettings.PaperWidth = 8.5F;
			this.Parameters.AddRange(new DataDynamics.ActiveReports.Parameter[] {
																					this.owner,
																					this.startdate,
																					this.enddate});
			this.Sections.Add(this.pageHeader);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.pageFooter);
			((System.ComponentModel.ISupportInitialize)(this.txtTitle1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDateAndTimeOfIncident1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtShortDescription1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtWitness1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtHasMaritimeReport1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtWitnessPersonnelRowId1)).EndInit();

		}
		#endregion
	}
}
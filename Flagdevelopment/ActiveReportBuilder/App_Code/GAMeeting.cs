using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

namespace ActiveReportGeneration
{
	/// <summary>
	/// Summary description for GAMeeting.
	/// </summary>
	public class GAMeeting : DataDynamics.ActiveReports.ActiveReport3
	{
		private DataDynamics.ActiveReports.PageHeader pageHeader;
		private DataDynamics.ActiveReports.Detail detail;
		private DataDynamics.ActiveReports.PageFooter pageFooter;
		private DataDynamics.ActiveReports.SubReport subReport1;
		private DataDynamics.ActiveReports.TextBox txtName2;
		private DataDynamics.ActiveReports.TextBox txtMeetingReferenceId1;
		private DataDynamics.ActiveReports.Label label1;
		private DataDynamics.ActiveReports.Label label2;
		private DataDynamics.ActiveReports.Label label3;
		private DataDynamics.ActiveReports.Label label4;
		private DataDynamics.ActiveReports.Label label5;
		private DataDynamics.ActiveReports.Label label6;
		private DataDynamics.ActiveReports.Label label7;
		private DataDynamics.ActiveReports.Label label8;
		private DataDynamics.ActiveReports.Label label9;
		private DataDynamics.ActiveReports.Parameter OwnerName;
		private DataDynamics.ActiveReports.TextBox txtOwnerName1;
		private DataDynamics.ActiveReports.TextBox txtDateOfMeeting1;
		private DataDynamics.ActiveReports.Label label10;
		private DataDynamics.ActiveReports.TextBox txtTimeEnd1;
		private DataDynamics.ActiveReports.TextBox txtConductor1;
		private DataDynamics.ActiveReports.TextBox txtReporterPersonnelRowId1;
		private DataDynamics.ActiveReports.TextBox txtInvolvedPersonnelCount1;
		private DataDynamics.ActiveReports.TextBox txtDateAndTimeNextMeeting1;
		private DataDynamics.ActiveReports.TextBox txtPurpose1;
		private DataDynamics.ActiveReports.TextBox txtAgenda1;
		private DataDynamics.ActiveReports.TextBox txtComment;
		private DataDynamics.ActiveReports.SubReport subReport2;
		private DataDynamics.ActiveReports.SubReport subReport3;
		private GASystem.DataModel.View.ReportView reportView1;
		private DataDynamics.ActiveReports.Label label11;
		private DataDynamics.ActiveReports.Label label12;
		private DataDynamics.ActiveReports.Field Today;
		private DataDynamics.ActiveReports.Picture picture1;
		private DataDynamics.ActiveReports.TextBox txtparamToday1;
		private DataDynamics.ActiveReports.ReportInfo reportInfo1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GAMeeting()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GAMeeting));
			this.pageHeader = new DataDynamics.ActiveReports.PageHeader();
			this.txtName2 = new DataDynamics.ActiveReports.TextBox();
			this.txtMeetingReferenceId1 = new DataDynamics.ActiveReports.TextBox();
			this.label11 = new DataDynamics.ActiveReports.Label();
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.subReport1 = new DataDynamics.ActiveReports.SubReport();
			this.label1 = new DataDynamics.ActiveReports.Label();
			this.label2 = new DataDynamics.ActiveReports.Label();
			this.label3 = new DataDynamics.ActiveReports.Label();
			this.label4 = new DataDynamics.ActiveReports.Label();
			this.label5 = new DataDynamics.ActiveReports.Label();
			this.label6 = new DataDynamics.ActiveReports.Label();
			this.label7 = new DataDynamics.ActiveReports.Label();
			this.label8 = new DataDynamics.ActiveReports.Label();
			this.label9 = new DataDynamics.ActiveReports.Label();
			this.txtOwnerName1 = new DataDynamics.ActiveReports.TextBox();
			this.txtDateOfMeeting1 = new DataDynamics.ActiveReports.TextBox();
			this.label10 = new DataDynamics.ActiveReports.Label();
			this.txtTimeEnd1 = new DataDynamics.ActiveReports.TextBox();
			this.txtConductor1 = new DataDynamics.ActiveReports.TextBox();
			this.txtReporterPersonnelRowId1 = new DataDynamics.ActiveReports.TextBox();
			this.txtInvolvedPersonnelCount1 = new DataDynamics.ActiveReports.TextBox();
			this.txtDateAndTimeNextMeeting1 = new DataDynamics.ActiveReports.TextBox();
			this.txtPurpose1 = new DataDynamics.ActiveReports.TextBox();
			this.txtAgenda1 = new DataDynamics.ActiveReports.TextBox();
			this.txtComment = new DataDynamics.ActiveReports.TextBox();
			this.subReport2 = new DataDynamics.ActiveReports.SubReport();
			this.subReport3 = new DataDynamics.ActiveReports.SubReport();
			this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
			this.label12 = new DataDynamics.ActiveReports.Label();
			this.picture1 = new DataDynamics.ActiveReports.Picture();
			this.txtparamToday1 = new DataDynamics.ActiveReports.TextBox();
			this.reportInfo1 = new DataDynamics.ActiveReports.ReportInfo();
			this.OwnerName = new DataDynamics.ActiveReports.Parameter();
			this.reportView1 = new GASystem.DataModel.View.ReportView();
			this.Today = new DataDynamics.ActiveReports.Field();
			((System.ComponentModel.ISupportInitialize)(this.txtName2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtMeetingReferenceId1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label11)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label9)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtOwnerName1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDateOfMeeting1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label10)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtTimeEnd1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtConductor1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtReporterPersonnelRowId1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtInvolvedPersonnelCount1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDateAndTimeNextMeeting1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtPurpose1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtAgenda1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtComment)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label12)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtparamToday1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).BeginInit();
			// 
			// pageHeader
			// 
			this.pageHeader.BackColor = System.Drawing.Color.Silver;
			this.pageHeader.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																							 this.txtName2,
																							 this.txtMeetingReferenceId1,
																							 this.label11});
			this.pageHeader.Height = 0.6875F;
			this.pageHeader.Name = "pageHeader";
			// 
			// txtName2
			// 
			this.txtName2.DataField = "Name";
			this.txtName2.DistinctField = null;
			this.txtName2.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtName2.Location = ((System.Drawing.PointF)(resources.GetObject("txtName2.Location")));
			this.txtName2.Name = "txtName2";
			this.txtName2.OutputFormat = null;
			this.txtName2.Size = ((System.Drawing.SizeF)(resources.GetObject("txtName2.Size")));
			this.txtName2.Text = "txtName2";
			// 
			// txtMeetingReferenceId1
			// 
			this.txtMeetingReferenceId1.DataField = "MeetingReferenceId";
			this.txtMeetingReferenceId1.DistinctField = null;
			this.txtMeetingReferenceId1.Font = new System.Drawing.Font("Times New Roman", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtMeetingReferenceId1.Location = ((System.Drawing.PointF)(resources.GetObject("txtMeetingReferenceId1.Location")));
			this.txtMeetingReferenceId1.Name = "txtMeetingReferenceId1";
			this.txtMeetingReferenceId1.OutputFormat = null;
			this.txtMeetingReferenceId1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtMeetingReferenceId1.Size")));
			this.txtMeetingReferenceId1.Text = "txtMeetingReferenceId1";
			// 
			// label11
			// 
			this.label11.DataField = "TypeOfMeetingListsRowId";
			this.label11.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label11.HyperLink = null;
			this.label11.Location = ((System.Drawing.PointF)(resources.GetObject("label11.Location")));
			this.label11.Name = "label11";
			this.label11.Size = ((System.Drawing.SizeF)(resources.GetObject("label11.Size")));
			this.label11.Text = "label11";
			// 
			// detail
			// 
			this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																						 this.subReport1,
																						 this.label1,
																						 this.label2,
																						 this.label3,
																						 this.label4,
																						 this.label5,
																						 this.label6,
																						 this.label7,
																						 this.label8,
																						 this.label9,
																						 this.txtOwnerName1,
																						 this.txtDateOfMeeting1,
																						 this.label10,
																						 this.txtTimeEnd1,
																						 this.txtConductor1,
																						 this.txtReporterPersonnelRowId1,
																						 this.txtInvolvedPersonnelCount1,
																						 this.txtDateAndTimeNextMeeting1,
																						 this.txtPurpose1,
																						 this.txtAgenda1,
																						 this.txtComment,
																						 this.subReport2,
																						 this.subReport3});
			this.detail.Height = 5.708333F;
			this.detail.Name = "detail";
			this.detail.Format += new System.EventHandler(this.detail_Format);
			// 
			// subReport1
			// 
			this.subReport1.CloseBorder = false;
			this.subReport1.Location = ((System.Drawing.PointF)(resources.GetObject("subReport1.Location")));
			this.subReport1.Name = "subReport1";
			this.subReport1.Report = null;
			this.subReport1.ReportName = "GAMeetingText";
			this.subReport1.Size = ((System.Drawing.SizeF)(resources.GetObject("subReport1.Size")));
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label1.HyperLink = null;
			this.label1.Location = ((System.Drawing.PointF)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.Size = ((System.Drawing.SizeF)(resources.GetObject("label1.Size")));
			this.label1.Text = "Meeting belongs to:";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label2.HyperLink = null;
			this.label2.Location = ((System.Drawing.PointF)(resources.GetObject("label2.Location")));
			this.label2.Name = "label2";
			this.label2.Size = ((System.Drawing.SizeF)(resources.GetObject("label2.Size")));
			this.label2.Text = "Date and time";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label3.HyperLink = null;
			this.label3.Location = ((System.Drawing.PointF)(resources.GetObject("label3.Location")));
			this.label3.Name = "label3";
			this.label3.Size = ((System.Drawing.SizeF)(resources.GetObject("label3.Size")));
			this.label3.Text = "Chairman";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label4.HyperLink = null;
			this.label4.Location = ((System.Drawing.PointF)(resources.GetObject("label4.Location")));
			this.label4.Name = "label4";
			this.label4.Size = ((System.Drawing.SizeF)(resources.GetObject("label4.Size")));
			this.label4.Text = "Reporter:";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label5.HyperLink = null;
			this.label5.Location = ((System.Drawing.PointF)(resources.GetObject("label5.Location")));
			this.label5.Name = "label5";
			this.label5.Size = ((System.Drawing.SizeF)(resources.GetObject("label5.Size")));
			this.label5.Text = "# of Involved Personnel";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label6.HyperLink = null;
			this.label6.Location = ((System.Drawing.PointF)(resources.GetObject("label6.Location")));
			this.label6.Name = "label6";
			this.label6.Size = ((System.Drawing.SizeF)(resources.GetObject("label6.Size")));
			this.label6.Text = "Next Meeting:";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label7.HyperLink = null;
			this.label7.Location = ((System.Drawing.PointF)(resources.GetObject("label7.Location")));
			this.label7.Name = "label7";
			this.label7.Size = ((System.Drawing.SizeF)(resources.GetObject("label7.Size")));
			this.label7.Text = "Purpose";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label8.HyperLink = null;
			this.label8.Location = ((System.Drawing.PointF)(resources.GetObject("label8.Location")));
			this.label8.Name = "label8";
			this.label8.Size = ((System.Drawing.SizeF)(resources.GetObject("label8.Size")));
			this.label8.Text = "Agenda";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label9.HyperLink = null;
			this.label9.Location = ((System.Drawing.PointF)(resources.GetObject("label9.Location")));
			this.label9.Name = "label9";
			this.label9.Size = ((System.Drawing.SizeF)(resources.GetObject("label9.Size")));
			this.label9.Text = "Comments";
			// 
			// txtOwnerName1
			// 
			this.txtOwnerName1.DataField = "param:OwnerName";
			this.txtOwnerName1.DistinctField = null;
			this.txtOwnerName1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtOwnerName1.Location = ((System.Drawing.PointF)(resources.GetObject("txtOwnerName1.Location")));
			this.txtOwnerName1.Name = "txtOwnerName1";
			this.txtOwnerName1.OutputFormat = null;
			this.txtOwnerName1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtOwnerName1.Size")));
			this.txtOwnerName1.Text = "txtOwnerName1";
			// 
			// txtDateOfMeeting1
			// 
			this.txtDateOfMeeting1.DataField = "DateOfMeeting";
			this.txtDateOfMeeting1.DistinctField = null;
			this.txtDateOfMeeting1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtDateOfMeeting1.Location = ((System.Drawing.PointF)(resources.GetObject("txtDateOfMeeting1.Location")));
			this.txtDateOfMeeting1.Name = "txtDateOfMeeting1";
			this.txtDateOfMeeting1.OutputFormat = "d-MMM-yyyy hh:mm";
			this.txtDateOfMeeting1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtDateOfMeeting1.Size")));
			this.txtDateOfMeeting1.Text = "txtDateOfMeeting1";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label10.HyperLink = null;
			this.label10.Location = ((System.Drawing.PointF)(resources.GetObject("label10.Location")));
			this.label10.Name = "label10";
			this.label10.Size = ((System.Drawing.SizeF)(resources.GetObject("label10.Size")));
			this.label10.Text = "To:";
			// 
			// txtTimeEnd1
			// 
			this.txtTimeEnd1.DataField = "TimeEnd";
			this.txtTimeEnd1.DistinctField = null;
			this.txtTimeEnd1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtTimeEnd1.Location = ((System.Drawing.PointF)(resources.GetObject("txtTimeEnd1.Location")));
			this.txtTimeEnd1.Name = "txtTimeEnd1";
			this.txtTimeEnd1.OutputFormat = "d-MMM-yyyy hh:mm";
			this.txtTimeEnd1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtTimeEnd1.Size")));
			this.txtTimeEnd1.Text = "txtTimeEnd1";
			// 
			// txtConductor1
			// 
			this.txtConductor1.DataField = "Conductor";
			this.txtConductor1.DistinctField = null;
			this.txtConductor1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtConductor1.Location = ((System.Drawing.PointF)(resources.GetObject("txtConductor1.Location")));
			this.txtConductor1.Name = "txtConductor1";
			this.txtConductor1.OutputFormat = null;
			this.txtConductor1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtConductor1.Size")));
			this.txtConductor1.Text = "txtConductor1";
			// 
			// txtReporterPersonnelRowId1
			// 
			this.txtReporterPersonnelRowId1.DataField = "ReporterPersonnelRowId";
			this.txtReporterPersonnelRowId1.DistinctField = null;
			this.txtReporterPersonnelRowId1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtReporterPersonnelRowId1.Location = ((System.Drawing.PointF)(resources.GetObject("txtReporterPersonnelRowId1.Location")));
			this.txtReporterPersonnelRowId1.Name = "txtReporterPersonnelRowId1";
			this.txtReporterPersonnelRowId1.OutputFormat = null;
			this.txtReporterPersonnelRowId1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtReporterPersonnelRowId1.Size")));
			this.txtReporterPersonnelRowId1.Text = "txtReporterPersonnelRowId1";
			// 
			// txtInvolvedPersonnelCount1
			// 
			this.txtInvolvedPersonnelCount1.DataField = "InvolvedPersonnelCount";
			this.txtInvolvedPersonnelCount1.DistinctField = null;
			this.txtInvolvedPersonnelCount1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtInvolvedPersonnelCount1.Location = ((System.Drawing.PointF)(resources.GetObject("txtInvolvedPersonnelCount1.Location")));
			this.txtInvolvedPersonnelCount1.Name = "txtInvolvedPersonnelCount1";
			this.txtInvolvedPersonnelCount1.OutputFormat = null;
			this.txtInvolvedPersonnelCount1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtInvolvedPersonnelCount1.Size")));
			this.txtInvolvedPersonnelCount1.Text = "txtInvolvedPersonnelCount1";
			// 
			// txtDateAndTimeNextMeeting1
			// 
			this.txtDateAndTimeNextMeeting1.DataField = "DateAndTimeNextMeeting";
			this.txtDateAndTimeNextMeeting1.DistinctField = null;
			this.txtDateAndTimeNextMeeting1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtDateAndTimeNextMeeting1.Location = ((System.Drawing.PointF)(resources.GetObject("txtDateAndTimeNextMeeting1.Location")));
			this.txtDateAndTimeNextMeeting1.Name = "txtDateAndTimeNextMeeting1";
			this.txtDateAndTimeNextMeeting1.OutputFormat = "d-MMM-yyyy hh:mm";
			this.txtDateAndTimeNextMeeting1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtDateAndTimeNextMeeting1.Size")));
			this.txtDateAndTimeNextMeeting1.Text = "txtDateAndTimeNextMeeting1";
			// 
			// txtPurpose1
			// 
			this.txtPurpose1.DataField = "Purpose";
			this.txtPurpose1.DistinctField = null;
			this.txtPurpose1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtPurpose1.Location = ((System.Drawing.PointF)(resources.GetObject("txtPurpose1.Location")));
			this.txtPurpose1.Name = "txtPurpose1";
			this.txtPurpose1.OutputFormat = null;
			this.txtPurpose1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtPurpose1.Size")));
			this.txtPurpose1.Text = "txtPurpose1";
			// 
			// txtAgenda1
			// 
			this.txtAgenda1.DataField = "Agenda";
			this.txtAgenda1.DistinctField = null;
			this.txtAgenda1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtAgenda1.Location = ((System.Drawing.PointF)(resources.GetObject("txtAgenda1.Location")));
			this.txtAgenda1.Name = "txtAgenda1";
			this.txtAgenda1.OutputFormat = null;
			this.txtAgenda1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtAgenda1.Size")));
			this.txtAgenda1.Text = "txtAgenda1";
			// 
			// txtComment
			// 
			this.txtComment.DataField = "Comment";
			this.txtComment.DistinctField = null;
			this.txtComment.Font = new System.Drawing.Font("Arial", 10F);
			this.txtComment.Location = ((System.Drawing.PointF)(resources.GetObject("txtComment.Location")));
			this.txtComment.Name = "txtComment";
			this.txtComment.OutputFormat = null;
			this.txtComment.Size = ((System.Drawing.SizeF)(resources.GetObject("txtComment.Size")));
			this.txtComment.Text = "txtComment";
			// 
			// subReport2
			// 
			this.subReport2.CloseBorder = false;
			this.subReport2.Location = ((System.Drawing.PointF)(resources.GetObject("subReport2.Location")));
			this.subReport2.Name = "subReport2";
			this.subReport2.Report = null;
			this.subReport2.ReportName = "GAMeetingPersonList";
			this.subReport2.Size = ((System.Drawing.SizeF)(resources.GetObject("subReport2.Size")));
			// 
			// subReport3
			// 
			this.subReport3.CloseBorder = false;
			this.subReport3.Location = ((System.Drawing.PointF)(resources.GetObject("subReport3.Location")));
			this.subReport3.Name = "subReport3";
			this.subReport3.Report = null;
			this.subReport3.ReportName = "GAAction";
			this.subReport3.Size = ((System.Drawing.SizeF)(resources.GetObject("subReport3.Size")));
			// 
			// pageFooter
			// 
			this.pageFooter.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																							 this.label12,
																							 this.picture1,
																							 this.txtparamToday1,
																							 this.reportInfo1});
			this.pageFooter.Height = 0.90625F;
			this.pageFooter.Name = "pageFooter";
			// 
			// label12
			// 
			this.label12.Font = new System.Drawing.Font("Arial", 8F);
			this.label12.ForeColor = System.Drawing.Color.Red;
			this.label12.HyperLink = null;
			this.label12.Location = ((System.Drawing.PointF)(resources.GetObject("label12.Location")));
			this.label12.Name = "label12";
			this.label12.Size = ((System.Drawing.SizeF)(resources.GetObject("label12.Size")));
			this.label12.Text = "This report contains data registered up to:                          Information " +
				"may have been changed after this time.";
			// 
			// picture1
			// 
			this.picture1.Image = ((System.Drawing.Image)(resources.GetObject("picture1.Image")));
			this.picture1.LineWeight = 0F;
			this.picture1.Location = ((System.Drawing.PointF)(resources.GetObject("picture1.Location")));
			this.picture1.Name = "picture1";
			this.picture1.Size = ((System.Drawing.SizeF)(resources.GetObject("picture1.Size")));
			this.picture1.SizeMode = DataDynamics.ActiveReports.SizeModes.Stretch;
			// 
			// txtparamToday1
			// 
			this.txtparamToday1.DataField = "Today";
			this.txtparamToday1.DistinctField = null;
			this.txtparamToday1.Font = new System.Drawing.Font("Arial", 8F);
			this.txtparamToday1.ForeColor = System.Drawing.Color.Red;
			this.txtparamToday1.Location = ((System.Drawing.PointF)(resources.GetObject("txtparamToday1.Location")));
			this.txtparamToday1.Name = "txtparamToday1";
			this.txtparamToday1.OutputFormat = null;
			this.txtparamToday1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtparamToday1.Size")));
			this.txtparamToday1.Text = "txtparamToday1";
			// 
			// reportInfo1
			// 
			this.reportInfo1.Alignment = DataDynamics.ActiveReports.TextAlignment.Center;
			this.reportInfo1.Font = new System.Drawing.Font("Arial", 10F);
			this.reportInfo1.FormatString = "Page{PageNumber} of {PageCount}";
			this.reportInfo1.Location = ((System.Drawing.PointF)(resources.GetObject("reportInfo1.Location")));
			this.reportInfo1.Name = "reportInfo1";
			this.reportInfo1.Size = ((System.Drawing.SizeF)(resources.GetObject("reportInfo1.Size")));
			this.reportInfo1.SummaryRunning = DataDynamics.ActiveReports.SummaryRunning.All;
			// 
			// OwnerName
			// 
			this.OwnerName.DefaultValue = "";
			this.OwnerName.Key = "OwnerName";
			this.OwnerName.Prompt = null;
			this.OwnerName.PromptUser = true;
			this.OwnerName.QueryCreated = false;
			this.OwnerName.Tag = null;
			this.OwnerName.Type = DataDynamics.ActiveReports.Parameter.DataType.String;
			// 
			// reportView1
			// 
			this.reportView1.DataSetName = "ReportView";
			this.reportView1.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// Today
			// 
			this.Today.DefaultValue = null;
			this.Today.FieldType = DataDynamics.ActiveReports.FieldTypeEnum.String;
			this.Today.Formula = "System.DateTime.Now.ToString(\"d-MMM-yyyy\")";
			this.Today.Name = "Today";
			this.Today.Tag = null;
			// 
			// GAMeeting
			// 
			this.CalculatedFields.Add(this.Today);
			this.DataMember = "GAMeeting";
			this.DataSource = this.reportView1;
			this.PageSettings.DefaultPaperSize = false;
			this.PageSettings.PaperHeight = 11.69291F;
			this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
			this.PageSettings.PaperWidth = 8.267716F;
			this.Parameters.AddRange(new DataDynamics.ActiveReports.Parameter[] {
																					this.OwnerName});
			this.PrintWidth = 7.0625F;
			this.Script = "public bool ActiveReport_FetchData(bool eof)\n{\n\treturn eof;\n\n}";
			this.Sections.Add(this.pageHeader);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.pageFooter);
			this.ShowParameterUI = false;
			((System.ComponentModel.ISupportInitialize)(this.txtName2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtMeetingReferenceId1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label11)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label9)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtOwnerName1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDateOfMeeting1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label10)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtTimeEnd1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtConductor1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtReporterPersonnelRowId1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtInvolvedPersonnelCount1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDateAndTimeNextMeeting1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtPurpose1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtAgenda1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtComment)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label12)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtparamToday1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.reportInfo1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).EndInit();

		}
		#endregion

		private void detail_Format(object sender, System.EventArgs e)
		{
		
		}
	}
}
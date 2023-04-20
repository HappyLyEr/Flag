using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

namespace ActiveReportGeneration
{
	/// <summary>
	/// Summary description for GAAction.
	/// </summary>
	public class GAAction : DataDynamics.ActiveReports.ActiveReport3
	{
		private DataDynamics.ActiveReports.Detail detail;
		private GASystem.DataModel.View.ReportView reportView1;
		private DataDynamics.ActiveReports.ReportHeader reportHeader1;
		private DataDynamics.ActiveReports.ReportFooter reportFooter1;
		private DataDynamics.ActiveReports.Label label1;
		private DataDynamics.ActiveReports.Label label2;
		private DataDynamics.ActiveReports.Label label3;
		private DataDynamics.ActiveReports.Label label4;
		private DataDynamics.ActiveReports.Label label5;
		private DataDynamics.ActiveReports.Label label6;
		private DataDynamics.ActiveReports.Label label8;
		private DataDynamics.ActiveReports.Label label7;
		private DataDynamics.ActiveReports.Label label9;
		private DataDynamics.ActiveReports.Label label10;
		private DataDynamics.ActiveReports.TextBox txtPriorityListsRowId1;
		private DataDynamics.ActiveReports.TextBox txtResponsibleRoleListsRowId1;
		private DataDynamics.ActiveReports.TextBox txtResponsible1;
		private DataDynamics.ActiveReports.TextBox txtReportDate1;
		private DataDynamics.ActiveReports.TextBox txtDateEndEstimated1;
		private DataDynamics.ActiveReports.TextBox txtDateEndActual1;
		private DataDynamics.ActiveReports.TextBox txtProcedureId1;
		private DataDynamics.ActiveReports.TextBox txtDescription1;
		private DataDynamics.ActiveReports.TextBox txtActionReferenceId1;
		private DataDynamics.ActiveReports.TextBox txtSubject1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GAAction()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GAAction));
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.label2 = new DataDynamics.ActiveReports.Label();
			this.label3 = new DataDynamics.ActiveReports.Label();
			this.label4 = new DataDynamics.ActiveReports.Label();
			this.label5 = new DataDynamics.ActiveReports.Label();
			this.label6 = new DataDynamics.ActiveReports.Label();
			this.label8 = new DataDynamics.ActiveReports.Label();
			this.label7 = new DataDynamics.ActiveReports.Label();
			this.label9 = new DataDynamics.ActiveReports.Label();
			this.label10 = new DataDynamics.ActiveReports.Label();
			this.txtPriorityListsRowId1 = new DataDynamics.ActiveReports.TextBox();
			this.txtResponsibleRoleListsRowId1 = new DataDynamics.ActiveReports.TextBox();
			this.txtResponsible1 = new DataDynamics.ActiveReports.TextBox();
			this.txtReportDate1 = new DataDynamics.ActiveReports.TextBox();
			this.txtDateEndEstimated1 = new DataDynamics.ActiveReports.TextBox();
			this.txtDateEndActual1 = new DataDynamics.ActiveReports.TextBox();
			this.txtProcedureId1 = new DataDynamics.ActiveReports.TextBox();
			this.txtDescription1 = new DataDynamics.ActiveReports.TextBox();
			this.reportView1 = new GASystem.DataModel.View.ReportView();
			this.reportHeader1 = new DataDynamics.ActiveReports.ReportHeader();
			this.label1 = new DataDynamics.ActiveReports.Label();
			this.reportFooter1 = new DataDynamics.ActiveReports.ReportFooter();
			this.txtActionReferenceId1 = new DataDynamics.ActiveReports.TextBox();
			this.txtSubject1 = new DataDynamics.ActiveReports.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label9)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label10)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtPriorityListsRowId1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtResponsibleRoleListsRowId1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtResponsible1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtReportDate1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDateEndEstimated1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDateEndActual1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtProcedureId1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDescription1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtActionReferenceId1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtSubject1)).BeginInit();
			// 
			// detail
			// 
			this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																						 this.label2,
																						 this.label3,
																						 this.label4,
																						 this.label5,
																						 this.label6,
																						 this.label8,
																						 this.label7,
																						 this.label9,
																						 this.label10,
																						 this.txtPriorityListsRowId1,
																						 this.txtResponsibleRoleListsRowId1,
																						 this.txtResponsible1,
																						 this.txtReportDate1,
																						 this.txtDateEndEstimated1,
																						 this.txtDateEndActual1,
																						 this.txtProcedureId1,
																						 this.txtDescription1,
																						 this.txtActionReferenceId1});
			this.detail.Height = 2.15625F;
			this.detail.Name = "detail";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label2.HyperLink = null;
			this.label2.Location = ((System.Drawing.PointF)(resources.GetObject("label2.Location")));
			this.label2.Name = "label2";
			this.label2.Size = ((System.Drawing.SizeF)(resources.GetObject("label2.Size")));
			this.label2.Text = "Action Ref Id:";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label3.HyperLink = null;
			this.label3.Location = ((System.Drawing.PointF)(resources.GetObject("label3.Location")));
			this.label3.Name = "label3";
			this.label3.Size = ((System.Drawing.SizeF)(resources.GetObject("label3.Size")));
			this.label3.Text = "Priority:";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label4.HyperLink = null;
			this.label4.Location = ((System.Drawing.PointF)(resources.GetObject("label4.Location")));
			this.label4.Name = "label4";
			this.label4.Size = ((System.Drawing.SizeF)(resources.GetObject("label4.Size")));
			this.label4.Text = "Responsible";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label5.HyperLink = null;
			this.label5.Location = ((System.Drawing.PointF)(resources.GetObject("label5.Location")));
			this.label5.Name = "label5";
			this.label5.Size = ((System.Drawing.SizeF)(resources.GetObject("label5.Size")));
			this.label5.Text = "Responsible Role";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label6.HyperLink = null;
			this.label6.Location = ((System.Drawing.PointF)(resources.GetObject("label6.Location")));
			this.label6.Name = "label6";
			this.label6.Size = ((System.Drawing.SizeF)(resources.GetObject("label6.Size")));
			this.label6.Text = "Report Date";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label8.HyperLink = null;
			this.label8.Location = ((System.Drawing.PointF)(resources.GetObject("label8.Location")));
			this.label8.Name = "label8";
			this.label8.Size = ((System.Drawing.SizeF)(resources.GetObject("label8.Size")));
			this.label8.Text = "Procedure";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label7.HyperLink = null;
			this.label7.Location = ((System.Drawing.PointF)(resources.GetObject("label7.Location")));
			this.label7.Name = "label7";
			this.label7.Size = ((System.Drawing.SizeF)(resources.GetObject("label7.Size")));
			this.label7.Text = "Date End Estimate";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label9.HyperLink = null;
			this.label9.Location = ((System.Drawing.PointF)(resources.GetObject("label9.Location")));
			this.label9.Name = "label9";
			this.label9.Size = ((System.Drawing.SizeF)(resources.GetObject("label9.Size")));
			this.label9.Text = "Actual End Date";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label10.HyperLink = null;
			this.label10.Location = ((System.Drawing.PointF)(resources.GetObject("label10.Location")));
			this.label10.Name = "label10";
			this.label10.Size = ((System.Drawing.SizeF)(resources.GetObject("label10.Size")));
			this.label10.Text = "Description";
			// 
			// txtPriorityListsRowId1
			// 
			this.txtPriorityListsRowId1.DataField = "PriorityListsRowId";
			this.txtPriorityListsRowId1.DistinctField = null;
			this.txtPriorityListsRowId1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtPriorityListsRowId1.Location = ((System.Drawing.PointF)(resources.GetObject("txtPriorityListsRowId1.Location")));
			this.txtPriorityListsRowId1.Name = "txtPriorityListsRowId1";
			this.txtPriorityListsRowId1.OutputFormat = null;
			this.txtPriorityListsRowId1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtPriorityListsRowId1.Size")));
			this.txtPriorityListsRowId1.Text = "txtPriorityListsRowId1";
			// 
			// txtResponsibleRoleListsRowId1
			// 
			this.txtResponsibleRoleListsRowId1.DataField = "ResponsibleRoleListsRowId";
			this.txtResponsibleRoleListsRowId1.DistinctField = null;
			this.txtResponsibleRoleListsRowId1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtResponsibleRoleListsRowId1.Location = ((System.Drawing.PointF)(resources.GetObject("txtResponsibleRoleListsRowId1.Location")));
			this.txtResponsibleRoleListsRowId1.Name = "txtResponsibleRoleListsRowId1";
			this.txtResponsibleRoleListsRowId1.OutputFormat = null;
			this.txtResponsibleRoleListsRowId1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtResponsibleRoleListsRowId1.Size")));
			this.txtResponsibleRoleListsRowId1.Text = "txtResponsibleRoleListsRowId1";
			// 
			// txtResponsible1
			// 
			this.txtResponsible1.DataField = "Responsible";
			this.txtResponsible1.DistinctField = null;
			this.txtResponsible1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtResponsible1.Location = ((System.Drawing.PointF)(resources.GetObject("txtResponsible1.Location")));
			this.txtResponsible1.Name = "txtResponsible1";
			this.txtResponsible1.OutputFormat = null;
			this.txtResponsible1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtResponsible1.Size")));
			this.txtResponsible1.Text = "txtResponsible1";
			// 
			// txtReportDate1
			// 
			this.txtReportDate1.DataField = "ReportDate";
			this.txtReportDate1.DistinctField = null;
			this.txtReportDate1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtReportDate1.Location = ((System.Drawing.PointF)(resources.GetObject("txtReportDate1.Location")));
			this.txtReportDate1.Name = "txtReportDate1";
			this.txtReportDate1.OutputFormat = "d-MMM-yyyy hh:mm";
			this.txtReportDate1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtReportDate1.Size")));
			this.txtReportDate1.Text = "txtReportDate1";
			// 
			// txtDateEndEstimated1
			// 
			this.txtDateEndEstimated1.DataField = "DateEndEstimated";
			this.txtDateEndEstimated1.DistinctField = null;
			this.txtDateEndEstimated1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtDateEndEstimated1.Location = ((System.Drawing.PointF)(resources.GetObject("txtDateEndEstimated1.Location")));
			this.txtDateEndEstimated1.Name = "txtDateEndEstimated1";
			this.txtDateEndEstimated1.OutputFormat = "d-MMM-yyyy";
			this.txtDateEndEstimated1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtDateEndEstimated1.Size")));
			this.txtDateEndEstimated1.Text = "txtDateEndEstimated1";
			// 
			// txtDateEndActual1
			// 
			this.txtDateEndActual1.DataField = "DateEndActual";
			this.txtDateEndActual1.DistinctField = null;
			this.txtDateEndActual1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtDateEndActual1.Location = ((System.Drawing.PointF)(resources.GetObject("txtDateEndActual1.Location")));
			this.txtDateEndActual1.Name = "txtDateEndActual1";
			this.txtDateEndActual1.OutputFormat = "d-MMM-yyyy hh:mm";
			this.txtDateEndActual1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtDateEndActual1.Size")));
			this.txtDateEndActual1.Text = "txtDateEndActual1";
			// 
			// txtProcedureId1
			// 
			this.txtProcedureId1.DataField = "ProcedureId";
			this.txtProcedureId1.DistinctField = null;
			this.txtProcedureId1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtProcedureId1.Location = ((System.Drawing.PointF)(resources.GetObject("txtProcedureId1.Location")));
			this.txtProcedureId1.Name = "txtProcedureId1";
			this.txtProcedureId1.OutputFormat = null;
			this.txtProcedureId1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtProcedureId1.Size")));
			this.txtProcedureId1.Text = "txtProcedureId1";
			// 
			// txtDescription1
			// 
			this.txtDescription1.DataField = "Description";
			this.txtDescription1.DistinctField = null;
			this.txtDescription1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtDescription1.Location = ((System.Drawing.PointF)(resources.GetObject("txtDescription1.Location")));
			this.txtDescription1.Name = "txtDescription1";
			this.txtDescription1.OutputFormat = null;
			this.txtDescription1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtDescription1.Size")));
			this.txtDescription1.Text = "txtDescription1";
			// 
			// reportView1
			// 
			this.reportView1.DataSetName = "ReportView";
			this.reportView1.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// reportHeader1
			// 
			this.reportHeader1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(233)), ((System.Byte)(235)), ((System.Byte)(235)));
			this.reportHeader1.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																								this.label1,
																								this.txtSubject1});
			this.reportHeader1.Height = 0.6979167F;
			this.reportHeader1.Name = "reportHeader1";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.HyperLink = null;
			this.label1.Location = ((System.Drawing.PointF)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.Size = ((System.Drawing.SizeF)(resources.GetObject("label1.Size")));
			this.label1.Text = "Action";
			// 
			// reportFooter1
			// 
			this.reportFooter1.Height = 0.25F;
			this.reportFooter1.Name = "reportFooter1";
			// 
			// txtActionReferenceId1
			// 
			this.txtActionReferenceId1.DataField = "ActionReferenceId";
			this.txtActionReferenceId1.DistinctField = null;
			this.txtActionReferenceId1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtActionReferenceId1.Location = ((System.Drawing.PointF)(resources.GetObject("txtActionReferenceId1.Location")));
			this.txtActionReferenceId1.Name = "txtActionReferenceId1";
			this.txtActionReferenceId1.OutputFormat = null;
			this.txtActionReferenceId1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtActionReferenceId1.Size")));
			this.txtActionReferenceId1.Text = "txtActionReferenceId1";
			// 
			// txtSubject1
			// 
			this.txtSubject1.DataField = "Subject";
			this.txtSubject1.DistinctField = null;
			this.txtSubject1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtSubject1.Location = ((System.Drawing.PointF)(resources.GetObject("txtSubject1.Location")));
			this.txtSubject1.Name = "txtSubject1";
			this.txtSubject1.OutputFormat = null;
			this.txtSubject1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtSubject1.Size")));
			this.txtSubject1.Text = "txtSubject1";
			// 
			// GAAction
			// 
			this.DataMember = "GAAction";
			this.DataSource = this.reportView1;
			this.PageSettings.PaperHeight = 11.69F;
			this.PageSettings.PaperWidth = 8.27F;
			this.PrintWidth = 7.0625F;
			this.Sections.Add(this.reportHeader1);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.reportFooter1);
			((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label9)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label10)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtPriorityListsRowId1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtResponsibleRoleListsRowId1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtResponsible1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtReportDate1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDateEndEstimated1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDateEndActual1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtProcedureId1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDescription1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtActionReferenceId1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtSubject1)).EndInit();

		}
		#endregion
	}
}
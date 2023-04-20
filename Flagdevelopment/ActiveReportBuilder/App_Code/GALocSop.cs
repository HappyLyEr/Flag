using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

namespace ActiveReportBuilder
{
	/// <summary>
	/// Summary description for GALocSop.
	/// </summary>
	public class GALocSop : DataDynamics.ActiveReports.ActiveReport3
	{
		private DataDynamics.ActiveReports.Detail detail;
		private GASystem.DataModel.View.ReportView reportView1;
		private DataDynamics.ActiveReports.TextBox txtName1;
		private DataDynamics.ActiveReports.Label label1;
		private DataDynamics.ActiveReports.Label label2;
		private DataDynamics.ActiveReports.Label label3;
		private DataDynamics.ActiveReports.SubReport subReport1;
		private DataDynamics.ActiveReports.ReportHeader reportHeader1;
		private DataDynamics.ActiveReports.ReportFooter reportFooter1;
		private DataDynamics.ActiveReports.Label label4;
		private DataDynamics.ActiveReports.Parameter EndDate;
		private DataDynamics.ActiveReports.Parameter StartDate;
		private DataDynamics.ActiveReports.Parameter OwnerName;
		private DataDynamics.ActiveReports.TextBox txtEndDate1;
		private DataDynamics.ActiveReports.TextBox txtStartDate1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GALocSop()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GALocSop));
			this.txtName1 = new DataDynamics.ActiveReports.TextBox();
			this.label1 = new DataDynamics.ActiveReports.Label();
			this.label2 = new DataDynamics.ActiveReports.Label();
			this.label3 = new DataDynamics.ActiveReports.Label();
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.subReport1 = new DataDynamics.ActiveReports.SubReport();
			this.reportView1 = new GASystem.DataModel.View.ReportView();
			this.reportHeader1 = new DataDynamics.ActiveReports.ReportHeader();
			this.label4 = new DataDynamics.ActiveReports.Label();
			this.reportFooter1 = new DataDynamics.ActiveReports.ReportFooter();
			this.EndDate = new DataDynamics.ActiveReports.Parameter();
			this.StartDate = new DataDynamics.ActiveReports.Parameter();
			this.OwnerName = new DataDynamics.ActiveReports.Parameter();
			this.txtEndDate1 = new DataDynamics.ActiveReports.TextBox();
			this.txtStartDate1 = new DataDynamics.ActiveReports.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.txtName1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtEndDate1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtStartDate1)).BeginInit();
			// 
			// txtName1
			// 
			this.txtName1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtName1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtName1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtName1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtName1.DataField = "Name";
			this.txtName1.DistinctField = null;
			this.txtName1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtName1.Location = ((System.Drawing.PointF)(resources.GetObject("txtName1.Location")));
			this.txtName1.Name = "txtName1";
			this.txtName1.OutputFormat = null;
			this.txtName1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtName1.Size")));
			this.txtName1.Text = "txtName1";
			// 
			// label1
			// 
			this.label1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label1.Font = new System.Drawing.Font("Arial", 10F);
			this.label1.HyperLink = null;
			this.label1.Location = ((System.Drawing.PointF)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.Size = ((System.Drawing.SizeF)(resources.GetObject("label1.Size")));
			this.label1.Text = "SOP Report for";
			// 
			// label2
			// 
			this.label2.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label2.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label2.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label2.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label2.Font = new System.Drawing.Font("Arial", 10F);
			this.label2.HyperLink = null;
			this.label2.Location = ((System.Drawing.PointF)(resources.GetObject("label2.Location")));
			this.label2.Name = "label2";
			this.label2.Size = ((System.Drawing.SizeF)(resources.GetObject("label2.Size")));
			this.label2.Text = "To:";
			// 
			// label3
			// 
			this.label3.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label3.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label3.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label3.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label3.Font = new System.Drawing.Font("Arial", 10F);
			this.label3.HyperLink = null;
			this.label3.Location = ((System.Drawing.PointF)(resources.GetObject("label3.Location")));
			this.label3.Name = "label3";
			this.label3.Size = ((System.Drawing.SizeF)(resources.GetObject("label3.Size")));
			this.label3.Text = "From: ";
			// 
			// detail
			// 
			this.detail.ColumnSpacing = 0F;
			this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																						 this.subReport1,
																						 this.label2,
																						 this.label1,
																						 this.txtEndDate1,
																						 this.label4,
																						 this.txtName1,
																						 this.label3,
																						 this.txtStartDate1});
			this.detail.Height = 2.614583F;
			this.detail.Name = "detail";
			// 
			// subReport1
			// 
			this.subReport1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.subReport1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.subReport1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.subReport1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.subReport1.CloseBorder = false;
			this.subReport1.Location = ((System.Drawing.PointF)(resources.GetObject("subReport1.Location")));
			this.subReport1.Name = "subReport1";
			this.subReport1.Report = null;
			this.subReport1.ReportName = "SOPList";
			this.subReport1.Size = ((System.Drawing.SizeF)(resources.GetObject("subReport1.Size")));
			// 
			// reportView1
			// 
			this.reportView1.DataSetName = "ReportView";
			this.reportView1.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// reportHeader1
			// 
			this.reportHeader1.Height = 0.1875F;
			this.reportHeader1.Name = "reportHeader1";
			// 
			// label4
			// 
			this.label4.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label4.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label4.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label4.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label4.Font = new System.Drawing.Font("Arial", 10F);
			this.label4.HyperLink = null;
			this.label4.Location = ((System.Drawing.PointF)(resources.GetObject("label4.Location")));
			this.label4.Name = "label4";
			this.label4.Size = ((System.Drawing.SizeF)(resources.GetObject("label4.Size")));
			this.label4.Text = "";
			// 
			// reportFooter1
			// 
			this.reportFooter1.Height = 0.25F;
			this.reportFooter1.Name = "reportFooter1";
			// 
			// EndDate
			// 
			this.EndDate.DefaultValue = "";
			this.EndDate.Key = "EndDate";
			this.EndDate.Prompt = null;
			this.EndDate.PromptUser = false;
			this.EndDate.QueryCreated = false;
			this.EndDate.Tag = null;
			this.EndDate.Type = DataDynamics.ActiveReports.Parameter.DataType.Date;
			// 
			// StartDate
			// 
			this.StartDate.DefaultValue = "";
			this.StartDate.Key = "StartDate";
			this.StartDate.Prompt = null;
			this.StartDate.PromptUser = false;
			this.StartDate.QueryCreated = false;
			this.StartDate.Tag = null;
			this.StartDate.Type = DataDynamics.ActiveReports.Parameter.DataType.Date;
			// 
			// OwnerName
			// 
			this.OwnerName.DefaultValue = "norsolutions";
			this.OwnerName.Key = "OwnerName";
			this.OwnerName.Prompt = null;
			this.OwnerName.PromptUser = false;
			this.OwnerName.QueryCreated = false;
			this.OwnerName.Tag = null;
			this.OwnerName.Type = DataDynamics.ActiveReports.Parameter.DataType.String;
			// 
			// txtEndDate1
			// 
			this.txtEndDate1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtEndDate1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtEndDate1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtEndDate1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtEndDate1.DataField = "param:EndDate";
			this.txtEndDate1.DistinctField = null;
			this.txtEndDate1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtEndDate1.Location = ((System.Drawing.PointF)(resources.GetObject("txtEndDate1.Location")));
			this.txtEndDate1.Name = "txtEndDate1";
			this.txtEndDate1.OutputFormat = "dd-MMM-yyyy";
			this.txtEndDate1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtEndDate1.Size")));
			this.txtEndDate1.Text = "txtEndDate1";
			// 
			// txtStartDate1
			// 
			this.txtStartDate1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtStartDate1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtStartDate1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtStartDate1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtStartDate1.DataField = "param:StartDate";
			this.txtStartDate1.DistinctField = null;
			this.txtStartDate1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtStartDate1.Location = ((System.Drawing.PointF)(resources.GetObject("txtStartDate1.Location")));
			this.txtStartDate1.Name = "txtStartDate1";
			this.txtStartDate1.OutputFormat = "dd-MMM-yyyy";
			this.txtStartDate1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtStartDate1.Size")));
			this.txtStartDate1.Text = "txtStartDate1";
			// 
			// GALocSop
			// 
			this.DataMember = "GALocation";
			this.DataSource = this.reportView1;
			this.PageSettings.PaperHeight = 11F;
			this.PageSettings.PaperWidth = 8.5F;
			this.Parameters.Add(this.EndDate);
			this.Parameters.Add(this.StartDate);
			this.Parameters.Add(this.OwnerName);
			this.PrintWidth = 11.0625F;
			this.Script = "public bool ActiveReport_FetchData(bool eof)\n{\n\treturn eof;\n\n}";
			this.Sections.Add(this.reportHeader1);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.reportFooter1);
			this.ShowParameterUI = false;
			((System.ComponentModel.ISupportInitialize)(this.txtName1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtEndDate1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtStartDate1)).EndInit();

		}
		#endregion
	}
}
using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

namespace ActiveReportBuilder
{
	/// <summary>
	/// Summary description for LocationSOPs.
	/// </summary>
	public class LocationSOPs : DataDynamics.ActiveReports.ActiveReport3
	{
		private DataDynamics.ActiveReports.PageHeader pageHeader;
		private DataDynamics.ActiveReports.Detail detail;
		private DataDynamics.ActiveReports.PageFooter pageFooter;
		private GASystem.DataModel.View.ReportView reportView1;
		private DataDynamics.ActiveReports.Label label1;
		private DataDynamics.ActiveReports.TextBox txtName1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public LocationSOPs()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(LocationSOPs));
			this.pageHeader = new DataDynamics.ActiveReports.PageHeader();
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
			this.reportView1 = new GASystem.DataModel.View.ReportView();
			this.label1 = new DataDynamics.ActiveReports.Label();
			this.txtName1 = new DataDynamics.ActiveReports.TextBox();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtName1)).BeginInit();
			// 
			// pageHeader
			// 
			this.pageHeader.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																							 this.label1,
																							 this.txtName1});
			this.pageHeader.Height = 0.5625F;
			this.pageHeader.Name = "pageHeader";
			// 
			// detail
			// 
			this.detail.Height = 2F;
			this.detail.Name = "detail";
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
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.label1.HyperLink = null;
			this.label1.Location = ((System.Drawing.PointF)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.Size = ((System.Drawing.SizeF)(resources.GetObject("label1.Size")));
			this.label1.Text = "SOP Report - Reporting Open SOP\'s only";
			// 
			// txtName1
			// 
			this.txtName1.DataField = "Name";
			this.txtName1.DistinctField = null;
			this.txtName1.Font = new System.Drawing.Font("Arial", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtName1.Location = ((System.Drawing.PointF)(resources.GetObject("txtName1.Location")));
			this.txtName1.Name = "txtName1";
			this.txtName1.OutputFormat = null;
			this.txtName1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtName1.Size")));
			this.txtName1.Text = "txtName1";
			// 
			// LocationSOPs
			// 
			this.DataMember = "GALocation";
			this.DataSource = this.reportView1;
			this.PageSettings.PaperHeight = 11F;
			this.PageSettings.PaperWidth = 8.5F;
			this.Sections.Add(this.pageHeader);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.pageFooter);
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtName1)).EndInit();

		}
		#endregion
	}
}
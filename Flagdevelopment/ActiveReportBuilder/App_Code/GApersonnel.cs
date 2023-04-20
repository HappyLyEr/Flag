using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

namespace ActiveReportGeneration
{
	/// <summary>
	/// Summary description for GApersonnel.
	/// </summary>
	public class GApersonnel : DataDynamics.ActiveReports.ActiveReport3
	{
		private DataDynamics.ActiveReports.PageHeader pageHeader;
		private DataDynamics.ActiveReports.Detail detail;
		private DataDynamics.ActiveReports.PageFooter pageFooter;
		private GASystem.DataModel.View.ReportView reportView1;
		private DataDynamics.ActiveReports.TextBox txtFamilyName1;
		private DataDynamics.ActiveReports.TextBox txtGivenName1;
		private DataDynamics.ActiveReports.Field FullName;
		private DataDynamics.ActiveReports.TextBox txtFullName1;
		private DataDynamics.ActiveReports.GroupHeader groupHeader1;
		private DataDynamics.ActiveReports.GroupFooter groupFooter1;
		private DataDynamics.ActiveReports.SubReport subReport1;
		private DataDynamics.ActiveReports.Picture picture1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GApersonnel()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GApersonnel));
			this.pageHeader = new DataDynamics.ActiveReports.PageHeader();
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.pageFooter = new DataDynamics.ActiveReports.PageFooter();
			this.reportView1 = new GASystem.DataModel.View.ReportView();
			this.txtFamilyName1 = new DataDynamics.ActiveReports.TextBox();
			this.txtGivenName1 = new DataDynamics.ActiveReports.TextBox();
			this.FullName = new DataDynamics.ActiveReports.Field();
			this.txtFullName1 = new DataDynamics.ActiveReports.TextBox();
			this.groupHeader1 = new DataDynamics.ActiveReports.GroupHeader();
			this.groupFooter1 = new DataDynamics.ActiveReports.GroupFooter();
			this.subReport1 = new DataDynamics.ActiveReports.SubReport();
			this.picture1 = new DataDynamics.ActiveReports.Picture();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtFamilyName1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtGivenName1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtFullName1)).BeginInit();
			// 
			// pageHeader
			// 
			this.pageHeader.Height = 0.25F;
			this.pageHeader.Name = "pageHeader";
			// 
			// detail
			// 
			this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																						 this.txtFamilyName1,
																						 this.txtGivenName1,
																						 this.txtFullName1,
																						 this.subReport1});
			this.detail.Height = 2F;
			this.detail.Name = "detail";
			this.detail.Format += new System.EventHandler(this.detail_Format);
			// 
			// pageFooter
			// 
			this.pageFooter.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																							 this.picture1});
			this.pageFooter.Height = 1.708333F;
			this.pageFooter.Name = "pageFooter";
			// 
			// reportView1
			// 
			this.reportView1.DataSetName = "ReportView";
			this.reportView1.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// txtFamilyName1
			// 
			this.txtFamilyName1.BackColor = System.Drawing.Color.Yellow;
			this.txtFamilyName1.DataField = "FamilyName";
			this.txtFamilyName1.DistinctField = null;
			this.txtFamilyName1.Font = new System.Drawing.Font("AvantGarde Bk BT", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtFamilyName1.Location = ((System.Drawing.PointF)(resources.GetObject("txtFamilyName1.Location")));
			this.txtFamilyName1.Name = "txtFamilyName1";
			this.txtFamilyName1.OutputFormat = null;
			this.txtFamilyName1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtFamilyName1.Size")));
			this.txtFamilyName1.Text = "txtFamilyName1";
			// 
			// txtGivenName1
			// 
			this.txtGivenName1.BackColor = System.Drawing.Color.Yellow;
			this.txtGivenName1.DataField = "GivenName";
			this.txtGivenName1.DistinctField = null;
			this.txtGivenName1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtGivenName1.Location = ((System.Drawing.PointF)(resources.GetObject("txtGivenName1.Location")));
			this.txtGivenName1.Name = "txtGivenName1";
			this.txtGivenName1.OutputFormat = null;
			this.txtGivenName1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtGivenName1.Size")));
			// 
			// FullName
			// 
			this.FullName.DefaultValue = null;
			this.FullName.FieldType = DataDynamics.ActiveReports.FieldTypeEnum.String;
			this.FullName.Formula = "GivenName + \" \" + FamilyName";
			this.FullName.Name = "FullName";
			this.FullName.Tag = null;
			// 
			// txtFullName1
			// 
			this.txtFullName1.BackColor = System.Drawing.Color.Yellow;
			this.txtFullName1.DataField = "FullName";
			this.txtFullName1.DistinctField = null;
			this.txtFullName1.Font = new System.Drawing.Font("AvantGarde Bk BT", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.txtFullName1.Location = ((System.Drawing.PointF)(resources.GetObject("txtFullName1.Location")));
			this.txtFullName1.Name = "txtFullName1";
			this.txtFullName1.OutputFormat = null;
			this.txtFullName1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtFullName1.Size")));
			this.txtFullName1.Text = "txtFullName1";
			// 
			// groupHeader1
			// 
			this.groupHeader1.Height = 0.25F;
			this.groupHeader1.Name = "groupHeader1";
			// 
			// groupFooter1
			// 
			this.groupFooter1.Height = 0.01041667F;
			this.groupFooter1.Name = "groupFooter1";
			// 
			// subReport1
			// 
			this.subReport1.CloseBorder = false;
			this.subReport1.Location = ((System.Drawing.PointF)(resources.GetObject("subReport1.Location")));
			this.subReport1.Name = "subReport1";
			this.subReport1.Report = null;
			this.subReport1.ReportName = "GAMeansOfContact";
			this.subReport1.Size = ((System.Drawing.SizeF)(resources.GetObject("subReport1.Size")));
			// 
			// picture1
			// 
			this.picture1.DataField = "GivenName";
			this.picture1.Image = null;
			this.picture1.LineWeight = 0F;
			this.picture1.Location = ((System.Drawing.PointF)(resources.GetObject("picture1.Location")));
			this.picture1.Name = "picture1";
			this.picture1.Size = ((System.Drawing.SizeF)(resources.GetObject("picture1.Size")));
			// 
			// GApersonnel
			// 
			this.CalculatedFields.Add(this.FullName);
			this.DataMember = "GAPersonnel";
			this.DataSource = this.reportView1;
			this.PageSettings.PaperHeight = 11.69F;
			this.PageSettings.PaperWidth = 8.27F;
			this.Sections.Add(this.pageHeader);
			this.Sections.Add(this.groupHeader1);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.groupFooter1);
			this.Sections.Add(this.pageFooter);
			this.ReportStart += new System.EventHandler(this.GApersonnel_ReportStart);
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtFamilyName1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtGivenName1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtFullName1)).EndInit();

		}
		#endregion

		private void GApersonnel_ReportStart(object sender, System.EventArgs e)
		{
		
		}

		private void detail_Format(object sender, System.EventArgs e)
		{
		
		}
	}
}
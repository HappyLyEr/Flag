using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

namespace ActiveReportGeneration
{
	/// <summary>
	/// Summary description for GAMeetingPersonList.
	/// </summary>
	public class GAMeetingPersonList : DataDynamics.ActiveReports.ActiveReport3
	{
		private DataDynamics.ActiveReports.Detail detail;
		private GASystem.DataModel.View.ReportView reportView1;
		private DataDynamics.ActiveReports.TextBox txtPersonId1;
		private DataDynamics.ActiveReports.TextBox txtComment1;
		private DataDynamics.ActiveReports.CheckBox checkBox1;
		private DataDynamics.ActiveReports.ReportHeader reportHeader1;
		private DataDynamics.ActiveReports.ReportFooter reportFooter1;
		private DataDynamics.ActiveReports.Label label1;
		private DataDynamics.ActiveReports.TextBox textBox1;
		private DataDynamics.ActiveReports.Label label2;
		private DataDynamics.ActiveReports.Label label3;
		private DataDynamics.ActiveReports.CheckBox checkBox2;
		private DataDynamics.ActiveReports.Label label4;
		private DataDynamics.ActiveReports.CheckBox checkBox3;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GAMeetingPersonList()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GAMeetingPersonList));
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.txtPersonId1 = new DataDynamics.ActiveReports.TextBox();
			this.txtComment1 = new DataDynamics.ActiveReports.TextBox();
			this.checkBox1 = new DataDynamics.ActiveReports.CheckBox();
			this.checkBox2 = new DataDynamics.ActiveReports.CheckBox();
			this.checkBox3 = new DataDynamics.ActiveReports.CheckBox();
			this.reportView1 = new GASystem.DataModel.View.ReportView();
			this.reportHeader1 = new DataDynamics.ActiveReports.ReportHeader();
			this.label1 = new DataDynamics.ActiveReports.Label();
			this.textBox1 = new DataDynamics.ActiveReports.TextBox();
			this.label2 = new DataDynamics.ActiveReports.Label();
			this.label3 = new DataDynamics.ActiveReports.Label();
			this.label4 = new DataDynamics.ActiveReports.Label();
			this.reportFooter1 = new DataDynamics.ActiveReports.ReportFooter();
			((System.ComponentModel.ISupportInitialize)(this.txtPersonId1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtComment1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.checkBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.checkBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.checkBox3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
			// 
			// detail
			// 
			this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																						 this.txtPersonId1,
																						 this.txtComment1,
																						 this.checkBox1,
																						 this.checkBox2,
																						 this.checkBox3});
			this.detail.Height = 0.3020833F;
			this.detail.Name = "detail";
			// 
			// txtPersonId1
			// 
			this.txtPersonId1.DataField = "PersonId";
			this.txtPersonId1.DistinctField = null;
			this.txtPersonId1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtPersonId1.Location = ((System.Drawing.PointF)(resources.GetObject("txtPersonId1.Location")));
			this.txtPersonId1.Name = "txtPersonId1";
			this.txtPersonId1.OutputFormat = null;
			this.txtPersonId1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtPersonId1.Size")));
			this.txtPersonId1.Text = "txtPersonId1";
			// 
			// txtComment1
			// 
			this.txtComment1.DataField = "Comment";
			this.txtComment1.DistinctField = null;
			this.txtComment1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtComment1.Location = ((System.Drawing.PointF)(resources.GetObject("txtComment1.Location")));
			this.txtComment1.Name = "txtComment1";
			this.txtComment1.OutputFormat = null;
			this.txtComment1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtComment1.Size")));
			this.txtComment1.Text = "txtComment1";
			// 
			// checkBox1
			// 
			this.checkBox1.DataField = "Absent";
			this.checkBox1.Font = new System.Drawing.Font("Arial", 10F);
			this.checkBox1.Location = ((System.Drawing.PointF)(resources.GetObject("checkBox1.Location")));
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = ((System.Drawing.SizeF)(resources.GetObject("checkBox1.Size")));
			this.checkBox1.Text = "";
			// 
			// checkBox2
			// 
			this.checkBox2.DataField = "Participant";
			this.checkBox2.Font = new System.Drawing.Font("Arial", 10F);
			this.checkBox2.Location = ((System.Drawing.PointF)(resources.GetObject("checkBox2.Location")));
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = ((System.Drawing.SizeF)(resources.GetObject("checkBox2.Size")));
			this.checkBox2.Tag = "";
			this.checkBox2.Text = "";
			// 
			// checkBox3
			// 
			this.checkBox3.DataField = "DistributionTo";
			this.checkBox3.Font = new System.Drawing.Font("Arial", 10F);
			this.checkBox3.Location = ((System.Drawing.PointF)(resources.GetObject("checkBox3.Location")));
			this.checkBox3.Name = "checkBox3";
			this.checkBox3.Size = ((System.Drawing.SizeF)(resources.GetObject("checkBox3.Size")));
			this.checkBox3.Text = "";
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
																								this.textBox1,
																								this.label2,
																								this.label3,
																								this.label4});
			this.reportHeader1.Height = 0.5208333F;
			this.reportHeader1.Name = "reportHeader1";
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label1.HyperLink = null;
			this.label1.Location = ((System.Drawing.PointF)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.Size = ((System.Drawing.SizeF)(resources.GetObject("label1.Size")));
			this.label1.Text = "Comment";
			// 
			// textBox1
			// 
			this.textBox1.DistinctField = null;
			this.textBox1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.textBox1.Location = ((System.Drawing.PointF)(resources.GetObject("textBox1.Location")));
			this.textBox1.Name = "textBox1";
			this.textBox1.OutputFormat = null;
			this.textBox1.Size = ((System.Drawing.SizeF)(resources.GetObject("textBox1.Size")));
			this.textBox1.Text = "Involved Persons:";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label2.HyperLink = null;
			this.label2.Location = ((System.Drawing.PointF)(resources.GetObject("label2.Location")));
			this.label2.Name = "label2";
			this.label2.Size = ((System.Drawing.SizeF)(resources.GetObject("label2.Size")));
			this.label2.Text = "Absent";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label3.HyperLink = null;
			this.label3.Location = ((System.Drawing.PointF)(resources.GetObject("label3.Location")));
			this.label3.Name = "label3";
			this.label3.Size = ((System.Drawing.SizeF)(resources.GetObject("label3.Size")));
			this.label3.Text = "Participant";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label4.HyperLink = null;
			this.label4.Location = ((System.Drawing.PointF)(resources.GetObject("label4.Location")));
			this.label4.Name = "label4";
			this.label4.Size = ((System.Drawing.SizeF)(resources.GetObject("label4.Size")));
			this.label4.Text = "To";
			// 
			// reportFooter1
			// 
			this.reportFooter1.Height = 0.0625F;
			this.reportFooter1.Name = "reportFooter1";
			// 
			// GAMeetingPersonList
			// 
			this.DataMember = "GAMeetingPersonList";
			this.DataSource = this.reportView1;
			this.PageSettings.PaperHeight = 11.69F;
			this.PageSettings.PaperWidth = 8.27F;
			this.PrintWidth = 7.0625F;
			this.Sections.Add(this.reportHeader1);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.reportFooter1);
			((System.ComponentModel.ISupportInitialize)(this.txtPersonId1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtComment1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.checkBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.checkBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.checkBox3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.textBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();

		}
		#endregion
	}
}
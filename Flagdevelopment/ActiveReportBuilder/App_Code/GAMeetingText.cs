using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

namespace ActiveReportGeneration
{
	/// <summary>
	/// Summary description for GAMeetingText.
	/// </summary>
	public class GAMeetingText : DataDynamics.ActiveReports.ActiveReport3
	{
		private DataDynamics.ActiveReports.Detail detail;
		private GASystem.DataModel.View.ReportView reportView1;
		private DataDynamics.ActiveReports.Field field1;
		private DataDynamics.ActiveReports.RichTextBox richTextBox1;
		private DataDynamics.ActiveReports.ReportHeader reportHeader1;
		private DataDynamics.ActiveReports.ReportFooter reportFooter1;
		private DataDynamics.ActiveReports.Label label2;
		private DataDynamics.ActiveReports.TextBox txtName1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GAMeetingText()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(GAMeetingText));
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.richTextBox1 = new DataDynamics.ActiveReports.RichTextBox();
			this.txtName1 = new DataDynamics.ActiveReports.TextBox();
			this.reportView1 = new GASystem.DataModel.View.ReportView();
			this.field1 = new DataDynamics.ActiveReports.Field();
			this.reportHeader1 = new DataDynamics.ActiveReports.ReportHeader();
			this.label2 = new DataDynamics.ActiveReports.Label();
			this.reportFooter1 = new DataDynamics.ActiveReports.ReportFooter();
			((System.ComponentModel.ISupportInitialize)(this.txtName1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
			// 
			// detail
			// 
			this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																						 this.richTextBox1,
																						 this.txtName1});
			this.detail.Height = 2.010417F;
			this.detail.Name = "detail";
			this.detail.Format += new System.EventHandler(this.detail_Format);
			// 
			// richTextBox1
			// 
			this.richTextBox1.AutoReplaceFields = true;
			this.richTextBox1.BackColor = System.Drawing.Color.Transparent;
			this.richTextBox1.DataField = "field1";
			this.richTextBox1.Font = new System.Drawing.Font("Arial", 10F);
			this.richTextBox1.ForeColor = System.Drawing.Color.Black;
			this.richTextBox1.Location = ((System.Drawing.PointF)(resources.GetObject("richTextBox1.Location")));
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.RTF = "{\\rtf1{\\fonttbl{\\f0\\fnil\\fcharset0 Arial;}}{\\stylesheet{\\ql\\li0\\ri0\\nowidctlpar\\s" +
				"l240\\slmult\\faauto\\fs20\\f0 Normal;}}\\pard\\plain\\s0\\li0\\ri0\\ql\\nowidctlpar\\sl240\\" +
				"slmult\\faauto\\f0\\fs20{\\f0 richTextBox1\\par}}";
			this.richTextBox1.SelectedText = "";
			this.richTextBox1.SelectionAlignment = DataDynamics.ActiveReports.TextAlignment.Left;
			this.richTextBox1.SelectionBackColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.richTextBox1.SelectionBullet = false;
			this.richTextBox1.SelectionCharOffset = 0F;
			this.richTextBox1.SelectionColor = System.Drawing.Color.FromArgb(((System.Byte)(0)), ((System.Byte)(0)), ((System.Byte)(0)));
			this.richTextBox1.SelectionFont = new System.Drawing.Font("Arial", 10F);
			this.richTextBox1.SelectionHangingIndent = 0F;
			this.richTextBox1.SelectionIndent = 0F;
			this.richTextBox1.SelectionLength = 0;
			this.richTextBox1.SelectionRightIndent = 0F;
			this.richTextBox1.SelectionStart = 0;
			this.richTextBox1.Size = ((System.Drawing.SizeF)(resources.GetObject("richTextBox1.Size")));
			// 
			// txtName1
			// 
			this.txtName1.DataField = "Name";
			this.txtName1.DistinctField = null;
			this.txtName1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.txtName1.Location = ((System.Drawing.PointF)(resources.GetObject("txtName1.Location")));
			this.txtName1.Name = "txtName1";
			this.txtName1.OutputFormat = null;
			this.txtName1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtName1.Size")));
			this.txtName1.Text = "txtName1";
			// 
			// reportView1
			// 
			this.reportView1.DataSetName = "ReportView";
			this.reportView1.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// field1
			// 
			this.field1.DefaultValue = null;
			this.field1.FieldType = DataDynamics.ActiveReports.FieldTypeEnum.String;
			this.field1.Formula = "\"<html>\" + Text";
			this.field1.Name = "field1";
			this.field1.Tag = null;
			// 
			// reportHeader1
			// 
			this.reportHeader1.BackColor = System.Drawing.Color.FromArgb(((System.Byte)(233)), ((System.Byte)(235)), ((System.Byte)(235)));
			this.reportHeader1.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																								this.label2});
			this.reportHeader1.Height = 0.4270833F;
			this.reportHeader1.Name = "reportHeader1";
			this.reportHeader1.Tag = System.Drawing.Color.White;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Times New Roman", 16F);
			this.label2.HyperLink = null;
			this.label2.Location = ((System.Drawing.PointF)(resources.GetObject("label2.Location")));
			this.label2.Name = "label2";
			this.label2.Size = ((System.Drawing.SizeF)(resources.GetObject("label2.Size")));
			this.label2.Text = "Details";
			// 
			// reportFooter1
			// 
			this.reportFooter1.Height = 0.25F;
			this.reportFooter1.Name = "reportFooter1";
			// 
			// GAMeetingText
			// 
			this.CalculatedFields.Add(this.field1);
			this.DataMember = "GAMeetingText";
			this.DataSource = this.reportView1;
			this.PageSettings.PaperHeight = 11.69F;
			this.PageSettings.PaperWidth = 8.27F;
			this.PrintWidth = 7.0625F;
			this.Sections.Add(this.reportHeader1);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.reportFooter1);
			this.ShowParameterUI = false;
			((System.ComponentModel.ISupportInitialize)(this.txtName1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();

		}
		#endregion

		private void detail_Format(object sender, System.EventArgs e)
		{
		
		}
	}
}
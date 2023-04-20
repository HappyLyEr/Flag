using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DataDynamics.ActiveReports;
using DataDynamics.ActiveReports.Document;

namespace ActiveReportBuilder
{
	/// <summary>
	/// Summary description for SOPList.
	/// </summary>
	public class SOPList : DataDynamics.ActiveReports.ActiveReport3
	{
		private DataDynamics.ActiveReports.Detail detail;
		private GASystem.DataModel.View.ReportView reportView1;
		private DataDynamics.ActiveReports.TextBox txtDateOfInspection1;
		private DataDynamics.ActiveReports.Label label1;
		private DataDynamics.ActiveReports.TextBox txtSafetyObservationReferenceId1;
		private DataDynamics.ActiveReports.Label label2;
		private DataDynamics.ActiveReports.Label label3;
		private DataDynamics.ActiveReports.TextBox txtSOPTypeOfObservationListsRowId1;
		private DataDynamics.ActiveReports.Label label4;
		private DataDynamics.ActiveReports.TextBox txtActivityObserved1;
		private DataDynamics.ActiveReports.Label label5;
		private DataDynamics.ActiveReports.TextBox txtSuggestionForImprovement1;
		private DataDynamics.ActiveReports.Label label6;
		private DataDynamics.ActiveReports.TextBox txtObserver1;
		private DataDynamics.ActiveReports.Label label7;
		private DataDynamics.ActiveReports.TextBox txtNotGoodComments1;
		private DataDynamics.ActiveReports.Label label8;
		private DataDynamics.ActiveReports.TextBox txtTextFree11;
		private DataDynamics.ActiveReports.TextBox txtIntFree31;
		private DataDynamics.ActiveReports.Label label9;
		private DataDynamics.ActiveReports.Field CalcResp;
		private DataDynamics.ActiveReports.Label label12;
		private DataDynamics.ActiveReports.TextBox txtCalcResp1;
		private DataDynamics.ActiveReports.ReportHeader reportHeader1;
		private DataDynamics.ActiveReports.ReportFooter reportFooter1;

		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public SOPList()
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
			System.Resources.ResourceManager resources = new System.Resources.ResourceManager(typeof(SOPList));
			this.label1 = new DataDynamics.ActiveReports.Label();
			this.label2 = new DataDynamics.ActiveReports.Label();
			this.label3 = new DataDynamics.ActiveReports.Label();
			this.label4 = new DataDynamics.ActiveReports.Label();
			this.label5 = new DataDynamics.ActiveReports.Label();
			this.label6 = new DataDynamics.ActiveReports.Label();
			this.label7 = new DataDynamics.ActiveReports.Label();
			this.label8 = new DataDynamics.ActiveReports.Label();
			this.label9 = new DataDynamics.ActiveReports.Label();
			this.label12 = new DataDynamics.ActiveReports.Label();
			this.detail = new DataDynamics.ActiveReports.Detail();
			this.txtDateOfInspection1 = new DataDynamics.ActiveReports.TextBox();
			this.txtSafetyObservationReferenceId1 = new DataDynamics.ActiveReports.TextBox();
			this.txtSOPTypeOfObservationListsRowId1 = new DataDynamics.ActiveReports.TextBox();
			this.txtActivityObserved1 = new DataDynamics.ActiveReports.TextBox();
			this.txtSuggestionForImprovement1 = new DataDynamics.ActiveReports.TextBox();
			this.txtObserver1 = new DataDynamics.ActiveReports.TextBox();
			this.txtNotGoodComments1 = new DataDynamics.ActiveReports.TextBox();
			this.txtTextFree11 = new DataDynamics.ActiveReports.TextBox();
			this.txtIntFree31 = new DataDynamics.ActiveReports.TextBox();
			this.txtCalcResp1 = new DataDynamics.ActiveReports.TextBox();
			this.reportView1 = new GASystem.DataModel.View.ReportView();
			this.CalcResp = new DataDynamics.ActiveReports.Field();
			this.reportHeader1 = new DataDynamics.ActiveReports.ReportHeader();
			this.reportFooter1 = new DataDynamics.ActiveReports.ReportFooter();
			((System.ComponentModel.ISupportInitialize)(this.label1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label5)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label7)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label8)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label9)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.label12)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDateOfInspection1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtSafetyObservationReferenceId1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtSOPTypeOfObservationListsRowId1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtActivityObserved1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtSuggestionForImprovement1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtObserver1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtNotGoodComments1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtTextFree11)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtIntFree31)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.txtCalcResp1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).BeginInit();
			// 
			// label1
			// 
			this.label1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label1.HyperLink = null;
			this.label1.Location = ((System.Drawing.PointF)(resources.GetObject("label1.Location")));
			this.label1.Name = "label1";
			this.label1.Size = ((System.Drawing.SizeF)(resources.GetObject("label1.Size")));
			this.label1.Text = "Date";
			// 
			// label2
			// 
			this.label2.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label2.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label2.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label2.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label2.HyperLink = null;
			this.label2.Location = ((System.Drawing.PointF)(resources.GetObject("label2.Location")));
			this.label2.Name = "label2";
			this.label2.Size = ((System.Drawing.SizeF)(resources.GetObject("label2.Size")));
			this.label2.Text = "Reference Id";
			// 
			// label3
			// 
			this.label3.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label3.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label3.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label3.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label3.HyperLink = null;
			this.label3.Location = ((System.Drawing.PointF)(resources.GetObject("label3.Location")));
			this.label3.Name = "label3";
			this.label3.Size = ((System.Drawing.SizeF)(resources.GetObject("label3.Size")));
			this.label3.Text = "Type of Observation";
			// 
			// label4
			// 
			this.label4.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label4.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label4.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label4.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label4.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label4.HyperLink = null;
			this.label4.Location = ((System.Drawing.PointF)(resources.GetObject("label4.Location")));
			this.label4.Name = "label4";
			this.label4.Size = ((System.Drawing.SizeF)(resources.GetObject("label4.Size")));
			this.label4.Text = "Activity Observed";
			// 
			// label5
			// 
			this.label5.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label5.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label5.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label5.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label5.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label5.HyperLink = null;
			this.label5.Location = ((System.Drawing.PointF)(resources.GetObject("label5.Location")));
			this.label5.Name = "label5";
			this.label5.Size = ((System.Drawing.SizeF)(resources.GetObject("label5.Size")));
			this.label5.Text = "Suggestions for Improvement";
			// 
			// label6
			// 
			this.label6.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label6.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label6.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label6.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label6.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label6.HyperLink = null;
			this.label6.Location = ((System.Drawing.PointF)(resources.GetObject("label6.Location")));
			this.label6.Name = "label6";
			this.label6.Size = ((System.Drawing.SizeF)(resources.GetObject("label6.Size")));
			this.label6.Text = "Observer";
			// 
			// label7
			// 
			this.label7.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label7.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label7.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label7.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label7.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label7.HyperLink = null;
			this.label7.Location = ((System.Drawing.PointF)(resources.GetObject("label7.Location")));
			this.label7.Name = "label7";
			this.label7.Size = ((System.Drawing.SizeF)(resources.GetObject("label7.Size")));
			this.label7.Text = "NonConformity Description";
			// 
			// label8
			// 
			this.label8.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label8.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label8.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label8.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label8.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label8.HyperLink = null;
			this.label8.Location = ((System.Drawing.PointF)(resources.GetObject("label8.Location")));
			this.label8.Name = "label8";
			this.label8.Size = ((System.Drawing.SizeF)(resources.GetObject("label8.Size")));
			this.label8.Text = "SOP Response";
			// 
			// label9
			// 
			this.label9.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label9.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label9.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label9.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label9.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label9.HyperLink = null;
			this.label9.Location = ((System.Drawing.PointF)(resources.GetObject("label9.Location")));
			this.label9.Name = "label9";
			this.label9.Size = ((System.Drawing.SizeF)(resources.GetObject("label9.Size")));
			this.label9.Text = "Status";
			// 
			// label12
			// 
			this.label12.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label12.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label12.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label12.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.label12.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
			this.label12.HyperLink = null;
			this.label12.Location = ((System.Drawing.PointF)(resources.GetObject("label12.Location")));
			this.label12.Name = "label12";
			this.label12.Size = ((System.Drawing.SizeF)(resources.GetObject("label12.Size")));
			this.label12.Text = "Responsible";
			// 
			// detail
			// 
			this.detail.ColumnSpacing = 0F;
			this.detail.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																						 this.txtDateOfInspection1,
																						 this.txtSafetyObservationReferenceId1,
																						 this.txtSOPTypeOfObservationListsRowId1,
																						 this.txtActivityObserved1,
																						 this.txtSuggestionForImprovement1,
																						 this.txtObserver1,
																						 this.txtNotGoodComments1,
																						 this.txtTextFree11,
																						 this.txtIntFree31,
																						 this.txtCalcResp1});
			this.detail.Height = 0.3854167F;
			this.detail.KeepTogether = true;
			this.detail.Name = "detail";
			// 
			// txtDateOfInspection1
			// 
			this.txtDateOfInspection1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtDateOfInspection1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtDateOfInspection1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtDateOfInspection1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtDateOfInspection1.DataField = "DateOfInspection";
			this.txtDateOfInspection1.DistinctField = null;
			this.txtDateOfInspection1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtDateOfInspection1.Location = ((System.Drawing.PointF)(resources.GetObject("txtDateOfInspection1.Location")));
			this.txtDateOfInspection1.Name = "txtDateOfInspection1";
			this.txtDateOfInspection1.OutputFormat = "dd-MMM-yy";
			this.txtDateOfInspection1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtDateOfInspection1.Size")));
			this.txtDateOfInspection1.Text = "txtDateOfInspection1";
			// 
			// txtSafetyObservationReferenceId1
			// 
			this.txtSafetyObservationReferenceId1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtSafetyObservationReferenceId1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtSafetyObservationReferenceId1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtSafetyObservationReferenceId1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtSafetyObservationReferenceId1.DataField = "SafetyObservationReferenceId";
			this.txtSafetyObservationReferenceId1.DistinctField = null;
			this.txtSafetyObservationReferenceId1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtSafetyObservationReferenceId1.Location = ((System.Drawing.PointF)(resources.GetObject("txtSafetyObservationReferenceId1.Location")));
			this.txtSafetyObservationReferenceId1.Name = "txtSafetyObservationReferenceId1";
			this.txtSafetyObservationReferenceId1.OutputFormat = null;
			this.txtSafetyObservationReferenceId1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtSafetyObservationReferenceId1.Size")));
			this.txtSafetyObservationReferenceId1.Text = "txtSafetyObservationReferenceId1";
			// 
			// txtSOPTypeOfObservationListsRowId1
			// 
			this.txtSOPTypeOfObservationListsRowId1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtSOPTypeOfObservationListsRowId1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtSOPTypeOfObservationListsRowId1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtSOPTypeOfObservationListsRowId1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtSOPTypeOfObservationListsRowId1.DataField = "SOPTypeOfObservationListsRowId";
			this.txtSOPTypeOfObservationListsRowId1.DistinctField = null;
			this.txtSOPTypeOfObservationListsRowId1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtSOPTypeOfObservationListsRowId1.Location = ((System.Drawing.PointF)(resources.GetObject("txtSOPTypeOfObservationListsRowId1.Location")));
			this.txtSOPTypeOfObservationListsRowId1.Name = "txtSOPTypeOfObservationListsRowId1";
			this.txtSOPTypeOfObservationListsRowId1.OutputFormat = null;
			this.txtSOPTypeOfObservationListsRowId1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtSOPTypeOfObservationListsRowId1.Size")));
			this.txtSOPTypeOfObservationListsRowId1.Text = "txtSOPTypeOfObservationListsRowId1";
			// 
			// txtActivityObserved1
			// 
			this.txtActivityObserved1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtActivityObserved1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtActivityObserved1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtActivityObserved1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtActivityObserved1.DataField = "ActivityObserved";
			this.txtActivityObserved1.DistinctField = null;
			this.txtActivityObserved1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtActivityObserved1.Location = ((System.Drawing.PointF)(resources.GetObject("txtActivityObserved1.Location")));
			this.txtActivityObserved1.Name = "txtActivityObserved1";
			this.txtActivityObserved1.OutputFormat = null;
			this.txtActivityObserved1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtActivityObserved1.Size")));
			this.txtActivityObserved1.Text = "txtActivityObserved1";
			// 
			// txtSuggestionForImprovement1
			// 
			this.txtSuggestionForImprovement1.BackColor = System.Drawing.Color.LimeGreen;
			this.txtSuggestionForImprovement1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid;
			this.txtSuggestionForImprovement1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid;
			this.txtSuggestionForImprovement1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid;
			this.txtSuggestionForImprovement1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.Solid;
			this.txtSuggestionForImprovement1.DataField = "SuggestionForImprovement";
			this.txtSuggestionForImprovement1.DistinctField = null;
			this.txtSuggestionForImprovement1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtSuggestionForImprovement1.Location = ((System.Drawing.PointF)(resources.GetObject("txtSuggestionForImprovement1.Location")));
			this.txtSuggestionForImprovement1.Name = "txtSuggestionForImprovement1";
			this.txtSuggestionForImprovement1.OutputFormat = null;
			this.txtSuggestionForImprovement1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtSuggestionForImprovement1.Size")));
			this.txtSuggestionForImprovement1.Text = "txtSuggestionForImprovement1";
			// 
			// txtObserver1
			// 
			this.txtObserver1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtObserver1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtObserver1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtObserver1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtObserver1.DataField = "Observer";
			this.txtObserver1.DistinctField = null;
			this.txtObserver1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtObserver1.Location = ((System.Drawing.PointF)(resources.GetObject("txtObserver1.Location")));
			this.txtObserver1.Name = "txtObserver1";
			this.txtObserver1.OutputFormat = null;
			this.txtObserver1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtObserver1.Size")));
			this.txtObserver1.Text = "txtObserver1";
			// 
			// txtNotGoodComments1
			// 
			this.txtNotGoodComments1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtNotGoodComments1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtNotGoodComments1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtNotGoodComments1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtNotGoodComments1.DataField = "NotGoodComments";
			this.txtNotGoodComments1.DistinctField = null;
			this.txtNotGoodComments1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtNotGoodComments1.Location = ((System.Drawing.PointF)(resources.GetObject("txtNotGoodComments1.Location")));
			this.txtNotGoodComments1.Name = "txtNotGoodComments1";
			this.txtNotGoodComments1.OutputFormat = null;
			this.txtNotGoodComments1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtNotGoodComments1.Size")));
			this.txtNotGoodComments1.Text = "txtNotGoodComments1";
			// 
			// txtTextFree11
			// 
			this.txtTextFree11.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtTextFree11.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtTextFree11.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtTextFree11.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtTextFree11.DataField = "TextFree1";
			this.txtTextFree11.DistinctField = null;
			this.txtTextFree11.Font = new System.Drawing.Font("Arial", 10F);
			this.txtTextFree11.Location = ((System.Drawing.PointF)(resources.GetObject("txtTextFree11.Location")));
			this.txtTextFree11.Name = "txtTextFree11";
			this.txtTextFree11.OutputFormat = null;
			this.txtTextFree11.Size = ((System.Drawing.SizeF)(resources.GetObject("txtTextFree11.Size")));
			this.txtTextFree11.Text = "txtTextFree11";
			// 
			// txtIntFree31
			// 
			this.txtIntFree31.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtIntFree31.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtIntFree31.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtIntFree31.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtIntFree31.DataField = "IntFree3";
			this.txtIntFree31.DistinctField = null;
			this.txtIntFree31.Font = new System.Drawing.Font("Arial", 10F);
			this.txtIntFree31.Location = ((System.Drawing.PointF)(resources.GetObject("txtIntFree31.Location")));
			this.txtIntFree31.Name = "txtIntFree31";
			this.txtIntFree31.OutputFormat = null;
			this.txtIntFree31.Size = ((System.Drawing.SizeF)(resources.GetObject("txtIntFree31.Size")));
			this.txtIntFree31.Text = "txtIntFree31";
			// 
			// txtCalcResp1
			// 
			this.txtCalcResp1.Border.BottomStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtCalcResp1.Border.LeftStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtCalcResp1.Border.RightStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtCalcResp1.Border.TopStyle = DataDynamics.ActiveReports.BorderLineStyle.None;
			this.txtCalcResp1.DataField = "CalcResp";
			this.txtCalcResp1.DistinctField = null;
			this.txtCalcResp1.Font = new System.Drawing.Font("Arial", 10F);
			this.txtCalcResp1.Location = ((System.Drawing.PointF)(resources.GetObject("txtCalcResp1.Location")));
			this.txtCalcResp1.Name = "txtCalcResp1";
			this.txtCalcResp1.OutputFormat = null;
			this.txtCalcResp1.Size = ((System.Drawing.SizeF)(resources.GetObject("txtCalcResp1.Size")));
			this.txtCalcResp1.Text = "txtCalcResp1";
			// 
			// reportView1
			// 
			this.reportView1.DataSetName = "ReportView";
			this.reportView1.Locale = new System.Globalization.CultureInfo("en-US");
			// 
			// CalcResp
			// 
			this.CalcResp.DefaultValue = "";
			this.CalcResp.FieldType = DataDynamics.ActiveReports.FieldTypeEnum.String;
			this.CalcResp.Formula = "(IntFree1 == \"\" && IntFree2 ==\"\") ? \"Party Chief\"  :  IntFree1+IntFree2";
			this.CalcResp.Name = "CalcResp";
			this.CalcResp.Tag = null;
			// 
			// reportHeader1
			// 
			this.reportHeader1.Controls.AddRange(new DataDynamics.ActiveReports.ARControl[] {
																								this.label1,
																								this.label7,
																								this.label2,
																								this.label3,
																								this.label12,
																								this.label9,
																								this.label8,
																								this.label4,
																								this.label6,
																								this.label5});
			this.reportHeader1.Height = 0.5208333F;
			this.reportHeader1.Name = "reportHeader1";
			// 
			// reportFooter1
			// 
			this.reportFooter1.Height = 0.25F;
			this.reportFooter1.Name = "reportFooter1";
			// 
			// SOPList
			// 
			this.CalculatedFields.Add(this.CalcResp);
			this.DataMember = "GASafetyObservation";
			this.DataSource = this.reportView1;
			this.PageSettings.PaperHeight = 11F;
			this.PageSettings.PaperWidth = 8.5F;
			this.PrintWidth = 10.85417F;
			this.Script = @"//public void detail_Format()
//{
//   if ((rpt.Fields[""IntFree3""].Value.ToString().ToLower().IndexOf(""clos"") > -1) || (rpt.Fields[""IntFree3""].Value.ToString().ToLower().IndexOf(""reje"") > -1))
//    {
//        rpt.LayoutAction = LayoutAction.NextRecord;

//    }
//}";
			this.Sections.Add(this.reportHeader1);
			this.Sections.Add(this.detail);
			this.Sections.Add(this.reportFooter1);
			this.ShowParameterUI = false;
			((System.ComponentModel.ISupportInitialize)(this.label1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label4)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label5)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label6)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label7)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label8)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label9)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.label12)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtDateOfInspection1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtSafetyObservationReferenceId1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtSOPTypeOfObservationListsRowId1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtActivityObserved1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtSuggestionForImprovement1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtObserver1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtNotGoodComments1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtTextFree11)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtIntFree31)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.txtCalcResp1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.reportView1)).EndInit();

		}
		#endregion

		private void pageHeader_Format(object sender, System.EventArgs e)
		{
		
		}
	}
}
using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Web;
using System.Web.SessionState;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GASystem;

namespace GASystem.WebForms
{
	/// <summary>
	/// Summary description for HelpDialog.
	/// </summary>
	public class HelpDialog : System.Web.UI.Page
	{
		protected GASystem.ViewDataRecord MyViewDataRecord;
		protected ListDataRecords ListFileDataRecords;
		


		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			if (!IsPostBack)
				SetupViewForm();
			

		}

		private GASystem.DataModel.GADataClass HelpClass 
		{
			get 
			{
				try 
				{
					return GASystem.DataModel.GADataRecord.ParseGADataClass(Request["helpclass"].ToString());
				}
				catch 
				{
					return GASystem.DataModel.GADataClass.GAHelp;
				}
			}
		}

		private void SetupViewForm() 
		{
			GASystem.DataModel.HelpDS ds = GASystem.BusinessLayer.Help.GetHelpByClass(HelpClass);
			MyViewDataRecord.RecordDataSet = ds;
			MyViewDataRecord.DataClass = GASystem.DataModel.GADataClass.GAHelp.ToString();
            // Tor 20140320 Added ownerclass. Parameter required to setup form correctly. Field GAFieldDefinitions.HideIfOwnerClass (TextFree3)
            MyViewDataRecord.SetupForm("tor");
			ListFileDataRecords.RecordsDataSet = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(DataModel.GADataClass.GAFile, new GASystem.DataModel.GADataRecord(ds.GAHelp[0].HelpRowId, GASystem.DataModel.GADataClass.GAHelp));
			ListFileDataRecords.DataClass = DataModel.GADataClass.GAFile.ToString();
			ListFileDataRecords.RefreshGrid();
			ListFileDataRecords.DisplayEditButton = false;
			ListFileDataRecords.DisplayNewButton = false;
			ListFileDataRecords.DisplaySelectButton = false;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			ListFileDataRecords.SelectRecordClicked += new GASystem.GAGUIEvents.GACommandEventHandler(ListFileDataRecords_SelectRecordClicked);
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ListFileDataRecords_SelectRecordClicked(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			Response.Redirect("document.aspx?filerowid=" + e.CommandIntArgument + "&dataclass=gafile");
		}
	}
}

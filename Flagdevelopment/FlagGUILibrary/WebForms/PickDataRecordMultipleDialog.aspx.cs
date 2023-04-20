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
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem.AppUtils;

namespace GASystem
{
	/// <summary>
	/// Summary description for PickDepartment.
	/// </summary>
	public class PickDataRecordMultipleDialog : System.Web.UI.Page
	{
		protected GASystem.PickDataRecordMultiple PickDataRecordControl;
		protected GASystem.UserMessage UserMessageControl;
		private string dataClass = string.Empty;

		private string ownerclass = string.Empty;
		protected System.Web.UI.WebControls.Label lblFilter;     //jof 200506, is now used to hold the class of the form calling this lookupform
		private string ownerField = string.Empty;	  //jof 200506, used to hold the field id of the control calling this lookup
		private string filterDescription = string.Empty;
	

		private void Page_Load(object sender, System.EventArgs e)
		{
			String displayName = "";

			//displaycolumn
			//dataclass
			if (null!=Request["DataClass"])
				dataClass = Request["DataClass"];
			if (null!=Request["DisplayName"])
				displayName = Request["DisplayName"];

			
			if (null!=Request["ownerclass"])			//class of the calling control
				ownerclass = Request["ownerclass"].ToString();
			if (null!=Request["ownerField"])			//field of the calling control
				ownerField = Request["ownerField"].ToString();

			if (displayName.Length==0 || dataClass.Length==0)
			{
				UserMessageControl.MessageText = Localization.GetErrorText("ParameterMissing");
				lblFilter.Visible = false;
			}

			else //if (!Page.IsPostBack)
			{
				PickDataRecordControl.RecordsDataSet = GetDataSet(dataClass);
				PickDataRecordControl.DisplayColumnName = displayName;
				PickDataRecordControl.DataClass = dataClass;

				lblFilter.Text = filterDescription;

				if (!IsPostBack) 
				{
					DataClass = dataClass;
				} 
			}
		}

		private string DataClass 
		{
			get
			{
				return (string) ViewState["DataClass1"] ;
			}
			set
			{
				ViewState["DataClass1"] = value;
			}
		}


		/// <summary>
		/// Get data for the lookup table based on the dataclass, owner en filter definition
		/// This method is similar for single and multipicker. TODO move to business layer
		/// </summary>
		/// <param name="DataClass"></param>
		/// <returns></returns>
		private DataSet GetDataSet(String DataClass)
		{
			GADataClass gaDataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(DataClass);

		
			//get owner and filter
			AppUtils.LookupFilterGenerator.ILookupFilter lookupFilter;
			if (ownerclass != string.Empty && ownerField != string.Empty) 
			{
				lookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(GADataRecord.ParseGADataClass(ownerclass), ownerField, SessionManagement.GetCurrentDataContext().SubContextRecord);
			} 
			else 
			{
				lookupFilter = new AppUtils.LookupFilterGenerator.GeneralLookupFilter();
			}
			filterDescription = lookupFilter.FilterDescription;

		
			//get data
			//TODO replace all of this with a new method in the bussinesslayer

			ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(DataClass);
			DataSet ds;
			if (cd.IsTop) 
			{
				ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(gaDataClass, null, lookupFilter.Filter);

			} 
			else 
			{
			
				ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(gaDataClass, lookupFilter.Owner, lookupFilter.Filter);
				//if there are no rows returned, check for owner of owner
				//TODO, should we check recursivly all the way to the top??
				if (ds.Tables[gaDataClass.ToString()].Rows.Count == 0) 
				{
					GADataRecord ownerOfOwner = GASystem.BusinessLayer.DataClassRelations.GetOwner(lookupFilter.Owner);
					if (ownerOfOwner != null)
						ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(gaDataClass, ownerOfOwner);
				}

			}

			return ds;
		}


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
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

	

		private int GetDefaultFolderId() 
		{
			string defaultFolderName = System.Configuration.ConfigurationSettings.AppSettings.Get("DefaultFileFolder");
			if (!GASystem.AppUtils.GAUtils.IsNumeric(defaultFolderName)) 
			{
				throw new Exception("No default folder defined");
			} 
			int defaultFolder = int.Parse(defaultFolderName);
			return 	defaultFolder;
		}

		
	}
}

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
	public class PickDataRecordDialog : System.Web.UI.Page
	{
		protected GASystem.PickDataRecord PickDataRecordControl;
		protected GASystem.EditDataRecord MyEditDataRecord;
		protected System.Web.UI.WebControls.LinkButton LinkButton1;
		protected GASystem.UserMessage UserMessageControl;
		private string dataClass = string.Empty;
		private string ownerclass = string.Empty;     //jof 200506, is now used to hold the class of the form calling this lookupform
		private string ownerField = string.Empty;
		protected System.Web.UI.WebControls.Label lblFilter;
		protected System.Web.UI.WebControls.Button btnClearFilter;	  //jof 200506, used to hold the field id of the control calling this lookup
		private int ownerid = 0;
		private string displayName = "";
		private string keyName = string.Empty;
		private string filterDescription = string.Empty;
		private bool isListView = true;

		private void Page_Load(object sender, System.EventArgs e)
		{
			
			
			//displaycolumn
			//dataclass
			if (null!=Request["DataClass"])				//dataclass of the lookup window
				dataClass = Request["DataClass"];
			if (null!=Request["DisplayName"])			//column used for displaying selected data in the calling control
				displayName = Request["DisplayName"];
			if (null!=Request["KeyName"])				//column holding the data we want to save in the record of the calling control
				keyName = Request["KeyName"];

			if (null!=Request["ownerclass"])			//class of the calling control
				ownerclass = Request["ownerclass"].ToString();
			if (null!=Request["ownerField"])			//field of the calling control
				ownerField = Request["ownerField"].ToString();

//
//			if (null != Request["ownerid"]) 
//			{
//				string reqOwnerId = Request["ownerid"].ToString();
//				if (AppUtils.GAUtils.IsNumeric(reqOwnerId)) 
//					ownerid = int.Parse(reqOwnerId);
//			}

            if (!IsPostBack)
            {
                DataClass = dataClass;
            }

            GADataClass gaDataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(DataClass);
            MyEditDataRecord.RecordDataSet = BusinessLayer.Utils.RecordsetFactory.Make(gaDataClass).GetNewRecord(); // BusinessLayer.Personnel.GetNewPersonnel();
            MyEditDataRecord.DataClass = gaDataClass.ToString();//GASystem.DataModel.GADataClass.GAPersonnel.ToString();
            MyEditDataRecord.SetupForm();
			
				


			PickDataRecordControl.RecordsDataSet = GetDataSet(dataClass);
			PickDataRecordControl.DisplayColumnName = displayName;
			PickDataRecordControl.DataClass = dataClass;
			PickDataRecordControl.KeyName = keyName;
			

			
		}

		protected override void OnPreRender(EventArgs e)
		{
			if (displayName.Length==0 || dataClass.Length==0)
			{
				UserMessageControl.MessageText = Localization.GetErrorText("ParameterMissing");
				btnClearFilter.Visible = false;
				lblFilter.Visible = false;
			}

			else //if (!Page.IsPostBack)
			{
				PickDataRecordControl.RecordsDataSet = GetDataSet(dataClass);
				PickDataRecordControl.DisplayColumnName = displayName;
				PickDataRecordControl.DataClass = dataClass;
				PickDataRecordControl.KeyName = keyName;

				if (!IsPostBack) 
				{
					DataClass = dataClass;
					DoUserAction("LIST");
				} 

				//display filter links

				btnClearFilter.Text = GASystem.AppUtils.Localization.GetGuiElementText("ClearLookupFilter");
				if (IsDataFiltered) 
				{
					lblFilter.Text = filterDescription;
					btnClearFilter.Visible = CanToggleFilter && isListView;
				} 
				else 
				{
					lblFilter.Text = GASystem.AppUtils.Localization.GetGuiElementText("LookupFilterOff");
					btnClearFilter.Visible = CanToggleFilter && isListView;
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

		private bool IsDataFiltered 
		{
			get
			{
				return ViewState["IsDataFiltered"] == null ? false : (bool) ViewState["IsDataFiltered"] ;
			}
			set
			{
				ViewState["IsDataFiltered"] = value;
			}
		}

		
		private bool CanToggleFilter 
		{
			get
			{
				return ViewState["CanToggleFilter"] == null ? false : (bool) ViewState["CanToggleFilter"] ;
			}
			set
			{
				ViewState["CanToggleFilter"] = value;
			}
		}

		private void DoUserAction(string userAction)
		{
			if (userAction.ToUpper().Equals("LIST"))
			{
				isListView = true;
				PickDataRecordControl.Visible = true;
				lblFilter.Visible = true;
				btnClearFilter.Visible = true;
				
				MyEditDataRecord.Visible = false;
				if (dataClass == GADataClass.GAPersonnel.ToString()) 
				{
					LinkButton1.Visible = true;
					LinkButton1.Text = String.Format(Localization.GetGuiElementText("NewRecord"), Localization.GetGuiElementText(dataClass));
				}
				else
					LinkButton1.Visible = false;
			}
			if (userAction.ToUpper().Equals("NEW"))
			{
				isListView = false;
				PickDataRecordControl.Visible = false;
				lblFilter.Visible = false;
				btnClearFilter.Visible = false;
				
				MyEditDataRecord.Visible = true;
				LinkButton1.Visible = false;
			}
		}

		private DataSet GetDataSet(String DataClass)
		{
			GADataClass gaDataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(DataClass);

			
			//get owner and filter
			string dataFilter = string.Empty;
			
			if(!this.IsPostBack)   //initial setting for filtering of data;
				if (ownerField != string.Empty && ownerclass != string.Empty)      //these variables might be empty. eg. delegate on workitem
				{
					CanToggleFilter  = GASystem.AppUtils.FieldDefintion.GetFieldDescription(ownerField, ownerclass).hasLookupFilter();
					IsDataFiltered = CanToggleFilter;
				}
	
			AppUtils.LookupFilterGenerator.ILookupFilter lookupFilter;
			if (ownerclass != string.Empty && ownerField != string.Empty) 
			{
				lookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(GADataRecord.ParseGADataClass(ownerclass), ownerField, SessionManagement.GetCurrentDataContext().SubContextRecord);
			} 
			else 
			{
				lookupFilter = new AppUtils.LookupFilterGenerator.GeneralLookupFilter();
			}
			if (IsDataFiltered)
				dataFilter = lookupFilter.Filter;
			CanToggleFilter = lookupFilter.CanDisableFilter;
			filterDescription = lookupFilter.FilterDescription;
		
		
			//get data
			//TODO replace all of this with a new method in the bussinesslayer

			ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(DataClass);
			DataSet ds;
			if (cd.IsTop) 
			{
				ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(gaDataClass, null, dataFilter);

			} 
			else 
			{
			
				ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(gaDataClass, lookupFilter.Owner, dataFilter);
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
			this.MyEditDataRecord.EditRecordCancel += new GASystem.GAGUIEvents.GACommandEventHandler(MyEditDataRecord_EditRecordCancel);
			this.MyEditDataRecord.EditRecordSave += new GASystem.GAGUIEvents.GACommandEventHandler(MyEditDataRecord_EditRecordSave);
			this.LinkButton1.Click += new EventHandler(LinkButton1_Click);
			
			
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.btnClearFilter.Click += new System.EventHandler(this.btnClearFilter_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void LinkButton1_Click(object sender, System.EventArgs e)
		{
            //GADataClass gaDataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(DataClass);
            //MyEditDataRecord.RecordDataSet =  BusinessLayer.Utils.RecordsetFactory.Make(gaDataClass).GetNewRecord(); // BusinessLayer.Personnel.GetNewPersonnel();
            //MyEditDataRecord.DataClass	= gaDataClass.ToString();//GASystem.DataModel.GADataClass.GAPersonnel.ToString();
            //MyEditDataRecord.SetupForm();
			DoUserAction("NEW");
		}

		private void MyEditDataRecord_EditRecordCancel(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			DoUserAction("LIST");
		}

		private void MyEditDataRecord_EditRecordSave(object sender, GASystem.GAGUIEvents.GACommandEventArgs e)
		{
			//PersonnelDS PersonnelData = (PersonnelDS)e.CommandDataSetArgument;
			GADataClass gaDataClass = GASystem.DataModel.GADataRecord.ParseGADataClass(DataClass);
			
			BusinessLayer.BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(gaDataClass);

//			if (0 == PersonnelData.GAPersonnel[0].PersonnelRowId)
//				PersonnelData = BusinessLayer.Personnel.SaveNewPersonnel(PersonnelData, null);
//			else
//				PersonnelData = BusinessLayer.Personnel.UpdatePersonnel(PersonnelData);
//			
			//we are always saving a new record at this stage
			GADataRecord owner = null;
			if (gaDataClass == GADataClass.GAFile) 
			{
				owner = new GADataRecord(GetDefaultFolderId(), GADataClass.GAFileFolder);
			}

			DataSet ds = bc.SaveNew(e.CommandDataSetArgument, owner);
			//DataSet ds = bc.CommitDataSet(e.CommandDataSetArgument, owner);
			
			//reset the pickdatarecord controls dataset to the saved record, set the new record as selected.
			//PickDataRecordControl will then run javascript code for updating the parent form/browser
			PickDataRecordControl.RecordsDataSet = ds;

			GADataRecord dispatchToRecord = new GADataRecord((int)ds.Tables[0].Rows[0][bc.DataClass.ToString().Substring(2)+"RowId"], bc.DataClass);
		
			PickDataRecordControl.GenerateDataGrid();
			PickDataRecordControl.SelectRecord(dispatchToRecord.RowId.ToString()); //  bc.  PersonnelData.GAPersonnel[0].PersonnelRowId.ToString());
			DoUserAction("LIST");

		}
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

		private void btnClearFilter_Click(object sender, System.EventArgs e)
		{
			IsDataFiltered = !IsDataFiltered;
		}
	}
}

namespace GASystem
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.DataModel;
	using GASystem;
	using GASystem.AppUtils;
	using System.Collections;

	/// <summary>
	///		Summary description for PersonnelField.
	/// </summary>
	/// 
	[ValidationPropertyAttribute("TextValue")]
	public class RelatedDataRecordFieldMultiple : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox DisplayValueTextBox;
		protected System.Web.UI.HtmlControls.HtmlInputHidden KeyValueHidden;
		protected System.Web.UI.HtmlControls.HtmlImage CheckNameButton;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		private FieldDescription _fieldDescription;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			RequiredFieldValidator1.ErrorMessage = Localization.GetErrorText("FieldRequired");
		}


		

		//Provide a way to store the DisplayValue in viewState
		public String DisplayValue
		{
			get
			{
				return "".Equals(DisplayValueTextBox.Text) ? "" : DisplayValueTextBox.Text;
			}
			set
			{
				DisplayValueTextBox.Text = value;
			}
		}

		public String DisplayName
		{
			get
			{
				return null==ViewState["DisplayName"] ? null : (String) ViewState["DisplayName"];
			}
			set
			{
				ViewState["DisplayName"] = value;
			}
		}

		

		public GADataClass DataClass
		{
			get
			{
				return (GADataClass) ViewState["DataClass"] ;
			}
			set
			{
				ViewState["DataClass"] = value;
			}
		}

		public FieldDescription FieldDescriptionInfo 
		{
			get
			{
				return _fieldDescription;
			}
			set
			{
				_fieldDescription = value;
			}
		}

		public bool FieldRequired 
		{
			get {return RequiredFieldValidator1.Enabled;}
			set {RequiredFieldValidator1.Enabled = value;}
		}


		//Provide a way to store the dataset in viewState
		public int RowId
		{
			get
			{
				if (IsMultiple)
					return 0;
				else
					return "".Equals(KeyValueHidden.Value) ? 0 : int.Parse(KeyValueHidden.Value);
			}
			set
			{
				KeyValueHidden.Value = value.ToString();
			}
		}

		public bool IsMultiple
		{
			get
			{
				//if keyvalue contains comma (,) characters, multiple values are selected
				return KeyValueHidden.Value.IndexOf(",")>0;
			}
		}

		//
		public int[] RowIds
		{
			get
			{
				//return "".Equals(KeyValueHidden.Value) ? new int[0] : KeyValueHidden.Value.Split(new char[]{','});
				string[] rowIds =  KeyValueHidden.Value.Split(new char[]{','});
				ArrayList integerRowIds = new ArrayList();
				foreach (string stringRowId in rowIds) 
				{
					integerRowIds.Add(int.Parse(stringRowId));
				}
				return (int[]) integerRowIds.ToArray(typeof(int));
			}
			set
			{
				KeyValueHidden.Value = value.ToString();
			}
		}

		
		public string OwnerClass 
		{
			get
			{
				return ViewState["OwnerClass"] != null ? ViewState["OwnerClass"].ToString() : string.Empty ;
			}
			set
			{
				ViewState["OwnerClass"] = value;
			}
		}

		public string OwnerField 
		{
			get
			{
				return ViewState["OwnerField"] != null ? (string)ViewState["OwnerField"] : string.Empty;
			}
			set
			{
				ViewState["OwnerField"] = value;
			}
		}

		public string TextValue 
		{
			get {return KeyValueHidden.Value;}
		}

		public void GenerateControl() 
		{
			//PersonnelDS personnelData = new PersonnelDS();
			DataSet ds = null;
			DataClass = GADataRecord.ParseGADataClass(FieldDescriptionInfo.LookupTable);
			if (RowId > 0)
				ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsByDataRecord(new GADataRecord(RowId, DataClass));
				//personnelData = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelRowId(RowId);
			DisplayValue = string.Empty;
			if (null != ds && 0!=ds.Tables[0].Rows.Count) 
			{
				ArrayList DisplayColumns = ParseDisplayColumns(FieldDescriptionInfo.LookupTableDisplayValue);
				foreach (String columnName in DisplayColumns)
				{
					if (ds.Tables[0].Columns.Contains(columnName))
						DisplayValue = DisplayValue + ds.Tables[0].Rows[0][columnName].ToString() + " ";
				}
			}
				//DisplayValue = personnelData.GAPersonnel[0].GivenName + " " + personnelData.GAPersonnel[0].FamilyName;
			
			
			DisplayName = FieldDescriptionInfo.LookupTableDisplayValue;

			OwnerClass = FieldDescriptionInfo.TableId;
			OwnerField = FieldDescriptionInfo.FieldId;
		}

		private ArrayList ParseDisplayColumns(String ColumnName)
		{
			ArrayList columnList = new ArrayList();
			if (ColumnName.IndexOf(" ")>-1)
			{
				foreach (String columnName in ColumnName.Split(new char[] {' '}))
				{
					columnList.Add(columnName);
				}
			}
			else
			{
				columnList.Add(ColumnName);
			}
			return columnList;
		}

        protected override void OnPreRender(EventArgs e)
        {
            DisplayValueTextBox.ReadOnly = false;    //.net 2.0 workaround set to false to allow posted value to be assigned to textbox to make box readonly
		
            base.OnPreRender(e);
        }

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
            DisplayValueTextBox.ReadOnly = true;     //.net 2.0 workaround set to true to make box readonly
            
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}

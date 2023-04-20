using System;
using System.Collections;
using System.Web.UI;
using System.Data;
using System.Web.UI.WebControls;
using GASystem.AppUtils;
using GASystem.UserControls;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem.DataAccess;

namespace GASystem.WebControls.EditControls
{
	/// <summary>
	/// Summary description for Responsible.
	/// </summary>
	public class Responsible  : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private RadioButton checkUser;
	    private RadioButton checkRole;
//		private RelatedDataRecordField users;
//		private DropDownList roles;
		private Panel userPH;
		private Panel rolePH;
        private Hashtable lookupTables = new Hashtable();

		//RelatedDataRecordField drf;
        Telerik.WebControls.RadComboBox drf;

		DropDownList ddl;
		
		
		
		
		public Responsible()
		{
			
		}
		
		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			//users = new RelatedDataRecordField();
			//roles = new DropDownList();
			generateCheckBoxs();
			
		}

		
		private void generateCheckBoxs()
		{
			checkUser = new RadioButton();
			checkRole = new RadioButton();
			checkUser.GroupName = "userrole";
			checkRole.GroupName = "userrole";
//			checkUser.ID = this.ID + "radiouser";
//			checkRole.ID = this.ID + "radiorole";
			checkUser.EnableViewState = true;
			checkRole.EnableViewState = true;
			checkUser.Checked = true;
			checkUser.Attributes.Add("onclick", "javasript:setuservisible();");
			checkRole.Attributes.Add("onclick", "javasript:setrolevisible();");
			checkUser.Text = Localization.GetGuiElementText("Person");
			// Tor 20130331 checkRole.Text = Localization.GetGuiElementText("Role");
            checkRole.Text = Localization.GetGuiElementText("Job Title");
            this.Controls.Add(checkUser);
			this.Controls.Add(checkRole);
			this.Controls.Add(GASystem.GUIUtils.HTMLLiteralTags.CreateBRTag());
			
			userPH = new Panel();
			this.Controls.Add(userPH);
			rolePH = new Panel();
			this.Controls.Add(rolePH);
			
			
			//add dropdown and relatedrecord picker
			ddl = new DropDownList();
			rolePH.Controls.Add(ddl);
			
			//drf = (RelatedDataRecordField) UserControlUtils.GetUserControl(UserControlType.RelatedDataRecordField, this.Page);



			//			drf.ID = c.ColumnName;
            drf = new Telerik.WebControls.RadComboBox();
            drf.Skin = "FlagCombo";

			userPH.Controls.Add(drf);
			
			
			
				
			
		}

        protected Control AddLookupField(DataColumn c)
        {
            FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
            string currentValueText = c.Table.Rows[0][c].ToString();
            return AddLookupField(fieldDesc, currentValueText);

            //     drf.ID = c.ColumnName;
            // placeHolder.Controls.Add(combo);

            //GADataClass dataClass = GADataRecord.ParseGADataClass(fieldDesc.LookupTable);



            //BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataRecord.ParseGADataClass(fieldDesc.LookupTable));

            //DataSet ds = bc.GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(new GADataRecord(1, GADataClass.GAFlag), System.DateTime.MinValue, System.DateTime.MaxValue, lookupFilter.Filter);

            //drf.AllowCustomText = false;

            //drf.MarkFirstMatch = true;
            //drf.Height = new Unit(300, UnitType.Pixel);
            //drf.Sort = Telerik.WebControls.RadComboBoxSort.Ascending;
            //string currentValueText = c.Table.Rows[0][c].ToString();
            //String[] displayTexts = fieldDesc.LookupTableDisplayValue.Trim().Split(' ');
            //foreach (DataRow row in ds.Tables[0].Rows)
            //{
            //    string displayText = string.Empty;
            //    foreach (string aValue in displayTexts)
            //    {
            //        displayText += row[aValue].ToString() + " ";
            //    }
            //    displayText = displayText.Trim();
            //    Telerik.WebControls.RadComboBoxItem item = new Telerik.WebControls.RadComboBoxItem(displayText, row[fieldDesc.LookupTableKey].ToString());
            //    drf.Items.Add(item);

            //}
            //drf.SelectedValue = currentValueText;

            //return (Control)drf;

        }

        // Tor 20140128 Added method overload to transfer current record (new or edit record) owner
        protected Control AddLookupField(DataColumn c, GASystem.DataModel.GADataRecord owner)
        {
            FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
            string currentValueText = c.Table.Rows[0][c].ToString();
            // Tor 20140128 added GADataRecord owner for lookupfilter to work properly
            //            return AddLookupField(fieldDesc, currentValueText);
            return AddLookupField(fieldDesc, currentValueText, owner);


        }


		/// <summary>
		/// Add person lookupfield control
		/// </summary>
		/// <param name="fieldDesc">Fielddescription</param>
		/// <returns></returns>
        public Control AddLookupField(FieldDescription fieldDesc, string currentValue)
        {
           // FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);


            drf.ID = fieldDesc.FieldId;
            // placeHolder.Controls.Add(combo);

            //TODO add correct lookupfilter - needs current record and current record owner record!!!!!!
            // Tor 20140127 Replace owner GAFlag.1 with current record ownet
            GADataClass myDataClass = GADataRecord.ParseGADataClass(fieldDesc.TableId);
            //new GADataRecord owner=DataClassRelations.GetOwnerDataRecord();

            AppUtils.LookupFilterGenerator.ILookupFilter lookupFilter;
//            if (Owner == null)
                lookupFilter = new AppUtils.LookupFilterGenerator.GeneralLookupFilter();
//            else
//                lookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(myDataClass, fieldDesc.LookupTableKey, Owner, fieldDesc.LookupFilter);
            
//            AppUtils.LookupFilterGenerator.ILookupFilter lookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(myDataClass, fieldDesc.FieldId, new GADataRecord(1, GADataClass.GAFlag), fieldDesc.LookupFilter);
//            AppUtils.LookupFilterGenerator.ILookupFilter lookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(myDataClass, fieldDesc.FieldId, new GADataRecord(DataClassRelations.GetOwnerDataRecord()), fieldDesc.LookupFilter);



            BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataRecord.ParseGADataClass(fieldDesc.LookupTable));

            DataSet ds = bc.GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(new GADataRecord(1, GADataClass.GAFlag), System.DateTime.MinValue, System.DateTime.MaxValue, lookupFilter.Filter);
           
            drf.AllowCustomText = false;

            drf.MarkFirstMatch = true;
            drf.Height = new Unit(300, UnitType.Pixel);
 //           drf.Sort = Telerik.WebControls.RadComboBoxSort.Ascending;
            string currentValueText = currentValue;
            String[] displayTexts = fieldDesc.LookupTableDisplayValue.Trim().Split(' ');

            ds.Tables[0].DefaultView.Sort = displayTexts[0] + " ASC";
            for (int t = 1; t < displayTexts.Length; t++)
                ds.Tables[0].DefaultView.Sort += ", " + displayTexts[t] + " ASC";

            drf.Items.Add(new Telerik.WebControls.RadComboBoxItem(string.Empty, string.Empty));
            
            foreach (DataRowView row in ds.Tables[0].DefaultView)
            {
                string displayText = string.Empty;
                foreach (string aValue in displayTexts)
                {
                    displayText += row[aValue].ToString() + " ";
                }
                displayText = displayText.Trim();
                Telerik.WebControls.RadComboBoxItem item = new Telerik.WebControls.RadComboBoxItem(displayText, row[fieldDesc.LookupTableKey].ToString());
                drf.Items.Add(item);

            }
            drf.SelectedValue = currentValueText;

            return (Control)drf;
            //drf.FieldDescriptionInfo = fieldDesc;
            //drf.FieldRequired = fieldDesc.RequiredField;

            //drf.KeyValue = keyValue;                 //c.Table.Rows[0][c].ToString();
            //drf.GenerateControl();
            //return (Control) drf;
        }

        // Tor 20140128 Added method overload to transfer current record (new or edit record) owner
        public Control AddLookupField(FieldDescription fieldDesc, string currentValue, GASystem.DataModel.GADataRecord owner)
        {
            drf.ID = fieldDesc.FieldId;
            // Tor 20140127 Replace owner GAFlag.1 with current record ownet
            GADataClass myDataClass = GADataRecord.ParseGADataClass(fieldDesc.TableId);

            // AppUtils.LookupFilterGenerator.ILookupFilter lookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(myDataClass, fieldDesc.FieldId, new GADataRecord(1, GADataClass.GAFlag), fieldDesc.LookupFilter);
            // AppUtils.LookupFilterGenerator.ILookupFilter lookupefilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(myDataClass, fieldDesc.FieldId, owner, fieldDesc.LookupFilter);

            // Tor 20130128 commented: BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataRecord.ParseGADataClass(fieldDesc.LookupTable));

            // Tor 20140128
            //            DataSet ds = bc.GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(new GADataRecord(1, GADataClass.GAFlag), System.DateTime.MinValue, System.DateTime.MaxValue, lookupFilter.Filter);
            // DataSet ds = bc.GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(owner, System.DateTime.MinValue, System.DateTime.MaxValue, fieldDesc.LookupFilter);
            // Tor 20140129 Replace last commented statement above with copies from EditDataRecord.ascx.cs from line 1533
            AppUtils.LookupFilterGenerator.ILookupFilter lookupFilter;
            if (owner == null) //modified from Owner to owner, 
                lookupFilter = new AppUtils.LookupFilterGenerator.GeneralLookupFilter();
            else
                // lookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(GADataRecord.ParseGADataClass(this.DataClass), fieldDesc.LookupTableKey, owner, fieldDesc.LookupFilter);
                lookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(GASystem.DataModel.GADataRecord.ParseGADataClass(fieldDesc.TableId), fieldDesc.LookupTableKey, owner, fieldDesc.LookupFilter);


            DataSet ds;
            if (this.lookupTables.ContainsKey(fieldDesc.LookupTable))
                ds = (DataSet)lookupTables[fieldDesc.LookupTable];
            else
            {
//                ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetForDataClassWithinOwner(GADataRecord.ParseGADataClass(fieldDesc.LookupTable), new GADataRecord(1, GADataClass.GAFlag), lookupFilter.Filter, System.DateTime.MinValue, System.DateTime.MaxValue);
                ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetForDataClassWithinOwner(GADataRecord.ParseGADataClass(fieldDesc.LookupTable), new GADataRecord(1, GADataClass.GAFlag), lookupFilter.Filter, System.DateTime.MinValue, System.DateTime.MaxValue);
                lookupTables.Add(fieldDesc.LookupTable, ds);
            }
            // Tor 20140129 end 

            drf.AllowCustomText = false;

            drf.MarkFirstMatch = true;
            drf.Height = new Unit(300, UnitType.Pixel);
            //           drf.Sort = Telerik.WebControls.RadComboBoxSort.Ascending;
            string currentValueText = currentValue;
            String[] displayTexts = fieldDesc.LookupTableDisplayValue.Trim().Split(' ');

            ds.Tables[0].DefaultView.Sort = displayTexts[0] + " ASC";
            for (int t = 1; t < displayTexts.Length; t++)
                ds.Tables[0].DefaultView.Sort += ", " + displayTexts[t] + " ASC";

            drf.Items.Add(new Telerik.WebControls.RadComboBoxItem(string.Empty, string.Empty));

            foreach (DataRowView row in ds.Tables[0].DefaultView)
            {
                string displayText = string.Empty;
                foreach (string aValue in displayTexts)
                {
                    displayText += row[aValue].ToString() + " ";
                }
                displayText = displayText.Trim();
                Telerik.WebControls.RadComboBoxItem item = new Telerik.WebControls.RadComboBoxItem(displayText, row[fieldDesc.LookupTableKey].ToString());
                drf.Items.Add(item);

            }
            drf.SelectedValue = currentValueText;

            return (Control)drf;
        }


		/// <summary>
		/// Add and load role drop down control. Method used by SetResponsibles method. Method is also setting current value
		/// based on data from ColumnInfo passed from SetResponsible.
		/// </summary>
		/// <param name="c"></param>
		protected void AddRoleDropDown(DataColumn c)
		{
			FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
			//ddl = new DropDownList();					
			
			AddRoleDropDown(fieldDesc);

			if (DBNull.Value == c.Table.Rows[0][c]) 
			{
				ddl.Items.Insert(0, new ListItem(string.Empty, "0"));  
				//no current value, insert "null value" at start of list
			}
			if (DBNull.Value != c.Table.Rows[0][c])
			{
				ListItem tmpItem = ddl.Items.FindByValue(c.Table.Rows[0][c].ToString());
				if (null!=tmpItem) 
					tmpItem.Selected=true;

			} 
			//rolePH.Controls.Add(ddl);
		}

		/// <summary>
		/// Add and load role dropdown control. 
		/// </summary>
		/// <param name="fieldDesc"></param>
		public void AddRoleDropDown(FieldDescription fieldDesc) 
		{
			ddl.CssClass = fieldDesc.CssClass;
			
			ArrayList dropDownValues = CodeTables.GetList(fieldDesc.ListCategory, GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord);
			
			CodeTables.BindCodeTable(ddl, dropDownValues);

		}
		
	
		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			
			//setuservisible
			string jscode = "<script language=\"javascript\">\n";
			jscode += "function setuservisible() {\n";
			jscode +=  "var rescontrol = document.getElementById('" + this.ClientID +  "');\n";
			jscode += "rescontrol.lastChild.style.display  = 'none'; ";
			jscode += "rescontrol.lastChild.previousSibling.style.display  = 'block'; ";
			//visible

			jscode += "}\n </script>";
			this.Page.ClientScript.RegisterStartupScript(typeof(Responsible), "respuservisible" + this.ID, jscode);
			
			
			
			//setrolevisible
			jscode = "<script language=\"javascript\">\n";
			jscode += "function setrolevisible() {\n";
			jscode +=  "var rescontrol = document.getElementById('" + this.ClientID +  "');\n";
			jscode += "rescontrol.lastChild.style.display  = 'block'; ";
			jscode += "rescontrol.lastChild.previousSibling.style.display  = 'none'; ";
			//visible

			jscode += "}\n </script>";
            this.Page.ClientScript.RegisterStartupScript(typeof(Responsible), "setrolevisible" + this.ID, jscode);
			
			if (checkUser.Checked)
			{
				rolePH.Style.Add("display", "none");
				userPH.Style.Add("display", "block");
			}
			if (checkRole.Checked)
			{
				userPH.Style.Add("display", "none");
				rolePH.Style.Add("display", "block");
			}
			
		}

		
		
        // Tor 20140128 Added method overload to transfer current record (new or edit record) owner
        public void SetResponsibles(DataColumn UserColumn, DataColumn RoleColumn, GASystem.DataModel.GADataRecord owner)
        {
            AddLookupField(UserColumn,owner);
            AddRoleDropDown(RoleColumn);
            //set element to show at startup
            if (UserColumn.Table.Rows[0][UserColumn] != DBNull.Value)
            {
                checkUser.Checked = true;
                checkRole.Checked = false;
            }

            if (RoleColumn.Table.Rows[0][RoleColumn] != DBNull.Value)
            {
                checkUser.Checked = false;
                checkRole.Checked = true;
            }
        }

        public void SetResponsibles(DataColumn UserColumn, DataColumn RoleColumn)
		{
			AddLookupField(UserColumn);
			AddRoleDropDown(RoleColumn);
			//set element to show at startup
			if (UserColumn.Table.Rows[0][UserColumn] != DBNull.Value)
			{
				checkUser.Checked = true;
				checkRole.Checked = false;
			}
			
			if (RoleColumn.Table.Rows[0][RoleColumn] != DBNull.Value)
			{
				checkUser.Checked = false;
				checkRole.Checked = true;
			}
		}
		
		
		public string getResponsibleId()
		{
            if (checkUser.Checked)
            {
                return drf.SelectedValue;
            }
			if (checkRole.Checked)
				return ddl.SelectedItem.Value;
			return "-1";  //TODO throw an error;
 		}
		
		public bool IsResponsibleARole
		{
			get
			{
				if (!checkRole.Checked)
					return false;
				//check whether a valid role is selected
				if (null==ddl.SelectedItem)
					return false;
				if (ddl.SelectedItem.Value == "0") //string with "0" indicates that null value is selected
					return false;
				return true;
			}
		}
		
		public bool IsResponsibleAnUser
		{
			get
			{
				if (!checkUser.Checked)
					return false;
				
				//check whether a user is selected
				if (drf.SelectedValue == string.Empty) 
					return false;
				
				return true;
			}
		}
	}
}

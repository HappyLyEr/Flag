using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using GASystem.BusinessLayer;
using GASystem.AppUtils;
using System.Data;
using System.Collections;
using GASystem.DataModel;

namespace GASystem.GUIUtils
{
	/// <summary>
	/// Template for displaying a value as a textbox template in a DataGrid.
	/// Use this for custom editable columns
	/// </summary>
	/// <remarks>
	/// TODO: Apply formatting string.
	/// </remarks>
	public class GAListItemEditTemplate : ITemplate
	{
		//private Control editControl;
		private string _columnName;
		private string _formattingString;
		private FieldDescription _fieldDesc;
		private Hashtable dropDownListContent = new Hashtable();

		public GAListItemEditTemplate(string columnName, FieldDescription fieldDesc)
		{
			_fieldDesc = fieldDesc;
			_columnName = columnName;
		}

		#region ITemplate Members

		public void InstantiateIn(Control container)
		{
			InstantiateEditControlForDataField(_fieldDesc, container);
			//editControl = GetEditControlForDataField(
			//textBox = new TextBox();
			//textBox.DataBinding += new EventHandler(lbl_DataBinding);
			//container.Controls.Add(textBox);
		}

		#endregion

		protected void TextBoxDataBinding(object sender, EventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			
			DataGridItem dgi = (DataGridItem)textBox.NamingContainer;
			string val = ((DataRowView)dgi.DataItem)[_columnName].ToString();
			textBox.Text = val;
		}
		
		protected void LookupFieldTextBoxDataBinding(object sender, EventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			DataGridItem dgi = (DataGridItem)textBox.NamingContainer;
			string val = ((DataRowView)dgi.DataItem)[_columnName].ToString();
			textBox.Text = val;
		}


		protected void LabelDataBinding(object sender, EventArgs e)
		{
			Label label = (Label)sender;
			DataGridItem dgi = (DataGridItem)label.NamingContainer;
			string val = ((DataRowView)dgi.DataItem)[_columnName].ToString();
			label.Text = val;

		}

		protected void NumericControlDataBinding(object sender, EventArgs e)
		{
			WebControls.EditControls.Numeric numControl = (WebControls.EditControls.Numeric)sender;
			DataGridItem dgi = (DataGridItem)numControl.NamingContainer;
			string val = ((DataRowView)dgi.DataItem)[_columnName].ToString();
			
			if (AppUtils.GAUtils.IsNumeric(val))
			{
				numControl.Value = int.Parse(val);
			}

		}

		protected void DateControlDataBinding(object sender, EventArgs e)
		{
			WebControls.EditControls.DateControl dateControl = (WebControls.EditControls.DateControl) sender;
			DataGridItem dgi = (DataGridItem)dateControl.NamingContainer;
			object val = ((DataRowView)dgi.DataItem)[_columnName];
			if (val!=System.DBNull.Value) 
			{
				dateControl.Value = (DateTime) val;
			}
		}

		protected void DateTimeControlDataBinding(object sender, EventArgs e)
		{
			WebControls.EditControls.ListDateTimeControl dateControl = (WebControls.EditControls.ListDateTimeControl) sender;
			DataGridItem dgi = (DataGridItem)dateControl.NamingContainer;
			object val = ((DataRowView)dgi.DataItem)[_columnName];
			if (val!=System.DBNull.Value) 
			{
				dateControl.Value = (DateTime) val;
			}
		}


		protected void DropDownListDataBinding(object sender, EventArgs e)
		{
			DropDownList ddl = (DropDownList)sender;
			DataGridItem dgi = (DataGridItem)ddl.NamingContainer;
			object val = ((DataRowView)dgi.DataItem)[_columnName];
			if (null != val )
			{
				ListItem tmpItem = ddl.Items.FindByText(val.ToString());
				if (null!=tmpItem) 
				{
					ddl.SelectedIndex = ddl.Items.IndexOf(tmpItem);
					
				}
			}
			
		}

		protected void CheckBoxDataBinding(object sender, EventArgs e)
		{
			CheckBox checkBox = (CheckBox)sender;
			DataGridItem dgi = (DataGridItem)checkBox.NamingContainer;
			object val = ((DataRowView)dgi.DataItem)[_columnName];
			if (null != val)
				checkBox.Checked = "True".Equals(val.ToString());
			
		}

		protected void PersonnelPickerDataBinding(object sender, EventArgs e)
		{
			PersonnelField pField = (PersonnelField )sender;
			DataGridItem dgi = (DataGridItem)pField.NamingContainer;
			string val = ((DataRowView)dgi.DataItem)[_columnName].ToString();
			pField.RowId = int.Parse(val);
			PersonnelDS personnelData = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelRowId(pField.RowId);
			if (0!=personnelData.GAPersonnel.Rows.Count)
                pField.DisplayValue = personnelData.GAPersonnel[0].FamilyName + " " + personnelData.GAPersonnel[0].GivenName;
		}

		private Control InstantiateEditControlForDataField(FieldDescription fieldDesc, Control placeHolder)
		{
			Control control;
			Object content;
			TextBox txtBox;
			CheckBox chkBox;
			Label label;
			
			//	FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
			if (null==fieldDesc) return new Control();
			if (fieldDesc.ControlType==null || fieldDesc.ControlType.Length==0)
			{
				fieldDesc.ControlType = "TEXTBOX"; //Textbox is default (in case ControlType is not given)

			}
			switch(fieldDesc.ControlType.ToUpper())			 
			{	
				case "NUMERIC":
					WebControls.EditControls.Numeric numControl = new GASystem.WebControls.EditControls.Numeric();
					numControl.ID = _columnName;
					numControl.DataBinding += new EventHandler(NumericControlDataBinding);
					placeHolder.Controls.Add(numControl);
					//numControl.TabIndex = tabIndex;
					
					//if (c.Table.Rows[0][c] != System.DBNull.Value)
					//	numControl.Value = (int)c.Table.Rows[0][c];
				
					control = (Control) numControl;
					break;
				case "DATE":
					WebControls.EditControls.DateControl dateControl = new GASystem.WebControls.EditControls.DateControl();
					dateControl.ID = _columnName;
					dateControl.DataBinding += new EventHandler(DateControlDataBinding);	
					placeHolder.Controls.Add(dateControl);
					//dateControl.Width = 5;
					//dateControl.TabIndex = tabIndex;
					
					//if (c.Table.Rows[0][c] != System.DBNull.Value)
					//	dateControl.Value = (DateTime)c.Table.Rows[0][c];
					
					control = (Control) dateControl;
					break;
				case "DATETIME":
					WebControls.EditControls.ListDateTimeControl dateTimeControl = new GASystem.WebControls.EditControls.ListDateTimeControl();
					dateTimeControl.ID = _columnName;
					dateTimeControl.DataBinding += new EventHandler(DateTimeControlDataBinding);	
					placeHolder.Controls.Add(dateTimeControl);
					dateTimeControl.Width = 12*8;
					//dateTimeControl.TabIndex = tabIndex;
					//if (c.Table.Rows[0][c] != System.DBNull.Value)
					//	dateTimeControl.Value = (DateTime)c.Table.Rows[0][c];
					
					control = (Control) dateTimeControl;
					break;
				case "TEXTBOX":			  
						
					txtBox = new TextBox();
					txtBox.ID = _columnName;
					txtBox.DataBinding += new EventHandler(TextBoxDataBinding);
					placeHolder.Controls.Add(txtBox);
					txtBox.CssClass = fieldDesc.CssClass;
					//txtBox.TabIndex = tabIndex;
					txtBox.MaxLength = fieldDesc.DataLength;
					
					//if (null != stringContent)
					//	txtBox.Text = stringContent;

					
					if (fieldDesc.ControlWidth!=null && fieldDesc.ControlWidth.Length>0)
						txtBox.Width = new Unit(int.Parse(fieldDesc.ControlWidth)<160 ? int.Parse(fieldDesc.ControlWidth) : 160);
					else //if width is not set. Use default rule
						txtBox.Width = new Unit( (fieldDesc.DataLength< 12 ? fieldDesc.DataLength: 12 )*8+"px"); //at most 4*50 pixel wide

					control = (Control) txtBox;
					//AddTextBoxValidatorControl(c, placeHolder, txtBox);

					break;
				
					//TODO gadataclass is a copy of textbox, replace with dropdown generated from gadataclass enum
				case "GADATACLASS":	  		  
					txtBox = new TextBox();
					txtBox.ID = _columnName;
					txtBox.DataBinding += new EventHandler(TextBoxDataBinding);
					placeHolder.Controls.Add(txtBox);
					txtBox.CssClass = fieldDesc.CssClass;
					//txtBox.TabIndex = tabIndex;
					txtBox.MaxLength = fieldDesc.DataLength;
					
					//if (null != stringContent)
					//	txtBox.Text = stringContent;

					if (fieldDesc.ControlWidth!=null && fieldDesc.ControlWidth.Length>0)
						txtBox.Width = new Unit(fieldDesc.ControlWidth);
					else //if width is not set. Use default rule
						txtBox.Width = new Unit( (fieldDesc.DataLength< 20 ? fieldDesc.DataLength: 20 )*8+"px"); //at most 7*50 pixel wide

					control = (Control) txtBox;
					//AddTextBoxValidatorControl(c, placeHolder, txtBox);

					break;

				case "CHECKBOX":			  
					chkBox = new CheckBox();
					chkBox.ID = _columnName;
					chkBox.DataBinding += new EventHandler(CheckBoxDataBinding);
					placeHolder.Controls.Add(chkBox);
					chkBox.CssClass = fieldDesc.CssClass;
					//chkBox.TabIndex = tabIndex;
					
					//if (null != stringContent)
					//	chkBox.Checked = "True".Equals(stringContent);
	
					control = (Control) chkBox;
					break;

				case "TEXTAREA":
					txtBox = new TextBox();
					txtBox.ID = _columnName;
					txtBox.DataBinding += new EventHandler(TextBoxDataBinding);
					placeHolder.Controls.Add(txtBox);
					txtBox.CssClass = fieldDesc.CssClass;
					//txtBox.TabIndex = tabIndex;
					//if (null != stringContent)
					//	txtBox.Text = stringContent;
            		
					txtBox.TextMode = TextBoxMode.MultiLine;
					txtBox.MaxLength = fieldDesc.DataLength;
		
					txtBox.Rows = 4;
					if (fieldDesc.ControlWidth!=null && fieldDesc.ControlWidth.Length>0)
						txtBox.Width = new Unit(fieldDesc.ControlWidth);
					else //if width is not set. Use default rule
						txtBox.Width = new Unit( (fieldDesc.DataLength< 20 ? fieldDesc.DataLength: 20 )*8+"px"); //at most 8*50 pixel wide

					control = (Control) txtBox;
					//AddTextBoxValidatorControl(c, placeHolder, txtBox);
					break;

				case "DROPDOWNLIST":
					DropDownList ddl = new DropDownList();
					ddl.ID = _columnName;
					ddl.DataBinding += new EventHandler(DropDownListDataBinding);
					ddl.Enabled = true;
					placeHolder.Controls.Add(ddl);
					ddl.CssClass = fieldDesc.CssClass;

					if (!dropDownListContent.ContainsKey(ddl.ID)) 
					{
						ArrayList dropDownValues = GetListItems(fieldDesc);
						dropDownListContent[ddl.ID] = dropDownValues;
					}

					ArrayList items = (ArrayList) dropDownListContent[ddl.ID];
					foreach (ListItem item in items)
					{
						ddl.Items.Add(new ListItem(item.Text, item.Value));
					}

					/**label = new Label();
					label.ID = _columnName;
					label.DataBinding += new EventHandler(LabelDataBinding);
					placeHolder.Controls.Add(label);
					label.CssClass = fieldDesc.CssClass;*/

					//ddl.TabIndex = tabIndex;
					//CodeTables.BindCodeTable(ddl, GetListItems(fieldDesc));
					
					/*content = c.Table.Rows[0][c];
					if (null != content)
					{
						ListItem tmpItem = ddl.Items.FindByValue(content.ToString());
						if (null!=tmpItem) 
							tmpItem.Selected=true;

					}*/
					control = (Control) ddl;
					//control = (Control) label;
					break;

				case "LOOKUPFIELD":
					txtBox = new TextBox();
					txtBox.ID = _columnName;
					txtBox.DataBinding += new EventHandler(LookupFieldTextBoxDataBinding);
					txtBox.Enabled = false;
					placeHolder.Controls.Add(txtBox);
					txtBox.CssClass = fieldDesc.CssClass;
					//txtBox.TabIndex = tabIndex;
					txtBox.MaxLength = fieldDesc.DataLength;
					
					//if (null != stringContent)
					//	txtBox.Text = stringContent;

					if (fieldDesc.ControlWidth!=null && fieldDesc.ControlWidth.Length>0)
						txtBox.Width = new Unit(int.Parse(fieldDesc.ControlWidth)/2);
					else //if width is not set. Use default rule
						txtBox.Width = new Unit( 12*8+"px"); //at most 8*20 pixel wide

					control = (Control) txtBox;
					
					break;

				case "LOOKUPFIELDMULTIPLE":
					label = new Label();
					label.ID = _columnName;
					label.DataBinding += new EventHandler(LabelDataBinding);
					placeHolder.Controls.Add(label);
					label.CssClass = fieldDesc.CssClass;
					control = (Control) label;
					break;
				/**	case "FILELOOKUPFIELD":LOOKUPFIELDMULTIPLE
						control = AddLookupField(c, placeHolder);
						break;
				
					case "PERSONNELPICKER":   //Optional personnel (not foreign key constraint)
						control = AddPersonnelPicker(c, placeHolder);
						break;*/
				case "PERSONNELPICKERFK":   //Personnel foreign key
					PersonnelField pField = (PersonnelField) placeHolder.Page.LoadControl("PersonnelField.ascx");
					pField.ID = _columnName;
					pField.DataBinding += new EventHandler(PersonnelPickerDataBinding);
					placeHolder.Controls.Add(pField);
					control = (Control) pField;
					break;
					/*	case "FILECONTENT":
							WebControls.EditControls.FileContent fc = new GASystem.WebControls.EditControls.FileContent();
							fc.ID = c.ColumnName;
							placeHolder.Controls.Add(fc);
							control = fc;
							break;
						case "YEARMONTHSPAN":
							WebControls.EditControls.YearMonthSpan ym = new GASystem.WebControls.EditControls.YearMonthSpan();
							ym.ID = c.ColumnName;
							placeHolder.Controls.Add(ym);
							if (c.Table.Rows[0][c] != System.DBNull.Value)
								ym.Value = (int)c.Table.Rows[0][c];
							control = ym;
							break;
				
						case "URL":			  
							txtBox = new TextBox();
							txtBox.ID = c.ColumnName;
							placeHolder.Controls.Add(txtBox);
							txtBox.CssClass = fieldDesc.CssClass;
							txtBox.TabIndex = tabIndex;
							if (null != stringContent)
								txtBox.Text = stringContent;

							if (fieldDesc.ControlWidth!=null && fieldDesc.ControlWidth.Length>0)
								txtBox.Width = new Unit(fieldDesc.ControlWidth);
	
							control = (Control) txtBox;
							break;*/
				default :
					control = new Control();
					break;
			}

			//TODO: relateddatarecord.ascx used by lookupfield does not have a element with the id of the control. 
			// are ignoring validators here. they are specified in the case statement above
			// consider rewriting relateddatarecord to a webcontrol
		/*	if (!fieldDesc.ControlType.ToUpper().Equals("LOOKUPFIELD")) 
			{
				GASystem.GUIUtils.ValidationControl.ValidationControlsCreator validationCreator = 
					new GASystem.GUIUtils.ValidationControl.ValidationControlsCreator(placeHolder, fieldDesc);
				validationCreator.AddAllValidatorControls();
			}*/			
			return control;
		}

		private ArrayList GetListItems(FieldDescription fieldDesc)
		{
			ArrayList listItems = new ArrayList();
			listItems = CodeTables.GetList(fieldDesc.ListCategory, GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord);
			//listItems = CodeTables.GetList(fieldDesc.ListCategory);
			return listItems;
		}


	}
}

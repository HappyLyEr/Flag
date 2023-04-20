using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
//using DBauer.Web.UI.WebControls;
using GASystem.AppUtils;
using GASystem.BusinessLayer;
using GASystem.DataModel;
using GASystem.GAExceptions;
using GASystem.GAGUIEvents;
using log4net;

namespace GASystem.WebControls.EditControls.EditForm
{
	/// <summary>
	/// Summary description for GeneralEditForm.
	/// </summary>
	public class GeneralEditForm : WebControl,  INamingContainer
	{
		//private DBauer.Web.UI.WebControls.DynamicControlsPlaceholder DCP;
		private PlaceHolder DCP;
		private ArrayList columnsToDisplay = new ArrayList();
		
		public GeneralEditForm()
		{
			//
			// TODO: Add constructor logic here
			//
			//DCP = new DynamicControlsPlaceholder();
			DCP = new PlaceHolder();
		}
		

		const int MAX_NUMBER_OF_COLUMNS = 15;
		private static readonly ILog _logger = LogManager.GetLogger(typeof(EditDataRecord));

		//
		//TODO: Clientside datatype validation
		//TODO: Formulabased generation of userdefined ID's (eg. Incident ID)
		//ISSUE: When editing existing records, some fields may not be editbale according to business rules. 
		//		 How do describe this in terms of businessrules, and how do we implement support for this in GUI?
		
		private short tabIndex = 0;
		
		
		
		private bool isNewRecord = false;

		private void Page_Load(object sender, System.EventArgs e)
		{
			
		}





		public void SetupForm()
		{
			try
			{
				generateForm();
			}
			catch (GAException gaEx)
			{
				
			}
			catch (Exception e)
			{
				_logger.Error(e.Message, e);
			}
			
		}

		/// <summary>
		/// Check whether the speficid column should be displayed on the form. 
		/// Column to display are added using the public method AddColumnToDisplay.
		/// If no columns are added we are by default showing all columns.
		/// </summary>
		/// <param name="columnName"></param>
		/// <returns>
		/// True if no columns has been added using AddColumnToDisplay. 
		/// otherwise True or False depending on whether the column has been added using AddColumnToDisplay
		/// </returns>
		private bool DisplayColumn(string columnName)
		{
			//by default display all columns if no spesific columns has been added
			if (columnsToDisplay.Count == 0)
				return true;	
			
			return columnsToDisplay.Contains(columnName);
		}
		
		/// <summary>
		/// Add column to display in the form.
		/// If no columns are specified all columns are display as defined in gafielddefinition.
		/// </summary>
		/// <param name="ColumnName"></param>
		public void AddColumnToDisplay(string ColumnName)
		{
			columnsToDisplay.Add(ColumnName);
		}
		
		
		private void generateForm()
		{
			
			Label lblHeader = new Label();
			lblHeader.Text = this.DataClass.ToString();
			
			
			
			if (RecordDataSet==null) return;
		 
			HtmlTable tbl = new HtmlTable();
			tbl.ID = "Tablex" + this.DataClass.ToString();
		
			DCP.Controls.Clear();
			DCP.Controls.Add(lblHeader);
			DCP.Controls.Add(tbl);
			
			//added by JOF, set class on table.. Moved by FW (attributes can only be added after control is added to parentcontrol)
			tbl.Attributes.Add("class", "EditForm_Table");
			
			//set tabindex on buttons
	//		int previousRowOrderPart = -1;
			//int numberOfColumns = RecordDataSet.Tables[DataClass].Columns.Count;
            // Tor 20140320 added ownerclass (2nd parameter in call below) to hide fields that should not show in current memberclass, ownerclass record)
			FieldDescription[] AllFieldDescriptions = FieldDefintion.GetFieldDescriptionsDetailsForm(DataClass,"");
			//foreach (FieldDescription fieldDesc in FieldDefintion.GetFieldDescriptions(GADataRecord.ParseGADataClass(DataClass)))
			


			int numberOfColumns = AllFieldDescriptions.Length;
            bool lastField = false;
			for (int cNo=0; cNo <numberOfColumns ; cNo++  ) 
			{
				FieldDescription fd = AllFieldDescriptions[cNo];
				if (fd != null && DisplayFieldOnForm(fd) && DisplayColumn(fd.FieldId) ) 
				{
				
				
					//find where to put this field

					int previousRowOrder, currentRowOrder, nextRowOrder;
					bool inRow = false, LastInRow = false, FirstInRow = false;

					if (cNo == 0)
						previousRowOrder = int.MinValue;
					else
						previousRowOrder = AllFieldDescriptions[cNo - 1].ColumnOrder / 1000;

					if (cNo + 1 == numberOfColumns) 
					{
						lastField = true;
						nextRowOrder = int.MaxValue;
					} 
					else
						nextRowOrder = AllFieldDescriptions[cNo + 1].ColumnOrder / 1000;

				
					currentRowOrder = fd.ColumnOrder / 1000;
				
					inRow = previousRowOrder == currentRowOrder;

					//get data about column
					DataColumn c = RecordDataSet.Tables[DataClass].Columns[fd.FieldId];


					//add column
				
					if (inRow)
						LastInRow =  currentRowOrder != nextRowOrder; //we are in a row, is this the last field in the row?
					else 
					{
						FirstInRow = currentRowOrder == nextRowOrder; //we are not in a row, are we starting a new row?
					}

					if (inRow || FirstInRow) 
					{
						AddMultiRow(tbl, c, FirstInRow, LastInRow, fd);
					} 
					else    // not inRow
					{
						//if textarea add siglecontent below
						AddLabelContentRow(tbl, c, fd);
					}
			
				
				}   //end if fd != null
				
				
			}  // end for
				
		}  //end class
				
				
				
		private void AddLabelContentRow(HtmlTable tbl, DataColumn c, FieldDescription fd) 
		{
			//hack add textarea as content below
			if (fd.ControlType.ToUpper().TrimEnd() == "TEXTAREA" || fd.ControlType.ToUpper().TrimEnd() == "TEXTEDITOR") 
			{
				AddLabelContentBelowRow(tbl,c,fd);
			} 
			else 
			{
				//add row
				HtmlTableRow tblRow;
				tblRow = new HtmlTableRow();
				tblRow.ID = tbl.ID + c.ColumnName;
				tbl.Rows.Add(tblRow);
			
				//label cell
				HtmlTableCell cell = new HtmlTableCell();
				cell.ID = fd.FieldId + "c1labels";
				tblRow.Cells.Add(cell);
			
				Label lbl = new Label();
				lbl.ID = "Label_" + fd.FieldId;
				cell.Controls.Add(lbl);
				lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);
				lbl.EnableViewState = true;
				
				cell.Attributes.Add("class", "FieldViewLabelCell");

				//content cell
				HtmlTableCell contentCell = new HtmlTableCell();
				contentCell = new HtmlTableCell();
				contentCell.ID = fd.FieldId + "c" + 3;

				tblRow.Cells.Add(contentCell);
				contentCell.Attributes.Add("class", "FieldLastInRow");
				contentCell.ColSpan = MAX_NUMBER_OF_COLUMNS - 1;
			
			
			
				//add control	
				AddEditControl(c, contentCell);

			}

		}
				

		private void AddMultiRow(HtmlTable tbl, DataColumn c, bool FirstInRow, bool LastInRow, FieldDescription fd)
		{
			HtmlTableRow tblRow;
			HtmlTableRow tblRowContent;

			if (FirstInRow)    //create new rows
			{		
				tblRow = new HtmlTableRow();
				tblRow.ID = tbl.ID + c.ColumnName;
				tbl.Rows.Add(tblRow);

				tblRowContent = new HtmlTableRow();
				tblRowContent.ID = tbl.ID + c.ColumnName + "contentrow";
				tbl.Rows.Add(tblRowContent);
			} 
			else    //reuse exsisting rows 
			{
				tblRow = tbl.Rows[tbl.Rows.Count - 2];
				tblRowContent = tbl.Rows[tbl.Rows.Count - 1];
			}
			
			
			//label cell
			HtmlTableCell cell = new HtmlTableCell();
			cell.ID = fd.FieldId + "c1labels";
			tblRow.Cells.Add(cell);
			
			Label lbl = new Label();
			lbl.ID = "Label_" + fd.FieldId;
			cell.Controls.Add(lbl);
			lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);
			lbl.EnableViewState = true;
			
			cell.Attributes.Add("class", "FieldViewLabelCell");

			//content cell
			HtmlTableCell contentCell = new HtmlTableCell();
			contentCell = new HtmlTableCell();
			contentCell.ID = fd.FieldId + "c" + 3;

			tblRowContent.Cells.Add(contentCell);
			contentCell.Attributes.Add("class", "FieldViewContentBelowCell");
			
			
			if (LastInRow) 
			{
				//add dummy cell
				//label cell
				HtmlTableCell dummycell = new HtmlTableCell();
				dummycell.ID = fd.FieldId + "c1labelslastcell";
			
			
				
				tblRow.Cells.Add(dummycell);
				dummycell.Attributes.Add("class", "FieldLastInRow");
				dummycell.ColSpan = MAX_NUMBER_OF_COLUMNS - tblRow.Cells.Count;

				//content cell
				HtmlTableCell contentDummyCell = new HtmlTableCell();
				contentDummyCell = new HtmlTableCell();
				contentDummyCell.ID = fd.FieldId + "c" + 3 + "lastcell";

				tblRowContent.Cells.Add(contentDummyCell);
				contentDummyCell.Attributes.Add("class", "FieldLastInRow");
				contentDummyCell.ColSpan = MAX_NUMBER_OF_COLUMNS - tblRowContent.Cells.Count;

				//				//content cell, allow content cell to expand to the right
				//				
				//				tblRowContent.Cells[tblRowContent.Cells.Count - 1].Attributes.Add("class", "FieldLastInRow");
				//				tblRowContent.Cells[tblRowContent.Cells.Count - 1].ColSpan = MAX_NUMBER_OF_COLUMNS - tblRowContent.Cells.Count;


			}

			//add control	
			AddEditControl(c, contentCell);



		}
				
				
				
		
		private void AddLabelContentBelowRow(HtmlTable tbl, DataColumn c, FieldDescription fd)
		{
			//create rows

			HtmlTableRow tblRow;
			HtmlTableRow tblRowContent;

			
			tblRow = new HtmlTableRow();
			tblRow.ID = tbl.ID + c.ColumnName;
			tbl.Rows.Add(tblRow);

			tblRowContent = new HtmlTableRow();
			tblRowContent.ID = tbl.ID + c.ColumnName + "contentrow";
			tbl.Rows.Add(tblRowContent);
			

			
			//label cell
			HtmlTableCell cell = new HtmlTableCell();
			cell.ID = fd.FieldId + "c1labels";
			tblRow.Cells.Add(cell);
			
			Label lbl = new Label();
			lbl.ID = "Label_" + fd.FieldId;
			cell.Controls.Add(lbl);
			lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);
			lbl.EnableViewState = true;
			
			cell.Attributes.Add("class", "FieldViewSingleLabelCell");
			cell.ColSpan = MAX_NUMBER_OF_COLUMNS;

			//content cell
			HtmlTableCell contentCell = new HtmlTableCell();
			contentCell = new HtmlTableCell();
			contentCell.ID = fd.FieldId + "c" + 3;

			tblRowContent.Cells.Add(contentCell);
			contentCell.Attributes.Add("class", "FieldViewSingleContentBelowCell");
			contentCell.ColSpan = MAX_NUMBER_OF_COLUMNS;
			
						
			//add control	
			AddEditControl(c, contentCell);



		}
				
		/// <summary>
		/// Method for deciding whether field should be displayed on form. 
		/// Factored out in a separate metode for ease of overriding
		/// </summary>
		/// <param name="fd">Fielddescription for the field in question</param>
		/// <returns>bool</returns>
		protected virtual bool DisplayFieldOnForm(FieldDescription fd) 
		{
			return !fd.HideInDetail;
		}



		private void setColSpanOnRows(HtmlTable tbl) 
		{
			//			if (tbl.Rows.Count == 0)
			//				return;
			//
			//			HtmlTableRow tblRow = tbl.Rows[tbl.Rows.Count -1];
			foreach (HtmlTableRow tblRow in tbl.Rows) 
			{
				int numberOfCells = tblRow.Cells.Count;
				if (numberOfCells > 0)
					tblRow.Cells[numberOfCells - 1].ColSpan = 15 - numberOfCells;
			}
			//
			//			tblRow = tbl.Rows[tbl.Rows.Count -2];
			//
			//			numberOfCells = tblRow.Cells.Count;
			//			tblRow.Cells[numberOfCells - 1].ColSpan = 15 - numberOfCells;
			
			
		}

		/// <summary>
		/// set css class attribute for tables cells where content is below the header
		/// </summary>
		/// <param name="fd"></param>
		/// <param name="Cell"></param>
		private void setCellStyle(FieldDescription fd, HtmlTableCell Cell) 
		{
			switch(fd.ControlType.ToUpper().TrimEnd()) 
			{
				case "CHECKBOX":
					Cell.Attributes.Add("class", "FieldViewContentBelowCell_checkbox");
					break;
				default:
					Cell.Attributes.Add("class", "FieldViewContentBelowCell");
					break;

			}
		}

		private Control AddEditControl(DataColumn c, Control placeHolder)
		{
			Control control;
			Object content;
			TextBox txtBox;
			CheckBox chkBox;
			
			String stringContent = c.Table.Rows[0][c].ToString();//Get data from first row
			
			FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
			if (null==fieldDesc) return new Control();
			if (fieldDesc.ControlType==null || fieldDesc.ControlType.Length==0)
			{
				if (c.DataType==System.Type.GetType("System.Int32"))
					fieldDesc.ControlType = "NUMERIC";
				else
					fieldDesc.ControlType = "TEXTBOX"; //Textbox is default (in case ControlType is not given)

			}
			switch(fieldDesc.ControlType.ToUpper().TrimEnd())			  //JOF added trimend, easy to accidentally add whitespace in the database
			{	
				case "NUMERIC":
					WebControls.EditControls.Numeric numControl = new GASystem.WebControls.EditControls.Numeric();
					numControl.ID = c.ColumnName;
					placeHolder.Controls.Add(numControl);
					numControl.TabIndex = tabIndex;
		
					
					if (c.Table.Rows[0][c] != System.DBNull.Value)
						numControl.Value = (int)c.Table.Rows[0][c];
				
					control = (Control) numControl;
					break;
				case "DATE":
					WebControls.EditControls.DateControl dateControl = new GASystem.WebControls.EditControls.DateControl();
					dateControl.ID = c.ColumnName;
						
					placeHolder.Controls.Add(dateControl);
					dateControl.TabIndex = tabIndex;
					if (c.Table.Rows[0][c] != System.DBNull.Value)
						dateControl.Value = (DateTime)c.Table.Rows[0][c];
					
					control = (Control) dateControl;
					break;
				case "DATETIME":
					WebControls.EditControls.DateTimeControl dateTimeControl = new GASystem.WebControls.EditControls.DateTimeControl();
					dateTimeControl.ID = c.ColumnName;
						
					placeHolder.Controls.Add(dateTimeControl);
					dateTimeControl.TabIndex = tabIndex;
					if (c.Table.Rows[0][c] != System.DBNull.Value)
						dateTimeControl.Value = (DateTime)c.Table.Rows[0][c];
					
					control = (Control) dateTimeControl;
					break;
				case "WORKFLOWSTARTED":
					WebControls.EditControls.WorkflowStarted ws = new GASystem.WebControls.EditControls.WorkflowStarted();
					ws.ID = c.ColumnName;
					placeHolder.Controls.Add(ws);
					ws.EnableViewState = true;
					ws.NewRecord = this.isNewRecord;
					
					control = ws;
					break;
				case "TEXTBOX":			  
						
					txtBox = new TextBox();
					txtBox.ID = c.ColumnName;
					placeHolder.Controls.Add(txtBox);
					if (fieldDesc.CssClass == string.Empty)
						txtBox.CssClass = "input_text";
					else
						txtBox.CssClass = fieldDesc.CssClass;
					txtBox.TabIndex = tabIndex;
					txtBox.MaxLength = fieldDesc.DataLength;
					txtBox.ReadOnly = fieldDesc.IsReadOnly;
					
					if (null != stringContent)
						txtBox.Text = stringContent;

					//commented out by JOF 11.05.06. Do not set width on text input directly. 
					//Use CssClass and add an entry in norsolutions/skins.css instead

					//						if (fieldDesc.ControlWidth!=null && fieldDesc.ControlWidth.Length>0)
					//							txtBox.Width = new Unit(fieldDesc.ControlWidth);
					//						else //if width is not set. Use default rule
					//							txtBox.Width = new Unit( (fieldDesc.DataLength<50 ? fieldDesc.DataLength: 50 )*8+"px"); //at most 7*50 pixel wide

					control = (Control) txtBox;
					AddTextBoxValidatorControl(c, placeHolder, txtBox);

					break;
				
					//TODO gadataclass is a copy of textbox, replace with dropdown generated from gadataclass enum
				case "GADATACLASS":	  		  
						
					txtBox = new TextBox();
					txtBox.ID = c.ColumnName;
					placeHolder.Controls.Add(txtBox);
					txtBox.CssClass = fieldDesc.CssClass;
					txtBox.TabIndex = tabIndex;
					txtBox.MaxLength = fieldDesc.DataLength;
					
					if (null != stringContent)
						txtBox.Text = stringContent;

					if (fieldDesc.ControlWidth!=null && fieldDesc.ControlWidth.Length>0)
						txtBox.Width = new Unit(fieldDesc.ControlWidth);
					else //if width is not set. Use default rule
						txtBox.Width = new Unit( (fieldDesc.DataLength<50 ? fieldDesc.DataLength: 50 )*8+"px"); //at most 7*50 pixel wide

					control = (Control) txtBox;
					AddTextBoxValidatorControl(c, placeHolder, txtBox);

					break;

				case "CHECKBOX":			  
					chkBox = new CheckBox();
					chkBox.ID = c.ColumnName;
					placeHolder.Controls.Add(chkBox);
					chkBox.CssClass = fieldDesc.CssClass;
					chkBox.TabIndex = tabIndex;
					if (null != stringContent)
						chkBox.Checked = "True".Equals(stringContent);
	
					control = (Control) chkBox;
					break;

				case "TEXTAREA":
					txtBox = new TextBox();
					txtBox.ID = c.ColumnName;
					placeHolder.Controls.Add(txtBox);
					//txtBox.CssClass = fieldDesc.CssClass;
					txtBox.CssClass = "FieldContent_TextArea";
					txtBox.TabIndex = tabIndex;
					if (null != stringContent)
						txtBox.Text = stringContent;
            		
					txtBox.TextMode = TextBoxMode.MultiLine;
					txtBox.MaxLength = fieldDesc.DataLength;
		
					if (fieldDesc.ControlHeight == 0)
						txtBox.Rows = 4;				//default to 4 rows
					else 
						txtBox.Rows = fieldDesc.ControlHeight;



					//					if (fieldDesc.ControlWidth!=null && fieldDesc.ControlWidth.Length>0)
					//						txtBox.Width = new Unit(fieldDesc.ControlWidth);
					//					else //if width is not set. Use default rule
					//						txtBox.Width = new Unit( (fieldDesc.DataLength<50 ? fieldDesc.DataLength: 50 )*8+"px"); //at most 8*50 pixel wide

					control = (Control) txtBox;
					AddTextBoxValidatorControl(c, placeHolder, txtBox);
					break;

			

				case "TEXTEDITOR":
					string appPath = this.Page.Request.ApplicationPath;

			
					
					txtBox = new TextBox();
					txtBox.ID = c.ColumnName;
					placeHolder.Controls.Add(txtBox);
					//txtBox.CssClass = fieldDesc.CssClass;
					txtBox.CssClass = "FieldContent_MCETextEditor";
					txtBox.TabIndex = tabIndex;
					if (null != stringContent)
						txtBox.Text = stringContent;
            		
					txtBox.TextMode = TextBoxMode.MultiLine;
					txtBox.MaxLength = fieldDesc.DataLength;
		
					if (fieldDesc.ControlHeight == 0)
						txtBox.Rows = 4;				//default to 4 rows
					else 
						txtBox.Rows = fieldDesc.ControlHeight;

					control = (Control) txtBox;
					//	AddTextBoxValidatorControl(c, placeHolder, txtBox);
					//	break;
					break;
				case "DROPDOWNLIST":
					DropDownList ddl = new DropDownList();
					
					ddl.ID = c.ColumnName;
					placeHolder.Controls.Add(ddl);
					ddl.CssClass = fieldDesc.CssClass;
					ddl.TabIndex = tabIndex;
					
					content = c.Table.Rows[0][c];
					ArrayList dropDownValues = GetListItems(fieldDesc);
					

					CodeTables.BindCodeTable(ddl, dropDownValues);

					if (DBNull.Value == content) 
					{
						ddl.Items.Insert(0, new ListItem(string.Empty, "0"));  
						//no current value, insert "null value" at start of list
					}
					if (DBNull.Value != content)
					{
						ListItem tmpItem = ddl.Items.FindByValue(content.ToString());
						if (null!=tmpItem) 
							tmpItem.Selected=true;

					} 
					
					
					control = (Control) ddl;
					break;

				case "LOOKUPFIELD":
					control = AddLookupField(c, placeHolder);
					break;
				case "LOOKUPFIELDMULTIPLE":
					
					control = CreateLookupFieldMultipleControl(c, placeHolder);
					//					
					//					//Only support multiple select when in new-mode
					//					if (c.Table.Rows[0].RowState==DataRowState.Added) 
					//						control = AddLookupFieldMultiSelect(c, placeHolder);
					//					else
					//						control = AddLookupField(c, placeHolder);

					break;
				case "FILELOOKUPFIELD":
					control = AddLookupField(c, placeHolder);
					break;
				
				case "PERSONNELPICKER":   //Optional personnel (not foreign key constraint)
					control = AddPersonnelPicker(c, placeHolder);
					break;
				case "PERSONNELPICKERFK":   //Personnel foreign key
					control = AddPersonnelPickerFk(c, placeHolder);
					break;
				case "FILECONTENT":
					WebControls.EditControls.FileContent fc = new GASystem.WebControls.EditControls.FileContent();
					fc.ID = c.ColumnName;
					placeHolder.Controls.Add(fc);
					control = fc;
					break;
				case "FILEURL":
					WebControls.EditControls.FileURL furl = new GASystem.WebControls.EditControls.FileURL();
					furl.ID = c.ColumnName;
					placeHolder.Controls.Add(furl);
					if (null != stringContent)
						furl.Value = stringContent;
					control = furl;
					break;
				
				case "FILEMIMETYPE":
					WebControls.EditControls.FileMimetype fm = new GASystem.WebControls.EditControls.FileMimetype();
					fm.ID = c.ColumnName;
					placeHolder.Controls.Add(fm);
					control = fm;
					break;
				case "HAZARDMATRIX":
					//					HazardMatrix
					WebControls.EditControls.HazardMatrix.HazardMatrixControl hm = new GASystem.WebControls.EditControls.HazardMatrix.HazardMatrixControl();
					hm.ID = c.ColumnName;
					placeHolder.Controls.Add(hm);
					hm.Text = stringContent;
					
					control = hm;
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
					break;
				default :
					control = new Control();
					break;
			}

			
			//TODO: relateddatarecord.ascx used by lookupfield does not have a element with the id of the control. 
			// are ignoring validators here. they are specified in the case statement above
			// consider rewriting relateddatarecord to a webcontrol
			
			if (! (fieldDesc.ControlType.ToUpper().Equals("LOOKUPFIELD") || fieldDesc.ControlType.ToUpper().Equals("LOOKUPFIELDMULTIPLE"))) 
			{
				GASystem.GUIUtils.ValidationControl.ValidationControlsCreator validationCreator = 
					new GASystem.GUIUtils.ValidationControl.ValidationControlsCreator(placeHolder, fieldDesc);
				validationCreator.AddAllValidatorControls(false);
			}			
	


			return control;
		}


		/// <summary>
		/// Method for creating the Multiplepicker control. Multiple picker is only displayed if we are in new mode and 
		/// in add group of records mode. In proteced metod for ease of overrideing.
		/// </summary>
		/// <param name="IsInNewMode">Is form new record?</param>
		/// <returns>Control</returns>
		protected virtual Control CreateLookupFieldMultipleControl(DataColumn c, Control placeHolder) 
		{
			
			//			//Only support multiple select when in new-mode
			//			if (c.Table.Rows[0].RowState==DataRowState.Added) 
			//				return  AddLookupFieldMultiSelect(c, placeHolder);
			
			return AddLookupField(c, placeHolder);
		}


		private Control AddPersonnelPicker(DataColumn c, Control placeHolder)
		{
			FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
			PersonnelField pField = (PersonnelField) this.Page.LoadControl("PersonnelField.ascx");
			pField.ID = c.ColumnName;
			placeHolder.Controls.Add(pField);
			pField.DisplayValue = c.Table.Rows[0][c].ToString();
			
			String rowIdColumnName = c.ColumnName + "PersonnelRowId"; 
		
			//TODO: ignore this bit for now if table does not contain c.ColumnName + "PersonnelRowId" column
			// ask Frank what it is supposed to do. Looks like it is not used.
			if (c.Table.Columns.Contains(rowIdColumnName)) 
			{
				if (null!=c.Table.Rows[0][rowIdColumnName] && 0<c.Table.Rows[0][rowIdColumnName].ToString().Length)
					pField.RowId = (int) int.Parse(c.Table.Rows[0][rowIdColumnName].ToString());
			}
			pField.IsForeignKeyConstraint = false;

			return (Control) pField;
		}

		private Control AddPersonnelPickerFk(DataColumn c, Control placeHolder)
		{

			PersonnelField pField = (PersonnelField) this.Page.LoadControl("PersonnelField.ascx");
			pField.ID = c.ColumnName;
			placeHolder.Controls.Add(pField);
			if (null!=c.Table.Rows[0][c] && 0!=(int)c.Table.Rows[0][c])
			{
				pField.RowId = int.Parse(c.Table.Rows[0][c].ToString());
				PersonnelDS personnelData = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelRowId(pField.RowId);
				if (0!=personnelData.GAPersonnel.Rows.Count) 
					pField.DisplayValue = personnelData.GAPersonnel[0].GivenName + " " + personnelData.GAPersonnel[0].FamilyName;
			}

			return (Control) pField;
		}

		/// <summary>
		/// Create a usercontrol that displays a rowId column (display a value from a related parent table)
		/// </summary>
		/// <param name="c">This must be a integer rowId column</param>
		/// <param name="placeHolder">The Lookupfield usercontrol will be added to this control</param>
		/// <returns></returns>
		protected Control AddLookupField(DataColumn c, Control placeHolder)
		{
			FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
			RelatedDataRecordField drf = (RelatedDataRecordField) this.Page.LoadControl("~/gagui/usercontrols/RelatedDataRecordField.ascx");
			drf.ID = c.ColumnName;
			placeHolder.Controls.Add(drf);

			drf.FieldDescriptionInfo = fieldDesc;
			drf.FieldRequired = fieldDesc.RequiredField;
			//			if (c.Table.Rows[0][c].ToString().Length>0) 
			//			{
			//				drf.RowId = int.Parse(c.Table.Rows[0][c].ToString());
			//			}
			drf.KeyValue = c.Table.Rows[0][c].ToString();
			drf.GenerateControl();
			return (Control) drf;
		}

		/// <summary>
		/// Same as AddLookupField, but is allowed to hold multiple related records. Use when creating
		/// multiple instances of an object. May only be used on new (not when editing existing datarevcord)
		/// </summary>
		/// <param name="c">This must be a integer rowId column</param>
		/// <param name="placeHolder">The Lookupfield usercontrol will be added to this control</param>
		/// <returns></returns>
		protected Control AddLookupFieldMultiSelect(DataColumn c, Control placeHolder)
		{
			FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
			RelatedDataRecordFieldMultiple drf = (RelatedDataRecordFieldMultiple) this.Page.LoadControl("RelatedDataRecordFieldMultiple.ascx");
			drf.ID = c.ColumnName;
			placeHolder.Controls.Add(drf);

			drf.FieldDescriptionInfo = fieldDesc;
			drf.FieldRequired = fieldDesc.RequiredField;
			if (c.Table.Rows[0][c].ToString().Length>0) 
			{
				drf.RowId = int.Parse(c.Table.Rows[0][c].ToString());
			}
			
			drf.GenerateControl();
			return (Control) drf;
		}

		private void AddTextBoxValidatorControl(DataColumn c, Control placeHolder, TextBox textBox)
		{
			if (!c.AllowDBNull)
			{
				RequiredFieldValidator val = new RequiredFieldValidator();
				val.ID = "ReqFVal_" + textBox.ID;
				placeHolder.Controls.Add(val);
				val.ControlToValidate = textBox.ID;
				val.ErrorMessage = Localization.GetErrorText("FieldRequired");
				val.EnableClientScript = false;
			}

		}

		private ArrayList GetListItems(FieldDescription fieldDesc)
		{
			
			ArrayList listItems = new ArrayList();
			listItems = CodeTables.GetList(fieldDesc.ListCategory, GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord);
			return listItems;
		}

		
		/// <summary>
		/// Convert a string date to shortformat. If input date has invalid format, return
		/// smallest possible date
		/// </summary>
		/// <param name="stringDate"></param>
		/// <returns></returns>
		private string GetShortDateString(string stringDate)
		{
			string returnDate = DateTime.Now.ToShortDateString();
			try
			{
				returnDate = DateTime.Parse(stringDate).ToShortDateString();
			}
			catch (Exception ex)
			{

			}
			return returnDate;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			
			this.Controls.Add(DCP);
			this.generateForm();
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

		/// <summary>
		/// Copy values from formcontrols to dataset. Dataset is not saved to database
		/// </summary>
		public void UpdateRecordSet() 
		{
			SaveRecord();
		}
		
		/// <summary>
		/// Copy values from formcontrols to dataset. Dataset is not updated. Users of this control must perform the databaseupdate
		/// </summary>
		private void SaveRecord()
		{
			DataSet dataset = RecordDataSet;
			ArrayList multipleRowIds = null;
			String duplicateColumnName = "";
			
			//Traverse every column of the datatable and update altered columns
			foreach (DataColumn c in dataset.Tables[DataClass].Columns)
			{
				TextBox tmpTextBox;
				CheckBox tmpCheckBox;
				DropDownList tmpDropDownList;
				PersonnelField tmpPersonnelField;
				//				FreeTextBoxControls.FreeTextBox tmpFTB;
				//				CuteEditor.Editor tmpCEditor;
				
				RelatedDataRecordField tmpRelatedDataRecordField;
				RelatedDataRecordFieldMultiple tmpRelatedDataRecordFieldMultiple;
				WebControls.EditControls.DateControl tmpDateControl;
				WebControls.EditControls.DateTimeControl tmpDateTimeControl;
				WebControls.EditControls.Numeric tmpNumeric;
				WebControls.EditControls.FileContent tmpFileContent;
				WebControls.EditControls.FileURL tmpFileURL;
				WebControls.EditControls.FileMimetype tmpFileMimetype;
				WebControls.EditControls.HazardMatrix.HazardMatrixControl tmpHazardMatrix;
				WebControls.EditControls.YearMonthSpan tmpYearMonthSpan;
				WebControls.EditControls.WorkflowStarted tmpWorkflow;
				//ID of GUI elements corresponds to columnNames in dataTable
				Control control = FindControl(c.ColumnName);
				if (null!=control)
				{
					if (null != (tmpTextBox = control as TextBox))
					{
						
						if (c.DataType.Equals( typeof(System.Int32))) 
						{
							if (GAUtils.IsNumeric(tmpTextBox.Text))
								c.Table.Rows[0][c]= int.Parse(tmpTextBox.Text);
							else 
								c.Table.Rows[0][c] = DBNull.Value;
						}
						else if (c.DataType.Equals(typeof(System.DateTime))) 
							//support empty/null values for dates
						{
							if (tmpTextBox.Text == null || tmpTextBox.Text == string.Empty)
								c.Table.Rows[0][c] = DBNull.Value;
							else
								c.Table.Rows[0][c]= tmpTextBox.Text;
						}
						else
							c.Table.Rows[0][c]= tmpTextBox.Text;
					}
					
					//					//Free textbox
					//					if (null != (tmpFTB = control as FreeTextBoxControls.FreeTextBox))
					//					{
					//						c.Table.Rows[0][c]= tmpFTB.Text;
					//					}

					//					//cute editor
					//					if (null != (tmpCEditor = control as CuteEditor.Editor ))
					//					{
					//						c.Table.Rows[0][c]= tmpCEditor.Text;
					//					}


					if (null != (tmpCheckBox = control as CheckBox))
						c.Table.Rows[0][c]= tmpCheckBox.Checked;
					
					if (null != (tmpWorkflow = control as WebControls.EditControls.WorkflowStarted)) 
					{
						if (tmpWorkflow.Value)  //Value is true if workflow is to be started automatically
							c.Table.Rows[0][c] = Action.START_WORKFLOW_AUTOMATICALLY;
					}

					if (null != (tmpDateControl = control as WebControls.EditControls.DateControl)) 
					{
						if (!tmpDateControl.IsNull)
							c.Table.Rows[0][c]= tmpDateControl.Value;
						else
							c.Table.Rows[0][c] = System.DBNull.Value;
					}
					if (null != (tmpDateTimeControl = control as WebControls.EditControls.DateTimeControl)) 
					{
						if (!tmpDateTimeControl.IsNull)
							c.Table.Rows[0][c]= tmpDateTimeControl.Value;
						else
							c.Table.Rows[0][c] = System.DBNull.Value;
					}

					if (null != (tmpFileContent = control as WebControls.EditControls.FileContent)) 
					{
						//only update if actor has selected a new file
						if (!tmpFileContent.IsNull)
							c.Table.Rows[0][c]= tmpFileContent.Value;
					}
					if (null != (tmpFileURL = control as WebControls.EditControls.FileURL)) 
					{
						//only update if actor has selected a new file
						if (!tmpFileURL.IsNull)
							c.Table.Rows[0][c]= tmpFileURL.Value;
					}
					
					if (null != (tmpFileMimetype = control as WebControls.EditControls.FileMimetype)) 
					{
						//only update if actor has selected a new file
						if (!tmpFileMimetype.IsNull)
							c.Table.Rows[0][c]= tmpFileMimetype.Value;
					}
					
					if (null != (tmpNumeric = control as WebControls.EditControls.Numeric))
						if (!tmpNumeric.IsNull)
							c.Table.Rows[0][c]= tmpNumeric.Value;
						else
							c.Table.Rows[0][c] = System.DBNull.Value;
				
					if (null != (tmpHazardMatrix = control as WebControls.EditControls.HazardMatrix.HazardMatrixControl))
						c.Table.Rows[0][c]= tmpHazardMatrix.Text;
				
					if (null != (tmpYearMonthSpan = control as WebControls.EditControls.YearMonthSpan))
						c.Table.Rows[0][c]= tmpYearMonthSpan.Value;
				
					
					if (null != (tmpDropDownList = control as DropDownList))
						if (null!=tmpDropDownList.SelectedItem )
							if (tmpDropDownList.SelectedItem.Value == "0") //string with "0" indicates that null value is selected
								c.Table.Rows[0][c] = DBNull.Value;
							else
								c.Table.Rows[0][c] = tmpDropDownList.SelectedItem.Value;
					
					if (null != (tmpPersonnelField = control as PersonnelField))
					{
						if (tmpPersonnelField.IsForeignKeyConstraint) //Is this a foreign key personnel field?
						{
							c.Table.Rows[0][c] = tmpPersonnelField.RowId;
						}
						else  //This is a "optional" foreign key (ex witness on incidentReport)
						{
							//String rowIdColumnName = c.ColumnName + "PersonnelRowId"; 
							//c.Table.Rows[0][rowIdColumnName] = tmpPersonnelField.RowId;
							//TODO: Must make sure that there is a match between displayfield and rowid
							//TODO: Add check for related personnel. If related personnel exist, store personnelrowId	
							c.Table.Rows[0][c] = tmpPersonnelField.DisplayValue;
						}
					}
					
					if (null != (tmpRelatedDataRecordFieldMultiple = control as RelatedDataRecordFieldMultiple))
					{	//if (tmpRelatedDataRecordField.IsMultiple) 
						//{
						c.Table.Rows[0][c] = tmpRelatedDataRecordFieldMultiple.RowIds[0];
						multipleRowIds = new ArrayList(tmpRelatedDataRecordFieldMultiple.RowIds);
						duplicateColumnName = c.ColumnName;

						//} 
					}
					if (null!= (tmpRelatedDataRecordField = control as RelatedDataRecordField))
					{
						if (tmpRelatedDataRecordField.KeyValue == string.Empty) 
							c.Table.Rows[0][c] = DBNull.Value;
						else if (AppUtils.GAUtils.IsNumeric(tmpRelatedDataRecordField.KeyValue)) 
							c.Table.Rows[0][c] = tmpRelatedDataRecordField.RowId;  //tmpRelatedDataRecordField.RowId;
						else
							c.Table.Rows[0][c] = tmpRelatedDataRecordField.KeyValue;

					}
				}
			}
			if (multipleRowIds!=null) 
			{
				DuplicateDataRows(dataset.Tables[DataClass], duplicateColumnName, multipleRowIds);
			}
		}
		
		private void DuplicateDataRows(DataTable Table, string DuplicateColumnName, ArrayList RowIds) 
		{
			DataRow firstRow = Table.Rows[0];
			string DataClassRowIdName = DataClass.Substring(2)+"RowId";
			//Before we duplicate rows, we remove first entry of rowIds array. This is because the first
			//id for the first row, and we don't make another copy of this row.
			RowIds.RemoveAt(0);

			//Create new datarow duplicating data. Only the DuplicateColumnName values are different (from the RowIds array)
			foreach (int rowId in RowIds) 
			{
				
				DataRow newRow = Table.NewRow();
				newRow = fillDataRow(newRow, firstRow);

				//newRow.ItemArray = firstRow.ItemArray;
				newRow[DuplicateColumnName] = rowId;
				
				Table.Rows.Add(newRow);
				
			}
		}

		private DataRow fillDataRow(DataRow targetRow, DataRow sourceRow) 
		{
			string dataClassRowIdName = DataClass.Substring(2)+"RowId";
			
			foreach (DataColumn column in sourceRow.Table.Columns) 
			{
				//don't copy rowid field (it is autoincrement, readonly)
				if (!column.ColumnName.Equals(dataClassRowIdName)) 
				{
					targetRow[column.ColumnName] = sourceRow[column.ColumnName];
				}
			}
			return targetRow;

		}

		public void SetErrorMessage(String message)
		{
            UserMessage msg = (UserMessage)this.Page.LoadControl("~/gagui/usercontrols/usermessage.ascx");
			msg.ID = this.ID + "errormsg";
			this.Controls.AddAt(0, msg);
			msg.MessageText = message;
			msg.MessageType = UserMessage.UserMessageType.Error;
		}


	
		

		//Provide a way to store the dataset in viewState
		public DataSet RecordDataSet
		{
			get
			{
				return null==ViewState["RecordDataSet"] ? null : (DataSet) ViewState["RecordDataSet"];
			}
			set
			{
				ViewState["RecordDataSet"] = value;
			}
		}

		//Provide a way to store the dataset in viewState
		public String DataClass
		{
			get
			{
				return null==ViewState["DataClass"] ? null : (String) ViewState["DataClass"];
			}
			set
			{
				ViewState["DataClass"] = value;
			}
		}

	}
}

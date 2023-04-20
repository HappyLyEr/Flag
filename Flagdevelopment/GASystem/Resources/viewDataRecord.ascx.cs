using GASystem.WebControls.ViewControls;

namespace GASystem
{
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
	using GASystem.AppUtils;
	using System.Reflection;
	using GASystem.DataModel;
    using Telerik.WebControls;


	/// <summary>
	///		Summary description for viewDataRecord.
	/// </summary>
	public class ViewDataRecord : System.Web.UI.UserControl
	{
		const int MAX_NUMBER_OF_COLUMNS = 15;

		private DataSet _recordDataSet;
	//	protected DBauer.Web.UI.WebControls.DynamicControlsPlaceholder DCPold;
		protected System.Web.UI.WebControls.PlaceHolder DCP;
		private string _dataClass;
		private ArrayList _columnsToDisplay = new ArrayList();
        bool nextLastInRowIsAlternate = false;    //used for indicating whether the next field of class LastInRow is alternate.

        private bool formHasPages = false;
        private RadTabStrip formPagesTabStrip;
        private RadMultiPage formPagesMultiPage;


		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}


		public void SetupForm()
		{
			
				//DCP.Controls.Clear();
				generateForm();
			
		}

		private bool IsDependsOnFieldEmpty(FieldDescription FieldDesc, DataTable Table) 
		{
			if (!Table.Columns.Contains(FieldDesc.DependsOnField))
				return true;

			if (Table.Rows[0][FieldDesc.DependsOnField].ToString() == string.Empty)
				return false;
			return true;
		}


		private bool displayField(FieldDescription fd) 
		{
			if (_columnsToDisplay.Count == 0)
				return fd != null;
			return fd != null && _columnsToDisplay.Contains(fd.FieldId.ToLower());
		}

		/// <summary>
		/// Add one or more specific column to display in the Flag datagrid. Adding column here will override the default settings in 
		/// fielddefinition. If one or more columns are added will the grid only display these columns.
		/// </summary>
		/// <param name="ColumnName"></param>
		public void AddColumnToDisplay(string ColumnName) 
		{
			_columnsToDisplay.Add(ColumnName.ToLower());
		}



		private void generateForm()
		{
			if (RecordDataSet==null) return;

            formPagesTabStrip = new RadTabStrip();
            formPagesTabStrip.ID = "editform_pagesPlaceHolder";
            formPagesTabStrip.Skin = "FlagWizard";
            formPagesTabStrip.EnableViewState = false;
            DCP.Controls.Add(formPagesTabStrip);
            

			HtmlTable tbl = new HtmlTable();
			tbl.EnableViewState = true;
			tbl.ID = "Tablex";
			DCP.Controls.Add(tbl);
			//tbl.Width = "100%";
			tbl.Border = 0;
			tbl.Attributes.Add("class", "ViewForm_Table");
            tbl.CellPadding = 0;
            tbl.CellSpacing = 0;


	//		int previousRowOrderPart = -1;
            
 //test new layout

            HtmlTable currentTable = tbl;


            FieldDescription[] AllFieldDescriptions = FieldDefintion.GetFieldDescriptionsDetailsForm(DataClass);
			
            int numberOfColumns = AllFieldDescriptions.Length;
         //   bool lastField = false;

            //preparsing of elements
            ArrayList rows = new ArrayList();
            ArrayList currentRow = null;
            FieldDescription previousFd = null;

            for (int cNo = 0; cNo < numberOfColumns; cNo++)
            {
                FieldDescription fd = AllFieldDescriptions[cNo];
                if (fd != null && displayField(fd))
                {
                    if (previousFd == null)  //first row 
                    {
                        ArrayList newrow = new ArrayList();
                        rows.Add(newrow);
                        currentRow = newrow;
                    }
                    else
                    {
                        //check whether to create new row
                        if (!(fd.ColumnOrder/1000 == previousFd.ColumnOrder/1000))
                        {
                            //create new row
                            ArrayList newrow = new ArrayList();
                            rows.Add(newrow);
                            currentRow = newrow;
                        }
                    }
                    if (currentRow != null)
                    {
                        currentRow.Add(fd);
                        previousFd = fd;
                    }
                }
            }

            //add data and labes using structure found above
            foreach (ArrayList row in rows)
            {
                //check for paging in form. insert paging if control is of type formpage
                FieldDescription fd = (FieldDescription)row[0];                       //first fd in row
                DataColumn c = RecordDataSet.Tables[DataClass].Columns[fd.FieldId];   //first column in row
                if (fd.ControlType.ToUpper() == "FORMPAGE")
                   // createEmptyLabelSeparator(currentTable, fd);
                    currentTable = addNewFormPage(fd);
                else
                {
                    if (row.Count == 1)
                        AddLabelContentRow(currentTable, c, fd);
                    else if (row.Count == 2)
                    {
                        FieldDescription fd2 = (FieldDescription)row[1];
                        DataColumn c2 = RecordDataSet.Tables[DataClass].Columns[fd2.FieldId];
                        AddDoubleContentRow(currentTable, c, fd, c2, fd2);
                    }
                    else
                        for (int columnNumber = 0; columnNumber < row.Count; columnNumber++)
                        {
                            fd = (FieldDescription)row[columnNumber];
                            AddMultiRow(currentTable, RecordDataSet.Tables[DataClass].Columns[fd.FieldId],
                                columnNumber == 0, columnNumber == row.Count - 1, fd);
                        }
                }
            }
		
		}

        private HtmlTable addNewFormPage(FieldDescription fd)
        {
            //if this is the first page, add the initial RAD panel
            if (!formHasPages)
            {
                //formPagesTabStrip = new RadTabStrip();
                //formPagesTabStrip.ID = "editform_pagesPlaceHolder";
                //formPagesTabStrip.SkinID = "SimpleBarGrey";
                //DCP.Controls.Add(formPagesTabStrip);



                formPagesMultiPage = new RadMultiPage();
                formPagesMultiPage.ID = "editform_pagesPlaceHolderMultipage";
                formPagesMultiPage.EnableViewState = false;
                DCP.Controls.Add(formPagesMultiPage);
                formPagesTabStrip.MultiPageID = formPagesMultiPage.ID;


                formHasPages = true;
            }


            formPagesTabStrip.Tabs.Add(new Tab(AppUtils.Localization.GetCaptionText(fd.DataType)));
            formPagesTabStrip.SelectedIndex = 0;
            formPagesMultiPage.SelectedIndex = 0;
            PageView item = new PageView();
            item.EnableViewState = false;
            item.ID = "pageview" + fd.FieldId;

            formPagesMultiPage.PageViews.Add(item);


            HtmlTable tbl = new HtmlTable();
            tbl.ID = "Tablex" + fd.FieldId;
            item.Controls.Add(createEmptyLabelSeparator(fd));

            item.Controls.Add(tbl);

            //added by JOF, set class on table.. Moved by FW (attributes can only be added after control is added to parentcontrol)
            tbl.Attributes.Add("class", "ViewForm_Table");
            tbl.Border = 0;
            tbl.CellPadding = 0;
            tbl.CellSpacing = 0;

            return tbl;

        }  //end class


        //private void createEmptyLabelSeparator(HtmlTable currentTable, FieldDescription fd)
        //{
        //    //add row
        //    HtmlTableRow currentTableRow = new HtmlTableRow();
        //    currentTableRow.ID = currentTable.ID + fd.FieldId;
        //    currentTable.Rows.Add(currentTableRow);

        //    HtmlTableCell tblCell = new HtmlTableCell();
        //    tblCell.ID = currentTable.ID + fd.FieldId + "c";
        //    tblCell.ColSpan = MAX_NUMBER_OF_COLUMNS;
            
        //    currentTableRow.Cells.Add(tblCell);

        //    //create seperator
        //    HtmlTable tbl = new HtmlTable();
        //    tbl.ID = "editform_seperator" + fd.FieldId;
        //    tbl.Attributes.Add("class", "EditForm_Table_wizardSeparator");
        //    tbl.Border = 0;
        //    tbl.CellPadding = 0;
        //    tbl.CellSpacing = 0;

        //    HtmlTableRow tblRow;

        //    tblRow = new HtmlTableRow();
        //    tblRow.ID = tbl.ID + fd.FieldId;
        //    tbl.Rows.Add(tblRow);

        //    //label cell
        //    HtmlTableCell cell = new HtmlTableCell();
        //    cell.ID = fd.FieldId + "seperatorc1labels";
        //    tblRow.Cells.Add(cell);

        //    Label lbl = new Label();
        //    lbl.ID = "Label_seperator" + fd.FieldId;
        //    cell.Controls.Add(lbl);
        //    lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);


        //    tblCell.Controls.Add(tbl);

        //    //cell.Attributes.Add("class", "FieldViewLabelCell");

        //    return;
        //}



        private HtmlTable createEmptyLabelSeparator(FieldDescription fd)
        {
            //create rows
            HtmlTable tbl = new HtmlTable();
            tbl.ID = "editform_seperator" + fd.FieldId;
            tbl.Attributes.Add("class", "EditForm_Table_wizardSeparator");
            tbl.Border = 0;
            tbl.CellPadding = 0;
            tbl.CellSpacing = 0;

            HtmlTableRow tblRow;

            tblRow = new HtmlTableRow();
            tblRow.ID = tbl.ID + fd.FieldId;
            tbl.Rows.Add(tblRow);

            //label cell
            HtmlTableCell cell = new HtmlTableCell();
            cell.ID = fd.FieldId + "seperatorc1labels";
            tblRow.Cells.Add(cell);

            Label lbl = new Label();
            lbl.ID = "Label_seperator" + fd.FieldId;
            cell.Controls.Add(lbl);
            lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);

            //cell.Attributes.Add("class", "FieldViewLabelCell");

            return tbl;
        }






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

                if (nextLastInRowIsAlternate)
                {
                    AddLabelAndContentToRow(tblRow, c, fd, "ViewFormField_ContentLastInRow_Alternate");
                }
                else
                {
                    AddLabelAndContentToRow(tblRow, c, fd, "ViewFormField_ContentLastInRow");
                }
                nextLastInRowIsAlternate = !nextLastInRowIsAlternate;   //toggle next lastinrow
                //adjust colspan
                tblRow.Cells[tblRow.Cells.Count - 1].ColSpan = MAX_NUMBER_OF_COLUMNS - tblRow.Cells.Count + 1;



			
                ////label cell
                //HtmlTableCell cell = new HtmlTableCell();
                //cell.ID = fd.FieldId + "c1labels";
			
			
                //Label lbl = new Label();
                //lbl.ID = "Label_" + fd.FieldId;
                //cell.Controls.Add(lbl);
                //lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);
                ////lbl.EnableViewState = false;
                //tblRow.Cells.Add(cell);
                //cell.Attributes.Add("class", "FieldViewLabelCell");

                ////content cell
                //HtmlTableCell contentCell = new HtmlTableCell();
                //contentCell = new HtmlTableCell();
                //contentCell.ID = fd.FieldId + "c" + 3;

                //tblRow.Cells.Add(contentCell);
                //if (nextLastInRowIsAlternate)
                //{
                //    contentCell.Attributes.Add("class", "ViewFormField_ContentLastInRow_Alternate");
                //} else 
                //{
                //    contentCell.Attributes.Add("class", "ViewFormField_ContentLastInRow");
                //}
                //nextLastInRowIsAlternate = !nextLastInRowIsAlternate;   //toggle next lastinrow
				
                //contentCell.ColSpan = MAX_NUMBER_OF_COLUMNS - 1;
			
			
			
                ////add control	
                //AddEditControl(c, contentCell);

			}

		}

        private void AddDoubleContentRow(HtmlTable currentTable, DataColumn c1, FieldDescription fd1, DataColumn c2, FieldDescription fd2)
        {
            //add row
            HtmlTableRow tblRow;
            tblRow = new HtmlTableRow();
            tblRow.ID = currentTable.ID + c1.ColumnName;
            currentTable.Rows.Add(tblRow);

            AddLabelAndContentToRow(tblRow, c1, fd1, "ViewFormField_ContentBelowCell");
            AddLabelAndContentToRow(tblRow, c2, fd2, "ViewFormField_ContentBelowCell");
            //adjust colspan
            tblRow.Cells[tblRow.Cells.Count - 1].ColSpan = MAX_NUMBER_OF_COLUMNS - tblRow.Cells.Count + 1;

        }

        private void AddLabelAndContentToRow(HtmlTableRow currentTableRow, DataColumn c, FieldDescription fd, string contentCSS)
        {
            //label cell
            HtmlTableCell cell = new HtmlTableCell();
            cell.ID = fd.FieldId + "c1labels";


            Label lbl = new Label();
            lbl.ID = "Label_" + fd.FieldId;
            cell.Controls.Add(lbl);
            lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);
            //lbl.EnableViewState = false;
            currentTableRow.Cells.Add(cell);
            cell.Attributes.Add("class", "FieldViewLabelCell");

            //content cell
            HtmlTableCell contentCell = new HtmlTableCell();
            contentCell = new HtmlTableCell();
            contentCell.ID = fd.FieldId + "c" + 3;

            currentTableRow.Cells.Add(contentCell);
            //if (nextLastInRowIsAlternate)
            //{
                contentCell.Attributes.Add("class", contentCSS);
            //}
            //else
            //{
            //    contentCell.Attributes.Add("class", "ViewFormField_ContentLastInRow");
            //}
            //nextLastInRowIsAlternate = !nextLastInRowIsAlternate;   //toggle next lastinrow

            //contentCell.ColSpan = MAX_NUMBER_OF_COLUMNS - 1;



            //add control	
            AddEditControl(c, contentCell);
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
			
			
			Label lbl = new Label();
			lbl.ID = "Label_" + fd.FieldId;
			cell.Controls.Add(lbl);
			lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);
			//lbl.EnableViewState = false;
			tblRow.Cells.Add(cell);
			cell.Attributes.Add("class", "FieldViewLabelCell");

			//content cell
			HtmlTableCell contentCell = new HtmlTableCell();
			contentCell = new HtmlTableCell();
			contentCell.ID = fd.FieldId + "c" + 3;

			tblRowContent.Cells.Add(contentCell);
			contentCell.Attributes.Add("class", "ViewFormField_ContentBelowCell");
			
			
			if (LastInRow) 
			{
                if (tblRowContent.Cells.Count == 3)
                {


                    //add dummy cell
                    //label cell
                    HtmlTableCell dummycell = new HtmlTableCell();
                    dummycell.ID = fd.FieldId + "c1labelslastcell";



                    tblRow.Cells.Add(dummycell);
                    dummycell.Attributes.Add("class", "FieldViewLabelCell");
                    dummycell.ColSpan = MAX_NUMBER_OF_COLUMNS - tblRow.Cells.Count;
                    dummycell.Controls.Add(GASystem.GUIUtils.HTMLLiteralTags.CreateTextElement("&nbsp;"));

                    //content cell
                    HtmlTableCell contentDummyCell = new HtmlTableCell();
                    contentDummyCell = new HtmlTableCell();
                    contentDummyCell.ID = fd.FieldId + "c" + 3 + "lastcell";

                    tblRowContent.Cells.Add(contentDummyCell);
                    contentDummyCell.Attributes.Add("class", "ViewFormField_ContentBelowCell");
                    contentDummyCell.ColSpan = MAX_NUMBER_OF_COLUMNS - tblRowContent.Cells.Count;
                    contentDummyCell.Controls.Add(GASystem.GUIUtils.HTMLLiteralTags.CreateTextElement("&nbsp;"));


                }
                else
                {
                    tblRow.Cells[tblRow.Cells.Count - 1].ColSpan = MAX_NUMBER_OF_COLUMNS - tblRowContent.Cells.Count;
                    tblRowContent.Cells[tblRowContent.Cells.Count - 1].ColSpan = MAX_NUMBER_OF_COLUMNS - tblRowContent.Cells.Count;
                }



                ////content cell
                //HtmlTableCell contentDummyEndCell = new HtmlTableCell();
                //contentDummyEndCell = new HtmlTableCell();
                //contentDummyEndCell.ID = fd.FieldId + "cextralastcell";

                //tblRowContent.Cells.Add(contentDummyEndCell);
                //contentDummyEndCell.Attributes.Add("class", "dummycell");
                //contentDummyEndCell.ColSpan = MAX_NUMBER_OF_COLUMNS - tblRowContent.Cells.Count;


               
               // cell.ColSpan = MAX_NUMBER_OF_COLUMNS - tblRowContent.Cells.Count;

//				//content cell, allow content cell to expand to the right
//				tblRowContent.Cells[tblRowContent.Cells.Count - 1].Attributes.Add("class", "FieldLastInRow");
//				tblRowContent.Cells[tblRowContent.Cells.Count - 1].ColSpan = MAX_NUMBER_OF_COLUMNS - tblRowContent.Cells.Count;

			}


			
			//add control	
			AddEditControl(c, contentCell);

            nextLastInRowIsAlternate = true;

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
			
			
			Label lbl = new Label();
			lbl.ID = "Label_" + fd.FieldId;
			cell.Controls.Add(lbl);
			lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);
			//lbl.EnableViewState = false;
			tblRow.Cells.Add(cell);
			cell.Attributes.Add("class", "FieldViewLabelCell");
			cell.ColSpan = MAX_NUMBER_OF_COLUMNS;

			//content cell
			HtmlTableCell contentCell = new HtmlTableCell();
			contentCell = new HtmlTableCell();
			contentCell.ID = fd.FieldId + "c" + 3;

			tblRowContent.Cells.Add(contentCell);
            contentCell.Attributes.Add("class", "ViewFormField_SingleContentBelowCell");
			contentCell.ColSpan = MAX_NUMBER_OF_COLUMNS;
			
						
			//add control	
			AddEditControl(c, contentCell);
            nextLastInRowIsAlternate = true;


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
					Cell.Attributes.Add("class", "ViewFormField_ContentBelowCell_checkbox");
					break;
				default:
					Cell.Attributes.Add("class", "ViewFormField_ContentBelowCell");
					break;

			}
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
				{
					tblRow.Cells[numberOfCells - 1].ColSpan = 15 - numberOfCells;
					if (numberOfCells != 1)  //hack do not reset if there is only one field, textarea
                        tblRow.Cells[numberOfCells - 1].Attributes.Add("class", "ViewFormField_ContentLastInRow");

				}

//				if (numberOfCells == 1)
//					tblRow.Cells[0].ColSpan = 5;
//
//				if (numberOfCells > 1)
//					for (int nCell = numberOfCells; nCell < 5; nCell++) 
//					{
//						tblRow.Cells.Add(new HtmlTableCell());
//					}

					






			}
//
//			tblRow = tbl.Rows[tbl.Rows.Count -2];
//
//			numberOfCells = tblRow.Cells.Count;
//			tblRow.Cells[numberOfCells - 1].ColSpan = 15 - numberOfCells;
			
			
		}

		
		private Control AddEditControl(DataColumn c, Control placeHolder)
		{
			Control control;
			//Object content;
			Label label;
			Literal literal;
			WebControls.ViewControls.FileLink fileLink;
			WebControls.ViewControls.URLLink urlLink;
			WebControls.ViewControls.LocalizedLabel locLabel;
			WebControls.ViewControls.CheckBoxView checkBoxView;
			String stringContent = c.Table.Rows[0][c].ToString();//Get data from first row

            if (stringContent.Trim() == string.Empty) stringContent = "&nbsp;";


			FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
			if (null==fieldDesc) return new Control();
			
			switch(fieldDesc.ControlType.ToUpper().TrimEnd())	//jof added trimend 061005		 
			{	
					//				case "LOOKUPFIELD":
					//					control = AddLookupField(c, placeHolder);
					//					break;
				case "FILEURL":
					fileLink = new GASystem.WebControls.ViewControls.FileLink();
					fileLink.ID = c.ColumnName;
					placeHolder.Controls.Add(fileLink);
					fileLink.DocumentRowId = Convert.ToInt32(c.Table.Rows[0][this.DataClass.ToString().Substring(2) + "RowId"]);
					fileLink.DataClass = GADataRecord.ParseGADataClass(this.DataClass);
					control = (Control) fileLink;
					break;
				case "REPORTURL" :
					fileLink = new GASystem.WebControls.ViewControls.FileLink();
					fileLink.ID = c.ColumnName;
					placeHolder.Controls.Add(fileLink);
					fileLink.DocumentRowId = Convert.ToInt32(c.Table.Rows[0][this.DataClass.ToString().Substring(2) + "RowId"]);
					fileLink.DataClass = GADataRecord.ParseGADataClass(this.DataClass);
					control = (Control) fileLink;
					break;
				case "GADATACLASS":
					locLabel = new GASystem.WebControls.ViewControls.LocalizedLabel();
					locLabel.ID = c.ColumnName;
					locLabel.EnableViewState = false;
					placeHolder.Controls.Add(locLabel);
					locLabel.Text = stringContent;
					control = (Control) locLabel;
					break;
				case "CHECKBOX":
					checkBoxView = new GASystem.WebControls.ViewControls.CheckBoxView();
					checkBoxView.ID = c.ColumnName;
			
					placeHolder.Controls.Add(checkBoxView);
				    if (c.Table.Rows[0][c] != System.DBNull.Value) 
						checkBoxView.Checked = (bool)c.Table.Rows[0][c];
					else 
						checkBoxView.Checked = false;
					control = (Control)checkBoxView;
					break;
				case "TEXTAREA":
					literal = new Literal();
					literal.ID = c.ColumnName;
					placeHolder.Controls.Add(literal);
					literal.Text = "<pre class=\"FieldText_TextArea\">" + stringContent + " </pre>";
					control = (Control)literal;
					break;
				case "TEXTEDITOR":
					literal = new Literal();
					literal.ID = c.ColumnName;
					placeHolder.Controls.Add(literal);
					literal.Text = stringContent;
					control = (Control)literal;
					break;
				case "URL":
					urlLink = new GASystem.WebControls.ViewControls.URLLink();
					urlLink.ID = c.ColumnName;
					placeHolder.Controls.Add(urlLink);
					urlLink.URL = c.Table.Rows[0][c].ToString();
					control = (Control) urlLink;
					break;
				case "YEARMONTHSPAN":
					WebControls.ViewControls.YearMonthSpan ym = new GASystem.WebControls.ViewControls.YearMonthSpan();
					ym.ID = c.ColumnName;
					placeHolder.Controls.Add(ym);
					if (c.Table.Rows[0][c] != System.DBNull.Value)
						ym.Value = (int)c.Table.Rows[0][c];
					control = ym;
					break;
				case "WORKFLOWSTARTED":
					WebControls.ViewControls.WorkflowStarted ws = new GASystem.WebControls.ViewControls.WorkflowStarted();
					ws.ID = c.ColumnName;
					placeHolder.Controls.Add(ws);
					ws.EnableViewState = true;
					ws.ActionId = Convert.ToInt32(c.Table.Rows[0][this.DataClass.ToString().Substring(2) + "RowId"]);
					//if (c.Table.Rows[0][c] != System.DBNull.Value)
					ws.Value = c.Table.Rows[0][c].ToString();
					
					
					control = ws;
					break;
				case "FILELOOKUPFIELD":
					
					WebControls.ViewControls.FileLookupURL flu = new GASystem.WebControls.ViewControls.FileLookupURL();
					flu.ID = c.ColumnName;
					placeHolder.Controls.Add(flu);
                    if (c.Table.Rows[0][c] != System.DBNull.Value)
                    {
                        flu.LinkText = c.Table.Rows[0][c].ToString();
                        string rowidColumn = c.ColumnName.ToString() + "_rowid";
                        if (c.Table.Columns.Contains(rowidColumn))
                        {
                            if (c.Table.Rows[0][rowidColumn] != System.DBNull.Value)
                                flu.FileRowId = (int)c.Table.Rows[0][c.ColumnName.ToString() + "_rowid"];
                            else
                                flu.FileRowId = -1;
                        }
                    }
                    else
                    {
                        flu.LinkText = "&nbsp;";
                    }
					control = flu;
					
					break;
				case "LOOKUPFIELD":
					
					WebControls.ViewControls.RecordLookupURL rlu = new GASystem.WebControls.ViewControls.RecordLookupURL();
					rlu.ID = c.ColumnName;
					placeHolder.Controls.Add(rlu);
					if (c.Table.Rows[0][c] != System.DBNull.Value) 
					{
						rlu.LinkText = c.Table.Rows[0][c].ToString();
						string rowidColumn = c.ColumnName.ToString() + "_keyid";
						if (c.Table.Columns.Contains(rowidColumn)) 
						{
							if (c.Table.Rows[0][rowidColumn] != System.DBNull.Value)
								rlu.DataRecord = new GADataRecord((int)c.Table.Rows[0][rowidColumn], GADataRecord.ParseGADataClass(fieldDesc.LookupTable) );
							else 
								rlu.DataRecord = null;
						}
                    }
                    else
                    {
                        placeHolder.Controls.Add(GASystem.GUIUtils.HTMLLiteralTags.CreateTextElement("&nbsp;"));
                    }
					control = rlu;
					
					break;
				case "WORKITEMRESPONSIBLE":
					WebControls.ViewControls.WorkitemResponsible wir = new GASystem.WebControls.ViewControls.WorkitemResponsible();
					wir.ID = c.ColumnName;
					placeHolder.Controls.Add(wir);
					if (c.Table.Columns.Contains("ActionRowId")) 
					{
						if (c.Table.Rows[0]["ActionRowId"] != System.DBNull.Value) 
						{
							wir.ActionId = (int)c.Table.Rows[0]["ActionRowId"];
						}
					}
					wir.Participant = c.Table.Rows[0][c].ToString();
					control = (Control) wir;
					break;
				case "RESPONSIBLE":
					Responsible resp = new Responsible();
					resp.ID = c.ColumnName;
					placeHolder.Controls.Add(resp);
					resp.CssClass = fieldDesc.CssClass;
					//get dependent field
					DataColumn depCol = null;
					if (c.Table.Columns.Contains(fieldDesc.DependsOnField))
						depCol = c.Table.Columns[fieldDesc.DependsOnField];
					resp.SetResponsibles(c,depCol);
					control = resp;
					break;
				default :
					label = new Label();
					label.ID = c.ColumnName;
					
					label.EnableViewState = false;
					placeHolder.Controls.Add(label);
					//					label.CssClass = "FieldDefaultViewLabel";
					if (fieldDesc.Dataformat.ToUpper().Equals("DATE"))
						//stringContent = DateTime.Parse(stringContent).ToShortDateString();
						try 
						{
							if (fieldDesc.ControlType.ToUpper() == "DATETIME")
								//display date and time
								stringContent = Convert.ToDateTime(c.Table.Rows[0][c]).ToLongDateString() + " " + Convert.ToDateTime(c.Table.Rows[0][c]).ToShortTimeString();
							else
								stringContent = Convert.ToDateTime(c.Table.Rows[0][c]).ToLongDateString();
						} 
						catch (System.InvalidCastException e)
						{
							//could not cast to datetime, just display standard tostring();
							stringContent = c.Table.Rows[0][c].ToString();
						}
						
					if (null != stringContent)
						label.Text = stringContent + " ";

					//commentent out by JOF 11.05.06
					//Do not set width directly, use cssclass instead

//					if (fieldDesc.ControlWidth!=null && fieldDesc.ControlWidth.Length>0)
//						label.Width = new Unit(fieldDesc.ControlWidth);
//	
					control = (Control) label;
					break;
			}
			return control;
		}


		/// <summary>
		/// Create a usercontrol that displays a rowId column (display a value from a related parent table)
		/// </summary>
		/// <param name="c">This must be a integer rowId column</param>
		/// <param name="placeHolder">The Lookupfield usercontrol will be added to this control</param>
		/// <returns></returns>
		private Control AddLookupField(DataColumn c, Control placeHolder)
		{
			FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
			
			DatarecordField drf = (DatarecordField) LoadControl("DatarecordField.ascx");
			drf.ID = c.ColumnName;
			placeHolder.Controls.Add(drf);

			String stringValue = "";
			DataTable lookupTable = c.Table.DataSet.Tables[fieldDesc.LookupTable];
					
			if (lookupTable!=null)
				stringValue = lookupTable.Rows[0][fieldDesc.LookupTableDisplayValue].ToString();
					
			drf.DisplayValue = stringValue;
			drf.RowId = int.Parse(c.Table.Rows[0][c].ToString());
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
			}

		}

		private ArrayList GetListItems(FieldDescription fieldDesc)
		{
			
			ArrayList listItems = new ArrayList();
			listItems = CodeTables.GetList(fieldDesc.ListCategory, GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord);
			return listItems;
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
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		//Provide a way to store the dataset in viewState
		//		public DataSet RecordDataSet
		//		{
		//			get
		//			{
		//				return null==ViewState["RecordDataSet"] ? null : (DataSet) ViewState["RecordDataSet"];
		//			}
		//			set
		//			{
		//				ViewState["RecordDataSet"] = value;
		//			}
		//		}
		public DataSet RecordDataSet
		{
			get
			{
				return _recordDataSet;
			}
			set
			{
				_recordDataSet = value;
			}
		}

		//Provide a way to store the dataset in viewState
		public String DataClass
		{
			get
			{
				//return null==ViewState["DataClass"] ? null : (String) ViewState["DataClass"];
				return _dataClass;
			}
			set
			{
				//ViewState["DataClass"] = value;
				_dataClass = value;
			}
		}


	}
}

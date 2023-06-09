using System.Web.UI.Design;
using GASystem.GAExceptions;
using GASystem.WebControls.EditControls;
using log4net;

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
	using GASystem.GAControls;
	using GASystem.GAGUIEvents;
    using Telerik.WebControls;
    using GASystem.UserControls;
    using Action = BusinessLayer.Action;
    using System.Text;
    using System.CodeDom;

	/// <summary>
	///		Summary description for EditDataRecord.
	/// </summary>
	public class EditDataRecord : System.Web.UI.UserControl
	{
		const int MAX_NUMBER_OF_COLUMNS = 15;
		private static readonly ILog _logger = LogManager.GetLogger(typeof(EditDataRecord));

		//
		//TODO: Clientside datatype validation
		//TODO: Formulabased generation of userdefined ID's (eg. Incident ID)
		//ISSUE: When editing existing records, some fields may not be editbale according to business rules. 
		//		 How do describe this in terms of businessrules, and how do we implement support for this in GUI?
		
		private short tabIndex = 0;
		
		protected System.Web.UI.WebControls.PlaceHolder DCP;
        protected System.Web.UI.WebControls.LinkButton ButtonSave;
        protected System.Web.UI.WebControls.LinkButton ButtonCancel;
		protected System.Web.UI.WebControls.HyperLink HelpLink;
        protected System.Web.UI.WebControls.LinkButton ButtonDelete;
		protected System.Web.UI.WebControls.Button Button1;
		protected System.Web.UI.WebControls.PlaceHolder PlaceHolderSubClasses;
        //protected System.Web.UI.WebControls.HyperLink ProcedureLink;
        protected System.Web.UI.WebControls.Label classHelpText;
        protected System.Web.UI.WebControls.Table tblHelpText;

        private RadTabStrip formPagesTabStrip;
        private RadMultiPage formPagesMultiPage;




        protected System.Web.UI.WebControls.ImageButton ImgButtonSave;
        protected System.Web.UI.WebControls.ImageButton ImgButtonCancel;
        protected System.Web.UI.WebControls.ImageButton ImgButtonDelete;



		public event GACommandEventHandler EditRecordCancel;
		public event GACommandEventHandler EditRecordSave;
        public event GACommandEventHandler EditRecordDelete;

		private bool isNewRecord = false;

        private string currentControlPosition = string.Empty;
        private bool formHasPages = false;

        private Hashtable lookupTables = new Hashtable();
        private DataSet _recordSet = null;


        //recordtype and classdescription
        private int recordType = 0;
        private ClassDescription classDesc = null;
        private bool _isWorkflowTriggered = false;


		private void Page_Load(object sender, System.EventArgs e)
		{
			//if (!Page.IsPostBack)
			//	generateForm();
			ButtonCancel.Text =  GASystem.AppUtils.Localization.GetGuiElementText("Cancel");
            ButtonSave.Text = GASystem.AppUtils.Localization.GetGuiElementText("Save");
            ButtonDelete.Text = GASystem.AppUtils.Localization.GetGuiElementText("Delete");
		}

		private void DisplayUserMessageError(string Message)
		{
			try
			{
				UserMessage userMessageControl = (UserMessage) this.Page.LoadControl("~/gagui/UserControls/UserMessage.ascx");
				userMessageControl.MessageType = UserMessage.UserMessageType.Error;
				userMessageControl.MessageText = Message;
				this.Controls.Add(userMessageControl);
			}
			catch (Exception e)
			{
				_logger.Error(Localization.GetExceptionMessage("ErrorDisplayUserMessage"), e);
			}
		}
		
		public void AddSubClassForm(WebControls.EditControls.EditForm.GeneralEditForm subForm)
		{
			PlaceHolderSubClasses.Controls.Add(subForm);
		}

		public void SetupForm()
		{
			//try
			//{
				generateForm();
			//}
			//catch (GAException gaEx)
			//{
		//		DisplayUserMessageError(gaEx.Message);
		//	}
		//	catch (Exception e)
		//	{
		//		_logger.Error(e.Message, e);
		//		DisplayUserMessageError(Localization.GetExceptionMessage("UnhandledError"));
		//	}
		
	//JOF 20141019 commented out catch for simpler errro handling and testing

		}

		private void generateForm()
		{
			//set help link
			HelpLink.Text = Localization.GetGuiElementText("Help");
			HelpLink.NavigateUrl = GUIUtils.LinkUtils.GenerateHelpLink(this.DataClass);
			
			//hide delete link if new record or record has members
			SetDeleteButtonDisplay(); 
			CheckIsNewRecord();


            //add helptextbox
            ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(this.DataClass);
            classDesc = cd;
            if (cd.HelpText == string.Empty)
                tblHelpText.Visible = false;
            else
                classHelpText.Text = Localization.GetCurrentLocalizedClassHelp(this.DataClass) ?? cd.HelpText;

            //if (cd.ProcedureURL == string.Empty)
            //    ProcedureLink.Visible = false;
            //else
            //{
            //    ProcedureLink.Text = cd.ProcedureURLText;
            //    ProcedureLink.NavigateUrl = cd.ProcedureURL;
            //    ProcedureLink.Target = "_new";
            //    ProcedureLink.Visible = true;
            //}
            
            


			
			if (RecordDataSet==null) return;

            DCP.Controls.Clear();

            formPagesTabStrip = new RadTabStrip();
            formPagesTabStrip.ID = "editform_pagesPlaceHolder";
            formPagesTabStrip.Skin = "FlagWizard";
            DCP.Controls.Add(formPagesTabStrip);



			Trace.Warn("Number of rows: "+RecordDataSet.Tables[0].Rows.Count);
			HtmlTable tbl = new HtmlTable();
			tbl.ID = "Tablex";


            DCP.Controls.Add(tbl);
			
			//added by JOF, set class on table.. Moved by FW (attributes can only be added after control is added to parentcontrol)
			tbl.Attributes.Add("class", "EditForm_Table");
            tbl.Border = 0;
            tbl.CellPadding =  0;
            tbl.CellSpacing = 0;
			


			//set tabindex on buttons
		//	int previousRowOrderPart = -1;
			//int numberOfColumns = RecordDataSet.Tables[DataClass].Columns.Count;
			FieldDescription[] AllFieldDescriptions = FieldDefintion.GetFieldDescriptionsDetailsForm(DataClass, this.Owner.DataClass.ToString());
			//foreach (FieldDescription fieldDesc in FieldDefintion.GetFieldDescriptions(GADataRecord.ParseGADataClass(DataClass)))
			
            HtmlTable currentTable = tbl;

			int numberOfColumns = AllFieldDescriptions.Length;
		//	bool lastField = false;

            //preparsing of elements
            ArrayList rows = new ArrayList();
            ArrayList currentRow = null;
            FieldDescription previousFd = null; 

            for (int cNo = 0; cNo < numberOfColumns; cNo++)
            {
                FieldDescription fd = AllFieldDescriptions[cNo];
                if (fd != null && DisplayFieldOnForm(fd))
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
                        if (!(fd.ColumnOrder / 1000 == previousFd.ColumnOrder / 1000))
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
                    currentTable = addNewFormPage(fd);
                else
                {
                    if (row.Count == 1)
                    {
                        if (fd.ControlType.ToUpper() == "LABELROW" || fd.ControlType.ToUpper() == "LABELGROUPROW")
                        {
                            AddLabelRow(currentTable, c, fd);
                        }
                        else
                        {
                            AddLabelContentRow(currentTable, c, fd);
                        }
                    }
                    else
                    {
                        FieldDescription fd2 = (FieldDescription)row[1];
                        if (row.Count == 2 && !(fd.ControlType.ToUpper() == "SELECTLIST" || fd2.ControlType.ToUpper() == "SELECTLIST"))
                        {
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
            


            //for (int cNo=0; cNo <numberOfColumns ; cNo++  ) 
            //{
            //    FieldDescription fd = AllFieldDescriptions[cNo];
            //    if (fd != null && DisplayFieldOnForm(fd)) 
            //    {
				    
            //        //check for paging in form. insert paging if control is of type formpage
            //        if (fd.ControlType.ToUpper() == "FORMPAGE")
            //            currentTable = addNewFormPage(fd);
            //        else
            //        {




            //            //find where to put this field

            //            int previousRowOrder, currentRowOrder, nextRowOrder;
            //            bool inRow = false, LastInRow = false, FirstInRow = false;

            //            if (cNo == 0)
            //                previousRowOrder = int.MinValue;
            //            else
            //                previousRowOrder = AllFieldDescriptions[cNo - 1].ColumnOrder / 1000;

            //            if (cNo + 1 == numberOfColumns)
            //            {
            //                lastField = true;
            //                nextRowOrder = int.MaxValue;
            //            }
            //            else
            //                nextRowOrder = AllFieldDescriptions[cNo + 1].ColumnOrder / 1000;


            //            currentRowOrder = fd.ColumnOrder / 1000;

            //            inRow = previousRowOrder == currentRowOrder;

            //            //get data about column
            //            DataColumn c = RecordDataSet.Tables[DataClass].Columns[fd.FieldId];


            //            //add column

            //            if (inRow)
            //                LastInRow = currentRowOrder != nextRowOrder; //we are in a row, is this the last field in the row?
            //            else
            //            {
            //                FirstInRow = currentRowOrder == nextRowOrder; //we are not in a row, are we starting a new row?
            //            }

            //            if (inRow || FirstInRow)
            //            {
            //                AddMultiRow(currentTable, c, FirstInRow, LastInRow, fd);
            //            }
            //            else    // not inRow
            //            {
            //                //if textarea add siglecontent below
            //                AddLabelContentRow(currentTable, c, fd);
            //            }
            //        }
				
            //    }   //end if fd != null
				
				
            //}  // end for
				
		}

        private void AddDoubleContentRow(HtmlTable currentTable, DataColumn c1, FieldDescription fd1, DataColumn c2, FieldDescription fd2)
        {
            //add row
            HtmlTableRow tblRow;
            tblRow = new HtmlTableRow();
            tblRow.ID = currentTable.ID + c1.ColumnName;
            currentTable.Rows.Add(tblRow);

            AddLabelAndContentToRow(tblRow, c1, fd1, "FieldViewContentBelowCell");
            AddLabelAndContentToRow(tblRow, c2, fd2, "FieldViewContentBelowCell");
            //adjust colspan
            tblRow.Cells[tblRow.Cells.Count - 1].ColSpan = MAX_NUMBER_OF_COLUMNS - tblRow.Cells.Count + 1;

        }

        private void AddLabelRow(HtmlTable tbl, DataColumn c, FieldDescription fd)
        {

            //create rows

            HtmlTableRow tblRow;


            tblRow = new HtmlTableRow();
            tblRow.ID = tbl.ID + c.ColumnName;
            tbl.Rows.Add(tblRow);


            //label cell
            HtmlTableCell cell = new HtmlTableCell();
            cell.Visible = !hideBasedOnFormType(fd);
            cell.ID = fd.FieldId + "c1labels";


            Label lbl = new Label();
            lbl.ID = "Label_" + fd.FieldId;
            lbl.EnableViewState = false;
            cell.Controls.Add(lbl);
            lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);
            //lbl.EnableViewState = false;
            tblRow.Cells.Add(cell);
            if (fd.ControlType.ToUpper() == "LABELGROUPROW")
            {
                cell.Attributes.Add("class", "FieldTotalLabelCell");
            }
            else
            {
                cell.Attributes.Add("class", "FieldViewLabelCell");
            }
            cell.ColSpan = MAX_NUMBER_OF_COLUMNS;
        }

        private void AddLabelAndContentToRow(HtmlTableRow currentTableRow, DataColumn c, FieldDescription fd, string contentCSS)
        {
            //first label cell
            HtmlTableCell cell = new HtmlTableCell();
            cell.Visible = !hideBasedOnFormType(fd);
            cell.ID = fd.FieldId + "c1labels";
            
            currentTableRow.Cells.Add(cell);

            Label lbl = new Label();
            lbl.ID = "Label_" + fd.FieldId;
            cell.Controls.Add(lbl);
            lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);
            lbl.EnableViewState = true;

            cell.Attributes.Add("class", "FieldViewLabelCell");

            //first content cell
            HtmlTableCell contentCell = new HtmlTableCell();
            //contentCell = new HtmlTableCell();
            contentCell.Visible = !hideBasedOnFormType(fd);
            contentCell.ID = fd.FieldId + "c" + 3;
            currentTableRow.Cells.Add(contentCell);
            
           // contentCell.ColSpan = MAX_NUMBER_OF_COLUMNS - 1;

            //add control	
            Control editControl = AddEditControl(c, contentCell);
            if (editControl is Label)
            {
                contentCell.Attributes.Add("class", "ViewFormField_ContentBelowCell");
            }
            else
            {
                contentCell.Attributes.Add("class", contentCSS);
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
                DCP.Controls.Add(formPagesMultiPage);
                formPagesTabStrip.MultiPageID = formPagesMultiPage.ID;


                formHasPages = true;
            }


            formPagesTabStrip.Tabs.Add(new Tab(AppUtils.Localization.GetCaptionText(fd.DataType)));
            formPagesTabStrip.SelectedIndex = 0;
            formPagesMultiPage.SelectedIndex = 0;
            PageView item = new PageView();
            item.ID = "pageview" + fd.FieldId;

            formPagesMultiPage.PageViews.Add(item);

              
            HtmlTable tbl = new HtmlTable();
            tbl.ID = "Tablex" + fd.FieldId;
            item.Controls.Add(createEmptyLabelSeparator(fd));

            item.Controls.Add(tbl);

            //added by JOF, set class on table.. Moved by FW (attributes can only be added after control is added to parentcontrol)
            tbl.Attributes.Add("class", "EditForm_Table");
            tbl.Border = 0;
            tbl.CellPadding = 0;
            tbl.CellSpacing = 0;

            return tbl;

        }  //end class



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
            cell.Visible = !hideBasedOnFormType(fd);
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
            if (fd.ControlType.ToUpper().TrimEnd() == "TEXTAREA" || 
                fd.ControlType.ToUpper().TrimEnd() == "TEXTEDITOR" || 
                fd.ControlType.ToUpper().TrimEnd() == "SELECTLIST" || 
                fd.ControlType.ToUpper() == "MULTIPARTCHECKLIST" ||
                fd.ControlType.ToUpper() == "FEEDBACKEVALUATIONTABLE")
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

                AddLabelAndContentToRow(tblRow, c, fd, "FieldLastInRow");
                //adjust colspan
                tblRow.Cells[tblRow.Cells.Count - 1].ColSpan = MAX_NUMBER_OF_COLUMNS - tblRow.Cells.Count + 1;


                ////label cell
                //HtmlTableCell cell = new HtmlTableCell();
                //cell.ID = fd.FieldId + "c1labels";
                //tblRow.Cells.Add(cell);
			
                //Label lbl = new Label();
                //lbl.ID = "Label_" + fd.FieldId;
                //cell.Controls.Add(lbl);
                //lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);
                //lbl.EnableViewState = true;
				
                //cell.Attributes.Add("class", "FieldViewLabelCell");

                ////content cell
                //HtmlTableCell contentCell = new HtmlTableCell();
                //contentCell = new HtmlTableCell();
                //contentCell.ID = fd.FieldId + "c" + 3;

                //tblRow.Cells.Add(contentCell);
                //contentCell.Attributes.Add("class", "FieldLastInRow");
                //contentCell.ColSpan = MAX_NUMBER_OF_COLUMNS - 1;


                //currentControlPosition = "singlerow";
                ////add control	
                //AddEditControl(c, contentCell);

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
            cell.Visible = !hideBasedOnFormType(fd);
			cell.ID = fd.FieldId + "c1labels";
			tblRow.Cells.Add(cell);
			
			Label lbl = new Label();
			lbl.ID = "Label_" + fd.FieldId;
			cell.Controls.Add(lbl);
			lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);
			lbl.EnableViewState = true;
            if (fd.ControlType.ToUpper() == "SELECTLIST")
                cell.ColSpan = 2;

			cell.Attributes.Add("class", "FieldViewLabelCell");

			//content cell
			HtmlTableCell contentCell = new HtmlTableCell();
            contentCell.Visible = !hideBasedOnFormType(fd);
			//contentCell = new HtmlTableCell();
			contentCell.ID = fd.FieldId + "c" + 3;

			tblRowContent.Cells.Add(contentCell);
			contentCell.Attributes.Add("class", "FieldViewContentBelowCell");

            //double size on some columns
            if (fd.ControlType.ToUpper() == "SELECTLIST")
                contentCell.ColSpan = 2;

			
			if (LastInRow) 
			{
                //compute cellnumbers
                int numberOfCells = 0;               
                bool atLeastOneVisibleCell = false;
                foreach (HtmlTableCell aCell in tblRowContent.Cells)
                {
                    if (aCell.Visible)
                    {
                        atLeastOneVisibleCell = true;
                    }
                    numberOfCells += aCell.ColSpan == -1 ? 1 : aCell.ColSpan;
                }

                // Make sure that there is at least one visible cell in the row.
                // If all cells in row are hidden then do not insert the dummy cell
                if (numberOfCells == 3 && atLeastOneVisibleCell)
                {


                    //add dummy cell
                    //label cell
                    HtmlTableCell dummycell = new HtmlTableCell();
                    dummycell.ID = fd.FieldId + "c1labelslastcell";

                    Label lbldummy = new Label();
                    lbldummy.ID = "Label_" + fd.FieldId + "c1labelslastcell";
                    dummycell.Controls.Add(lbldummy);
                    lbldummy.Text = "";
                    lbldummy.EnableViewState = true;

                    tblRow.Cells.Add(dummycell);
                    dummycell.Attributes.Add("class", "FieldViewLabelCell");
                    dummycell.Controls.Add(GASystem.GUIUtils.HTMLLiteralTags.CreateTextElement("&nbsp;"));
                    //dummycell.ColSpan = MAX_NUMBER_OF_COLUMNS - tblRow.Cells.Count;


                    //content cell
                    HtmlTableCell contentFourthCell = new HtmlTableCell();
                    contentFourthCell.Visible = !hideBasedOnFormType(fd);
                    //contentFourthCell = new HtmlTableCell();
                    contentFourthCell.ID = fd.FieldId + "c" + 3 + "lastcell";
                    contentFourthCell.Controls.Add(GASystem.GUIUtils.HTMLLiteralTags.CreateTextElement("&nbsp;"));

                    tblRowContent.Cells.Add(contentFourthCell);
                    //   contentDummyCell.Attributes.Add("class", "dummycell");
                    // contentDummyCell.ColSpan = MAX_NUMBER_OF_COLUMNS - tblRowContent.Cells.Count;
                    dummycell.ColSpan = MAX_NUMBER_OF_COLUMNS - numberOfCells;
                    contentFourthCell.ColSpan = MAX_NUMBER_OF_COLUMNS - numberOfCells;

                }
                else
                {

                    contentCell.ColSpan += MAX_NUMBER_OF_COLUMNS - numberOfCells;
                    cell.ColSpan += MAX_NUMBER_OF_COLUMNS - numberOfCells;
                }
                    //content cell
                    //////HtmlTableCell contentDummyCell = new HtmlTableCell();
                    //////contentDummyCell = new HtmlTableCell();
                    //////contentDummyCell.ID = fd.FieldId + "cextralastcell";

                    //////tblRowContent.Cells.Add(contentDummyCell);
                    //////contentDummyCell.Attributes.Add("class", "dummycell");
                    //////contentDummyCell.ColSpan = MAX_NUMBER_OF_COLUMNS - tblRowContent.Cells.Count;

                    //content cell, allow content cell to expand to the right

                    //tblRowContent.Cells[tblRowContent.Cells.Count - 1].Attributes.Add("class", "FieldLastInRow");
                    //tblRowContent.Cells[tblRowContent.Cells.Count - 1].ColSpan = MAX_NUMBER_OF_COLUMNS - tblRowContent.Cells.Count;
               
                
			}

            currentControlPosition = "contentbelow";
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
            cell.Visible = !hideBasedOnFormType(fd);
			cell.ID = fd.FieldId + "c1labels";
			tblRow.Cells.Add(cell);
			
			Label lbl = new Label();
			lbl.ID = "Label_" + fd.FieldId;
			cell.Controls.Add(lbl);
			lbl.Text = AppUtils.Localization.GetCaptionText(fd.DataType);
			lbl.EnableViewState = true;

            cell.Attributes.Add("class", "FieldViewLabelCell");
			cell.ColSpan = MAX_NUMBER_OF_COLUMNS;

			//content cell
			HtmlTableCell contentCell = new HtmlTableCell();
			contentCell.Visible = !hideBasedOnFormType(fd);
			contentCell.ID = fd.FieldId + "c" + 3;

			tblRowContent.Cells.Add(contentCell);
			contentCell.Attributes.Add("class", "FieldViewSingleContentBelowCell");
			contentCell.ColSpan = MAX_NUMBER_OF_COLUMNS;

            currentControlPosition = "contentbelow";
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
			//always hide in if hide in details
			if (fd.HideInDetail)    
				return false;

            // Tor 20171203
            //bool displayInForm = true;

			//check whether to hide for new records
            // Tor 20171203 
            //if (isNewRecord)
            //    displayInForm = displayInForm && !fd.HideInNew;
            if (isNewRecord && fd.HideInNew)
                return false;

            // Tor 20180907 remarked out
            //// Tor 20171203 check whether to display based on fd.HideIfFormType
            //if (fd.HideIfFormType != string.Empty && classDesc != null && classDesc.FormTypeField != string.Empty)
            //    if (RecordDataSet != null && RecordDataSet.Tables.Count > 0)
            //        //if (RecordDataSet.Tables[0].Columns.Contains(classDesc.FormTypeField + "_keyid"))
            //        if (RecordDataSet.Tables[0].Columns.Contains(classDesc.FormTypeField))
            //            //if (RecordDataSet.Tables[0].Rows.Count > 0 && RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField + "_keyid"] != DBNull.Value)
            //            if (RecordDataSet.Tables[0].Rows.Count > 0 && RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField] != DBNull.Value)
            //                //return !fd.HideIfFormType.Contains(";" + RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField + "_keyid"] + ";");
            //                // Tor 20180831 return !fd.HideIfFormType.Contains(";" + RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField] + ";");
            //                if (fd.HideIfFormType.Contains(";" + RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField] + ";"))
            //                    return false;
            
            
            return fd != null;


            
            ////check whether to hide based on type
            //if (classDesc != null && classDesc.FormTypeField != string.Empty && fd.HideIfFormType != string.Empty)
            //    if (RecordDataSet != null && RecordDataSet.Tables.Count > 0)
            //        if (RecordDataSet.Tables[0].Columns.Contains(classDesc.FormTypeField))
            //            if (RecordDataSet.Tables[0].Rows.Count > 0 && RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField] != DBNull.Value)
            //            {
            //                string formType = "-1";
            //                //if (RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField] == DBNull.Value)
            //                //    formType = RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField, DataRowVersion.Proposed].ToString();
            //                //else
            //                    formType = RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField].ToString();


            //                displayInForm = displayInForm && !fd.HideIfFormType.Contains(";" + formType + ";");
            //            }

            // tor 20171203
            //return displayInForm;
			
		}

        private bool hideBasedOnFormType(FieldDescription fd)
        {
            bool hideField = false;
            //check whether to hide based on type
            if (classDesc != null && classDesc.FormTypeField != string.Empty && fd.HideIfFormType != string.Empty)
                if (RecordDataSet != null && RecordDataSet.Tables.Count > 0)
                    if (RecordDataSet.Tables[0].Columns.Contains(classDesc.FormTypeField))
                        if (RecordDataSet.Tables[0].Rows.Count > 0 && RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField] != DBNull.Value)
                        {
                            string formType = "-1";
                            //if (RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField] == DBNull.Value)
                            //    formType = RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField, DataRowVersion.Proposed].ToString();
                            //else
                            formType = RecordDataSet.Tables[0].Rows[0][classDesc.FormTypeField].ToString();


                            hideField = fd.HideIfFormType.Contains(";" + formType + ";");
                        }


            return hideField;
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

		//hide delete link if new record or record has members
		private void SetDeleteButtonDisplay() 
		{
			int RowId = 0;
			string RowIdName = DataClass.Substring(2) + "RowId";
			if (RecordDataSet.Tables[DataClass].Columns.Contains(RowIdName))
				RowId = (int)RecordDataSet.Tables[DataClass].Rows[0][RowIdName];
            if (RowId > 0)
            { 
                GADataRecord datarecord = new DataModel.GADataRecord(RowId, DataModel.GADataRecord.ParseGADataClass(DataClass));
                ButtonDelete.Visible = !BusinessLayer.DataClassRelations.HasMembers(datarecord);
//				ButtonDelete.Visible = !BusinessLayer.DataClassRelations.HasMembers(new DataModel.GADataRecord(RowId, DataModel.GADataRecord.ParseGADataClass(DataClass)));
                // set visible if record has no members/children
                if (ButtonDelete.Visible)
                {
                    GADataRecord owner = GASystem.BusinessLayer.DataClassRelations.GetOwner(datarecord);
                    GASystem.DataAccess.Security.GASecurityDb_new gasec = new GASystem.DataAccess.Security.GASecurityDb_new(datarecord.DataClass, null);
                    // set visible if user has delete permission
                    ButtonDelete.Visible = gasec.HasDeleteInContext(owner);
                    // Tor 20140108 added check if user has delete permission

                }

            }
            else
				ButtonDelete.Visible = false;
		}

		private void CheckIsNewRecord() 
		{
			int RowId = 0;
			string RowIdName = DataClass.Substring(2) + "RowId";
			if (RecordDataSet.Tables[DataClass].Columns.Contains(RowIdName))
				RowId = (int)RecordDataSet.Tables[DataClass].Rows[0][RowIdName];
			if (RowId > 0)
				isNewRecord = false;
			else
				isNewRecord = true;
		}

		
		private Control AddEditControl(DataColumn c, Control placeHolder)
		{
			Control control;
			Object content;
			TextBox txtBox;
			CheckBox chkBox;

           
			//FreeTextBoxControls.FreeTextBox FTB;
			String stringContent = c.Table.Rows[0][c].ToString();//Get data from first row
			Trace.Warn("stringContent: "+stringContent);

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
                // JOF 20190124 added float
                case "FLOAT":
                    WebControls.EditControls.FloatValue floatControl = new GASystem.WebControls.EditControls.FloatValue();
                    floatControl.ID = c.ColumnName;
                    placeHolder.Controls.Add(floatControl);
                    floatControl.TabIndex = tabIndex;

                    int numDigits = 2;
                    if (fieldDesc.Dataformat.StartsWith("float"))
                    {
                        try
                        {
                            numDigits = int.Parse(fieldDesc.Dataformat.Replace("float", ""));
                        }
                        catch
                        {
                            numDigits = 2;
                        }

                    }
                    floatControl.NumberBase = numDigits;


                    if (c.Table.Rows[0][c] != System.DBNull.Value)
                    {
                        floatControl.Value = Convert.ToDouble(c.Table.Rows[0][c]);
                    }
                    floatControl.ReadOnly = fieldDesc.IsReadOnly;
                    control = (Control)floatControl;
                    break;
                // JOF 20190124 added float End

				case "NUMERIC":
					WebControls.EditControls.Numeric numControl = new GASystem.WebControls.EditControls.Numeric();
					numControl.ID = c.ColumnName;
					placeHolder.Controls.Add(numControl);
					numControl.TabIndex = tabIndex;

                    if (fieldDesc.Dataformat.Equals("DECIMAL2"))
                        numControl.NumberBase = 100;

                    if (c.Table.Rows[0][c] != System.DBNull.Value )
                    {
                        numControl.Value = Convert.ToInt32(c.Table.Rows[0][c]);
                    }
                    numControl.ReadOnly = fieldDesc.IsReadOnly;
					control = (Control) numControl;
					break;
				case "DATE":
					//WebControls.EditControls.DateControl dateControl = new GASystem.WebControls.EditControls.DateControl();
                    Telerik.WebControls.RadDatePicker dateControl = new Telerik.WebControls.RadDatePicker();
                    dateControl.ID = c.ColumnName;
                   // dateControl.DateInput.CssClass = "raddatepicker";
                  
                  
                    //dateControl.CssClass = "raddatepicker";
					placeHolder.Controls.Add(dateControl);
                    //dateControl.Calendar.Skin = "FlagCalendar";
                    dateControl.Skin = "FlagCalendar";
                    dateControl.MinDate = new DateTime(1800, 1, 1);
                    //dateControl.DateInput.CssClass = "FlagCalendarInput";
					dateControl.TabIndex = tabIndex;
                   // dateControl.Height = new Unit(22);
                 //   dateControl.Width = new Unit(100);
					if (c.Table.Rows[0][c] != System.DBNull.Value)
						dateControl.SelectedDate = (DateTime)c.Table.Rows[0][c];
					

					control = (Control) dateControl;
					break;
				case "DATETIME":
                    Telerik.WebControls.RadDateTimePicker dateTimeControl = new Telerik.WebControls.RadDateTimePicker();
					//WebControls.EditControls.DateTimeControl dateTimeControl = new GASystem.WebControls.EditControls.DateTimeControl();

                    // Office settings
                    //dateTimeControl.TimeView.StartTime = new TimeSpan(7, 0, 0);
                    //dateTimeControl.TimeView.EndTime = new TimeSpan(17, 0, 0);
                    //dateTimeControl.TimeView.Interval = new TimeSpan(0, 15, 0);

                    // no settings defaults listing every whole hour from 0 to 23

                    dateTimeControl.ID = c.ColumnName;
                    //dateTimeControl.CssClass = "raddatepicker";
                  //  dateTimeControl.DateInput.CssClass = "raddatepicker";
                   // dateTimeControl.Calendar.Skin = "FlagCalendar";
                   // dateTimeControl.TimeView.Skin = "FlagCalendar";
                    //dateTimeControl.Width = new Unit(100);
                    dateTimeControl.Skin = "FlagCalendar";
                    
                  
					placeHolder.Controls.Add(dateTimeControl);
					dateTimeControl.TabIndex = tabIndex;
                    dateTimeControl.MinDate = new DateTime(1800, 1, 1);
					if (c.Table.Rows[0][c] != System.DBNull.Value)
						dateTimeControl.SelectedDate = (DateTime)c.Table.Rows[0][c];
					
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
                    if (fieldDesc.DataLength > 0)
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
					
					
					if (null != stringContent)
						txtBox.Text = stringContent;

					if (fieldDesc.ControlWidth!=null && fieldDesc.ControlWidth.Length>0)
						txtBox.Width = new Unit(fieldDesc.ControlWidth);
					else //if width is not set. Use default rule
						txtBox.Width = new Unit( (fieldDesc.DataLength<50 ? fieldDesc.DataLength: 50 )*8+"px"); //at most 7*50 pixel wide

					control = (Control) txtBox;
					AddTextBoxValidatorControl(c, placeHolder, txtBox);

					break;
                case "LOCALIZEDLABEL":
                    GASystem.WebControls.ViewControls.LocalizedLabel mylocLabel = new GASystem.WebControls.ViewControls.LocalizedLabel();
                    mylocLabel.ID = c.ColumnName;
                    mylocLabel.EnableViewState = false;
                    placeHolder.Controls.Add(mylocLabel);
                    mylocLabel.Text = stringContent;
                    control = (Control)mylocLabel;
                    break;   

				case "CHECKBOX":			  
					chkBox = new CheckBox();
					chkBox.ID = c.ColumnName;
					placeHolder.Controls.Add(chkBox);
					chkBox.CssClass = fieldDesc.CssClass;
               
					chkBox.TabIndex = tabIndex;
					if (null != stringContent)
						chkBox.Checked = "True".Equals(stringContent) || "1".Equals(stringContent);
	
					control = (Control) chkBox;
					break;

				case "TEXTAREA":
                    if (fieldDesc.IsReadOnly)
                    {
                        Literal literal = new Literal();
                        literal.ID = c.ColumnName;
                        placeHolder.Controls.Add(literal);
                        literal.Text = "<pre class=\"FieldText_TextArea\">" + stringContent + " </pre>";
                        control = (Control)literal;
                    }
                    else
                    {
                        //add editor to 'comment' column when it is 'GALists localized class text help'
                        if (fieldDesc.TableId == GADataClass.GALists.ToString()
                            && this.DataClass == GADataClass.GALists.ToString()
                            && fieldDesc.FieldId == "Comment"
                            && RecordDataSet != null && RecordDataSet.Tables.Count > 0
                            && RecordDataSet.Tables[0].TableName == GADataClass.GALists.ToString()
                            && ((string)RecordDataSet.Tables[0].Rows[0]["GAListCategory"])
                            .StartsWith(Localization.GetCurrentLocalizedHelpPrefix()))
                        {
                            control = (Control)AddRadEditorTextEditor(c, placeHolder, stringContent);
                        }
                        else
                        {

                            txtBox = new TextBox();
                            txtBox.ID = c.ColumnName;
                            placeHolder.Controls.Add(txtBox);
                            //txtBox.CssClass = fieldDesc.CssClass;
                            txtBox.CssClass = "FieldContent_TextArea";
                            txtBox.TabIndex = tabIndex;

                            if (null != stringContent)
                                txtBox.Text = stringContent;

                            txtBox.TextMode = TextBoxMode.MultiLine;


                            //limit number of characters typed
                            if (fieldDesc.DataLength > 0)
                            {
                                Label maxLabel = new Label();
                                maxLabel.CssClass = "TextBoxMaxCharatersLabel";
                                maxLabel.Text = string.Format(AppUtils.Localization.GetGuiElementText("MaxCharacters"),
                                    fieldDesc.DataLength.ToString());
                                placeHolder.Controls.Add(maxLabel);

                                int maxDataLength = fieldDesc.DataLength - 2;

                                this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), "textarea-limittext",
                                    @"
                                    function limitText(limitField, limitNum) {
                                        if (limitField.value.length > limitNum) 
                                            limitField.value = limitField.value.substring(0, limitNum);
                                    }", true);
                                txtBox.Attributes.Add("onKeyDown",
                                    "limitText(this, " + maxDataLength.ToString() + ");");
                            }

                            if (fieldDesc.ControlHeight == 0)
                                txtBox.Rows = 4; //default to 4 rows
                            else
                                txtBox.Rows = fieldDesc.ControlHeight;


                            control = (Control)txtBox;
                            AddTextBoxValidatorControl(c, placeHolder, txtBox);
                        }
                    }
					break;

			
                case "TEXTEDITOR":
                    control = (Control)AddRadEditorTextEditor(c, placeHolder, stringContent);
                    break;
				case "DROPDOWNLIST":
                    if (fieldDesc.IsReadOnly)
                        control = CreateReadOnlyDropDown(c, placeHolder);
                    else
                    {

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
                            if (null != tmpItem)
                                tmpItem.Selected = true;

                        }
                        control = (Control)ddl;
                    }
					break;

                case "POSTBACKDROPDOWNLIST":
                    if (fieldDesc.IsReadOnly)
                        control = CreateReadOnlyDropDown(c, placeHolder);
                    else
                    {

                        DropDownList ddl = new DropDownList();
                        ddl.CausesValidation = false;
                        ddl.AutoPostBack = true;

                        ddl.SelectedIndexChanged += new EventHandler(ddl_SelectedIndexChanged);

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
                            if (null != tmpItem)
                                tmpItem.Selected = true;

                        }
                        control = (Control)ddl;
                    }
                    break;

                case "MULTIPARTCHECKLIST":
                    {
                        Telerik.WebControls.RadPanelbar radPanel = new RadPanelbar();

                        if (fieldDesc.IsReadOnly)
                            radPanel.Enabled = false;

                        radPanel.ID = c.ColumnName; 
                        
                        placeHolder.Controls.Add(radPanel);
                        //radPanel.CssClass = "ViewSelectList";
                        radPanel.OnClientLoad = "onTbtChecklistPanelBarLoad"+c.ColumnName;
                        radPanel.TabIndex = tabIndex;
                        radPanel.Width = Unit.Percentage(100);
                        radPanel.Skin = "FlagMultipartCheck";
                        radPanel.PersistStateInCookie = true;

                        GASystem.DataModel.ListsDS lds = BusinessLayer.Lists.GetListsRowIdByCategory(fieldDesc.ListCategory);

                        
                        Table table = new Table();;
                        string tablePartId = "tablpart";
                        string panelPartId = "panelpart";
                        int idy = 0;
                        int pastPart = 0;

                        string result = string.Empty;
                        bool Expanded = true;
                        ListsSelectedDS.GAListsSelectedDataTable tableSelected = (ListsSelectedDS.GAListsSelectedDataTable)RecordDataSet.Tables[GADataClass.GAListsSelected.ToString()];

                        foreach (GASystem.DataModel.ListsDS.GAListsRow row in lds.GALists.Rows)
                        {
                            if (!row.IsrowguidNull() && !row.IsSort1Null())
                            {
                                result = SelectByListRowId(tableSelected, row.ListsRowId.ToString(), "nTextFree1");

                                idy = row.Sort1 / 100;

                                InitPatts(c, ref radPanel, ref table, tablePartId, panelPartId, idy, ref pastPart, ref Expanded);

                                if (pastPart == 1)
                                    Expanded &= !string.IsNullOrEmpty(result) && ("No" == result);
                                else if(pastPart == 2)
                                    Expanded &= !string.IsNullOrEmpty(result) && ("No" == result);
                               
                                TableRow newRow = new TableRow();
                                newRow = MultiPartCheckListTemplate.GetRow(row.GAListDescription, result, idy > 1);
                                newRow.ID = row.ListsRowId.ToString();
                                table.Rows.Add(newRow);
                            }
                        }

                        //Register the javaScript to manage the parts events business logic (hide and show parts on the client-side), this will only work for GAMeeting Toolbox
                        MultiPartCheckListScript
                            .RegisterScript(tablePartId, panelPartId, c.ColumnName, radPanel.OnClientLoad, this.GetType(), Page.ClientScript);

                        control = radPanel;                      
                    }

                    break;
                
                case "SELECTLIST":
                    {

                        CheckBoxList cbl = new CheckBoxList();

                        if (fieldDesc.IsReadOnly)
                            cbl.Enabled = false;

                        cbl.ID = c.ColumnName;
                        placeHolder.Controls.Add(cbl);
                        cbl.CssClass = "SELECTLIST";
                        cbl.TabIndex = tabIndex;
                        
                        

                        content = c.Table.Rows[0][c];
                        ArrayList dropDownValues = GetListItems(fieldDesc);


                        CodeTables.BindCodeTable(cbl, dropDownValues);


                         ListsSelectedDS.GAListsSelectedDataTable tableSelected = (ListsSelectedDS.GAListsSelectedDataTable)RecordDataSet.Tables[GADataClass.GAListsSelected.ToString()];

                         foreach (ListsSelectedDS.GAListsSelectedRow row in tableSelected)
                         {
                             ListItem foundItem = cbl.Items.FindByValue(row.ListsRowId.ToString());
                             if (foundItem != null)
                                 foundItem.Selected = true;
                         }
               
                  
                        control = (Control)cbl;
                    }
                    break;

				case "LOOKUPFIELD":
					//control = AddLookupField(c, placeHolder);

                    control = AddComboLookupField(c, placeHolder);
                    Telerik.WebControls.RadComboBox radcob = control as Telerik.WebControls.RadComboBox;
                    if (fieldDesc.DataType == "GroupsRowId" && fieldDesc.LookupTable == GADataClass.GAGroups.ToString())
                    {
                        if (isNewRecord)
                        {
                            radcob.AutoPostBack = true;
                            radcob.SelectedIndexChanged += MyRadCob_SelectedIndexChanged;
                        }
                        else
                        {
                            radcob.Enabled = false;
                        }
                    }
					break;				
                case "RESPONSIBLE":
					Responsible resp = new Responsible();
					resp.ID = c.ColumnName;
					placeHolder.Controls.Add(resp);
					resp.CssClass = fieldDesc.CssClass;
					resp.TabIndex = tabIndex;
					//get dependent field
					DataColumn depCol = null;
					if (c.Table.Columns.Contains(fieldDesc.DependsOnField))
						depCol = c.Table.Columns[fieldDesc.DependsOnField];
					
                    // Tor 20140128 Add record owner for siblingfilter to work
                    // resp.SetResponsibles(c,depCol);
                    resp.SetResponsibles(c, depCol, Owner);
					control = resp;
					break;

                case "LOOKUPFIELDEDIT":
                    //control = AddLookupField(c, placeHolder);

                    control = AddComboLookupField(c, placeHolder);
                    break;
                case "LOOKUPFIELDVIEW":
                    //control = AddLookupField(c, placeHolder);

                    control = AddComboLookupField(c, placeHolder);
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
                case "LOOKUPFIELDGROUPS":

                    control = AddLookupFieldMultiSelect(c, placeHolder);
                    WebControls.EditControls.ComboMultiple myCombo = control as WebControls.EditControls.ComboMultiple;

                    if (myCombo != null && !string.IsNullOrEmpty(stringContent))
                        myCombo.InitEditAtTable(stringContent);

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

                    if (c.Table.Rows[0][c] != DBNull.Value)
                        wir.Participant = c.Table.Rows[0][c].ToString();

                    //if (c.Table.Columns.Contains(fieldDesc.DependantField) && c.Table.Rows[0][fieldDesc.DependantField] != DBNull.Value)
                    //    wir.ParticipantRole = c.Table.Rows[0][fieldDesc.DependantField].ToString();

                    control = (Control)wir;
                    break;
                case "WORKITEMRESPONSIBLEROLE":
                    WebControls.ViewControls.WorkitemResponsibleRole wirr = new GASystem.WebControls.ViewControls.WorkitemResponsibleRole();
                    wirr.ID = c.ColumnName;
                    placeHolder.Controls.Add(wirr);
                    if (c.Table.Columns.Contains("ActionRowId"))
                    {
                        if (c.Table.Rows[0]["ActionRowId"] != System.DBNull.Value)
                        {
                            wirr.ActionId = (int)c.Table.Rows[0]["ActionRowId"];
                        }
                    }

                    //if (c.Table.Rows[0][c] != DBNull.Value)
                    //    wir.Participant = c.Table.Rows[0][c].ToString();

                    //if (c.Table.Rows[0][c] != DBNull.Value)
                    //    wir.Participant = c.Table.Rows[0][c].ToString();

                    if (c.Table.Rows[0][c] != DBNull.Value)
                        wirr.ParticipantRole = c.Table.Rows[0][c].ToString();
                      

                    control = (Control)wirr;
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
                case "GENERALURL":
                    GeneralURL gURL = new GeneralURL();
                    gURL.ID = c.ColumnName;
                    placeHolder.Controls.Add(gURL);
                    gURL.CssClass = fieldDesc.CssClass;
                    gURL.TabIndex = tabIndex;
                    if (null != stringContent)
                        gURL.Text = stringContent;

                    if (fieldDesc.ControlWidth != null && fieldDesc.ControlWidth.Length > 0)
                        gURL.Width = new Unit(fieldDesc.ControlWidth);

                    control = (Control)gURL;
                    break;
                case "EMAIL":
                    EmailURL eURL = new EmailURL();
                    eURL.ID = c.ColumnName;
                    placeHolder.Controls.Add(eURL);
                    eURL.CssClass = fieldDesc.CssClass;
                    eURL.TabIndex = tabIndex;
                    if (null != stringContent)
                        eURL.Text = stringContent;

                    if (fieldDesc.ControlWidth != null && fieldDesc.ControlWidth.Length > 0)
                        eURL.Width = new Unit(fieldDesc.ControlWidth);

                    
                    control = (Control)eURL;
                    AddEmailValidatorControl(c, placeHolder, eURL);
                    break;

                case "SHOWEXECUTESQLRESULT":
                    {
                        Label label = new Label();
                        label.ID = c.ColumnName;
                        label.EnableViewState = false;
                        placeHolder.Controls.Add(label);
                        GASystem.BusinessLayer.DefaultValues.DefaultValueFactory valueFactory = new GASystem.BusinessLayer.DefaultValues.DefaultValueFactory();
                        GASystem.BusinessLayer.DefaultValues.IDefaultValue defaultValue = valueFactory.MakeOnUpdate(fieldDesc, Owner, null, RecordDataSet, GADataRecord.ParseGADataClass(this.DataClass));
                        object sqlResultObj = defaultValue.GetValue();
                        if (sqlResultObj != null)
                        {
                            stringContent = sqlResultObj.ToString();
                            if (fieldDesc.Dataformat.ToUpper().Equals("DATE"))
                            {
                                try
                                {
                                    stringContent = Convert.ToDateTime(stringContent).ToLongDateString();
                                    DateTime tmpDateTime = (DateTime)sqlResultObj;
                                    if (tmpDateTime == DateTime.MinValue)
                                    {
                                        stringContent = "";
                                    }
                                    else if (fieldDesc.ControlType.ToUpper() == "DATETIME")
                                    {
                                        stringContent = Convert.ToDateTime(stringContent).ToLongDateString() + " " +
                                            Convert.ToDateTime(stringContent).ToShortTimeString();
                                    }
                                }
                                catch
                                {
                                }
                            }
                            label.Text = "&nbsp;" + stringContent;
                        }
                        control = (Control)label;
                    }
                    break;

                case "FEEDBACKEVALUATIONTABLE":
                    {
                        GASystem.DataModel.ListsDS lds = BusinessLayer.Lists.GetListsRowIdByCategory(fieldDesc.ListCategory);
                        Table table = new Table();
                        table.ID = c.ColumnName;
                        table.CssClass = "FeedbackEvaluationTable";

                        TableHeaderRow headerRow = new TableHeaderRow();
                        TableHeaderCell tdCriteriaTitle = new TableHeaderCell();
                        tdCriteriaTitle.Text = Localization.GetGuiElementText("DescriptionOfCriteria");
                        tdCriteriaTitle.Style.Add("text-align", "center");
                        tdCriteriaTitle.Style.Add("font-weight", "bold");
                        TableHeaderCell tdRankingTitle = new TableHeaderCell();
                        tdRankingTitle.Text = Localization.GetGuiElementText("Ranking");
                        tdRankingTitle.Style.Add("text-align", "center");
                        tdRankingTitle.Style.Add("font-weight", "bold");
                        TableHeaderCell tdRemarkTitle = new TableHeaderCell();
                        tdRemarkTitle.Text = Localization.GetGuiElementText("Remarks");
                        tdRemarkTitle.Style.Add("text-align", "center");
                        tdRemarkTitle.Style.Add("font-weight", "bold");
                        headerRow.Cells.Add(tdCriteriaTitle);
                        headerRow.Cells.Add(tdRankingTitle);
                        headerRow.Cells.Add(tdRemarkTitle);
                        table.Rows.Add(headerRow);

                        ListsSelectedDS.GAListsSelectedDataTable tableSelected = (ListsSelectedDS.GAListsSelectedDataTable)RecordDataSet.Tables[GADataClass.GAListsSelected.ToString()];
                        for (int i = 0; i < lds.GALists.Rows.Count; i++)
                        {
                            GASystem.DataModel.ListsDS.GAListsRow row = lds.GALists.Rows[i] as GASystem.DataModel.ListsDS.GAListsRow;
                            TableRow tableRow = new TableRow();
                            tableRow.ID = row.ListsRowId.ToString();

                            TableCell tdCriteria = new TableCell();
                            tdCriteria.Width = Unit.Percentage(50);
                            tdCriteria.Text = (i + 1).ToString() + ". " + row.GAListDescription;

                            TableCell tdRanking = new TableCell();
                            tdRanking.Style.Add("text-align", "center");
                            tdRanking.Width = Unit.Percentage(15);
                            TextBox tbRanking = new TextBox();
                            tbRanking.ID = "tbRanking" + tableRow.ID;
                            tbRanking.Width = Unit.Pixel(80);
                            tbRanking.Text = SelectByListRowId(tableSelected, row.ListsRowId.ToString(), "TextFree1");
                            tdRanking.Controls.Add(tbRanking);

                            TableCell tdRemark = new TableCell();
                            tdRemark.Style.Add("text-align", "center");
                            tdRemark.Width = Unit.Percentage(35);
                            TextBox tbRemark = new TextBox();
                            tbRemark.ID = "tbRemark" + tableRow.ID;
                            tbRemark.Width = Unit.Pixel(250);
                            tbRemark.Text = SelectByListRowId(tableSelected, row.ListsRowId.ToString(), "nTextFree1");
                            tdRemark.Controls.Add(tbRemark);

                            tableRow.Cells.Add(tdCriteria);
                            tableRow.Cells.Add(tdRanking);
                            tableRow.Cells.Add(tdRemark);
                            table.Rows.Add(tableRow);
                        }

                        TableRow footerRow = new TableRow();
                        TableCell footerCell = new TableCell();
                        footerCell.ColumnSpan = 3;
                        footerCell.Text = Localization.GetGuiElementText("RankingDescription");
                        footerRow.Cells.Add(footerCell);
                        table.Rows.Add(footerRow);

                        placeHolder.Controls.Add(table);
                        control = table;
                    }
                    break;

				default :
					control = new Control();
					break;
			}

			
			//TODO: relateddatarecord.ascx used by lookupfield does not have a element with the id of the control. 
			// are ignoring validators here. they are specified in the case statement above
			// consider rewriting relateddatarecord to a webcontrol

            //ADDED JOF, readonly control can not have validators
			
            if (!fieldDesc.IsReadOnly)
                if (!(fieldDesc.ControlType.ToUpper().Equals("LOOKUPFIELDMULTIPLE")) || !(fieldDesc.ControlType.ToUpper().Equals("LOOKUPFIELDGROUPS"))) 
			    {
				    GASystem.GUIUtils.ValidationControl.ValidationControlsCreator validationCreator = 
					    new GASystem.GUIUtils.ValidationControl.ValidationControlsCreator(placeHolder, fieldDesc);
				    validationCreator.AddAllValidatorControls(false);
			    }			
    	


			return control;
		}

        private Control AddRadEditorTextEditor(DataColumn c, Control placeHolder, string stringContent)
        {
            Telerik.WebControls.RadEditor editor = new Telerik.WebControls.RadEditor();
            editor.ID = c.ColumnName;
            editor.ToolsFile = "~/RadControls/Editor/ToolsFile.xml";
            editor.ShowSubmitCancelButtons = false;
            editor.SkinsPath = "~/RadControls/Editor/Skins/";
            editor.RadControlsDir = "~/RadControls/";
            editor.Skin = "Flag20"; //"Office2000"; // 
            editor.Width = new Unit(720);
            placeHolder.Controls.Add(editor);
            editor.TabIndex = tabIndex;

            if (Request.Browser.Browser.ToLowerInvariant() == "firefox")
            {
                System.Reflection.FieldInfo browserCheckedField = typeof(RadEditor).GetField("_browserCapabilitiesRetrieved", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                browserCheckedField.SetValue(editor, true);
                System.Reflection.FieldInfo browserSupportedField = typeof(RadEditor).GetField("_isSupportedBrowser", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                browserSupportedField.SetValue(editor, true);
            }


            //editor.CssClass = "telrikeditor";
            if (null != stringContent)
                editor.Html = stringContent;

            return editor;
        }

        private void MyRadCob_SelectedIndexChanged(object o, RadComboBoxSelectedIndexChangedEventArgs e)
        {
            string findCol = "PersonId";
            if (DataClass == "GAHazardIdentification")
            {
                findCol = "Reporter";
            }
            DataTable table = RecordDataSet.Tables[DataClass];
            Telerik.WebControls.RadComboBox myRadCob = o as Telerik.WebControls.RadComboBox;
            
            ComboMultiple myCombox = null;
            if (!table.Columns.Contains(findCol))
                return;
            Control finder = FindControl(findCol);
            if (finder != null && finder as ComboMultiple != null)
                myCombox = finder as ComboMultiple;
            else
                return;

            FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(myRadCob.ID, table.TableName);
            if (myRadCob != null && !string.IsNullOrEmpty(myRadCob.SelectedValue))
            {
                AppUtils.LookupFilterGenerator.ILookupFilter lookupFilter;
                if (Owner == null)
                    lookupFilter = new AppUtils.LookupFilterGenerator.GeneralLookupFilter();
                else
                    lookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(GADataRecord.ParseGADataClass(this.DataClass), fieldDesc.LookupTableKey, Owner, fieldDesc.LookupFilter);

                DataSet ds;
                if (this.lookupTables.ContainsKey(fieldDesc.LookupTable))
                    ds = (DataSet)lookupTables[fieldDesc.LookupTable];
                else
                {
                    ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetForDataClassWithinOwner(GADataRecord.ParseGADataClass(fieldDesc.LookupTable), new GADataRecord(1, GADataClass.GAFlag), lookupFilter.Filter, System.DateTime.MinValue, System.DateTime.MaxValue);
                    lookupTables.Add(fieldDesc.LookupTable, ds);
                }

                DataRow[] data = ds.Tables[0].Select(fieldDesc.LookupTableKey + " = " + myRadCob.SelectedValue);
                if (data != null && data.Length == 1)
                {

                    if (ds.Tables[0].Columns.Contains("MemberTable") && table.Columns.Contains(findCol) && ds.Tables[0].Columns.Contains("MemberTableListRowId") && !data[0].IsNull("MemberTableListRowId"))
                    {
                        string value = (string)data[0]["MemberTableListRowId"];
                        myCombox.InitEditAtTable(value);
                    }
                }

            }
        }

        private static void InitPatts(DataColumn c, ref RadPanelbar radPanel,  ref Table table, string tablePartId, string panelPartId, int idy, ref int pastPart, ref bool Expanded)
        {
            RadPanelItem part = null;
            RadPanelItem itemTemplate = null;
            if (idy != pastPart)
            {
                pastPart = idy;
                table = new Table();
                table.ID = tablePartId + idy;
                table.CssClass = "multichecktablelist";

                GASystem.AppUtils.Localization.GetGuiElementText("PartMultiCheck");//todo utilize
                part = new RadPanelItem();
                part.Text = Localization.GetGuiElementText("Part" + idy);
                part.Value = panelPartId + idy;
                if(pastPart == 2)
                    part.Expanded = !Expanded;
                else
                    part.Expanded = Expanded;

                itemTemplate = new RadPanelItem();
                part.Items.Add(itemTemplate);

                MultiPartCheckListTableUtil.AddPartTemplate(table, itemTemplate, part, ref radPanel);

                if (c.ColumnName == "MeetingOwner")
                {
                    switch (idy)
                    {
                        case 1:
                            radPanel.Items.Add(MultiPartCheckListTemplate.GetSeparatorItem(Localization.GetGuiElementText("Part1Tip"), true));
                            break;
                        case 2:
                            radPanel.Items.Add(
                                MultiPartCheckListTemplate.GetSeparatorItem(Localization.GetGuiElementText("Part2Tip"), true));

                            break;
                        case 3:
                            radPanel.Items.Add(MultiPartCheckListTemplate.GetSeparatorItem(Localization.GetGuiElementText("Part3Tip"), true));

                            break;
                    }
                }
                radPanel.Items.Add(MultiPartCheckListTemplate.GetSeparatorItem("&nbsp;&nbsp;", false));
            }
        }

        private string SelectByListRowId(ListsSelectedDS.GAListsSelectedDataTable tableSelected, string rowId, string columnName)
        {
            foreach (ListsSelectedDS.GAListsSelectedRow row in tableSelected)
            {
                if (row.RowState != DataRowState.Deleted && !row.IsListsRowIdNull() && row.ListsRowId.ToString() == rowId)
                {
                    return row[columnName].ToString(); 
                }
            }
            return string.Empty;
        }

        void ddl_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList abc = (DropDownList)sender;
            
            if (RecordDataSet.Tables[0].Columns.Contains( abc.ID))
                RecordDataSet.Tables[0].Rows[0][abc.ID] = abc.SelectedValue;
            generateForm();
        }


        /// <summary>
        /// Displays a readonly label for the current value in a dropdown box
        /// </summary>
        /// <param name="c"></param>
        /// <param name="placeHolder"></param>
        /// <returns></returns>
        protected virtual Control CreateReadOnlyDropDown(DataColumn c, Control placeHolder)
        {

            TextBox txtBox;
                      
            String stringContent = c.Table.Rows[0][c].ToString();//Get datfa from first row
            FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);


            txtBox = new TextBox();
            txtBox.ID = c.ColumnName;
            placeHolder.Controls.Add(txtBox);
            if (fieldDesc.CssClass == string.Empty)
                txtBox.CssClass = "input_text";
            else
                txtBox.CssClass = fieldDesc.CssClass;
            txtBox.TabIndex = tabIndex;
            //if (fieldDesc.DataLength > 0)
            //    txtBox.MaxLength = fieldDesc.DataLength;
            txtBox.ReadOnly = true;

            if (null != stringContent && AppUtils.GAUtils.IsNumeric(stringContent))
                txtBox.Text = BusinessLayer.Lists.GetListDescriptionByRowId(int.Parse(stringContent));
            else
                txtBox.Text = string.Empty;
            
            
            
            return  (Control)txtBox;
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

            return AddComboLookupField(c, placeHolder);
		}


		private Control AddPersonnelPicker(DataColumn c, Control placeHolder)
		{
			FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
			PersonnelField pField = (PersonnelField) LoadControl("PersonnelField.ascx");
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

			PersonnelField pField = (PersonnelField) LoadControl("PersonnelField.ascx");
			pField.ID = c.ColumnName;
			placeHolder.Controls.Add(pField);
			if (null!=c.Table.Rows[0][c] && 0!=(int)c.Table.Rows[0][c])
			{
				pField.RowId = int.Parse(c.Table.Rows[0][c].ToString());
				PersonnelDS personnelData = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelRowId(pField.RowId);
				if (0!=personnelData.GAPersonnel.Rows.Count)
                    pField.DisplayValue = personnelData.GAPersonnel[0].FamilyName + " " + personnelData.GAPersonnel[0].GivenName;
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


        protected Control AddComboLookupField(DataColumn c, Control placeHolder)
        {
            FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
            Telerik.WebControls.RadComboBox combo = new Telerik.WebControls.RadComboBox();

            combo.ID = c.ColumnName;
            

            if (currentControlPosition == "singlerow")
                combo.Skin = "FlagComboWide";     //combo.Width = new Unit(586);
            else
                combo.Skin = "FlagCombo";
               //combo.Width = new Unit(186);

            placeHolder.Controls.Add(combo);



            //get lookupfilter
            //string dataFilter = string.Empty;
           // GADataRecord owner = DataClassRelations.GetOwner(SessionManagement.GetCurrentDataContext().SubContextRecord);
            AppUtils.LookupFilterGenerator.ILookupFilter lookupFilter;
            if (Owner == null)
                lookupFilter = new AppUtils.LookupFilterGenerator.GeneralLookupFilter();
            else
                lookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(GADataRecord.ParseGADataClass(this.DataClass), fieldDesc.LookupTableKey, Owner, fieldDesc.LookupFilter);

            //AppUtils.LookupFilterGenerator.ILookupFilter lookupFilter;
            //if (ownerclass != string.Empty && ownerField != string.Empty)
            //{
            //    lookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(GADataRecord.ParseGADataClass(ownerclass), ownerField, SessionManagement.GetCurrentDataContext().SubContextRecord);
            //}
            //else
            //{
            //    lookupFilter = new AppUtils.LookupFilterGenerator.GeneralLookupFilter();
            //}
           
            //dataFilter = lookupFilter.Filter;

         //   BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataRecord.ParseGADataClass(fieldDesc.LookupTable));

         //   DataSet ds = bc.GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(new GADataRecord(1, GADataClass.GAFlag), System.DateTime.MinValue, System.DateTime.MaxValue, lookupFilter.Filter);

            DataSet ds;
            if (this.lookupTables.ContainsKey(fieldDesc.LookupTable))
                ds = (DataSet)lookupTables[fieldDesc.LookupTable];
            else  
            {
                ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetForDataClassWithinOwner(GADataRecord.ParseGADataClass(fieldDesc.LookupTable), new GADataRecord(1, GADataClass.GAFlag), lookupFilter.Filter, System.DateTime.MinValue, System.DateTime.MaxValue);
                lookupTables.Add(fieldDesc.LookupTable, ds);
            }
           
		    combo.AllowCustomText = false;
            
            combo.MarkFirstMatch = true;
            combo.Height = new Unit(300, UnitType.Pixel);
    //        combo.Sort = Telerik.WebControls.RadComboBoxSort.Ascending;
            string currentValueText = c.Table.Rows[0][c].ToString();
          

            String[] displayTexts = fieldDesc.LookupTableDisplayValue.Trim().Split(' ');
            ds.Tables[0].DefaultView.Sort = displayTexts[0] + " ASC";
            for (int t = 1; t < displayTexts.Length; t++)
                ds.Tables[0].DefaultView.Sort += ", " + displayTexts[t] + " ASC";

            ListsDS lstSDs = null;

            string currentLocalizedTableKey = Localization.GetCurrentLocalizedTableKey();
            if (!Localization.AvoidLocalizedListKey(Localization.GetCurrentLanguage().ToUpper()) && fieldDesc.IsLookupForTranslation)
            {
                int category = GASystem.BusinessLayer.ListCategory.GetListCategoryRowIdByName(currentLocalizedTableKey);
                if (category > -1)
                {
                    lstSDs = GASystem.BusinessLayer.Lists.GetListsRowIdByCategory(currentLocalizedTableKey);
                    lstSDs.DataSetName = fieldDesc.LookupTable;
                }
            }
            
            //add empty item to collection
            combo.Items.Add(new Telerik.WebControls.RadComboBoxItem(string.Empty, string.Empty));
            foreach (DataRowView row in ds.Tables[0].DefaultView) 
            {
                string displayText = string.Empty;
                foreach (string aValue in displayTexts)
                {
                    displayText += FindRecordInGAList(lstSDs, row[fieldDesc.LookupTableKey].ToString(), row[aValue].ToString()) + " ";
                }
                Telerik.WebControls.RadComboBoxItem item = new Telerik.WebControls.RadComboBoxItem(displayText, row[fieldDesc.LookupTableKey].ToString());
                combo.Items.Add(item);
                
            }
            combo.SelectedValue = FindRecordInGAList(lstSDs, c.Table.Rows[0][c.Table.TableName.Substring(2) + "RowId"].ToString(), currentValueText);
          
            return (Control)combo;

        }

        private string FindRecordInGAList(ListsDS lstSDs, string key, string value)
        {
            string result = value;
            if (lstSDs != null)
            {
                foreach (GASystem.DataModel.ListsDS.GAListsRow itemRow in lstSDs.GALists.Rows)
                {
                    if (!itemRow.IsGroup1Null() && lstSDs.DataSetName == itemRow.Group1
                                                && itemRow.GAListValue == key)
                    {
                        result = itemRow.GAListDescription;
                        break;
                    }
                }
            }

            return result;
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
            WebControls.EditControls.ComboMultiple myCombo = new WebControls.EditControls.ComboMultiple();
            myCombo.fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
            myCombo.ID = c.ColumnName;
            myCombo.Owner = this.Owner;
            myCombo.DataClass = this.DataClass;
            placeHolder.Controls.Add(myCombo);

            return (Control)myCombo;
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

        private void AddEmailValidatorControl(DataColumn c, Control placeHolder, EmailURL textBox)
        {

            RequiredFieldValidator val = new RequiredFieldValidator();
            val.ID = "ReqFVal_" + textBox.ID;
            placeHolder.Controls.Add(val);
            val.ControlToValidate = textBox.ID;
            val.ErrorMessage = Localization.GetErrorText("FieldRequired");
            val.EnableClientScript = false;
            
            
            //if (!c.AllowDBNull)
            ////{
            //    CustomValidator valcust = new CustomValidator();
                
            //    //RequiredFieldValidator val = new RequiredFieldValidator();
            //    valcust.ID = "ReqFVal_" + textBox.ID;
            //    placeHolder.Controls.Add(valcust);
            //    valcust.ControlToValidate = textBox.ID;
            //    valcust.ErrorMessage = "invalid email address";  //Localization.GetErrorText("FieldRequired");
            //    valcust.EnableClientScript = false;


            //    //if (textBox.Text == string.Empty)

            //        valcust.IsValid = false;
            //    //else
            //      //  valcust.IsValid = true;   
            
           // }

        }

       

       

		

		private ArrayList GetListItems(FieldDescription fieldDesc)
		{
			
			ArrayList listItems = new ArrayList();
            string filter;
            //ToDo Tor 20140123 if filter, get filter and add to paremeterlist for CodeTables.GetList 
            //if (!(fieldDesc.LookupFilter == null))
            //{
            //    filter = GASystem.AppUtils.LookupFilterGenerator.DropdownFilterFactory.Make(Owner.DataClass, fieldDesc.FieldId, Context, fieldDesc.LookupFilter);
            //}
            //listItems = CodeTables.GetList(fieldDesc.ListCategory, GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord
            //    , filter);

            // Tor 20140623 call GetList with current class and ownerclass to decide for each listitem if it is to be part of the list to display to the user
			//listItems = CodeTables.GetList(fieldDesc.ListCategory, GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord);
            listItems = CodeTables.GetList(fieldDesc.ListCategory, fieldDesc.TableId,fieldDesc.FieldId, GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord);
            
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
			InitializeComponent();
			base.OnInit(e);
            //this.EnableViewState = false;
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ButtonSave.Click += new System.EventHandler(this.ButtonSave_Click);
			this.ButtonDelete.Click += new System.EventHandler(this.ButtonDelete_Click);
			this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);

            this.ImgButtonSave.Click += new ImageClickEventHandler(this.ButtonSave_Click);
            this.ImgButtonDelete.Click += new ImageClickEventHandler(this.ButtonDelete_Click);
            this.ImgButtonCancel.Click += new ImageClickEventHandler(this.ButtonCancel_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}

       
		#endregion

		/// <summary>
		/// Copy values from formcontrols to dataset. Dataset is not saved to database
		/// </summary>
		public void UpdateRecordSet() 
		{
			SaveRecord();   //save this forms dataset
			//merge in datasets from subclassforms
			if (PlaceHolderSubClasses != null) 
			{
				foreach (Control subForm in PlaceHolderSubClasses.Controls)
				{
					GASystem.WebControls.EditControls.EditForm.GeneralEditForm tmpForm;
					if (null != (tmpForm = subForm as WebControls.EditControls.EditForm.GeneralEditForm))
					{
						tmpForm.UpdateRecordSet();
						this.RecordDataSet.Merge(tmpForm.RecordDataSet);
					}
				}
			}
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
                FieldDescription fd = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);

                if (null != fd && !fd.IsReadOnly) {
                
                TextBox tmpTextBox;
				CheckBox tmpCheckBox;
				DropDownList tmpDropDownList;
                Telerik.WebControls.RadPanelbar tmpRadPanelBar;
                CheckBoxList tmpCheckBoxList;
				PersonnelField tmpPersonnelField;
//				FreeTextBoxControls.FreeTextBox tmpFTB;
//				CuteEditor.Editor tmpCEditor;
				
				RelatedDataRecordField tmpRelatedDataRecordField;
				RelatedDataRecordFieldMultiple tmpRelatedDataRecordFieldMultiple;
				Telerik.WebControls.RadDatePicker tmpDateControl;
				//WebControls.EditControls.DateTimeControl tmpDateTimeControl;
                Telerik.WebControls.RadDateTimePicker tmpDateTimeControl;
                    // JOF 20190124 start
                WebControls.EditControls.FloatValue tmpFloat;
                    // JOF 20190124 end
				WebControls.EditControls.Numeric tmpNumeric;
                WebControls.EditControls.FileContent tmpFileContent;
				WebControls.EditControls.FileURL tmpFileURL;
				WebControls.EditControls.FileMimetype tmpFileMimetype;
				WebControls.EditControls.HazardMatrix.HazardMatrixControl tmpHazardMatrix;
				WebControls.EditControls.YearMonthSpan tmpYearMonthSpan;
				WebControls.EditControls.WorkflowStarted tmpWorkflow;
                Telerik.WebControls.RadEditor tmpEditor;
                Telerik.WebControls.RadComboBox tmpCombo;
                WebControls.EditControls.ComboMultiple tmpComboMultiple;
				Responsible tmpResponsible;
                WebControls.EditControls.GeneralURL tmpGeneralURL;
                WebControls.EditControls.EmailURL tmpEmailURL;
				//ID of GUI elements corresponds to columnNames in dataTable
                Table tmpTable;

				Control control = FindControl(c.ColumnName);
                if (null != control)
                {
                    if (null != (tmpTextBox = control as TextBox))
                    {

                        if (c.DataType.Equals(typeof(System.Int32)))
                        {
                            if (GAUtils.IsNumeric(tmpTextBox.Text))
                            {
                               
                                c.Table.Rows[0][c] = int.Parse(tmpTextBox.Text);

                            }
                            else
                                c.Table.Rows[0][c] = DBNull.Value;
                        }
                        else if (c.DataType.Equals(typeof(System.Int16)))
                        {
                            if (GAUtils.IsNumeric(tmpTextBox.Text))
                                c.Table.Rows[0][c] = Convert.ToInt16(tmpTextBox.Text);
                            else
                                c.Table.Rows[0][c] = DBNull.Value;
                        }
                        else if (c.DataType.Equals(typeof(System.Int64)))
                        {
                            if (GAUtils.IsNumeric(tmpTextBox.Text))
                                c.Table.Rows[0][c] = Convert.ToInt64(tmpTextBox.Text);
                            else
                                c.Table.Rows[0][c] = DBNull.Value;
                        }
                        else if (c.DataType.Equals(typeof(System.DateTime)))
                        //support empty/null values for dates
                        {
                            if (tmpTextBox.Text == null || tmpTextBox.Text == string.Empty)
                                c.Table.Rows[0][c] = DBNull.Value;
                            else
                                c.Table.Rows[0][c] = tmpTextBox.Text;
                        }
                        else
                            c.Table.Rows[0][c] = tmpTextBox.Text;
                    }

                    //telerik editor
                    if (null != (tmpEditor = control as Telerik.WebControls.RadEditor))
                    {
                        c.Table.Rows[0][c] = tmpEditor.Html;
                    }

                    if (null != (tmpCheckBox = control as CheckBox))
                        c.Table.Rows[0][c] = tmpCheckBox.Checked;

                    if (null != (tmpWorkflow = control as WebControls.EditControls.WorkflowStarted))
                    {
                        if (tmpWorkflow.Value)  //Value is true if workflow is to be started automatically
                            c.Table.Rows[0][c] = Action.START_WORKFLOW_AUTOMATICALLY;
                    }

                    if (null != (tmpDateControl = control as Telerik.WebControls.RadDatePicker))
                    {
                        if (!tmpDateControl.IsEmpty)
                            c.Table.Rows[0][c] = tmpDateControl.SelectedDate;
                        else
                            c.Table.Rows[0][c] = System.DBNull.Value;
                    }
                    if (null != (tmpDateTimeControl = control as Telerik.WebControls.RadDateTimePicker))
                    {
                        if (!tmpDateTimeControl.IsEmpty)
                            c.Table.Rows[0][c] = tmpDateTimeControl.SelectedDate;
                        else
                            c.Table.Rows[0][c] = System.DBNull.Value;
                    }

                    //telerik combo
                    if (null != (tmpCombo = control as Telerik.WebControls.RadComboBox))
                    {
                        if (tmpCombo.SelectedValue != string.Empty)
                            c.Table.Rows[0][c] = int.Parse(tmpCombo.SelectedValue);
                        // JOF 20180409 Store null when null record has been selected
                        else
                            c.Table.Rows[0][c] = DBNull.Value;
                    }

                    if (null != (tmpFileContent = control as WebControls.EditControls.FileContent))
                    {
                        //only update if actor has selected a new file
                        if (!tmpFileContent.IsNull)
                            c.Table.Rows[0][c] = tmpFileContent.Value;
                    }
                    if (null != (tmpFileURL = control as WebControls.EditControls.FileURL))
                    {
                        //only update if actor has selected a new file
                        if (!tmpFileURL.IsNull)
                            c.Table.Rows[0][c] = tmpFileURL.Value;
                    }


                    if (null != (tmpGeneralURL = control as WebControls.EditControls.GeneralURL))
                    {
                        if (!tmpGeneralURL.IsNull)
                            c.Table.Rows[0][c] = tmpGeneralURL.Text;
                    }

                    if (null != (tmpEmailURL = control as WebControls.EditControls.EmailURL))
                    {
                        if (!tmpEmailURL.IsNull)
                            c.Table.Rows[0][c] = tmpEmailURL.Text;
                    }

                    if (null != (tmpFileMimetype = control as WebControls.EditControls.FileMimetype))
                    {
                        //only update if actor has selected a new file
                        if (!tmpFileMimetype.IsNull)
                            c.Table.Rows[0][c] = tmpFileMimetype.Value;
                    }

                    if (null != (tmpNumeric = control as WebControls.EditControls.Numeric))
                        if (!tmpNumeric.IsNull)
                        {
                            c.Table.Rows[0][c] = tmpNumeric.Value;
                        }
                        else
                            c.Table.Rows[0][c] = System.DBNull.Value;

                    // JOF 20190124
                    if (null != (tmpFloat = control as WebControls.EditControls.FloatValue))
                        if (!tmpFloat.IsNull)
                        {
                            c.Table.Rows[0][c] = tmpFloat.Value;
                        }
                        else
                            c.Table.Rows[0][c] = System.DBNull.Value;
                    // JOF 20190124 end

                    if (null != (tmpHazardMatrix = control as WebControls.EditControls.HazardMatrix.HazardMatrixControl))
                        c.Table.Rows[0][c] = tmpHazardMatrix.Text;

                    if (null != (tmpYearMonthSpan = control as WebControls.EditControls.YearMonthSpan))
                        c.Table.Rows[0][c] = tmpYearMonthSpan.Value;


                    if (null != (tmpDropDownList = control as DropDownList))
                        if (null != tmpDropDownList.SelectedItem)
                            if (tmpDropDownList.SelectedItem.Value == "0") //string with "0" indicates that null value is selected
                            {
                                c.Table.Rows[0][c] = DBNull.Value;
                            }
                            else
                            {
                                if (this.DataClass == GADataClass.GAProject.ToString() && fd.FieldId == "CreatedBy")
                                {
                                    if (!string.IsNullOrEmpty(tmpDropDownList.SelectedItem.Value))
                                    {
                                        if (c.Table.Rows[0][c] == null)
                                        {   
                                            // phase从空到非空启动工作流
                                            _isWorkflowTriggered = true;
                                        }
                                        else if (c.Table.Rows[0][c].ToString() != tmpDropDownList.SelectedItem.Value)
                                        {
                                            // phase变化了启动工作流
                                            _isWorkflowTriggered = true;
                                        }
                                        else
                                        {
                                            // phase没变不启动工作流
                                            _isWorkflowTriggered = false;
                                        }
                                    }
                                }
                                c.Table.Rows[0][c] = tmpDropDownList.SelectedItem.Value;                                
                            }


                    if (null != (tmpCheckBoxList = control as CheckBoxList))
                    {
                        //if (null != tmpCheckBoxList.SelectedItem)
                        //{

                        //if (null != tmpCheckBoxList.SelectedItem)
                        //    c.Table.Rows[0][c] = tmpCheckBoxList.SelectedItem.Value;  //add first value to datarecordset table
                        ListsSelectedDS.GAListsSelectedDataTable tableSelected = (ListsSelectedDS.GAListsSelectedDataTable)RecordDataSet.Tables[GADataClass.GAListsSelected.ToString()];

                        tableSelected.DefaultView.RowFilter = "fieldid = '" + fd.FieldId + "'";
                        BusinessClass listSelectedBC = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAListsSelected);

                        foreach (ListItem listItem in tmpCheckBoxList.Items)
                        {
                            if (listItem.Selected)    //insert value if missing in list
                            {

                                bool itemExistsInList = false;
                                //check if item exists in selected table
                                foreach (ListsSelectedDS.GAListsSelectedRow row in tableSelected)
                                    if (row.RowState != DataRowState.Deleted && row.FieldId == c.ColumnName && row.ListsRowId.ToString() == listItem.Value)
                                        itemExistsInList = true;

                                if (!itemExistsInList)
                                {
                                    //add new row, ignore if value is not a integer
                                    int itemValue = 0;
                                    if (int.TryParse(listItem.Value, out itemValue))
                                    {
                                        ListsSelectedDS.GAListsSelectedRow newRow = (ListsSelectedDS.GAListsSelectedRow)listSelectedBC.GetNewRecord(null).Tables[0].Rows[0];  //tableSelected.NewGAListsSelectedRow();
                                        newRow.ListsRowId = itemValue;
                                        newRow.FieldId = c.ColumnName;
                                        tableSelected.ImportRow(newRow);
                                    }
                                }
                            }
                        }
                        foreach (ListItem listItem in tmpCheckBoxList.Items)
                        {
                            if (!listItem.Selected)    //remove value if it exists in list
                            {
                                foreach (ListsSelectedDS.GAListsSelectedRow row in tableSelected)
                                    if (row.RowState != DataRowState.Deleted)
                                        if (row.FieldId == c.ColumnName && row.ListsRowId.ToString() == listItem.Value)
                                            row.Delete();
                            }
                        }
                        //}
                    }


                    if (null != (tmpRadPanelBar = control as Telerik.WebControls.RadPanelbar))
                    {

                        ListsSelectedDS.GAListsSelectedDataTable tableSelected = (ListsSelectedDS.GAListsSelectedDataTable)RecordDataSet.Tables[GADataClass.GAListsSelected.ToString()];

                        tableSelected.DefaultView.RowFilter = "fieldid = '" + fd.FieldId + "'";
                        BusinessClass listSelectedBC = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAListsSelected);

                        foreach (RadPanelItem listItem in tmpRadPanelBar.Items)
                        {
                            if (listItem.Items.Count > 0 && listItem.Items[0].Controls.Count > 0)    //insert value if missing in list
                            {
                                RadPanelItem radItem = listItem.Items[0];
                                Table table = radItem.Controls[0] as Table;
                                if (table == null)
                                    continue;
                                foreach (TableRow itemRow in table.Rows)
                                {
                                    TableCell last = itemRow.Cells[itemRow.Cells.Count - 1];
                                        
                                    if(last != null)
                                    { 
                                        if (string.IsNullOrEmpty(last.ID))
                                            continue;
                                        bool itemExistsInList = false;
                                        //check if item exists in selected table
                                        foreach (ListsSelectedDS.GAListsSelectedRow row in tableSelected)
                                            if (row.RowState != DataRowState.Deleted)
                                                if (row.FieldId == c.ColumnName && row.ListsRowId.ToString() == itemRow.ID)
                                                {
                                                    if (last.ID != row.nTextFree1)
                                                    {
                                                        row.Delete();
                                                        itemExistsInList = last.ID == "N/A";
                                                    }
                                                    else {
                                                        itemExistsInList = true;
                                                    }
                                                    break;
                                                }

                                            
                                        if (!itemExistsInList)
                                        {
                                            //add new row, ignore if value is not a integer
                                            int itemValue = 0;
                                            if (int.TryParse(itemRow.ID, out itemValue))
                                            {
                                                ListsSelectedDS.GAListsSelectedRow newRow = (ListsSelectedDS.GAListsSelectedRow)listSelectedBC.GetNewRecord(null).Tables[0].Rows[0];
                                                newRow.ListsRowId = itemValue;
                                                newRow.FieldId = c.ColumnName;
                                                newRow.nTextFree1 = last.ID;
                                                tableSelected.ImportRow(newRow);
                                            }
                                        }
                                    }
                                }
                                 
                            }
                        }                 
                    }

                    if (null != (tmpResponsible = control as Responsible))
                    {
                        //get dependson field
                        //TODO add error handling
                       // FieldDescription fd = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
                        DataColumn depField = c.Table.Columns[fd.DependsOnField];
                        //set person resposible
                        if (tmpResponsible.IsResponsibleAnUser)
                            c.Table.Rows[0][c] = tmpResponsible.getResponsibleId();
                        else
                            c.Table.Rows[0][c] = DBNull.Value;
                        //set role responsible
                        if (tmpResponsible.IsResponsibleARole)
                            depField.Table.Rows[0][depField] = tmpResponsible.getResponsibleId();
                        else
                            depField.Table.Rows[0][depField] = DBNull.Value;


                    }

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
                    if (null != (tmpComboMultiple = control as WebControls.EditControls.ComboMultiple))
                    {
                        if (c.Table.TableName.Equals(GADataClass.GAGroups.ToString()))
                        {
                            FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);

                            if (c.Table.Columns.Contains("MemberTable") && fieldDesc != null)
                            {
                                c.Table.Rows[0]["MemberTable"] = fieldDesc.LookupTable;
                            }
                            StringBuilder stringBuilder = new StringBuilder();
                            foreach (int id in tmpComboMultiple.RowIds)
                                stringBuilder.AppendFormat("{0};", id);
                            c.Table.Rows[0][c] = stringBuilder.Length > 0 ? stringBuilder.ToString().TrimEnd(';') : string.Empty;

                            if (c.Table.Columns.Contains("nTextFree2"))
                            {
                                c.Table.Rows[0]["nTextFree2"] = tmpComboMultiple.Texts.Length > 0 ? string.Join(";", tmpComboMultiple.Texts) : string.Empty;
                            }
                        }
                        else if (c.Table.TableName.Equals(GADataClass.GAManageChange.ToString()))
                        {
                            FieldDescription fieldDesc = FieldDefintion.GetFieldDescription(c.ColumnName, c.Table.TableName);
                            if (c.ColumnName == "ReviewerJobTitleIds")
                            {
                                StringBuilder stringBuilder = new StringBuilder();
                                foreach (int id in tmpComboMultiple.RowIds)
                                    stringBuilder.AppendFormat("{0};", id);
                                c.Table.Rows[0][c] = stringBuilder.Length > 0 ? stringBuilder.ToString().TrimEnd(';') : string.Empty;

                                if (c.Table.Columns.Contains("ReviewerJobTitleNames"))
                                {
                                    c.Table.Rows[0]["ReviewerJobTitleNames"] = tmpComboMultiple.Texts.Length > 0 ? string.Join(";", tmpComboMultiple.Texts) : string.Empty;
                                }
                            }
                        }
                        else if (tmpComboMultiple.RowIds.Length > 0)
                        {
                            c.Table.Rows[0][c] = tmpComboMultiple.RowIds[0];
                            multipleRowIds = new ArrayList(tmpComboMultiple.RowIds);
                            duplicateColumnName = c.ColumnName;
                        }
                    }
                    if (null != (tmpRelatedDataRecordField = control as RelatedDataRecordField))
                    {
                        if (tmpRelatedDataRecordField.KeyValue == string.Empty)
                            c.Table.Rows[0][c] = DBNull.Value;
                        else if (AppUtils.GAUtils.IsNumeric(tmpRelatedDataRecordField.KeyValue))
                            c.Table.Rows[0][c] = tmpRelatedDataRecordField.RowId;  //tmpRelatedDataRecordField.RowId;
                        else
                            c.Table.Rows[0][c] = tmpRelatedDataRecordField.KeyValue;

                    }

                    // custom feedback 表格保存
                    if (c.ColumnName == "FeedbackEvaluationTable" && null != (tmpTable = control as Table))
                    {
                        ListsSelectedDS.GAListsSelectedDataTable tableSelected = (ListsSelectedDS.GAListsSelectedDataTable)RecordDataSet.Tables[GADataClass.GAListsSelected.ToString()];

                        tableSelected.DefaultView.RowFilter = "fieldid = '" + fd.FieldId + "'";
                        BusinessClass listSelectedBC = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAListsSelected);

                        foreach (TableRow tableRow in tmpTable.Rows)
                        {
                            if (!string.IsNullOrEmpty(tableRow.ID))
                            {
                                ListsSelectedDS.GAListsSelectedRow newRow = (ListsSelectedDS.GAListsSelectedRow)listSelectedBC.GetNewRecord(null).Tables[0].Rows[0];
                                foreach (ListsSelectedDS.GAListsSelectedRow row in tableSelected)
                                {
                                    if (row.RowState != DataRowState.Deleted &&
                                        row.FieldId == c.ColumnName && 
                                        row.ListsRowId.ToString() == tableRow.ID)
                                    {
                                        row.Delete();
                                        break;
                                    }
                                }                                
                                newRow.ListsRowId = int.Parse(tableRow.ID);
                                newRow.FieldId = c.ColumnName;
                                TextBox tbRanking = tableRow.FindControl("tbRanking" + tableRow.ID) as TextBox;
                                newRow.TextFree1 = tbRanking.Text;
                                TextBox tbRemark = tableRow.FindControl("tbRemark" + tableRow.ID) as TextBox;
                                newRow.nTextFree1 = tbRemark.Text;
                                tableSelected.ImportRow(newRow);
                            }                             
                        }   
                    }
                }
				}
			}
			if (multipleRowIds!=null) 
			{
				DuplicateDataRows(dataset.Tables[DataClass], duplicateColumnName, multipleRowIds);
			}
			//Personnel.Update((PersonnelDS)dataset);
		
		}

        private static bool SelectColumn(string columnName, ListsSelectedDS.GAListsSelectedDataTable tableSelected, RadPanelItem listItem)
        {
            bool itemExistsInList = false;

            //check if item exists in selected table
            foreach (ListsSelectedDS.GAListsSelectedRow row in tableSelected)
                if (row.RowState != DataRowState.Deleted && row.FieldId == columnName && row.ListsRowId.ToString() == listItem.Value)
                    itemExistsInList = true;
            return itemExistsInList;
        }

        private void DuplicateDataRows(DataTable Table, string DuplicateColumnName, ArrayList RowIds) 
		{
			DataRow firstRow = Table.Rows[0];
			string DataClassRowIdName = DataClass.Substring(2)+"RowId";
			//Before we duplicate rows, we remove first entry of rowIds array. This is because the first
			//id for the first row, and we don't make another copy of this row.
			RowIds.RemoveAt(0);
			//Table.Columns[DataClassRowIdName].AutoIncrement = true;
			//Table.Columns[DataClassRowIdName].AutoIncrementStep = -1;
			//Table.Columns[DataClassRowIdName].AutoIncrementSeed = -1;
			

			//Create new datarow duplicating data. Only the DuplicateColumnName values are different (from the RowIds array)
			foreach (int rowId in RowIds) 
			{
				//	string DataClassRowIdName = DataClass.Substring(2)+"RowId";
				//	int tmpRowId = (int) firstRow[DataClassRowIdName];
				//	tmpRowId++;
				//firstRow[DataClassRowIdName] = tmpRowId;
				
				DataRow newRow = Table.NewRow();
                newRow = newRow = BusinessClass.fillDataRow(newRow, firstRow, DataClass);

				//newRow.ItemArray = firstRow.ItemArray;
				newRow[DuplicateColumnName] = rowId;
				
				Table.Rows.Add(newRow);
				
				//Table.ImportRow(firstRow);
				//int lastRowIndex = Table.Rows.Count-1;
				//Table.Rows[lastRowIndex][DuplicateColumnName] = rowId;
			}
		}
	
		private void Button1_Click(object sender, System.EventArgs e)
		{
		
		}

		private void ButtonSave_Click(object sender, System.EventArgs e)
		{
			if (this.Page.IsValid) 
			{
				//SaveRecord();
				UpdateRecordSet();
				if (null!=EditRecordSave)
				{
					GACommandEventArgs args = new GACommandEventArgs();
					args.CommandDataSetArgument = RecordDataSet;
					args.CommandName = "Save";
					EditRecordSave(sender, args);
				}
			}
		}


		public void SetErrorMessage(String message)
		{
			UserMessage msg = new UserMessage();
			msg.ID = this.ID + "errormsg";
			this.Controls.AddAt(0, msg);
			msg.MessageText = message;
			msg.MessageType = UserMessage.UserMessageType.Error;
		}


		private void ButtonCancel_Click(object sender, System.EventArgs e)
		{
			if (null!=EditRecordCancel)
				EditRecordCancel(sender, new GACommandEventArgs());
		}

		private void ButtonDelete_Click(object sender, System.EventArgs e)
		{
			
            if (null != EditRecordDelete)
                EditRecordDelete(sender, new GACommandEventArgs());

            //JOF removed logic for deleting the record from here, pass event instead
            //GADataRecord owner = DataClassRelations.GetOwner(SessionManagement.GetCurrentDataContext().SubContextRecord);
            //BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(GADataRecord.ParseGADataClass(DataClass));
            //bc.DeleteRow((int)RecordDataSet.Tables[DataClass].Rows[0][DataClass.Substring(2) + "RowId"]);
            //if (owner == null)
            //    PageDispatcher.GotoDataRecordListPage(this.Page.Response, GADataRecord.ParseGADataClass(DataClass));
            //else
            //    PageDispatcher.GotoDataRecordViewPage(this.Page.Response, owner.DataClass, owner.RowId, null); // GADataRecord.ParseGADataClass(DataClass), SessionManagement.GetCurrentDataContext().SubContextRecord); 
			
		}

		//Provide a way to store the dataset in viewState
		public DataSet RecordDataSet
		{
			get
			{
                return _recordSet;
               // return null==ViewState["RecordDataSet"] ? null : (DataSet) ViewState["RecordDataSet"];
			}
			set
			{
                _recordSet = value;
				//ViewState["RecordDataSet"] = value;
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

        /// <summary>
        /// Owner of the current datarecord being edited. 
        /// This value is used by lookupfilters.
        /// Must be set in frontcontroller.
        /// </summary>
        public GADataRecord Owner
        {
            get
            {
                return null == ViewState["Owner"] ? null : (GADataRecord)ViewState["Owner"];
            }
            set
            {
                ViewState["Owner"] = value;
            }
        }

		public bool DisplayDeleteButton 
		{
			get {return ButtonDelete.Visible; }
			set {ButtonDelete.Visible = value; }
		}

		public string FTBToolBarLayout 
		{
			get 
			{
				string defaultToolbar = "ParagraphMenu,FontFacesMenu,FontSizesMenu,FontForeColorsMenu|Bold,Italic,Underline,Strikethrough;Superscript,Subscript,RemoveFormat|JustifyLeft,JustifyRight,JustifyCenter,JustifyFull;BulletedList,NumberedList,Indent,Outdent;CreateLink,Unlink,InsertImage,InsertRule|Cut,Copy,Paste;Undo,Redo,Print|InsertTable, EditTable, InsertTableColumnAfter, InsertTableColumnBefore, InsertTableRowAfter, InsertTableRowBefore, DeleteTableRow, DeleteTableColumn";
				if (System.Configuration.ConfigurationManager.AppSettings.Get("FTBToolBarLayout") != null)
					return System.Configuration.ConfigurationManager.AppSettings.Get("FTBToolBarLayout");
				return defaultToolbar;
			}
		}

        public bool IsWorkflowTriggeredWhenUpdated
        {
            get
            {
                return _isWorkflowTriggered && isNewRecord == false;
            }
        }

	}
}

using System;
using System.Collections.Generic;
using System.Text;
using GASystem.AppUtils;
using GASystem.BusinessLayer;
using System.Collections;
using System.Web.UI;
using GASystem.DataModel;
using System.Data;
using System.Web.UI.WebControls;

namespace GASystem.WebControls.EditControls
{
    public class ComboMultiple : System.Web.UI.WebControls.WebControl, INamingContainer
    {
        private const string LISTCSSCLASS = "ComboMultipleRecordList";
        private const string CONTROLCSSCLASS = "ComboMultipleControl";
        //private const string LISTCSSCLASS = "ComboMultipleRecordList";
        FieldDescription _fd;
        Telerik.WebControls.RadComboBox combo;
        GADataRecord _owner;
        string _dataClass;
        Panel recordlist = new Panel();

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);


            //create choose button
            Button chooseRecord = new Button();
            chooseRecord.Click += new EventHandler(chooseRecord_Click);
            chooseRecord.Text = "->";

            //add controls
            Table table = new Table();
            table.CellPadding = 0;
            table.CellSpacing = 0;
            table.CssClass = CONTROLCSSCLASS;

            TableRow tRow = new TableRow();
            table.Rows.Add(tRow);

            TableCell cList = new TableCell();
            TableCell cChoose = new TableCell();
            TableCell cRecords = new TableCell();

            cChoose.Controls.Add(chooseRecord);

            recordlist.CssClass = LISTCSSCLASS;
            cRecords.Controls.Add(recordlist);

            tRow.Cells.Add(cList);
            tRow.Cells.Add(cChoose);
            tRow.Cells.Add(cRecords);

            this.Controls.Add(table);


            //create selector

            combo = new Telerik.WebControls.RadComboBox();

            combo.ID = fieldDesc.FieldId + "combo";
            combo.Skin = "FlagCombo";
            combo.AutoPostBack = true;
            combo.SelectedIndexChanged += new Telerik.WebControls.RadComboBoxSelectedIndexChangedEventHandler(combo_SelectedIndexChanged);


            cList.Controls.Add(combo);

            AppUtils.LookupFilterGenerator.ILookupFilter lookupFilter;
            if (Owner == null)
                lookupFilter = new AppUtils.LookupFilterGenerator.GeneralLookupFilter();
            else
                lookupFilter = AppUtils.LookupFilterGenerator.LookupFilterFactory.Make(GADataRecord.ParseGADataClass(this.DataClass), fieldDesc.LookupTableKey, Owner, fieldDesc.LookupFilter);

            DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetForDataClassWithinOwner(GADataRecord.ParseGADataClass(fieldDesc.LookupTable), new GADataRecord(1, GADataClass.GAFlag), lookupFilter.Filter, System.DateTime.MinValue, System.DateTime.MaxValue);

            //BusinessClass bc = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataRecord.ParseGADataClass(fieldDesc.LookupTable));

            //DataSet ds = bc.GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(new GADataRecord(1, GADataClass.GAFlag), System.DateTime.MinValue, System.DateTime.MaxValue, string.Empty);

            combo.AllowCustomText = false;

            combo.MarkFirstMatch = true;
            combo.Height = new Unit(300, UnitType.Pixel);
            //           combo.Sort = Telerik.WebControls.RadComboBoxSort.Ascending;
            String[] displayTexts = fieldDesc.LookupTableDisplayValue.Trim().Split(' ');
            ds.Tables[0].DefaultView.Sort = displayTexts[0] + " ASC";
            for (int t = 1; t < displayTexts.Length; t++)
                ds.Tables[0].DefaultView.Sort += ", " + displayTexts[t] + " ASC";

            ArrayList listItems = CodeTables.GetList(_fd.ListCategory, _fd.TableId, _fd.FieldId,
                GASystem.AppUtils.SessionManagement.GetCurrentDataContext().SubContextRecord);
            if (listItems != null && listItems.Count > 0)
            {
                foreach (ListItem li in listItems)
                {
                    Telerik.WebControls.RadComboBoxItem item = new Telerik.WebControls.RadComboBoxItem(li.Text, li.Value);
                    combo.Items.Add(item);
                }
            }
            else
            {
                foreach (DataRowView row in ds.Tables[0].DefaultView)
                {
                    string displayText = string.Empty;
                    foreach (string aValue in displayTexts)
                    {
                        displayText += row[aValue].ToString() + " ";
                    }
                    displayText = displayText.Trim();
                    Telerik.WebControls.RadComboBoxItem item = new Telerik.WebControls.RadComboBoxItem(displayText, row[fieldDesc.LookupTableKey].ToString());
                    combo.Items.Add(item);
                }
            }
        }

        void chooseRecord_Click(object sender, EventArgs e)
        {
            int itemkey = int.Parse(combo.SelectedItem.Value);
            if (!selectedRows.ContainsKey(itemkey))
                selectedRows.Add(itemkey, combo.SelectedItem.Text);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            generateRecordList();
        }

        public void InitEditAtTable(string RowsID)
        {
            if (combo.Items.Count > 0 && !string.IsNullOrEmpty(RowsID))
            {
                foreach (string id in RowsID.Split(';'))
                {
                    int itemkey = -1;
                    if (int.TryParse(id, out itemkey))
                    {
                        foreach (Telerik.WebControls.RadComboBoxItem item in combo.Items)
                        {
                            if (id == item.Value && !selectedRows.ContainsKey(itemkey))
                            {
                                selectedRows.Add(itemkey, item.Text);
                                break;
                            }
                        }
                    }
                }
            }

        }

        void combo_SelectedIndexChanged(object o, Telerik.WebControls.RadComboBoxSelectedIndexChangedEventArgs e)
        {
            int itemkey = int.Parse(combo.SelectedItem.Value);
            if (!selectedRows.ContainsKey(itemkey))
                selectedRows.Add(itemkey, combo.SelectedItem.Text);
        }

        protected override void OnPreRender(EventArgs e)
        {
            recordlist.Controls.Clear();
            generateRecordList();

            //lblSelectedUsers.Text = string.Empty;

            //foreach (string selectedRow in selectedRows.Values)
            //    lblSelectedUsers.Text += selectedRow + " ";
            base.OnPreRender(e);

        }

        private void generateRecordList()
        {


            Table recordTable = new Table();
            recordTable.CssClass = LISTCSSCLASS;
            recordTable.BorderWidth = 0;
            recordTable.CellPadding = 0;
            recordTable.CellSpacing = 0;

            foreach (object recordKey in selectedRows.Keys)
            {
                TableRow row = new TableRow();
                TableCell valuecell = new TableCell();
                TableCell keycell = new TableCell();

                row.Cells.Add(valuecell);
                row.Cells.Add(keycell);

                valuecell.Controls.Add(GUIUtils.HTMLLiteralTags.CreateTextElement(selectedRows[recordKey].ToString()));

                ImageButton deleteRecord = new ImageButton();
                deleteRecord.ID = "recordllist" + recordKey.ToString();
                deleteRecord.Click += new ImageClickEventHandler(deleteRecord_Click);
                //deleteRecord.Click += new ImageClickEventHandler(  new EventHandler(deleteRecord_Click);
                deleteRecord.CommandArgument = recordKey.ToString();
                deleteRecord.ImageUrl = "~/images/ikon/orangeKryss.gif";

                keycell.Controls.Add(deleteRecord);
                recordTable.Rows.Add(row);
            }

            recordlist.Controls.Add(recordTable);

        }

        void deleteRecord_Click(object sender, ImageClickEventArgs e)
        {
            int key = int.Parse(((ImageButton)sender).CommandArgument);
            selectedRows.Remove(key);

        }

        public String DataClass
        {
            get { return _dataClass; }
            set { this._dataClass = value; }
        }

        public GADataRecord Owner
        {
            get { return _owner; }
            set { this._owner = value; }
        }

        public FieldDescription fieldDesc
        {
            get { return _fd; }
            set { _fd = value; }
        }

        public int[] RowIds
        {
            get
            {
                ArrayList integerRowIds = new ArrayList();
                foreach (int selectedRow in selectedRows.Keys)
                    integerRowIds.Add(selectedRow);

                return (int[])integerRowIds.ToArray(typeof(int));
            }
        }

        public string[] Texts
        {
            get
            {
                ArrayList integerRowIds = new ArrayList();
                foreach (string selectedRow in selectedRows.Values)
                    integerRowIds.Add(selectedRow);

                return (string[])integerRowIds.ToArray(typeof(string));
            }
        }

        private Hashtable selectedRows
        {
            set { this.ViewState[this.ID + "selectedrows"] = value; }
            get
            {
                if (null == this.ViewState[this.ID + "selectedrows"])
                    selectedRows = new Hashtable();
                return (Hashtable)this.ViewState[this.ID + "selectedrows"];
            }

        }

    }
}

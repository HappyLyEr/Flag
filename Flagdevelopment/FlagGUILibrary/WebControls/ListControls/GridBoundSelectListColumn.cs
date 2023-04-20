using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using System.Data;
using GASystem.DataModel;

namespace FlagGUILibrary.WebControls.ListControls
{
    class GridBoundSelectListColumn : GridBoundColumn
    {
        private string _separator = "<br/>";

        private GASystem.DataModel.GADataClass dataClass = GASystem.DataModel.GADataClass.NullClass;
        private GASystem.BusinessLayer.BusinessClass bcListsSelected;

        private Dictionary<int, ListsSelectedDS> cachedValues; 



        public GridBoundSelectListColumn() : base()
        {
            bcListsSelected = GASystem.BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAListsSelected);

            

        }

        public string Separator
        {
            get
            {
                return _separator;
            }
            set
            {
                _separator = value;
            }
        }

        public override bool ReadOnly
        {
            get
            {
                return true;
            }
            set
            {
                base.ReadOnly = value;
            }
        }

        public GASystem.DataModel.GADataClass DataClass
        {
            get
            {
                return dataClass;
            }
            set
            {
                dataClass = value;
            }
        }

        protected override string FormatDataValue(object dataValue, GridItem item)
        {
           if (!item.Page.Items.Contains("selectListPageCache"))
               item.Page.Items.Add("selectListPageCache", new Dictionary<int, ListsSelectedDS>());

           cachedValues = (Dictionary<int, ListsSelectedDS>)item.Page.Items["selectListPageCache"];

           // string val = base.FormatDataValue(dataValue, item);

            int rowid = (int)((DataRowView)item.DataItem).Row[DataClass.ToString().Substring(2) + "rowid"];

            ListsSelectedDS ds;
            if (cachedValues.ContainsKey(rowid))
                ds = cachedValues[rowid];
            else
            {
                ds = (ListsSelectedDS)bcListsSelected.GetByOwner(new GADataRecord(rowid, this.DataClass), null);
                cachedValues.Add(rowid, ds);
            }

            string val = string.Empty;
            bool firstRow = true;

            foreach (ListsSelectedDS.GAListsSelectedRow row in ds.Tables[0].Rows) 
            {
                if (row.FieldId == this.DataField)
                {
                    if (!firstRow)
                        val = val + Separator;
                    val += GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(row.ListsRowId);
                    firstRow = false;
                }
            }

            return val;
          
        }

       
    }
}

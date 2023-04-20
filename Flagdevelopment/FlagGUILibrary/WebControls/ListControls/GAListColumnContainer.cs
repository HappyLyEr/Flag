using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using GASystem.AppUtils;

namespace GASystem.WebControls.ListControls
{
	/// <summary>
	/// Base class for Columns used in GA
	/// </summary>
	public class GAListColumnContainer : System.Object, IComparable
	{
        protected FieldDescription _fieldDescription;
        protected DataColumn _c;
        protected Boolean _sortColumn;
        protected string _formattingString;
       // protected Telerik.WebControls.GridEditableColumn myColumn;
        protected Telerik.WebControls.GridColumn myColumn;

        private bool _readOnly = false;
        private string _baseURL;


       
        public GAListColumnContainer(DataColumn c, bool readOnly)
        {
            _c = c;
            _readOnly = readOnly;
        }

        public void setBaseURL(string baseURL)
        {
            _baseURL = baseURL;
        }

        public void setSortColumn(Boolean sortColumn)
        {
            _sortColumn = sortColumn;
        }

        public void setFormattingString(string formattingString)
        {
            _formattingString = formattingString;

        }

        public FieldDescription getFieldDescription()
        {
            return _fieldDescription;
        }

        public Telerik.WebControls.GridColumn GridColumn
        {
            get
            {
                return myColumn;
            }
        }


        /// <summary>
        /// Init
        /// Setting sort expression for column
        /// </summary>
        public void init()
        {
            //TODO make a factory for getting the correct type
            //Telerik.WebControls.GridBoundColumn tmpColumn = new Telerik.WebControls.GridBoundColumn();
            //Telerik.WebControls.GridBoundColumn tmpColumn = new FlagGUILibrary.WebControls.ListControls.GridBoundLimitColumn();
            
            _fieldDescription = FieldDefintion.GetFieldDescription(_c.ColumnName, _c.Table.TableName);




            


            myColumn = GAColumnItemFactory.Make(_fieldDescription, _readOnly, _baseURL);
            //if (_fieldDescription.LookupTable != null && _fieldDescription.LookupTable.Length > 0)
            //{
            //    //				this.DataField = _fieldDescription.LookupTableDisplayValue;
            //    this.SortExpression = (_sortColumn) ? _c.ColumnName + " DESC" : _c.ColumnName;
            //}
            //else
            //{
            //    this.SortExpression = (_sortColumn) ? _c.ColumnName + " DESC" : _c.ColumnName;
            //}
        }

        public System.Int32 CompareTo(System.Object obj)
        {
            if (obj == null) return -1;
            if (!(obj is GAListColumnContainer))
                return -1; //allways sort GAListColumn before other column types

            GAListColumnContainer column = (GAListColumnContainer)obj;
            return this.getFieldDescription().ColumnOrder.CompareTo(column.getFieldDescription().ColumnOrder);
        }

        //public void setItemTemplate(ITemplate template)
        //{
        //    myColumn.ItemTemplate = template;
        //}
	}
}
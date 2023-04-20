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
	public class GAListExcelColumnContainer : System.Object, IComparable
	{
        protected FieldDescription _fieldDescription;
        protected DataColumn _c;
        protected Boolean _sortColumn;
        protected string _formattingString;
       // protected Telerik.WebControls.GridEditableColumn myColumn;
        protected Telerik.WebControls.GridColumn myColumn;

        public GAListExcelColumnContainer(DataColumn c)
        {
            _c = c;
            
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

            _fieldDescription = FieldDefintion.GetFieldDescription(_c.ColumnName, _c.Table.TableName);

            myColumn = GAColumnExcelItemFactory.Make(_fieldDescription);

        }

        public System.Int32 CompareTo(System.Object obj)
        {
            if (obj == null) return -1;
            if (!(obj is GAListColumn))
                return -1; //allways sort GAListColumn before other column types

            GAListColumn column = (GAListColumn)obj;
            return this.getFieldDescription().ColumnOrder.CompareTo(column.getFieldDescription().ColumnOrder);
        }

        //public void setItemTemplate(ITemplate template)
        //{
        //    myColumn.ItemTemplate = template;
        //}
	}
}
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
	public class GAListColumn : Telerik.WebControls.GridTemplateColumn, IComparable
	{
        protected FieldDescription _fieldDescription;
        protected DataColumn _c;
        protected Boolean _sortColumn;
        protected string _formattingString;

        public GAListColumn(DataColumn c)
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



        /// <summary>
        /// Init
        /// Setting sort expression for column
        /// </summary>
        public void init()
        {
            _fieldDescription = FieldDefintion.GetFieldDescription(_c.ColumnName, _c.Table.TableName);
            this.HeaderText = AppUtils.Localization.GetCaptionText(_fieldDescription.DataType);

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
            if (!(obj is GAListColumn))
                return -1; //throw new ArgumentException();

            GAListColumn column = (GAListColumn)obj;
            return this.getFieldDescription().ColumnOrder.CompareTo(column.getFieldDescription().ColumnOrder);
        }

        public void setItemTemplate(ITemplate template)
        {
            this.ItemTemplate = template;
        }
	}
}
using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections.Specialized;
namespace GASystem.WebControls.ListControls
{
    public class GridDateItemEditorTemplate : System.Web.UI.IBindableTemplate
    {
        //Label datoDisplay;
        string _formatString;
        string _columnName;
        RadDatePicker datePicker;


        public GridDateItemEditorTemplate(string formatString, string columnName) 
        {
            _formatString = formatString;
            _columnName = columnName;

        }

        #region ITemplate Members
        public void InstantiateIn(System.Web.UI.Control container)
        {
         //   datoDisplay = new Label();
            datePicker = new RadDatePicker();
            datePicker.ID = "raddate" + _columnName;
            datePicker.Calendar.Skin = "FlagCalendar";



            //a.Text = "dato";
            datePicker.DataBinding += new EventHandler(a_DataBinding);
            //datoDisplay.DataBinding += new EventHandler(a_DataBinding);
            container.Controls.Add(datePicker);
            datePicker.MinDate = new DateTime(1800, 1, 1);
        }
        #endregion

        void a_DataBinding(object sender, EventArgs e)
        {
            RadDatePicker l = (RadDatePicker)sender;
            //l.ID = "griddate_" + _columnName;
			Telerik.WebControls.GridDataItem dgi = (Telerik.WebControls.GridDataItem)l.NamingContainer;
            
            if (((DataRowView)dgi.DataItem)[_columnName] != DBNull.Value)
            {
                DateTime val = (DateTime)((DataRowView)dgi.DataItem)[_columnName];
                 datePicker.SelectedDate = val;
            }
            
        }
    
        #region IBindableTemplate Members

        public System.Collections.Specialized.IOrderedDictionary  ExtractValues(System.Web.UI.Control container)
        {
 	        OrderedDictionary od = new OrderedDictionary();


            object myControl = (((Telerik.WebControls.GridDataItem)(container)).FindControl("raddate" + _columnName));
            if (myControl != null)
                od.Add(_columnName, ((RadDatePicker)myControl).SelectedDate);
            //od.Add(_columnName, ((Label)(((Telerik.WebControls.GridDataItem)(container)).FindControl("griddate_" + _columnName))).Text); 
            
            return od; 
        }

        #endregion
        }
}

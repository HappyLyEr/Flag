using System;
using System.Collections;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
namespace GASystem.WebControls.CustomDataGrid
{
 /// <summary>
 /// CheckBoxColumn Derives from DataGrid Column
 /// </summary>
 public class RowSelectorColumn  : DataGridColumn
 {
      public RowSelectorColumn(): base()
      {
      }
  
      public override void InitializeCell(TableCell cell, 
        int columnIndex, ListItemType itemType) 
      {
           //let the base class initialize the cell
           base.InitializeCell(cell, columnIndex, itemType);



           //we don't want to add a checkbox to the header.
           if(    itemType == ListItemType.EditItem || 
            itemType == ListItemType.Item || 
            itemType == ListItemType.AlternatingItem || 
            itemType == ListItemType.SelectedItem){

                HtmlInputCheckBox checkbox = new HtmlInputCheckBox();
                //assign an ID that we can use to find the control later
                checkbox.ID = "checkboxCol";
                cell.Controls.Add(checkbox);
           }
      }
      public Int32[] SelectedIndexes 
      {
           get 
           {
                ArrayList selectedIndexList = new ArrayList();
                //iterate each DataGridItem and find our checkbox
                foreach( DataGridItem item in this.Owner.Items ) 
                {
                     HtmlInputCheckBox chkBox = 
                        (HtmlInputCheckBox) item.FindControl("checkboxCol");
     
                     //If it's selected then add it to our ArrayList
                     if ( chkBox != null && chkBox.Checked )  
                     {
                          selectedIndexList.Add( item.ItemIndex );
                     } 
     
                }
                return (Int32[])selectedIndexList.ToArray(typeof( 
                        System.Int32 ) );
           }
      }
      public object[] SelectedDataKeys 
      {
           get 
           {
                //Just iterate each of the selectedindexes and 
                //match it up to the datakey field
                ArrayList dataKeyList = new ArrayList();
                //make sure the datakeys have some values
                if(this.Owner.DataKeys.Count > 0)
                {
                     foreach( Int32 selectedIndex in SelectedIndexes ) 
                     {
     
                          object DataKey = 
                              (this.Owner.DataKeys[selectedIndex].ToString());
                          dataKeyList.Add(DataKey);
                     }
                }
                return (object[])dataKeyList.ToArray(typeof( object ) );
           }
   
      }
   }
}
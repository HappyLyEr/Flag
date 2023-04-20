//===========================================================================
// This file was generated as part of an ASP.NET 2.0 Web project conversion.
// This code file 'App_Code\Migrated\usercontrols\Stub_listdatarecords_ascx_cs.cs' was created and contains an abstract class 
// used as a base class for the class 'Migrated_ListDataRecords' in file 'usercontrols\listdatarecords.ascx.cs'.
// This allows the the base class to be referenced by all code files in your project.
// For more information on this code pattern, please refer to http://go.microsoft.com/fwlink/?LinkId=46995 
//===========================================================================


using GASystem.GAGUI.GUIUtils;
using GASystem.WebControls.ListControls;


namespace GASystem
 {

	using System;
	using System.Collections;
	using System.Data;
	using System.Web.UI.WebControls;
	using System.Drawing;
	using GASystem.BusinessLayer; 
	using GASystem.AppUtils;
	using GASystem.GAGUIEvents;
	using log4net;

abstract public class ListDataRecords :  System.Web.UI.UserControl
{
	abstract public void ClearGrid();
	abstract public void RefreshGrid();
	abstract public void FillGrid();
	abstract public void AddFilterControl(bool SetDefaultFilter);
	abstract public string GetFilterString();
		abstract public DataSet RecordsDataSet
		{
		  get;
		  set;
		}
		abstract public string SortColumn
		{
		  get;
		  set;
		}
		abstract public bool DisplayNewButton
		{
		  get;
		  set;
		}
		abstract public bool DisplaySelectButton
		{
		  get;
		  set;
		}
		abstract public bool DisplaySelectPostBackButton
		{
		  get;
		  set;
		}
		abstract public bool DisplayEditButton
		{
		  get;
		  set;
		}
		abstract public String DataClass
		{
		  get;
		  set;
		}
		abstract public GASystem.DataModel.GADataRecord Owner
		{
		  get;
		  set;
		}
		abstract public bool DisplayFilter
		{
		  get;
		  set;
		}
		abstract public int CurrentPage 
		{
		  get;
		  set;
		}


}



}

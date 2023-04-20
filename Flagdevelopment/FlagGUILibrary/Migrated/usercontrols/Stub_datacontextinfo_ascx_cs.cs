//===========================================================================
// This file was generated as part of an ASP.NET 2.0 Web project conversion.
// This code file 'App_Code\Migrated\usercontrols\Stub_datacontextinfo_ascx_cs.cs' was created and contains an abstract class 
// used as a base class for the class 'Migrated_DataContextInfo' in file 'usercontrols\datacontextinfo.ascx.cs'.
// This allows the the base class to be referenced by all code files in your project.
// For more information on this code pattern, please refer to http://go.microsoft.com/fwlink/?LinkId=46995 
//===========================================================================




namespace GASystem
 {

	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.AppUtils;
	using GASystem.DataModel;
	using GASystem.BusinessLayer;

abstract public class DataContextInfo :  System.Web.UI.UserControl
{
	abstract public void SetupContextInfo();
		abstract public GADataClass CurrentDataClass
		{
		  get;
		  set;
		}
		abstract public GADataRecord OwnerDataRecord
		{
		  get;
		  set;
		}
		abstract public GADataRecord ContextDataRecord
		{
		  get;
		  set;
		}
		abstract public bool CurrentIsSingleRecord
		{
		  get;
		  set;
		}
		abstract public bool ViewAllRecordsWithin 
		{
		  get;
		  set;
		}
		abstract public bool NewRecord 
		{
		  get;
		  set;
		}


}



}

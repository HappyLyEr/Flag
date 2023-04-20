//===========================================================================
// This file was generated as part of an ASP.NET 2.0 Web project conversion.
// This code file 'App_Code\Migrated\gacontrols\Stub_viewdataclass_ascx_cs.cs' was created and contains an abstract class 
// used as a base class for the class 'Migrated_ViewDataClass' in file 'gacontrols\viewdataclass.ascx.cs'.
// This allows the the base class to be referenced by all code files in your project.
// For more information on this code pattern, please refer to http://go.microsoft.com/fwlink/?LinkId=46995 
//===========================================================================


using System.Collections;
using GASystem.BusinessLayer;
using GASystem.DataAccess;


namespace GASystem.GAControls
 {

	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.DataModel;
	using GASystem.GAGUIEvents;
using log4net;

abstract public class ViewDataClass :  System.Web.UI.UserControl
{
		abstract public GADataRecord Owner
		{
		  get;
		  set;
		}
		abstract public GADataRecord DataRecord
		{
		  get;
		  set;
		}
	abstract public void GenerateView();
		abstract public bool DisplayReportLink 
		{
		  get;
		  set;
		}
		abstract public bool DisplayEditLink 
		{
		  get;
		  set;
		}


}



}

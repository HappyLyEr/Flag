//===========================================================================
// This file was generated as part of an ASP.NET 2.0 Web project conversion.
// This code file 'App_Code\Migrated\gacontrols\viewform\Stub_viewworkitem_ascx_cs.cs' was created and contains an abstract class 
// used as a base class for the class 'Migrated_ViewWorkItem' in file 'gacontrols\viewform\viewworkitem.ascx.cs'.
// This allows the the base class to be referenced by all code files in your project.
// For more information on this code pattern, please refer to http://go.microsoft.com/fwlink/?LinkId=46995 
//===========================================================================




namespace GASystem.GAControls.ViewForm
 {

	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.BusinessLayer;
	using GASystem.DataModel;
	using GASystem.GAControls;
	using GASystem;
	using GASystem.AppUtils;

abstract public class ViewWorkItem :  System.Web.UI.UserControl
{
		abstract public bool HasEditPermissions 
		{
		  get;
		  set;
		}
	abstract public void SetupMyViewDataClass(int RowId);


}



}

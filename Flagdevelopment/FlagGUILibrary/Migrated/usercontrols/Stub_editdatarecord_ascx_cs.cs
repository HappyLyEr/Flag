//===========================================================================
// This file was generated as part of an ASP.NET 2.0 Web project conversion.
// This code file 'App_Code\Migrated\usercontrols\Stub_editdatarecord_ascx_cs.cs' was created and contains an abstract class 
// used as a base class for the class 'Migrated_EditDataRecord' in file 'usercontrols\editdatarecord.ascx.cs'.
// This allows the the base class to be referenced by all code files in your project.
// For more information on this code pattern, please refer to http://go.microsoft.com/fwlink/?LinkId=46995 
//===========================================================================


using GASystem.GAExceptions;
using GASystem.WebControls.EditControls;
using log4net;


namespace GASystem
 {

	using System;
	using System.Collections;
	using System.ComponentModel;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.SessionState;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.BusinessLayer;
	using GASystem.AppUtils;
	using System.Reflection;
	using GASystem.DataModel;
	using GASystem.GAControls;
	using GASystem.GAGUIEvents;

abstract public class EditDataRecord :  System.Web.UI.UserControl
{
	abstract public void AddSubClassForm(WebControls.EditControls.EditForm.GeneralEditForm subForm);
	abstract public void SetupForm();
	abstract public void UpdateRecordSet();
	abstract public void SetErrorMessage(String message);
		abstract public DataSet RecordDataSet
		{
		  get;
		  set;
		}
		abstract public String DataClass
		{
		  get;
		  set;
		}
		abstract public bool DisplayDeleteButton 
		{
		  get;
		  set;
		}
		abstract public string FTBToolBarLayout 
		{
		  get;
		}


}



}

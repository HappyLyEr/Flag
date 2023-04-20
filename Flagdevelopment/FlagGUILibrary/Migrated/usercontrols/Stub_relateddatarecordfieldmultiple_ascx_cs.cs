//===========================================================================
// This file was generated as part of an ASP.NET 2.0 Web project conversion.
// This code file 'App_Code\Migrated\usercontrols\Stub_relateddatarecordfieldmultiple_ascx_cs.cs' was created and contains an abstract class 
// used as a base class for the class 'Migrated_RelatedDataRecordFieldMultiple' in file 'usercontrols\relateddatarecordfieldmultiple.ascx.cs'.
// This allows the the base class to be referenced by all code files in your project.
// For more information on this code pattern, please refer to http://go.microsoft.com/fwlink/?LinkId=46995 
//===========================================================================




namespace GASystem
 {

	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.DataModel;
	using GASystem;
	using GASystem.AppUtils;
	using System.Collections;

abstract public class RelatedDataRecordFieldMultiple :  System.Web.UI.UserControl
{
		abstract public String DisplayValue
		{
		  get;
		  set;
		}
		abstract public String DisplayName
		{
		  get;
		  set;
		}
		abstract public GADataClass DataClass
		{
		  get;
		  set;
		}
		abstract public FieldDescription FieldDescriptionInfo 
		{
		  get;
		  set;
		}
		abstract public bool FieldRequired 
		{
		  get;
		  set;
		}
		abstract public int RowId
		{
		  get;
		  set;
		}
		abstract public bool IsMultiple
		{
		  get;
		}
		abstract public int[] RowIds
		{
		  get;
		  set;
		}
		abstract public string OwnerClass 
		{
		  get;
		  set;
		}
		abstract public string OwnerField 
		{
		  get;
		  set;
		}
		abstract public string TextValue 
		{
		  get;
		}
	abstract public void GenerateControl();


}



}

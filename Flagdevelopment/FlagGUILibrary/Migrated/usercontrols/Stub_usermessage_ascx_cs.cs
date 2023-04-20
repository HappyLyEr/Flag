//===========================================================================
// This file was generated as part of an ASP.NET 2.0 Web project conversion.
// This code file 'App_Code\Migrated\usercontrols\Stub_usermessage_ascx_cs.cs' was created and contains an abstract class 
// used as a base class for the class 'Migrated_UserMessage' in file 'usercontrols\usermessage.ascx.cs'.
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

abstract public class UserMessage :  System.Web.UI.UserControl
{
		abstract public String MessageText
		{
		  set;
		}
		abstract public UserMessageType MessageType
		{
		  set;
		}
		public enum UserMessageType {Critical, Error, ValidationError, Warning, Info};


}



}

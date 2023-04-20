using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.AppUtils;
using GASystem.DataModel;

namespace GASystem.WebControls.EditControls
{
	/// <summary>
	/// Summary description for DataRecordPicker.
	/// </summary>
	[DefaultProperty("Value"), 
		ToolboxData("<{0}:DataRecordPicker runat=server></{0}:DataRecordPicker>")]
	public class DataRecordPicker : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private System.Web.UI.WebControls.TextBox DisplayValueTextBox;
		private HyperLink CheckDataButton;
		private Literal JavaScriptOpenWindow;
		private System.Web.UI.HtmlControls.HtmlInputHidden KeyValueHidden;
		//private System.Web.UI.HtmlControls.HtmlImage CheckNameButton;
		//private int _value = 0;
		private FieldDescription _fieldDescription;



		protected override void CreateChildControls()
		{
			DisplayValueTextBox = new TextBox();
			DisplayValueTextBox.ID = "DisplayValueTextBox";
			DisplayValueTextBox.Columns = 25;
			
			//DisplayValueTextBox.ID = "displayvaluetextbox";
			Controls.Add(DisplayValueTextBox);
			
//			CheckNameButton = new System.Web.UI.HtmlControls.HtmlImage();
//			//CheckNameButton = this.ID +  "_CheckNameButton";
//			CheckNameButton.Src = "../Images/checkname.gif";
//			CheckNameButton.Border = 0;
//			this.Controls.Add(CheckNameButton);
			
			CheckDataButton = new HyperLink();
			CheckDataButton.ImageUrl = "../gagui/Images/checkname.gif";
			CheckDataButton.NavigateUrl = "javascript:openPickerWindow();";
			Controls.Add(CheckDataButton);

			KeyValueHidden = new System.Web.UI.HtmlControls.HtmlInputHidden();
			KeyValueHidden.ID =  "KeyValueHidden";
			Controls.Add(KeyValueHidden);
			
			JavaScriptOpenWindow = new Literal();
			JavaScriptOpenWindow.Text = "<script language=\"javascript\">\n function openPickerWindow() {\n var Fwin= null;\n";
			JavaScriptOpenWindow.Text +="Fwin = window.open('/DotNetNuke/gagui/WebForms/PickDataRecordDialog.aspx?ValueControlId=" + DisplayValueTextBox.ClientID + "&KeyControlId=" + KeyValueHidden.ClientID + "&DataClass=GAPersonnel&DisplayName=GivenName+FamilyName', 'pick_dialog', 'toolbar=no, directories=no, location=no, status=no, menubar=no, resizable=yes, scrollbars=yes, width=400, height=600');";
			JavaScriptOpenWindow.Text += "\n if (Fwin && !Fwin.closed) Fwin.focus(); \n } \n</script>";
			Controls.Add(JavaScriptOpenWindow);

			
			base.CreateChildControls ();

			//DisplayValueText();
		}

//		protected override void OnPreRender(EventArgs e)
//		{
//			EnsureChildControls();
//			DisplayValueTextBox.Text = displayText;
//			KeyValueHidden.Value = Value.ToString();
//			base.OnPreRender (e);
//		}


	

		[Bindable(true), 
			Category("Appearance"), 
			DefaultValue("")] 
		public int Value 
		{
			get
			{
//				if (ViewState["value"] == null)
//					return -1;
//
//				return Int32.Parse(ViewState["value"].ToString());
				EnsureChildControls();
				return Int32.Parse(KeyValueHidden.Value);

			}

			set
			{
				EnsureChildControls();
				KeyValueHidden.Value = value.ToString();
				//ViewState["value"] = value;
				displayText = getDisplayValueText();
				//EnsureChildControls();
				//KeyValueHidden.Value = value.ToString();
				//DisplayValueText();
			}
		}

		private string displayText 
		{
			set 
			{
				EnsureChildControls();
				//ViewState["displayText"] = value;
				DisplayValueTextBox.Text = value;
			}
			get 
			{
				//if (ViewState["displayText"] == null)
				//	return string.Empty;
				EnsureChildControls();
				//return ViewState["displayText"].ToString();
				return DisplayValueTextBox.Text;
			}
		}

		public FieldDescription FieldDescriptionInfo 
		{
			get
			{				
				return _fieldDescription;
			}

			set
			{
				_fieldDescription = value;
			}
		}

		private string getDisplayValueText() 
		{

//Commented ou ty jof 27/10/05, is this needed??			
//			PersonnelDS personnelData = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelRowId(Value);
//			if (0!=personnelData.GAPersonnel.Rows.Count) 
//				return personnelData.GAPersonnel[0].GivenName + " " + personnelData.GAPersonnel[0].FamilyName;

			return string.Empty;
		}
		#region IPostBackDataHandler Members

		public void RaisePostDataChangedEvent()
		{
			// TODO:  Add DataRecordPicker.RaisePostDataChangedEvent implementation
		}

		public bool LoadPostData(string postDataKey, System.Collections.Specialized.NameValueCollection postCollection)
		{
			// TODO:  Add DataRecordPicker.LoadPostData implementation
			return false;
		}

		#endregion
	}
}

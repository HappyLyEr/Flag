namespace GASystem.GAControls
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

	/// <summary>
	///		Summary description for ActionOwnerContext.
	/// </summary>
	public class ActionOwnerContext : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label LabelCaption;
		protected System.Web.UI.WebControls.Panel Panel1;
		private string _displayPanelId = string.Empty;
		private HyperLink lblDisplayViewPanel = new HyperLink();
		protected System.Web.UI.WebControls.Panel PlaceHolderShow;
		protected System.Web.UI.WebControls.Panel PlaceHolderHide;
		protected System.Web.UI.WebControls.Image ImageHide;
		protected System.Web.UI.WebControls.Image ImageShow;

		private HyperLink lblHideViewPanel = new HyperLink();
		
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			SetUpDisplayLinks();
		}

		public void SetActionId(int ActionId) 
		{
			//get owner
			GADataRecord owner = DataClassRelations.GetOwner(new GADataRecord(ActionId, GADataClass.GAAction));
			//get name
			SetActionOwner(owner);
//			string ownerName = getNameForDataRecord(owner);
//			//set link
//			//HyperLink1.NavigateUrl = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordView(owner.DataClass.ToString(), owner.RowId.ToString());
//			//HyperLink1.Text = GASystem.AppUtils.Localization.GetGuiElementText(owner.DataClass.ToString()) + " " + ownerName;
//			lblDisplayViewPanel.Text = GASystem.AppUtils.Localization.GetGuiElementText(owner.DataClass.ToString()) + " " + ownerName;
//			lblHideViewPanel.Text = GASystem.AppUtils.Localization.GetGuiElementText(owner.DataClass.ToString()) + " " + ownerName;

		}

		public void SetActionOwner(GADataRecord Owner) 
		{
			string ownerName = getNameForDataRecord(Owner);
			//set link
			//HyperLink1.NavigateUrl = GASystem.GUIUtils.LinkUtils.GenerateURLForSingleRecordView(owner.DataClass.ToString(), owner.RowId.ToString());
			//HyperLink1.Text = GASystem.AppUtils.Localization.GetGuiElementText(owner.DataClass.ToString()) + " " + ownerName;
			lblDisplayViewPanel.Text = GASystem.AppUtils.Localization.GetGuiElementText(Owner.DataClass.ToString()) + " " + ownerName;
			lblHideViewPanel.Text = GASystem.AppUtils.Localization.GetGuiElementText(Owner.DataClass.ToString()) + " " + ownerName;

		}

		private void SetUpDisplayLinks() 
		{
			//lblDisplayViewPanel = new HyperLink();
			//lblDisplayViewPanel.NavigateUrl = "#";
			lblDisplayViewPanel.ID = "lblDisplayViewPanel";
			lblDisplayViewPanel.CssClass = "linkLabel";
			lblDisplayViewPanel.Attributes.Add("onclick", "javascript:setviewpanelvisible();return false;");
			this.PlaceHolderShow.Controls.Add(lblDisplayViewPanel);
			ImageShow.Attributes.Add("onclick", "javascript:setviewpanelvisible();return false;");	
					
			//lblHideViewPanel = new HyperLink();
			//lblHideViewPanel.NavigateUrl = "#";
			lblHideViewPanel.CssClass = "linkLabel";
			lblHideViewPanel.Attributes.Add("onclick", "javascript:hideviewpanel();return false;");
			this.PlaceHolderHide.Controls.Add(lblHideViewPanel);
			ImageHide.Attributes.Add("onclick", "javascript:hideviewpanel();return false;");
			
		}

		private string getNameForDataRecord(GADataRecord DataRecord)
		{
			string ownerName;
			DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(DataRecord);
			AppUtils.ClassDescription cd = AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataRecord.DataClass);
			
			if (cd.ObjectName != string.Empty) 
			{
				if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Contains(cd.ObjectName))
					ownerName = ds.Tables[0].Rows[0][cd.ObjectName].ToString();
				else
					ownerName = ""; 
			
			} 
			else 
			{
				ownerName = "";
			}
			return ownerName;
		}

		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
			 
		}
		
		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		public string DisplayPanelId 
		{
			get{return _displayPanelId;}
			set{_displayPanelId = value;}
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			//setviewpanelvisible
			string jscode = "<script language=\"javascript\">\n";
			jscode += "function setviewpanelvisible() {\n";
			jscode +=  "var rescontrol = document.getElementById('" + DisplayPanelId +  "');\n";
			//lblDisplayViewPanel
			jscode +=  "var displayLabel = document.getElementById('" + PlaceHolderShow.ClientID +  "');\n";
			jscode +=  "var hideLabel = document.getElementById('" + PlaceHolderHide.ClientID +  "');\n";
			jscode += "rescontrol.style.display  = 'block'; ";
			jscode += "displayLabel.style.display  = 'none'; ";
			jscode += "hideLabel.style.display  = 'block'; ";
			//visible

			jscode += "}\n </script>";
			this.Page.ClientScript.RegisterStartupScript(typeof(ActionOwnerContext), "respuservisible" + this.ID, jscode);
			
			
			
			//hideviewpanel
			jscode = "<script language=\"javascript\">\n";
			jscode += "function hideviewpanel() {\n";
			jscode +=  "var rescontrol = document.getElementById('" + DisplayPanelId +  "');\n";
			jscode +=  "var displayLabel = document.getElementById('" + PlaceHolderShow.ClientID +  "');\n";
			jscode +=  "var hideLabel = document.getElementById('" + PlaceHolderHide.ClientID +  "');\n";
			
			
			jscode += "displayLabel.style.display  = 'block'; ";
			jscode += "rescontrol.style.display  = 'none'; ";
			jscode += "hideLabel.style.display  = 'none'; ";
			//visible

			jscode += "}\n </script>";
			this.Page.ClientScript.RegisterStartupScript(typeof(ActionOwnerContext), "setrolevisible" + this.ID, jscode);


            if (!this.Page.IsPostBack)
            {

                PlaceHolderHide.Style.Add("display", "none");
                PlaceHolderShow.Style.Add("display", "block");
            }
		}

	}
}

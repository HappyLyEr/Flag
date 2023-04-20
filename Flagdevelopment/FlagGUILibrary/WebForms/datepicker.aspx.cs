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
using System.Text;
using GASystem.AppUtils;


namespace GASystem.WebForms
{
	/// <summary>
	/// Summary description for datepicker.
	/// </summary>
	public class datepicker : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Calendar Calendar1;
		protected System.Web.UI.WebControls.Button Button1;
		private string _parentValueControlId;
		private string _selectedValue;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
			if (!IsPostBack)
				if (Request["date"] != null) 
				{
					string newText = Page.Server.UrlDecode(Request["date"].ToString());
					if (newText == string.Empty)
						Calendar1.SelectedDates.Clear();
					else 
					{
						if (GAUtils.IsDate(newText)) 
						{
							Calendar1.SelectedDate = Convert.ToDateTime(newText);
							Calendar1.VisibleDate = Calendar1.SelectedDate;
						}
					}
				}


			ParentValueControlId = Request["ParentValueControlId"].ToString();
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
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Button1.Click += new System.EventHandler(this.Button1_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Button1_Click(object sender, System.EventArgs e)
		{
			SelectedValue = Calendar1.SelectedDate.ToShortDateString();
			
			StringBuilder sb = new StringBuilder();
			sb.Append("<script language=javascript>");

			sb.Append("setParentWindowValues();");
			sb.Append("window.close();");
			sb.Append("</script>");
			Page.RegisterStartupScript("Setvalues", sb.ToString());
			

		}

		public string ParentValueControlId 
		{
			get {return _parentValueControlId;}
			set {_parentValueControlId = value;}
		}

		public string SelectedValue 
		{
			get {return _selectedValue;}
			set {_selectedValue = value;}
		}
	}
}

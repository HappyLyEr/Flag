namespace GASystem.GAControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.DataModel;
	using GASystem.AppUtils;
	using System.Collections;

	/// <summary>
	///		Summary description for ShortCutLinks.
	/// </summary>
	public class ShortCutLinks : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.DataGrid DataGrid1;

		// Tor 20151021 private static ArrayList _linkList;
        private ArrayList _linkList;
        // Tor 20151021 private static object synchLock = new object();
        private object synchLock = new object();

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		public void GenerateLinks() 
		{
            //TODO reload arraylist here. set correct language

			DataGrid1.DataSource =  getLinks();   //GetMembers().Tables[0]; 
			DataGrid1.DataBind();
		}

// Tor 20151021		private static ArrayList getLinks() 
		private ArrayList getLinks() 
		{
			if (_linkList != null)
				return _linkList;
			
			lock(synchLock)
			{
				if (_linkList != null)   //check for null once more in case a thread was waiting while generating links
					return _linkList;
				
				_linkList = GASystem.GUIUtils.ShortcutLinks.getAllLinks();
				return _linkList;		
			}
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
	}
}

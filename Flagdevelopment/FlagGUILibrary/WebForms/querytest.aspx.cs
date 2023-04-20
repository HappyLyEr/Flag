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

namespace GASystem.WebForms
{
	/// <summary>
	/// Summary description for querytest.
	/// </summary>
	public class querytest : System.Web.UI.Page
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DataGrid DataGrid1;
		protected System.Web.UI.WebControls.Label Label2;
		protected System.Web.UI.WebControls.Label Label3;
		protected System.Web.UI.WebControls.TextBox txtOwnerClass;
		protected System.Web.UI.WebControls.TextBox txtRowId;
		protected System.Web.UI.WebControls.Label Label4;
		protected System.Web.UI.WebControls.Button Button1;
	
		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
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
			this.txtOwnerClass.TextChanged += new System.EventHandler(this.txtOwnerClass_TextChanged);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void Button1_Click(object sender, System.EventArgs e)
		{
			int[] aLongList;
			GASystem.DataAccess.SuperClassDb superclass = new GASystem.DataAccess.SuperClassDb();
		
			
//		aLongList = superclass.FindAllMemberRowIds(txtOwnerClass.Text, (int)Convert.ToInt32(txtRowId.Text), "GAFile");
//			Label1.Text = aLongList.Length.ToString();
//			DataGrid1.DataSource = GASystem.DataAccess.FileDb.GetFilesById(aLongList);
			DataGrid1.DataMember = "GAFile";
			DataGrid1.DataBind();
		}

		private void txtOwnerClass_TextChanged(object sender, System.EventArgs e)
		{
		
		}
	}
}

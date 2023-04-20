namespace GASystem
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for PersonnelField.
	/// </summary>
	public class PersonnelField : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.TextBox DisplayValueTextBox;
		protected System.Web.UI.HtmlControls.HtmlInputHidden KeyValueHidden;
		protected System.Web.UI.HtmlControls.HtmlImage CheckNameButton;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}


		

		//Provide a way to store the DisplayValue in viewState
		public String DisplayValue
		{
			get
			{
				return "".Equals(DisplayValueTextBox.Text) ? "" : DisplayValueTextBox.Text;
			}
			set
			{
				DisplayValueTextBox.Text = value;
			}
		}


		//Provide a way to store the dataset in viewState
		public int RowId
		{
			get
			{
				return "".Equals(KeyValueHidden.Value) ? 0 : int.Parse(KeyValueHidden.Value);
			}
			set
			{
				KeyValueHidden.Value = value.ToString();
			}
		}

		public bool IsForeignKeyConstraint
		{
			get
			{
				return null==ViewState["IsForeignKeyConstraint"] ? true : bool.Parse(ViewState["IsForeignKeyConstraint"].ToString() );
			}
			set
			{
				ViewState["IsForeignKeyConstraint"] = value;
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

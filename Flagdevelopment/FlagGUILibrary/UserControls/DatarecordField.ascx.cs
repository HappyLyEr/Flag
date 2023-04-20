namespace GASystem
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		DEPRECATED 17.01.2005 (Frank Wathne)
	///		DEPRECATED 17.01.2005
	///		DEPRECATED 17.01.2005
	///		DEPRECATED 17.01.2005
	///		DEPRECATED 17.01.2005
	///		DEPRECATED 17.01.2005
	/// </summary>
	public class DatarecordField : System.Web.UI.UserControl
	{
		protected System.Web.UI.HtmlControls.HtmlInputHidden KeyValueHidden;
		protected System.Web.UI.HtmlControls.HtmlImage MagnifyButton;
		protected System.Web.UI.WebControls.Label DisplayValueLabel;
		protected System.Web.UI.WebControls.LinkButton DisplayValueLinkButton;

		

		private void Page_Load(object sender, System.EventArgs e)
		{
			
			
		}

		//Provide a way to store the DisplayValue in viewState
		public String DisplayValue
		{
			get
			{
				return "".Equals(DisplayValueLabel.Text) ? "" : DisplayValueLabel.Text;
			}
			set
			{
				DisplayValueLabel.Text = value;
			}
		}

		///		DEPRECATED 17.01.2005 (Frank Wathne)
		///		DEPRECATED 17.01.2005
		///		DEPRECATED 17.01.2005
		///		DEPRECATED 17.01.2005
		///		DEPRECATED 17.01.2005
		///		DEPRECATED 17.01.2005


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



		///		DEPRECATED 17.01.2005 (Frank Wathne)
		///		DEPRECATED 17.01.2005
		///		DEPRECATED 17.01.2005
		///		DEPRECATED 17.01.2005
		///		DEPRECATED 17.01.2005
		///		DEPRECATED 17.01.2005


		//Provide a way to store the dataset in viewState
		public String DataClass
		{
			get
			{
				return null==ViewState[this.ID+"DataClass"] ? null : (String) ViewState[this.ID+"DataClass"];
			}
			set
			{
				ViewState[this.ID+"DataClass"] = value;
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

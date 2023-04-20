namespace GASystem
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;

	/// <summary>
	///		Summary description for UserMessage.
	/// </summary>
	public class UserMessage : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label MessageLabel;
		protected System.Web.UI.WebControls.Panel Panel1;

		private void Page_Load(object sender, System.EventArgs e)
		{
			
		}

		public String MessageText
		{
			set
			{
				MessageLabel.Text = value;
			}
		}

		public UserMessageType MessageType
		{
			set 
			{
				if (value==UserMessageType.Error)
					MessageLabel.CssClass = "MessageError";
				else if (value==UserMessageType.Warning)
					MessageLabel.CssClass = "MessageError";
				else if (value==UserMessageType.ValidationError)
					MessageLabel.CssClass = "MessageValidationError";
				else 
					MessageLabel.CssClass = "MessageNormal";
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
		
		public enum UserMessageType {Critical, Error, ValidationError, Warning, Info};
	
	}

	
}

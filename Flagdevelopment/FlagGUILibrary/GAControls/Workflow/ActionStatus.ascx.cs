namespace GASystem.GAControls
{
	using System;
	using System.Data;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using GASystem.DataModel;
	using GASystem.BusinessLayer;

	/// <summary>
	///		Summary description for ActionStatus.
	/// </summary>
	public class ActionStatus : System.Web.UI.UserControl
	{
		protected System.Web.UI.WebControls.Label Label1;
		protected System.Web.UI.WebControls.DataGrid ActionDataGrid;

		private void Page_Load(object sender, System.EventArgs e)
		{
			// Put user code to initialize the page here
		}

		public GADataRecord Owner 
		{
			get 
			{
				return (GADataRecord)ViewState["ActionOwner"];
			}
			set 
			{
				ViewState["ActionOwner"] = value;
				buildDataGrid();
			}
		}

		
		private void buildDataGrid() 
		{
			
			ActionDS ds = Action.GetActionsByOwner(Owner);
			DataView dv = new DataView(ds.GAAction);
			dv.RowFilter = "Workflowid is null and workflowname is not null";
			
			if (dv.Count == 0) 
			{
				Label1.Visible = false;
				ActionDataGrid.Visible = false;
			} 
			else 
			{
				Label1.Visible = true;
				ActionDataGrid.Visible = true;
			}
			
			//ActionDataGrid.DataSource = Action.GetActionsByOwner(Owner);
			//ActionDataGrid.DataMember = GADataClass.GAAction.ToString();
			ActionDataGrid.DataSource = dv;
			ActionDataGrid.DataBind();
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
			this.ActionDataGrid.ItemCommand += new System.Web.UI.WebControls.DataGridCommandEventHandler(this.ActionDataGrid_ItemCommand);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void ActionDataGrid_ItemCommand(object source, System.Web.UI.WebControls.DataGridCommandEventArgs e)
		{
			GAWorkflow.OWFEWorkFlowEngine.SetStartPending(GASystem.AppUtils.GAUsers.GetUserId(), Convert.ToInt32(e.Item.Cells[0].Text));
			//Label1.Text +=  e.Item.Cells[0].Text;
			buildDataGrid();
		}

	}
}

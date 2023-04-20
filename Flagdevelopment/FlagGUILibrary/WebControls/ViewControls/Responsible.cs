using System;
using System.Data;
using System.Web.UI.WebControls;
using GASystem.AppUtils;
using GASystem.BusinessLayer;
using GASystem.DataModel;

namespace GASystem.WebControls.ViewControls
{
	/// <summary>
	/// Summary description for Responsible.
	/// </summary>
	public class Responsible : WebControl
	{
		private Label lblResponsible;
		
		public Responsible()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		
		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			lblResponsible = new Label();
			this.Controls.Add(lblResponsible);
		}

		
		
		public void SetResponsibles(DataColumn UserColumn, DataColumn RoleColumn)
		{
			bool RespIsAnUser = true;
			bool RespIsARole = true;
			if (UserColumn.Table.Rows[0][UserColumn] == DBNull.Value) 
				RespIsAnUser = false;
			if (RoleColumn.Table.Rows[0][RoleColumn] == DBNull.Value) 
				RespIsARole = false;
			
			if (RespIsAnUser)
			{
				lblResponsible.Text = Localization.GetGuiElementText("Person") + ": " + UserColumn.Table.Rows[0][UserColumn].ToString();
				return;
			}
			
			if (RespIsARole)
			{
				lblResponsible.Text = Localization.GetGuiElementText("Role") + ": " + RoleColumn.Table.Rows[0][RoleColumn].ToString();
				return;
			}
			
			lblResponsible.Text = Localization.GetGuiElementText("NoResponsibleSet");
		}
		
		
		
	}
}

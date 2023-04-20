using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using GASystem.GAGUIEvents;

namespace GASystem.GAControls.UtilityControls
{
	/// <summary>
	/// WebControl for selecting a procedure. Sets SelelectedProcudre to the 
	/// selected procedure and raises a ProcedureSelected event when the users chooses a 
	/// procedure 
	/// </summary>
	public class ProcedureSelector : System.Web.UI.WebControls.WebControl
	{
		private System.Web.UI.WebControls.Label Label1;
		private System.Web.UI.WebControls.DropDownList DropDownList1;
		private System.Web.UI.WebControls.LinkButton LinkButton1;

		public event GACommandEventHandler ProcedureSelected;
		
		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
			Label1 = new Label();
		
			DropDownList1 = new DropDownList();
			LinkButton1 = new LinkButton();
			LinkButton1.CausesValidation = false;


			Label1.Text = "Select Template";
			LinkButton1.Text = "Apply Template";

			this.Controls.Add(Label1);
			this.Controls.Add(DropDownList1);
			this.Controls.Add(LinkButton1);

			LinkButton1.Click += new EventHandler(LinkButton1_Click);

		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			GASystem.DataModel.ProcedureDS pds = GASystem.BusinessLayer.Procedure.GetAllProcedures();
			DropDownList1.DataSource = pds.GAProcedure;
			DropDownList1.DataMember = "ProcedureRowId";
			DropDownList1.DataTextField = "Shortname";
			DropDownList1.DataValueField = "ProcedureRowId";
			DropDownList1.DataBind();
		}

		private void LinkButton1_Click(object sender, EventArgs e)
		{
			
			if (null!=ProcedureSelected)
			{
				int procedureId = Convert.ToInt32(DropDownList1.SelectedItem.Value);
				GASystem.DataModel.ProcedureDS ds = GASystem.BusinessLayer.Procedure.GetProcedureByProcedureRowId(procedureId);
			
				
				GACommandEventArgs args = new GACommandEventArgs();
				args.CommandDataSetArgument = ds;
				args.CommandName = "Select";
				ProcedureSelected(sender, args);
			}

			
		}
	}
}

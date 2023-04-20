using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.ComponentModel;

namespace GASystem.WebControls.EditControls
{
	/// <summary>
	/// Summary description for WorkflowStarted.
	/// </summary>
	public class WorkflowStarted : System.Web.UI.WebControls.WebControl, INamingContainer
	{
		private CheckBox startWorkflowCheckBox;
		private Label lblStartWorkflow;
        // Tor 201611 Security 20161122 (never referenced) private bool _newRecord = false;
		

		public WorkflowStarted() 
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public bool NewRecord 
		{
			get
			{
				return null==ViewState["newrecord"] ? false : (bool) ViewState["newrecord"];
			}
			set
			{
				ViewState["newrecord"] = value;
			}

		}

		public bool Value 
		{
			get {return NewRecord && startWorkflowCheckBox.Checked;}  //only return true if checked and action is a new record
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);
	
			startWorkflowCheckBox = new CheckBox();
			startWorkflowCheckBox.Checked = true;
			this.Controls.Add(startWorkflowCheckBox);
			lblStartWorkflow = new Label();
			lblStartWorkflow.Text = "Start workflow when saving";
			this.Controls.Add(lblStartWorkflow);
			
		
		}

		protected override void OnPreRender(EventArgs e)
		{
			base.OnPreRender (e);
			startWorkflowCheckBox.Visible = NewRecord;
			lblStartWorkflow.Visible = NewRecord;
		}

		
	}
}

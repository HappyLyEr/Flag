using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Data;
using GASystem.BusinessLayer;

namespace GASystem.WebControls.ViewControls
{
	/// <summary>
	/// Summary description for WorkitemResponsible.
	/// </summary>
	public class WorkitemResponsibleRole : System.Web.UI.WebControls.WebControl
	{
		//private GASystem.ListDataRecords MyListDataRecords;
		private string _participants = string.Empty;
        private string _participantRoles = string.Empty;
		private int _actionId = -1;
        Table resonsiblesTable = new Table();

        public WorkitemResponsibleRole()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);

            resonsiblesTable.BorderWidth = 0;
            resonsiblesTable.BorderStyle = BorderStyle.None;
            this.Controls.Add(resonsiblesTable);
		}


		protected override void OnLoad(EventArgs e)
		{
			
			base.OnLoad (e);
//            addWorkitemRoleAssignments();

			//MyListDataRecords.RecordsDataSet = GASystem.BusinessLayer.Workitem.GetPersonnelForGAParticipantsAndActionId(Participant, ActionId);
           // responsibels.Text = getWorkitemUserAssignments() + getWorkitemRoleAssignments();
		
		}

       
        private void addWorkitemRoleAssignments()
        {
            if (ParticipantRole == string.Empty)
                return;
            
            string participantRoleIds = string.Empty;
            if (ParticipantRole.Length > 2)
            {
                participantRoleIds = ParticipantRole.Substring(1, ParticipantRole.Length - 2);
                participantRoleIds = participantRoleIds.Replace(";", ",");
            }

            string[] participantRoles = participantRoleIds.Split(',');

            //Lists lists = new Lists();

            string names = string.Empty;
           if (participantRoles.Length > 0)
           {

               foreach (string participantPersonnelID in participantRoles)
               {
                   TableRow trow = new TableRow();
                   
                   TableCell rightCell = new TableCell();
                   rightCell.CssClass = "workitemparticipent";
                   trow.Cells.Add(rightCell);
                   try
                   {
                       rightCell.Text = Lists.GetListDescriptionByRowId(int.Parse(participantPersonnelID));
                   }
                   catch (GAExceptions.GADataAccessException gaex)
                   {
                       //TODO LOG
                   }
                   catch (System.ArgumentException aex)
                   {
                       //todo log
                   }
                   catch (System.FormatException fex)
                   {
                       //todo log
                   }
                   resonsiblesTable.Rows.Add(trow);
               }
           }
               
           return;
        }



		protected override void OnPreRender(EventArgs e)
		{
			
			base.OnPreRender (e);
            addWorkitemRoleAssignments(); // Tor 20141208
		}

        public string ParticipantRole
        {
            get { return _participantRoles; }
            set { _participantRoles = value; }
        }

		public int ActionId 
		{
			get {return _actionId;}
			set {_actionId = value;}
		}

	}
}

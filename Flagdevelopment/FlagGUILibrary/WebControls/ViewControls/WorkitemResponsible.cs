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
	public class WorkitemResponsible : System.Web.UI.WebControls.WebControl
	{
		//private GASystem.ListDataRecords MyListDataRecords;
		private string _participants = string.Empty;
        private int _actionId = -1;
        Table resonsiblesTable = new Table();

		public WorkitemResponsible()
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
//            addWorkitemUserAssignments();
        }

        private void addWorkitemUserAssignments()
        {
            if (Participant == string.Empty)
                return;
            
            string participantIds = string.Empty;
            if (Participant.Length > 2)
            {
                participantIds = Participant.Substring(1, Participant.Length - 2);
                participantIds = participantIds.Replace(";", ",");
            }




            string[] participants = participantIds.Split(',');

            Personnel personnelBC = new Personnel();

            string names = string.Empty;
            if (participants.Length > 0)
            {


                // Tor 201611 Security 20161122 (never referenced) TableCell firstUserCell = null;
                // Tor 201611 Security 20161122 (never referenced) bool isFirstParticipantUserCell = true;

                foreach (string participantPersonnelID in participants)
                {
                    TableRow trow = new TableRow();
  
                    TableCell rightCell = new TableCell();
                    rightCell.CssClass = "workitemparticipent";
                    trow.Cells.Add(rightCell);
                    try
                    {
                        rightCell.Text =  personnelBC.getPersonnelFullName(int.Parse(participantPersonnelID));
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
            addWorkitemUserAssignments(); // Tor 20141208
 
		}

		public string Participant 
		{
			get {return _participants;}
			set {_participants = value;}
		}

  		public int ActionId 
		{
			get {return _actionId;}
			set {_actionId = value;}
		}

	}
}

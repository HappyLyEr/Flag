using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using GASystem.BusinessLayer;
using GASystem.DataModel;

namespace GASystem.GAControls.Workflow
{
    [ToolboxData("<{0}:WorkitemNotifierWebControl runat=server></{0}:WorkitemNotifierWebControl>")]
    public class WorkitemNotifierWebControl : WebControl
    {
        private GADataRecord _dataRecord;
        private Label workitemMessage = new Label();

        
        public WorkitemNotifierWebControl(GADataRecord dataRecord ) : base()
        {

            _dataRecord = dataRecord;
           
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
           
          
        }

        protected override void OnInit(EventArgs e)
        {
            this.Controls.Add(workitemMessage);
            DisplayWorkitemInfo();
        }

        private void DisplayWorkitemInfo()
        {
            if (_dataRecord.RowId == 0)
                return;  //new record, do nothing

            //only display these workitems for records containing actions
            if (!DataClassRelations.IsDataClassValidMember(this._dataRecord.DataClass, GADataClass.GAAction))
                return;



            //get all workitems for record.
            BusinessClass wbc = BusinessLayer.Utils.RecordsetFactory.Make(GADataClass.GAWorkitem);

            string workitemFilter = "Workitemstatus ='" + Workitem.WorkitemStatus.Active.ToString() + "'";
            workitemFilter += " and extra1 like '%;" + User.GetPersonnelIdByLogonId(GASystem.AppUtils.GAUsers.GetUserId()).ToString() + ";%'";
            WorkitemDS wds = (WorkitemDS)wbc.GetAllRecordsFromGATableWithinOwnerAndLinkedRecords(_dataRecord, System.DateTime.MinValue, System.DateTime.MaxValue, workitemFilter);

            if (wds.GAWorkitem.Rows.Count == 0)
                workitemMessage.Visible = false;

            if (wds.GAWorkitem.Rows.Count == 1)
                workitemMessage.Text = "You have one outstanding workitem for this " + AppUtils.Localization.GetGuiElementText(this._dataRecord.DataClass.ToString()) + "</br>";

            if (wds.GAWorkitem.Rows.Count > 1)
                workitemMessage.Text = "You have outstanding workitems for this " + AppUtils.Localization.GetGuiElementText(this._dataRecord.DataClass.ToString()) + "</br>";


            foreach ( WorkitemDS.GAWorkitemRow wrow in wds.GAWorkitem.Rows)
            {
                this.Controls.Add(new WorkitemNotifierSingleWebControl(wrow));
            }


          
        }

      
    }
}

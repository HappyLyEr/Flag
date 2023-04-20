using System;
using System.Web.UI.WebControls;
using GASystem.AppUtils;
using GASystem.BusinessLayer;
using GASystem.DataModel;

namespace GASystem.GAControls.ViewDetailsList
{
	/// <summary>
	/// Summary description for GeneralViewDetails.
	/// </summary>
	public class GeneralViewDetail : System.Web.UI.WebControls.WebControl
	{
		GADataRecord _owner;
		GADataClass _dataClass;
		
		public GeneralViewDetail(GADataRecord Owner, GADataClass DataClass)
		{
			_owner = Owner;
			_dataClass = DataClass;
		}
		
		protected override void OnInit(EventArgs e)
		{
			base.OnInit (e);



            //add info panel on top
            Panel myDetailsInfoPanel = new Panel();
            myDetailsInfoPanel.CssClass = "detailsInfoPanel";

            Label myDetailsInfoLabel = new Label();
            myDetailsInfoLabel.Text = Localization.GetGuiElementTextPlural(_dataClass.ToString());
            myDetailsInfoLabel.CssClass = "detailsInfoPanelCaption";

            //myDetailsInfoPanel.Controls.Add(myDetailsInfoLabel);
            myDetailsInfoPanel.Controls.Add(myDetailsInfoLabel);
            this.Controls.Add(myDetailsInfoPanel);
			
			ListData.ListClassByOwner dg = new ListData.ListClassByOwner(_dataClass, _owner);
            this.Controls.Add(dg);
            
		}

	}
}

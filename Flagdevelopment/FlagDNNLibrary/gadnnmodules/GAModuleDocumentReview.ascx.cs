namespace gadnnmodules
{
	using System;
	using GASystem.DataModel;
    using Bring2mind.DNN.Modules.DMX.Business;

	/// <summary>
	///		Summary description for GAModule.
	/// </summary>
	public class GAModuleDocumentReview : DotNetNuke.Entities.Modules.PortalModuleBase
	{

		private GASystem.GAGUI.GUIUtils.DocumentReviewFrontController moduleController;
        protected System.Web.UI.WebControls.Panel gamodulepanel;
		

		override protected void OnInit(EventArgs e)
		{
			//TODO add code for getting class from config
            string entryId = getEntryId();
            string command = getCommand();

            if (!(command == string.Empty || entryId == string.Empty))
            {

                //get data from dmx
                EntriesController dmxcontroller = new EntriesController();
                EntriesInfo dmxinfo = dmxcontroller.GetEntries(int.Parse(entryId), this.PortalId);

                if (dmxinfo.EntryType.ToUpper() != "COLLECTION")
                {



                    moduleController = new GASystem.GAGUI.GUIUtils.DocumentReviewFrontController();
                    moduleController.DocInfo = new GASystem.BusinessLayer.Documents.DocumentInfo(dmxinfo.PortalId.ToString() + "url", entryId, dmxinfo.Description, GASystem.BusinessLayer.Documents.DocumentManagentSystem.DMX);
                    //moduleController.HasEditPermissions = utils.Security.HasEditPermissions(this.ModuleId, this.TabId);
                    gamodulepanel.Controls.Add(moduleController);
                }
            }
			base.OnInit(e);
		}	

        //EntryId=2167&Command=Core_ViewDetails

        private string getEntryId()
        {
            if (this.Page.Request.QueryString["EntryId"] == null)
                return string.Empty;
            string entryId = this.Page.Request.QueryString["EntryId"].ToString();
            //dmx require that entryid is numeric, check this
            if (!GASystem.AppUtils.GAUtils.IsNumeric(entryId))
                return string.Empty;

            return entryId;
        }

        private string getCommand()
        {
            if (this.Page.Request.QueryString["Command"] == null)
                return "Core_ViewDetails";  //no command counts as core_viewdetails.
            string command = this.Page.Request.QueryString["Command"].ToString();
            //dmx command is Core_ViewDetails for docreview
            
            
            if (command != "Core_ViewDetails")
                return string.Empty;

            return command;
        }
	}
}

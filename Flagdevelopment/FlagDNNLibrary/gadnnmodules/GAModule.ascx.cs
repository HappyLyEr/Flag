namespace gadnnmodules
{
	using System;
	using GASystem.DataModel;
    using GASystem.GUIUtils;
    using GASystem.GAGUI.GUIUtils;

	/// <summary>
	///		Summary description for GAModule.
	/// </summary>
	public class GAModule : DotNetNuke.Entities.Modules.PortalModuleBase
	{

		private GASystem.GAGUI.GUIUtils.FrontController moduleController;
        private GASystem.GAGUI.GUIUtils.WorkitemFrontController moduleWorkitemController;
        protected System.Web.UI.WebControls.Panel gamodulepanel;
		

		override protected void OnInit(EventArgs e)
		{
			//TODO add code for getting class from config


            GeneralQueryStringUtils requestUtil = new GeneralQueryStringUtils(this.Page.Request);
            string dataclass = requestUtil.getSingleAlphaNumericQueryStringParam("dataclass");

            if (dataclass.Length == 0)  
            {
                //dataclass is not specified in querystring. most likely because the current tab is a starter tab for
                //admin classes or personnel class get class by tabid
                dataclass = System.Configuration.ConfigurationManager.AppSettings.Get("GADataClassForPage" + this.TabId.ToString());
                

            }

            GADataClass dc = GADataRecord.ParseGADataClass(dataclass);    //
            if (dc == GADataClass.GAWorkitem)
            {
                moduleWorkitemController = new WorkitemFrontController(dc);
                moduleWorkitemController.HasEditPermissions = utils.Security.HasEditPermissions(this.ModuleId, this.TabId);
                gamodulepanel.Controls.Add(moduleWorkitemController);
            }
            else
            {
                moduleController = new GASystem.GAGUI.GUIUtils.FrontController(dc);
                moduleController.HasEditPermissions = utils.Security.HasEditPermissions(this.ModuleId, this.TabId);
                gamodulepanel.Controls.Add(moduleController);

            }

            
           
			base.OnInit(e);
		}	
	}
}

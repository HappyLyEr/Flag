using GASystem.GAGUI.GUIUtils;

namespace gadnnmodules
{
	using System;
	using GASystem.DataModel;

	/// <summary>
	///		Summary description for GAModule.
	/// </summary>
	public class GAWorkitem : DotNetNuke.Entities.Modules.PortalModuleBase
	{

		private GASystem.GAGUI.GUIUtils.WorkitemFrontController moduleController;
        protected System.Web.UI.WebControls.Panel gamodulepanel;
		

		override protected void OnInit(EventArgs e)
		{
			//TODO add code for getting class from config

            GADataClass dc = GADataRecord.ParseGADataClass(System.Configuration.ConfigurationManager.AppSettings.Get("GADataClassForPage" + this.TabId.ToString()));
			moduleController = new WorkitemFrontController(dc);
			moduleController.HasEditPermissions = utils.Security.HasEditPermissions(this.ModuleId, this.TabId);
            gamodulepanel.Controls.Add(moduleController);

			base.OnInit(e);
		}	
	}
}

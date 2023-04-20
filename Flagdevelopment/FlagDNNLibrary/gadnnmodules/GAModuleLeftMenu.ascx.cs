namespace gadnnmodules
{
	using System;
	using GASystem.DataModel;

	/// <summary>
	///		Summary description for GAModule.
	/// </summary>
    public class GAModuleLeftMenu : DotNetNuke.Entities.Modules.PortalModuleBase
	{

        private GASystem.GAGUI.GUIUtils.LeftMenuFrontController moduleController;

		

		override protected void OnInit(EventArgs e)
		{
			//TODO add code for getting class from config

            string dataClass = System.Configuration.ConfigurationManager.AppSettings.Get("GADataClassForPage" + this.TabId.ToString());
            GADataClass dc;
            try
            {
                dc = GADataRecord.ParseGADataClass(dataClass);
            }
            catch 
            {
                //dataclass can not be found set dc to nullclass
                dc = GADataClass.NullClass;
            }
            moduleController = new GASystem.GAGUI.GUIUtils.LeftMenuFrontController(dc);
			this.Controls.Add(moduleController);

			base.OnInit(e);
		}	
	}
}

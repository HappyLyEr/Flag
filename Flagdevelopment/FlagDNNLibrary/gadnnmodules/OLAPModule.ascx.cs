namespace gadnnmodules
{
	using System;
	using GASystem.DataModel;
    using GASystem.GAGUI.GAControls.Analysis;

	/// <summary>
	///		Summary description for GAModule.
	/// </summary>
	public class OLAPModule : DotNetNuke.Entities.Modules.PortalModuleBase
	{

        //private GASystem.GAGUI.GAControls.OLAP.CubeView myCubeView;
        private GASystem.GAGUI.GAControls.Analysis.PivotCustomControlTest myCubeView;

		override protected void OnInit(EventArgs e)
		{
         //   myCubeView = (GASystem.GAGUI.GAControls.OLAP.CubeView)this.Page.LoadControl("~/gagui/gacontrols/OLAP/CubeView.ascx");
            myCubeView = (GASystem.GAGUI.GAControls.Analysis.PivotCustomControlTest)this.Page.LoadControl("~/gagui/gacontrols/Analysis/Default.ascx");
            this.Controls.Add(myCubeView);

			base.OnInit(e);
		}	
	}
}

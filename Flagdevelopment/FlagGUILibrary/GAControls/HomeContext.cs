using System;
using System.Collections.Generic;
using System.Text;
using GASystem.DataModel;
using System.Web.UI.WebControls;
using System.Data;
using GASystem.BusinessLayer;
using GASystem.AppUtils;

namespace GASystem.GAGUI.GAControls
{
    public class HomeContext : System.Web.UI.UserControl
    {
       protected GASystem.GAControls.ShortCutLinks MyShortCutLinks;
        protected HomeContextNavigationHeader MyHomeContextNavigationHeader;

       

        private void Page_Load(object sender, System.EventArgs e)
        {
            Page.ClientScript.RegisterClientScriptInclude("utilities", ResolveUrl("~/js/utilities.js"));
            System.Web.HttpCookie cookie = new System.Web.HttpCookie("clientLang", Localization.GetCurrentLanguage());
            cookie.HttpOnly = false;
            this.Page.Response.AppendCookie(cookie);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // Put user code to initialize the page here
                        MyShortCutLinks.GenerateLinks();
                 

            

        }

        //private System.Data.DataSet GetMembers()
        //{
        //    DataSet ds = new DataSet();
        //    DataTable dt = new DataTable("memberclasses");
        //    dt.Columns.Add("dataclass");
        //    dt.Columns.Add("newdataclassname");
        //    foreach (object dataClass in GASystem.BusinessLayer.DataClassRelations.GetNextLevelDataClasses(CurrentContext.DataClass))
        //        dt.Rows.Add(new string[] { dataClass.ToString(), String.Format(Localization.GetGuiElementText("NewRecord"), Localization.GetGuiElementText(dataClass.ToString())) });
        //    ds.Tables.Add(dt);
        //    return ds;
        //}


      

        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        ///		Required method for Designer support - do not modify
        ///		the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Load += new System.EventHandler(this.Page_Load);

        }
        #endregion


    }
}

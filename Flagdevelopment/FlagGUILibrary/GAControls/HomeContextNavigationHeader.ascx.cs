using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using GASystem.DataModel;

namespace GASystem.GAGUI.GAControls
{
    public class HomeContextNavigationHeader : System.Web.UI.UserControl
    {
        protected System.Web.UI.WebControls.HyperLink HyperLink1;
        protected System.Web.UI.WebControls.Label lblError;

        private string GetDataRecordName(GADataRecord DataRecord)
        {
            string ownerName=string.Empty;
            DataSet ds = GASystem.BusinessLayer.Utils.RecordsetFactory.GetRecordSetByDataRecord(DataRecord);

            // Tor 20150325 Get Owner Name Column name from GAClass instead of using fixed fieldname "Name"
            string ObjectName = "ObjectName";
            string DefaultObjectName = "Name";
            DataSet cds = GASystem.BusinessLayer.Class.GetClassByClass(DataRecord.DataClass.ToString());
            string objectName = cds.Tables[0].Rows[0][ObjectName].ToString();
            if (objectName != null && objectName != string.Empty && ds.Tables[0].Columns.Contains(objectName)) return ds.Tables[0].Rows[0][objectName].ToString();
            // Tor 20150325 End

            if (ds.Tables[0].Columns.Contains(DefaultObjectName)) return ds.Tables[0].Rows[0][DefaultObjectName].ToString();

            // Neither columns GAClass.ObjectName nor DataRecord.Name exists. Return empty-
            return ownerName;
        }

        private GADataRecord CurrentContext
        {
            get
            {
                return ViewState["CurrentContext"] == null ? null : (GADataRecord)ViewState["CurrentContext"];
            }
            set
            {
                ViewState["CurrentContext"] = value;
            }
        }


        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            // Put user code to initialize the page here

            if (CurrentContext != GASystem.AppUtils.SessionManagement.GetCurrentDataContext().InitialContextRecord)
            {
                if (GASystem.AppUtils.SessionManagement.GetCurrentDataContext().InitialContextRecord != null)
                {
                    CurrentContext = GASystem.AppUtils.SessionManagement.GetCurrentDataContext().InitialContextRecord;
                    HyperLink1.Text = GASystem.AppUtils.Localization.GetGuiElementText("Home") + ": ";

                    String rowIdName = CurrentContext.DataClass.ToString() + "RowId";
                    rowIdName = rowIdName.Substring(2);
                    String tabId = System.Configuration.ConfigurationManager.AppSettings.Get(CurrentContext.DataClass.ToString() + "Details" + "TabId");
                    // Tor 20150325 Added: use DefaultTabId if tabid for current class is empty
                    if (tabId == null)
                    {
                        tabId = System.Configuration.ConfigurationManager.AppSettings.Get("DefaultTabId");
                    }

                    // Tor 20150325 added &dataclass=CurrentContext.DataClass.ToString() at the end to make the link work for defaultTabId classes	HyperLink1.NavigateUrl = "~/Default.aspx?tabId=" + tabId + "&" + rowIdName + "=" + CurrentContext.RowId;
                    HyperLink1.NavigateUrl = "~/Default.aspx?tabId=" + tabId + "&" + rowIdName + "=" + CurrentContext.RowId + "&dataclass=" + CurrentContext.DataClass.ToString();
                    try
                    {
                        HyperLink1.Text += GASystem.AppUtils.Localization.GetGuiElementText(CurrentContext.DataClass.ToString()) + " " + GetDataRecordName(CurrentContext);
                    }
                    catch (Exception ex)
                    {
                        //TODO change this to a gaexception
                        lblError.Text = "</br>" + ex.Message;
                    }
                }
                else
                {
                    lblError.Text = "</br>" + "Can not find your context";
                    //TODO remove this, should never get here, handle differently
                    CurrentContext = null;
                    HyperLink1.Visible = false;    
                }
            }
        }
     }
}
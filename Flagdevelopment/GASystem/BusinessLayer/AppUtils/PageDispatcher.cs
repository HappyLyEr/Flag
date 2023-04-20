using System;
using System.Web;
using GASystem.DataModel;
using System.Web.Configuration;

namespace GASystem.AppUtils
{
	/// <summary>
	/// Summary description for RequestDispatcher.
	/// </summary>
	public class PageDispatcher
	{
		//TODO move this to the gagui project
		
		public PageDispatcher()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		/// <summary>
		/// Redirects the response to a DotNetNuke page depending on the given dataclass. Web.config must contain a mapping of
		/// datatypes and DNN tabIDs
		/// </summary>
		/// <param name="CurrentResponse"></param>
		/// <param name="DataClass"></param>
		/// <param name="RowId"></param>
		public static void GotoDataRecordDetailsPage(HttpResponse CurrentResponse, GADataClass DataClass, int RowId, GADataRecord Owner)
		{
			String rowIdName = DataClass.ToString() + "RowId";
			rowIdName = rowIdName.Substring(2);
			String tabId = System.Configuration.ConfigurationManager.AppSettings.Get(DataClass.ToString() + "Details" + "TabId");
			
			if (tabId!=null && tabId.Length>0)
			{
				SessionManagement.SetCurrentSubContext(Owner); //new subcontext is owner of the datarecord we are looking at
				CurrentResponse.Redirect("~/Default.aspx?tabId="+tabId+"&"+rowIdName+"="+RowId+"&edit=true");	
			}
		}

        public static void GotoDataRecordNewPage(HttpResponse CurrentResponse, GADataClass DataClass, GADataRecord Owner)
        {
            string tabId = System.Configuration.ConfigurationManager.AppSettings.Get(DataClass.ToString() + "Details" + "TabId");
            string owner = string.Empty;
            if (null != Owner)
                owner = "&ownerclass=" + Owner.DataClass.ToString() + "&ownerrowid=" + Owner.RowId.ToString();

            string returnURL = "~/Default.aspx?tabId=" + tabId + "&" + DataClass.ToString().Substring(2) + "rowid=0&edit=true&dataclass=" + DataClass.ToString() + owner;
            CurrentResponse.Redirect(returnURL);
        }

		public static void GotoDataRecordViewPage(HttpResponse CurrentResponse, GADataClass DataClass, int RowId, GADataRecord Owner)
		{
			String rowIdName = DataClass.ToString() + "RowId";
			rowIdName = rowIdName.Substring(2);
			String tabId = System.Configuration.ConfigurationManager.AppSettings.Get(DataClass.ToString() + "Details" + "TabId");
			
			if (tabId!=null && tabId.Length>0)
			{
				SessionManagement.SetCurrentSubContext(Owner); //new subcontext is owner of the datarecord we are looking at
				CurrentResponse.Redirect("~/Default.aspx?tabId="+tabId+"&"+rowIdName+"="+RowId);	
			}
		}
		public static void GotoDataRecordViewPage(HttpResponse CurrentResponse, GADataClass DataClass, string RowId, GADataRecord Owner)
		{
			String rowIdName = DataClass.ToString() + "RowId";
			rowIdName = rowIdName.Substring(2);
			String tabId = System.Configuration.ConfigurationManager.AppSettings.Get(DataClass.ToString() + "Details" + "TabId");
			
			if (tabId!=null && tabId.Length>0)
			{
				SessionManagement.SetCurrentSubContext(Owner); //new subcontext is owner of the datarecord we are looking at
				CurrentResponse.Redirect("~/Default.aspx?tabId="+tabId+"&"+rowIdName+"="+RowId);	
			}
		}

		public static void GotoDataRecordViewPage(HttpResponse CurrentResponse, GADataClass DataClass, int RowId, GADataRecord Owner, GADataClass DisplayTab)
		{
			String rowIdName = DataClass.ToString() + "RowId";
			rowIdName = rowIdName.Substring(2);
			String tabId = System.Configuration.ConfigurationManager.AppSettings.Get(DataClass.ToString() + "Details" + "TabId");
			
			if (tabId!=null && tabId.Length>0)
			{
				SessionManagement.SetCurrentSubContext(Owner); //new subcontext is owner of the datarecord we are looking at
				CurrentResponse.Redirect("~/Default.aspx?tabId="+tabId+"&"+rowIdName+"="+RowId+"&subclass="+DisplayTab.ToString());	
			}
		}

		

		/// <summary>
		/// Redirects the response to a DotNetNuke page depending on the given dataclass. Web.config must contain a mapping of
		/// datatypes and DNN tabIDs
		/// </summary>
		/// <param name="CurrentResponse"></param>
		/// <param name="DataClass"></param>
	/*	public static void GotoDataRecordNewPage(HttpResponse CurrentResponse, GADataClass DataClass, GADataRecord Owner)
		{
			String rowIdName = DataClass.ToString() + "RowId";
			rowIdName = rowIdName.Substring(2);
			String tabId = System.Configuration.ConfigurationManager.AppSettings.Get(DataClass.ToString() + "Details" + "TabId");
			
			if (tabId!=null && tabId.Length>0)
			{
				SessionManagement.SetCurrentSubContext(Owner.RowId, Owner.DataClass); //new subcontext is the datarecord we are looking at
				CurrentResponse.Redirect("~/Default.aspx?tabId="+tabId+"&"+rowIdName+"=0");	
			}
		}*/



		/// <summary>
		/// 
		/// </summary>
		/// <param name="CurrentResponse"></param>
		/// <param name="DataClass">The dataclass of the records we are going to list</param>
		/// <param name="Owner">The owner of the records we are listing</param>
		public static void GotoDataRecordListPage(HttpResponse CurrentResponse, GADataClass DataClass, GADataRecord Owner)
		{
			String tabId = System.Configuration.ConfigurationManager.AppSettings.Get(DataClass.ToString() + "Details" + "TabId");
			SessionManagement.SetCurrentSubContext(Owner); //new subcontext is the owner of the datarecord
			if (Owner != null) 
			{
				if (tabId!=null && tabId.Length>0)
				{
					CurrentResponse.Redirect("~/Default.aspx?tabId="+tabId+"&ownerclass="+Owner.DataClass.ToString()+"&ownerrowid="+Owner.RowId);	
				}
			} 
			else
			{
				//owner was set to null user differnt url for displaying next page
				GotoDataRecordListPage(CurrentResponse, DataClass);
			}
		}

		/// <summary>
		/// Redirects the response to a DotNetNuke page depending on the given dataclass. Web.config must contain a mapping of
		/// datatypes and DNN tabIDs. Redirected without setting a new GA sub context
		/// </summary>
		public static void GotoDataRecordListPage(HttpResponse CurrentResponse, GADataClass DataClass) 
		{
			String tabId = System.Configuration.ConfigurationManager.AppSettings.Get(DataClass.ToString() + "Details" + "TabId");
			if (tabId!=null && tabId.Length>0)
			{
				CurrentResponse.Redirect("~/Default.aspx?tabId="+tabId);	
			}	
		}

		/// <summary>
		/// Redirects the current response to the list my workitems page.
		/// </summary>
		public static void GotoDataRecordListMyWorkitemPage(HttpResponse CurrentResponse)
		{
			String tabId = System.Configuration.ConfigurationManager.AppSettings.Get(GADataClass.GAWorkitem.ToString() + "Details" + "TabId");
			if (tabId!=null && tabId.Length>0)
			{
				CurrentResponse.Redirect("~/Default.aspx?tabId="+tabId + "&wcurrentuser=true");	
			}	
		}

	}
}

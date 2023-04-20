using System;
using System.Web;
using GASystem.DataModel;
using System.Web.Configuration;
using GASystem.AppUtils;

namespace GASystem.GUIUtils
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
            SessionManagement.SetCurrentSubContext(Owner); //new subcontext is owner of the datarecord we are looking at
            CurrentResponse.Redirect(LinkUtils.GenerateURLForSingleRecordDetails(DataClass.ToString(), RowId.ToString())); //   "~/Default.aspx?tabId=" + tabId + "&" + rowIdName + "=" + RowId + "&edit=true");	
		}

        public static void GotoDataRecordViewPage(HttpResponse CurrentResponse, GADataClass DataClass, int RowId, GADataRecord Owner, string lastCommand)
		{
            GotoDataRecordViewPage(CurrentResponse, DataClass, RowId.ToString(), Owner, lastCommand);
		}

        public static void GotoDataRecordViewPage(HttpResponse CurrentResponse, GADataClass DataClass, int RowId, GADataRecord Owner)
        {
            GotoDataRecordViewPage(CurrentResponse, DataClass, RowId.ToString(), Owner, string.Empty);
        }

        public static void GotoDataRecordViewPage(HttpResponse CurrentResponse, GADataClass DataClass, string RowId, GADataRecord Owner, string lastCommand)
		{
            SessionManagement.SetCurrentSubContext(Owner);
            CurrentResponse.Redirect(LinkUtils.GenerateURLForSingleRecordView(DataClass.ToString(), RowId.ToString()) + "&lastcmd=" +  lastCommand);
		}

        public static void GotoDataRecordViewPage(HttpResponse CurrentResponse, GADataClass DataClass, int RowId, GADataRecord Owner, GADataClass DisplayTab)
        {
            SessionManagement.SetCurrentSubContext(Owner);
            CurrentResponse.Redirect(LinkUtils.GenerateURLForSingleRecordView(DataClass.ToString(), RowId.ToString(), DisplayTab.ToString()));
        }

		public static void GotoDataRecordViewPage(HttpResponse CurrentResponse, GADataClass DataClass, int RowId, GADataRecord Owner, GADataClass DisplayTab, string lastCommand)
		{
            SessionManagement.SetCurrentSubContext(Owner);
            CurrentResponse.Redirect(LinkUtils.GenerateURLForSingleRecordView(DataClass.ToString(), RowId.ToString(), DisplayTab.ToString()) + "&lastcmd=" +  lastCommand);
		}

		
		/// <summary>
		/// 
		/// </summary>
		/// <param name="CurrentResponse"></param>
		/// <param name="DataClass">The dataclass of the records we are going to list</param>
		/// <param name="Owner">The owner of the records we are listing</param>
		public static void GotoDataRecordListPage(HttpResponse currentResponse, GADataClass dataClass, GADataRecord owner)
		{
			SessionManagement.SetCurrentSubContext(owner); //new subcontext is the owner of the datarecord
            if (owner != null) 
			{
					currentResponse.Redirect(LinkUtils.GenerateURLForListAll(dataClass.ToString(), owner)); //   "~/Default.aspx?tabId="+tabId+"&ownerclass="+Owner.DataClass.ToString()+"&ownerrowid="+Owner.RowId);	
			} 
			else
			{
				//owner was set to null user differnt url for displaying next page
				GotoDataRecordListPage(currentResponse, dataClass);
			}
		}

		/// <summary>
		/// Redirects the response to a DotNetNuke page depending on the given dataclass. Web.config must contain a mapping of
		/// datatypes and DNN tabIDs. Redirected without setting a new GA sub context
		/// </summary>
		public static void GotoDataRecordListPage(HttpResponse CurrentResponse, GADataClass DataClass) 
		{
            CurrentResponse.Redirect(LinkUtils.GenerateURLForListAll(DataClass.ToString()));    //"~/Default.aspx?tabId="+tabId);	
		}

		/// <summary>
		/// Redirects the current response to the list my workitems page.
		/// </summary>
		public static void GotoDataRecordListMyWorkitemPage(HttpResponse currentResponse)
		{
            currentResponse.Redirect(LinkUtils.GenerateURLForListMyWorkitems());
           
		}


        public static void GotoDataRecordNewPage(HttpResponse httpResponse, GADataClass dataClass, GADataRecord owner)
        {
            SessionManagement.SetCurrentSubContext(owner); //new subcontext is owner of the datarecord we are looking at
            httpResponse.Redirect(LinkUtils.GenerateURLForNewRecord(dataClass, owner));
        }
    }
}

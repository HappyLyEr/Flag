using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace GASystem.GAGUI.GUIUtils.SessionData
{

    /// <summary>
    /// SessionInfo provides a facade to the asp.net httpcontext session object. All requests for general 
    /// session data must go via this class
    /// </summary>

    public static class SessionInfo
    {

        private const string currentUserPersonnelRowId = "currentUserPersonnelRowId";

                       
        public static string Username
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }


        public static int UserPersonnelRowId
        {
            get
            {
                object personnelRowId = HttpContext.Current.Session[currentUserPersonnelRowId];

                if (personnelRowId == null)
                {
                    personnelRowId = GASystem.BusinessLayer.User.GetPersonnelIdByLogonId(Username);
                    UserPersonnelRowId = (int)personnelRowId;
                }
                return (int)personnelRowId;
            }

            private set
            {
                HttpContext.Current.Session[currentUserPersonnelRowId] = value;
            }


        }



    }
}

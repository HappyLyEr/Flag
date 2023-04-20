using System;
using GASystem.DataModel;
using System.Data.SqlClient;
using openwfe.workitem;
//using WorkflowStarter = GASystem.BusinessLayer.WorkflowStarter;
using GASystem.DataAccess.Utils;
using System.Collections.Generic;
using System.Text;

namespace GASystem.DataAccess
{
    public class VendorDb
    {
        public VendorDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

        public static VendorDS GetAllVendorsToStart()
        {
            return GetAllVendorsToStart(null);
        }

        public static VendorDS GetAllVendorsToStart(GADataTransaction transaction)
        {
            VendorDS ds = new VendorDS();
            SqlConnection myConnection = DataUtils.GetConnection(transaction);
            SqlDataAdapter da = new SqlDataAdapter(WSVendorSQL, myConnection);
            if (transaction != null)
                da.SelectCommand.Transaction = (SqlTransaction)transaction.Transaction;

            da.Fill(ds, GADataClass.GAVendor.ToString());
            return ds;
        }

        public static string WSVendorSQL
        {
            get
            {
                // Tor 20160308 return System.Configuration.ConfigurationSettings.AppSettings.Get("CoordinatorWorkitemGivenName");
                return new GASystem.AppUtils.FlagSysResource().GetResourceString("WSVendorSQL");
            }
        }


    }
}
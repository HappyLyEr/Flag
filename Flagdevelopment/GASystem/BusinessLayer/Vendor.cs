using System;
using System.Data;
using GASystem.DataModel;
using GASystem.DataAccess;
using log4net;
using System.Collections;
using GASystem.DataAccess.Security;
using GASystem.BusinessLayer.Utils;
using System.Collections.Generic;
using System.Text;

namespace GASystem.BusinessLayer
{
    public class Vendor : BusinessClass
    {
        public static VendorDS GetAllVendorsToStart() // returns all vendors where workflow is not running
        {
            return GetAllVendorsToStart(null);
        }
        public static VendorDS GetAllVendorsToStart(GADataTransaction transaction) // returns all vendors where workflow is not running
        {
            VendorDS ds = GASystem.DataAccess.VendorDb.GetAllVendorsToStart(transaction);
            return ds;
        }
    }

}
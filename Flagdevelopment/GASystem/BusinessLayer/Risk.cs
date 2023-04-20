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
    public class Risk : BusinessClass
    {
        public static RiskDS GetAllRiskToStart() // returns all documents where workflow is not running
        {
            return GetAllRiskToStart(null);
        }
        public static RiskDS GetAllRiskToStart(GADataTransaction transaction) 
        {
            RiskDS ds = GASystem.DataAccess.RiskDb.GetAllRiskToStart(transaction);
            return ds;
        }

        // Gao 20230403 added for risk verification process
        public static RiskDS GetAllRiskVerifyToStart()
        {
            return GetAllRiskVerifyToStart(null);
        }
        public static RiskDS GetAllRiskVerifyToStart(GADataTransaction transaction)
        {
            RiskDS ds = GASystem.DataAccess.RiskDb.GetAllRiskVerifyToStart(transaction);
            return ds;
        }
    }
}

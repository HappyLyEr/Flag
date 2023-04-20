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
    public class ProjectRisk : BusinessClass
    {
        public static ProjectRiskDS GetAllProjectRiskToStart() 
        {
            return GetAllProjectRiskToStart(null);
        }
        public static ProjectRiskDS GetAllProjectRiskToStart(GADataTransaction transaction)
        {
            ProjectRiskDS ds = GASystem.DataAccess.ProjectRiskDb.GetAllProjectRiskToStart(transaction);
            return ds;
        }
    }
}

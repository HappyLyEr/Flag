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
    public class DocumentControl : BusinessClass
    {
        public static DocumentControlDS GetAllDocumentControlsToStart() // returns all documents where workflow is not running
        {
            return GetAllDocumentControlsToStart(null);
        }
        public static DocumentControlDS GetAllDocumentControlsToStart(GADataTransaction transaction) // returns all documents where workflow is not running
        {
            DocumentControlDS ds = GASystem.DataAccess.DocumentControlDb.GetAllDocumentControlsToStart(transaction);
            return ds;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace GASystem.BusinessLayer.Documents
{
    public class DocumentInfo
    {
        private string _url;
        private string _externalrefid;
        private string _displayName;
        private DocumentManagentSystem _documentSystem;
    
        public DocumentInfo(string url, string externalReferenceId, string displayName ,DocumentManagentSystem documentSystem)
        {
            _url = url;
            _externalrefid = externalReferenceId;
            _documentSystem = documentSystem;
            _displayName = displayName;
        }
    
        public string URL
        {
            get
            {
                return _url;
            }
          
        }

        public DocumentManagentSystem DocumentSystem
        {
            get
            {
                return _documentSystem;
            }
          
        }

        public string ExternalReferenceId
        {
            get
            {
               return _externalrefid;
            }
          
        }

        public string DisplayName
        {
            get
            {
                return _displayName;
            }

        }
    }
}

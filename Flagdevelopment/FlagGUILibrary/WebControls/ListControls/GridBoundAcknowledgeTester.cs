using System;
using System.Collections.Generic;
using System.Text;
using Telerik.WebControls;
using System.Data;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using GASystem.AppUtils;

namespace FlagGUILibrary.WebControls.ListControls
{
    class GridBoundAcknowledgeTester : GridBoundColumn
    {
        private string _separator = "<br/>";
        public const string ACKNOWLEDGEDBYMSG = "AcknowledgedByMessage";
        public const string AWAITINGACKNOWLEDGEDBYMSG = "AwaitingAcknowledgedByMessage";

        private FieldDescription _fd;

        public GridBoundAcknowledgeTester(FieldDescription fd)
            : base() 
        {
            _fd = fd;
        }

        public string Separator
        {
            get
            {
                return _separator;
            }
            set
            {
                _separator = value;
            }
        }

        public override bool ReadOnly
        {
            get
            {
                return true;
            }
            set
            {
                base.ReadOnly = value;
            }
        }

        private string dataClass;

        public string DataClass
        {
            get { return dataClass; }
            set { dataClass = value; }
        }

        private GASystem.AppUtils.ClassDescription cd;

        public GASystem.AppUtils.ClassDescription CD
        {
            get { return cd; }
            set { cd = value; }
        }


        private GADataRecord _owner;

        public GADataRecord Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }


        protected override string FormatDataValue(object dataValue, GridItem item)
        {
            string acknowledgeStatus = dataValue.ToString();

            if (acknowledgeStatus == Workitem.AcknowledgementStatus.AwaitingAcknowledgement.ToString())
                return "<img src=\"images/warning.png\" title=\"" + GASystem.AppUtils.Localization.GetGuiElementText(AWAITINGACKNOWLEDGEDBYMSG) + "\" >";


            if (acknowledgeStatus == Workitem.AcknowledgementStatus.Acknowledged.ToString())
                return "<img src=\"images/node.gif\" title=\"" + string.Format(GASystem.AppUtils.Localization.GetGuiElementText(ACKNOWLEDGEDBYMSG), string.Empty) + "\" >";



            return "&nbsp;";

            
            
        }

       
    }
}

using System;
using GASystem;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using System.Data;

namespace GASystem.DotNetApre.impl
{
    class GetOwnerObjectNameAgent : AbstractAgent
    {
        public const string FIELDVALUE = "GAFieldValue";
        private GADataRecord owner;

        public GetOwnerObjectNameAgent()
        {

        }
        public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
        {
            System.Console.WriteLine("Starting Get Owner Object Name Agent");
            string returnValue = string.Empty;
            //get owner for current action
            owner = DataClassRelations.GetOwner(new GADataRecord(GetActionId(wi), GADataClass.GAAction));
            if (owner != null)
            {
                returnValue = GASystem.DataAccess.DataAccess.getObjectNameFromRecord(owner.DataClass.ToString(), owner.RowId);
            }

            utils.AttributeHelper.SetAttribute(wi, FIELDVALUE, returnValue);
            return wi;
        }

    }
}

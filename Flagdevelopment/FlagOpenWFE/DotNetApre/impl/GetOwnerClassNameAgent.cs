using System;
using GASystem;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using System.Data;

namespace GASystem.DotNetApre.impl
{
    class GetOwnerClassNameAgent : AbstractAgent
    {
        public const string FIELDVALUE = "GAFieldValue";
        private GADataRecord owner;

        public GetOwnerClassNameAgent()
        {

        }
        public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
        {
            System.Console.WriteLine("Starting Get Owner Class Name Agent");
            string returnValue = string.Empty;
            //get owner for current action
            owner = DataClassRelations.GetOwner(new GADataRecord(GetActionId(wi), GADataClass.GAAction));
            if (owner != null)
            {
                returnValue = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId(GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey("GEN", owner.DataClass.ToString()));
            }

            utils.AttributeHelper.SetAttribute(wi, FIELDVALUE, returnValue);
            return wi;
        }

    }
}

using System;
using GASystem;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using System.Data;

namespace GASystem.DotNetApre.impl
{
	/// <summary>
	/// Agent for getting GAListDescription with GAListCategory and GAListValue
	/// Expected workitem attributes:
    ///		GAOwnerDataClass = GAListCategory
    ///		GAFieldName = GAListValue
    ///		returns GAListDescription in GAFieldValue 
	/// </summary>
	public class GetListDescriptionFromCodeValue : AbstractAgent
	{
        public const string GALISTCATEGORY = "GAListCategory";
        public const string GALISTVALUE = "GAListValue";
        public const string GAFIELDVALUE = "GAListDescription";
		

        public GetListDescriptionFromCodeValue()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
		{
            string myGAListCategory=utils.AttributeHelper.GetAttribute(wi,GALISTCATEGORY);
            string myGAListValue = utils.AttributeHelper.GetAttribute(wi, GALISTVALUE);
            if (myGAListCategory == "TITL")
            {
                myGAListValue = Workitem.getRoleIdentiferNamePart(myGAListValue);
            }
            
            System.Console.WriteLine("GetListDescriptionFromCodeValue " + myGAListCategory + " " + myGAListValue);
            int? myListRowId = GASystem.BusinessLayer.Lists.GetListsRowIdByCategoryAndKey(myGAListCategory, myGAListValue);
            string myGAListDescription=string.Empty;
            
            if (!myListRowId.HasValue || (int)myListRowId==-1 /*not found*/)
                throw new GASystem.GAExceptions.GAException("NOT FOUND GetListDescriptionFromCodeValue " + myGAListCategory + " " + myGAListValue);

            myGAListDescription = GASystem.BusinessLayer.Lists.GetListDescriptionByRowId((int)myListRowId);
            utils.AttributeHelper.SetAttribute(wi, GAFIELDVALUE, myGAListDescription);

            return wi;
		}
	}
}

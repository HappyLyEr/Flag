using System;
using GASystem;
using GASystem.DataModel;
using GASystem.DataAccess;
using GASystem.BusinessLayer;
using System.Data;

namespace GASystem.DotNetApre.impl
{
    // Tor 20140227 Added class to get Action ownerClass from list of ownerclasses (; separated list, eg GALocation;GAProject;GACrew;GACompany)
    // passed to GetOwnerclassAgent in field GAOwnerDataClass
    class GetOwnerclassAgent : AbstractAgent
    {
        public const string DATACLASS = "GAOwnerDataClass";
        // Tor 20171030 public const string FIELDNAME = "GAFieldName";
        public const string FIELDVALUE = "GAFieldValue";
        public const string CODEVALUE = "GAFieldCodeValue";
        private const int MAXNUMBEROFLEVELS = 20;

        // Tor 20171030 private string fieldName;
        // Tor 20171030 private GADataRecord owner;

        public GetOwnerclassAgent()
		{
			//
			// TODO: Add constructor logic here
			//
		}


        public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
        {
            System.Console.WriteLine("Get OwnerClass Agent, look for class(es): " + utils.AttributeHelper.GetAttribute(wi, DATACLASS).ToString());

            // get class(es) to look for in field GAOwnerDataclass
            string[] ownerClassesRequested = utils.AttributeHelper.GetAttribute(wi, DATACLASS).Split(';');
            if (ownerClassesRequested.Length < 1)
            {
                //table - no classes to look for have been declared
                throw new GASystem.GAExceptions.GAException("Owner dataclass and fieldname is not defined in fielddescription");
            }

            // Tor 20171030 Get path in GAAction arclink record
            SuperClassDS sds = GASystem.BusinessLayer.DataClassRelations.GetByMember(new GADataRecord(GetActionId(wi), GADataClass.GAAction));
            if (sds.GASuperClass.Rows.Count != 1)
            {
                //table - no classes to look for have been declared
                throw new GASystem.GAExceptions.GAException("GAAction memberclass has 0 or more than 1 entries in GASuperClass");
            }
            string path = sds.Tables[0].Rows[0]["Path"].ToString();

//            // Tor 20171030 removed code START
//            owner = DataClassRelations.GetOwner(new GADataRecord(GetActionId(wi), GADataClass.GAAction));
//            //get GASuperClass path for current action

//            // GADataRecord owner = null;
//            SuperClassDS superClassDS = (new SuperClassDb()).GetSuperClassByMember(owner.RowId, owner.DataClass);
//            if (superClassDS.GASuperClass.Rows.Count > 0)
//            {
//                GADataClass ownerclass = GADataRecord.ParseGADataClass(superClassDS.GASuperClass[0].OwnerClass);
//                //			if (ownerclass == GADataClass.GAFlag)
//                //				return null;   //added by jof 3.8.2006. return null when at flag root. Used for backward compatibility. TODO change code to use flagroot correctly

//                owner = new GADataRecord(superClassDS.GASuperClass[0].OwnerClassRowId, ownerclass);
//            }

//            // Tor 20140510 DO NOT UNDERSTAND THIS: return owner;



//            owner = DataClassRelations.GetOwner(new GADataRecord(GetActionId(wi), GADataClass.GAAction));
////            ownerData = (new SuperClassDb()).GetSuperClassByMember(owner.RowId, owner.DataClass);
//            DataSet ds = BusinessLayer.Utils.RecordsetFactory.GetRecordSetAllDetailsForDataClassByOwner(owner.DataClass, owner);
//            if (ds.Tables[0].Rows.Count == 0 || ds.Tables[0].Rows.Count > 1)
//                throw new GASystem.GAExceptions.GAException("Zero or more than one superclassrecords found");
//            string path = ds.Tables[0].Rows[0]["Path"].ToString();
//            // Tor 20171030 removed code END
            if (path.Length < 5)
                    throw new GAExceptions.GAException("No or invalid path in GASuperClass. Path="+path);
            path = path.Substring(1, path.Length - 2); //remove first and last '/' in path
            string[] pathElements = path.Split('/');
            if (pathElements.Length<1)
                    throw new GAExceptions.GAException("Invalid definition sibling class and field for sibling filter");


            // find class in path. Compare path with classes to look for. Search from righ in path and return the first class that matches. This will be the lowest level action owner class
            for(int i=pathElements.Length-1; i>-1; i--)
            {
                string[] pathPart = pathElements[i].Split('-');
                // Tor 20171030 for (int j = 0; j < ownerClassesRequested.Length; i++)
                for (int j = 0; j < ownerClassesRequested.Length; j++)
                {
                    if (ownerClassesRequested[j] == pathPart[0]) // class found
                    {   // get class and classrowid from Path
                        utils.AttributeHelper.SetAttribute(wi, FIELDVALUE, pathPart [0]);
                        utils.AttributeHelper.SetAttribute(wi, CODEVALUE , pathPart [1]);
                        return wi;
                    }
                }
            }

            // Tor 20171028 Class not found
            //utils.AttributeHelper.SetAttribute(wi, FIELDVALUE, string.Empty);
            //return wi;
            string a = utils.AttributeHelper.GetAttribute(wi, DATACLASS);
            throw new GASystem.GAExceptions.GAException("OwnerClass not found in classlist: " + a);
        }
    }
}

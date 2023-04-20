using System;
using GASystem;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using GASystem.DataAccess;
using System.Data;

namespace GASystem.DotNetApre.impl
{
    class OpportunityGoPublicAgent : AbstractAgent
    {
        //public const string DATACLASS = "GAOwnerDataClass";
        //public const string FIELDNAME = "GAFieldName";
        //public const string FIELDVALUE = "GAFieldValue";
        //public const string CODEVALUE = "GAFieldCodeValue";
        //private const int MAXNUMBEROFLEVELS = 20;
        private const string memberclassName = "GAOpportunityDetail";
		
        //private string fieldName;

        private GADataRecord owner;
        private GADataClass memberclass;

        public OpportunityGoPublicAgent()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public override openwfe.workitem.InFlowWorkitem Use(openwfe.workitem.InFlowWorkitem wi)
		{
            System.Console.WriteLine("Starting Opportunity Go Public Agent");
            //get owner for current action
            owner = DataClassRelations.GetOwner(new GADataRecord(GetActionId(wi), GADataClass.GAAction));
            
            //expected dataclass GAOpportunity, used to verify returned owner
            if (owner.DataClass.ToString() != "GAOpportunity")
            {
				System.Console.WriteLine("OwnerClass is not GAOpportunity. is: " + owner.DataClass.ToString()+ " No updates performed");
				return wi;
            }

            // Get GAOpportunityDetail record
            memberclass = GADataRecord.ParseGADataClass(memberclassName);
            if (memberclass.ToString()!=memberclassName)
            {
				System.Console.WriteLine("Memberclass "+memberclassName+" does not exist. No updates performed");
				return wi;
            }
            SuperClassDS sds = GASystem.DataAccess.SuperClassDb.GetSuperClassByOwnerAndMemberClass(owner, memberclass,null);
            if (sds==null)
            {
				System.Console.WriteLine("No "+memberclassName+" records exists in ArcLink. No updates performed");
				return wi;
            }
            // Get owner record
			BusinessLayer.BusinessClass bc = BusinessLayer.Utils.RecordsetFactory.Make(owner.DataClass);
			DataSet ds = bc.GetByRowId(owner.RowId);
            
            // get member record
            string a=sds.Tables[0].Rows[0]["MemberClass"].ToString();
            int b=(int)sds.Tables[0].Rows[0]["MemberClassRowId"];
            GADataClass c=new GADataRecord(b,GADataRecord.ParseGADataClass(a)).DataClass;
            
            GADataRecord memberRecord = new GADataRecord(b, c);

            BusinessLayer.BusinessClass bcm = BusinessLayer.Utils.RecordsetFactory.Make(memberRecord.DataClass);
			DataSet dsm = bcm.GetByRowId(memberRecord.RowId);
            if (dsm == null)
            {
                System.Console.WriteLine("No " + memberclassName + " records exists. No updates performed");
                return wi;
            }
            string subject=dsm.Tables[0].Rows[0]["Subject"].ToString();

            // update owner record from memberrecord
            string sqlUpdate = "Update GAOpportunity set Subject='{0}',Description='{1}',ReasonsOrFactsIllustrating='{2}',SuggestedImplementation='{3}' where OpportunityRowId={4}";
            string sql=string.Format(sqlUpdate
                ,dsm.Tables[0].Rows[0]["Subject"].ToString()
                ,dsm.Tables[0].Rows[0]["Description"].ToString()
                ,dsm.Tables[0].Rows[0]["ReasonsOrFactsIllustrating"].ToString()
                ,dsm.Tables[0].Rows[0]["SuggestedImplementation"].ToString()
                ,ds.Tables[0].Rows[0]["OpportunityRowId"]);
            //ds.Tables[0].Rows[0]["Subject"]=dsm.Tables[0].Rows[0]["Subject"];
            //ds.Tables[0].Rows[0]["Description"]=dsm.Tables[0].Rows[0]["Description"];
            //ds.Tables[0].Rows[0]["ReasonsOrFactsIllustrating"]=dsm.Tables[0].Rows[0]["ReasonsOrFactsIllustrating"];
            //ds.Tables[0].Rows[0]["SuggestedImplementation"]=dsm.Tables[0].Rows[0]["SuggestedImplementation"];
            //ds.Tables[0].Rows[0]["Comment"]=dsm.Tables[0].Rows[0]["Comment"];
            //bc.UpdateDataSetNoOwner(ds, null);
            GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
            int i = GASystem.DataAccess.DataUtils.executeNoneQuery(sql, transaction);
            transaction.Commit();
            //bc.CommitDataSet(ds);

            System.Console.WriteLine("GAOpportunity with subject "+subject+" has been updated");
			
            // delete member record
            bcm.DeleteRow(memberRecord.RowId);
            System.Console.WriteLine("GAOpportunityDetail with subject "+subject+" has been deleted");
			
			return wi;
		}
    }
}

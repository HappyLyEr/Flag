using System;
using System.Data;
using GASystem.DataModel;
using GASystem.BusinessLayer;
using System.Collections;
using System.Collections.Generic;
using GASystem.DataAccess;
using GASystem.AppUtils;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for DataClassRelations.
	/// </summary>
	public class DataClassRelations
	{
		public DataClassRelations()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static void CreateDataClassRelation(int OwnerRowId, GADataClass OwnerDataClass, int MemberRowId, GADataClass MemberClass)
		{
			CreateDataClassRelation(OwnerRowId, OwnerDataClass, MemberRowId, MemberClass, null);
		}

		public static void CreateDataClassRelation(int OwnerRowId, GADataClass OwnerDataClass, int MemberRowId, GADataClass MemberClass, GADataTransaction transaction)
		{
			//get path for owner
			string path = string.Empty;
			SuperClassDS superClassDS = (new SuperClassDb()).GetSuperClassByMember(OwnerRowId, OwnerDataClass, transaction);
			if (superClassDS.GASuperClass.Rows.Count > 0 && !((SuperClassDS.GASuperClassRow)superClassDS.GASuperClass.Rows[0]).IsPathNull())
				path = ((SuperClassDS.GASuperClassRow)superClassDS.GASuperClass.Rows[0]).Path;
			if (path == string.Empty)
				path = "/";
			path = path + OwnerDataClass.ToString() + "-" + OwnerRowId.ToString()  + "/";
			
			SuperClassDS superClassSet = new SuperClassDS();
			SuperClassDS.GASuperClassRow row = superClassSet.GASuperClass.NewGASuperClassRow();
			row.OwnerClassRowId = OwnerRowId;
			row.OwnerClass = OwnerDataClass.ToString();
			row.MemberClassRowId = MemberRowId;
			row.MemberClass = MemberClass.ToString();
			row.Path = path;
            // Tor 20160929 Add CreatedBy from to enable record creator without view permission to see record created by him/her self
            row.CreatedBy = GASystem.DataAccess.Security.GASecurityDb_new.GetPersonnelRowIdByLogonId(GASystem.DataAccess.Security.GASecurityDb_new.GetCurrentUserId().ToString());
            row.DateCreated = System.DateTime.UtcNow;
            // Tor 20160929 End
			superClassSet.GASuperClass.Rows.Add(row);
			SuperClassDb.UpdateSuperClass(superClassSet, transaction);
		}

		public static void DeleteDataClassRelation(GADataRecord Owner, GADataRecord Member, GADataTransaction Transaction)
		{
			SuperClassDS ds = SuperClassDb.GetSuperClassByOwnerAndMember(Owner, Member, Transaction);
			if (ds.GASuperClass.Rows.Count == 1) 
			{
				ds.GASuperClass[0].Delete();
				SuperClassDb.UpdateSuperClass(ds, Transaction);
			} else
				throw new Exception("Could not find arc link for dataclass");
		}

        public static void DeleteAllMembersInDataClassRelation(GADataRecord Owner, GADataClass MemberClass, GADataTransaction Transaction) {
            SuperClassDS ds = SuperClassDb.GetSuperClassByOwnerAndMemberClass(Owner, MemberClass, Transaction);

            if (ds.GASuperClass.Rows.Count > 0)
            {
                for (int n = 0; n < ds.GASuperClass.Rows.Count; n++)
                {
                    ds.GASuperClass[n].Delete();
                }
                SuperClassDb.UpdateSuperClass(ds, Transaction);
            }
        }
        

		public static DataSet GetOwnerDataRecord(GADataRecord Member) 
		{
			GADataRecord owner = GetOwner(Member);
			return Utils.RecordsetFactory.GetRecordSetByDataRecord(owner);
		}

		public static GADataRecord GetOwner(GADataRecord Member, GADataTransaction Transaction)
		{
			GADataRecord owner = null;
            
            if (Member.DataClass == GADataClass.GAFlag) return owner; /* // Tor 20170110 */

            SuperClassDS superClassDS = (new SuperClassDb()).GetSuperClassByMember(Member.RowId, Member.DataClass, Transaction);
			if (superClassDS.GASuperClass.Rows.Count>0)
			{
				GADataClass ownerclass = GADataRecord.ParseGADataClass(superClassDS.GASuperClass[0].OwnerClass);
	//			if (ownerclass == GADataClass.GAFlag)
	//				return null;   //added by jof 3.8.2006. return null when at flag root. Used for backward compatibility. TODO change code to use flagroot correctly

				owner = new GADataRecord(superClassDS.GASuperClass[0].OwnerClassRowId, ownerclass );
			}
			return owner;
		}

		public static GADataRecord GetOwner(GADataRecord Member) 
		{
			return GetOwner(Member, null);
		}

		public static SuperClassDS GetByMember(GADataRecord Member) 
		{
            return (new SuperClassDb()).GetSuperClassByMember(Member.RowId, Member.DataClass);
		}

		public static SuperClassDS GetMembers(GADataRecord Owner)
		{
			return SuperClassDb.GetSuperClassByOwner(Owner.RowId, Owner.DataClass.ToString());
		}

		public static bool HasMembers(GADataRecord DataRecord) 
		{
			bool result = false;
			if (SuperClassDb.GetNumberOfMembers(DataRecord.RowId, DataRecord.DataClass.ToString()) > 0)
				result = true;
			
			//if datarecord is of type action, return true if it has a started workflow attached
			//TODO expand this to a general check on all classes with regard to "foreign key"  links
			if (DataRecord.DataClass == GADataClass.GAAction) 
			{
				ActionDS action = Action.GetActionByActionRowId(DataRecord.RowId);
				if (!action.GAAction[0].IsWorkflowIdNull())
					result = true;
			}
			return result;
		}

		/// <summary>
		/// Returns a list of dataclasses defined on next level in dataclass hierarcy. (Defined in databasetable GASuperClassLinks)
		/// </summary>
		/// <param name="OwnerDataClass"></param>
		/// <returns></returns>
		public static ArrayList GetNextLevelDataClasses(GADataClass OwnerDataClass)
		{
			ArrayList dataClassList = new ArrayList();
			SuperClassLinksDS superclassLinksData = SuperClassDb.GetSuperClassLinksByOwner(OwnerDataClass.ToString());
			foreach (SuperClassLinksDS.GASuperClassLinksRow row in superclassLinksData.GASuperClassLinks)
			{
				dataClassList.Add(row.MemberClass);
			}
			return dataClassList;
		}

        /// <summary>
        /// Returns a list of dataclasses defined on parent level in dataclass hierarcy. (Defined in databasetable GASuperClassLinks)
        /// </summary>
        /// <param name="OwnerDataClass"></param>
        /// <returns></returns>
        public static ArrayList GetParentLevelDataClasses(GADataClass memberDataClass)
        {
            ArrayList dataClassList = new ArrayList();
            SuperClassLinksDS superclassLinksData = SuperClassDb.GetSuperClassLinksByMember(memberDataClass);
            foreach (SuperClassLinksDS.GASuperClassLinksRow row in superclassLinksData.GASuperClassLinks)
            {
                dataClassList.Add(row.OwnerClass);
            }
            return dataClassList;
        }

        /// <summary>
        /// Returns a list of datarecords defined on parent level in dataclass hierarcy. (Defined in databasetable GASuperClassLinks)
        /// </summary>
        /// <param name="MemberDataRecord"></param>
        /// <returns></returns>
        public static System.Collections.Generic.List<GADataRecord> GetCurrentParentLevelDataRecords(GADataRecord memberRecord)
        {
            System.Collections.Generic.List<GADataRecord> dataRecordList = new System.Collections.Generic.List<GADataRecord>();
            GADataRecord ownerRecord=memberRecord;
            while (ownerRecord != null && ownerRecord.DataClass.ToString() != "GAFlag")
            {   // if owner is current
                ownerRecord = GetOwner(ownerRecord);
                if (ownerRecord != null && ownerRecord.DataClass.ToString() != "GAFlag" && GASystem.BusinessLayer.Utils.RecordSetUtils.IsRecordCurrent(ownerRecord, System.DateTime.Now))
                {
                    dataRecordList.Add(ownerRecord);
                    bool isFound=true;
                    GADataRecord foundRecord=ownerRecord;
                    while (isFound && foundRecord!=null)
                    {
                        List<GADataRecord> manyToManyOwnerDataRecordsFound = new List<GADataRecord>();
                        // returns next level upwards from owner data record where many to many record is current 
                        manyToManyOwnerDataRecordsFound = GASystem.BusinessLayer.ManyToManyLinks.GetManyToManyOwnerDataRecordsByMemberDataRecordAndDate(foundRecord, System.DateTime.Now);
                        isFound = false;
                        foreach (GADataRecord mrecord in manyToManyOwnerDataRecordsFound)
                        { // returns link records, now get owner of link record
                            foundRecord = GetOwner(mrecord);
                            if (foundRecord != null && GASystem.BusinessLayer.Utils.RecordSetUtils.IsRecordCurrent(ownerRecord, System.DateTime.Now))
                            {
                                isFound = true;
                                dataRecordList.Add(foundRecord);
                            }
                            else
                            {
                                isFound = false;
                            }
                        }
                    }
                }
            }
            return dataRecordList;
        }
		
		
		/// <summary>
		/// Returns a list of dataclasses defined on next level in dataclass hierarcy and is marked to be displayed inline in views
		/// </summary>
		/// <param name="OwnerDataClass"></param>
		/// <returns></returns>
		public static ArrayList GetNextLevelInViewDataClasses(GADataClass OwnerDataClass)
		{
			ArrayList dataClassList = new ArrayList();
			SuperClassLinksDS superclassLinksData = SuperClassDb.GetSuperClassLinksByOwner(OwnerDataClass.ToString());
			foreach (SuperClassLinksDS.GASuperClassLinksRow row in superclassLinksData.GASuperClassLinks)
			{
				if (!row.IsSwitchFree1Null() && row.SwitchFree1)
					dataClassList.Add(row.MemberClass);
			}
			return dataClassList;
		}

        // Tor 20150924 Added to decide if TAB is to be displayed based on permissions and specifications in SuperClassLinks record (nTextFree1)
        public static ArrayList GetNextLevelInViewDataClasses(GADataRecord Owner)
        {
            ArrayList dataClassList = new ArrayList();
            SuperClassLinksDS superclassLinksData = SuperClassDb.GetSuperClassLinksByOwner(Owner.DataClass.ToString());
            foreach (SuperClassLinksDS.GASuperClassLinksRow row in superclassLinksData.GASuperClassLinks)
            {
                if (!row.IsSwitchFree1Null() && row.SwitchFree1)
                { 
                    if (IsIncludeMemberClass(Owner, row))
                        dataClassList.Add(row.MemberClass);
                }
            }
            return dataClassList;
        }
        
        /// <summary>
		/// Returns a list of dataclasses defined on next level in dataclass hierarcy and is marked to be displayed in tabs
        /// comment, 
		/// </summary>
		/// <param name="OwnerDataClass"></param>
		/// <returns></returns>
		public static ArrayList GetNextLevelTabDataClasses(GADataClass OwnerDataClass)
		{
			ArrayList dataClassList = new ArrayList();
			SuperClassLinksDS superclassLinksData = SuperClassDb.GetSuperClassLinksByOwner(OwnerDataClass.ToString());
			foreach (SuperClassLinksDS.GASuperClassLinksRow row in superclassLinksData.GASuperClassLinks)
			{
				if (row.IsSwitchFree1Null() || !row.SwitchFree1)
                   // if (!row.MemberClass.Equals(GADataClass.GAAction.ToString(), StringComparison.CurrentCultureIgnoreCase))
                    if (row.IsSwitchFree2Null() || !row.SwitchFree2)   
                         dataClassList.Add(row.MemberClass);
			}
			return dataClassList;
		}

        // Tor 20150924 Added to decide if TAB is to be displayed based on permissions and specifications in SuperClassLinks record (nTextFree1)
        // Tor 201611 Security 20161111 Added test if OwnerClass is GAPersonnel, then get PersonnelRowId in current user and display all tabs if if current personnel=OwnerClass.RowId 
        public static ArrayList GetNextLevelTabDataClasses(GADataRecord owner)
        {
            // Tor 201611 Security 20161111 start
            bool isGAPersonnelOwner = false;
            if (owner.DataClass.ToString() == "GAPersonnel")
            {
//                int personnelRowId = GASystem.DataAccess.Security.GASecurityDb_new.GetPersonnelRowIdByLogonId(AppUtils.GAUsers.GetUserId());
                if (GASystem.DataAccess.Security.GASecurityDb_new.GetPersonnelRowIdByLogonId(AppUtils.GAUsers.GetUserId()) == owner.RowId)
//                    if (personnelRowId == owner.RowId)
                {
                    isGAPersonnelOwner = true;
                }
            }
            // Tor 201611 Security 20161111 end

            ArrayList dataClassList = new ArrayList();
            SuperClassLinksDS superclassLinksData = SuperClassDb.GetSuperClassLinksByOwner(owner.DataClass.ToString());
            foreach (SuperClassLinksDS.GASuperClassLinksRow row in superclassLinksData.GASuperClassLinks)
            {
                if (row.IsSwitchFree1Null() || !row.SwitchFree1) // switchFree1=false = present memberclass in TAB
                    if (row.IsSwitchFree2Null() || !row.SwitchFree2) // switchFree2=false = memberclass is NOT hidden
                        if (IsIncludeMemberClass(owner, row))
                            dataClassList.Add(row.MemberClass);

            // Tor 201611 Security 20161111 start: add if isGAPersonnelOwner (owner=GAPersonnel and ownerRowId is current logged in personnel) and not hide Tab when ownerrowid=current personnelrowid
                        else
                            if (isGAPersonnelOwner && (row.IsInlineDisplayNull() || !row.InlineDisplay))
                                dataClassList.Add(row.MemberClass);
                // Tor 201611 Security 20161111 end

            }
            return dataClassList;
        }
		

		public static ArrayList GetNextLevelManyToManyDataClasses(GADataClass OwnerDataClass) 
		{
			ArrayList allNextLevel = GetNextLevelDataClasses(OwnerDataClass);

			ArrayList manyToMany = new ArrayList();

			foreach (string dataClassString in allNextLevel) 
			{
				GADataClass dataClass = GADataRecord.ParseGADataClass(dataClassString);
				ClassDescription cd = ClassDefinition.GetClassDescriptionByGADataClass(dataClass);
				if (cd.isClassManyToManyLink()) 
				{
					manyToMany.Add(dataClassString);
				}
			}
			return manyToMany;
		}

		public static bool IsDataClassValidMember(GADataClass OwnerDataClass, GADataClass MemberDataClass) 
		{
			ArrayList dataClassList = GetNextLevelDataClasses(OwnerDataClass);
			return dataClassList.Contains(MemberDataClass.ToString());
		}

		
		public static ClassDS GetAlleClasses() 
		{

            // Tor 20140515 replaced with below to conform DataAccess class names with standard <class>DS: return Class.GetAllClasses();
            return ClassDb.GetAllClasses();
		}


		public static void SetNewOwner(GADataRecord DataRecord, GADataRecord NewOwner) 
		{
			//1.	Find the record in gasuperclass where the object is listed as member.
			SuperClassDS sds = DataClassRelations.GetByMember(DataRecord);
			//2.	Store existing path of the object in a variable oldPath.
			string oldPath;
			if (sds.GASuperClass.Rows.Count == 0)
				oldPath = "/"; //record is toplevel node
			else
				oldPath = sds.GASuperClass[0].Path;
			//add data from DataRecord to path
			oldPath = oldPath + DataRecord.DataClass.ToString() + "-" + DataRecord.RowId.ToString() + "/";

			//get new owner path
			SuperClassDS ownersds = DataClassRelations.GetByMember(NewOwner);
			string ownerPath;
			if (ownersds.GASuperClass.Rows.Count == 0)
				ownerPath = "/"; //record is toplevel node
			else
				ownerPath = ownersds.GASuperClass[0].Path;
			//3.	Update OwnerClass and OwnerClassRowId to new value
			//4.	Set new path to owner objects path + “<ownerclassname>-<ownerclassrowid>/”.
			

			//start transaction
			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			try 
			{


				if (sds.GASuperClass.Rows.Count == 0) 
				{
					//toplevel record is moved, create new relation

					//get by member again in order to get generated path

				} 
				else 
				{
					//update existing relation
					sds.GASuperClass[0].OwnerClass = NewOwner.DataClass.ToString();
					sds.GASuperClass[0].OwnerClassRowId = NewOwner.RowId;
					sds.GASuperClass[0].Path = ownerPath + NewOwner.DataClass.ToString() + "-" + NewOwner.RowId.ToString() + "/";

					sds = SuperClassDb.UpdateSuperClass(sds, transaction);   //TODO add transaction !!!!!!!

				}

				//5.	Store new path in a variable newPath.
				string newPath = sds.GASuperClass[0].Path;
				//add DataRecord to path;
				newPath = newPath + DataRecord.DataClass.ToString() + "-" + DataRecord.RowId.ToString() + "/";
			
				//6.	Find all object having a path starting with oldPath + recordclass and recordrowid
				//7.	For each object in 5 do a string replace of the path of oldPath to newPath
				SuperClassDb.UpdateMembersOwnerPath(oldPath, newPath, transaction);
				transaction.Commit();
			}
			catch (Exception ex)
			{
				transaction.Rollback();
				throw ex;
			}
			finally 
			{
				transaction.Connection.Close();
			}



		}
        private static bool IsIncludeMemberClass(GADataRecord owner,SuperClassLinksDS.GASuperClassLinksRow row)
        {
            // check if user has read or create permissions on owner member relation
            if (GASystem.DataAccess.Security.GASecurityDb_new.IsGAAdministrator() ||
                // Tor 201611 Security 20161114 
                GASystem.DataAccess.Security.GASecurityDb_new.HasStaticReadOrCreateAccessToMemberUnderOwner(GADataRecord.ParseGADataClass(row.MemberClass.ToString()), GADataRecord.ParseGADataClass(row.OwnerClass.ToString())) 
                // Tor 20170222 Rollback 201611 ||
                // GASystem.DataAccess.Security.GASecurityDb_new.HasStaticReadOrCreateAccessToMemberUnderOwner(owner,GADataRecord.ParseGADataClass(row.MemberClass.ToString()))
                )
            {
                if (!row.IsnTextFree1Null()) // contains sql script to decide if tab is to be excluded
                {
                    // check if owner member is to be included based on sql query defined in GASuperclassLinks.nTextFree
                    new GASystem.BusinessLayer.TABUtils.TABSQLExpression();
                    return GASystem.BusinessLayer.TABUtils.TABSQLExpression.isTabVisible(owner, row);
                }
                else
                {
                    return true;
                }
            }
            return false;

            // eg sql statement: 
            // if exists (select * from <ownerclass> o 
            // inner join GALists l on l.ListsRowId = o.columnname and l.GAListCaategory='xxx' and l.GAListValue='shore' 
            // where o.<ownerclassrowid> = <ownerclassrowid>
            // permissions: check if user has read or create permissions to owner/member combination (when IntFree1=1)
        }


        // Tor 201611 Security 20161112 Check if class relation is closed for current user when access is based on personnelrowid (used to restrict current logged in user tab access and record listing)
        public static bool IsClassRelationLockedForCurrentUser(GADataClass OwnerDataClass, GADataClass MemberDataClass)
        {
            SuperClassLinksDS ds = GASystem.DataAccess.SuperClassDb.GetSuperClassLinksByOwnerAndMember(OwnerDataClass, MemberDataClass);
            if (ds.Tables[0].Rows[0]["InlineDisplay"] == DBNull.Value || (System.Boolean)ds.Tables[0].Rows[0]["InlineDisplay"] == false)
                return false;
            return true;
        }


	}

	
}

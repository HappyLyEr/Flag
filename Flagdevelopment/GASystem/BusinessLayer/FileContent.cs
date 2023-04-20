using System;
using GASystem.DataModel;
using GASystem.DataAccess;
using System.Data;


namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for FileContent.
	/// </summary>
	public class FileContent : BusinessClass
	{
		public FileContent()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAFileContent;
		}

        // Tor 20161028 use std method in : BusinessClass
        //public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
        //{
        //    return UpdateFileContent((FileContentDS)ds, transaction);
        //}
		
		public override System.Data.DataSet GetByRowId(int RowId)
		{
			return GetFileContentByFileContentRowId(RowId);
		}

		public static FileContentDS GetNewFileContent() 
		{
			FileContentDS dsFileContent = new FileContentDS();
			dsFileContent.GAFileContent.Rows.Add(dsFileContent.GAFileContent.NewGAFileContentRow());
			return dsFileContent;
		}
		
		public static FileContentDS GetFileContentByFileContentRowId(int FileContentRowId)
		{
			return FileContentDb.GetFileContentByFileContentRowId(FileContentRowId);
		}

		public static FileContentDS GetFileContentNoContentByFileContentRowId(int FileContentRowId)
		{
			return FileContentDb.GetFileContentNoContentByFileContentRowId(FileContentRowId);
		}

		public static FileContentDS GetFileContentsByOwner(GADataRecord IncidentOwner)
		{
			return FileContentDb.GetFileContentsByOwner(IncidentOwner.RowId, IncidentOwner.DataClass);
		}

		//SaveNewFileContent(FileContentData, FileContentOwner)

		public static FileContentDS SaveNewFileContent(FileContentDS FileContentSet, GADataRecord FileContentOwner)
		{
//			if (FileContentSet.GAFileContent[0].IsDateAndTimeOfIncidentNull())
//				FileContentSet.GAFileContent[0].DateAndTimeOfIncident = System.DateTime.Now;
			//Generate ReferenceId
//			FileContentSet.GAFileContent[0].IncidentId = IdGenerator.GenerateId(GADataClass.GAFileContent, FileContentOwner, FileContentSet.GAFileContent[0].DateAndTimeOfIncident);

			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
			try
			{
				FileContentSet = UpdateFileContent(FileContentSet, transaction);
				DataClassRelations.CreateDataClassRelation(FileContentOwner.RowId, FileContentOwner.DataClass, FileContentSet.GAFileContent[0].FileContentRowId, GADataClass.GAFileContent, transaction);
				//add member classes
			//	Utils.StoreObject.AddMemberClasses(new GADataRecord(FileContentSet.GAFileContent[0].FileContentRowId, GADataClass.GAFileContent), transaction);
			
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
			return FileContentSet;
		}

		public static FileContentDS UpdateFileContent(FileContentDS FileContentSet)
		{
			return UpdateFileContent(FileContentSet, null);
		}
		public static FileContentDS UpdateFileContent(FileContentDS FileContentSet, GADataTransaction transaction)
		{
			return FileContentDb.UpdateFileContent(FileContentSet, transaction);
		}
	}
}

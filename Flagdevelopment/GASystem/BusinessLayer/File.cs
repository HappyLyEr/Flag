using System;
using GASystem.DataModel;
using GASystem.DataAccess;
using System.Data;



namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for File.
	/// </summary>
	public class File : BusinessClass
	{
		public File()
		{
			//
			// TODO: Add constructor logic here
			//
			DataClass = GADataClass.GAFile;
		}

		public override void DeleteRow(int RowId)
		{
			//get record
			FileDS fds = (FileDS)this.GetByRowId(RowId);


			string fileName = "llnotafile";
			if (!fds.GAFile[0].IsurlNull())
			fileName = URLPath + fds.GAFile[0].url;

			//delete record
			base.DeleteRow (RowId);
			//delete file from disk
			if (System.IO.File.Exists(fileName) )
				System.IO.File.Delete(fileName);

		}


//		public override System.Data.DataSet Update(System.Data.DataSet ds, GADataTransaction transaction)
//		{
//			return UpdateFile((FileDS)ds, transaction);
//		}
//		
//		public override System.Data.DataSet GetByRowId(int RowId)
//		{
//			return GetByRowId(RowId, null);
//			//return GetFileByFileRowId(RowId);
//		}
//
//		public static FileDS GetNewFile() 
//		{
//			FileDS dsFile = new FileDS();
//			dsFile.GAFile.Rows.Add(dsFile.GAFile.NewGAFileRow());
//			return dsFile;
//		}
		
//		public static FileDS GetFileByFileRowId(int FileRowId)
//		{
//			return FileDb.GetFileByFileRowId(FileRowId);
//			
//		}
//
//		public static FileDS GetFileNoContentByFileRowId(int FileRowId)
//		{
//			return FileDb.GetFileNoContentByFileRowId(FileRowId);
//		}
//
//		public static FileDS GetFilesByOwner(GADataRecord IncidentOwner)
//		{
//			return FileDb.GetFilesByOwner(IncidentOwner.RowId, IncidentOwner.DataClass);
//		}
//
//		//SaveNewFile(FileData, FileOwner)
//
//		public static FileDS SaveNewFile(FileDS FileSet, GADataRecord FileOwner)
//		{
////			if (FileSet.GAFile[0].IsDateAndTimeOfIncidentNull())
////				FileSet.GAFile[0].DateAndTimeOfIncident = System.DateTime.Now;
//			//Generate ReferenceId
////			FileSet.GAFile[0].IncidentId = IdGenerator.GenerateId(GADataClass.GAFile, FileOwner, FileSet.GAFile[0].DateAndTimeOfIncident);
//
//			GADataTransaction transaction = GADataTransaction.StartGADataTransaction();
//			try
//			{
//				FileSet = UpdateFile(FileSet, transaction);
//				DataClassRelations.CreateDataClassRelation(FileOwner.RowId, FileOwner.DataClass, FileSet.GAFile[0].FileRowId, GADataClass.GAFile, transaction);
//				//add member classes
//			//	Utils.StoreObject.AddMemberClasses(new GADataRecord(FileSet.GAFile[0].FileRowId, GADataClass.GAFile), transaction);
//			
//				transaction.Commit();
//			}
//			catch (Exception ex)
//			{
//				transaction.Rollback();
//				throw ex;
//			}
//			finally 
//			{
//				transaction.Connection.Close();
//			}
//			return FileSet;
//		}
//
//		public static FileDS UpdateFile(FileDS FileSet)
//		{
//			return UpdateFile(FileSet, null);
//		}
//		public static FileDS UpdateFile(FileDS FileSet, GADataTransaction transaction)
//		{
//			return FileDb.UpdateFile(FileSet, transaction);
//		}
		public static FileDS GetAllFiles() 
		{
			return FileDb.GetAllFiles();
		}

		public static  string URLPath
		{
			get 
			{
				//return "ga"; 
                string path = System.Configuration.ConfigurationManager.AppSettings.Get("FileURLPath");
				if (path.LastIndexOf("\\") != path.Length - 1)  //check that last characther is a backslash
					path = path + "\\";
				//TODO throw exception if path is invalid
				return path;
			}	
		}

		public static  string TemporaryPath
		{
			get 
			{
				//return "ga"; 
                string path = System.Configuration.ConfigurationManager.AppSettings.Get("TemporaryPath");
				if (path.LastIndexOf("\\") != path.Length - 1)  //check that last characther is a backslash
					path = path + "\\";
				//TODO throw exception if path is invalid
				return path;
			}	
		}


	}
}

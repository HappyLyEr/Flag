using System;
using GASystem.DataAccess;
using GASystem.DataModel;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for Document.
	/// </summary>
	//public class Document : BusinessClass
	public class Document 

	{
		public Document()
		{
			//
			// TODO: Add constructor logic here
			//
			//DataClass = GADataClass.GADocument;
		}

//		public override System.Data.DataSet Update(System.Data.DataSet ds)
//		{
//			return null;
//		}

//		public override System.Data.DataSet GetByRowId(int RowId)
//		{
//			return null;
//		}

		

		public static DocumentDS GetGroupDocuments(int groupId)
		{
			//DocumentDS dds = new DocumentDS();
			return DocumentDb.GetGroupDocuments(groupId);
		}

		public static DocumentDS GetDocument(int documentId)
		{
			//DocumentDS dds = new DocumentDS();
			return DocumentDb.GetDocument(documentId);
		}

		public static DocumentDS GetNewDocument(int groupId)
		{
			DocumentDS dds = new DocumentDS();
			dds = DocumentDb.GetDocumentGroup(groupId);
			DocumentDS.GADocumentRow row = dds.GADocument.NewGADocumentRow();
			
			//commented out by jof 8/-2005 update of all tables showed that these fields does not longer exist
			//row.DocumentGroupId = groupId;
			//row.SetParentRow(dds.GADocumentGroup[0]);
			//row.VersionNo = "0";
			//row.DocumentId = 1;
			dds.GADocument.Rows.Add(row);
			return dds;
		}


	}
}

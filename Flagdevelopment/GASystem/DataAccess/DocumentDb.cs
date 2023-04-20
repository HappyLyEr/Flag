using System;
using GASystem.DataModel;
using System.Data;
using System.Data.SqlClient;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for DocumentDb.
	/// </summary>
	public class DocumentDb
	{

		private static string _selectDocumentGroup = "SELECT * FROM GADocumentGroup ";
		private static string _selectDocuments = "SELECT * FROM GADocument ";

		public DocumentDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static DocumentDS GetGroupDocuments(int documentGroupId)
		{
			DocumentDS docDs = new DocumentDS();
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

			String selectDocumentGroup = _selectDocumentGroup + " WHERE DocumentGroupId=" + documentGroupId;
			String selectGroupDocuments = _selectDocuments + "WHERE DocumentGroupId=" + documentGroupId;
			
			SqlDataAdapter documentGroupDa = new SqlDataAdapter(selectDocumentGroup, myConnection);
			SqlDataAdapter documentDa = new SqlDataAdapter(selectGroupDocuments, myConnection);
			
			documentGroupDa.Fill(docDs,"GADocumentGroup");
			documentDa.Fill(docDs,"GADocument");

			return docDs;
		}

		public static DocumentDS GetDocument(int documentId)
		{
			DocumentDS docDs = new DocumentDS();
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

			String selectDocuments = _selectDocuments + "WHERE DocumentId=" + documentId;
			SqlDataAdapter documentDa = new SqlDataAdapter(selectDocuments, myConnection);
			
			documentDa.Fill(docDs,"GADocument");

			return docDs;
		}

		public static DocumentDS GetDocumentGroup(int groupId)
		{
			DocumentDS docDs = new DocumentDS();
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());

			String selectDocumentGroup = _selectDocumentGroup + " WHERE DocumentGroupId=" + groupId;
			SqlDataAdapter documentDa = new SqlDataAdapter(selectDocumentGroup, myConnection);
			
			documentDa.Fill(docDs,"GADocumentGroup");
			return docDs;

		}
	}
}

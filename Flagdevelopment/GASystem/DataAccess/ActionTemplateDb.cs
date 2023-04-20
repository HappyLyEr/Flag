using System;
using GASystem.DataModel;
using System.Data.SqlClient;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for ActionTemplateDb.
	/// </summary>
	public class ActionTemplateDb
	{
		private static string _selectSql = @"SELECT * FROM GAActionTemplate";
		
		public ActionTemplateDb()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ActionTemplateDS GetActionTemplates()
		{
			ActionTemplateDS actionTemplateData = new ActionTemplateDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql , myConnection);
			da.Fill(actionTemplateData, "GAActionTemplate");
		
			return actionTemplateData;
		}

		public static ActionTemplateDS GetActionTemplateByRowId(int actionTemplateId)
		{
			ActionTemplateDS actionTemplateData = new ActionTemplateDS();	
			SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
			
			SqlDataAdapter da = new SqlDataAdapter(_selectSql + " WHERE ActionTemplateRowId="+actionTemplateId, myConnection);
			da.Fill(actionTemplateData, "GAActionTemplate");
		
			return actionTemplateData;
		}
	}
}

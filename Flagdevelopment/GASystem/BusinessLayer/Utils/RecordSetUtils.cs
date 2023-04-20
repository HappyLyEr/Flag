using System;
using GASystem.DataModel;
using System.Data;

namespace GASystem.BusinessLayer.Utils
{
	/// <summary>
	/// Summary description for RecordSetUtils.
	/// </summary>
	public class RecordSetUtils
	{
		public RecordSetUtils()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		/// <summary>
		/// Checks whether a record exists in the database. Please note that this method calls the database and
		/// tries to retrive the record in order to check that it exists.
		/// </summary>
		/// <param name="DataRecord">GADataRecord</param>
		/// <returns>True is record exists. False otherwise</returns></returns>
		public static bool DoesRecordExist(GADataRecord DataRecord) 
		{
			DataSet ds = RecordsetFactory.GetRecordSetByDataRecord(DataRecord);
			if (ds.Tables[DataRecord.DataClass.ToString()].Rows.Count > 0) 
				return true;
			return false;
		}
		
		/// <summary>
		/// Get name of an datarecord
		/// </summary>
		/// <param name="DataClass">Dataclass</param>
		/// <param name="RowId">RowId</param>
		/// <returns></returns>
		public static string GetDataRecordName(GADataClass DataClass, int RowId)
		{
			BusinessClass bc = RecordsetFactory.Make(DataClass);
			return bc.GetDataRecordName(RowId);
		}
		
		/// <summary>
		/// Get name of an datarecord
		/// </summary>
		/// <param name="DataClass">Dataclass name</param>
		/// <param name="RowId">RowId</param>
		/// <returns></returns>
		public static string GetDataRecordName(string DataClass, int RowId)
		{
			GADataClass dataclas = GADataRecord.ParseGADataClass(DataClass);
			return GetDataRecordName(dataclas, RowId);
		}

        // Tor 20140709 added method to check if combination ownerclass memberclass exists in GASuperclassLinks
        /// <summary>
        /// Check if memberdatarecord has specified ownerclass
        /// </summary>
        /// <param name="member">Dataclass name</param>
        /// <param name="owner">Dataclass</param>
        /// <returns></returns>
        public static bool IfMemberDataClassHasOwnerDataClass(GADataClass member, GADataClass owner)
        {
            SuperClassLinksDS ds = GASystem.DataAccess.SuperClassDb.GetSuperClassLinksByOwnerAndMember(owner, member);
            if (ds.GASuperClassLinks.Rows.Count == 1)
            {
                return true;
            }
            return false;
        }

        // Tor 20150409 added method to check if owner member combination in GASuperclassLinks is to be reported on new records in memberclass(to GARecordHistoryLog)
        /// <summary>
        /// Check if switchfree3 in owner member combination is true
        /// </summary>
        /// <param name="member">Dataclass name</param>
        /// <param name="owner">Dataclass</param>
        /// <returns></returns>
        public static bool IfReportOnOwnerMemberDataClass(GADataClass member, GADataClass owner)
        {
            SuperClassLinksDS ds = GASystem.DataAccess.SuperClassDb.GetSuperClassLinksByOwnerAndMember(owner, member);
            if (ds.GASuperClassLinks.Rows.Count == 1)
            {
                // Tor 20151214 make test more robust
                if (ds.Tables[0].Rows[0]["SwitchFree3"]!=DBNull.Value)
                    return (bool)ds.Tables[0].Rows[0]["SwitchFree3"];
            }
            return false;
        }
        /// <summary>
		/// Get name of an datarecord
		/// </summary>
		/// <param name="DataClass">Dataclass name</param>
		/// <param name="RowId">RowId</param>
		/// <returns></returns>
		public static string GetDataRecordName(string DataClass, string RowId)
		{
			try
			{
				int rowid = int.Parse(RowId);
				GADataClass dataclas = GADataRecord.ParseGADataClass(DataClass);
				return GetDataRecordName(dataclas, rowid);
			} catch (Exception ex)
			{
				//TODO log
				return string.Empty;
			}
		}
        /// <summary>
        /// Tor 20131228 Check if record parameter date is within from and to date
        /// </summary>
        /// <param name="DataRecord"></param>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static bool IsRecordCurrent(GADataRecord DataRecord, System.DateTime Date)
        {
            GASystem.AppUtils.ClassDescription cd = GASystem.AppUtils.ClassDefinition.GetClassDescriptionByGADataClass(DataRecord.DataClass);
            if (cd.hasDateFromField() && cd.hasDateToField()) 
            { 
                DataSet ds = RecordsetFactory.GetRecordSetByDataRecord(DataRecord);
                try 
                {
                    DateTime To = DateTime.MaxValue;
                    DateTime From = DateTime.MinValue;
                    if (!ds.Tables[0].Rows[0].IsNull(cd.DateFromField))
                    {
                        From = Convert.ToDateTime(ds.Tables[0].Rows[0][cd.DateFromField.ToString()]);
                    }
                    if (!ds.Tables[0].Rows[0].IsNull(cd.DateToField))
                    {
                        To = Convert.ToDateTime(ds.Tables[0].Rows[0][cd.DateToField.ToString()]);
                    }

                    if (From <= Date && To >= Date)
                    {
                        return true;
                    }
                    else 
                    {
                        return false;
                    }
                } catch (Exception ex)
                {
				    //TODO log
				    return true;
                }
            }
            return true;
        }
		
		
	}
}

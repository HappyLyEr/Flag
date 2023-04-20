using System;
using GASystem.DataModel;
using System.Web;

namespace GASystem.BusinessLayer
{
	/// <summary>
	/// Summary description for DataRecordLocator.
	/// </summary>
	public class DataRecordLocator
	{
		public DataRecordLocator()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public int LocateEmployeeByRole(int ActionRowId, String RoleName)
		{
			GADataRecord owner = DataClassRelations.GetOwner(new GADataRecord(ActionRowId, GADataClass.GAAction));
			/**
			 * printEmployers(datarecord rec)
			 *	 if (rec==employment)
			 *		print rec
			 * 
			 *   if (rec.members.count=0)
			 *		return;
			 *	 else
			 *		foreach member in rec.members
			 *			printEmployers(member);
			 * 
			 * 
			 * 
			 */
			return 0;
		}

		private void PrintEmploymentsClimb(GADataRecord record)
		{
			PrintEmployments(record);
			GADataRecord owner = DataClassRelations.GetOwner(new GADataRecord(record.RowId, GADataClass.GAAction));
			if (null!=owner)
				PrintEmploymentsClimb(owner);
			else
				return;
		}

		private void PrintEmployments(GADataRecord record)
		{
			if (record.DataClass==GADataClass.GAEmployment)
			{
				EmploymentDS employment = Employment.GetEmploymentByEmploymentRowId(record.RowId);
				HttpContext.Current.Trace.Warn("Found emplyment: "+record.RowId);
			}
			SuperClassDS members = DataClassRelations.GetMembers(record);
			if (members.GASuperClass.Rows.Count!=0) 
			{
				foreach (SuperClassDS.GASuperClassRow row in members.GASuperClass.Rows)
				{
					PrintEmployments(new GADataRecord(row.MemberClassRowId, GADataRecord.ParseGADataClass(row.MemberClass)));
				}
			}
			else
				return;
			
		}
	}
}

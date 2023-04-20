using System;
using GASystem;
using GASystem.DataModel;

namespace GASystem.DataAccess.Utils.SQLView
{
	/// <summary>
	/// Summary description for CrewListSQLView.
	/// </summary>
	public class CrewListSQLView : ISQLView
	{
		private const string sqlbase = @"SELECT  GALocation.Name AS Location, GALocation.CallSign, GALocation.LocationIdNumber, 
CountryOfRegistrationC.GAListDescription as CountryOfRegistration,
GAPersonnel.FamilyName, GAPersonnel.GivenName, GAPersonnel.Address, myHomePhone.ContactDeviceAddress as PersonnelHomePhone, SexC.GAListDescription AS Sex, GAPersonnel.DateOfBirth, MaritalStatusC.GAListDescription AS MaritalStatus, NationalityC.GAListDescription AS Nationality,
GATimeAndAttendance.DateTimeFrom AS Board, GATimeAndAttendance.DateTimeTo AS Leave, GATimeAndAttendance.PlaceOfEmbarkment
, TypeOfIdC.GAListDescription AS TypeOfId, GAPersonnel.TextFree2 AS IdentificationIDNumber,
RoleC.GAListDescription AS Role, GAEmployment.ContractNumber, DepartmentC.GAListDescription AS Department, EmploymentCategoryC.GAListDescription AS EmploymentCategory, GAEmployment.FromDate

, GANextOfKin.Name as NextOfKin , GANextOfKin.Address as NextOfKinAddress, GANextOfKin.PhoneNumber as NextOfKinPhoneNumber 
, NextOfKinCountry.GAListDescription as NextOfKinCountry , NextOfKinPriority.GAListDescription as NextOfKinPriority , NextOfKinPriority.Sort1 as NextOfKinPrioritySort , NextOfKinRelationship.GAListDescription as NextOfKinRelationship
, RoleC.Group5 as EmploymentGroup, RoleC.Group5Sort1 as EmploymentLevel, RoleC.Group4 as ReportsTo

FROM        GATimeAndAttendance 
INNER JOIN  GASuperClass ON GATimeAndAttendance.TimeAndAttendanceRowId = GASuperClass.MemberClassRowId 
INNER JOIN  GALocation ON GASuperClass.OwnerClassRowId = GALocation.LocationRowId 
INNER JOIN  GAPersonnel ON GATimeAndAttendance.EmploymentRowId = GAPersonnel.PersonnelRowId
LEFT JOIN  (select personnel, max(ContractNumber) as ContractNumber, max(RoleListsRowId) as RoleListsRowId, max(DepartmentListsRowId) as DepartmentListsRowId, max(EmploymentCategoryListsRowId) as EmploymentCategoryListsRowId, max(FromDate) as FromDate
from GAEmployment inner join GASuperClass on GAEmployment.EmploymentRowId = GASuperClass.MemberClassRowId 
where GASuperClass.MemberClass = N'GAEmployment' and (GASuperClass.OwnerClass = N'GALocation') 
AND 
      (GASuperClass.ReadRoles = '{0}-{1}'
      OR GASuperClass.UpdateRoles = '{0}-{1}'
      OR GASuperClass.CreateRoles = '{0}-{1}'
      OR GASuperClass.DeleteRoles = '{0}-{1}'
      OR GASuperClass.TextFree1 = '{0}-{1}'
      OR GASuperClass.TextFree2 = '{0}-{1}')
and {2} group by personnel) GAEmployment 
on GAEmployment.personnel = GAPersonnel.PersonnelRowId
LEFT JOIN GALists SexC on GAPersonnel.SexListsRowId = SexC.ListsRowId
LEFT JOIN GALists MaritalStatusC on GAPersonnel.MaritalStatusListsRowId = MaritalStatusC.ListsRowId
LEFT JOIN GALists NationalityC on GAPersonnel.NationalityListsRowId = NationalityC.ListsRowId

LEFT JOIN GALists TypeOfIdC on GAPersonnel.IntFree2 = TypeOfIdC.ListsRowId
LEFT OUTER JOIN 
(SELECT GAMeansOfContact.ContactDeviceAddress, GASuperClass.OwnerClassRowId FROM GAMeansOfContact 
	INNER JOIN GALists ON GAMeansOfContact.ContactDeviceTypeListsRowId = GALists.ListsRowId AND GALists.GAListValue = N'Home phone' 
	INNER JOIN GASuperClass ON GAMeansOfContact.MeansOfContactRowId = GASuperClass.MemberClassRowId AND GASuperClass.MemberClass = N'GAMeansOfContact' AND GASuperClass.OwnerClass = N'GAPersonnel'
) myHomePhone on GAPersonnel.PersonnelRowId = myHomePhone.OwnerClassRowId

LEFT JOIN GALists RoleC on GAEmployment.RoleListsRowId = RoleC.ListsRowId
LEFT JOIN GALists DepartmentC on GAEmployment.DepartmentListsRowId = DepartmentC.ListsRowId
LEFT JOIN GALists EmploymentCategoryC on GAEmployment.EmploymentCategoryListsRowId = EmploymentCategoryC.ListsRowId
LEFT JOIN GALists CountryOfRegistrationC on GALocation.CountryOfRegistrationListsRowId = CountryOfRegistrationC.ListsRowId

LEFT JOIN GASuperClass SCNOK ON GAPersonnel.PersonnelRowId = SCNOK.OwnerClassRowId AND SCNOK.OwnerClass = N'GAPersonnel' AND SCNOK.MemberClass = N'GANextOfKin' 
LEFT OUTER JOIN GANextOfKin ON SCNOK.MemberClassRowId = GANextOfKin.NextOfKinRowId 
LEFT OUTER JOIN GALists NextOfKinPriority ON GANextOfKin.Priority = NextOfKinPriority.ListsRowId

LEFT OUTER JOIN GALists NextOfKinRelationship ON GANextOfKin.TypeOfKinListsRowId = NextOfKinRelationship.ListsRowId
LEFT OUTER JOIN GALists NextOfKinCountry ON GANextOfKin.CountryCodeListsRowId = NextOfKinCountry.ListsRowId

WHERE    (GASuperClass.MemberClass = N'GATimeAndAttendance') 
AND (GASuperClass.OwnerClass = N'GALocation') 
AND 
      (GASuperClass.ReadRoles = '{0}-{1}'
      OR GASuperClass.UpdateRoles = '{0}-{1}'
      OR GASuperClass.CreateRoles = '{0}-{1}'
      OR GASuperClass.DeleteRoles = '{0}-{1}'
      OR GASuperClass.TextFree1 = '{0}-{1}'
      OR GASuperClass.TextFree2 = '{0}-{1}')
and {3} order by 4,5";
        //(GASuperClass.path like '%{0}-{1}/%') has been replaced twice in the statement above								
        //ORDER BY GAPersonnel.FamilyName, GAPersonnel.GivenName";

		public CrewListSQLView(AppUtils.ClassDescription cd, GADataRecord Owner) : base(cd, Owner) {}

		public override string GetSQLViewQuery()
		{
			string sqlfilter = generateDateSpanFilter("GATimeAndAttendance.DateTimeFrom", "GATimeAndAttendance.DateTimeTo");
			
			string sql=  string.Format(sqlbase, Owner.DataClass.ToString(), Owner.RowId.ToString(), generateDateSpanFilter("GAEmployment.FromDate", "GAEmployment.ToDate"), sqlfilter);
			//if (sqlfilter != string.Empty)
			//	sql += " and " + sqlfilter;

			return sql;
		}

	


	}
}

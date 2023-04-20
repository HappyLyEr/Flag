using System;
using GASystem;
using GASystem.DataModel;

namespace GASystem.DataAccess.Utils.SQLView
{
	/// <summary>
	/// Summary description for LastUserLoginSQLView.
	/// </summary>
	public class LastUserLoginSQLView : ISQLView
	{
        private const string sqlbase = @"SELECT  GAPersonnel.GivenName,  GAPersonnel.FamilyName, GAUser.DNNUserId as LoginID, GAEmployment.EmploymentRowId, max({2}.dbo.SiteLog.DateTime) LastLoginDate
										FROM   GALocation 
										INNER JOIN GASuperClass ON GALocation.LocationRowId = GASuperClass.OwnerClassRowId 
										INNER JOIN GAEmployment ON GASuperClass.MemberClassRowId = GAEmployment.EmploymentRowId 
										INNER JOIN GAUser ON GAEmployment.Personnel = GAUser.PersonnelRowId 
										INNER JOIN GAPersonnel ON GAUser.PersonnelRowId = GAPersonnel.PersonnelRowId 

										INNER JOIN		{2}.dbo.Users ON GAUser.DNNUserId = {2}.dbo.Users.Username COLLATE SQL_Latin1_General_CP1_CI_AS 
										LEFT OUTER JOIN {2}.dbo.SiteLog ON {2}.dbo.Users.UserID = {2}.dbo.SiteLog.UserId
										WHERE     (
                                                        GASuperClass.ReadRoles = '{0}-{1}'
                                                      OR GASuperClass.UpdateRoles = '{0}-{1}'
                                                      OR GASuperClass.CreateRoles = '{0}-{1}'
                                                      OR GASuperClass.DeleteRoles = '{0}-{1}'
                                                      OR GASuperClass.TextFree1 = '{0}-{1}'
                                                      OR GASuperClass.TextFree2 = '{0}-{1}'
                                                  )
												AND (GASuperClass.OwnerClass = N'GALocation') 
												AND (GASuperClass.MemberClass = N'GAEmployment')
										group by GAPersonnel.FamilyName, GAPersonnel.GivenName, GAUser.DNNUserId, GAEmployment.EmploymentRowId";
        //(GASuperClass.path like '%{0}-{1}/%') has been replaced in the statement above



		public LastUserLoginSQLView(AppUtils.ClassDescription cd, GADataRecord Owner) : base (cd, Owner)
		{
		}

		public override string GetSQLViewQuery()
		{
			return string.Format(sqlbase, Owner.DataClass.ToString(), Owner.RowId.ToString(), Utils.DatabaseSettings.DNNDatabaseName); 
		}

	}
}

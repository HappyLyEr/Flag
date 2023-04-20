using System;
using System.Collections.Generic;
using System.Text;
using GASystem.DataModel;
using System.Data.SqlClient;
using GASystem.DataAccess;
using System.Collections;
using GASystem.BusinessLayer;
using System.Data;

namespace GASystem.DotNetApre
{
    public class DefaultNotifier
    {
        private ClassDS _classDataSet;
        private WorkflowStarterDS.GAWorkflowStarterRow _wfRow;
        private string _class;

        public DefaultNotifier(WorkflowStarterDS.GAWorkflowStarterRow row, ClassDS cds)
        {
            _wfRow = row;
            _classDataSet = cds;
            _class = cds.GAClass[0].Class;
            _emailSQLKey = "EmailSQL_" + _class + row.ClassDateFieldId;
            _emailTemplateKey = "EmailTemplate_" + _class + row.ClassDateFieldId;
        }

        private string _emailSQLKey;

        public string EmailSQLKey
        {
            get { return _emailSQLKey; }
            set { _emailSQLKey = value; }
        }

        private string _emailTemplateKey;

        public string EmailTemplateKey
        {
            get { return _emailTemplateKey; }
            set { _emailTemplateKey = value; }
        }

        public void Notify()
        {
            string SMTPFromAddress = new GASystem.AppUtils.FlagSysResource().GetResourceString("SMTPFromAddress");
            string sql = new GASystem.AppUtils.FlagSysResource().GetResourceString(this.EmailSQLKey);
            SqlConnection myConnection = new SqlConnection(DataUtils.getConnectionString());
            SqlCommand myCommand = new SqlCommand(sql, myConnection);
            SqlDataReader result;
            try
            {
                myConnection.Open();
                result = myCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception e)
            {
                myConnection.Close();
                throw e;
            }

            if (!result.HasRows)
            {
                Console.WriteLine("No records returned from select statement");
                myConnection.Close();
                return;
            }

            while (result.Read())
            {
                int personnelRowId = result.GetInt32(0);
                string eMailTo = result.GetString(1);
                string eMailCC = result.GetString(2);
                
                GADataTransaction tran = GADataTransaction.StartGADataTransaction();
                try
                {
                    // 邮件正文
                    string body = new GASystem.AppUtils.FlagSysResource().GetResourceString(this.EmailTemplateKey);
                    string myPersonnelFullName = GetPersonFullName(personnelRowId);
                    body = string.Format(body, myPersonnelFullName);

                    // 收件人
                    ArrayList toList = new ArrayList();
                    AddElementToArrayIfNotAlreadyThere(ref toList, eMailTo);
                    // 抄送
                    ArrayList ccList = new ArrayList();
                    string[] ccArray = eMailCC.Split('|');
                    foreach (string mail in ccArray)
                    {
                        AddElementToArrayIfNotAlreadyThere(ref ccList, mail);
                    }
                    // 邮件主题
                    string subject = _wfRow.WorkflowDefaultSubject;
                    // 发送邮件
                    FlagSMTP.SendEmailMsg(toList, ccList, null, subject, body, true);
                    Console.WriteLine("Email sent to: " + eMailTo);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
                finally
                {
                    tran.Connection.Close();
                }
            }
            myConnection.Close();
        }

        private static void AddElementToArrayIfNotAlreadyThere(ref ArrayList list, string element)
        {
            if (element != string.Empty)
            {
                // check if email address already in list before adding
                bool found = false;
                foreach (string a in list)
                {
                    if (a == element.Trim()) found = true;
                }
                if (!found) list.Add(element.Trim());
            }
            return;
        }

        private string GetPersonFullName(int PersonnelRowId)
        {
            PersonnelDS ds = GASystem.BusinessLayer.Personnel.GetPersonnelByPersonnelRowId(PersonnelRowId);
            return (ds.Tables[0].Rows.Count > 0) ? ds.GAPersonnel[0].GivenName + " " + ds.GAPersonnel[0].FamilyName : string.Empty;
        }
    }
}

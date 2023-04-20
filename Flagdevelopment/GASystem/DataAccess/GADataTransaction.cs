using System;
using System.Data.SqlClient;
using System.Data;

namespace GASystem.DataAccess
{
	/// <summary>
	/// Summary description for GADataTransaction.
	/// </summary>
	public class GADataTransaction  
	{
		private IDbConnection _connection;
		private IDbTransaction _transaction;
		
		public GADataTransaction(IDbConnection Connection, IDbTransaction Transaction)
		{
			this.Connection = Connection;
			this.Transaction = Transaction;
		}
	

		public static GADataTransaction StartGADataTransaction()
		{
			SqlConnection c = new SqlConnection(DataUtils.getConnectionString());
			c.Open();
			SqlTransaction t = c.BeginTransaction();
			return new GADataTransaction(c, t);
		}

		public void CommitAndClose()
		{
			Commit();
            Connection.Close();
		}


		public void Commit()
		{
			Transaction.Commit();		
		}

		public void RollbackAndClose()
		{
			Rollback();
            Connection.Close();
		}


		public void Rollback()
		{
			Transaction.Rollback();
		}

		public IDbConnection Connection
		{
			get
			{
				return _connection;
			}
			set
			{
				_connection = value;
			}
		}

		public IDbTransaction Transaction
		{
			get
			{
				return _transaction;
			}
			set
			{
				_transaction = value;
			}
		}


	}
}

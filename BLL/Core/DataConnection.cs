using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Util;

namespace BLL.Core
{
	public class DataConnection : IDisposable
	{

		/// <summary>
		/// 数据库连接类
		/// </summary>
		public DataConnection()
			: this(DBType.Bill)
		{
		}

		/// <summary>
		/// 数据库连接类
		/// </summary>
		/// <param name="dbtype">数据库类型</param>
		public DataConnection(DBType dbtype = DBType.Bill)
		{
			SetConnection(dbtype);
		}

		/// <summary>
		/// 设置Connection
		/// </summary>
		/// <param name="dbtype"></param>
		private void SetConnection(DBType dbtype)
		{
			switch (dbtype)
			{
				case DBType.Bill:
					this._Connection = new SqlConnection(Connection_Bill_DB.ConnectionString_Bill_DB);
					break;
			}
			this._Connection.Open();
		}

		#region Properties

		private SqlConnection _Connection;
		/// <summary>
		/// 数据库连接
		/// </summary>
		public SqlConnection Connection
		{
			get
			{
				return this._Connection;
			}
			set
			{
				this._Connection = value;
			}
		}


		private SqlTransaction _Transaction;
		/// <summary>
		/// Sql事务
		/// </summary>
		public SqlTransaction Transaction
		{
			get
			{
				return this._Transaction;
			}
		}

		#endregion

		#region Methods

		private int _iTran = 0;
		/// <summary>
		/// 事务开始
		/// </summary>
		public void BeginTransaction()
		{
			this._iTran = 1;
			this._Transaction = this._Connection.BeginTransaction();
		}

		/// <summary>
		/// 事务提交
		/// </summary>
		/// <returns></returns>
		public void Commit()
		{
			this._iTran = 2;
			this._Transaction.Commit();
		}

		/// <summary>
		/// 事务回滚
		/// </summary>
		public void Rollback()
		{
			this._iTran = 3;
			this._Transaction.Rollback();
		}

		/// <summary>
		/// 事务回滚
		/// </summary>
		/// <param name="transactionName">事务名</param>
		public void Rollback(string transactionName)
		{
			this._Transaction.Rollback(transactionName);
		}

		/// <summary>
		/// 事务保存
		/// </summary>
		/// <param name="savePointName">保存点名</param>
		public void Save(string savePointName)
		{
			this._Transaction.Save(savePointName);
		}
		#endregion

		#region Dispose
		/// <summary>
		/// 释放资源
		/// </summary>
		public void Dispose()
		{
			if (_iTran == 0 || _iTran > 1)
			{
				if (this._Transaction != null)
				{
					this._Transaction.Dispose();
				}
				if (this._Connection != null)
				{
					this._Connection.Close();
					this._Connection.Dispose();
				}
			}
		}

		#endregion

		/// <summary>
		/// 账单数据库的连接
		/// </summary>
		internal class Connection_Bill_DB
		{
			public static string ConnectionString_Bill_DB
			{
				get
				{
					if (HttpRuntime.Cache[ConstClass.APPLICATION_BILL_DB] == null)
					{
						string connectionString = ConfigurationManager.ConnectionStrings[ConstClass.APPLICATION_BILL_DB].ToString();
						string ConStringEncrypt = ConfigurationManager.AppSettings[ConstClass.APPLICATION_CON_ENCRYPT];
						if (ConStringEncrypt == "true")
						{
							connectionString = Util.Encryptions.DESEncrypt(connectionString);
						}
						HttpRuntime.Cache.Insert(ConstClass.APPLICATION_BILL_DB, connectionString);
					}
					return HttpRuntime.Cache[ConstClass.APPLICATION_BILL_DB].ToString();
				}
			}
		}
	}
}

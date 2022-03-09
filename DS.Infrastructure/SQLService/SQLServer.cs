using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using DS.Infrastructure.Context;

namespace DS.Infrastructure.SQLService
{
	public class SQLServer : ISQL
	{
		private readonly SQLContext _databaseContext;

		public SQLServer(SQLContext databaseContext)
		{
			_databaseContext = databaseContext;
		}

		private void SetParameters(DLParam[] parameters, SqlCommand cmd)
		{
			if (parameters == null)
				return;
			for (int i = 0; i < parameters.Length; i++)
			{
				DLParam p = parameters[i];
				SqlParameter np = new SqlParameter();
				np.ParameterName = "@" + p.Name;
				if (p.Value == null)
					np.Value = DBNull.Value;
				else
					np.Value = p.Value;
				//np.Direction = p.Direction;
				np.Direction = p.Out ? System.Data.ParameterDirection.InputOutput : System.Data.ParameterDirection.Input;

				if (p.T == DBType.String && p.Value is string && p.Value != null && (p.Value as string).Length > p.Length)
				{
					ApplicationException ae = new ApplicationException(string.Format("Could not save parameter.  Max Length not long enough.  {0}", np.ParameterName));
					throw ae;
				}

				switch (p.T)
				{
					case DBType.DBNullUnknown:
						break;
					case DBType.DateTime:
						np.SqlDbType = SqlDbType.DateTime;
						//np.DbType = DbType.DateTime;
						break;
					case DBType.Float:
						np.SqlDbType = SqlDbType.Float;
						break;
					case DBType.BigInt:
						np.SqlDbType = SqlDbType.BigInt;
						break;
					case DBType.Integer:
						np.SqlDbType = SqlDbType.Int;
						break;
					case DBType.String:
						np.SqlDbType = SqlDbType.VarChar;
						if (np.Value != null && np.Value != DBNull.Value && np.Value.GetType() == typeof(string) && ((string)np.Value).Length > 8000)
							np.SqlDbType = SqlDbType.Text;
						break;
					case DBType.Time:
						np.SqlDbType = SqlDbType.Time;
						break;
					case DBType.BLOBImage:
						np.SqlDbType = SqlDbType.Image;
						np.Size = p.Length;
						break;
					case DBType.BLOBText:
						np.SqlDbType = SqlDbType.Text;
						break;
					case DBType.BLOBNText:
						np.SqlDbType = SqlDbType.NText;
						break;
					case DBType.Object:
						np.SqlDbType = SqlDbType.Image;
						break;
					case DBType.Binary:
						np.SqlDbType = SqlDbType.Binary;
						np.Size = p.Length;
						break;
					case DBType.Bit:
						np.SqlDbType = SqlDbType.Bit;
						break;
					case DBType.Guid:
						np.SqlDbType = SqlDbType.UniqueIdentifier;
						break;
					case DBType.Money:
						np.SqlDbType = SqlDbType.Money;
						break;
					case DBType.XML:
						np.SqlDbType = SqlDbType.Xml;
						break;
					case DBType.Structured:
						np.SqlDbType = SqlDbType.Structured;
						np.TypeName = p.TypeName;
						break;
					default:
						throw new NotSupportedException(string.Format("DB Type not supported:  {0}", p.T.ToString()));
				}
				cmd.Parameters.Add(np);
			}
		}

		private void SetTimeout(SqlCommand cmd)
		{
			if (cmd.CommandType == CommandType.StoredProcedure)
			{
				throw new NotImplementedException();
			}
		}

		public DataTable ExecDataTable(string text, bool isSproc, DLParam[] prams)
		{
			DataSet ds = ExecDataSet(text, isSproc, prams, 0);
			if (ds != null && ds.Tables.Count > 0)
				return ds.Tables[0];
			return null;
		}
		public DataSet ExecDataSet(string text, bool isSproc, DLParam[] prams)
		{
			return ExecDataSet(text, isSproc, prams, 0);
		}

		public DataSet ExecDataSet(string text, bool isSproc, DLParam[] prams, int secondsTimeout)
		{
			if (isSproc)
			{
				int secTimeoutSprocConfig = 5;  //Config.CommandTimeout(text);
				if (secTimeoutSprocConfig > 0)
					secondsTimeout = secTimeoutSprocConfig;
			}

			DateTime start = DateTime.Now;
			int durationMS = 0;

			Exception excOriginal = null;
			try
			{
				int max = TotalRuns((isSproc ? (text) : string.Empty));
				for (int i = 0; i < max; i++)
				{
					try
					{
						DataSet dsRet = new DataSet("Set");
						using (var conn = _databaseContext.CreateConnection())
						{
							//System.Diagnostics.Debug.WriteLine(connS);
							using (SqlCommand cmd = new SqlCommand(text, conn))
							{
								cmd.CommandType = isSproc ? CommandType.StoredProcedure : CommandType.Text;
								SetParameters(prams, cmd);
								if (secondsTimeout != 0)
									SetTimeout(cmd);
								else
								{
									cmd.CommandTimeout = secondsTimeout;
								}
								conn.Open();
								using (SqlDataAdapter da = new SqlDataAdapter(cmd))
								{
									da.SelectCommand.CommandType = isSproc ? CommandType.StoredProcedure : CommandType.Text;
									da.Fill(dsRet);
								}
							}
						}

						durationMS = Convert.ToInt32(DateTime.Now.Subtract(start).TotalMilliseconds);
						//LogUtil.SQLLog(connS, text, durationMS, null, null, DLPramToTuple(prams));

						return dsRet;
					}
					catch (Exception excSql)
					{
						durationMS = Convert.ToInt32(DateTime.Now.Subtract(start).TotalMilliseconds);
						//LogUtil.SQLLog(connS, text, durationMS, excSql.Message, excSql.GetType().Name, DLPramToTuple(prams));
						System.Diagnostics.Debug.WriteLine(string.Format("Sproc:  {0}.  Type:  {1}.  Message:  {2}", text, excSql.GetType().Name, excSql.Message));
						if (excOriginal == null)
							excOriginal = excSql;

						bool runAgain = RunAgain(excSql);
						if (!runAgain)
						{
							throw;
						}
						else
						{
							if (SQLRetriesDelayMS > 0 && (i + 1) < max)
							{
								System.Diagnostics.Debug.WriteLine(string.Format("Start Wait:  {0}.  Loop:  {1}", SQLRetriesDelayMS, i));
								System.Threading.Thread.Sleep(0);
								System.Threading.Thread.Sleep(1);
								System.Threading.Thread.Sleep(SQLRetriesDelayMS);
							}

						}
					}
				}
			}
			catch (Exception exc)
			{
				durationMS = Convert.ToInt32(DateTime.Now.Subtract(start).TotalMilliseconds);
				//LogUtil.SQLLog(connS, text, durationMS, exc.Message, exc.GetType().Name, DLPramToTuple(prams));
				System.Diagnostics.Debug.WriteLine(string.Format("Sproc DataSet:  {0}.  {1}.  {2}", text, exc.GetType().Name, exc.Message));
				throw;
			}
			if (excOriginal != null)
				throw excOriginal;

			throw new DataException(string.Format("Internal Sql Issue:  {0}", text));
		}


		public object ExecScalar(string text, bool isSproc, DLParam[] prams)
		{
			int durationMS = 0;
			Exception excOriginal = null;
			DateTime start = DateTime.Now;
			try
			{
				int max = TotalRuns((isSproc ? (text) : string.Empty));
				for (int i = 0; i < max; i++)
				{
					try
					{
						start = DateTime.Now;
						using (var conn = _databaseContext.CreateConnection())
						{
							using (SqlCommand cmd = new SqlCommand(text, conn))
							{
								cmd.CommandType = isSproc ? CommandType.StoredProcedure : CommandType.Text;
								SetParameters(prams, cmd);
								SetTimeout(cmd);
								conn.Open();
								object o = cmd.ExecuteScalar(); // num rows

								//Log this
								durationMS = Convert.ToInt32(DateTime.Now.Subtract(start).TotalMilliseconds);
								//LogUtil.SQLLog(connS, text, durationMS, null, null, DLPramToTuple(prams));

								return o;
							}
						}

					}
					catch (Exception excSql)
					{
						durationMS = Convert.ToInt32(DateTime.Now.Subtract(start).TotalMilliseconds);
						//LogUtil.SQLLog(connS, text, durationMS, excSql.Message, excSql.GetType().Name, DLPramToTuple(prams));

						System.Diagnostics.Debug.WriteLine(string.Format("Sproc:  {0}.  Type:  {1}.  Message:  {2}", text, excSql.GetType().Name, excSql.Message));
						if (excOriginal == null)
							excOriginal = excSql;

						start = DateTime.Now;
						bool runAgain = RunAgain(excSql);
						if (!runAgain)
						{
							throw;
						}
						else
						{
							if (SQLRetriesDelayMS > 0 && (i + 1) < max)
							{
								System.Diagnostics.Debug.WriteLine(string.Format("Start Wait:  {0}.  Loop:  {1}", SQLRetriesDelayMS, i));
								System.Threading.Thread.Sleep(0);
								System.Threading.Thread.Sleep(1);
								System.Threading.Thread.Sleep(SQLRetriesDelayMS);
							}

						}
					}
				}
			}
			catch (Exception exc)
			{
				durationMS = Convert.ToInt32(DateTime.Now.Subtract(start).TotalMilliseconds);
				//LogUtil.SQLLog(connS, text, durationMS, exc.Message, exc.GetType().Name, DLPramToTuple(prams));
				System.Diagnostics.Debug.WriteLine(string.Format("Sproc Scalar:  {0}.  {1}.  {2}", text, exc.GetType().Name, exc.Message));
				throw;
			}
			if (excOriginal != null)
				throw excOriginal;

			throw new DataException(string.Format("Internal Sql Issue:  {0}", text));
		}
		private int SQLRetriesDelayMS
		{
			get => 2000;
		}

		public int TotalRuns(string sprocName)
		{
			return 1;

			/*
			int ExtraRetry = Config.ExtraCommandRetry(sprocName);
			int i = 1;
			if (SQLRetries)
			{
				i += SQLRetriesReEnterCount;
				i += ExtraRetry;
			}
			return i;
			*/
		}
		public bool RunAgain(Exception exc)
		{
			return false;
		}
	}
}

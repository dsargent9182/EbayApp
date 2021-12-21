using System.Data;
using Microsoft.Data.SqlClient;

namespace Ebay.Context.Dapper
{
	public class DapperContext
	{
		private readonly string _connectionString;
		public DapperContext(string connectionString)
		{
			_connectionString = connectionString;
		}

		public IDbConnection CreateConnection() 
		{
			using var conn = new SqlConnection(_connectionString);
			
			return conn;
		} 
	}
}

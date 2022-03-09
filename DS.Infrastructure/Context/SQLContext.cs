using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Infrastructure.Context
{
	public class SQLContext
	{
		private readonly string _connectionString;
		public SQLContext(string connectionString)
		{
			_connectionString = connectionString;
		}

		public SqlConnection CreateConnection()
		{
			return new SqlConnection(_connectionString);
		}
	}
}

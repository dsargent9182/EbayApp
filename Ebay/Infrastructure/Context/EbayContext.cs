using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Ebay.MicroService.Infrastructure.Context
{
	public class EbayContext : IDatabaseContext
	{
		private readonly string _connectionString;
		public EbayContext(string connectionString)
		{
			_connectionString = connectionString;
		}

		public IDbConnection CreateConnection()
		{
			return new SqlConnection(_connectionString);
		}
	}
}

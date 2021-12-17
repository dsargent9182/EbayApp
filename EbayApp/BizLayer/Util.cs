using Microsoft.Data.SqlClient;
using System.Data;

namespace EbayApp.BizLayer
{
	public class Util
	{
		public static DataSet GetDataSet(string connString,bool isSproc, string sprocName)
		{
			DataSet ds = null;
			using(SqlConnection con = new SqlConnection())
			{
				
			}




			return ds;
		}


	}
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Ebay.MicroService.Infrastructure.Context
{
	public interface IDatabaseContext
	{
		public IDbConnection CreateConnection();
	}
}

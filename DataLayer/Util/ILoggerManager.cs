using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ebay.Util
{
	public interface ILoggerManager
	{
		void LogInfo(string message);

		void LogError(string message);

		void LogError(string message,Exception ex);
	}
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Infrastructure.SQLService
{
	public interface ISQL
	{
		DataSet ExecDataSet(string text, bool isSproc, DLParam[] prams);
		DataSet ExecDataSet(string text, bool isSproc, DLParam[] prams, int secondsTimeout);
		DataTable ExecDataTable(string text, bool isSproc, DLParam[] prams);
		object ExecScalar(string text, bool isSproc, DLParam[] prams);
		bool RunAgain(Exception exc);
		int TotalRuns(string sprocName);
	}
}

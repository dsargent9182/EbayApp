using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Infrastructure.SQLService
{
	public enum DBType
	{
		BLOBText,
		BLOBNText,
		BLOBImage,
		String,
		Integer,
		Float,
		DateTime,
		Time,
		Object,
		BigInt,
		Bit,
		Guid,
		Money,
		Binary,
		XML,
		DBNullUnknown,
		Structured,
		Unknown
	}
	public class DLParam
	{
		public DBType T;
		/// <summary>Name of variable.  Do not include ":" or "@" sign in front.  That is added on the fly.</summary>
		public string Name;
		public object Value;
		public bool Out;
		public int Length;
		public string TypeName;

		public DLParam(DBType tt, string aname, object avalue, int length, bool aout)
		{
			T = tt;
			Name = aname;
			Value = avalue;
			Out = aout;
			Length = length;
		}
		public DLParam(string aname, DataTable avalue, string aTypeName)
		{
			T = DBType.Structured;
			Name = aname;
			Value = avalue;
			Out = false;
			Length = avalue.Rows.Count;
			TypeName = aTypeName;
		}
		public DLParam(string aname, object avalue)
		{
			Length = 4;
			Name = aname;
			Value = avalue;
			Out = false;
			
			if (Value == null)
			{
				T = DBType.String;
			}
			else if (Value == DBNull.Value)
			{
				T = DBType.DBNullUnknown;
			}
			else
			{
				Type t = Value.GetType();
				if (t == typeof(string))
				{
					T = DBType.String;
					Length = ((string)avalue).Length;
				}
				else if (t == typeof(long))
					T = DBType.BigInt;
				else if (t == typeof(long?))
					T = DBType.BigInt;
				else if (t == typeof(ulong))
					T = DBType.BigInt;
				else if (t == typeof(ulong?))
					T = DBType.BigInt;

				else if (t == typeof(int))
					T = DBType.Integer;
				else if (t == typeof(int?))
					T = DBType.Integer;

				else if (t == typeof(float))
					T = DBType.Float;
				else if (t == typeof(float?))
					T = DBType.Float;
				else if (t == typeof(decimal))
					T = DBType.Float;
				else if (t == typeof(decimal?))
					T = DBType.Float;
				else if (t == typeof(DateTime))
				{
					T = DBType.DateTime;
					if (((DateTime)avalue) == DateTime.MinValue)
						Value = DBNull.Value;
				}
				else if (t == typeof(DateTime?))
				{
					T = DBType.DateTime;
					DateTime? q = avalue as DateTime?;
					if (q.HasValue && q.Value == DateTime.MinValue)
						Value = DBNull.Value;
				}
				else if (t == typeof(TimeSpan))
					T = DBType.Time;
				else if (t == typeof(byte[]))
				{
					Length = ((byte[])Value).Length;
					if (Length > 512)
						T = DBType.BLOBImage;
					else
						T = DBType.Binary;
				}
				else if (t == typeof(char))
					T = DBType.String;
				else if (t == typeof(bool))
					T = DBType.Bit;
				else if (t == typeof(bool?))
					T = DBType.Bit;
				else if (t == typeof(Guid))
					T = DBType.Guid;
				else if (t == typeof(Guid?))
					T = DBType.Guid;
				else
					T = DBType.Object;
			}
		}
	}
}

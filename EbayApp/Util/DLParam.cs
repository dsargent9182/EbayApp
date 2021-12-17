using System.Data;

namespace EbayApp.Util
{
	public class DLParam
	{
		public DBType T;
		/// <summary>Name of variable.  Do not include ":" or "@" sign in front.  That is added on the fly.</summary>
		public string Name;
		public object Value;
		public bool Out;
		public int Length;
		public string TypeName;
		//public System.Data.ParameterDirection Direction;

		public DLParam(DBType tt, string aname, object avalue, int length, bool aout)
		{
			T = tt;
			Name = aname;
			Value = avalue;
			Out = aout;
			Length = length;
			//Direction = System.Data.ParameterDirection.Input;
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
			//Direction = System.Data.ParameterDirection.Input;


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

	public class SqlUtil
	{
		public static DBType StringToDBType(string s)
		{
			DBType t = DBType.Object;
			if (0 == string.Compare(s, "nvarchar", true))
				t = DBType.String;
			else if (0 == string.Compare(s, "nvarchar", true))
				t = DBType.String;
			else if (0 == string.Compare(s, "varchar", true))
				t = DBType.String;
			else if (0 == string.Compare(s, "bigint", true))
				t = DBType.BigInt;
			else if (0 == string.Compare(s, "int", true))
				t = DBType.Integer;
			else if (0 == string.Compare(s, "smallint", true))
				t = DBType.Integer;
			else if (0 == string.Compare(s, "tinyint", true))
				t = DBType.Integer;
			else if (0 == string.Compare(s, "bit", true))
				t = DBType.Bit;
			else if (0 == string.Compare(s, "decimal", true))
				t = DBType.Float;
			else if (0 == string.Compare(s, "numeric", true))
				t = DBType.Integer;
			else if (0 == string.Compare(s, "money", true))
				t = DBType.Money;
			else if (0 == string.Compare(s, "smallmoney", true))
				t = DBType.Money;
			else if (0 == string.Compare(s, "float", true))
				t = DBType.Float;
			else if (0 == string.Compare(s, "real", true))
				t = DBType.Float;
			else if (0 == string.Compare(s, "datetime", true))
				t = DBType.DateTime;
			else if (0 == string.Compare(s, "smalldatetime", true))
				t = DBType.DateTime;
			else if (0 == string.Compare(s, "char", true))
				t = DBType.String;
			else if (0 == string.Compare(s, "nchar", true))
				t = DBType.String;
			else if (0 == string.Compare(s, "text", true))
				t = DBType.String;
			else if (0 == string.Compare(s, "ntext", true))
				t = DBType.String;
			else if (0 == string.Compare(s, "binary", true))
				t = DBType.Binary;
			else if (0 == string.Compare(s, "varbinary", true))
				t = DBType.Binary;
			else if (0 == string.Compare(s, "image", true))
				t = DBType.BLOBImage;
			else if (0 == string.Compare(s, "xml", true))
				t = DBType.XML;
			else if (0 == string.Compare(s, "uniqueidentifier", true))
				t = DBType.Guid;
			else if (0 == string.Compare(s, "timestamp", true))
				t = DBType.DateTime;

			return t;
		}
	}
}

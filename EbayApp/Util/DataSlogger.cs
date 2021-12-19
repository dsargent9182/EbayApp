//using System.Data;

//namespace EbayApp.Util
//{
//	public class DataSlogger
//	{
//		IDataRecord pRec = null;
//		DataRow pDR = null;

//		public DataSlogger(IDataRecord p)
//		{
//			pRec = p;
//		}
//		public DataSlogger(DataRow p)
//		{
//			pDR = p;
//		}



//		public DataSlogger(object p)
//		{
//			if (p == null)
//				throw new ArgumentException("DataSlogger::p is null");
//			else if (p is IDataRecord)
//				pRec = p as IDataRecord;
//			else if (p is DataRow)
//				pDR = p as DataRow;
//			else
//				throw new ArgumentException("DataSlogger::p is either a  IDataRecord or DataRow");
//		}

//		private object RecRow
//		{
//			get
//			{
//				if (pRec != null)
//					return pRec;
//				else if (pDR != null)
//					return pDR;
//				throw new InvalidProgramException("DataSlogger has pRec and pDR as null.");
//			}
//		}

//		private object RecRowSafe
//		{
//			get
//			{
//				if (pRec != null)
//					return pRec;
//				else if (pDR != null)
//					return pDR;
//				return null;
//			}
//		}

//		public string GetString(string colName) { return DataSlogger.GetString(this.RecRow, colName); }
//		public System.Int32 GetInt(string colName) { return DataSlogger.GetInt(RecRow, colName); }
//		public System.Int32 GetInt(string colName, int defVal) { return DataSlogger.GetInt(RecRow, colName, defVal); }
//		public System.Int32? GetIntN(string colName) { return DataSlogger.GetIntN(RecRow, colName); }
//		public System.Int64 GetLong(string colName) { return DataSlogger.GetLong(RecRow, colName); }
//		public System.Int64 GetLong(string colName, long defVal) { return DataSlogger.GetLong(RecRow, colName, defVal); }
//		public System.Int64? GetLongN(string colName) { return DataSlogger.GetLongN(RecRow, colName); }

//		public ulong GetULong(string colName) { return DataSlogger.GetULong(RecRow, colName); }
//		public ulong GetULong(string colName, ulong defVal) { return DataSlogger.GetULong(RecRow, colName, defVal); }
//		public ulong? GetULongN(string colName) { return DataSlogger.GetULongN(RecRow, colName); }

//		public DateTime? GetDateTimeN(string colName) { return DataSlogger.GetDateTimeN(RecRow, colName); }
//		public DateTime GetDateTime(string colName) { return DataSlogger.GetDateTime(RecRow, colName); }
//		public DateTime GetDateTime(string colName, DateTime def) { return DataSlogger.GetDateTime(RecRow, colName, def); }
//		public Guid? GetGuidN(string colName) { return GetGuidN(RecRow, colName); }
//		public Guid GetGuid(string colName, Guid def) { return GetGuid(RecRow, colName, def); }
//		public Guid GetGuid(string colName) { return GetGuid(RecRow, colName); }
//		public bool? GetBoolN(string colName) { return GetBoolN(RecRow, colName); }
//		public bool GetBool(string colName, bool def) { return GetBool(RecRow, colName, def); }
//		public bool GetBool(string colName) { return GetBool(RecRow, colName); }
//		public Decimal GetFloat(string colName) { return GetFloat(RecRow, colName); }
//		public Decimal GetFloat(string colName, decimal def) { return GetFloat(RecRow, colName, def); }
//		public Decimal? GetFloatN(string colName) { return GetFloatN(RecRow, colName); }
//		public object GetValue(string colName) { return GetValue(RecRow, colName); }
//		public byte[] GetByteArray(string colName) { return GetByteArray(RecRow, colName); }

//		public bool IsNull(string colName)
//		{
//			return IsNull(RecRow, colName);
//		}
//		public bool RowExists(string colName)
//		{
//			return RowExists(RecRow, colName);
//		}
//		public bool RowExists(string colName, bool safe)
//		{
//			if (safe)
//				return RowExists(RecRowSafe, colName, safe);
//			else
//				return RowExists(RecRow, colName, safe);
//		}

//		public static string GetString(object recrow, string colName)
//		{
//			if (recrow is DataRow)
//			{
//				DataRow p = recrow as DataRow;
//				if (!p.Table.Columns.Contains(colName))
//					throw new IndexOutOfRangeException(string.Format("ColumnName:  {0} does not exist in datarow/datarecord", colName));
//				object oRet = p[colName];

//				if (null == oRet)
//					return null;
//				else if (DBNull.Value == oRet)
//					return null;
//				else if (oRet is string)
//					return oRet as string;
//				return oRet.ToString();
//			}
//			else if (recrow is IDataRecord)
//			{
//				IDataRecord p = recrow as IDataRecord;
//				int loc = p.GetOrdinal(colName);    // location.  Can throw a IndexOutOfRangeException.
//				if (p.IsDBNull(loc))
//					return null;
//				object oRet = p.GetValue(loc);
//				if (oRet == null)
//					return null;
//				else if (oRet is string)
//					return oRet as string;
//				return oRet.ToString();
//			}
//			throw new InvalidOperationException("GetString recrow is not an IDataRecord or a DataRow.");
//		}
//		public static long? GetLongN(object recrow, string colName)
//		{
//			object oRet = null;
//			if (recrow is DataRow)
//			{
//				DataRow p = recrow as DataRow;
//				if (!p.Table.Columns.Contains(colName))
//					throw new IndexOutOfRangeException(string.Format("ColumnName:  {0} does not exist in datarow/datarecord", colName));
//				oRet = p[colName];
//			}
//			else if (recrow is IDataRecord)
//			{
//				IDataRecord p = recrow as IDataRecord;
//				int loc = p.GetOrdinal(colName);
//				if (p.IsDBNull(loc))
//					return null;
//				oRet = p.GetValue(loc);
//			}
//			else
//			{
//				throw new InvalidOperationException("GetLongN recrow is not an IDataRecord or a DataRow.");
//			}
//			if (null == oRet)
//				return null;
//			else if (DBNull.Value == oRet)
//				return null;
//			else if (oRet is System.UInt64)
//				return (long)oRet;
//			else if (oRet is System.UInt32)
//				return (long)oRet;
//			else if (oRet is System.UInt16)
//				return (long)oRet;
//			else if (oRet is System.Int16)
//				return (long)oRet;
//			else if (oRet is System.Int64)
//				return (long)oRet;
//			else if (oRet is System.Int32)
//				return (int)oRet;
//			else if (oRet is string)
//				return long.Parse((string)oRet);
//			throw new InvalidCastException(string.Format("Column:  {0} is not a long", colName));

//		}
//		public static System.Int64 GetLong(object recrow, string colName, System.Int64 defVal)
//		{
//			System.Int64? i = GetLongN(recrow, colName);
//			if (!i.HasValue)
//				return defVal;
//			return i.Value;
//		}
//		public static System.Int64 GetLong(object recrow, string colName)
//		{
//			System.Int64? i = GetLongN(recrow, colName);
//			if (!i.HasValue)
//				throw new InvalidCastException(string.Format("Column:  {0} is null and should not be.", colName));
//			return i.Value;
//		}
//		public static ulong? GetULongN(object recrow, string colName)
//		{
//			object oRet = null;
//			if (recrow is DataRow)
//			{
//				DataRow p = recrow as DataRow;
//				if (!p.Table.Columns.Contains(colName))
//					throw new IndexOutOfRangeException(string.Format("ColumnName:  {0} does not exist in datarow/datarecord", colName));
//				oRet = p[colName];
//			}
//			else if (recrow is IDataRecord)
//			{
//				IDataRecord p = recrow as IDataRecord;
//				int loc = p.GetOrdinal(colName);
//				if (p.IsDBNull(loc))
//					return null;
//				oRet = p.GetValue(loc);
//			}
//			else
//			{
//				throw new InvalidOperationException("GetLongN recrow is not an IDataRecord or a DataRow.");
//			}
//			if (null == oRet)
//				return null;
//			else if (DBNull.Value == oRet)
//				return null;
//			else if (oRet is System.UInt64)
//				return (ulong)oRet;
//			else if (oRet is System.UInt32)
//				return (ulong)oRet;
//			else if (oRet is System.UInt16)
//				return (ulong)oRet;
//			else if (oRet is System.Int16)
//				return (ulong)oRet;
//			else if (oRet is System.Int64)
//			{
//				long l = (long)oRet;
//				ulong ul = (ulong)l;
//				return ul;
//				//return (ulong)((int)oRet);
//			}
//			else if (oRet is System.Int32)
//				return (ulong)((int)oRet);
//			else if (oRet is string)
//				return ulong.Parse((string)oRet);
//			throw new InvalidCastException(string.Format("Column:  {0} is not a long", colName));

//		}
//		public static ulong GetULong(object recrow, string colName, ulong defVal)
//		{
//			ulong? i = GetULongN(recrow, colName);
//			if (!i.HasValue)
//				return defVal;
//			return i.Value;
//		}
//		public static ulong GetULong(object recrow, string colName)
//		{
//			ulong? i = GetULongN(recrow, colName);
//			if (!i.HasValue)
//				throw new InvalidCastException(string.Format("Column:  {0} is null and should not be.", colName));
//			return i.Value;
//		}
//		public static System.Int32 GetInt(object recrow, string colName)
//		{
//			System.Int32? i = GetIntN(recrow, colName);
//			if (!i.HasValue)
//				throw new InvalidCastException(string.Format("Column:  {0} is null and should not be.", colName));
//			return i.Value;
//		}
//		public static System.Int32 GetInt(object recrow, string colName, System.Int32 defVal)
//		{
//			System.Int32? i = GetIntN(recrow, colName);
//			if (!i.HasValue)
//				return defVal;
//			return i.Value;
//		}
//		public static System.Int32? GetIntN(object recrow, string colName)
//		{
//			object oRet = null;
//			if (recrow is DataRow)
//			{
//				DataRow p = recrow as DataRow;
//				if (!p.Table.Columns.Contains(colName))
//					throw new IndexOutOfRangeException(string.Format("ColumnName:  {0} does not exist in datarow/datarecord", colName));
//				oRet = p[colName];
//			}
//			else if (recrow is IDataRecord)
//			{
//				IDataRecord p = recrow as IDataRecord;
//				int loc = p.GetOrdinal(colName);
//				if (p.IsDBNull(loc))
//					return null;
//				oRet = p.GetValue(loc);
//			}
//			if (null == oRet)
//				return null;
//			else if (DBNull.Value == oRet)
//				return null;
//			else if (oRet is System.Byte)
//				return (int)((System.Byte)oRet);
//			else if (oRet is System.UInt16)
//				return (int)oRet;
//			else if (oRet is System.UInt32)
//				return (int)oRet;
//			else if (oRet is System.Int16)
//				return (int)oRet;
//			else if (oRet is System.Int32)
//				return (int)oRet;
//			else if (oRet is string)
//				return int.Parse((string)oRet);
//			throw new InvalidCastException(string.Format("Column:  {0} is not an int", colName));

//			/*
//				if ( null == oRet )
//					return null;
//				else if ( oRet is System.Int32 )
//					return (int)oRet;
//				else if ( oRet is string )
//					return int.Parse((string)oRet);
//				throw new InvalidCastException(string.Format("Column:  {0} is not an int", colName));
//			}
//			throw new InvalidOperationException("GetInt recrow is not an IDataRecord or a DataRow.");
//		*/
//		}
//		public static DateTime? GetDateTimeN(object recrow, string colName)
//		{
//			if (recrow is DataRow)
//			{
//				DataRow p = recrow as DataRow;
//				if (!p.Table.Columns.Contains(colName))
//					throw new IndexOutOfRangeException(string.Format("ColumnName:  {0} does not exist in datarow/datarecord", colName));
//				object oRet = p[colName];
//				if (null == oRet)
//					return null;
//				else if (DBNull.Value == oRet)
//					return null;
//				else if (oRet is DateTime)
//					return (DateTime)oRet;
//				else if (oRet is string)
//					return DateTime.Parse((string)oRet);
//				throw new InvalidCastException(string.Format("Column:  {0} is not an int", colName));
//			}
//			else if (recrow is IDataRecord)
//			{
//				IDataRecord p = recrow as IDataRecord;
//				int loc = p.GetOrdinal(colName);
//				if (p.IsDBNull(loc))
//					return null;
//				object oRet = p.GetValue(loc);
//				if (null == oRet)
//					return null;
//				else if (oRet is DateTime)
//					return (DateTime)oRet;
//				else if (oRet is string)
//					return DateTime.Parse((string)oRet);
//				throw new InvalidCastException(string.Format("Column:  {0} is not an int", colName));
//			}
//			throw new InvalidOperationException("GetDateTimeN recrow is not an IDataRecord or a DataRow.");
//		}
//		public static DateTime GetDateTime(object recrow, string colName)
//		{
//			DateTime? dt = GetDateTimeN(recrow, colName);
//			if (!dt.HasValue)
//				throw new InvalidCastException(string.Format("Column:  {0} is null and should not be.", colName));
//			else
//				return dt.Value;
//		}
//		public static DateTime GetDateTime(object recrow, string colName, DateTime def)
//		{
//			DateTime? dt = GetDateTimeN(recrow, colName);
//			if (!dt.HasValue)
//				return def;
//			else
//				return dt.Value;
//		}
//		public static Guid? GetGuidN(object recrow, string colName)
//		{
//			if (recrow is DataRow)
//			{
//				DataRow p = recrow as DataRow;
//				if (!p.Table.Columns.Contains(colName))
//					throw new IndexOutOfRangeException(string.Format("ColumnName:  {0} does not exist in datarow/datarecord", colName));
//				object oRet = p[colName];
//				if (null == oRet)
//					return null;
//				else if (DBNull.Value == oRet)
//					return null;
//				else if (oRet is string)
//					return new Guid((string)oRet);
//				else if (oRet is Guid)
//					return (Guid)oRet;
//				else if (oRet is string)
//					return new Guid((string)oRet);
//				throw new InvalidCastException(string.Format("Column:  {0} is not an Guid", colName));
//			}
//			else if (recrow is IDataRecord)
//			{
//				IDataRecord p = recrow as IDataRecord;
//				int loc = p.GetOrdinal(colName);
//				if (p.IsDBNull(loc))
//					return null;
//				object oRet = p.GetValue(loc);
//				if (null == oRet)
//					return null;
//				else if (oRet is string)
//					return new Guid((string)oRet);
//				else if (oRet is Guid)
//					return (Guid)oRet;
//				else if (oRet is string)
//					return new Guid((string)oRet);
//				throw new InvalidCastException(string.Format("Column:  {0} is not an Guid", colName));
//			}
//			throw new InvalidOperationException("GetGuidN recrow is not an IDataRecord or a DataRow.");
//		}
//		public static Guid GetGuid(object recrow, string colName, Guid def)
//		{
//			return GetGuidN(recrow, colName) ?? def;
//		}
//		public static Guid GetGuid(object recrow, string colName)
//		{
//			Guid? g = GetGuidN(recrow, colName);
//			if (!g.HasValue)
//				throw new InvalidCastException(string.Format("Column:  {0} is null and should not be.", colName));
//			return g.Value;
//		}

//		public static bool? GetBoolN(object recrow, string colName)
//		{
//			object oRet = null;
//			if (recrow is DataRow)
//			{
//				DataRow p = recrow as DataRow;
//				if (!p.Table.Columns.Contains(colName))
//					throw new IndexOutOfRangeException(string.Format("ColumnName:  {0} does not exist in datarow/datarecord", colName));
//				oRet = p[colName];
//			}
//			/*
//				if ( null == oRet )
//					return null;
//				else if ( DBNull.Value == oRet )
//					return null;
//				else if ( oRet is bool )
//					return (bool)oRet;
//				else if ( oRet is string )
//					return 0 == string.Compare("true", (string)oRet, StringComparison.InvariantCultureIgnoreCase) || 0 == string.Compare("1", (string)oRet, StringComparison.InvariantCultureIgnoreCase );
//				throw new InvalidCastException(string.Format("Column:  {0} is not a bool", colName));
//			}
//			*/
//			else if (recrow is IDataRecord)
//			{
//				IDataRecord p = recrow as IDataRecord;
//				int loc = p.GetOrdinal(colName);
//				if (p.IsDBNull(loc))
//					return null;
//				oRet = p.GetValue(loc);
//			}
//			if (null == oRet)
//				return null;
//			else if (DBNull.Value == oRet)
//				return null;
//			else if (oRet.GetType() == typeof(DBNull))
//				return null;
//			else if (oRet is bool)
//				return (bool)oRet;
//			else if (oRet is int)
//				return (((int)oRet) == 1);
//			else if (oRet is uint)
//				return (((uint)oRet) == 1);
//			else if (oRet is System.Int16)
//				return (((System.Int16)oRet) == 1);
//			else if (oRet is System.UInt16)
//				return (((System.UInt16)oRet) == 1);
//			else if (oRet is byte)
//				return (((int)((byte)oRet)) == 1);
//			else if (oRet is System.UInt64)
//				return (((System.UInt64)oRet) == 1);
//			else if (oRet is System.Int64)
//				return (((System.Int64)oRet) == 1);

//			else if (oRet is string)
//				return 0 == string.Compare("true", (string)oRet, StringComparison.InvariantCultureIgnoreCase) || 0 == string.Compare("1", (string)oRet, StringComparison.InvariantCultureIgnoreCase);
//			throw new InvalidCastException(string.Format("Column:  {0} is not a bool", colName));
//			//}
//			//throw new InvalidOperationException("GetBoolN recrow is not an IDataRecord or a DataRow.");
//		}
//		public static bool GetBool(object recrow, string colName, bool def)
//		{
//			bool? b = GetBoolN(recrow, colName);
//			if (b.HasValue)
//				return b.Value;
//			else
//				return def;
//		}
//		public static bool GetBool(object recrow, string colName)
//		{
//			bool? b = GetBoolN(recrow, colName);
//			if (b.HasValue)
//				return b.Value;
//			throw new Exception(string.Format("Column:  {0} contains a null", colName));
//		}
//		public static Decimal GetFloat(object recrow, string colName)
//		{
//			decimal? d = GetFloatN(recrow, colName);
//			if (!d.HasValue)
//				throw new Exception(string.Format("Column:  {0} contains a null", colName));
//			return d.Value;
//		}
//		public static Decimal GetFloat(object recrow, string colName, decimal def)
//		{
//			decimal? d = GetFloatN(recrow, colName);
//			if (!d.HasValue)
//				d = def;
//			return d.Value;
//		}
//		public static Decimal? GetFloatN(object recrow, string colName)
//		{
//			object oRet = null;
//			string sRet = null;
//			decimal dRet = Decimal.MinValue;

//			if (recrow is DataRow)
//			{
//				DataRow p = recrow as DataRow;
//				if (!p.Table.Columns.Contains(colName))
//					throw new IndexOutOfRangeException(string.Format("ColumnName:  {0} does not exist in datarow/datarecord", colName));
//				oRet = p[colName];
//			}
//			else if (recrow is IDataRecord)
//			{
//				IDataRecord p = recrow as IDataRecord;
//				int loc = p.GetOrdinal(colName);
//				if (p.IsDBNull(loc))
//					return null;
//				oRet = p.GetValue(loc);
//			}
//			else
//			{
//				throw new InvalidOperationException("GetFloatN recrow is not an IDataRecord or a DataRow.");
//			}
//			try { if (oRet != null) { sRet = oRet.ToString(); } } catch { } // intentional don't do anything

//			if (null == oRet)
//				return null;
//			else if (DBNull.Value == oRet)
//				return null;
//			else if (oRet.GetType() == typeof(DBNull))
//				return null;
//			else if (oRet is float)//|| oRet is double || oRet is decimal )
//				return ((Decimal)((float)oRet));
//			else if (oRet is double)
//				return (decimal)((double)oRet);
//			else if (oRet is decimal)
//				return (decimal)oRet;
//			else if (oRet is string)
//				return Decimal.Parse((string)oRet);
//			else if (oRet is int)
//				return (decimal)((int)oRet);
//			else if (Decimal.TryParse(sRet, out dRet))
//			{
//				return dRet;
//			}
//			throw new InvalidCastException(string.Format("Column:  {0} is not a float or float subtype", colName));


//		}
//		public static object GetValue(object recrow, string colName)
//		{
//			if (recrow is DataRow)
//			{
//				DataRow p = recrow as DataRow;
//				if (!p.Table.Columns.Contains(colName))
//					throw new IndexOutOfRangeException(string.Format("ColumnName:  {0} does not exist in datarow/datarecord", colName));
//				object oRet = p[colName];
//				if (oRet.GetType() == typeof(DBNull))
//					oRet = null;
//				return oRet;
//			}
//			else if (recrow is IDataRecord)
//			{
//				IDataRecord p = recrow as IDataRecord;
//				int loc = p.GetOrdinal(colName);
//				if (p.IsDBNull(loc))
//					return null;
//				object oRet = p.GetValue(loc);
//				return oRet;
//			}
//			throw new InvalidOperationException("GetValue recrow is not an IDataRecord or a DataRow.");
//		}

//		/*
//		public static byte[] GetByteArray(object recrow, string colName)
//		{
//			if ( recrow is DataRow )
//			{
//				DataRow p = recrow as DataRow;
//				if ( !p.Table.Columns.Contains(colName) )
//					throw new IndexOutOfRangeException(string.Format("ColumnName:  {0} does not exist in datarow/datarecord", colName));
//				object oRet = p[colName];
//				if ( oRet.GetType() == typeof(DBNull) )
//					oRet = null;
//				return oRet;
//			}
//			else if ( recrow is IDataRecord )
//			{
//				IDataRecord p = recrow as IDataRecord;
//				int loc = p.GetOrdinal(colName);
//				if ( p.IsDBNull(loc) )
//					return null;
//				object oRet = p.GetValue(loc);
//				return oRet;
//			}
//			throw new InvalidOperationException("GetByteArray recrow is not an IDataRecord or a DataRow.");
//		}
//		*/

//		public static byte[] GetByteArray(object recrow, string colName)
//		{
//			if (recrow is DataRow)
//			{
//				DataRow p = recrow as DataRow;
//				if (!p.Table.Columns.Contains(colName))
//					throw new IndexOutOfRangeException(string.Format("ColumnName:  {0} does not exist in datarow/datarecord", colName));
//				object oRet = p[colName];
//				if (null == oRet)
//					return null;
//				else if (DBNull.Value == oRet)
//					return null;
//				else
//					return (byte[])p[colName];
//				throw new InvalidCastException(string.Format("Column:  {0} is not a byte[]", colName));
//			}
//			else if (recrow is IDataRecord)
//			{
//				IDataRecord p = recrow as IDataRecord;
//				int loc = p.GetOrdinal(colName);
//				if (p.IsDBNull(loc))
//					return null;
//				object oRet = p.GetValue(loc);
//				if (null == oRet)
//					return null;
//				else
//					return (byte[])p[loc];
//				throw new InvalidCastException(string.Format("Column:  {0} is not a byte[]", colName));
//			}
//			throw new InvalidOperationException("GetDateTimeN recrow is not an IDataRecord or a DataRow.");
//		}

//		public static bool IsNull(object recrow, string colName)
//		{
//			object o = null;
//			if (recrow is DataRow)
//			{
//				DataRow p = recrow as DataRow;
//				if (!p.Table.Columns.Contains(colName))
//					throw new IndexOutOfRangeException(string.Format("ColumnName:  {0} does not exist in datarow/datarecord", colName));
//				o = p[colName];
//			}
//			else if (recrow is IDataRecord)
//			{
//				IDataRecord p = recrow as IDataRecord;
//				int loc = p.GetOrdinal(colName);
//				if (p.IsDBNull(loc))
//					return true;
//				o = p.GetValue(loc);
//			}
//			return (o == null || o == DBNull.Value);
//		}
//		public static bool RowExists(object recrow, string colName, bool safe)
//		{
//			if (recrow is DataRow)
//			{
//				DataRow p = recrow as DataRow;
//				return p.Table.Columns.Contains(colName);
//			}
//			else if (recrow is IDataRecord)
//			{
//				IDataRecord p = recrow as IDataRecord;
//				try
//				{
//					return p.GetOrdinal(colName) >= 0;
//				}
//				catch (IndexOutOfRangeException e)
//				{
//					return false;
//				}
//			}
//			else
//			{
//				if (safe)
//					return false;
//				else
//					throw new ArgumentException(string.Format("RecRow is not of type DataRow or IDataRecord.  It is:  {0}", recrow == null ? "Null" : recrow.GetType().Name));
//			}
//		}

//		// Future:  Write these as extentions?  Or add these as a partial class.
//		public static bool RowExists(object recrow, string colName)
//		{
//			return RowExists(recrow, colName, false);
//		}

//		public static int? GetMultiSourceIntN(DataSlogger[] arSl, string colName)
//		{
//			int? ret = null;
//			foreach (DataSlogger ds in arSl)
//			{
//				if (ds.RowExists(colName, true))
//				{
//					ret = ds.GetIntN(colName);
//					if (ret != null)
//						break;
//				}
//			}

//			return ret;
//		}

//		public static int GetMultiSourceInt(DataSlogger[] arSl, string colName)
//		{
//			int? ret = null;
//			foreach (DataSlogger ds in arSl)
//			{
//				if (ds.RowExists(colName, true))
//				{
//					ret = ds.GetIntN(colName);
//					if (ret != null)
//						break;
//				}
//			}

//			if (!ret.HasValue)
//				throw new Exception(string.Format("Column:  {0} contains a null", colName));
//			return ret.Value;
//		}

//		public static decimal GetMultiSourceFloat(DataSlogger[] arSl, string colName)
//		{
//			decimal? ret = null;
//			foreach (DataSlogger ds in arSl)
//			{
//				if (ds.RowExists(colName, true))
//				{
//					ret = ds.GetFloatN(colName);
//					if (ret != null)
//						break;
//				}
//			}

//			if (!ret.HasValue)
//				throw new Exception(string.Format("Column:  {0} contains a null", colName));
//			return ret.Value;
//		}

//		public static decimal? GetMultiSourceFloatN(DataSlogger[] arSl, string colName)
//		{
//			decimal? ret = null;
//			foreach (DataSlogger ds in arSl)
//			{
//				if (ds.RowExists(colName, true))
//				{
//					ret = ds.GetFloatN(colName);
//					if (ret != null)
//						break;
//				}
//			}

//			return ret;
//		}

//		public static string GetMultiSourceString(DataSlogger[] arSl, string colName)
//		{
//			string ret = string.Empty;
//			foreach (DataSlogger ds in arSl)
//			{
//				if (ds.RowExists(colName, true))
//				{
//					ret = ds.GetString(colName);
//					if (ret != null)
//						break;
//				}
//			}
//			return ret;
//		}

//		public static bool GetMultiSourceBool(DataSlogger[] arSl, string colName)
//		{
//			bool? ret = null;
//			foreach (DataSlogger ds in arSl)
//			{
//				if (ds.RowExists(colName, true))
//				{
//					ret = ds.GetBoolN(colName);
//					if (ret != null)
//						break;
//				}
//			}

//			if (!ret.HasValue)
//				throw new Exception(string.Format("Column:  {0} contains a null", colName));
//			return ret.Value;
//		}

//		public static bool? GetMultiSourceBoolN(DataSlogger[] arSl, string colName)
//		{
//			bool? ret = null;
//			foreach (DataSlogger ds in arSl)
//			{
//				if (ds.RowExists(colName, true))
//				{
//					ret = ds.GetBoolN(colName);
//					if (ret != null)
//						break;
//				}
//			}

//			return ret;
//		}
//	}

//}

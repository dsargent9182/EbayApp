using Ebay.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using Ebay.DataLayer;

namespace Ebay.DataLayer
{
	public class WatchList
	{
		[Display(Name = "Ebay Id")]
		public Int64 EbayId { get; set; }

		public string Title { get; set; }

		[Display(Name = "Sold Count")]
		[DisplayFormat(DataFormatString = "{0:N0}")]
		public int SoldCount { get; set; }

		[Display(Name = "Total Sold")]
		[DisplayFormat(DataFormatString = "{0:N0}")]
		public int TotalSold { get; set; }

		[Display(Name = "Sold Amount")]
		[DisplayFormat(DataFormatString = "{0:C2}")]
		public decimal SoldAmount { get; set; }

		[DisplayFormat( DataFormatString = "{0:C2}")]
		public decimal Price { get; set; }

		[Display(Name = "Ebay User ")]
		public string EbayUser { get; set; }

		[Display(Name = "Date Last Sold")]
		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime LastDateSold { get; set; }

		//public long Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

		public static IEnumerable<WatchList> Get(string ebayUser,DateTime? dateToRun,Config.SQLConfig config, int daysToRun = 30)
		{
			SQL sql = new SQL(config);

			List<DLParam> list = new List<DLParam>();
			if( !string.IsNullOrWhiteSpace(ebayUser))
			{
				list.Add(new DLParam("EbayUser", ebayUser));
			}
			if( dateToRun.HasValue)
			{
				list.Add(new DLParam("End", dateToRun));
			}
			list.Add(new DLParam("Days", daysToRun));
			
			DataTable dt = sql.ExecDataTable("SnipeList_Get", true, new DLParam[] {
				new DLParam("SnipeList_Get","")
			});
			_ = dt ?? throw new NullReferenceException("Unable to retrieve records");

			return ConvertDataTable(dt);
		}
		public static List<WatchList> ConvertDataTable(DataTable dt)
		{
			List<WatchList> list = new List<WatchList>();
			
			foreach(DataRow dr in dt.Rows)
			{
				list.Add(ConvertDataRow(dr));
			}
			return list;
		}
		public static WatchList ConvertDataRow(DataRow dr)
		{
			DataSlogger ds = new DataSlogger(dr);
			WatchList wl = new WatchList();
			wl.EbayId = ds.GetLong("EbayId");
			wl.EbayUser = ds.GetString("EbayUser");
			wl.SoldCount = ds.GetInt("SoldCount");
			wl.Title = ds.GetString("Title");
			wl.Price = ds.GetFloat("Price");
			return wl;
		}
	}

	

}	

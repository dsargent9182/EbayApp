namespace DS.EbayAPI.Dto
{
	public class WatchListDto
	{
		public Int64 EbayId { get; set; }

		public string Title { get; set; }

		//[Display(Name = "Sold Count")]
		//[DisplayFormat(DataFormatString = "{0:N0}")]
		public int SoldCount { get; set; }

		//[Display(Name = "Total Sold")]
		//[DisplayFormat(DataFormatString = "{0:N0}")]
		public int TotalSold { get; set; }

		//[Display(Name = "Sold Amount")]
		//[DisplayFormat(DataFormatString = "{0:C2}")]
		public decimal SoldAmount { get; set; }

		//[DisplayFormat(DataFormatString = "{0:C2}")]
		public decimal Price { get; set; }

		//[Display(Name = "Ebay User ")]
		public string EbayUser { get; set; }

		//[Display(Name = "Date Last Sold")]
		//[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime LastDateSold { get; set; }
	}
}

using System;
using System.ComponentModel.DataAnnotations;

namespace Ebay.DataLayer
{
	public class Message
	{
		public int Id { get; set; }

		public string Sender { get; set; }

		public string StoreName { get; set; }

		public string BuyerUserName { get; set; }

		public string Subject { get; set; }

		public string Text { get; set; }

		public string ItemId { get; set; }

		public string SendToName { get; set; }

		public string ItemTitle { get; set; }

		[DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime Received { get; set; }

		public string MessageId { get; set; }
	}
}

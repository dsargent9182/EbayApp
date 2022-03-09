﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DS.Domain.Models
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
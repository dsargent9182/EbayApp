using EbayApp.DataLayer;
using Microsoft.EntityFrameworkCore;

namespace EbayApp.Data
{
	public class WatchListContext : DbContext
	{
		public WatchListContext(DbContextOptions<WatchListContext> options) : base(options)
		{

		}

		public DbSet<WatchList> Courses { get; set; }
	}
}

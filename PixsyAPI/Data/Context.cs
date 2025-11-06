using Microsoft.EntityFrameworkCore;
using PixsyAPI.Models;

namespace PixsyAPI.Data
{
	public class AppContext : DbContext
	{
		public AppContext(DbContextOptions<AppContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Board> Boards { get; set; }
		public DbSet<Picture> Pictures { get; set; }
		public DbSet<Tag> Tags { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<User>()
				.HasKey(u => u.UserID);

			modelBuilder.Entity<Board>()
				.HasKey(b => b.BoardID);

			modelBuilder.Entity<Picture>()
				.HasKey(p => p.PictureID);

			modelBuilder.Entity<Tag>()
				.HasKey(t => t.TagID);

			modelBuilder.Entity<Board>()
				.Property(b => b.BoardVisibility)
				.HasConversion<int>();

			modelBuilder.Entity<User>()
				.Property(u => u.Role)
				.HasConversion<int>();
		}
	}
}

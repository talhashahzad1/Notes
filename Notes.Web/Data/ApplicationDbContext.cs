using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Notes.Web.Models;

namespace Notes.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Asset> Assets { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Notebook> Notebooks { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<TagItem> TagItems { get; set; }
        public DbSet<TagItemView> TagItemViews { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<TagItem>().HasIndex(ti => new { ti.ItemId, ti.ItemType, ti.TagId }).IsUnique();
        }
    }
}

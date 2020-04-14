using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ModusCreateSampleApp.Data.Entities;

namespace ModusCreateSampleApp.Data
{
    public class AppDatabaseContext : IdentityDbContext<User>
    {
        public DbSet<Feed> Feeds { get; set; }
        public DbSet<FeedItem> FeedItems { get; set; }
        public DbSet<FeedUserSubscription> FeedUserSubscriptions { get; set; }

        public AppDatabaseContext(DbContextOptions<AppDatabaseContext> options)
            : base(options)
        {

        }


        

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<FeedUserSubscription>()
                .HasKey(s => new { s.UserSubscriberId, s.FeedId });

            builder.Entity<FeedUserSubscription>()
                .HasOne(s => s.Feed)
                .WithMany(f => f.FeedUserSubscriptions)
                .HasForeignKey(s => s.FeedId);

            builder.Entity<FeedUserSubscription>()
                .HasOne(s => s.Subscriber)
                .WithMany(u => u.FeedSubscriptions)
                .HasForeignKey(s => s.UserSubscriberId);

                   

            base.OnModelCreating(builder);


        }
    }
}

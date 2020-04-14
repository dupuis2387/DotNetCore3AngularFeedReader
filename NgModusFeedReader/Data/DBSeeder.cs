using System;
using System.Collections.Generic;
using System.Linq;
using ModusCreateSampleApp.Data.Entities;

namespace ModusCreateSampleApp.Data
{
    /// <summary>
    /// This class will seed our database with data, if needed
    /// </summary>
    public class DBSeeder
    {
        private readonly AppDatabaseContext _context;
        public DBSeeder(AppDatabaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Seed with sample categories
        /// </summary>
        /// <returns></returns>
        private IEnumerable<FeedCategory> SeedCategories()
        {
            //if we dont have categories, we can have feeds, and we cant have feed items
            //so, seed them
            IList<string> categories = new List<string>()
                {
                    "Movies",
                    "News",
                    "Astrology"
                };
            foreach (var category in categories)
            {
                _context.FeedCategories.Add(new FeedCategory()
                {
                    Name = category
                });
            }
            _context.SaveChanges();
            return _context.FeedCategories.AsEnumerable();
        }

        /// <summary>
        /// Seed with sample feeds belonging to categories
        /// </summary>
        /// <param name="categories"></param>
        /// <returns></returns>
        private IEnumerable<Feed> SeedFeeds(IEnumerable<FeedCategory> categories)
        {
            foreach (var category in categories)
            {
                _context.Feeds.Add(new Feed()
                {
                    FeedCategory = category,
                    Name = $"An Arbitrary {category.Name} Feed",
                    ShortDescription = $"This is a short {category.Name} feed description",
                    LongDescription = $"This is a long feed description. A lot of text could go in this {category.Name} feed description",
                });
            }
            _context.SaveChanges();
            return _context.Feeds.AsEnumerable();
        }

        /// <summary>
        /// Seed with sample feed items, belonging to feeds
        /// </summary>
        /// <param name="Feeds"></param>
        private void SeedFeedItems(IEnumerable<Feed> Feeds)
        {
            foreach (var feed in Feeds)
            {

                _context.FeedItems.Add(new FeedItem()
                {
                    DatePublished = DateTime.Now,
                    Feed = feed,
                    Heading = $"Insane {feed.FeedCategory.Name} secrets that will drive you insane!",
                    ShortDescription = $"Read below to get more information about these *Crazy* {feed.FeedCategory.Name} secrets.",
                    LongDescription = $"Oh, so you're actually interested in these {feed.FeedCategory.Name} secrets? Is there something wrong with you? If so, please enter you credit card number below, and we'll ship you a mystical candle, with healing powers that'll cure you in no time! All for $999,999.99. Act Now!"
                });
            }
            _context.SaveChanges();

        }

        /// <summary>
        /// Seed our DB with data, if needed
        /// </summary>
        public void SeedData()
        {
            //make sure the db actually exists
            _context.Database.EnsureCreated();


            //check if we have category data
            if (!_context.FeedCategories.Any())
            {
                //if we don't, we don't have anything else either. so, do it up!
                var seededCategories = SeedCategories();
                var seededFeeds = SeedFeeds(seededCategories);
                SeedFeedItems(seededFeeds);

            }


        }
    }
}

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
        /// Seed with sample feeds
        /// </summary>
        /// <returns></returns>
        private IEnumerable<Feed> SeedFeeds()
        {

            //seed some sample feeds
            IList<string> sampleFeedCategories = new List<string>()
                {
                    "Movies",
                    "News",
                    "Astrology",
                    "Gaming",
                    "Gardening"
                };
            foreach (var category in sampleFeedCategories)
            {
                _context.Feeds.Add(new Feed()
                {

                    Name = category,
                    ShortDescription = $"This is a short {category} feed description",
                    LongDescription = $"A lot of text could go in this {category} feed description",
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
                switch (feed.Name)
                {
                    case "Astrology":
                        _context.FeedItems.Add(new FeedItem()
                        {
                            DatePublished = DateTime.Now.AddHours(1),
                            Feed = feed,
                            Heading = $"Insane {feed.Name} secrets that will drive you insane!",
                            ShortDescription = $"Read below to get more information about these *Crazy* {feed.Name} secrets.",
                            LongDescription = $"Oh, so you're actually interested in these {feed.Name} secrets? Is there something wrong with you? If so, please enter you credit card number below, and we'll ship you a mystical candle, with healing powers that'll cure you in no time! All for $999,999.99. Act Now!"
                        });
                        break;
                    case "Gaming":
                        _context.FeedItems.Add(new FeedItem()
                        {
                            DatePublished = DateTime.Now.AddDays(-1),
                            Feed = feed,
                            Heading = $"OMG Control!!",
                            YoutubeVideoId = "DGtuQRohHds",
                            ShortDescription = $"The game Control is *INSANE*. You have to play it!!!!!!",
                            LongDescription = $"There's this crazy Ashtray Maze level, and the soundtrack that feeds it is like everything i could want in life. Seriously, you have to check it out!!!"
                        });
                        break;
                    default:
                        _context.FeedItems.Add(new FeedItem()
                        {
                            DatePublished = DateTime.Now,
                            Feed = feed,
                            Heading = $"Just a typical {feed.Name} feed heading!",
                            ShortDescription = $"A short {feed.Name} description could go here.",
                            LongDescription = $"A long {feed.Name} description could go here."
                        });
                        break;


                }

                
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


            //check if we have feeds
            if (!_context.Feeds.Any())
            {
                //if we don't, we don't have anything else either. so, do it up!
                var seededFeeds = SeedFeeds();
                SeedFeedItems(seededFeeds);

            }


        }
    }
}

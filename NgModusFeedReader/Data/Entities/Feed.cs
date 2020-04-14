using System;
using System.Collections.Generic;

namespace ModusCreateSampleApp.Data.Entities
{
    public class Feed
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        /// <summary>
        /// category it belongs to
        /// </summary>
        public virtual FeedCategory FeedCategory { get; set; }

        /// <summary>
        /// child feed items
        /// </summary>
        public virtual ICollection<FeedItem> FeedItems { get; set; }

        /// <summary>
        /// collection of users subscribed to a given feed
        /// </summary>
        public virtual ICollection<FeedUserSubscription> FeedUserSubscriptions { get; set; }
    }
}

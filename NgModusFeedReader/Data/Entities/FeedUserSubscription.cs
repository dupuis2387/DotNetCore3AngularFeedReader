using System;
namespace ModusCreateSampleApp.Data.Entities
{
    /// <summary>
    /// Relationship table to manage subscriptions of users to feeds
    /// </summary>
    public class FeedUserSubscription
    {
        
        public virtual Feed Feed { get; set; }
        public int FeedId { get; set; }

        public virtual User Subscriber { get; set; }
        public string UserSubscriberId { get; set; }
    }
}

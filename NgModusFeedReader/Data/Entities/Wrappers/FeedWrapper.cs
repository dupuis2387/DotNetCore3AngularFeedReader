using System;
using ModusCreateSampleApp.Data.Entities;

namespace NgModusFeedReader.Data.Entities.Wrappers
{
    public class FeedWrapper: Feed
    {
        public bool Subscribed { get; set; }
    }
}

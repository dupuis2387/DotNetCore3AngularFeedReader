using System;
namespace ModusCreateSampleApp.Data.Entities
{
    public class FeedItem
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public DateTime DatePublished { get; set; }

        /// <summary>
        /// navigation prop for parent feed stream
        /// </summary>
        public virtual Feed Feed {get;set;}

        /// <summary>
        /// Id reference for parent feed stream
        /// </summary>
        public int FeedId { get; set; }
    }
}

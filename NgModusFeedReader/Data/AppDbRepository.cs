using System;
using System.Collections.Generic;
using ModusCreateSampleApp.Data.Entities;
using System.Linq;
using Microsoft.Extensions.Logging;
using NgModusFeedReader.Data.Entities.Wrappers;
using Microsoft.EntityFrameworkCore;

namespace ModusCreateSampleApp.Data
{
    public interface IAppDbRepository
    {
        
        IEnumerable<FeedWrapper> GetFeeds(int? feedId, string userId);
        IEnumerable<FeedItem> GetFeedItemsByFeedId(int feedId);
        IEnumerable<FeedItem> SearchFeedItems(string searchTerm);
        IEnumerable<FeedItem> GetAllFeedItems();
    }

    

    public class AppDbRepository : IAppDbRepository
    {
        private readonly AppDatabaseContext _appDbContext;
        private readonly ILogger<AppDbRepository> _logger;

        public AppDbRepository(AppDatabaseContext appDbContext, ILogger<AppDbRepository> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }

        public IEnumerable<FeedItem> SearchFeedItems(string searchTerm)
        {
            try
            {
                return _appDbContext.FeedItems
                    .Where(item => EF.Functions.Like(item.Heading, $"%{searchTerm }%")
                    || EF.Functions.Like(item.ShortDescription, $"%{searchTerm }%")
                    || EF.Functions.Like(item.LongDescription, $"%{searchTerm }%"))
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"SearchFeedItems with the term {searchTerm} threw an error: {ex}");
                return null;
            }
        }

        public IEnumerable<FeedItem> GetAllFeedItems()
        {
            try
            {
                return _appDbContext.FeedItems
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllFeedItems threw an error: {ex}");
                return null;
            }
        }

        

        /// <summary>
        /// Get all <see cref="T:Feed"/>s
        /// </summary>
        /// <returns>All the <see cref="T:Feed"/>s or null if an exception takes place. The exception is logged</returns>
        public IEnumerable<FeedWrapper> GetFeeds(int? feedId, string userId)
        {
            try
            {
                //cheating and using a shortcut, not good production code
                if (feedId.HasValue)
                {
                    return _appDbContext.Feeds
                        .Where(feed => feed.Id == feedId.Value)
                        .Select(f => new FeedWrapper()
                        {

                            Subscribed = f.FeedUserSubscriptions.Where(s => s.UserSubscriberId == userId).Any(),
                            Id = f.Id,
                            Name = f.Name,
                            ShortDescription = f.ShortDescription,
                            LongDescription = f.LongDescription

                        })
                        .ToList();
                }
                else
                {
                    return _appDbContext.Feeds
                        .OrderBy(feed => feed.Id)
                        .Select(f => new FeedWrapper()
                        {

                            Subscribed = f.FeedUserSubscriptions.Where(s => s.UserSubscriberId == userId).Any(),
                            Id = f.Id,
                            Name = f.Name,
                            ShortDescription = f.ShortDescription,
                            LongDescription = f.LongDescription

                        })
                        .ToList();
                }


            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllFeeds threw an error: {ex}");
                return null;

            }
        }




        

        /// <summary>
        /// Get all individual <see cref="T:FeedItem"/>s for a given feed id of a <see cref="Feed"/>
        /// </summary>
        /// <param name="feedId">A given <see cref="T:Feed"/>s Id</param>
        /// <returns>An <see cref="T:IEnumerable"/> of <see cref="T:FeedItem"/>s that belongs to a specific feed id or null if an exception takes place. The exception is logged</returns>
        public IEnumerable<FeedItem> GetFeedItemsByFeedId(int feedId)
        {
            try
            {
                return _appDbContext.FeedItems
                    .Where(item => item.FeedId == feedId)
                    .OrderBy(item => item.DatePublished)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetFeedItemsByFeed, with feedId='{feedId}' threw an error: {ex}");
                return null;
            }
        }

    }
}

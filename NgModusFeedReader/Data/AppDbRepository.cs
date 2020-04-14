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
        IEnumerable<FeedCategory> GetAllCategories();
        IEnumerable<FeedWrapper> GetFeeds(int? feedId, string userId);
        IEnumerable<Feed> GetFeedByCategory(string categoryName);
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
        /// Get all feed categories
        /// </summary>
        /// <returns>An <see cref="T:IEnumerable"/> of all <see cref="FeedCategory"/> or null if an exception takes place. The exception is logged</returns>
        public IEnumerable<FeedCategory> GetAllCategories()
        {
            try
            {
                return _appDbContext.FeedCategories
                    .OrderBy(category => category.Name)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetAllCategories threw an error: {ex}");
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
                            FeedCategory = f.FeedCategory,
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
                            FeedCategory = f.FeedCategory,
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
        /// Get all the <see cref="T:Feed"/>s for a given <see cref="T:FeedCategory"/> name
        /// </summary>
        /// <param name="categoryName">A <see cref="T:FeedCategory"/> name that a feed belongs to</param>
        /// <returns>An <see cref="T:IEnumerable"/> of <see cref="T:Feed"/>s that belongs to a specific category name or null if an exception takes place. The exception is logged</returns>
        public IEnumerable<Feed> GetFeedByCategory(string categoryName)
        {
            try
            {
                return _appDbContext.Feeds
                    //use the navigation prop to categories, to fetch feeds associated with a given category name
                    .Where(feed => feed.FeedCategory.Name == categoryName)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetFeedByCategory, with categoryName='{categoryName}' threw an error: {ex}");
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

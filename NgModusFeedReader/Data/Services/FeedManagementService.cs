using System;
using Microsoft.Extensions.Logging;
using ModusCreateSampleApp.Data;
using ModusCreateSampleApp.Data.Entities;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NgModusFeedReader.Data.Services
{

    public interface IFeedManagementService
    {
        bool Subscribe(int feedId, string userId);
        bool Unubscribe(int feedId, string userId);
        IEnumerable<FeedItem> GetSubscriberFeedStream(string userId);

    }

    public class FeedManagementService : IFeedManagementService
    {


        private readonly AppDatabaseContext _appDbContext;
        private readonly ILogger<FeedManagementService> _logger;

        public FeedManagementService(AppDatabaseContext appDbContext, ILogger<FeedManagementService> logger)
        {
            _appDbContext = appDbContext;
            _logger = logger;
        }


        public IEnumerable<FeedItem> GetSubscriberFeedStream(string userId)
        {

            return  _appDbContext.FeedUserSubscriptions
                        .Where(u => u.UserSubscriberId == userId)
                        .Select(u => u.Feed)
                        .SelectMany(f => f.FeedItems)
                        .OrderByDescending(n => n.DatePublished);


        }



        public bool Subscribe(int feedId, string userId)
        {
            bool success = true;
            try
            {
                _appDbContext.FeedUserSubscriptions.Add(new FeedUserSubscription
                {
                    FeedId = feedId,
                    UserSubscriberId = userId
                });

                _appDbContext.SaveChanges();

            }
            catch (Exception ex)
            {
                _logger.LogError($"An attempt to subscribe User: '{userId}' to Feed: {feedId} threw an error: {ex}");
                success = false;
            }
            return success;


        }

        public bool Unubscribe(int feedId, string userId)
        {

            bool success = true;
            try
            {
                var subscription = _appDbContext.FeedUserSubscriptions
                                                .Where(item => item.UserSubscriberId == userId && item.FeedId == feedId)
                                                .FirstOrDefault();

                _appDbContext.FeedUserSubscriptions.Remove(subscription);

                _appDbContext.SaveChanges();



            }
            catch (Exception ex)
            {
                _logger.LogError($"An attempt to unsubscribe User: '{userId}' from Feed: {feedId} threw an error: {ex}");
                success = false;
            }
            return success;




        }
    }
}

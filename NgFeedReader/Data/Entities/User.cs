using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace ModusCreateSampleApp.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        /// <summary>
        /// nav prop to get all the subscriptions of a given user
        /// </summary>
        public virtual ICollection<FeedUserSubscription> FeedSubscriptions { get; set; }
    }
}

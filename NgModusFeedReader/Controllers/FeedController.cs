using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ModusCreateSampleApp.Data;
using ModusCreateSampleApp.Data.Entities;
using NgModusFeedReader.Data.Services;
using NgModusFeedReader.Data.ViewModels;



namespace NgModusFeedReader.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    public class FeedController : ControllerBase
    {
        private readonly ILogger<FeedController> _logger;
        private readonly IConfiguration _config;
        private readonly IAppDbRepository _repo;
        private readonly IFeedManagementService _feedManagementService;
        private readonly UserManager<User> _userManager;

        public FeedController(ILogger<FeedController> logger,
            IAppDbRepository repo,
            IFeedManagementService feedManagementService,
            UserManager<User> userManager)
        {
            _logger = logger;
            _repo = repo;
            _feedManagementService = feedManagementService;
            _userManager = userManager;
        }
                            
        

        

        [HttpGet("[action]")]
        public async Task<IActionResult> Feeds(int? feedId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            try
            {
                return Ok(_repo.GetFeeds(feedId, user.Id));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Feeds threw an error: {ex}");
                return BadRequest();
            }
        }



        //PS: STUPID MICROSOFT https://github.com/dotnet/aspnetcore/issues/17811
        [HttpPost("[action]")]
        public async Task<IActionResult> Subscribe([FromBody] int feedId)
        {
            
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = _feedManagementService.Subscribe(feedId, user.Id);

            if (!result)
                return BadRequest();
            else
                return Ok();
        }


        //Note: I know this should be HttpDelete but I think there's breaking changed in AspnetCore 3 that
        //seemingly dont allow this to work. So, I'm cheating, to not waste more time
        [HttpPost("[action]")]
        public async Task<IActionResult> Unsubscribe([FromBody] int feedId)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var result = _feedManagementService.Unubscribe(feedId, user.Id);
            if (!result)
                return BadRequest();
            else
                return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetFeedStream()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var results = _feedManagementService.GetSubscriberFeedStream(user.Id);
            if (results != null && results.Count() > 0)
                return Ok(results);
            else
                return NoContent();
        }

        [HttpGet("[action]")]
        public IActionResult Search ([FromQuery] string searchString)
        {
            var results = _repo.SearchFeedItems(searchString);
            if (results != null && results.Count() > 0)
                return Ok(results);
            else
                return NoContent();
        }

        [HttpGet("[action]")]
        public IActionResult FeedItems()
        {
            var results = _repo.GetAllFeedItems();
            if (results != null && results.Count() > 0)
                return Ok(results);
            else
                return NoContent();
        }






    }
}

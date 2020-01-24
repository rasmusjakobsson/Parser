using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Parser.Contracts;
using Parser.Models;

namespace RssFeed.Api.Controllers
{
    [ApiController]
    [Route("api")]
    public class RssFeedController : ControllerBase
    {
        private readonly IRssFeedService _rssFeedService;

        public RssFeedController(IRssFeedService rssFeedService)
        {
            _rssFeedService = rssFeedService;
        }
        [HttpGet]
        public ActionResult<List<ParsedEpisodeInfo>> Get(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest();
            }

            try
            {
                return Ok(_rssFeedService.GetRssFeed(url));
            }
            catch(InvalidOperationException e)
            {
                return BadRequest("Invalid element during parsing");
            }
        }
    }
}

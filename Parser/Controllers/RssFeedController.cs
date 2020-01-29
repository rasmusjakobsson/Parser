using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Parser.Api.Models;
using Parser.Contracts;
using Parser.Models;

namespace Parser.Api.Controllers
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
        public async Task<ActionResult<List<ParsedEpisodeInfo>>> Get(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return BadRequest();
            }

            try
            {
                return Ok(await _rssFeedService.ParseRssFeedAsync(url));
            }
            catch(Exception e)
            {
                return BadRequest(new BadRequestResponse("Failed to parse RSS feed") );
            }
        }
    }
}

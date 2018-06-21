using BuddyCloudCoreApi2.Services.Interfaces;
using BuddyCloudCoreApi2.Helper;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuddyCloudCoreApi2.Controllers
{
    [Authorize]
    [EnableCors("AllowSpecificOrigin")]
    [Produces("application/json")]
    [Route("api/v1/Statistic/{sellerid}")]
    [ApiController]
    public class StatisticController : ControllerBase
    {
        private IStatisticService _statisticSvc;

        public StatisticController(IStatisticService statisticSvc)
        {
            _statisticSvc = statisticSvc;
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetStatistics(string sellerid, [FromQuery] string stockid, [FromQuery] int month, [FromQuery] int year)
        {
            var stockId = GuidParserHelper.StringToGuidParser(stockid);
            var sellerId = GuidParserHelper.StringToGuidParser(sellerid);

            var statistic = await _statisticSvc.GetSalesStatisticsAsync(sellerId, stockId, month, year);

            return Ok(statistic != null ? statistic : null);
        }
    }
}
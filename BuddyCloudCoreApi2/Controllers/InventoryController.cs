using BuddyCloudCoreApi2.Core.Models;
using BuddyCloudCoreApi2.Services.Interfaces;
using BuddyCloudCoreApi2.Helper;

using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace BuddyCloudCoreApi2.Controllers
{
    [Authorize]
    [EnableCors("AllowSpecificOrigin")]
    [Produces("application/json")]
    [Route("api/Inventory/{sellerid}")]
    public class InventoryController : Controller
    {
        private IInventoryService _inventorySvc;

        public InventoryController(IInventoryService inventorySvc)
        {
            _inventorySvc = inventorySvc;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> PostStock([FromBody] Stock model)
        {
            if (model != null)
            {
                var isStockAdded = await _inventorySvc.AddStockAsync(model);

                if (isStockAdded)
                    return Ok();
                else
                    return BadRequest();
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("stocks")]
        public async Task<IActionResult> GetStocks(string sellerid)
        {
            var id = GuidParserHelper.StringToGuidParser(sellerid);

            var stocks = await _inventorySvc.GetStocksPerSellerAsync(id);

            return Ok(stocks);
        }

        [HttpGet]
        [Route("item")]
        public async Task<IActionResult> GetStock(string sellerid, [FromQuery] string itemid)
        {
            var id = GuidParserHelper.StringToGuidParser(sellerid);
            var stkId = GuidParserHelper.StringToGuidParser(itemid);

            var stockInfo = await _inventorySvc.GetStockInfo(id, stkId);

            return Ok(stockInfo);
        }

        [HttpPut]
        [Route("update")]
        public async Task<IActionResult> PutStockInfo([FromBody] Stock model)
        {
            if (ModelState.IsValid)
            {
                var isUpdateSuccessful = await _inventorySvc.UpdateStockAsync(model);

                if (isUpdateSuccessful)
                    return Ok();
                else
                    return BadRequest(StockUpdateErrorStatusCodesEnum.UpdateFailed);
            }

            return BadRequest(StockUpdateErrorStatusCodesEnum.ErrorProcessingRequest);
        }

        [HttpGet]
        [Route("stocks/available")]
        public async Task<IActionResult> GetAvailableStocks(string sellerid)
        {
            var id = GuidParserHelper.StringToGuidParser(sellerid);

            var stocks = await _inventorySvc.GetAvailableStocksPerSellerAsync(id);

            return Ok(stocks);
        }

        //[HttpPut]
        //[Route("replenish/{stockid:guid}/{qty:int}")]
        //public async Task<IActionResult> PutStockQty(Guid sellerid, Guid stockid, int qty)
        //{
        //    var sellerId = Guid.Parse(sellerid.ToString());
        //    var stockId = Guid.Parse(stockid.ToString());

        //    var isStockReplenished = await _inventorySvc.ReplenishStockQtyAsync(sellerId, stockId, qty);

        //    if (isStockReplenished)
        //        return Ok();
        //    else
        //        return BadRequest();
        //}
    }
}
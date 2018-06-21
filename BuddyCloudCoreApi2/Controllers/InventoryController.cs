using BuddyCloudCoreApi2.Core.Models;
using BuddyCloudCoreApi2.Core.Response;
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
    [Route("api/v1/Inventory/{sellerid}")]
    [ApiController]
    public class InventoryController : ControllerBase
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
                    return Ok(new ResponseModel { Status = true, Message = "Successfully added stock." });
                else
                    return BadRequest(new ResponseModel { Status = false, Message = "Failed to add stock." });
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
                    return Ok(new ResponseModel { Status = true, Message = "Successfully Updated Stock." });
                else
                    return BadRequest(new ResponseModel { Status = false, Message = "Unable to update Stock." });
            }

            return BadRequest(new ResponseModel { Status = false, Message = "Something went wrong." });
        }

        [HttpGet]
        [Route("stocks/available")]
        public async Task<IActionResult> GetAvailableStocks(string sellerid)
        {
            var id = GuidParserHelper.StringToGuidParser(sellerid);

            var stocks = await _inventorySvc.GetAvailableStocksPerSellerAsync(id);

            return Ok(stocks);
        }

        [HttpGet]
        [Route("stocks/sold")]
        public IActionResult GetTotalQtySold(string sellerid, [FromQuery] string stockid)
        {
            var sellerId = Guid.Parse(sellerid);
            var stkId = Guid.Parse(stockid);

            int totalQtySold = _inventorySvc.GetTotalQtySold(sellerId, stkId);

            return Ok(totalQtySold);
        }

        [HttpGet]
        [Route("stocks/sales")]
        public IActionResult GetTotalStockSales(string sellerid, [FromQuery] string stockid)
        {
            var sellerId = Guid.Parse(sellerid);
            var stkId = Guid.Parse(stockid);

            decimal totalQtySold = _inventorySvc.GetTotalStockSales(sellerId, stkId);

            return Ok(totalQtySold);
        }

        [HttpPut]
        [Route("stock/{stockid:guid}/price/{price:int}/percent/{percent:int}")]
        public async Task<IActionResult> PutStockSalePrice(string sellerid, Guid stockid, int price, int percent)
        {
            var sellerId = GuidParserHelper.StringToGuidParser(sellerid);
            var stockId = Guid.Parse(stockid.ToString());

            var isSet = await _inventorySvc.SetStockSalePriceAsync(sellerId, stockId, price, percent);

            if (isSet)
                return Ok(new ResponseModel { Status = true, Message = "Sale price for stock id " + stockId + "set." });
            else
                return BadRequest(new ResponseModel { Status = false, Message = "" });
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
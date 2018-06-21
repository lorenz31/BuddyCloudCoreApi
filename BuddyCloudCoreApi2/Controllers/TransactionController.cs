using BuddyCloudCoreApi2.Core.Models;
using BuddyCloudCoreApi2.DAL.DTO;
using BuddyCloudCoreApi2.Core.Response;
using BuddyCloudCoreApi2.Services.Interfaces;
using BuddyCloudCoreApi2.Helper;

using System;
using System.Collections.Generic;
using System.Linq;
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
    [Route("api/v1/Transaction/{sellerid}")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private ITransactionService _transactionSvc;

        public TransactionController(ITransactionService transactionSvc)
        {
            _transactionSvc = transactionSvc;
        }

        [HttpPost]
        [Route("add")]
        public async Task<IActionResult> PostAddTransaction([FromBody] Transactions transactions)
        {
            var isTransactionAdded = await _transactionSvc.AddTransactionAsync(transactions);

            if (isTransactionAdded)
                return Ok(new ResponseModel { Status = true, Message = "Transaction Successful." });
            else
                return BadRequest(new ResponseModel { Status = false, Message = "Something went wrong. Please contact System Administrator." });
        }

        [HttpGet]
        [Route("history")]
        public async Task<IActionResult> GetTransactionHistory(string sellerid)
        {
            var id = GuidParserHelper.StringToGuidParser(sellerid);

            var transHist = await _transactionSvc.GetTransactionHistoryAsync(id);

            return Ok(transHist);
        }

        [HttpGet]
        [Route("pending")]
        public async Task<IActionResult> GetPendingTransactionsAsync(string sellerid)
        {
            var id = GuidParserHelper.StringToGuidParser(sellerid);

            var pendingTrans = await _transactionSvc.GetPendingTransactionsAsync(id);

            return Ok(pendingTrans);
        }

        [HttpGet]
        [Route("order")]
        public async Task<IActionResult> GetTransactionDetailsAsync([FromQuery] int id)
        {
            var order = await _transactionSvc.GetTransactionDetailsAsync(id);

            return Ok(order);
        }

        [HttpPut]
        [Route("update")]
        public IActionResult PutOrderStatus([FromQuery] string sellerid, [FromQuery] int transactionid)
        {
            var userid = GuidParserHelper.StringToGuidParser(sellerid);

            var status = _transactionSvc.UpdateOrderStatus(userid, transactionid);

            if (status)
                return Ok(new ResponseModel { Status = true, Message = "Order has been delivered." });
            else
                return BadRequest(new ResponseModel { Status = false, Message = "Failed to process order." });
        }
    }
}
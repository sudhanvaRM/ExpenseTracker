using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Server.Models.Entities;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SplitController: ControllerBase
    {
        private readonly SplitService _splitServiceObj;
        private readonly JwtToken _jwt;
        public SplitController(SplitService splitServiceObj,JwtToken jwt)
        {
            _splitServiceObj = splitServiceObj;  
            _jwt = jwt;
        }

        [HttpPost]
        [Route("add-split")]
        [Authorize]
        public async Task<IActionResult> AddSplit(AddSplitDTO split)
        {
            try
            {
               if(await _splitServiceObj.ValidateTransactionID(split))
               {
                   var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
                   int userId = _jwt.FetchUserIdfromJWT(jwtToken);
                   if(await _splitServiceObj.CreateSplit(split,userId))
                   {
                      return Ok(new {Message = "Split Added Successfully"});
                   }

                   else{
                     return Conflict( new {Message = "Error while Creating a split"});
                   }
                   
               }

               else{
                   return Conflict( new {Message = "Invalid TransactionID"});
               }
            }

            catch(Exception ex)
            {
                 return StatusCode(500, new { message = "An error occurred while creating the Split.", error = ex.Message });
            }
            
        }

        [HttpPost]
        [Route("pay-split")]
        [Authorize]
        public async Task<IActionResult> PendingPayments(PaySplitDTO paySplit)
        {
             var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
             int userId = _jwt.FetchUserIdfromJWT(jwtToken);
             //First Validate Split ID

             if(await _splitServiceObj.ValidateSplitID(paySplit.splitID,userId))
             {
                if(await _splitServiceObj.PaySplit(paySplit.splitID, userId))
                   return Ok(new {Message = "Your payment is Successful"});

                else
                   return StatusCode(500, "An unexpected error occurred while paying the split");
             }
            
             else
             {
                return Conflict(new {Message = "You are attempting to pay for a split that you are not a participant of"});
             }
               
        }

        [HttpGet]
        [Route("fetch-pending-splits")]
        [Authorize]
        public async Task<IActionResult> FetchPendingSplits()
        {
             var jwtToken = Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);
             int userId = _jwt.FetchUserIdfromJWT(jwtToken);


             List<PendingSplitDTO> unpaidPayments = await _splitServiceObj.FetchPendingSplits(userId);
             return Ok(unpaidPayments);
        }
    }
}
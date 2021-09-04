using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using RPMCRUDAPIs.Models;

namespace RPMCRUDAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DynamoDbAPIsController : ControllerBase
    {
        private readonly IGetDataInterface _getData;
        private readonly IPutItem _putItem;

        public DynamoDbAPIsController(IGetDataInterface getData, IPutItem putItem)
        {
            _getData = getData;
            _putItem = putItem;
        }


        /// <summary>Put the db item based on jsonData.
        /// <param name="jsonData">Item List to put in table</param>
        /// <param name="tableName">Used to specify the table of the db to put the item.</param>
        /// </summary>
        //[Authorize]
        [Route("putitem")]
        [HttpPost]
        public async Task<IActionResult> PutItem([FromQuery] string jsonData, string tableName, string actionType)
        {
            try
            {
                if (jsonData != null && tableName != null && actionType != null)
                {

                        await _putItem.PutItemByJson(jsonData, tableName, actionType);
                        if (actionType.ToLower() == "update")
                        {
                            return Ok("Update Sucessfully");
                        }
                        else
                        {
                            return Ok("Registered");
                        }
                   
                }
                else
                {
                    return Ok("Failed");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }

        /// <summary>Get the db item based on Query Request.
        /// <param name="queryRquest">Used for the Query Request to db.</param>
        /// <param name="requestFor">Used to specify request to maintain the session.</param>
        /// <param name="userId">Used to authorization.</param>
        /// </summary>
        [Authorize]
        [Route("getitem")]
        [HttpPost]
        public async Task<IActionResult> GetItem([FromBody] QueryRequest queryRquest)
        {
            try
            {
                if (queryRquest != null )
                {

                    var response = await _getData.GetItemByQueryRequest(queryRquest);
                    return Ok(response);


                }
                else
                {
                    return Ok("Failed");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        //[Authorize]
        //[Route("getallpageitem")]
        //[HttpPost]
        //public IActionResult GetAllPageItem([FromBody] QueryRequest queryRquest)
        //{
        //    try
        //    {
        //        if (queryRquest != null)
        //        {
        //            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
        //            Dictionary<string, AttributeValue> lastKeyEvaluated = null;
        //            do
        //            {
        //                var response = client.QueryAsync(queryRquest);

        //                //Console.WriteLine("No. of reads used (by query in FindRepliesForAThreadSpecifyLimit) {0}\n",
        //                          //response.ConsumedCapacity.CapacityUnits);
        //                foreach (Dictionary<string, AttributeValue> item
        //                     in response.Result.Items)
        //                {
        //                    //PrintItem(item);
        //                }
        //                lastKeyEvaluated = response.Result.LastEvaluatedKey;
        //                return Ok(response);
        //            } while (lastKeyEvaluated != null && lastKeyEvaluated.Count != 0);


        //        }
        //        else
        //        {
        //            return Ok("Failed");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Ok(ex.Message);
        //    }

        //}

        /// <summary>Get the all item of Table based on Ssan Request .
        /// <param name="scanRquest">Used for the Scan Request to db.</param>
        /// <param name="requestFor">Used to specify request to maintain the session.</param>
        ///  <param name="userId">Used to authorization.</param>
        /// </summary>
        [Authorize]
        [Route("getallitem")]
        [HttpPost]
        public async Task<IActionResult> ScanItem([FromBody] ScanRequest scanRquest)
        {
            try
            {
                if (scanRquest != null )
                {


                        var response = await _getData.GetItemByScanRequest(scanRquest);
                        return Ok(response);
                   
                }
                else
                {
                    return Ok("Failed");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }

        }


        /// <summary>update the db item based on update Request.
        /// <param name="updateItemRquest">Used for the item details to be update.</param>
        /// </summary>
        [Authorize]
        [Route("updateitem")]
        [HttpPost]
        public async Task<IActionResult> UpdateItem([FromBody] UpdateItemRequest updateItemRquest)
        {
            try
            {
                if (updateItemRquest != null)
                {


                    await _putItem.UpdateItem(updateItemRquest);
                    return Ok("Updated");


                }
                else
                {
                    return Ok("Failed");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>delete the db item based on update Request.
        /// <param name="deleteItemRquest">Item details which is going to delete.</param>
        /// </summary>
        [Authorize]
        [Route("deleteitem")]
        [HttpPost]
        public async Task<IActionResult> DeleteItem([FromBody] DeleteItemRequest deleteItemRquest)
        {
            try
            {
                if (deleteItemRquest != null)
                {

                    await _putItem.DeleteItem(deleteItemRquest);
                    return Ok("Deleted");

                }
                else
                {
                    return Ok("Failed");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        /// <summary>delete the db item based on update Request.
        /// <param name="putItemRquest">Item details which is going to put.</param>
        /// </summary>
        [Authorize]
        [Route("putitems")]
        [HttpPost]
        public async Task<IActionResult> PutItems([FromBody] PutItemRequest putItemRquest)
        {
            try
            {
                if (putItemRquest != null)
                {

                    await _putItem.PutItems(putItemRquest);
                    return Ok("Registered");

                }
                else
                {
                    return Ok("Failed");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }

        [Route("updatewebhookitem")]
        [HttpPost]
        public async Task<IActionResult> UpdateWebhookItem([FromBody] UpdateItemRequest updateItemRquest)
        {
            try
            {
                if (updateItemRquest != null)
                {


                    await _putItem.UpdateItem(updateItemRquest);
                    return Ok("Updated");


                }
                else
                {
                    return Ok("Failed");
                }
            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
        }
    }
}
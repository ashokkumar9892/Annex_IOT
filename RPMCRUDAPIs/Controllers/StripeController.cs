using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RPMCRUDAPIs.Models;
using Stripe;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RPMCRUDAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : Controller
    {
        private readonly IPutItem _putItem;

        public StripeController(IGetDataInterface getData, IPutItem putItem)
        {
            _putItem = putItem;
        }
        /// <summary>
        /// Stripe webhook handling
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> StripeWebhookAsync()
        {
            try
            {

                if (Request.Headers["Stripe-Signature"].Count() > 0)
                {
                    string jsonOri = await new StreamReader(Request.Body).ReadToEndAsync();
                    string json = jsonOri.Trim(new char[] { ' ', '\n', '\r' });
                    string header = Request.Headers["Stripe-Signature"];
                    string signature = header.Trim(new char[] { ' ', '\n', '\r' });

                    // validate webhook called by stripe only
                    Event stripeEvent = EventUtility.ConstructEvent(json, signature, "whsec_dkoy8KbuF6fQlfpdEjiFhTt57C3P5NKa", 300, false);

                    switch (stripeEvent.Type)
                    {
                        case "customer.created":
                            var customer = stripeEvent.Data.Object as Customer;
                            // do work

                            break;

                        case "customer.subscription.created":
                        case "customer.subscription.updated":
                        case "customer.subscription.deleted":
                        case "customer.subscription.trial_will_end":
                            var subscription = stripeEvent.Data.Object as Subscription;
                            // do work

                            break;

                        case "invoice.created":
                            var newinvoice = stripeEvent.Data.Object as Invoice;
                            // do work

                            break;

                        case "invoice.upcoming":
                        case "invoice.payment_succeeded":
                        case "invoice.payment_failed":
                            var invoice = stripeEvent.Data.Object as Invoice;
                            // do work

                            break;

                        case "coupon.created":
                        case "coupon.updated":
                        case "coupon.deleted":
                            var coupon = stripeEvent.Data.Object as Coupon;
                            // do work

                            break;

                        case "checkout.session.completed":
                            var paymentobj = stripeEvent.Data.Object as Stripe.Checkout.Session;
                            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
                            try
                            {
                                var request = new UpdateItemRequest
                                {
                                    TableName = "UserDetail",
                                    Key = new Dictionary<string, AttributeValue>()
                                    {
                                        { "PK", new AttributeValue { S = "patient" } },
                                        { "SK", new AttributeValue { S = paymentobj.ClientReferenceId } }
                                    },
                                    
                                    ExpressionAttributeValues = new Dictionary<string, AttributeValue>()
                                    {
                                        {":v_AddressLine1",new AttributeValue {S = paymentobj.Shipping.Address.Line1}},
                                        {":v_AddressLine2",new AttributeValue {S = paymentobj.Shipping.Address.Line2}},
                                        {":v_City",new AttributeValue {S = paymentobj.Shipping.Address.City}},
                                        {":v_State",new AttributeValue {S = paymentobj.Shipping.Address.State}},
                                        {":v_Country",new AttributeValue {S = paymentobj.Shipping.Address.Country}},
                                        {":v_Zipcode",new AttributeValue {S = paymentobj.Shipping.Address.PostalCode}}

                                    },
                                    UpdateExpression = "SET AddressLine1=:v_AddressLine1 , AddressLine2=:v_AddressLine2 , U_City = :v_City , U_State= :v_State , U_Zipcode=:v_Zipcode , U_Country=:v_Country",

                                };
                                var response = _putItem.UpdateItem(request);
                            }

                            catch (Exception ex)
                            {
                                return Ok(ex.Message);
                            }

                            break;

                            // DO SAME FOR OTHER EVENTS
                    }
                }
                else { return Ok("Header not found"); }

                return Ok("success");
            }
            catch (StripeException ex)
            {
                //_logger.LogError(ex, $"StripWebhook: {ex.Message}");
                return BadRequest();
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, $"StripWebhook: {ex.Message}");
                return BadRequest();
            }
        }


        public class StripeWebHook : Controller
        {            
            // If you are testing your webhook locally with the Stripe CLI you
            // can find the endpoint's secret by running `stripe listen`
            // Otherwise, find your endpoint's secret in your webhook settings
            // in the Developer Dashboard
            const string endpointSecret = "whsec_...";

            [HttpPost]
            public async Task<IActionResult> Index()
            {
                var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

                try
                {
                    var stripeEvent = EventUtility.ConstructEvent(json,
                        Request.Headers["Stripe-Signature"], endpointSecret);

                    // Handle the event
                    if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                    {
                        var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
                        Console.WriteLine("PaymentIntent was successful!");
                    }
                    else if (stripeEvent.Type == Events.PaymentMethodAttached)
                    {
                        var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                        Console.WriteLine("PaymentMethod was attached to a Customer!");
                    }
                    // ... handle other event types
                    else
                    {
                        Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    }

                    return Ok();
                }
                catch (StripeException)
                {
                    return BadRequest();
                }
            }
        }
    }
}

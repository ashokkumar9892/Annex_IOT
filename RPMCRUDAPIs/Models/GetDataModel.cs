using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RPMCRUDAPIs.Models
{
    public class GetDataModel : IGetDataInterface
    {
        private readonly IAmazonDynamoDB _client;

        public GetDataModel(IAmazonDynamoDB client)
        {
            _client = client;
        }

        public async Task<DyanmoTableItems> GetUserDataAsync(string id)
        {
            var queryRequest = RequestBuilder(id);

            var result = await ScanAsync(queryRequest);

            return new DyanmoTableItems
            {
                Items = result.Items.Select(Map).ToList()
            };

        }

        private ScanRequest RequestBuilder(string id)
        {
            //if (id !=null)
            //{
            //    return new ScanRequest
            //    {
            //        TableName = "TR_UserRegistration",
            //    };
            //}

            return new ScanRequest
            {
                TableName = "TR_UserRegistration",
                ExpressionAttributeValues = new Dictionary<string, AttributeValue> {
                {
                    ":v_Id", new AttributeValue { N = id.ToString() }}
                },
                FilterExpression = "UserId = :v_Id",
                ProjectionExpression = "UserId"
            };
        }       

        private async Task<ScanResponse> ScanAsync(ScanRequest request)
        {
            var response = await _client.ScanAsync(request);

            return response;
        }

        private Item Map(Dictionary<string, AttributeValue> result)
        {
            return new Item
            {
                Id = result["UserId"].N//,
               // ReplyDatetime = result["ReplyDateTime"].N
            };
        }

        //public async Task<List<Dictionary<string, AttributeValue>>> GetItemByQueryRequest(QueryRequest queryRequest)
        //{
        //    var request = queryRequest;
        //    var response = await _client.QueryAsync(request);
        //    return response.Items;
        //}

        public async Task<List<Dictionary<string, AttributeValue>>> GetItemByQueryRequest(QueryRequest queryRequest)
        {
            Dictionary<string, AttributeValue> lastKeyEvaluated = null;
            AmazonDynamoDBClient client = new AmazonDynamoDBClient();
            List<Dictionary<string, AttributeValue>> responseList = new List<Dictionary<string, AttributeValue>>();
            do
            {
                var request = queryRequest;
                request.Limit = 5;
                request.ExclusiveStartKey = lastKeyEvaluated;

                var response = client.QueryAsync(request);
                
                foreach (Dictionary<string, AttributeValue> item
                     in response.Result.Items)
                {
                    responseList.Add(item);

                }
                lastKeyEvaluated = response.Result.LastEvaluatedKey;
                
            } while (lastKeyEvaluated != null && lastKeyEvaluated.Count != 0);
            return responseList;

        }

        public async Task<List<Dictionary<string, AttributeValue>>> GetItemByScanRequest(ScanRequest scanRequest)
        {
            var request = scanRequest;
            var response = await _client.ScanAsync(request);
            return response.Items;
        }
    }
}

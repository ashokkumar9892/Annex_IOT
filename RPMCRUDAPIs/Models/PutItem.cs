using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DocumentModel;
using Amazon.DynamoDBv2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPMCRUDAPIs.Models
{
    public class PutItem : IPutItem
    {
        private readonly IAmazonDynamoDB _dynamoClient;
        public PutItem(IAmazonDynamoDB dynamoClient)
        {
            _dynamoClient = dynamoClient;
        }
        public async Task AddNewEntry(int id, string replyDateTime,string tableName)
        {
            var queryRequest = RequestBuilder(id, replyDateTime, tableName);

            await PutItemAsync(queryRequest);
        }
        private PutItemRequest RequestBuilder(int id, string replyDateTime, string tableName)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                {"Id", new AttributeValue {N = id.ToString()}},
                {"ReplyDateTime", new AttributeValue {N = replyDateTime}}
            };

            return new PutItemRequest
            {
                TableName = tableName,
                Item = item
            };
        }
        private async Task PutItemAsync(PutItemRequest request)
        {
            await _dynamoClient.PutItemAsync(request);
        }

        public async Task PutItemByJson(string json, string tableName,string actionType)
        {
            var table = Table.LoadTable(_dynamoClient, tableName);
            var item = Document.FromJson(json);
            if (actionType.ToLower() == "update") {await table.UpdateItemAsync(item);}
            else { await table.PutItemAsync(item); }
        }


        public async Task UpdateItem(UpdateItemRequest updateItemRequest)
        {
            //var table = Table.LoadTable(_dynamoClient, tableName);
            //var item = Document.FromJson(json);
         var response =  await _dynamoClient.UpdateItemAsync(updateItemRequest); 
          
        }

        public async Task DeleteItem(DeleteItemRequest deleteItemRequest)
        {
            var response = await _dynamoClient.DeleteItemAsync(deleteItemRequest);
        }

        public async Task PutItems(PutItemRequest putItemRequest)
        {
            var response = await _dynamoClient.PutItemAsync(putItemRequest);
        }
    }
} 

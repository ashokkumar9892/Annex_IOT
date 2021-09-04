using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPMCRUDAPIs.Models
{
    public interface IPutItem
    {
        Task PutItemByJson(string json, string tableName, string actionType);
        Task AddNewEntry(int id, string replyDateTime, string tableName);

        Task UpdateItem(UpdateItemRequest updateItemRequest);
        Task DeleteItem(DeleteItemRequest deleteItemRequest);

        Task PutItems(PutItemRequest putItemRequest);
    }
}


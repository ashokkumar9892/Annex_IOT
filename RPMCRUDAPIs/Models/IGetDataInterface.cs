using Amazon.DynamoDBv2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPMCRUDAPIs.Models
{
    public interface IGetDataInterface
    {
        Task<DyanmoTableItems> GetUserDataAsync(string id);

        Task<List<Dictionary<string, AttributeValue>>> GetItemByQueryRequest(QueryRequest queryRequest);

        Task<List<Dictionary<string, AttributeValue>>> GetItemByScanRequest(ScanRequest scanRequest);
    }
}

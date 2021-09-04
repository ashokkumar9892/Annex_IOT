using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RPMCRUDAPIs.DBConfig
{
    public class Ddb_Client
    {
        public static bool createClient(bool useDynamoDBLocal)
        {
            if (useDynamoDBLocal)
            {
                operationSucceeded = false;
                operationFailed = false;

                // First, check to see whether anyone is listening on the DynamoDB local port
                // (by default, this is port 8000, so if you are using a different port, modify this accordingly)
                bool localFound = false;
                try
                {
                    using (var tcp_client = new TcpClient())
                    {
                        var result = tcp_client.BeginConnect("localhost", 8000, null, null);
                        localFound = result.AsyncWaitHandle.WaitOne(3000); // Wait 3 seconds
                        tcp_client.EndConnect(result);
                    }
                }
                catch
                {
                    localFound = false;
                }
                if (!localFound)
                {
                    Console.WriteLine("\n      ERROR: DynamoDB Local does not appear to have been started..." +
                                      "\n        (checked port 8000)");
                    operationFailed = true;
                    return (false);
                }

                // If DynamoDB-Local does seem to be running, so create a client
                Console.WriteLine("  -- Setting up a DynamoDB-Local client (DynamoDB Local seems to be running)");
                AmazonDynamoDBConfig ddbConfig = new AmazonDynamoDBConfig();
                ddbConfig.ServiceURL = "http://localhost:8000";
                try { client = new AmazonDynamoDBClient(ddbConfig); }
                catch (Exception ex)
                {
                    Console.WriteLine("     FAILED to create a DynamoDBLocal client; " + ex.Message);
                    operationFailed = true;
                    return false;
                }
            }

            else
            {
                try { client = new AmazonDynamoDBClient(); }
                catch (Exception ex)
                {
                    Console.WriteLine("     FAILED to create a DynamoDB client; " + ex.Message);
                    operationFailed = true;
                }
            }
            operationSucceeded = true;
            return true;
        }
    }
}


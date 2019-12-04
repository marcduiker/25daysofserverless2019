using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessAdvent.Day04
{
    public class ListDishesHttp
    {
        [StorageAccount("AzureWebJobsStorage")]
        [FunctionName(nameof(ListDishesHttp))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequestMessage message,
            [Table("potluck")] CloudTable cloudTable,
            ILogger log)
        {
            var query = new TableQuery<Food>()
                .Where(
                    TableQuery.GenerateFilterCondition(
                        "PartitionKey",
                        QueryComparisons.Equal,
                        Food.PartitionKeyValue));
            var result = await cloudTable.ExecuteQuerySegmentedAsync(query, null);

            return new OkObjectResult(result);
        }
    }
}

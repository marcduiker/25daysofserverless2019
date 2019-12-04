using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using Microsoft.WindowsAzure.Storage.Table;
using System.Net;

namespace ServerlessAdvent.Day04
{
    public class RemoveDishHttp
    {
        [StorageAccount("AzureWebJobsStorage")]
        [FunctionName(nameof(RemoveDishHttp))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = null)] HttpRequestMessage message,
            [Table("potluck")] CloudTable cloudTable,
            ILogger log)
        {
            IActionResult result = null;
            try
            {
                var food = await message.Content.ReadAsAsync<Food>();
                var retrieveOperation = TableOperation.Retrieve(Food.PartitionKeyValue, food.PreparedBy);
                var retrieveResult = await cloudTable.ExecuteAsync(retrieveOperation);
                if (retrieveResult.HttpStatusCode == (int)HttpStatusCode.OK)
                {
                    var entityToRemove = new TableEntity(Food.PartitionKeyValue, food.PreparedBy) { ETag = retrieveResult.Etag};
                    var deleteOperation = TableOperation.Delete(entityToRemove);
                    await cloudTable.ExecuteAsync(deleteOperation);
                    result = new OkObjectResult($"Removed {food.Dish} by {food.PreparedBy}.");
                }
                else
                {
                    result = new NotFoundObjectResult($"No items found for {food.PreparedBy}.");
                }
            }
            catch (System.Exception e)
            {
                result = new BadRequestObjectResult(e);
            }

            return result;
        }
    }
}

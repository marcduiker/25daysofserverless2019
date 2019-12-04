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
    public class UpdateDishHttp
    {
        [StorageAccount("AzureWebJobsStorage")]
        [FunctionName(nameof(UpdateDishHttp))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequestMessage message,
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
                    food.SetKeys();
                    food.ETag = retrieveResult.Etag;
                    var deleteOperation = TableOperation.Replace(food);
                    await cloudTable.ExecuteAsync(deleteOperation);
                    result = new OkObjectResult($"Updated {food.Dish} by {food.PreparedBy}.");
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

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;

namespace ServerlessAdvent.Day04
{
    public class AddDishHttp
    {
        [StorageAccount("AzureWebJobsStorage")]
        [FunctionName(nameof(AddDishHttp))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequestMessage message,
            [Table("potluck")] IAsyncCollector<Food> collector,
            ILogger log)
        {
            IActionResult result = null;
            try
            {
                var food = await message.Content.ReadAsAsync<Food>();
                food.SetKeys();
                await collector.AddAsync(food);
                result = new OkObjectResult($"Added {food.Dish} by {food.PreparedBy}");
            }
            catch (System.Exception e)
            {
                result = new BadRequestObjectResult(e);
            }
            
            return result;
        }
    }
}

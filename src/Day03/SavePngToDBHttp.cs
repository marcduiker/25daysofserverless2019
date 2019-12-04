using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Globalization;

namespace ServerlessAdvent.Day03
{
    public class SavePngToDBHttp
    {
        [StorageAccount("AzureWebJobsStorage")]
        [FunctionName(nameof(SavePngToDBHttp))]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequestMessage message,
            [Table("pngfiles")] IAsyncCollector<Record> collector,
            ILogger log)
        {
            var push = await message.Content.ReadAsAsync<Push>();
            var repoUrl = push.Repo.Url;

            foreach (var commit in push.Commits)
            {
                foreach (var addedItem in commit.Added)
                {
                    if (addedItem.EndsWith(".png", true, CultureInfo.InvariantCulture))
                    {
                        var pngUrl = repoUrl + "/" + addedItem;
                        var record = new Record
                        {
                            PartitionKey = "Secret Santa",
                            RowKey = Guid.NewGuid().ToString("D"),
                            PngFile = pngUrl
                        };
                        await collector.AddAsync(record);
                    }
                }
            }

            return new AcceptedResult();
        }
    }
}

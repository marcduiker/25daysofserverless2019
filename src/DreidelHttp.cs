using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;

namespace ServerlessAdventCalendar
{
    public class DreidelHttp
    {
        private readonly string[] dreidelNames = new string [4]{ "Nun", "Gimmel", "Hay", "Shin" };
        private readonly Random randomInt = new Random();
        
        [FunctionName(nameof(DreidelHttp))]
        public IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            // lowerbound is inclusive, upperbound is exclusive
            var chosenDreidel = dreidelNames[randomInt.Next(0, 4)];

            return new OkObjectResult($"{chosenDreidel}");
        }
    }
}

using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace ServerlessAdvent.Day04
{
    public class Food : TableEntity
    {
        [JsonProperty("dish")]
        public string Dish { get; set; }

        [JsonProperty("preparedBy")]
        public string PreparedBy { get; set; }

        public void SetKeys()
        {
            PartitionKey = PartitionKeyValue;
            RowKey = PreparedBy;
        }

        public const string PartitionKeyValue = "potluck";
    }
}
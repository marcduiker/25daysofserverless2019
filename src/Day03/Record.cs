using Microsoft.WindowsAzure.Storage.Table;

namespace ServerlessAdvent.Day03
{
    public class Record : TableEntity
    {
        public string PngFile { get; set; }
    }
}
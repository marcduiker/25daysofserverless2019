using Newtonsoft.Json;

namespace ServerlessAdvent.Day03
{
    public class Push
    {
        [JsonProperty("repository")]
        public Repository Repo { get; set; }

        [JsonProperty("commits")]
        public Commit[] Commits { get; set; }
    }

    public class Repository
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }

    public class Commit
    {
        [JsonProperty("added")]
        public string[] Added { get; set; }
    }
}
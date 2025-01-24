using Newtonsoft.Json;

namespace Unified.Connectors.Model
{
    public class GraphResultList<T>
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public T[] value { get; set; }
    }
}

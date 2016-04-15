using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuisParser
{
    public class Value
    {
        public string entity { get; set; }
        public string type { get; set; }
        public double score { get; set; }
    }

    public class Parameter
    {
        public string name { get; set; }
        public bool required { get; set; }
        public List<Value> value { get; set; }
    }

    public class Action
    {
        public bool triggered { get; set; }
        public string name { get; set; }
        public List<Parameter> parameters { get; set; }
    }

    public class Intent
    {
        public string intent { get; set; }
        public object score { get; set; }
        public List<Action> actions { get; set; }
    }

    public class Resolution
    {
        public string metadataType { get; set; }
        public string resolution_type { get; set; }
        public string value { get; set; }
    }

    public class Entity
    {
        public string entity { get; set; }
        public string type { get; set; }
        public Resolution resolution { get; set; }
    }

    public class LuisResponse
    {
        public string query { get; set; }
        public List<Intent> intents { get; set; }
        public List<Entity> entities { get; set; }
    }


    [JsonObject]
    public class MapEntity
    {
        [JsonProperty("EntityID")]
        public string EntityID { get; set; }
        [JsonProperty("DisplayName")]
        public string DisplayName { get; set; }
        [JsonProperty("Latitude")]
        public double Latitude { get; set; }
        [JsonProperty("Longitude")]
        public double Longitude { get; set; }
        [JsonProperty("__Distance")]
        public double Distance { get; set; }
    }
}

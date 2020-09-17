using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text.Json.Serialization;

namespace CrudMaster.Utils
{
    public class FilterNode
    {
        [JsonPropertyName("groupOp")]
        public string GroupOp { get; set; }
        [JsonPropertyName("rules")]
        public List<Rule> Rules { get; set; }
        [JsonPropertyName("groups")]
        public List<FilterNode> Groups { get; set; }
    }

    public class Rule
    {
        [JsonPropertyName("field")]
        public string Field { get; set; }
        [JsonPropertyName("op")]
        public string Op { get; set; }
        [JsonPropertyName("data")]
        public string Data { get; set; }

    }
}

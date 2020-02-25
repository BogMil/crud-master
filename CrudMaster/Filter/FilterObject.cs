﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CrudMaster.Filter
{
    public class FilterObject
    {
        [JsonPropertyName("groupOp")]
        public string GroupOp { get; set; }
        [JsonPropertyName("rules")]
        public Rule[] Rules { get; set; }
        [JsonPropertyName("groups")]
        public FilterObject[] Groups { get; set; }
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
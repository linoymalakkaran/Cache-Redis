using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace svc_global_cache.Models
{
    public class CacheSaveModel
    {
        public string Context { get; set; }
        public string Key { get; set; }
        public string Data { get; set; }
        public int ExpiryInSeconds { get; set; }
    }

    public class CacheSetModel
    {
        public string context_s { get; set; }
        public string key_s { get; set; }
        public string text_s { get; set; }
        public int expiryInSeconds_i { get; set; }
    }
}
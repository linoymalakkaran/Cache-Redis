using svc_global_cache.Models;
using svc_global_cache.Utilities;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace svc_global_cache.Controllers
{
    [RoutePrefix("rs/cache")]
    public class CacheController : ApiController
    {
        [HttpGet]
        [Route("read_cache/{cache_key}")]
        public string ReadFromCache(string cache_key)
        {
            try
            {
                CacheHelper cache = new CacheHelper();
                string json = cache.ReadFromCache(cache_key);
                return json;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("read_cache_post")]
        public string ReadFromCachePost([FromBody]ReadCacheModel model)
        {
            try
            {
                CacheHelper cache = new CacheHelper();
                string json = cache.ReadFromCache(model.cache_key);
                if (string.IsNullOrEmpty(json))
                {
                    return string.Empty;
                }
                else
                {
                    return json.ToString();
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("save_cache")]
        public string SaveToCache([FromBody]CacheSaveModel model)
        {
            try
            {
                CacheHelper cache = new CacheHelper();
                string json = cache.SaveToCache(model);
                return json;
            }
            catch (Exception ex)
            {
                return "ERROR";
            }
        }

        [HttpGet]
        [Route("clear_cache/{cache_key}")]
        public HttpResponseMessage ClearCache(string cache_key)
        {
            try
            {
                CacheHelper cache = new CacheHelper();
                string json = cache.ClearCacheKey(cache_key);
                if (json == "OK")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Cache Cleared Successfully!!!");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, CommonResult.GetErrorResult("Failed to Clear Cache!!!!"));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, CommonResult.GetErrorResult(ex.Message));
            }
        }

        [HttpGet]
        [Route("clear_cacheallkeys")]
        public HttpResponseMessage ClearCacheAllKeys()
        {
            try
            {
                CacheHelper cache = new CacheHelper();
                string json = (string)cache.ClearCacheAllKeys();
                if (json == "OK")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Cache Cleared Successfully!!!");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, CommonResult.GetErrorResult("Failed to Clear Cache!!!!"));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, CommonResult.GetErrorResult(ex.Message));
            }
        }

        [HttpGet]
        [Route("flush_database")]
        public HttpResponseMessage FlushDatabase()
        {
            try
            {
                CacheHelper cache = new CacheHelper();
                string json = (string)cache.ClearCacheDatabase();
                if (json == "OK")
                {

                    return Request.CreateResponse(HttpStatusCode.OK, "Database Flushed Successfully!!!");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, CommonResult.GetErrorResult("Failed to Clear Cache!!!!"));
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.OK, CommonResult.GetErrorResult(ex.Message));
            }
        }

    }
}

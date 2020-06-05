using Newtonsoft.Json.Linq;
using svc_global_cache.Models;
using svc_global_cache.Utilities;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace svc_global_cache.Controllers
{
    [RoutePrefix("rs/cachedb")]
    public class WebCacheController : ApiController
    {
        private CacheHelperWebPortal _cacheHelperWebPortal;
        public WebCacheController()
        {
            _cacheHelperWebPortal = new CacheHelperWebPortal();
        }

        [HttpPut]
        [Route("set")]
        public HttpResponseMessage SaveToCacheEsvcPortal([FromBody]CacheSetModel model)
        {
            try
            {
                if (_cacheHelperWebPortal.SaveToCacheEsvcPortal(model.context_s, model.key_s, model.text_s, model.expiryInSeconds_i))
                {
                    return Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("browse")]
        public dynamic ReadFromCacheEsvcPortal(string context_s, string key_s)
        {
            try
            {
                string json = _cacheHelperWebPortal.ReadFromCacheEsvcPortal(context_s, key_s);
                if (!string.IsNullOrEmpty(json))
                {
                    return new JArray() { json };
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpDelete]
        [Route("clear")]
        public HttpResponseMessage ClearCacheKeyForEsvcPortal(string context_s, string key_s)
        {
            try
            {
                string json = _cacheHelperWebPortal.ClearCacheKeyForEsvcPortal(context_s, key_s);
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }


    }
}
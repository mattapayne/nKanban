using System.Web;
using nKanban.Shared;

namespace nKanban.Infrastructure
{
    public class LocalHttpContextWrapper : IHttpContext
    {
        public HttpResponseBase Response
        {
            get { return new HttpResponseWrapper(HttpContext.Current.Response); }
        }

        public HttpRequestBase Request
        {
            get { return new HttpRequestWrapper(HttpContext.Current.Request); }
        }
    }
}
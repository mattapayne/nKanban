using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nKanban.Shared;
using System.Web;
using System.IO;

namespace Specs.nKanban.ServiceSpecs.LoginSvc
{
    public class StubHttpContext : IHttpContext
    {
        private static HttpCookieCollection _cookies;
        
        private class RequestWrapper : HttpRequestWrapper
        {
            public RequestWrapper(HttpRequest request)
                : base(request)
            {
                
            }

            public override HttpCookieCollection Cookies
            {
                get
                {
                    return _cookies;
                }
            }
        }

        private class ResponseWrapper : HttpResponseWrapper
        {
            public ResponseWrapper(HttpResponse response)
                : base(response)
            {
              
            }

            public override HttpCookieCollection Cookies
            {
                get
                {
                    return _cookies;
                }
            }
        }

        private readonly HttpRequestBase _request;
        private readonly HttpResponseBase _response;

        public StubHttpContext()
        {
            _cookies = new HttpCookieCollection();
            _request = new RequestWrapper(new HttpRequest(String.Empty, "http://tempuri.org", "test=test"));
            _response = new ResponseWrapper(new HttpResponse(new StringWriter()));
        }

        public HttpResponseBase Response
        {
            get { return _response; }
        }

        public HttpRequestBase Request
        {
            get { return _request; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace nKanban.Shared
{
    public interface IHttpContext
    {
        HttpResponseBase Response { get; }
        HttpRequestBase Request { get; }
    }
}

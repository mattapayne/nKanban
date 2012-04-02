using System.Web;

namespace nKanban.Shared
{
    public interface IHttpContext
    {
        HttpResponseBase Response { get; }
        HttpRequestBase Request { get; }
    }
}

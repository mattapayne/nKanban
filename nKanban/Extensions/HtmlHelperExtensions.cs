using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Linq.Expressions;
using System.Web.Routing;

namespace nKanban.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            if (htmlAttributes == null)
            {
                return helper.LabelFor(expression);
            }

            var hash = new RouteValueDictionary(htmlAttributes);
            var metadata = ModelMetadata.FromLambdaExpression(expression, helper.ViewData);

            var attrs = String.Join(" ", hash.Select(kvp => String.Format("{0}=\"{1}\"", kvp.Key, kvp.Value)).ToArray());

            return MvcHtmlString.Create(String.Format("<label for=\"{0}\" {1}>{2}</label>", metadata.PropertyName, attrs, metadata.DisplayName));
        }
    }
}
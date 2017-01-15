using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Viola
{
    public static class HtmlRequestHelper
    {
        /// <summary>
        /// Enum'ın model üzerindeki label'ını gösterir.
        /// Kullanımı:
        /// 
        /// @Html.EnumDisplayNameFor(item.UserRole)
        /// </summary>
        /// <param name="html"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public static MvcHtmlString EnumDisplayNameFor(this HtmlHelper html, Enum item)
        {
            var type = item.GetType();
            var member = type.GetMember(item.ToString());
            DisplayAttribute displayname = (DisplayAttribute)member[0].GetCustomAttributes(typeof(DisplayAttribute), false).FirstOrDefault();

            if (displayname != null)
            {
                return new MvcHtmlString(displayname.Name);
            }

            return new MvcHtmlString(item.ToString());
        }


        /// <summary>
        /// @Html.DisplayFor yerine kullanılır. Datetime tipindeki alanları kullanıcı ayarlarındaki timezone'a göre gösterir.
        /// 
        /// Kullanımı: @Html.DisplayDatetimeUserTimeZoneFor(m => m.CreatedDatetime)
        /// </summary>
        public static MvcHtmlString DisplayDatetimeUserTimeZoneFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, html.ViewData);

            try
            {
                if (metadata.Model.GetType() == typeof(DateTime))
                {
                    var dt = Convert.ToDateTime(html.Encode(metadata.Model));
                    return MvcHtmlString.Create(dt.UserTimeZone().ToString());
                }
            }
            catch
            {
            }

            return MvcHtmlString.Empty;
        }



        public static string Id(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("id"))
                return (string)routeValues["id"];
            else if (HttpContext.Current.Request.QueryString.AllKeys.Contains("id"))
                return HttpContext.Current.Request.QueryString["id"];

            return string.Empty;
        }

        public static string ControllerName(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("controller"))
                return (string)routeValues["controller"];

            return string.Empty;
        }

        public static string ActionName(this HtmlHelper htmlHelper)
        {
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;

            if (routeValues.ContainsKey("action"))
                return (string)routeValues["action"];

            return string.Empty;
        }
    }
}
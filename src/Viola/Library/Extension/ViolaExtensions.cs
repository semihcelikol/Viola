using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;

namespace Viola
{
    public static class ViolaExtensions
    {
        /// <summary>
        /// Returns a list of all the exception messages from the top-level
        /// exception down through all the inner exceptions. Useful for making
        /// logs and error pages easier to read when dealing with exceptions.
        /// 
        /// Usage: Exception.Messages()
        /// </summary>
        public static IEnumerable<string> Messages(this Exception ex)
        {
            // return an empty sequence if the provided exception is null
            if (ex == null) { yield break; }
            // first return THIS exception's message at the beginning of the list
            yield return ex.Message;
            // then get all the lower-level exception messages recursively (if any)
            IEnumerable<Exception> innerExceptions = Enumerable.Empty<Exception>();

            if (ex is AggregateException && (ex as AggregateException).InnerExceptions.Any())
            {
                innerExceptions = (ex as AggregateException).InnerExceptions;
            }
            else if (ex.InnerException != null)
            {
                innerExceptions = new Exception[] { ex.InnerException };
            }

            foreach (var innerEx in innerExceptions)
            {
                foreach (string msg in innerEx.Messages())
                {
                    yield return msg;
                }
            }
        }


        /// <summary>
        /// Login olan kullanıcının üzerindeki timezone seçimine göre tarihi getirir.
        /// 
        /// Kullanımı: Datetime.Now.UserTimeZone()
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static DateTime UserTimeZone(this DateTime source)
        {
            if (Viola.Models.User.IsAuthenticated())
            {
                TimeZoneInfo userTimeZone = TimeZoneInfo.FindSystemTimeZoneById(Viola.Models.User.GetCurrentUser().TimeZoneId);

                return TimeZoneInfo.ConvertTimeFromUtc(source, userTimeZone);
            }
            else
            {
                return source;
            }
        }


        /// <summary>
        /// Verilen tarihin bulunduğu haftanın ilk gününü döner

        /// Kullanımı:
        /// DateTime dt = DateTime.Now.StartOfWeek(DayOfWeek.Monday);
        /// DateTime dt = DateTime.Now.StartOfWeek(DayOfWeek.Sunday);
        /// </summary>
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }



        /// <summary>
        /// Verilen tarihin bulunduğu haftanın son gününün döner.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="startOfWeek"></param>
        /// <returns></returns>
        public static DateTime EndOfWeek(this DateTime dt, DayOfWeek startOfWeek = DayOfWeek.Monday)
        {
            DateTime dtStartOfWeek = dt.StartOfWeek(startOfWeek);
            return dtStartOfWeek.AddDays(6);
        }
    }
}
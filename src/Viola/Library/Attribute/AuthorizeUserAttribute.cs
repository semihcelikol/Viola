using System.Collections.Generic;
using System.Web;

namespace Viola
{
    /// <summary>
    /// standart "[Authorize]" yerine kullanılır.
    /// standart identity rolleri yerine kullanıcı tablosundaki custom açılan UserRole enum'ını kullanır.
    /// </summary>
    public class AuthorizeUserAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        // Custom property
        public List<UserRole> RoleList { get; set; }

        public AuthorizeUserAttribute(params object[] roles)
        {
            this.RoleList = new List<UserRole>();

            foreach (UserRole item in roles)
            {
                this.RoleList.Add(item);
            }
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            return RoleList.Contains(Viola.Models.User.GetCurrentUser().UserRole);
        }
    }
}
using System.Collections.Generic;
using System.Web;

namespace Viola
{
    /// <summary>
    /// standart "[Authorize]" yerine kullanılır.
    /// standart identity rolleri yerine kullanıcı tablosundaki custom açılan UserRole enum'ını kullanır.
    /// kullanıcının erişebilmesi için minimum gerekli yetki seviyesi belirtilir.
    /// </summary>
    public class AuthorizeLevelAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        // Custom property
        public UserRole UserRole { get; set; }

        public AuthorizeLevelAttribute(UserRole _UserRole)
        {
            this.UserRole = _UserRole;
        }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            var isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            return Viola.Models.User.GetCurrentUser().UserRole >= this.UserRole;
        }
    }
}
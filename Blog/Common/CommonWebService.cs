using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace Blog.Common
{
    public class CommonWebService
    {
        public void AddRememberMeCookie(String userName, String userData)
        {
            FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(1, userName, DateTime.Now, DateTime.Now.AddMinutes(30), true, userData);
            string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
            HttpCookie authCookie = new HttpCookie("viewerCookie", encryptedTicket);
            HttpContext.Current.Response.Cookies.Add(authCookie);
        }
    }
}
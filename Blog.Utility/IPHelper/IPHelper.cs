namespace Blog.Utility
{
    #region using directives

    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Web;

    #endregion using directives

    public class IpHelper
    {
        public static string GetClientIpAddress(HttpRequest request)
        {
            var result = request.ServerVariables["HTTP_X_FORWARDED_FOR"] ?? request.UserHostAddress;
            if (result == "::1")
            {
                result = Environment.MachineName;
            }
            return result;
        }

        /// <summary>
        ///     获取本机IPv4地址
        /// </summary>
        /// <returns>本机IPv4地址</returns>
        public static String GetHostIpAddress()
        {
            foreach (var ipAddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ipAddress.ToString();
                }
            }
            return String.Empty;
        }

        public static String ConvertToIpAddress(Int64 ipNumber)
        {
            return String.Format("{0}.{1}.{2}.{3}",
                (ipNumber >> 24) & 0xff,
                (ipNumber >> 16) & 0xff,
                (ipNumber >> 8) & 0xff,
                (ipNumber & 0xff));
        }

        public static Int64 ConvertToIpNumber(String ipAddress)
        {
            Int64 result = 0;
            var ipArr = ipAddress.Split(new[] { '.' });
            for (var i = 3; i >= 0; i--)
            {
                var ipNumber = Convert.ToInt64(ipArr[3 - i]);
                result |= ipNumber << i * 8;
            }
            return result;
        }
    }
}
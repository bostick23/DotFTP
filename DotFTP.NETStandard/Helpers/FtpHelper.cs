using System.Net;

namespace DotFTP.Helpers
{
    public class FtpHelper
    {
        public static string CheckAndFixPath(string path)
        {
            string result = path;
            result = result.Replace('\\', '/');

            if (result.StartsWith("/"))
                result = result.Substring(1);

            if (result.EndsWith("/"))
                result = result.Substring(0, result.Length - 1);
            return result;
        }
    }
}

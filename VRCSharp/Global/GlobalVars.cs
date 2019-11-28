using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VRCSharp.Global
{
    public static class GlobalVars
    {
        public static string ApiKey = "JlE5Jldo5Jibnk5O5hTx6XVqsJu4WJ26";

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
    }
}

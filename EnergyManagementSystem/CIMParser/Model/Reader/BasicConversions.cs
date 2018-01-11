using System;

namespace CIM.Model
{
    public class BasicConversions
    {
        public static bool StrToInt(string str, out int num)
        {
            return Int32.TryParse(str, out num);
        }
    }
}

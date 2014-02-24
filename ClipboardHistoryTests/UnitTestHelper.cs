using System;
using System.Linq;
using System.Reflection;

namespace ClipboardHistoryTests
{
    public class UnitTestHelper
    {
        // typeof(ClipboardDataItem),
        // "GetArrayOfLines",
        // new object[1] { inputString }
        public static object RunStaticMethod(Type classType, string methodName, object[] methodParameters)
        {
            var method = classType.GetMethod(methodName,
                    BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
            return method.Invoke(null, methodParameters);
        }

        // typeof(MyControl),
        // "AddStringToHistoryCollection",
        // myControl,
        // new object[] { inputString }
        public static object RunInstanceMethod(Type classType, string methodName, object classInstance, object[] methodParameters)
        {
            var method = classType.GetMethod(methodName,
                    BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            return method.Invoke(classInstance, methodParameters);
        }
    }
}

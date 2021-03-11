using System.Reflection;
using Com.Qsw.Framework.Session.Interface;

namespace Com.Qsw.Framework.Session.Impl
{
    public static class LockUtils
    {
        public static LockAttribute FindLockAttribute(MethodInfo methodInfo)
        {
            while (true)
            {
                var lockAttribute = methodInfo.GetCustomAttribute<LockAttribute>();
                if (lockAttribute == null)
                {
                    MethodInfo baseMethodInfo = methodInfo.GetBaseDefinition();
                    if (baseMethodInfo == methodInfo)
                    {
                        break;
                    }

                    methodInfo = baseMethodInfo;
                }
                else
                {
                    return lockAttribute;
                }
            }

            return null;
        }
    }
}
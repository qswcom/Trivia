using System.Reflection;
using Com.Qsw.Framework.Session.Interface;

namespace Com.Qsw.Framework.Session.Impl
{
    public static class TransactionUtils
    {
        public static TransactionAttribute FindTransactionAttribute(MethodInfo methodInfo)
        {
            while (true)
            {
                var transactionAttribute = methodInfo.GetCustomAttribute<TransactionAttribute>();
                if (transactionAttribute == null)
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
                    return transactionAttribute;
                }
            }

            return null;
        }
    }
}
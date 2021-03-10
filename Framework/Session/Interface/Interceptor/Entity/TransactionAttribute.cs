using System;
using System.Data;

namespace Com.Qsw.Framework.Session.Interface
{
    [Serializable]
    [AttributeUsage(AttributeTargets.Method)]
    public class TransactionAttribute : Attribute
    {
        public TransactionAttribute(bool readOnly) : this(readOnly, IsolationLevel.ReadCommitted)
        {
        }

        public TransactionAttribute(IsolationLevel isolationLevel) : this(false, isolationLevel)
        {
        }

        private TransactionAttribute(bool readOnly, IsolationLevel isolationLevel)
        {
            ReadOnly = readOnly;
            IsolationLevel = isolationLevel;
        }

        public IsolationLevel IsolationLevel { get; }

        public bool ReadOnly { get; }
    }
}
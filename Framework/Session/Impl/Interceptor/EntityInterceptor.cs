using System;
using Com.Qsw.Framework.Session.Interface;
using NHibernate;
using NHibernate.Type;

namespace Com.Qsw.Framework.Session.Impl
{
    public class EntityInterceptor : EmptyInterceptor
    {
        public override bool OnSave(object entity, object id, object[] state, string[] propertyNames, IType[] types)
        {
            var retValue = false;

            bool isTimeStamp = entity is ITimeStamp;
            bool isVersion = entity is IVersion;
            bool isEntityId = entity is IEntityId;
            DateTime utcNow = DateTime.UtcNow;
            for (var propertyIndex = 0; propertyIndex < propertyNames.Length; propertyIndex++)
            {
                string propertyName = propertyNames[propertyIndex];

                if (isEntityId && propertyName == nameof(IEntityId.Id))
                {
                    state[propertyIndex] = 0;
                    retValue = true;
                }

                if (isTimeStamp && propertyName == nameof(ITimeStamp.Timestamp))
                {
                    state[propertyIndex] = utcNow;
                    retValue = true;
                }

                if (isVersion && propertyName == nameof(IVersion.Version))
                {
                    state[propertyIndex] = 1;
                    retValue = true;
                }
            }

            return retValue;
        }

        public override bool OnFlushDirty(object entity, object id, object[] currentState, object[] previousState,
            string[] propertyNames,
            IType[] types)
        {
            var retValue = false;

            bool isTimeStamp = entity is ITimeStamp;
            DateTime utcNow = DateTime.UtcNow;

            for (var propertyIndex = 0; propertyIndex < propertyNames.Length; propertyIndex++)
            {
                string propertyName = propertyNames[propertyIndex];

                if (isTimeStamp && propertyName == nameof(ITimeStamp.Timestamp))
                {
                    currentState[propertyIndex] = utcNow;
                    retValue = true;
                }
            }

            return retValue;
        }
    }
}
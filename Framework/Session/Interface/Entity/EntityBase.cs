using System;

namespace Com.Qsw.Framework.Session.Interface
{
    [Serializable]
    public class EntityBase : IEntityBase
    {
        public long Id { get; set; }
        public DateTime Timestamp { get; set; }
        public int Version { get; set; }
    }
}
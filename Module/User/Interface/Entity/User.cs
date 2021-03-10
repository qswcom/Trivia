using System;
using Com.Qsw.Framework.Session.Interface;

namespace Com.Qsw.Module.User.Interface
{
    [Serializable]
    public class User : EntityBase
    {
        public string UserName { get; set; }
    }
}
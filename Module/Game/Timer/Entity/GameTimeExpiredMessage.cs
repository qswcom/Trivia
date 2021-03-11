using System;

namespace Com.Qsw.Module.Game.Timer
{
    [Serializable]
    public class GameTimeExpiredMessage
    {
        public long GameId { get; set; }
        public string UserId { get; set; }
    }
}
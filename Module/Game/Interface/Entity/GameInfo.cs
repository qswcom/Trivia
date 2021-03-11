using System;
using System.Collections.Generic;
using Com.Qsw.Framework.Session.Interface;

namespace Com.Qsw.Module.Game.Interface
{
    [Serializable]
    public class GameInfo : IEntityId
    {
        public GameInfo()
        {
            GameQuestionInfo = new GameQuestionInfo();
            GameUserInfoByUserIdDictionary = new Dictionary<string, GameUserInfo>();
            GameResult = new GameResult();
        }

        public long Id { get; set; }
        public GameQuestionInfo GameQuestionInfo { get; set; }
        public IDictionary<string, GameUserInfo> GameUserInfoByUserIdDictionary { get; set; }
        public GameState GameState { get; set; }
        public GameResult GameResult { get; set; }
    }
}
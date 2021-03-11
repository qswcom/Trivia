using System;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public class CommandInfo
    {
        public string Input { get; set; }
        public string Description { get; set; }
        public Action Action { get; set; }
    }
}
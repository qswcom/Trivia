using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    [Serializable]
    public class QuestionInfo
    {
        public QuestionCategory QuestionCategory { get; set; }
        public int Difficult { get; set; }
        public string Question { get; set; }
        public IList<string> Answers { get; set; }
        
        [JsonIgnore]
        public string AnswersJson
        {
            get => JsonConvert.SerializeObject(Answers);
            set => Answers = JsonConvert.DeserializeObject<IList<string>>(value);
        }
        
        public string CorrectAnswer { get; set; }
    }
}
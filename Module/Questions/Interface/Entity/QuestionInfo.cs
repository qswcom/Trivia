using System;
using System.Collections.Generic;
using Com.Qsw.Framework.Session.Interface;
using Newtonsoft.Json;

namespace Com.Qsw.Module.Question.Interface
{
    [Serializable]
    public class QuestionInfo : EntityBase
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
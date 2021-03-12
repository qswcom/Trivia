using System;
using System.ComponentModel;
using System.Linq;
using Autofac;
using IContainer = Autofac.IContainer;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class GameView : BaseView
    {
        private readonly IContainer container;
        private readonly IView parent;
        private readonly long gameId;

        private readonly GameViewModel gameViewModel;

        public GameView(IContainer container, IView parent, long gameId)
        {
            this.container = container;
            this.parent = parent;
            this.gameId = gameId;

            gameViewModel = container.Resolve<GameViewModel>();

            CommandInfoByInputDictionary["gi"] = new CommandInfo
            {
                Input = "gi",
                Description = "Try to init game again.",
                Action = Init
            };

            CommandInfoByInputDictionary["gp"] = new CommandInfo
            {
                Input = "gp",
                Description = "Print game information.",
                Action = HandleGameInfo
            };

            CommandInfoByInputDictionary["gs"] = new CommandInfo
            {
                Input = "gs",
                Description = "Submit answer.",
                Action = gameViewModel.SubmitAnswerCommand
            };

            CommandInfoByInputDictionary["gl"] = new CommandInfo
            {
                Input = "gl",
                Description = "Leave game.",
                Action = gameViewModel.LeaveGameCommand
            };

            gameViewModel.PropertyChanged += OnPropertyChanged;
        }

        public override void Dispose()
        {
            base.Dispose();
            gameViewModel.Dispose();
        }

        public void Init()
        {
            Console.WriteLine("Now in game view.");
            gameViewModel.Bind(gameId).Wait();
            HandleGameInfo();
        }

        #region Property Changed

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(gameViewModel.Error))
            {
                HandleError();
                return;
            }

            if (e.PropertyName == nameof(gameViewModel.GameInfo))
            {
                HandleGameInfo();
            }
        }

        private void HandleError()
        {
            Console.WriteLine(gameViewModel.Error);
        }

        private void HandleGameInfo()
        {
            Console.WriteLine("Show game details.");
            Console.WriteLine("State and Abbreviation");
            foreach (GameUserQuestionState gameUserQuestionState in Enum.GetValues<GameUserQuestionState>())
            {
                Console.WriteLine($"{GetPrintStr(gameUserQuestionState)} - {gameUserQuestionState}");
            }

            GameInfo gameInfo = gameViewModel.GameInfo;
            if (gameInfo == null)
            {
                Console.WriteLine("Error, can't find game information");
                return;
            }

            GameUserInfo selfGameUserInfo = null;
            Console.WriteLine($"Id: {gameInfo.Id}");
            Console.WriteLine($"Game state: {gameInfo.GameState}");
            if (gameInfo.GameResult != null)
            {
                Console.WriteLine($"Winner: {gameInfo.GameResult.WinnerUserId}");
            }

            Console.WriteLine("Users statistics:");
            foreach (GameUserInfo gameUserInfo in gameInfo.GameUserInfoByUserIdDictionary.Values)
            {
                if (gameUserInfo.UserId == gameViewModel.UserInfo.UserId)
                {
                    selfGameUserInfo = gameUserInfo;
                    continue;
                }

                Console.WriteLine($"User {gameUserInfo.UserId} detail:");
                Console.WriteLine($"State: {gameUserInfo.GameUserState}");
                string questionAnswers = string.Join("\t",
                    gameUserInfo.GameUserQuestionInfoList.Select(GetPrintStr).ToArray());
                Console.WriteLine($"Question Result: {questionAnswers}");
            }

            if (selfGameUserInfo != null)
            {
                Console.WriteLine();
                Console.WriteLine("Self detail:");
                Console.WriteLine($"State: {selfGameUserInfo.GameUserState}");
                string questionAnswers = string.Join("\t",
                    selfGameUserInfo.GameUserQuestionInfoList.Select(GetPrintStr).ToArray());
                Console.WriteLine($"Question Result: {questionAnswers}");
            }

            Console.WriteLine("Last Question:");
            QuestionInfo questionInfo = gameInfo.GameQuestionInfo.QuestionInfoList.Last();
            Console.WriteLine(questionInfo.Question);
            foreach (string answer in questionInfo.Answers)
            {
                Console.WriteLine(answer);
            }
            Console.WriteLine("You have 30sec to answer the question, input 'gs' to start submit:");
        }

        #endregion

        #region Helper

        private string GetPrintStr(GameUserQuestionInfo gameUserQuestionInfo)
        {
            return GetPrintStr(gameUserQuestionInfo.GameUserQuestionState);
        }

        private string GetPrintStr(GameUserQuestionState gameUserQuestionState)
        {
            switch (gameUserQuestionState)
            {
                case GameUserQuestionState.NotStart:
                    return "NS";
                case GameUserQuestionState.Process:
                    return "PR";
                case GameUserQuestionState.Success:
                    return "SU";
                case GameUserQuestionState.Failed:
                    return "FL";
                case GameUserQuestionState.Overtime:
                    return "OT";
                case GameUserQuestionState.UnApply:
                    return "UA";
                default:
                    return "UN";
            }
        }

        #endregion
    }
}
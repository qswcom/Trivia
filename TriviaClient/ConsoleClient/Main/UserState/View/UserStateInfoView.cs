using Autofac;

namespace Com.Qsw.TriviaClient.ConsoleClient.Main
{
    public class UserStateInfoView : BaseView
    {
        private readonly IContainer container;
        private readonly MainView parent;
        private readonly UserStateInfoViewModel userStateInfoViewModel;

        public UserStateInfoView(IContainer container, MainView parent)
        {
            this.container = container;
            this.parent = parent;

            userStateInfoViewModel = container.Resolve<UserStateInfoViewModel>();
            
        }

        public override void Dispose()
        {
            base.Dispose();
            userStateInfoViewModel.Dispose();
        }

        public void Init()
        {
            userStateInfoViewModel.Bind().Wait();
        }
        
        
    }
}
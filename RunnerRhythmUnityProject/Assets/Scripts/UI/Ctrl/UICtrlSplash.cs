using CHBase;

namespace RR
{
    [UIAttribute((int)ERRUIGroup.Splash)]
	public class UICtrlSplash : UICtrlGenericBase<UIViewSplash> 
	{
        private int _timer;
        protected override void InitEventListners()
        {
            base.InitEventListners();
            Messenger.AddListener(EMessengerType.OnApplicationStart, OnApplicationStart);
        }

        public override void Open(object parameter)
        {
            base.Open(parameter);
            _timer = RuntimeConfig.Instance.SplashShowTime;
        }

        public override void OnUpdate()
        {
            base.OnUpdate();
            _timer--;
            if (_timer <= 0)
            {
				RRUIManager.Instance.CloseUI<UICtrlSplash>(true);
            }
        }

		private void OnApplicationStart ()
        {
			RRUIManager.Instance.OpenUI<UICtrlSplash>(0);
        }
	}
}
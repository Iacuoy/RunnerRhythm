using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHBase;

namespace RR
{
	[UIAttribute((int)ERRUIGroup.Loading)]
	public class UICtrlResLoading : UICtrlGenericBase<UIViewResLoading> 
	{
		#region fields

		/// <summary>
		/// 资源加载界面最小显示时间
		/// </summary>
		private static float s_minShowTime = 1f;
		
		private float _rotateImgAngleSpeed = 1f;

		/// <summary>
		/// 进度条进度
		/// </summary>
		private float _targetFillAmount;

		/// <summary>
		/// 关闭界面倒计时
		/// </summary>
		private float _closeCountDown;

		/// <summary>
		/// 是否等Splash界面 关闭，然后自己关闭
		/// </summary>
		private bool _waitSplashUIClose;

		private string _progressTextFormat = "{0:N} b/{1:N} b ({2}/{3})";
		#endregion

		#region properties
		#endregion

		#region methods
        protected override void InitEventListners()
        {
            base.InitEventListners();
			Messenger.AddListener(EMessengerType.OnResourcesCheckStart, OnResourcesCheckStart);
	        Messenger<Vector2Int, long, long>.AddListener(EMessengerType.OnResourcesUpdateProgressUpdate, OnResourcesCheckProgressUpdate);
			Messenger<ELocalText>.AddListener(EMessengerType.OnVersionUpdateStateChange, OnResourcesCheckStateChange);
			Messenger.AddListener(EMessengerType.OnResourcesCheckFinish, OnResourcesCheckFinished);
        }

		protected override void OnOpen (object parameter)
		{
			base.OnOpen (parameter);
			UITools.SetText(_view.Status, ELocalText.CheckingRes);
			_closeCountDown = -1f;
			_waitSplashUIClose = false;
		}

		public override void OnUpdate ()
		{
			base.OnUpdate ();
			_view.RotateImg.Rotate(0, 0, _rotateImgAngleSpeed);
			_view.ProgressBar.fillAmount = _targetFillAmount;

			if (_waitSplashUIClose)
			{
				if (_closeCountDown > 0)
				{
					_closeCountDown -= Time.deltaTime;
					if (_closeCountDown <= 0)
					{
						RRUIManager.Instance.CloseUI<UICtrlResLoading>(true);
						Messenger.Broadcast (EMessengerType.OnGameBegin);
					}
				}
				else
				{
					if (!RRUIManager.Instance.GetUI<UICtrlSplash>().IsOpen)
					{
						_closeCountDown = s_minShowTime;
					}
				}
			}
		}

		private void OnResourcesCheckStart ()
		{
			RRUIManager.Instance.OpenUI<UICtrlResLoading>(0);
		}

		private void OnResourcesCheckStateChange (ELocalText stateText)
		{
			_view.Status.text = LocalizationManager.GetText(stateText);
		}

		private void OnResourcesCheckProgressUpdate (Vector2Int cnt, long sizeDone, long sizeTotal)
        {
			if (IsOpen)
			{
				_targetFillAmount = Mathf.Clamp01((float)sizeDone / sizeTotal);
				_view.ProgressText.text = string.Format(_progressTextFormat, sizeDone, sizeTotal, cnt.x, cnt.y);
			}
        }
        private void OnResourcesCheckFinished ()
        {
	        if (RRUIManager.Instance.GetUI<UICtrlSplash>().IsOpen)
	        {
		        _waitSplashUIClose = true;
	        }
	        else
	        {
		        RRUIManager.Instance.CloseUI<UICtrlResLoading>(true);
		        Messenger.Broadcast (EMessengerType.OnGameBegin);
	        }
        }
		#endregion
	}
}
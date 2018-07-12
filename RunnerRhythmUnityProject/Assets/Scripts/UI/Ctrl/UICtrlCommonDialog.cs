using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CHBase;

namespace RR
{
	[UIAttribute((int)ERRUIGroup.PopupDialog)]
	public class UICtrlCommonDialog : UICtrlGenericBase<UIViewCommonDialog> 
	{
		#region fields
		private System.Action _btn1CB;
		private System.Action _btn2CB;
		private System.Action _btn3CB;

		private bool _autoClose;
		#endregion
		#region properties
		#endregion
		#region methods
		protected override void InitEventListners ()
		{
			base.InitEventListners ();
			Messenger<CommonDialogMessageData>.AddListener(EMessengerType.ShowCommonDialog, ShowCommonDialog);
		}
		protected override void OnViewCreated ()
		{
			base.OnViewCreated ();
			_view.CloseBtn.onClick.AddListener(OnCloseBtn);
			_view.Btn1.onClick.AddListener(OnBtn1);
			_view.Btn2.onClick.AddListener(OnBtn2);
			_view.Btn3.onClick.AddListener(OnBtn3);
		}

		private void ShowCommonDialog (CommonDialogMessageData data)
		{
			RRUIManager.Instance.OpenUI<UICtrlCommonDialog>(1);
			_autoClose = data.AutoClose;
			_view.Title.text = data.Title;
			_view.Content.text = data.Content;
			_view.CloseBtnObj.SetActive(data.ShowCloseBtn);
			if (!string.IsNullOrEmpty(data.Btn1Text) && null != data.Btn1CB)
			{
				_btn1CB = data.Btn1CB;
				_view.Btn1Obj.SetActive(true);
				_view.Btn1Text.text = data.Btn1Text;
			}
			else
			{
				_view.Btn1Obj.SetActive(false);
			}
			if (!string.IsNullOrEmpty(data.Btn2Text) && null != data.Btn2CB)
			{
				_btn2CB = data.Btn2CB;
				_view.Btn2Obj.SetActive(true);
				_view.Btn2Text.text = data.Btn2Text;
			}
			else
			{
				_view.Btn2Obj.SetActive(false);
			}
			if (!string.IsNullOrEmpty(data.Btn3Text) && null != data.Btn3CB)
			{
				_btn3CB = data.Btn3CB;
				_view.Btn3Obj.SetActive(true);
				_view.Btn3Text.text = data.Btn3Text;
			}
			else
			{
				_view.Btn3Obj.SetActive(false);
			}
		}

		private void OnCloseBtn ()
		{
			RRUIManager.Instance.CloseUI<UICtrlCommonDialog>();
		}

		private void OnBtn1 ()
		{
			if (_autoClose)
			{
				RRUIManager.Instance.CloseUI<UICtrlCommonDialog>();
			}
			if (null != _btn1CB)
			{
				_btn1CB.Invoke();
			}
		}
		private void OnBtn2 ()
		{
			if (_autoClose)
			{
				RRUIManager.Instance.CloseUI<UICtrlCommonDialog>();
			}
			RRUIManager.Instance.CloseUI<UICtrlCommonDialog>();
			if (null != _btn2CB)
			{
				_btn2CB.Invoke();
			}
		}
		private void OnBtn3 ()
		{
			if (_autoClose)
			{
				RRUIManager.Instance.CloseUI<UICtrlCommonDialog>();
			}
			RRUIManager.Instance.CloseUI<UICtrlCommonDialog>();
			if (null != _btn3CB)
			{
				_btn3CB.Invoke();
			}
		}
		#endregion
	}
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CHBase;

namespace RR
{
	public class UIViewResLoading : UIViewBase
	{
		public RectTransform RotateImg;
		public Image ProgressBar;
		public Text Status;

		public Text CurrentAssetName;
		public Text ProgressText;
	}
}
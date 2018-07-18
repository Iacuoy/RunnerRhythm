using CHBase;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace RR
{
	public class UIViewMain : UIViewBase
	{
		public Button PlayBtn;
		public Button StopBtn;

		public Slider VolumeSlider;
		public RadialSlider TimeSlider;

		public Transform PlayPosIndicator;

		public SnapElementVerticalScroller LoopTimeScroller;
	}
}
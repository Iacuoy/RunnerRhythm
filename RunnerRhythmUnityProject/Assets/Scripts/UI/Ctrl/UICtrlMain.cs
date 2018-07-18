using CHBase;
using UnityEngine;

namespace RR
{
	[UIAttribute((int) ERRUIGroup.MainUI)]
	public class UICtrlMain : UICtrlGenericBase<UIViewMain>
	{
		private const float _rhythmPercentageMin = 0.1f;
		private static float[] s_loopTimes = {30f, 60f, 120f, 300f};
		protected override void OnViewCreated ()
		{
			base.OnViewCreated();
			
			_view.PlayBtn.onClick.AddListener(OnPlayBtn);
			_view.StopBtn.onClick.AddListener(OnStopBtn);
			
			_view.VolumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
			_view.TimeSlider.Value = RRMain.Instance.Player.RhythmTimePercentage;
			_view.TimeSlider.onValueChanged.AddListener(OnTimeSliderChanged);
			
			_view.LoopTimeScroller.onValueChanged.AddListener(OnLoopTimeScrollerChanged);
		}

		public override void OnUpdate ()
		{
			base.OnUpdate();
			_view.PlayPosIndicator.rotation = Quaternion.Euler(0,0,-360f * RRMain.Instance.Player.Pos);
		}

		private void OnPlayBtn ()
		{
			RRMain.Instance.Player.Play();
			_view.PlayBtn.gameObject.SetActive(false);
			_view.StopBtn.gameObject.SetActive(true);
		}

		private void OnStopBtn ()
		{
			RRMain.Instance.Player.Stop();
			_view.PlayBtn.gameObject.SetActive(true);
			_view.StopBtn.gameObject.SetActive(false);
		}

		private void OnVolumeSliderChanged (float value)
		{
			// dB 0 is 0.75 in Slider
			float dB = 20 * Mathf.Log10(value / 0.75f);
			RRMain.Instance.AudioMixer.SetFloat("MasterVolume", dB);
		}

		private void OnTimeSliderChanged (int angle)
		{
			var value = angle / 360f;
			RRMain.Instance.Player.RhythmTimePercentage = value;
			_view.TimeSlider.Value = RRMain.Instance.Player.RhythmTimePercentage;
		}

	    private void OnLoopTimeScrollerChanged (int value)
	    {
		    if (value >= 0 && value < s_loopTimes.Length)
		    {
			    RRMain.Instance.Player.LoopLength = s_loopTimes[value];
		    }
	    }
	}
}
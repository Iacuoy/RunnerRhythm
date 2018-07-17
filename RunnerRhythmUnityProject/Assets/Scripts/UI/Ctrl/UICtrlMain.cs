using CHBase;
using UnityEngine;

namespace RR
{
    [UIAttribute((int)ERRUIGroup.MainUI)]
	public class UICtrlMain : UICtrlGenericBase<UIViewMain> 
	{
		protected override void OnViewCreated ()
		{
			base.OnViewCreated();
			
			_view.PlayBtn.onClick.AddListener(OnPlayBtn);
			_view.StopBtn.onClick.AddListener(OnStopBtn);
			
			_view.VolumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
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
			// dB 0 is 0.75 in Slider (1 / 0.75 = 1.33333)
			float dB = 20 * Mathf.Log10(value * 1.3333333333333333333f);
			RRMain.Instance.AudioMixer.SetFloat("MasterVolume", dB);
		}
	}
}
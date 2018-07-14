using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RR
{
	public class RhythmPlayer
	{
		public enum EPlayerState
		{
			Stop,
			Play,
			Pause,
		}

		private EPlayerState _state = EPlayerState.Stop;
		
		private SoundEffectConfig _currentConfig;

		private int _beatCycleFrameLength = 1000;

		private int _frameCnter;
		
		
		
		public float RealBpm
		{
			get { return 0; }
		}
		
		public void Play ()
		{
			if (null == _currentConfig) return;
			_frameCnter = 1;
			_state = EPlayerState.Play;
		}

		public void Pause ()
		{
			if (null == _currentConfig) return;
			Stop();
		}

		public void Stop ()
		{
			if (null == _currentConfig) return;
			_state = EPlayerState.Stop;
		}

		public void SetSoundEffectConfig (SoundEffectConfig config)
		{
			_currentConfig = config;
		}

		public void FixedUpdate ()
		{
			if (EPlayerState.Play == _state)
			{
				_frameCnter--;
				if (_frameCnter <= 0)
				{
					_frameCnter = _beatCycleFrameLength;
					_currentConfig.AudioSourceA.Play();
				}
			}
		}
	}
}
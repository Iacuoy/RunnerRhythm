using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Sirenix.OdinInspector;
using UnityEngine;

namespace RR
{
	public class RhythmPlayer
	{
		private const float C_DefaultFadeTime = 5f;
		public enum EPlayerState
		{
			Stop,
			PlayRhythmFadeIn,
			PlayRhythmNormal,
			PlayRhythmFadeOut,
			PlaySilence,
			Pause,
		}

		[ShowInInspector] [EnumToggleButtons]
		private EPlayerState _state = EPlayerState.Stop;
		
		private SoundEffectConfig _currentConfig;

		/// <summary>
		/// 不播放节奏的时间
		/// </summary>
		[ShowInInspector] [MinValue(10)] [MaxValue(180)]
		private float _silenceTime = 60f;

		/// <summary>
		/// 播放节奏的时间
		/// </summary>
		/// <returns></returns>
		[ShowInInspector] [MinValue(5)] [MaxValue(60)]
		private float _rhythmTime = 20f;
		[ShowInInspector] [ReadOnly]
		private float _timer;
		
		
		public void Play ()
		{
			if (null == _currentConfig) return;
			_state = EPlayerState.PlayRhythmFadeIn;
			_currentConfig.AudioSourceA.volume = 0f;
			_currentConfig.AudioSourceA.Play();
			_timer = 0;
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
			_currentConfig.AudioSourceA.Stop();
		}

		public void SetSoundEffectConfig (SoundEffectConfig config)
		{
			_currentConfig = config;
		}

		public void Update ()
		{
			if (EPlayerState.Stop == _state) return;
			_timer += Time.deltaTime;
			if (EPlayerState.PlayRhythmFadeIn == _state)
			{
				var length = Mathf.Min(_rhythmTime * 0.25f, C_DefaultFadeTime);
				_currentConfig.AudioSourceA.volume = _timer / length;
				if (_timer > length)
				{
					_state = EPlayerState.PlayRhythmNormal;
					_currentConfig.AudioSourceA.volume = 1f;
				}
			}
			else if (EPlayerState.PlayRhythmNormal == _state)
			{
				if (_timer > Mathf.Max(_rhythmTime * 0.75f, _rhythmTime - C_DefaultFadeTime))
				{
					_state = EPlayerState.PlayRhythmFadeOut;
				}
			}
			else if (EPlayerState.PlayRhythmFadeOut == _state)
			{
				var a = _timer - Mathf.Max(_rhythmTime * 0.75f, _rhythmTime - C_DefaultFadeTime);
				_currentConfig.AudioSourceA.volume =
					1f - a / Mathf.Min(_rhythmTime * 0.25f, C_DefaultFadeTime);
				if (_timer >= _rhythmTime)
				{
					_state = EPlayerState.PlaySilence;
					_currentConfig.AudioSourceA.volume = 0f;
					_timer = 0;
				}
			}
			else if (EPlayerState.PlaySilence == _state)
			{
				if (_timer >= _silenceTime)
				{
					_state = EPlayerState.PlayRhythmFadeIn;
					_timer = 0;
				}
			}
		}
	}
}
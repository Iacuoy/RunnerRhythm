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
		private const float C_MinRhythmTime = 5f;
		private const float C_MinLoopTime = 10f;
		private const float C_MaxLoopTime = 600f;
		
		public enum EPlayerState
		{
			Stop,
			Play,
			Pause,
		}

		[ShowInInspector] [EnumToggleButtons]
		private EPlayerState _state = EPlayerState.Stop;
		
		private SoundEffectConfig _currentConfig;

		/// <summary>
		/// 播放节奏的时间占总时间的比例
		/// </summary>
		[ShowInInspector] [Range(0f, 1f)]
		private float _rhythmTimePercentage = .2f;
		/// <summary>
		/// 一个循环的时间，单位s
		/// </summary>
		/// <returns></returns>
		[ShowInInspector] [Range(5f, 300f)]
		private float _loopLength = 30f;
		
		[ShowInInspector] [ReadOnly]
		private float _pos;

		public float Pos
		{
			get { return _pos; }
		}
		
		public float RhythmTimePercentage
		{
			get { return _rhythmTimePercentage; }
			set
			{
				if (value * _loopLength < C_MinRhythmTime) _rhythmTimePercentage = C_MinRhythmTime / _loopLength;
				else _rhythmTimePercentage = value;
			}
		}
		public float LoopLength
		{
			get { return _loopLength; }
			set
			{
				if (value < C_MinLoopTime) _loopLength = C_MinLoopTime;
				if (value > C_MaxLoopTime) _loopLength = C_MaxLoopTime;
				else _loopLength = value;
			}
		}

		public void Play ()
		{
			if (null == _currentConfig) return;
			_state = EPlayerState.Play;
			_currentConfig.AudioSourceA.volume = 0f;
			_currentConfig.AudioSourceA.Play();
			_pos = 0;
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
			_pos = 0f;
		}

		public void SetSoundEffectConfig (SoundEffectConfig config)
		{
			_currentConfig = config;
		}

		public void Update ()
		{
			if (EPlayerState.Stop == _state) return;
			_pos += Time.deltaTime / _loopLength;
			if (_pos >= 1f) _pos -= 1f;
			if (EPlayerState.Play == _state)
			{
				var rhythmLength = _loopLength * _rhythmTimePercentage;
				var fadeInEndPos = Mathf.Min(rhythmLength * 0.25f, C_DefaultFadeTime) / _loopLength;

				if (_pos < fadeInEndPos)
				{
					_currentConfig.AudioSourceA.volume = _pos / fadeInEndPos;
				}
				else if (_pos < _rhythmTimePercentage - fadeInEndPos)
				{
					_currentConfig.AudioSourceA.volume = 1f;
				}
				else if (_pos < _rhythmTimePercentage)
				{
					_currentConfig.AudioSourceA.volume = (_rhythmTimePercentage - _pos) / fadeInEndPos;
				}
				else
				{
					_currentConfig.AudioSourceA.volume = 0f;
				}
			}
		}
	}
}
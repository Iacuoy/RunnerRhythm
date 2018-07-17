using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using CHBase;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;

namespace RR
{
	public class RRMain : MainBase<RRMain>
	{
		[ShowInInspector]
		private RhythmPlayer _player;
		[SerializeField] private SoundEffectConfig _config;

		[SerializeField]
		private AudioMixer _masterAudioMixer;

		public AudioMixer AudioMixer
		{
			get { return _masterAudioMixer; }
		}

		public RhythmPlayer Player
		{
			get { return _player; }
		}
		
		protected override void Init ()
		{
			base.Init();
			RRUIManager.Instance.Init(typeof(RRUIManager), (int)ERRUIGroup.Max);
			
			_player = new RhythmPlayer();
			_masterAudioMixer.SetFloat("MasterVolume", 0.75f);
		}

		protected override void GameBegin ()
		{
			base.GameBegin();

			RRUIManager.Instance.OpenUI<UICtrlMain>(1);
			_player.SetSoundEffectConfig(_config);
		}

		protected override void UpdateLogic ()
		{
			RRUIManager.Instance.Update();
			_player.Update();
		}

		protected override void CHFixedUpdate ()
		{
			RRUIManager.Instance.FixedUpdate();
		}
	}
}
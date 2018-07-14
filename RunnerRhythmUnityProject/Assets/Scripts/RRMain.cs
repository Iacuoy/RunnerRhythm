using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using CHBase;
using UnityEngine;

namespace RR
{
	public class RRMain : MainBase<RRMain>
	{
		private RhythmPlayer _player;
		private Stopwatch _stopwatch;
		[SerializeField] private SoundEffectConfig _config;

		private int cnt = 0;
		private long time3200;
		private long time9600;
		private long time38400;
		
		protected override void Init ()
		{
			base.Init();
			RRUIManager.Instance.Init(typeof(RRUIManager), (int)ERRUIGroup.Max);
			
			_player = new RhythmPlayer();
			_stopwatch = new Stopwatch();
		}

		protected override void GameBegin ()
		{
			base.GameBegin();
			
			_player.SetSoundEffectConfig(_config);
			_player.Play();
		}

		protected override void UpdateLogic ()
		{
			RRUIManager.Instance.Update();
		}

		protected override void CHFixedUpdate ()
		{
			RRUIManager.Instance.FixedUpdate();
			_player.FixedUpdate();
			if (cnt == 500)
			{
				_stopwatch.Start();
			}
			if (cnt == 3200 + 500)
			{
				time3200 = _stopwatch.ElapsedMilliseconds;
			}
			
			if (cnt == 9600 + 500)
			{
				time9600 = _stopwatch.ElapsedMilliseconds;
			}
			if (cnt == 38400 + 500)
			{
				time38400 = _stopwatch.ElapsedMilliseconds;
				LogTool.Info("3200 time: {0}, 9600 time: {1}, 38400: {2}", time3200, time9600, time38400);
			}
			
			cnt++;
		}
	}
}
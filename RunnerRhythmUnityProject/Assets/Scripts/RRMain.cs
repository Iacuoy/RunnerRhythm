using System.Collections;
using System.Collections.Generic;
using CHBase;
using UnityEngine;

namespace RR
{
	public class RRMain : MainBase<RRMain>
	{
		protected override void Init ()
		{
			base.Init();
			RRUIManager.Instance.Init(typeof(RRUIManager), (int)ERRUIGroup.Max);
		}

		protected override void UpdateLogic ()
		{
			RRUIManager.Instance.Update();
		}

		protected override void CHFixedUpdate ()
		{
			RRUIManager.Instance.FixedUpdate();
		}
	}
}
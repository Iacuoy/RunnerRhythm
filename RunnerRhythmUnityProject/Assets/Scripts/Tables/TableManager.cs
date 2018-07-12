using System;
using System.Collections.Generic;
using UnityEngine;

namespace CHBase
{
	[Serializable]
	public partial class TableManager : Singleton<TableManager>
	{
		#region fields
		private static TableManager _instance;
		public readonly Dictionary<int,Table_Text> Table_TextDic = new Dictionary<int, Table_Text>();
		[SerializeField] private Table_Text[] _tableTexts;
		#endregion
		#region properties=
		#endregion
		#region methods
		public void Init()
		{
			string TextJsonStr = ResourcesManager.Instance.GetTableStr("Text", ResDefine.UnloadImmediatelyScenary);
			_tableTexts = Newtonsoft.Json.JsonConvert.DeserializeObject<Table_Text[]>(TextJsonStr);
			ResourcesManager.Instance.UnloadScenary(ResDefine.UnloadImmediatelyScenary);
			for (int i = 0; i < _tableTexts.Length; i++)
			{
				if (!Table_TextDic.ContainsKey(_tableTexts[i].Id))
				{
					Table_TextDic.Add(_tableTexts[i].Id,_tableTexts[i]);
				}
				else
				{
					LogTool.Warning("_tableTexts table.Id {0} is duplicated!", _tableTexts[i].Id);
				}
			}
		}

		public Table_Text GetText(int key)
		{
			Table_Text tmp;
			if (Table_TextDic.TryGetValue(key,out tmp))
			{
				return tmp;
			}
			return null;
		}

		#endregion
	}
}

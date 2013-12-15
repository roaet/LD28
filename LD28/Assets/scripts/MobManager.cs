using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class MobManager {
	private List<MobInfo> m_mobs;

	public MobManager(string loadSource) {
		m_mobs = new List<MobInfo>();
		TextAsset json = (TextAsset)Resources.Load(loadSource, typeof(TextAsset));
		if(!json) {
			Debug.LogError("Couldn't find: " + loadSource);
		}
		string content = json.text;
		Debug.Log ("Created mob manager");
		JSONNode cards = JSON.Parse(content);
		int mobCount = cards["mobCount"].AsInt;
		Debug.Log ("There are  " + mobCount + " mobs");
		for(int i = 0; i < mobCount; i++) {
			MobInfo mobInfo = new MobInfo(cards["mobs"][i]);
			m_mobs.Add(mobInfo);
		}
		Debug.Log("Loaded " + m_mobs.Count + " mobs");
	}

	public MobInfo GetMobByName(string name) {
		foreach(MobInfo info in m_mobs) {
			if(info.name == name) {
				return info;
			}
		}
		return null;
	}
}

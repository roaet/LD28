using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class EventInfo {
	private List<MobInfo> m_mobs;

	public EventInfo(JSONNode eventJson) {
		m_mobs = new List<MobInfo>();
		int mobCount = eventJson["mobCount"].AsInt;
		for(int i = 0; i < mobCount; i++) {
			MobInfo mob = new MobInfo(eventJson["mobs"][i]);
			m_mobs.Add(mob);
		}
	}

	public List<MobInfo> mobs {
		get {
			return m_mobs;
		}
	}
}

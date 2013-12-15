using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class EventInfo {
	private List<string> m_mobs;

	public EventInfo(JSONNode eventJson) {
		m_mobs = new List<string>();
		int mobCount = eventJson["mobCount"].AsInt;
		for(int i = 0; i < mobCount; i++) {
			string mob = eventJson["mobs"][i];
			m_mobs.Add(mob);
		}
	}

	public List<string> mobs {
		get {
			return m_mobs;
		}
	}
}

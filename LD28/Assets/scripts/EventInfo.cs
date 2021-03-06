﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class EventInfo {
	private List<string> m_mobs;
	private bool m_isStore;
	private bool m_isInn;
	private bool m_isExit;

	public EventInfo(JSONNode eventJson) {
		m_mobs = new List<string>();
		m_isExit = eventJson["isExit"].AsBool;
		int mobCount = eventJson["mobCount"].AsInt;
		m_isStore = eventJson["isStore"].AsBool;
		m_isInn = eventJson["isInn"].AsBool;
		for(int i = 0; i < mobCount; i++) {
			string mob = eventJson["mobs"][i];
			m_mobs.Add(mob);
		}
	}
	public bool isExit { get { return m_isExit; } }
	public bool isInn { get { return m_isInn; } }

	public bool isStore {
		get {
			return m_isStore;
		}
	}

	public List<string> mobs {
		get {
			return m_mobs;
		}
	}
}

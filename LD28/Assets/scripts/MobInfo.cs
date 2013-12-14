using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class MobInfo {
	private string m_sprite;
	
	public MobInfo(JSONNode mobJson) {
		m_sprite = mobJson["sprite"];
	}

	public string sprite {
		get {
			return m_sprite;
		}
	}
}

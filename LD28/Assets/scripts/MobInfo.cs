using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class MobInfo {
	private string m_sprite;
	private string m_name;
	
	public MobInfo(JSONNode mobJson) {
		m_sprite = mobJson["sprite"];
		m_name = mobJson["name"];
	}

	public string sprite {
		get {
			return m_sprite;
		}
	}

	public string name {
		get {
			return m_name;
		}
	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class CharacterInfo {
	private string m_name;

	public CharacterInfo(JSONNode charJson) {
		m_name = charJson["name"];
	}

	public string name {
		get {
			return m_name;
		}
	}

}

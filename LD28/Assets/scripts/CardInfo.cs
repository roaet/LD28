using UnityEngine;
using System.Collections;
using SimpleJSON;

public class CardInfo {
	private string m_sprite;
	private string m_name;

	public CardInfo(JSONNode cardJson) {
		m_name = cardJson["name"];
		m_sprite = cardJson["sprite"];
		Debug.Log ("Loaded card named " + m_name);
	}

	public string name {
		get {
			return m_name;
		}
	}

	public string sprite {
		get {
			return m_sprite;
		}
	}
}

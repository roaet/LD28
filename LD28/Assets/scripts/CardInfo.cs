using UnityEngine;
using System.Collections;
using SimpleJSON;

public class CardInfo {
	private string m_sprite;
	private string m_name;
	private string m_type;
	private string m_target;

	public CardInfo(JSONNode cardJson) {
		m_name = cardJson["name"];
		m_sprite = cardJson["sprite"];
		m_type = cardJson["type"];
		m_target = cardJson["target"];
	}

	public string target { get { return m_target; } }

	public string name {
		get {
			return m_name;
		}
	}

	public string type { get { return m_type; } }

	public string sprite {
		get {
			return m_sprite;
		}
	}
}

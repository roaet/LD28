using UnityEngine;
using System.Collections;
using SimpleJSON;

public class CardInfo {
	private string m_sprite;
	private string m_name;
	private string m_type;
	private string m_target;
	private string m_tip;
	private int m_cost;
	private bool m_enabled;

	public CardInfo(JSONNode cardJson) {
		m_name = cardJson["name"];
		m_sprite = cardJson["sprite"];
		m_type = cardJson["type"];
		m_target = cardJson["target"];
		m_tip = cardJson["tip"];
		m_cost = cardJson["cost"].AsInt;
		m_enabled = cardJson["enabled"].AsBool;
	}

	public string target { get { return m_target; } }
	public string tip { get { return m_tip + " (Cost: " + m_cost + " gold)"; } }
	public int cost { get { return m_cost; } }
	public bool enabled { get { return m_enabled; } }

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

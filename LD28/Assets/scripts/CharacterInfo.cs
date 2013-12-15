using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class CharacterInfo {
	private string m_name;
	private string m_portrait;
	private string m_class;
	private int m_damage;
	private int m_health;

	public CharacterInfo(JSONNode charJson) {
		m_name = charJson["name"];
		m_portrait = charJson["portrait"];
		m_class = charJson["class"];
		m_damage = charJson["damage"].AsInt;
		m_health = charJson["hp"].AsInt;
	}

	public int damage { get { return m_damage; } }
	public int health { get { return m_health; } }

	public string name {
		get {
			return m_name;
		}
	}

	public string portrait {
		get {
			return m_portrait;
		}
	}

	public string charClass {
		get {
			return m_class;
		}
	}
}

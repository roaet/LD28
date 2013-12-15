﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class MobInfo {
	private string m_sprite;
	private string m_name;
	private Color m_color;
	private int m_health;
	private bool m_hasHealth;
	
	public MobInfo(JSONNode mobJson) {
		m_sprite = mobJson["sprite"];
		m_name = mobJson["name"];
		m_health = mobJson["health"].AsInt;
		m_hasHealth = mobJson["hasHealth"].AsBool;
		m_color = Color.white;

	}

	public bool hasHealth {
		get {
			return m_hasHealth;
		}
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
	
	public int health {
		get {
			return m_health;
		}
	}

	public Color color {
		get {
			return m_color;
		}
	}
}

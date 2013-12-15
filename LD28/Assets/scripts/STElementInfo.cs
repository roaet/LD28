using UnityEngine;
using System.Collections;
using SimpleJSON;

public class STElementInfo {
	private string m_spriteName;
	private Color m_spriteColor;
	private EventInfo m_eventInfo;
	private bool m_alwaysSeen;

	public STElementInfo(JSONNode stJson) {
		string spriteName = stJson["sprite"];
		m_alwaysSeen = stJson["doShow"].AsBool;
		JSONNode color = stJson["color"];
		Color col = Color.white;
		if(color != null) {
			col = new Color(color["r"].AsFloat, color["g"].AsFloat, color["b"].AsFloat);
		}
		m_spriteName = spriteName;
		m_spriteColor = col;
		m_eventInfo = new EventInfo(stJson);
	}

	public bool alwaysSeen { get { return m_alwaysSeen; } }

	public EventInfo eventInfo {
		get {
			return m_eventInfo;
		}
	}

	public string sprite {
		get {
			return m_spriteName;
		}
	}

	public Color color {
		get {
			return m_spriteColor;
		}
	}
}

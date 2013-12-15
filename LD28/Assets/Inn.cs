using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inn : MonoBehaviour {
	public Level level;
	private CharacterManager chars;
	private EventInfo m_info;
	private List<CharacterInfo> characters;

	void Awake() {
		characters = new List<CharacterInfo>();
	}

	public void InitializeInn(EventInfo info) {
		chars = level.characterManager;
		m_info = info;
		foreach(string name in m_info.mobs) {
			CharacterInfo c = chars.GetCharacterByName(name);
			if(c != null) {
				Debug.Log("Found character named: " + name);
				characters.Add(c);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InnClosed() {
		level.CloseInn();
	}
}

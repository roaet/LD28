﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class CharacterManager {
	private List<CharacterInfo> m_chars;
	private List<Person> m_party;

	public CharacterManager(string fileName) {
		m_chars = new List<CharacterInfo>();
		m_party = new List<Person>();
		TextAsset json = (TextAsset)Resources.Load(fileName, typeof(TextAsset));
		if(!json) {
			Debug.LogError("Couldn't find: " + fileName);
		}
		string content = json.text;
		Debug.Log ("Created character manager");
		JSONNode characters = JSON.Parse(content);
		int charCount = characters["charCount"].AsInt;
		Debug.Log ("There are  " + charCount + " characters");
		for(int i = 0; i < charCount; i++) {
			CharacterInfo charInfo = new CharacterInfo(characters["chars"][i]);
			m_chars.Add(charInfo);
		}
	}

	public CharacterInfo GetCharacterByName(string name) {
		foreach(CharacterInfo info in m_chars) {
			if(info.name == name) {
				return info;
			}
		}
		return null;
	}

	public int GetPartyVision() {
		int i = 1;
		foreach(Person p in party) {
			if(p.info.charClass == "wizard") i++;
		}
		return i;
	}

	public bool AddPersonToParty(Person p) {
		if(m_party.Count == 4) return false;
		m_party.Add(p);
		return true;
	}

	public bool AddPersonToPartyAt(Person p, int index) {
		Debug.Log ("Replacing at " + index);
		m_party[index] = p;
		return true;
	}


	public bool IsPartyFullHealth() {
		foreach(Person p in party) {
			if(p.currentHealth < p.totalHealth) return false;
		}
		return true;
	}

	public void HealPartyFor(int heal) {
		foreach(Person p in party) {
			p.Heal(heal);
		}
	}

	public bool PartyAlive() {
		foreach(Person p in party) {
			if(p.currentHealth > 0) return true;
		}
		return false;
	}

	public List<Person> party {
		get {
			return m_party;
		}
	}

}

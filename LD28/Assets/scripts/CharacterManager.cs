using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class CharacterManager {
	private List<CharacterInfo> m_chars;
	private List<CharacterInfo> m_party;

	public CharacterManager(string fileName) {
		m_chars = new List<CharacterInfo>();
		m_party = new List<CharacterInfo>();
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

	public List<CharacterInfo> party {
		get {
			return m_party;
		}
	}

}

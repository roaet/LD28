using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Inn : MonoBehaviour {
	public Level level;
	public InnSelection slot1;
	public InnSelection slot2;
	public InnSelection slot3;

	public InnReplacer repl1;
	public InnReplacer repl2;
	public InnReplacer repl3;
	public InnReplacer repl4;

	private CharacterManager chars;
	private EventInfo m_info;
	private List<CharacterInfo> characters;
	private CharacterInfo newRecruit;

	private List<InnSelection> slots;
	private bool inreplace = false;

	void Awake() {
		characters = new List<CharacterInfo>();
		slots = new List<InnSelection>();
		slots.Add(slot1);
		slots.Add(slot2);
		slots.Add(slot3);
		
		for(int i = 0; i < 3; i++) {
			slots[i].ToggleSlot(false);
		}
		ToggleReplacers(false);

	}

	public void ToggleReplacers(bool value) {
		repl1.ToggleReplacer(value);
		repl2.ToggleReplacer(value);
		repl3.ToggleReplacer(value);
		repl4.ToggleReplacer(value);

	}

	public void CharacterClicked(CharacterInfo info) {
		newRecruit = info;
		Debug.Log ("A character was clicked");
		if(chars.party.Count > 3) {
			ToggleReplacers(true);
			inreplace = true;
		} else {
			level.RecruitCharacter(newRecruit);
			level.CloseInn();
		}
	}

	public void CharacterReplaced(int index) {
		if(!inreplace) return;
		level.ReplaceCharacter(index - 1, newRecruit);
		ToggleReplacers(false);
		level.CloseInn();
		inreplace = false;

	}

	public void InitializeInn(EventInfo info) {
		characters.Clear();
		chars = level.characterManager;
		m_info = info;
		foreach(string name in m_info.mobs) {
			CharacterInfo c = chars.GetCharacterByName(name);
			if(c != null) {
				Debug.Log("Found character named: " + name);
				characters.Add(c);
			}
		}
		for(int i = 0; i < 3; i++) {
			slots[i].ToggleSlot(false);
		}
		for(int i = 0; i < characters.Count && i < 3; i++) {
			slots[i].InitializeSlot(characters[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void InnClosed() {
		level.CloseInn();
	}
}

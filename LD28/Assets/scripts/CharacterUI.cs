using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterUI : MonoBehaviour {
	public GUIText name1;
	public GUIText name2;
	public GUIText name3;
	public GUIText name4;

	public Character slot1;
	public Character slot2;
	public Character slot3;
	public Character slot4;

	private List<Character> m_slots;

	// Use this for initialization
	void Start () {
		m_slots = new List<Character>();
		m_slots.Add (slot1);
		m_slots.Add (slot2);
		m_slots.Add (slot3);
		m_slots.Add (slot4);
		name1.enabled = false;
		name2.enabled = false;
		name3.enabled = false;
		name4.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void UpdateParty(CharacterManager charMan) {
		for(int i = 0; i < charMan.party.Count; i++) {
			m_slots[i].InitializeCharacter(name1, charMan.party[i]);
		}
	}
}

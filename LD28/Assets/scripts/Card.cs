﻿using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
	private CardInfo m_info;
	private SpriteRenderer m_sprite;

	void Awake() {
		m_sprite = GetComponent<SpriteRenderer>();
	}

	public void InitializeCard(CardInfo info) {
		m_info = info;
		Sprite[] textures = Resources.LoadAll<Sprite>("images/cards");
		string sprite = info.sprite;
		foreach(Sprite s in textures) {
			if(s.name == sprite) {
				m_sprite.sprite = s;
				break;
			}
		}
	}

	public CardInfo info {
		get {
			return m_info;
		}
	}
}

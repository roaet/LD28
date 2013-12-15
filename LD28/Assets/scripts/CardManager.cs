using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;


public class CardManager {
	private List<CardInfo> m_cards;
	private Deck m_deck;


	public CardManager(string loadSource) {
		m_cards = new List<CardInfo>();
		m_deck = new Deck(this);
		TextAsset json = (TextAsset)Resources.Load(loadSource, typeof(TextAsset));
		if(!json) {
			Debug.LogError("Couldn't find: " + loadSource);
		}
		string content = json.text;
		Debug.Log ("Created card manager");
		JSONNode cards = JSON.Parse(content);
		int cardCount = cards["cardCount"].AsInt;
		Debug.Log ("There are  " + cardCount + " cards");
		for(int i = 0; i < cardCount; i++) {
			CardInfo cardInfo = new CardInfo(cards["cards"][i]);
			m_cards.Add(cardInfo);
		}
		Debug.Log("Loaded " + m_cards.Count + " cards");

		json = (TextAsset)Resources.Load("deck", typeof(TextAsset));
		if(!json) {
			Debug.LogError("Couldn't find: " + loadSource);
		}
		content = json.text;
		JSONNode deck = JSON.Parse(content);
		m_deck.LoadDeck(deck);
	}

	public CardInfo GetCardByName(string name) {
		foreach(CardInfo info in m_cards) {
			if(info.name == name) {
				return info;
			}
		}
		return null;
	}

	public Deck deck {
		get {
			return m_deck;
		}
	}
}

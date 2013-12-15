using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Deck {
	private List<CardInfo> m_cards;
	private List<CardInfo> m_discarded;
	private CardManager m_cardManager;

	public Deck(CardManager cardManager) {
		m_cards = new List<CardInfo>();
		m_discarded = new List<CardInfo>();
		m_cardManager = cardManager;
	}

	public void Shuffle() {
		System.Random rng = new System.Random();
		int n = m_cards.Count;
		while(n > 1) {
			n--;
			int k = rng.Next(n+1);
			CardInfo value = m_cards[k];
			m_cards[k] = m_cards[n];
			m_cards[n] = value;
		}
	}

	public int DeckSize {
		get {
			return m_cards.Count;
		}
	}

	public void MergeDiscardAndShuffle() {
		for(int i = 0; i < m_discarded.Count; i++) {
			m_cards.Add(m_discarded[i]);
		}
		m_discarded.Clear();
		Shuffle();
	}

	public CardInfo DrawCardIntoHand() {
		if(m_cards.Count == 0) {
			Debug.Log ("Shuffling discard into deck");
			if(m_discarded.Count == 0) {
				Debug.Log("Player has all cards in hand, cannot draw");
				return null;
			}
			MergeDiscardAndShuffle();
		}
		CardInfo info = m_cards[0];
		m_cards.RemoveAt(0);
		return info;
	}

	public void PutCardIntoDiscard(CardInfo card) {
		Debug.Log(card.name + " was discarded");
		m_discarded.Add(card);
	}

	public void LoadDeck(JSONNode deckJson) {
		int entryCount = deckJson["entryCount"].AsInt;
		for(int i = 0; i < entryCount; i++) {
			JSONNode entry = deckJson["entries"][i];
			int quantity = entry["quantity"].AsInt;
			string name = entry["name"];
			Debug.Log ("Found " + quantity + " of " + name);
			for(int j = 0; j < quantity; j++) {
				CardInfo card = m_cardManager.GetCardByName(name);
				m_cards.Add (card);
			}
		}
		Debug.Log ("Deck has " + m_cards.Count + " cards");
		Shuffle();
	}

	public void AddCard(string name) {
		
	}
	
}

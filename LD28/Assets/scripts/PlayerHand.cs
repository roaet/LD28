﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHand : MonoBehaviour {
	public GameObject cardPrefab;
	public GameObject firstCard;
	public GameObject lastCard;
	public GameObject cardConfirm;

	private CardManager m_cardManager;

	private List<CardInHand> m_cards;
	private CardInHand focusedCard;
	private CardInHand lastFocusedCard;

	private CardInHand selectedCard;
	private int selectedCardIndex;
	private GameObject confirmObject;
	private bool change;

	// Use this for initialization
	void Start () {
		m_cards = new List<CardInHand>();
		lastFocusedCard = focusedCard = null;
		change = false;
	}

	public CardManager cardManager {
		set {
			m_cardManager = value;
		} get {
			return m_cardManager;
		}
	}

	public void CardWasClicked(CardInHand card) {
		if(selectedCard != null) return;
		SelectACard(card);
	}

	public void CardFocused(CardInHand card) {
		if(selectedCard != null) return;
		focusedCard = card;
	}
	
	public void CardUnfocused(CardInHand card) {
		if(selectedCard != null) return;
		lastFocusedCard = card;
		focusedCard = null;
	}

	private void TweenTo(CardInHand card, Vector3 position) {
		iTween.MoveTo(card.gameObject,
		              iTween.Hash("time", 0.5f, 
		            			  "position", position));
	}

	private void TweenBy(CardInHand card, float xValue) {
		iTween.MoveBy(card.gameObject,
		              iTween.Hash("x", xValue,
		            "time", 0.5f,
		            "looptype", "none"));
	}
	
	private int FindIndexOfCard(CardInHand card) {
		for(int i = 0; i < m_cards.Count; i++) 
			if(m_cards[i] == card)
				return i;
		return -1;
	}

	private CardInHand CreateCard(Vector3 position, CardInfo info) {
		//if(infos.Count <= 0) return null;
		Vector3 target = GetCardSpawnPoint();
		GameObject element = Instantiate(cardPrefab,
		                                 target,
		                                 Quaternion.identity) as GameObject;
		Card c = element.GetComponent<Card>();
		c.InitializeCard(info);
		CardInHand card = element.GetComponent<CardInHand>();
		card.info = info;
		card.target = target;
		iTween.Init(card.gameObject);
		card.hand = this;
		return card;
	}

	private Vector3 GetCardSpawnPoint() {
		if(m_cards.Count == 0) {
			return PointByDistance(0.5f);
		}
		if(m_cards.Count == 1) {
			return PointByDistance(0.66f);
		}
		return lastCard.transform.position;
	}

	private void MoveCardsToTargets() {
		foreach(CardInHand card in m_cards) {
			if(card.transform.position.x == card.target.x) continue;
			TweenTo(card, card.target);
		}
	} 

	private void MoveCardsToResting() {
		foreach(CardInHand card in m_cards) {
			if(card.transform.position.x == card.resting.x) continue;
			TweenTo(card, card.resting);
		}
	}

	private void OrganizeCards() {
		Vector3 a = firstCard.transform.position;
		Vector3 b = lastCard.transform.position;
		if(m_cards.Count == 0) return;
		
		float baseZed = -1.0f;
		for(int i = 0; i < m_cards.Count; i++) {
			CardInHand card = m_cards[i];
			Vector3 pos = card.transform.position;
			pos.z = baseZed - (i * 0.25f);
			card.transform.position = pos;
		}
		// this is terrible and I should feel terrible (fix this)
		if(m_cards.Count == 1) {
			m_cards[0].resting = PointByDistance(0.5f);
		} else if(m_cards.Count == 2) {
			m_cards[0].resting = PointByDistance(0.25f);
			m_cards[1].resting = PointByDistance(0.75f);
		} else if(m_cards.Count == 3) {
			m_cards[0].resting = a;
			m_cards[1].resting = PointByDistance(0.5f);
			m_cards[2].resting = b;
		} else {
			m_cards[0].resting = a;
			m_cards[m_cards.Count-1].resting = b;
			for(int i = 1; i < m_cards.Count-1; i++) {
				Vector3 pos = PointByDistance(1.0f / (m_cards.Count-1) * i);
				m_cards[i].resting = pos;
			}
		}
		
		for(int i = 0; i < m_cards.Count; i++) {
			CardInHand card = m_cards[i];
			Vector3 pos = card.resting;
			pos.z = baseZed - (i * 0.25f);
			card.resting = pos;
		}
	}
	
	private void PushCardsAwayFrom(CardInHand card) {
		int idx = FindIndexOfCard(card);
		float popDistance = 0.5f;
		
		float baseZed = -5.0f;
		for(int i = 0; i < m_cards.Count; i++) {
			CardInHand c = m_cards[i];
			Vector3 pt = c.resting;
			Vector3 zpt = c.transform.position;
			if(i < idx) {
				zpt.z = baseZed + (idx - i * 0.25f);
				pt.x -= popDistance;
			} else if (i > idx) {
				zpt.z = baseZed + (i - idx + 1 * 0.25f);
				pt.x += popDistance;
			} else {
				zpt.z = baseZed;
				c.target = pt;
			}
			pt.z = zpt.z;
			c.transform.position = zpt;
			c.target = pt;
		}
	}

	private Vector3 PointByDistance(float distance) {
		Vector3 a = firstCard.transform.position;
		Vector3 b = lastCard.transform.position;
		return Vector3.Lerp(a, b, distance);
	}

	private void RemoveCardAt(int idx) {
		if(idx >= m_cards.Count) return;
		CardInHand card = m_cards[idx];
		Destroy(card.gameObject);
		m_cards.RemoveAt(idx);
	}

	private void SelectACard(CardInHand card) {
		selectedCard = card;
		int idx = FindIndexOfCard(card);
		selectedCardIndex = idx;
		m_cards.RemoveAt(idx);
		TweenTo (card, new Vector3(0.0f, 0.0f, -6.0f));
		iTween.ScaleTo(card.gameObject, new Vector3(2.0f, 2.0f, 1.0f), 1.0f);
		change = true;
		selectedCard.selected = true;
		lastFocusedCard = card;
		focusedCard = null;
		BringUpConfirmation();
	}

	private void BringUpConfirmation() {
		if(confirmObject != null) return;
		GameObject element = Instantiate(cardConfirm,
		                                 new Vector3(0.0f, 0.0f, -5.0f),
		                                 Quaternion.identity) as GameObject;
		iTween.Init(element);
		iTween.FadeFrom (element, iTween.Hash("alpha", 0.0f, "time", 0.25f));
		CardConfirm confirm = element.GetComponent<CardConfirm>();
		confirm.hand = this;
		confirmObject = element;
	}

	public void CancelSelection() {
		iTween.ScaleTo(selectedCard.gameObject, new Vector3(1.0f, 1.0f, 1.0f), 1.0f);
		m_cards.Insert(selectedCardIndex, selectedCard);
		selectedCard.selected = false;
		selectedCard = null;
		change = true;
		Destroy(confirmObject);
	}

	public void ConfirmSelection() {
		Destroy(confirmObject);
		m_cardManager.deck.PutCardIntoDiscard(selectedCard.info);
		Destroy(selectedCard.gameObject);
		selectedCard.selected = false;
		selectedCard = null;

	}

	public void DrawCard() {
		CardInfo info = m_cardManager.deck.DrawCardIntoHand();
		if(info == null) {
			Debug.Log ("Failed to draw");
			return;
		}
		CardInHand card = CreateCard(transform.position, info);
		m_cards.Add (card);
		change = true;
	}

	public void Discard(CardInHand card) {
		m_cardManager.deck.PutCardIntoDiscard(card.info);
		Destroy(card.gameObject);
	}

	public void DiscardAllBut(CardInHand save) {
		foreach(CardInHand card in m_cards) {
			if(save == card) continue;
			Discard(card);
		}
		m_cards.Clear();
		m_cards.Add(save);
		change = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.D)) {
			DrawCard();
		}
		if(Input.GetKeyDown (KeyCode.Alpha1)) {
			CardInHand save = m_cards[0];
			DiscardAllBut(save);
		}
		if(change) {
			OrganizeCards();
		}
		// if there is a card focused or the focus has changed
		if(focusedCard != null && lastFocusedCard != focusedCard) {
			lastFocusedCard = focusedCard;
			PushCardsAwayFrom(focusedCard);
		}
		if(focusedCard != null && m_cards.Count > 3) {
			MoveCardsToTargets();
		} else {
			MoveCardsToResting();
		}
		change = false;
	}
}

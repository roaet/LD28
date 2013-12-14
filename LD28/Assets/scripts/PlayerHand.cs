using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHand : MonoBehaviour {
	public GameObject cardPrefab;
	public GameObject firstCard;
	public GameObject lastCard;

	private List<CardInHand> m_cards;
	private CardInHand focusedCard;
	private CardInHand lastFocusedCard;

	// Use this for initialization
	void Start () {
		m_cards = new List<CardInHand>();
		lastFocusedCard = focusedCard = null;
	}

	public void CardWasClicked(CardInHand card) {
		Debug.Log ("A card was clicked, yo");
	}

	public void CardFocused(CardInHand card) {
		focusedCard = card;
		//PushCardsAwayFromIndex(idx);
	}
	
	public void CardUnfocused(CardInHand card) {
		lastFocusedCard = card;
		focusedCard = null;
		//OrganizeCards();
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

	private CardInHand CreateCard(Vector3 position) {
		//if(infos.Count <= 0) return null;
		Vector3 target = GetCardSpawnPoint();
		GameObject element = Instantiate(cardPrefab,
		                                 target,
		                                 Quaternion.identity) as GameObject;
		CardInHand card = element.GetComponent<CardInHand>();
		card.target = target;
		iTween.Init(card.gameObject);
		card.hand = this;
		SpriteRenderer sprite = card.GetComponent<SpriteRenderer>();
		sprite.color = new Color(Random.Range (0.0f, 1.0f),
		                         Random.Range (0.0f, 1.0f),
		                         Random.Range (0.0f, 1.0f));
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
		float time = 0.10f;
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
			m_cards[0].resting = PointByDistance(0.33f);
			m_cards[1].resting = PointByDistance(0.66f);
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
		Vector3 leper = card.resting;
		float popDistance = 0.5f;
		float time = 0.10f;
		
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
	
	// Update is called once per frame
	void Update () {
		bool change = false;
		if(Input.GetKeyDown(KeyCode.D)) {
			CardInHand card = CreateCard(transform.position);
			m_cards.Add (card);
			change = true;
		}
		if(Input.GetKeyDown(KeyCode.Alpha1)) {
			RemoveCardAt(0);
			change = true;
		}
		if(Input.GetKeyDown(KeyCode.Alpha2)) {
			RemoveCardAt(m_cards.Count/2);
			change = true;
		}
		if(Input.GetKeyDown(KeyCode.Alpha3)) {
			RemoveCardAt(m_cards.Count-1);
			change = true;
		}
		if(change) {
			OrganizeCards();
		}
		// if there is a card focused or the focus has changed
		if(focusedCard != null && lastFocusedCard != focusedCard) {
			lastFocusedCard = focusedCard;
			PushCardsAwayFrom(focusedCard);
		}
		if(focusedCard != null) {
			MoveCardsToTargets();
		} else {
			MoveCardsToResting();
		}
	}
}

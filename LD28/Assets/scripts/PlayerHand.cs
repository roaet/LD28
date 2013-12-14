using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHand : MonoBehaviour {
	public GameObject cardPrefab;
	public GameObject firstCard;
	public GameObject lastCard;

	private List<CardInHand> m_cards;

	// Use this for initialization
	void Start () {
		m_cards = new List<CardInHand>();
	}

	public void CardWasClicked(CardInHand card) {
		Debug.Log ("A card was clicked, yo");
	}

	private CardInHand CreateCard(Vector3 position) {
		//if(infos.Count <= 0) return null;
		GameObject element = Instantiate(cardPrefab,
		                                 GetCardSpawnPoint(),
		                                 Quaternion.identity) as GameObject;
		CardInHand card = element.GetComponent<CardInHand>();
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

	private void OrganizeCards() {
		Vector3 a = firstCard.transform.position;
		Vector3 b = lastCard.transform.position;
		if(m_cards.Count == 0) return;
		// this is terrible and I should feel terrible (fix this)
		if(m_cards.Count == 1) {
			iTween.MoveTo(m_cards[0].gameObject,iTween.Hash("position", PointByDistance(0.5f)));
		} else if(m_cards.Count == 2) {
			iTween.MoveTo(m_cards[0].gameObject,iTween.Hash("position", PointByDistance(0.33f)));
			iTween.MoveTo(m_cards[1].gameObject,iTween.Hash("position", PointByDistance(0.66f)));
			m_cards[1].transform.position = PointByDistance(0.5f);
		} else if(m_cards.Count == 3) {
			iTween.MoveTo(m_cards[0].gameObject,iTween.Hash("position", a));
			iTween.MoveTo(m_cards[1].gameObject,iTween.Hash("position", PointByDistance(0.5f)));
			iTween.MoveTo(m_cards[2].gameObject,iTween.Hash("position", b));
		} else {
			m_cards[0].transform.position = a;
			iTween.MoveTo(m_cards[0].gameObject,iTween.Hash("position", a));
			iTween.MoveTo(m_cards[m_cards.Count-1].gameObject,iTween.Hash("position", b));
			for(int i = 1; i < m_cards.Count-1; i++) {
				CardInHand card = m_cards[i];
				Vector3 pos = PointByDistance(1.0f / (m_cards.Count-1) * i);
				iTween.MoveTo(card.gameObject,iTween.Hash("position", pos));
			}
		}
		float baseZed = 0.0f;
		for(int i = 0; i < m_cards.Count; i++) {
			CardInHand card = m_cards[i];
			Vector3 pos = card.transform.position;
			pos.z -= baseZed + (i * 0.25f);
			card.transform.position = pos;
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
		if(change) OrganizeCards();
	}
}

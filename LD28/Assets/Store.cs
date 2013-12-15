using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Store : MonoBehaviour {
	public Card cardSlot1;
	public Card cardSlot2;
	public Card cardSlot3;
	public Card cardSlot4;
	public Card cardSlot5;
	public SpriteRenderer potion;

	public Level level;

	private bool m_isStoreUp;

	private List<Card> slots;
	private List<CardInfo> availableCards;

	private bool updateSprites;

	// Use this for initialization
	void Awake () {
		slots = new List<Card>();
		slots.Add(cardSlot1);
		slots.Add(cardSlot2);
		slots.Add(cardSlot3);
		slots.Add(cardSlot4);
		slots.Add(cardSlot5);

		m_isStoreUp = false;
	}

	public void StoreClosed() {
		level.CloseStore();
	}

	public void PotionClicked() {
		level.UsePotion();
		updateSprites = true;

	}

	public void CardClicked(Card card) {
		level.CardBuyAttempt(card.info);
		updateSprites = true;
	}

	public void InitializeStore() {
		CardManager cards = level.cardManager;
		availableCards = cards.GetEnabledCards();
		for(int i = 0; i < 5; i++) {
			slots[i].sprite.enabled = false;
			slots[i].sprite.color = Color.white;
			slots[i].store = null;
		}
		for(int i = 0; i < availableCards.Count && i < 5; i++) {

			slots[i].sprite.enabled = true;
			if(availableCards[i].cost > level.gold) {
				slots[i].sprite.color = new Color(0.5f, 0.5f, 0.5f);
			}
			slots[i].InitializeCard(availableCards[i]);
			slots[i].store = this;
		}
	}

	public bool isStoreUp {
		get { return m_isStoreUp; }
		set { m_isStoreUp = value; }
	}

	private void UpdateInterface() {
		if(updateSprites) {
			for(int i = 0; i < 5; i++) {
				if(slots[i].sprite.enabled) {
					if(slots[i].info.cost > level.gold) {
						slots[i].sprite.color = new Color(0.5f, 0.5f, 0.5f);
					}
				}
			}
			if(level.gold < 2) {
				potion.color = new Color(0.5f, 0.5f, 0.5f);
			} else {
				potion.color = Color.white;
			}
			updateSprites = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		UpdateInterface();
	
	}
}

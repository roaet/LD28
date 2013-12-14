using UnityEngine;
using System.Collections;

public class CardInHand : MonoBehaviour {

	private PlayerHand m_hand;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseEnter() {
	}

	void OnMouseExit() {
	}

	void OnMouseDown() {
		m_hand.CardWasClicked(this);
	}

	public PlayerHand hand {
		set {
			m_hand = value;
		}
	}
}

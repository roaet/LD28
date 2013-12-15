using UnityEngine;
using System.Collections;

public class CardInHand : MonoBehaviour {

	private PlayerHand m_hand;
	private Vector3 targetPosition;
	private Vector3 restingPosition;
	private bool isSelected;
	private CardInfo m_info;

	// Use this for initialization
	void Start () {
		isSelected = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnMouseEnter() {
		if(m_hand == null) return;
		m_hand.CardFocused(this);
	}

	void OnMouseExit() {
		if(m_hand == null) return;
		m_hand.CardUnfocused(this);
	}

	void OnMouseDown() {
		if(m_hand == null) return;
		m_hand.CardWasClicked(this);
	}

	public CardInfo info {
		get {
			return m_info;
		} set {
			m_info = value;
		}
	}

	public PlayerHand hand {
		set {
			m_hand = value;
		}
	}
	
	public Vector3 resting {
		set {
			restingPosition = value;
		} get {
			return restingPosition;
		}
	}

	public Vector3 target {
		set {
			targetPosition = value;
		} get {
			return targetPosition;
		}
	}

	public bool selected {
		set {
			isSelected = value;
		} get {
			return isSelected;
		}
	}
}

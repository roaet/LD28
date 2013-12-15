using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour {
	private CardInfo m_info;
	private SpriteRenderer m_sprite;
	private ToolTip m_toolTip;

	private Store m_store;

	public Store store {
		set {
			m_store = value;
		}
	}

	void OnMouseDown() {
		if(m_store != null) {
			m_store.CardClicked(this);
		}
	}

	void Awake() {
		m_sprite = GetComponent<SpriteRenderer>();
		m_toolTip = GetComponent<ToolTip>();
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
		if(m_toolTip != null) {
			m_toolTip.text = m_info.tip;
			m_toolTip.isEnabled = true;
		}
	}

	public SpriteRenderer sprite {
		get { return m_sprite; }
	}

	public CardInfo info {
		get {
			return m_info;
		}
	}
}

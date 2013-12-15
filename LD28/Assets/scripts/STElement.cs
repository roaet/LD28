using UnityEngine;
using System.Collections;
using SimpleJSON;

public class STElement : MonoBehaviour {
	public Sprite unknownSprite;

	private SpriteRenderer m_sprite;
	private STElementInfo m_info;
	private Sprite m_realTexture;

	private bool canBeSeen;
	// Use this for initialization
	void Awake () {
		m_sprite = GetComponent<SpriteRenderer>();
		canBeSeen = false;
	}

	public STElementInfo info {
		get {
			return m_info;
		}
	}

	public void ConfigureElement(STElementInfo info) {
		m_info = info;

		Sprite[] textures = Resources.LoadAll<Sprite>("images/st_elements");
		string sprite = info.sprite;
		if(sprite == "battle") {
			sprite = "battle" + info.eventInfo.mobs.Count;
		}
		foreach(Sprite s in textures) {
			if(s.name == sprite) {
				m_realTexture = s;
				break;
			}
		}
		m_sprite.color = info.color;

	}

	public void ToggleVisibility(bool value) {
		canBeSeen = value;
	}
	
	// Update is called once per frame
	void Update () {
		if(!m_info.alwaysSeen && !canBeSeen) {
			m_sprite.sprite = unknownSprite;
		} else {
			m_sprite.sprite = m_realTexture;
		}
	}
}

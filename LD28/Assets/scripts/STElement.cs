using UnityEngine;
using System.Collections;
using SimpleJSON;

public class STElement : MonoBehaviour {
	private SpriteRenderer m_sprite;
	private STElementInfo m_info;
	// Use this for initialization
	void Awake () {
		m_sprite = GetComponent<SpriteRenderer>();
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
		foreach(Sprite s in textures) {
			if(s.name == sprite) {
				m_sprite.sprite = s;
				break;
			}
		}
		m_sprite.color = info.color;

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

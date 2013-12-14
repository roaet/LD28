using UnityEngine;
using System.Collections;
using SimpleJSON;

public class STElement : MonoBehaviour {
	private SpriteRenderer sprite;
	private STElementInfo m_info;
	// Use this for initialization
	void Awake () {
		sprite = GetComponent<SpriteRenderer>();
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
			if(s.name == sprite) this.sprite.sprite = s;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

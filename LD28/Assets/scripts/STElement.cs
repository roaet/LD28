using UnityEngine;
using System.Collections;
using SimpleJSON;

public class STElement : MonoBehaviour {
	private SpriteRenderer sprite;
	// Use this for initialization
	void Awake () {
		sprite = GetComponent<SpriteRenderer>();
	
	}

	public void ConfigureElement(string sprite) {
		Sprite[] textures = Resources.LoadAll<Sprite>("images/st_elements");
		foreach(Sprite s in textures) {
			if(s.name == sprite) this.sprite.sprite = s;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

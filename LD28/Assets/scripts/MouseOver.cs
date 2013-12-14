using UnityEngine;
using System.Collections;

public class MouseOver : MonoBehaviour {
	private SpriteRenderer sprite;

	public Sprite selected;
	public Sprite normal;
	public Sprite activated;
	public Sprite deactivated;

	void Awake() {
		this.sprite = GetComponent<SpriteRenderer>();
		if(!this.sprite) {
			throw new MissingComponentException("There is no sprite renderer assigned");
		}
	}

	void OnMouseEnter() {
		this.sprite.sprite = this.selected;
		Debug.Log ("Mouse Over");
	}

	void OnMouseExit() {
		this.sprite.sprite = this.normal;
		Debug.Log ("Mouse Out");
	}

}

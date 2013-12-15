using UnityEngine;
using System.Collections;

public class InnReplacer : MonoBehaviour {
	public Inn inn;
	public int index;

	void OnMouseDown() {
		inn.CharacterReplaced(index);
	}

	public void ToggleReplacer(bool value) {
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		sprite.enabled = value;
	}
}

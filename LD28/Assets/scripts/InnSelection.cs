using UnityEngine;
using System.Collections;

public class InnSelection : MonoBehaviour {
	public Inn inn;
	public SpriteRenderer portrait;
	public SpriteRenderer type;

	private CharacterInfo m_info;

	public void ToggleSlot(bool value) {
		portrait.enabled = value;
		type.enabled = value;
		SpriteRenderer sprite = GetComponent<SpriteRenderer>();
		sprite.enabled = value;
	}

	void OnMouseDown() {
		inn.CharacterClicked(m_info);
	}

	public void InitializeSlot(CharacterInfo info) {
		m_info = info;
		ToggleSlot(true);

		ToolTip tip = GetComponent<ToolTip>();
		if(tip != null) {
			tip.text = info.tip;
			tip.isEnabled = true;
		}
		
		Sprite[] textures = Resources.LoadAll<Sprite>("images/portraits");
		string sprite = m_info.portrait;
		foreach(Sprite s in textures) {
			if(s.name == sprite) {
				portrait.sprite = s;
				break;
			}
		}
		textures = Resources.LoadAll<Sprite>("images/portraits");
		sprite = m_info.charClass;
		foreach(Sprite s in textures) {
			if(s.name == sprite) {
				type.sprite = s;
				break;
			}
		}

	}
}


using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public SpriteRenderer portrait;
	public SpriteRenderer characterClass;

	private GUIText m_name;
	private bool m_enabled;
	private Person m_person;

	public void InitializeCharacter(GUIText name, Person person) {
		m_person = person;
		m_person.charUIElement = this;
		m_name = name;
		m_enabled = true;
		m_name.text = m_person.desc;

		Sprite[] textures = Resources.LoadAll<Sprite>("images/portraits");
		string sprite = m_person.info.portrait;
		foreach(Sprite s in textures) {
			if(s.name == sprite) {
				portrait.sprite = s;
				break;
			}
		}
		textures = Resources.LoadAll<Sprite>("images/portraits");
		sprite = m_person.info.charClass;
		foreach(Sprite s in textures) {
			if(s.name == sprite) {
				characterClass.sprite = s;
				break;
			}
		}
	}

	public void AnimateAttack(float time) {
		float totalTime = time;
		int blinks = 3;
		float blinkRate = totalTime / (float)(blinks * 2);
		float current = 0.0f;
		for(int i = 0; i < blinks; i++) {
			iTween.ColorTo(gameObject, iTween.Hash("color", new Color(1.0f, 0.0f, 0.0f),
			                                       "time", blinkRate, "delay", current));
			current += blinkRate;
			iTween.ColorTo(gameObject, iTween.Hash("color", new Color(1.0f, 1.0f, 1.0f),
			                                       "time", blinkRate, "delay", current));
			current += blinkRate;
		}
	}

	public bool IsEnabled {
		get {
			return m_enabled;
		}
		set {
			m_enabled = value;
		}
	}

	// Use this for initialization
	void Start () {
		m_enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		UpdateVisibility();
	}

	private void UpdateVisibility() {
		renderer.enabled = m_enabled;
		if(m_name != null) {
			m_name.enabled = m_enabled;
			m_name.text = m_person.desc;
		}
		portrait.enabled = m_enabled;
		characterClass.enabled = m_enabled;
	}
}

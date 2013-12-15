using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public GUIText name;
	private bool m_enabled;
	private CharacterInfo m_info;

	public void InitializeCharacter(CharacterInfo info) {
		m_info = info;
		m_enabled = true;
		name.text = m_info.name;
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
		if(name != null) {
			name.enabled = m_enabled;
		}
	}
}

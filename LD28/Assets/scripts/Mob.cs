using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour {
	private MobInfo m_info;
	private SpriteRenderer m_sprite;

	private int m_health;

	void Awake() {
		m_sprite = GetComponent<SpriteRenderer>();
	}

	public void LoadInfo(MobInfo info) {
		m_info = info;
		Sprite[] textures = Resources.LoadAll<Sprite>("images/mobs");
		string sprite = m_info.sprite;
		foreach(Sprite s in textures) {
			if(s.name == sprite) {
				m_sprite.sprite = s;
				break;
			}
		}
		//m_sprite.color = info.color;
	}
}

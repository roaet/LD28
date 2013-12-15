using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour {
	public GameObject healthBarBG;
	public GameObject healthBarFG;

	private MobInfo m_info;
	private SpriteRenderer m_sprite;

	private int m_health;
	private int m_totalHealth;
	private bool m_hasHealth;
	private Event m_eventController;

	void Awake() {
		m_sprite = GetComponent<SpriteRenderer>();
	}

	public int damage { get { return m_info.damage; } }

	public int health {
		get {
			return m_health;
		}
	}

	void OnMouseDown() {
		m_eventController.MobClicked(this);
	}

	public void TakeDamage(int damage) {
		if(!m_hasHealth) return;
		m_health -= damage;
		Debug.Log(m_info.name + " took " + damage + " damage (" + m_health + "/" + m_totalHealth + ")");
		if(m_health < 0) m_health = 0;
		if(m_health == 0) {
			m_eventController.MobDying(this);
		}
		UpdateHealthBars();
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

	private void UpdateHealthBars() {
		Vector3 scale = healthBarFG.transform.localScale;
		scale.x = m_health / (float)m_totalHealth * healthBarBG.transform.localScale.x;
		healthBarFG.transform.localScale = scale;
	}

	public void LoadInfo(Event eventController, MobInfo info) {
		m_info = info;
		m_eventController = eventController;
		Sprite[] textures = Resources.LoadAll<Sprite>("images/mobs");
		string sprite = m_info.sprite;
		foreach(Sprite s in textures) {
			if(s.name == sprite) {
				m_sprite.sprite = s;
				break;
			}
		}
		m_sprite.color = info.color;
		m_health = m_totalHealth = info.health;
		m_hasHealth = info.hasHealth;
		if(!m_hasHealth) {
			m_health = m_totalHealth = 1;
			healthBarBG.renderer.enabled = false;
			healthBarFG.renderer.enabled = false;
		}
	}
}

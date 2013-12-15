using UnityEngine;
using System.Collections;

public class Person {

	private CharacterInfo m_info;
	private string m_name;
	private int m_health;
	private int m_currentHealth;
	private Character m_character;

	public Person(CharacterInfo info) {
		m_info = info;
		m_name = info.name;
		m_health = info.health;
		m_currentHealth = m_health;
	}

	public string desc { get { return m_name + "\nHP: "+ m_currentHealth; } } 
	public int totalHealth { get { return m_health; } }
	public int currentHealth { get { return m_currentHealth; } }
	public int damage { get { return m_info.damage; } }
	public Character charUIElement {
		get {
			return m_character;
		} set {
			m_character = value;
		}
	}

	public void Heal(int healFor) {
		m_currentHealth += healFor;
		if(m_currentHealth > m_health)
			m_currentHealth = m_health;
	}

	public void Hurt(int hurtFor) {
		Debug.Log(m_name + " took " + hurtFor + " damage " + m_currentHealth + "/" + m_health);
		m_currentHealth -= hurtFor;
		if(m_currentHealth < 0)
			m_currentHealth = 0;
	}

	public CharacterInfo info {
		get {
			return m_info;
		}
	}

	public string name {
		get {
			return m_name;
		}
	}
}

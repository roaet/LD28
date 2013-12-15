using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Event : MonoBehaviour {

	public GameObject mobPrefab;
	public GameObject spawn1;
	public GameObject spawn2;
	public GameObject spawn3;
	public GameObject spawn1a;
	public GameObject spawn2a;
	public SpriteRenderer targetSprite;

	private Storytrack m_st;
	private List<Mob> m_mobs;
	private EventInfo m_info;

	private Mob selectedMob;

	void Awake() {
		m_mobs = new List<Mob>();
		targetSprite.enabled = false;
	}

	public void MobClicked(Mob mob) {
		if(selectedMob == mob) selectedMob = null;
		else selectedMob = mob;
	}

	public void MobDying(Mob mob) {
		if(selectedMob == mob) selectedMob = null;
	}

	private void UpdateTarget() {
		if(selectedMob == null) {
			targetSprite.enabled = false;
			return;
		}
		targetSprite.enabled = true;
		float height = selectedMob.renderer.bounds.extents.y;
		Vector3 newPosition = selectedMob.transform.position;
		newPosition.y += height;
		targetSprite.transform.position = newPosition;
	}

	void Update() {
		UpdateTarget();
	}

	public void LoadEventInfo(MobManager mobManager, Storytrack storyTrack, EventInfo info) {
		ClearMobs();
		m_st = storyTrack;
		m_info = info;
		foreach(string mobName in m_info.mobs) {
			MobInfo mobInfo = mobManager.GetMobByName(mobName);
			Mob mob = CreateMob(mobInfo, transform.position);
			if(mob == null) continue;
			m_mobs.Add(mob);
		}
		MoveMobsToSpawns();
	}

	public Storytrack story {
		get { return m_st; }
	}

	private void MoveMobsToSpawns() {
		if(m_mobs.Count == 0 || m_info.isInn || m_info.isStore) return;
		// More jank. Fix this
		if(m_mobs.Count == 1) {
			m_mobs[0].transform.position = spawn2.transform.position;	
		}
		if(m_mobs.Count == 2) {
			m_mobs[0].transform.position = spawn1a.transform.position;	
			m_mobs[1].transform.position = spawn2a.transform.position;	
		}
		if(m_mobs.Count == 3) {
			m_mobs[0].transform.position = spawn1.transform.position;	
			m_mobs[1].transform.position = spawn2.transform.position;	
			m_mobs[2].transform.position = spawn3.transform.position;	
		}
	}

	private void ClearMobs() {
		if(m_info == null || m_info.isInn || m_info.isStore) return;
		foreach(Mob mob in m_mobs) {
			Destroy(mob.gameObject);		
		}
		m_mobs.Clear();
	}

	private Mob CreateMob(MobInfo info, Vector3 position) {
		if(m_info.mobs.Count <= 0 || m_info.isInn || m_info.isStore) return null;
		GameObject element = Instantiate(mobPrefab,
		                                 position,
		                                 Quaternion.identity) as GameObject;
		Mob mob = element.GetComponent<Mob>();
		mob.LoadInfo(this, info);
		return mob;
	}

	private struct MobActionData {
		public Level level;
		public CharacterManager chars;

		public MobActionData(Level level, CharacterManager chars) {
			this.level = level;
			this.chars = chars;
		}
	}

	public void HandleMobs(Level level, CharacterManager chars) {
		StartCoroutine("DoMobAction", new MobActionData(level, chars));
	}

	private IEnumerator DoMobAction(MobActionData data) {
		foreach(Mob mob in m_mobs) {
			int damage = mob.damage;
			float time = 0.25f;
			mob.AnimateAttack(time);
			yield return new WaitForSeconds(time);
			DamageAllPlayers(damage, data.chars);
		}
		data.level.EndMobAnimation();
	}

	public void HandleCard(Level level, CardInfo card, CharacterManager chars) {
		StartCoroutine("DoCharActions", new CharActionData(level, card, chars));
	}

	private struct CharActionData {
		public Level level;
		public CardInfo card;
		public CharacterManager chars;

		public CharActionData(Level l, CardInfo c, CharacterManager chars) {
			level = l;
			card = c;
			this.chars = chars;
		}
	}

	private IEnumerator DoCharActions(CharActionData data) {
		bool doMassAttack = data.card.target == "mass";
		foreach(Person p in data.chars.party) {
			if(data.card.type != "basic" && data.card.type != p.info.charClass) continue;
			int damage = p.damage;
			float time = 0.25f;
			p.charUIElement.AnimateAttack(time);
			yield return new WaitForSeconds(time);
			if(doMassAttack) DamageAllMobs(damage);
			else DamageRandomMob(damage);
			int monstersLeft = CheckMobStates(data.level);
			if(monstersLeft == 0) break;
		}
		data.level.EndCharacterAnimation();

	}

	private void DamageAllPlayers(int damage, CharacterManager chars) {
		foreach(Person p in chars.party) {
			p.Hurt(damage);
		}
	}

	private void DamageAllMobs(int damage) {
		foreach(Mob mob in m_mobs) {
			mob.TakeDamage(damage);
		}
	}

	private void DamageRandomMob(int damage) {
		if(selectedMob == null) {
			int idx = Random.Range (0, m_mobs.Count);
			m_mobs[idx].TakeDamage(damage);
		} else selectedMob.TakeDamage(damage);
	}

	public int CheckMobStates(Level level) {
		List<Mob> aliveMobs = new List<Mob>();
		List<Mob> deadMobs = new List<Mob>();
		foreach(Mob mob in m_mobs) {
			if(mob.health > 0) {
				aliveMobs.Add (mob);
			} else {
				deadMobs.Add (mob);
			}
		}
		foreach(Mob mob in deadMobs) {
			level.AddGold(mob.gold);
			Destroy(mob.gameObject);
		}
		deadMobs.Clear();
		m_mobs.Clear();
		m_mobs = aliveMobs;
		return m_mobs.Count;
	}
}

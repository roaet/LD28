using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Event : MonoBehaviour {

	public GameObject mobPrefab;

	private Storytrack m_st;
	private List<Mob> m_mobs;
	private EventInfo m_info;

	void Awake() {
		m_mobs = new List<Mob>();
	}

	public void LoadEventInfo(MobManager mobManager, Storytrack storyTrack, EventInfo info) {
		ClearMobs();
		m_st = storyTrack;
		m_info = info;
		Debug.Log ("Loaded info with " + m_info.mobs.Count + " mobs");
		for(int i = 0; i < m_info.mobs.Count; i++) {
			string mobName = m_info.mobs[i];
			MobInfo mobInfo = mobManager.GetMobByName(mobName);
			Mob mob = CreateMob(mobInfo, transform.position);
			m_mobs.Add(mob);
		}
	}

	private void ClearMobs() {
		foreach(Mob mob in m_mobs) {
			Destroy(mob.gameObject);		
		}
		m_mobs.Clear();
	}

	private Mob CreateMob(MobInfo info, Vector3 position) {
		if(m_info.mobs.Count <= 0) return null;
		m_info.mobs.RemoveAt (0);
		GameObject element = Instantiate(mobPrefab,
		                                 position,
		                                 Quaternion.identity) as GameObject;
		Mob mob = element.GetComponent<Mob>();
		mob.LoadInfo(info);
		return mob;
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.PageDown)) {
			m_st.EventCompleted();
		}
	}
}

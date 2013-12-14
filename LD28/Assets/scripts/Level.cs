using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {
	public Storytrack storyTrack;

	private bool m_debug = true;
	
	void Awake() {
	}

	public void LoadLevel(string level) {
		int loadedTiles = storyTrack.LoadStoryTrackJSON("test");
		if(loadedTiles > 0) {
			storyTrack.Display();
		}
	}

	public bool debug {
		set {
			m_debug = value;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Delete)) {
			storyTrack.PopBottom();
			if(storyTrack.CheckIfSpawnActionAvailable()) {
				storyTrack.SpawnElement();
			}
		}
		if(m_debug && Input.GetKeyDown(KeyCode.Home)) {
			LoadLevel ("test");
		}
	}
}

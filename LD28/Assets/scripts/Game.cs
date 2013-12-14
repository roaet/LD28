using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour {
	public Storytrack storyTrack;
	public GameObject eventElement;

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown (KeyCode.Home)) {
			int loadedTiles = storyTrack.LoadStoryTrackJSON("test");
			if(loadedTiles > 0) {
				storyTrack.Display();
			}
			UpdateEventElement();
		}
		if(Input.GetKeyDown (KeyCode.Delete)) {
			storyTrack.PopBottom();
			if(storyTrack.CheckIfSpawnActionAvailable()) {
				storyTrack.SpawnElement();
			}
			UpdateEventElement();
		}
	}

	private void UpdateEventElement() {
		STElement st = eventElement.GetComponent<STElement>();
		STElementInfo info = storyTrack.GetActiveElement();
		eventElement.renderer.enabled = info != null;
		if(info == null) return;
		st.ConfigureElement(info);
	}
}

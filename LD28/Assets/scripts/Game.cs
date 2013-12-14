using UnityEngine;
using System.Collections;



public class Game : MonoBehaviour {
	[HideInInspector]
	public Level currentLevel;

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.PageUp)) {
			Application.LoadLevel("game");
		}
	}

	void OnLevelWasLoaded() {
		currentLevel = GameObject.FindObjectOfType<Level>();
		if(currentLevel == null) {
			Debug.LogError("Could not find level object in scene");
			return;
		}
		currentLevel.debug = false;
		currentLevel.LoadLevel("test");
	}
}

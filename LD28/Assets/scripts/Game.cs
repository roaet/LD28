using UnityEngine;
using System.Collections;


public class Game : MonoBehaviour {
	[HideInInspector]
	public Level currentLevel;

	public string cardFile = "cards";

	private CardManager m_cardManager;

	void Awake() {
		m_cardManager = new CardManager(cardFile);
		DontDestroyOnLoad(gameObject);
	}

	public CardManager cardManager {
		get {
			return m_cardManager;
		}
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
		currentLevel.game = this;
		currentLevel.debug = false;
		currentLevel.LoadLevel("test");
	}
}

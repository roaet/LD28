using UnityEngine;
using System.Collections;

public class Level : MonoBehaviour {
	public Storytrack storyTrack;
	public PlayerHand playerHand;

	private bool m_debug = true;
	private Game m_game;
	private CardManager m_debugCardManager;
	
	void Awake() {
	}

	public void LoadLevel(string level) {
		int loadedTiles = storyTrack.LoadStoryTrackJSON("test");
		if(loadedTiles > 0) {
			storyTrack.Display();
		}
		if(m_debug) {
			m_debugCardManager = new CardManager("cards");
		}
		playerHand.cardManager = cardManager;
	}

	public bool debug {
		set {
			m_debug = value;
		}
	}

	public CardManager cardManager {
		get {
			if(m_game != null) return m_game.cardManager;
			return m_debugCardManager;
		}
	}

	public Game game {
		get {
			return m_game;
		} set {
			m_game = value;
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

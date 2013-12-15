using UnityEngine;
using System.Collections;

public enum TurnState {
	loading, draw, use, save, store
}

public class Level : MonoBehaviour {
	public Storytrack storyTrack;
	public PlayerHand playerHand;

	private bool m_debug = true;
	private Game m_game;
	private CardManager m_debugCardManager;
	private MobManager m_debugMobManager;
	private TurnState m_state;
	private bool drawing;
	private bool storeQueued;
	
	void Awake() {
		drawing = false;
		storyTrack.level = this;
		storeQueued = false;
	}

	public void LoadLevel(string level) {
		int loadedTiles = storyTrack.LoadStoryTrackJSON("test");
		if(loadedTiles > 0) {
			storyTrack.Display();
		}
		if(m_debug) {
			m_debugCardManager = new CardManager("cards");
			m_debugMobManager = new MobManager("mobs");
		}
		playerHand.cardManager = cardManager;
		playerHand.level = this;
		m_state = TurnState.draw;
	}

	public void QueueStore() {
		storeQueued = true;
	}

	public void BringUpStore() {
		m_state = TurnState.store;
		storeQueued = false;
	}

	public bool CardSelected(CardInHand card) {
		CardInfo cardInfo = card.info;
		if(m_state == TurnState.use) {
			Debug.Log (cardInfo.name + " was used!");
			m_state = TurnState.save;
			storyTrack.eventController.HandleCard(cardInfo);
			storyTrack.CheckEventState();
			
			if(storyTrack.CheckIfSpawnActionAvailable()) {
				storyTrack.SpawnElement();
			}
			return true;
		}
		if(m_state == TurnState.save) {
			Debug.Log (cardInfo.name + " was saved!");
			playerHand.PutCardBackIntoHand(card);
			playerHand.DiscardAllBut(card);
			m_state = TurnState.draw;
			return false;
		}
		return false;
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
	
	public MobManager mobManager {
		get {
			if(m_game != null) return m_game.mobManager;
			return m_debugMobManager;
		}
	}

	public Game game {
		get {
			return m_game;
		} set {
			m_game = value;
		}
	}

	public IEnumerator DoDraw() {
		playerHand.DrawCard();
		yield return new WaitForSeconds(0.5f);
		drawing = false;
	}
	
	// Update is called once per frame
	void Update () {
		if(m_debug && m_state == TurnState.loading) {
			LoadLevel ("test");
		}
		switch(m_state) {
		case TurnState.store: {
			if(Input.GetKeyDown(KeyCode.Escape)) {
				storyTrack.EventCompleted();
				if(storyTrack.CheckIfSpawnActionAvailable()) {
					storyTrack.SpawnElement();
				}
				m_state = TurnState.draw;
			}
		} break;
		case TurnState.draw: { // auto draw to max hand size (3)
			if(!drawing && playerHand.Handsize < 3) {
				drawing = true;
				StartCoroutine("DoDraw");
			}
			if(playerHand.Handsize >= 3) {
				if(storeQueued) {
					BringUpStore();
				} else {
					m_state = TurnState.use;
				}
			}
		} break;
		case TurnState.use: { // wait for player to select 1 for use

		} break;
		case TurnState.save: { // wait for player to select 1 for save and discard
			
		} break;
		}
	}
}

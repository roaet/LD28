using UnityEngine;
using System.Collections;

public enum TurnState {
	loading, draw, use, useAnimate, save, store, mobAnimate, characterAnimate, partyWipe, inn, win
}

public class Level : MonoBehaviour {
	public Storytrack storyTrack;
	public PlayerHand playerHand;
	public CharacterUI charController;
	public GameObject saveGuide;
	public GameObject useGuide;
	public GUIText goldText;
	public Store store;
	public Inn inn;
	public int potionCost = 2;
	public int potionPower = 4;
	public SpriteRenderer slain;
	public SpriteRenderer win;

	private bool m_debug = true;
	private Game m_game;
	private CardManager m_debugCardManager;
	private MobManager m_debugMobManager;
	private CharacterManager m_debugCharManager;
	private TurnState m_state;
	private bool drawing;
	private bool storeQueued;
	private int m_gold;
	private bool innQueued;
	private bool gameWon;
	
	void Awake() {
		drawing = false;
		storyTrack.level = this;
		innQueued = storeQueued = false;
		saveGuide.renderer.enabled = false;
		useGuide.renderer.enabled = false;
		slain.renderer.enabled = false;
		gameWon = false;
		m_gold = 0;
	}

	public void RecruitCharacter(CharacterInfo info) {
		Person p = new Person(info);
		characterManager.AddPersonToParty(p);
	}

	
	
	public void ReplaceCharacter(int index, CharacterInfo info) {
		Person p = new Person(info);
		characterManager.AddPersonToPartyAt(p, index);
	}

	public void UsePotion() {
		if(m_gold < potionCost || characterManager.IsPartyFullHealth()) {
			return;
		}
		UseGold(potionCost);
		characterManager.HealPartyFor(potionPower);
	}

	public void CardBuyAttempt(CardInfo card) {
		if(m_gold < card.cost) {
			return;
		}
		UseGold (card.cost);
		cardManager.deck.PutCardIntoDiscard(card);
	}

	public int gold {
		get {
			return m_gold;
		}
	}

	public void AddGold(int gold) {
		Debug.Log ("Gained " + gold + " gold");
		m_gold += gold;
	}

	public void UseGold(int gold) {
		m_gold -= gold;
		if(m_gold < 0) m_gold = 0;
	}

	public void ResetAllTheThings() {
		storyTrack.ResetAllTheThings();
	}

	public void LoadLevel(string level) {
		ResetAllTheThings();
		int loadedTiles = storyTrack.LoadStoryTrackJSON(level);
		if(loadedTiles > 0) {
			storyTrack.Display();
		}
		if(m_debug) {
			m_debugCardManager = new CardManager("cards");
			m_debugMobManager = new MobManager("mobs");
			m_debugCharManager = new CharacterManager("characters");
		}

		CharacterInfo info = characterManager.GetCharacterByName("squire");
		Person p = new Person(info);
		characterManager.AddPersonToParty(p);

		playerHand.cardManager = cardManager;
		playerHand.level = this;
		m_state = TurnState.draw;
	}

	public bool CardSelected(CardInHand card) {
		CardInfo cardInfo = card.info;
		if(m_state == TurnState.use) {
			Debug.Log (cardInfo.name + " was used!");
			playerHand.AnimateCardUse(card);
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
	
	public CharacterManager characterManager {
		get {
			if(m_game != null) return m_game.characterManager;
			return m_debugCharManager;
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

	public void StartCardAnimation() {
		m_state = TurnState.useAnimate;
	}

	public void EndCardAnimation(CardInHand card) {
		CardInfo cardInfo = card.info;
		m_state = TurnState.characterAnimate;
		storyTrack.eventController.HandleCard(this, cardInfo, characterManager);
	}

	public void EndCharacterAnimation() {
		bool completed = storyTrack.CheckEventState();
		if(completed) {
			m_state = TurnState.save;
			if(storyTrack.CheckIfSpawnActionAvailable()) {
				storyTrack.SpawnElement();
			}
			return;
		}
		m_state = TurnState.mobAnimate;
		storyTrack.eventController.HandleMobs(this, characterManager);
	}

	private bool CheckPartyState() {
		return characterManager.PartyAlive();
	}

	public void EndMobAnimation() {
		bool partyStillAlive = CheckPartyState();
		if(partyStillAlive) {
			m_state = TurnState.save;
		} else {
			m_state = TurnState.partyWipe;
			slain.renderer.enabled = true;
			Debug.Log ("Party was wiped.. do soemthing");
		}
	}


	private void UpdateGuides() {
		saveGuide.renderer.enabled = m_state == TurnState.save;
		useGuide.renderer.enabled = m_state == TurnState.use;
		playerHand.isEnabled = m_state == TurnState.save || m_state == TurnState.use;

		goldText.text = gold.ToString();
	}

	public void QueueInn() {
		innQueued = true;
	}

	public void BringUpInn() {
		m_state = TurnState.inn;
		innQueued = false;
		Vector3 curPosition = inn.transform.position;
		curPosition.y = 0.1f;
		inn.InitializeInn(storyTrack.GetActiveElement().eventInfo);
		iTween.MoveTo (inn.gameObject, iTween.Hash("position", curPosition,
		                                             "easetype", iTween.EaseType.easeOutBounce,
		                                             "oncompletetarget", this.gameObject,
		                                             "oncomplete", "InnOpened"));
	}

	public void InnOpened() {

	}
	
	public void InnClosed() {
		storyTrack.EventCompleted();
		if(storyTrack.CheckIfSpawnActionAvailable()) {
			storyTrack.SpawnElement();
		}
		m_state = TurnState.draw;
	}
	
	public void CloseInn() {
		Vector3 curPosition = inn.transform.position;
		curPosition.y = 10.0f;
		iTween.MoveTo (inn.gameObject, iTween.Hash("position", curPosition,
		                                           "oncompletetarget", this.gameObject,
		                                           "oncomplete", "InnClosed"));
	}
	
	
	public void QueueStore() {
		storeQueued = true;
	}
	
	public void BringUpStore() {
		m_state = TurnState.store;
		storeQueued = false;
		Vector3 curPosition = store.transform.position;
		curPosition.y = 0.1f;
		store.InitializeStore();
		iTween.MoveTo (store.gameObject, iTween.Hash("position", curPosition,
		                                             "easetype", iTween.EaseType.easeOutBounce,
		                                             "oncompletetarget", this.gameObject,
		                                             "oncomplete", "StoreOpened"));
	}

	public void StoreOpened() {
		Debug.Log ("In da store");
		store.isStoreUp = true;
	}

	public void StoreClosed() {
		storyTrack.EventCompleted();
		if(storyTrack.CheckIfSpawnActionAvailable()) {
			storyTrack.SpawnElement();
		}
		m_state = TurnState.draw;
	}

	public void CloseStore() {
		store.isStoreUp = false;
		Vector3 curPosition = store.transform.position;
		curPosition.y = 7.0f;
		iTween.MoveTo (store.gameObject, iTween.Hash("position", curPosition,
		                                             "oncompletetarget", this.gameObject,
		                                             "oncomplete", "StoreClosed"));
	}

	public void ExitReached() {
		Debug.Log ("Win");
		m_state = TurnState.win;
		win.renderer.enabled = true;
		gameWon = true;
	}
	// Update is called once per frame 
	void Update () {
		if(gameWon || m_state == TurnState.partyWipe || m_state == TurnState.win) {
			if(Input.anyKeyDown || Input.GetMouseButtonDown(0)) {
				Destroy(m_game.gameObject);
				Application.LoadLevel("initialscene");
			}
			return;
		}
		UpdateGuides();
		if(m_debug && m_state == TurnState.loading) {
			LoadLevel ("test");
		}
		if(m_state != TurnState.loading) {
			storyTrack.AdjustVisibility(characterManager.GetPartyVision());
		}
		switch(m_state) {
		case TurnState.store: {
		} break;
		case TurnState.draw: { // auto draw to max hand size (3)
			charController.UpdateParty(characterManager);
			if(!drawing && playerHand.Handsize < 3) {
				drawing = true;
				StartCoroutine("DoDraw");
			}
			if(playerHand.Handsize >= 3) {
				if(storeQueued) {
					BringUpStore();
				} else if(innQueued) {
					BringUpInn();
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

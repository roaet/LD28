using UnityEngine;
using System.Collections;


public class Game : MonoBehaviour {
	[HideInInspector]
	public Level currentLevel;

	public string cardFile = "cards";
	public string mobFile = "mobs";
	public string characterFile = "characters";

	private CardManager m_cardManager;
	private MobManager m_mobManager;
	private CharacterManager m_characterManager;

	void Awake() {
		DontDestroyOnLoad(gameObject);
	}

	public CardManager cardManager {
		get {
			return m_cardManager;
		}
	}
	
	public MobManager mobManager{
		get {
			return m_mobManager;
		}
	}
	
	public CharacterManager characterManager{
		get {
			return m_characterManager;
		}
	}


	public void LoadGame() {
		m_cardManager = new CardManager(cardFile);
		m_mobManager = new MobManager(mobFile);
		m_characterManager = new CharacterManager(characterFile);
		Application.LoadLevel("game");
	}

	void OnLevelWasLoaded() {
		currentLevel = GameObject.FindObjectOfType<Level>();
		if(currentLevel == null) {
			return;
		}
		currentLevel.game = this;
		currentLevel.debug = false;
		currentLevel.LoadLevel("test");
	}
}

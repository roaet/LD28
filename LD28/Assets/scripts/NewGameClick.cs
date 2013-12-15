using UnityEngine;
using System.Collections;

public class NewGameClick : MonoBehaviour {

	public Game game;

	void OnMouseDown() {
		game.LoadGame();
	}
}

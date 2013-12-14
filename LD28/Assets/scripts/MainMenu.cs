using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Texture logo;
	public Texture playButton;
	public Texture optionsButton;

	void OnGUI() {
		if(!logo || !playButton || !optionsButton) {
			Debug.LogError("Required texture not assigned");
			return;
		}
		if(GUI.Button(new Rect(10, 50, 500, 100), logo)) {

		}
	}
}

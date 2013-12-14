using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class storytrack : MonoBehaviour {

	public GameObject st_element;
	public GameObject spawn_point;

	public float insertPushUpForce = 1.0f;
	public int bottomCheck = 3;
	public int topCheck = 5;

	public string levelFilename = "levels";

	private List<GameObject> elements;
	private List<STElementInfo> infos;

	// Test variables
	int colorModulo;
	
	// Use this for initialization
	void Start () {
		elements = new List<GameObject>();
		infos = new List<STElementInfo>();
	}

	public bool LoadStoryTrackJSON(string trackName) {
		infos.Clear();
		TextAsset json = (TextAsset)Resources.Load(levelFilename, typeof(TextAsset));
		if(!json) {
			Debug.LogError("Couldn't find: " + levelFilename);
			return false;
		}
		string content = json.text;
		JSONNode track = JSON.Parse(content);
		int tileCount = track["tileCount"].AsInt;
		for(int j = 0; j < tileCount; j++) {
			string spriteName = track["tiles"][j]["sprite"];
			JSONNode color = track["tiles"][j]["color"];
			Color col = Color.white;
			if(color != null) {
				col = new Color(color["r"].AsFloat, color["g"].AsFloat, color["b"].AsFloat);
			}
			STElementInfo element = new STElementInfo(spriteName, col);
			infos.Add(element);
		}

		return true;
	}

	private void CreateTwist(string sprite) {
		STElementInfo info = new STElementInfo(sprite);
		infos.Insert (0, info);
	}

	private Color TestRandomColor() {
		int choice = (colorModulo++) % 4;
		switch(choice) {
		case 0:
			return Color.red;
		case 1:
			return Color.blue;
		case 2:
			return Color.green;
		case 3:
			return Color.gray;
		}
		return new Color(1.0f, 1.0f, 1.0f);
	}

	// Always get the STElementInfo from the front of the list
	private GameObject CreateElement(Vector3 position) {
		if(infos.Count <= 0) return null;
		STElementInfo info = infos[0]; 
		infos.RemoveAt (0);
		GameObject element = Instantiate(st_element,
		                                 position,
		                                 Quaternion.identity) as GameObject;
		SpriteRenderer sprite = element.GetComponent<SpriteRenderer>();
		sprite.color = info.color;
		STElement st = element.GetComponent<STElement>();
		st.ConfigureElement(info.sprite);
		return element;
	}


	// A vast majority of our insert methods 'act funny' if the rigid bodies 
	// aren't sleeping near the bottom
	private bool CheckIfInsertActionAvailable() {
		for(int i = 0; i < bottomCheck && i < elements.Count; i++) {
			GameObject element = elements[i];
			if(!element.rigidbody2D.IsSleeping()) return false;
		}
		return true;
	}

	private bool CheckIfSpawnActionAvailable() {
		if(elements.Count > topCheck) return false;
		return true;
	}
	
	public bool SpawnElement() {
		if(!CheckIfSpawnActionAvailable()) return false;
		GameObject element = CreateElement(spawn_point.transform.position);
		if(!element) return false;
		elements.Add(element);
		return true;
	}

	public bool InsertElementAfterFront() {
		if(!CheckIfInsertActionAvailable()) return false;	
		for(int i = 0; i < elements.Count; i++) {
			if(i == 0) continue;
			Rigidbody2D r2d = elements[i].rigidbody2D;
			r2d.AddForce (Vector2.up * insertPushUpForce);

		}
		BoxCollider2D col = st_element.GetComponent<BoxCollider2D>();
		Vector3 spawnPosition = elements[0].transform.position;
		spawnPosition.y += col.size.y;
		GameObject element = CreateElement(spawn_point.transform.position);
		elements.Insert(1, element);

		return true;

	}

	public void PopBottom() {
		if(elements.Count == 0) return;
		GameObject element = elements[0];
		Destroy (element);
		elements.RemoveAt(0);
	}

	public void ClearElements() {
		foreach(GameObject element in elements) {
			Destroy(element);
		}
		elements.Clear();
	}

	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha0)) {
			SpawnElement();
		}
		/*
		if(Input.GetKeyDown (KeyCode.Alpha9)) {
			InsertElementAfterFront();
		}
		*/
		if(Input.GetKeyDown (KeyCode.Backspace) && !Input.GetKey (KeyCode.LeftShift)) {
			PopBottom();
		}
		if(Input.GetKeyDown(KeyCode.Backspace) && Input.GetKey(KeyCode.LeftShift)) {
			ClearElements();
		}
		if(Input.GetKeyDown (KeyCode.Alpha7)) {
			ClearElements();
			LoadStoryTrackJSON("test");
		}
	
	}
}

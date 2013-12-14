using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class Storytrack : MonoBehaviour {

	public GameObject st_element;
	public GameObject spawn_point;

	public float insertPushUpForce = 1.0f;
	public int bottomCheck = 3;
	public int topCheck = 5;

	public string levelFilename = "levels";

	private List<STElement> elements;
	private List<STElementInfo> infos;

	// Test variables
	int colorModulo;
	
	// Use this for initialization
	void Start () {
		elements = new List<STElement>();
		infos = new List<STElementInfo>();
	}

	public int LoadStoryTrackJSON(string trackName) {
		infos.Clear();
		ClearElements();
		TextAsset json = (TextAsset)Resources.Load(levelFilename, typeof(TextAsset));
		if(!json) {
			Debug.LogError("Couldn't find: " + levelFilename);
			return 0;
		}
		string content = json.text;
		JSONNode track = JSON.Parse(content);
		int tileCount = track["tileCount"].AsInt;
		int loadedTiles = 0;
		for(int j = 0; j < tileCount; j++, loadedTiles++) {
			string spriteName = track["tiles"][j]["sprite"];
			JSONNode color = track["tiles"][j]["color"];
			Color col = Color.white;
			if(color != null) {
				col = new Color(color["r"].AsFloat, color["g"].AsFloat, color["b"].AsFloat);
			}
			STElementInfo element = new STElementInfo(spriteName, col);
			infos.Add(element);
		}
		return loadedTiles;
	}

	public void Display() {
		StartCoroutine("InitialLoad");
	}

	public bool CheckIfSpawnActionAvailable() {
		if(elements.Count > topCheck) return false;
		if(infos.Count == 0) return false;
		return true;
	}
	
	public bool SpawnElement() {
		if(!CheckIfSpawnActionAvailable()) return false;
		STElement element = CreateElement(spawn_point.transform.position);
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
		STElement element = CreateElement(spawn_point.transform.position);
		elements.Insert(1, element);

		return true;

	}

	public STElementInfo GetActiveElement() {
		if(elements.Count == 0) return null;
		return elements[0].info;
	}

	public void PopBottom() {
		if(elements.Count == 0) return;
		STElement element = elements[0];
		Destroy (element.gameObject);
		elements.RemoveAt(0);
	}

	public void ClearElements() {
		foreach(STElement element in elements) {
			Destroy(element.gameObject);
		}
		elements.Clear();
	}
	
	private IEnumerator InitialLoad() {
		while(CheckIfSpawnActionAvailable()) {
			SpawnElement();
			yield return new WaitForSeconds(0.5f);
		}
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
	private STElement CreateElement(Vector3 position) {
		if(infos.Count <= 0) return null;
		STElementInfo info = infos[0]; 
		infos.RemoveAt (0);
		GameObject element = Instantiate(st_element,
		                                 position,
		                                 Quaternion.identity) as GameObject;
		SpriteRenderer sprite = element.GetComponent<SpriteRenderer>();
		sprite.color = info.color;
		STElement st = element.GetComponent<STElement>();
		st.ConfigureElement(info);
		return st;
	}
	
	
	// A vast majority of our insert methods 'act funny' if the rigid bodies 
	// aren't sleeping near the bottom
	private bool CheckIfInsertActionAvailable() {
		for(int i = 0; i < bottomCheck && i < elements.Count; i++) {
			STElement element = elements[i];
			if(!element.rigidbody2D.IsSleeping()) return false;
		}
		return true;
	}
}

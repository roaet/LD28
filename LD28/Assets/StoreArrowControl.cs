using UnityEngine;
using System.Collections;

public class StoreArrowControl : MonoBehaviour {
	public Store store;
	public bool left;

	void OnMouseDown() {
		store.ScrollEvent(left);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

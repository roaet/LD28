using UnityEngine;
using System.Collections;

public class ExitStore : MonoBehaviour {

	public Store store;

	// Use this for initialization
	void OnMouseDown() {
		store.StoreClosed();
	}
}

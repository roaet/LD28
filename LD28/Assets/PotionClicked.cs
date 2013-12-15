using UnityEngine;
using System.Collections;

public class PotionClicked : MonoBehaviour {

	public Store store;

	void OnMouseDown() {
		store.PotionClicked();
	}
}

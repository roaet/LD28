using UnityEngine;
using System.Collections;

public class SimpleButton : MonoBehaviour {

	public CardConfirm confirm;
	public bool value;

	void OnMouseUp() {
		confirm.ButtonClick(value);
	}
}

using UnityEngine;
using System.Collections;

public class ExitInnButton : MonoBehaviour {
	public Inn inn;

	void OnMouseDown() {
		inn.InnClosed();
	}
}

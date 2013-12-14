using UnityEngine;
using System.Collections;

public class CardConfirm : MonoBehaviour {
	[HideInInspector]
	public PlayerHand hand;

	public void ButtonClick(bool value) {
		if(!value) {
			hand.CancelSelection();
		} else hand.ConfirmSelection();
	}
}

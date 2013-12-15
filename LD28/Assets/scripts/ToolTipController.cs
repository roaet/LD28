using UnityEngine;
using System.Collections;

public class ToolTipController : MonoBehaviour {
	public LayerMask mask;
	public GUIText toolText;

	private ToolTip currentTip;

	// Use this for initialization
	void Start () {
		currentTip = null;
	}
	
	// Update is called once per frame
	void Update () {
		if(currentTip != null) {
			toolText.text = currentTip.text;
		} else {
			toolText.text = "";
		}
		currentTip = null;
		RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition),
		                                     Vector2.zero,
		                                     float.PositiveInfinity, mask, -100.0f );
		if(hit.collider != null) {
			GameObject objectHit = hit.collider.gameObject;
			SpriteRenderer spriteRenderer = objectHit.GetComponent<SpriteRenderer>();
			if(spriteRenderer == null) return;
			if(spriteRenderer.enabled) {
				currentTip = objectHit.GetComponent<ToolTip>();
			}
		}

	}
}

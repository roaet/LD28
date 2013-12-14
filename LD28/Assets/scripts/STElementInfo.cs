using UnityEngine;
using System.Collections;

public class STElementInfo {
	private string spriteName;
	private Color spriteColor;

	public STElementInfo(string spriteName) {
		this.spriteName = spriteName;
		this.spriteColor = Color.white;
	}

	public STElementInfo(string spriteName, Color color) {
		this.spriteName = spriteName;
		this.spriteColor = color;
	}

	public string sprite {
		get {
			return spriteName;
		}
	}

	public Color color {
		get {
			return spriteColor;
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

//Head to fontawesome.io/icons/ to get unicode values for symbols

[ExecuteInEditMode]
public class FA_Glyph : MonoBehaviour {

	Text t;

	public string unicode = "f047";

	void Start () {
		t = GetComponent<Text>();
		char num = (char)int.Parse(unicode, System.Globalization.NumberStyles.HexNumber);
		t.text = num.ToString();
	}

	void OnValidate() {
		t = GetComponent<Text>();
		char num = (char)int.Parse(unicode, System.Globalization.NumberStyles.HexNumber);
		t.text = num.ToString();
	}
}

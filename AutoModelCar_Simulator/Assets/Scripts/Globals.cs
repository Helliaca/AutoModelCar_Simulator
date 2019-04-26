using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Globals : MonoBehaviour {

	public static Globals Instance { get; private set; }

	public DevConsoleController DevConsole;

	void Awake()
	{
		if(Instance != null) GameObject.Destroy(Instance);
		else Instance = this;

		DontDestroyOnLoad(this);
	}
		
	void Start () {
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.Comma)) DevConsole.toggle();
	}
}

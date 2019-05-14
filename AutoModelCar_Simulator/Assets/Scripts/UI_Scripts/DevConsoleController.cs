using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DevConsoleController : MonoBehaviour {
	private InputField input;
	private Text output;
	private string output_s;
	private GameObject consoleContainer;

	void Awake()
	{
		DontDestroyOnLoad(transform);
	}

	void Start () {
		Globals.Instance.DevConsole = this;
		consoleContainer = transform.Find("DevConsole").gameObject;
		output = transform.Find("DevConsole/Output").GetComponent<Text>();
		input = transform.Find("DevConsole/Input").GetComponent<InputField>();
		output_s = output.text;
		consoleContainer.SetActive(false);
	}

	void OnGUI() {
		if(input.isFocused && input.text != "" && Input.GetKey(KeyCode.Return)) {
			StartCoroutine(execute_sync(input.text));
			input.text = "";
		}
		output.text = output_s;
	}

	// Executing a command in a OnGUI-Loop leads to assertion errors. This co-routine will sync it up with the Update-Loop
	private IEnumerator execute_sync(string command) {
		yield return new WaitForEndOfFrame();
		execute(command);
	}

	public void execute(string command) {
		string[] cmds = command.Split(' ');

		if(cmds[0] == "hi") {
			print("whats up");
		}
		if(cmds[0] == "toggle") {
			GameObject obj = (GameObject) Globals.GlobalFind(cmds[1], typeof(GameObject));
			obj.SetActive(!obj.activeSelf);
		}
		if(cmds[0] == "toggleHUD") {
			GraphController[] objs = (GraphController[])Resources.FindObjectsOfTypeAll(typeof(GraphController));
			foreach(GraphController g in objs) { g.gameObject.SetActive(!g.gameObject.activeSelf); }
		}
		else {
			print("Command not recognized: " + command);
		}
		input.Select();
		input.ActivateInputField();
	}

	public void print(string s) {
		Debug.Log("CONSOLE:" + s);
		output_s += "\n> " + s;
	}

	public void show() {
		consoleContainer.SetActive(true);
		input.Select();
		input.ActivateInputField();
	}

	public void hide() {
		consoleContainer.SetActive(false);
	}

	public void toggle() {
		if(!consoleContainer.activeInHierarchy) show();
		else hide();
	}
}

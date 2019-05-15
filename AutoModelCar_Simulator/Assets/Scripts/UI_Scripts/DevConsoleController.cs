using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DevConsoleController : MonoBehaviour {
	public InputField input;
	public Text output;
	private string output_s;
	public GameObject consoleContainer;
	public ScrollRect output_scrollrect;

	void Awake()
	{
		DontDestroyOnLoad(transform);
	}

	void Start () {
		Globals.Instance.DevConsole = this;
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
		mirror(command);
		string[] cmds = command.Split(' ');

		if(cmds[0] == "hi") {
			print("whats up");
		}
		else if(cmds[0] == "clear") {
			output_s = "";
			print("Output cleared");
		}
		else if(cmds[0] == "fill") {
			for(int i=0; i<100; i++) print("Fill");
		}
		else if(cmds[0] == "toggle" && cmds[1]=="HUD") {
			GameObject HUDcanvas = GameObject.Find("HUDCanvas");
			foreach (Transform child in HUDcanvas.transform)
			{
				child.gameObject.SetActive(!child.gameObject.activeSelf);
				print(child.gameObject.name + " enabled: " + child.gameObject.activeSelf);
			}
		}
		else if(cmds[0] == "toggle" && cmds[1]=="Circle") {
			Globals.Instance.CircleDraw.gameObject.SetActive(!Globals.Instance.CircleDraw.gameObject.activeSelf);
			Globals.Instance.c_marker.gameObject.SetActive(!Globals.Instance.c_marker.gameObject.activeSelf);
			print("Circle toggled");
		}
		else if(cmds[0] == "toggle") {
			GameObject obj = (GameObject) Globals.GlobalFind(cmds[1], typeof(GameObject));
			obj.SetActive(!obj.activeSelf);
			print(obj.name + " enabled: " + obj.activeSelf);
		}
		else if(cmds[0] == "set_speed") {
			Globals.Instance.CurrentCar.speed = float.Parse(cmds[1]);
			print("Setting speed to " + cmds[1]);
		}
		else if(cmds[0] == "set_steering") {
			Globals.Instance.CurrentCar.steering = float.Parse(cmds[1]);
			print("Setting steering to " + cmds[1]);
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

	public void mirror(string s) {
		output_s += "\n# <color=green><b>" + s + "</b></color>";
	}

	public void error(string s) {
		output_s += "\n! <color=red>" + s + "</color>";
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

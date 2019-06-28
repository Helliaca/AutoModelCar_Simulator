using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

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

		if(cmds[0] == "clear") {
			output_s = "";
			print("Output cleared");
		}
		else if(cmds[0] == "fill") {
			for(int i=0; i<100; i++) print("Fill"); //Uset for testing DevConsole ScrollRect
		}
		else if(cmds[0] == "toggle" && cmds[1]=="HUD") {
			GameObject HUDcanvas = GameObject.Find("HUDCanvas");
			foreach (Transform child in HUDcanvas.transform)
			{
				child.gameObject.SetActive(!child.gameObject.activeSelf);
				print(child.gameObject.name + " enabled: " + child.gameObject.activeSelf);
			}
		}
		else if(cmds[0] == "HUD" && cmds[1]=="off") {
			GameObject HUDcanvas = GameObject.Find("HUDCanvas");
			foreach (Transform child in HUDcanvas.transform)
			{
				child.gameObject.SetActive(false);
				print(child.gameObject.name + " enabled: " + child.gameObject.activeSelf);
			}
		}
		else if(cmds[0] == "toggle" && cmds[1]=="Circle") {
			execute("toggle CircleRenderer");
		}
		else if(cmds[0] == "toggle" && cmds[1]=="lidar_spheres") {
			Globals.Instance.lsv_spheres.enabled = !Globals.Instance.lsv_spheres.enabled;
			print("Lidar Sphere Visualization toggled");
		}
		else if(cmds[0] == "toggle" && cmds[1]=="lidar_lines") {
			Globals.Instance.lsv_lines.enabled = !Globals.Instance.lsv_lines.enabled;
			print("Lidar Line Visualization toggled");
		}
		else if(cmds[0] == "toggle" && cmds[1]=="lidar_mesh") {
			Globals.Instance.lsv_mesh.enabled = !Globals.Instance.lsv_mesh.enabled;
			print("Lidar Mesh Visualization toggled");
		}
		else if(cmds[0] == "toggle") {
			GameObject obj = (GameObject) Globals.GlobalFind(cmds[1], typeof(GameObject));
			obj.SetActive(!obj.activeSelf);
			print(obj.name + " enabled: " + obj.activeSelf);
		}
		else if(cmds[0] == "set_speed") {
			Globals.Instance.CurrentCar.backAxle.speed_topic = float.Parse(cmds[1]);
			print("Setting speed to " + cmds[1]);
		}
		else if(cmds[0] == "set_steering") {
			Globals.Instance.CurrentCar.frontAxle.steering_topic = float.Parse(cmds[1]);
			print("Setting steering to " + cmds[1]);
		}
		else if(cmds[0] == "load" && cmds[1]== "scene") {
			SceneManager.LoadScene("scene_simple", LoadSceneMode.Single);
			Globals.Instance.PorpList.refresh_props();
		}
		else if(cmds[0] == "load" && cmds[1]== "scene_complex") {
			SceneManager.LoadScene("scene_complex", LoadSceneMode.Single);
			Globals.Instance.PorpList.refresh_props();
		}
		else if(cmds[0] == "load" && cmds[1]== "scene_min") {
			SceneManager.LoadScene("scene_lab_min", LoadSceneMode.Single);
			Globals.Instance.PorpList.refresh_props();
		}
		else if(cmds[0] == "rst") {
			execute("HUD off");
			Globals.Instance.lsv_spheres.enabled = false;
			Globals.Instance.lsv_mesh.enabled = false;
			Globals.Instance.lsv_lines.enabled = false;
		}
		else if(cmds[0] == "HUD" && cmds[1] == "basic") {
			execute("toggle FPS_Graph");
			execute("toggle Speed_Graph");
			execute("toggle Steering_Graph");
			execute("toggle Location_Graph");
			execute("toggle Ack_Graph");
		}
		else if(cmds[0]=="pause") {
			Time.timeScale = 0.0f;
		}
		else if(cmds[0]=="unpause") {
			Time.timeScale = 1.0f;
		}
		else if(cmds[0]=="scc") { //Set current car
			Globals.Instance.CurrentCar = (CarController)Globals.GlobalFind(cmds[1], typeof(CarController));
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
		output_s += "\n! <color=red><b>ERR:</b> " + s + "</color>";
	}

	public void warn(string s) {
		output_s += "\n! <color=yellow><b>WRN:</b> " + s + "</color>";
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

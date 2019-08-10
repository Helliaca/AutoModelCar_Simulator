using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Globalization;
using UnityEngine.SceneManagement;

/*
 * The attributes and methods included in this class are globally accessible by any other script via the 'Globals.Instance' variable.
 */
public class Globals : MonoBehaviour {

	public static Globals Instance { get; private set; }

	// RosConnector used for publishing and subscribing all of the application's topics.
	public RosSharp.RosBridgeClient.RosConnector Connection;
	// Reference to the developer's console.
	public DevConsoleController DevConsole;
	// Reference to the car that was last selected.
	public CarController CurrentCar;
	// Reference to the Prop that was last selected
	public UIListEntry_Prop Selected_Prop;
	// Reference to the Camera drawing most of the UI
	public Camera UI_GRAPH_CAMERA;
	// Reference to the current list of components
	public UIListController ComponentList;
	// Reference to the current list of props
	public UIListController PorpList;
	// Reference to UI Panel responsible for spawning in new props
	public NewPropMenuController PropSpawner;
	// Reference to the UI ContextPanel / Inspector window
	public ContextPanelController ContextPanel;
	// Reference to the 3D-axis object
	public SpaceHandleController spaceHandle;
	// Reference to lidar-line-visualization tool
	public LaserScanVisualizerLines lsv_lines;
	// Reference to lidar-mesh-visualization tool
    public LaserScanVisualizerMesh lsv_mesh;
	// Reference to lidar-spheres-visualization tool
    public RosSharp.SensorVisualization.LaserScanVisualizerSpheres lsv_spheres;

	// Prefab of GPS Component
	public GameObject GPS_prefab;
	// Prefab of LaserScanner / Lidar Component
    public GameObject LaserScanner_prefab;
	// Prefab of Camera Component
    public GameObject Camera_prefab;
	// Prefab of CollisionDetection Component
	public GameObject CollisionDetection_prefab;

	public TextAsset settings_file;

	//{ID}, {NAME} and {TYPE} are permitted
	private Dictionary<string, string> settings = new Dictionary<string, string>(){};
	private AsyncOperation asyncSceneLoader;

	void Awake()
	{
		if(Instance != null) GameObject.Destroy(Instance);
		else Instance = this;

		DontDestroyOnLoad(this);
		read_settings();
	}

	private static void read_settings() {
		string text;
		try {
			text = System.IO.File.ReadAllText("UserSettings\\settings.txt");
		}
		catch(System.IO.FileNotFoundException e) {
			Instance.DevConsole.error(e.FileName + " could not be found! Initializing keys from memory.");
			text = Instance.settings_file.text;
		}

		string[] linesInFile = text.Split('\n');
	
		foreach (string line in linesInFile)
		{
			if(line.Length<1 || line[0]=='#' || !line.Contains("=")) continue;
			string[] entry = line.Split('=');
			Instance.settings.Add(entry[0].Trim(), entry[1].Trim());
		}
	}

	public string get_setting(string id) {
		if(settings.ContainsKey(id)) return settings[id];
		else {
			Instance.DevConsole.error("Settings key \"" + id + "\" does not exist.");
			return "0";
		}
	}

	public bool has_setting(string id) {
		if(settings.ContainsKey(id)) return true;
		return false;
	}
		
	void Start () {
		if(get_setting("OnStart_Load_Scene")=="true") {
			if(!has_setting("OnStart_Default_Scene")) {
				Instance.DevConsole.warn("OnStart_Default_Scene is not set.");
			}
			Instance.load_scene(get_setting("OnStart_Default_Scene"));
		}
		StartCoroutine(onstart_spawn_car());
	}

	private IEnumerator onstart_spawn_car() {
		yield return new WaitUntil(() => asyncSceneLoader.isDone);
		if(get_setting("OnStart_Spawn_Car")=="true") {
			float xpos = float.Parse(get_setting("OnStart_Car_SpawnLocation_X"), CultureInfo.InvariantCulture);
			float zpos = float.Parse(get_setting("OnStart_Car_SpawnLocation_Z"), CultureInfo.InvariantCulture);
			spaceHandle.handleObject.position = new Vector3(xpos, 0.0f, zpos);
			if(get_setting("OnStart_Car_Type")=="max") PropSpawner.spawn_max_car();
			else if(get_setting("OnStart_Car_Type")=="min") PropSpawner.spawn_min_car();
			else if(get_setting("OnStart_Car_Type")=="cless") PropSpawner.spawn_cless_car();
			else PropSpawner.spawn_max_car();
		}
	}

	void Update () {
		if(Input.GetKeyDown(KeyCode.LeftControl)) DevConsole.toggle();
		if(Input.GetKeyDown(KeyCode.Tab)) DevConsole.toggle();
	}

	// Finds an Object in the Scene by name and type
	public static Object GlobalFind(string name, System.Type type)
	{
		Object [] objs = Resources.FindObjectsOfTypeAll(type);
	
		foreach (Object obj in objs)
		{
			if (obj.name == name)
			{
				return obj;
			}
		}
	
		return null;
	}

	public string normalize_string(string input, string id, string name, string type) {
		input = input.Replace("{ID}", id);
		input = input.Replace("{NAME}", name);
		input = input.Replace("{TYPE}", type);
		return input;
	}

	public string normalize_from_settings(string setting_id, string id, string name, string type) {
		string input = settings[setting_id];
		input = input.Replace("{ID}", id);
		input = input.Replace("{NAME}", name);
		input = input.Replace("{TYPE}", type);
		return input;
	}

	public void load_scene(string scene) {
		if(Application.CanStreamedLevelBeLoaded("sceneName")) {
			DevConsole.error("Scene " + scene + " does not exist!");
			return;
		}
		//SceneManager.LoadScene(scene, LoadSceneMode.Single);
		asyncSceneLoader = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
		PorpList.refresh_props();
	}

}

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine;

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

	void Awake()
	{
		if(Instance != null) GameObject.Destroy(Instance);
		else Instance = this;

		DontDestroyOnLoad(this);
	}
		
	void Start () {
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

}

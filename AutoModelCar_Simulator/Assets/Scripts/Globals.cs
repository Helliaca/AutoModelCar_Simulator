using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Globals : MonoBehaviour {

	public static Globals Instance { get; private set; }

	public RosSharp.RosBridgeClient.RosConnector Connection;
	public DevConsoleController DevConsole;
	public CarController CurrentCar;

	public CircleRenderer CircleDraw;
	public Transform c_marker;
	public Camera UI_GRAPH_CAMERA;
	public AckController Ack_HUD;
	public UIListController ComponentList;
	public UIListController PorpList;
	public ContextPanelController ContextPanel;
	public SpaceHandleController spaceHandle;
	public LaserScanVisualizerLines lsv_lines;
    public LaserScanVisualizerMesh lsv_mesh;
    public RosSharp.SensorVisualization.LaserScanVisualizerSpheres lsv_spheres;

	public GameObject GPS_prefab;
    public GameObject LaserScanner_prefab;
    public GameObject Camera_prefab;
	public GameObject CollisionDetection_prefab;
	public UIListEntry_Prop Selected_Prop;

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

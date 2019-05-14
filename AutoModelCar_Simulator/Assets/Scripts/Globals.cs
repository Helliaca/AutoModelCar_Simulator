using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Globals : MonoBehaviour {

	public static Globals Instance { get; private set; }

	public DevConsoleController DevConsole;

	public CarController CurrentCar;

	public CircleRenderer CircleDraw;
	public Transform c_marker;
	public Camera UI_GRAPH_CAMERA;

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

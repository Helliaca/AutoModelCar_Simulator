using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDCanvasController : MonoBehaviour
{
	void Awake()
	{
		DontDestroyOnLoad(transform);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleRenderer : MonoBehaviour
{
    public Vector3 center {
        get { return transform.position; }
        set { transform.position = value; }
    }
    public float xradius {
        get { return xr; }
        set { xr = value; CreatePoints(); }
    }
    public float yradius {
        get { return yr; }
        set { yr = value; CreatePoints(); }
    }
    public int resolution {
        get { return res; }
        set { res = value; line.positionCount = value; CreatePoints(); }
    }

    public Transform Center_marker; //Marks center of the circle

    private float xr=1, yr=1;
    private int res = 50;

    private LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = resolution;
    }

    void CreatePoints ()
    {
        float x;
        float z;

        float angle = 0.0f;

        for (int i = 0; i < resolution; i++)
        {
            x = Mathf.Sin (Mathf.Deg2Rad * angle) * xradius;
            z = Mathf.Cos (Mathf.Deg2Rad * angle) * yradius;

            line.SetPosition (i,new Vector3(x,0,z) );

            angle += (360f / resolution);
        }
    }

    void Update() {
        if(Globals.Instance.CurrentCar) {
            Globals.Instance.CurrentCar.UpdateTurningCircleValues(this);
        }
        if(Center_marker) {
            Center_marker.position = this.center;
        }
    }
}

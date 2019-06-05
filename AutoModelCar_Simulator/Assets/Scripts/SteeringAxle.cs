using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;

public class SteeringAxle : MonoBehaviour
{
    public string topic = "/steering";
    public RosConnector Connection;
    public Transform left_wheel, right_wheel;
    public AnimationCurve acceleration_curve;
    public AnimationCurve steering_interp = new AnimationCurve(new Keyframe(0, -25), new Keyframe(180, 25)); //converting coordinates from [0,180] to [-25,25]


    public float T {
        get {return Vector3.Distance(left_wheel.position, right_wheel.position);}
        set {Globals.Instance.DevConsole.error("Setting interwheel distance not implemented!");}
    }

    public float steering_topic {
        set { _steering_topic = value; _steering_real = steering_interp.Evaluate(value); }
        get { return _steering_topic; }
    }
    public float steering_real_deg {
        get { return _steering_real; }
    }

    public float steering_real_rad {
        get { return _steering_real*Mathf.Deg2Rad; }
    }

    private float _steering_real=0, _steering_topic=0;

    private string steering_sub;


    public void set_wheel_rotations(float left_phi, float right_phi) {
        left_wheel.localRotation = Quaternion.Euler(0, left_phi, 0);
        right_wheel.localRotation = Quaternion.Euler(0, right_phi, 0);
    }

    // Start is called before the first frame update
    void Start()
    {
        if(!Connection) Connection = Globals.Instance.Connection;
        steering_sub = Connection.RosSocket.Subscribe<std_msgs.UInt8>(topic, steering_callback);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void steering_callback(std_msgs.UInt8 data) {
        steering_topic = (float)data.data;        
        //Globals.Instance.DevConsole.print("New steering: " + data.data);
    }
}

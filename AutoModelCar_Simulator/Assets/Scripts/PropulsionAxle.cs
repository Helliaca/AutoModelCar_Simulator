using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;

public class PropulsionAxle : MonoBehaviour
{
    public RosConnector Connection;
    public AnimationCurve acceleration_curve;
    public AnimationCurve speed_interp = new AnimationCurve(new Keyframe(0, 0), new Keyframe(150, 0), new Keyframe(600, 3), new Keyframe(2500, 10)); //converting from [0, 150, 600] to [0, 0, 3] (m/s)

    public string topic = "/manual_control/speed";
    public Transform left_wheel, right_wheel;
    
    public float T {
        get {return Vector3.Distance(left_wheel.position, right_wheel.position);}
        set {Globals.Instance.DevConsole.error("Setting interwheel distance not implemented!");}
    }

    public float speed_topic {
        set { _speed_topic = value; _speed_real = speed_interp.Evaluate(value); }
        get { return _speed_topic; }
    }
    public float speed_real {
        get { return _speed_real; }
    }

    private float _speed_real=0, _speed_topic=0;
    private string speed_sub;

    // Start is called before the first frame update
    void Start()
    {
        if(!Connection) Connection = Globals.Instance.Connection;
        speed_sub = Connection.RosSocket.Subscribe<std_msgs.Int16>(topic, speed_callback);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void speed_callback(std_msgs.Int16 data) {
        speed_topic = (float)data.data;
        //Globals.Instance.DevConsole.print("New speed: " + data.data);
    }
}

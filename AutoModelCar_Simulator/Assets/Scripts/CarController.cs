using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;
using nav_msgs = RosSharp.RosBridgeClient.Messages.Navigation;
using geo_msgs = RosSharp.RosBridgeClient.Messages.Geometry;

public class CarController : MonoBehaviour
{

    private RosSocket rosSocket;

    private string speed_sub;
    private string steering_sub;
    private string odom_pub;

    private float speed;
    private float steering;

    // Start is called before the first frame update
    void Start()
    {
        rosSocket = new RosSocket(new RosSharp.RosBridgeClient.Protocols.WebSocketNetProtocol("ws://192.168.178.23:9090"));

        speed_sub = rosSocket.Subscribe<std_msgs.Int16>("/manual_control/speed", speed_callback);
        steering_sub = rosSocket.Subscribe<std_msgs.UInt8>("/steering", steering_callback);
        odom_pub = rosSocket.Advertise<nav_msgs.Odometry>("/localization/odom/5");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = new Vector3(0,0,0);
        offset += transform.forward * speed * Mathf.Cos(Mathf.Deg2Rad * steering);
        offset += transform.right * speed * Mathf.Sin(Mathf.Deg2Rad * steering);
        transform.position += offset * Time.deltaTime * 0.005f;
        transform.Rotate(Vector3.up, speed * steering * Time.deltaTime * 0.01f);

        nav_msgs.Odometry co = new nav_msgs.Odometry();
        co.pose.pose.position = new geo_msgs.Point();
        Vector3 pos = RosSharp.TransformExtensions.Unity2Ros(transform.position);
        co.pose.pose.position.x = pos.x;
        co.pose.pose.position.y = pos.y;
        co.pose.pose.position.z = pos.z;

        co.pose.pose.orientation = new geo_msgs.Quaternion();
        Quaternion rot = RosSharp.TransformExtensions.Unity2Ros(transform.rotation);
        co.pose.pose.orientation.x = rot.x;
        co.pose.pose.orientation.y = rot.y;
        co.pose.pose.orientation.z = rot.z;
        co.pose.pose.orientation.w = rot.w;

        rosSocket.Publish(odom_pub, co);
    }

    private void speed_callback(std_msgs.Int16 data) {
        speed = (float)data.data;
        //Globals.Instance.DevConsole.print("New speed: " + data.data);
    }

    private void steering_callback(std_msgs.UInt8 data) {
        steering = (float)data.data;
        steering = (steering/180.0f) * 50.0f - 25.0f; //converting coordinates from [0,180] to [-25,25]
        
        //Globals.Instance.DevConsole.print("New steering: " + data.data);
    }
}

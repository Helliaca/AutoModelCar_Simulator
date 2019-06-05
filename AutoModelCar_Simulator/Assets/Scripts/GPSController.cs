using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient;
using std_msgs = RosSharp.RosBridgeClient.Messages.Standard;
using nav_msgs = RosSharp.RosBridgeClient.Messages.Navigation;
using geo_msgs = RosSharp.RosBridgeClient.Messages.Geometry;

public class GPSController : MonoBehaviour
{
    public RosConnector Connection;
    public float odom_frequency = 30;
    public string topic = "/localization/odom/5";
    private string odom_pub;
    private float stime;

    void Start() {
        if(!Connection) Connection = Globals.Instance.Connection;
        odom_pub = Connection.RosSocket.Advertise<nav_msgs.Odometry>(topic);
        stime = Time.time;
    }
    void FixedUpdate() {
        if(Time.time < stime + 1.0f/odom_frequency) return;

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

        Connection.RosSocket.Publish(odom_pub, co);        
        stime = Time.time;
    }
}
